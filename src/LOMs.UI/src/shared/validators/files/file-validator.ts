// validators/file.validator.ts
import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';

export interface FileValidationConfig {
  maxSize?: number;
  minSize?: number; // in bytes
  allowedExtensions?: string[]; // e.g., ['pdf', 'jpg', 'png']
  allowedMimeTypes?: string[]; // e.g., ['image/jpeg', 'application/pdf']
  maxFiles?: number; // for multiple files
  minFiles?: number; // for multiple files
  maxWidth?: number; // for images only
  maxHeight?: number; // for images only
  minWidth?: number; // for images only
  minHeight?: number; // for images only
  exactDimensions?: { width: number; height: number }; // for images
  aspectRatio?: number; // width/height ratio for images
  preventDuplicates?: boolean; // check for duplicate files
}

export interface FileValidationError {
  file?: File;
  fileName?: string;
  fileSize?: number;
  actualExtension?: string;
  actualMimeType?: string;
  actualDimensions?: { width: number; height: number };
  message?: string;
  [key: string]: any;
}
const documentExtensions = [
  '.pdf',
  '.doc',
  '.docx',
  '.xls',
  '.xlsx',
  '.ppt',
  '.pptx',
  '.txt',
  '.rtf',
  '.odt',
  '.csv',
];

const imageExtensions = [
  '.jpg',
  '.jpeg',
  '.png',
  '.gif',
  '.bmp',
  '.webp',
  '.svg',
  '.tiff',
  '.tif',
];

export class FileValidator {
  /**
   * Comprehensive file validator factory
   */
  static validate(
    config: FileValidationConfig = {
      maxSize: 10 * 1024 * 1024,
      allowedExtensions: imageExtensions.concat(documentExtensions),
    }
  ): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;

      // Handle null/undefined values
      if (!value) return null;

      let file: File;

      // Handle both single file and array of files
      if (Array.isArray(value)) {
        if (value.length === 0) return null;
        file = value[0]; // Take the first file
      } else if (value instanceof File) {
        file = value;
      } else {
        console.log('not a file');
        return null;
      }

      const errors: ValidationErrors = {};

      const fileErrors = this.validateSingleFile(file, config);
      Object.assign(errors, fileErrors);

      return Object.keys(errors).length > 0 ? errors : null;
    };
  }

  /**
   * Quick validators for common scenarios
   */

  // Image files only with size limit
  static images(maxSizeMB: number = 5): ValidatorFn {
    return this.validate({
      maxSize: maxSizeMB * 1024 * 1024,
      allowedMimeTypes: [
        'image/jpeg',
        'image/jpg',
        'image/png',
        'image/gif',
        'image/webp',
        'image/svg+xml',
      ],
      allowedExtensions: ['jpg', 'jpeg', 'png', 'gif', 'webp', 'svg'],
    });
  }

  // Document files only
  static documents(maxSizeMB: number = 10): ValidatorFn {
    return this.validate({
      maxSize: maxSizeMB * 1024 * 1024,
      allowedMimeTypes: [
        'application/pdf',
        'application/msword',
        'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
        'application/vnd.ms-excel',
        'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
        'text/plain',
        'text/csv',
      ],
    });
  }

  // Profile pictures with specific dimensions
  static profilePicture(): ValidatorFn {
    return this.validate({
      maxSize: 2 * 1024 * 1024, // 2MB
      allowedMimeTypes: ['image/jpeg', 'image/png'],
      allowedExtensions: ['jpg', 'jpeg', 'png'],
      minWidth: 150,
      minHeight: 150,
      maxWidth: 2000,
      maxHeight: 2000,
      aspectRatio: 1, // Square images only
    });
  }

  // Multiple file upload with limits
  static multipleFiles(
    config: Partial<FileValidationConfig> = {}
  ): ValidatorFn {
    return this.validate({
      maxFiles: 5,
      minFiles: 1,
      maxSize: 10 * 1024 * 1024, // 10MB per file
      preventDuplicates: true,
      ...config,
    });
  }

  /**
   * Private validation methods
   */

  private static validateFileCount(
    files: File[],
    config: FileValidationConfig
  ): ValidationErrors | null {
    const errors: ValidationErrors = {};

    if (config.maxFiles && files.length > config.maxFiles) {
      errors['maxFiles'] = {
        max: config.maxFiles,
        actual: files.length,
        message: `Maximum ${config.maxFiles} files allowed`,
      };
    }

    if (config.minFiles && files.length < config.minFiles) {
      errors['minFiles'] = {
        min: config.minFiles,
        actual: files.length,
        message: `Minimum ${config.minFiles} files required`,
      };
    }

    return Object.keys(errors).length > 0 ? errors : null;
  }

  private static validateSingleFile(
    file: File,
    config: FileValidationConfig
  ): ValidationErrors | null {
    const errors: ValidationErrors = {};
    const filePrefix = `fileSize`;

    // File size validation
    if (config.maxSize && file.size > config.maxSize) {
      console.log('the file size: ', file.size);
      console.log('the config size: ', config.maxSize);

      errors[`maxSize`] = {
        file,
        fileName: file.name,
        fileSize: file.size,
        maxSize: config.maxSize,
        message: `${this.formatFileSize(
          config.maxSize
        )} يجب أن لا يتعدى الملف الحجم`,
      };
    }

    if (config.minSize && file.size < config.minSize) {
      errors[`minSize`] = {
        file,
        fileName: file.name,
        fileSize: file.size,
        minSize: config.minSize,
        message: `${this.formatFileSize(
          config.minSize
        )} يجب ان يكون الحجم اكبر من`,
      };
    }

    // Extension validation
    if (config.allowedExtensions && config.allowedExtensions.length > 0) {
      const extension = this.getFileExtension(file.name);
      if (!config.allowedExtensions.includes(extension)) {
        errors[`extension`] = {
          file,
          fileName: file.name,
          actualExtension: extension,
          allowedExtensions: config.allowedExtensions,
          message: `ادخل ملف صالح, الملفات الصالحة: ${config.allowedExtensions.join(
            ', '
          )}`,
        };
      }
    }
    return Object.keys(errors).length > 0 ? errors : null;
  }

  private static validateDuplicates(files: File[]): ValidationErrors | null {
    const duplicates: string[] = [];
    const seen = new Set<string>();

    files.forEach((file) => {
      const fileKey = `${file.name}_${file.size}_${file.lastModified}`;
      if (seen.has(fileKey)) {
        duplicates.push(file.name);
      } else {
        seen.add(fileKey);
      }
    });

    if (duplicates.length > 0) {
      return {
        duplicateFiles: {
          duplicates,
          message: `Duplicate files detected: ${duplicates.join(', ')}`,
        },
      };
    }

    return null;
  }

  /**
   * Async image dimension validation
   */
  static validateImageDimensions(
    file: File,
    config: FileValidationConfig
  ): Promise<ValidationErrors | null> {
    return new Promise((resolve) => {
      if (!this.isImage(file)) {
        resolve(null);
        return;
      }

      const img = new Image();
      const canvas = document.createElement('canvas');
      const ctx = canvas.getContext('2d');

      img.onload = () => {
        const errors: ValidationErrors = {};
        const { width, height } = img;

        // Dimension validations
        if (config.maxWidth && width > config.maxWidth) {
          errors['maxWidth'] = {
            file,
            fileName: file.name,
            actualDimensions: { width, height },
            maxWidth: config.maxWidth,
            message: `Image width ${width}px exceeds maximum ${config.maxWidth}px`,
          };
        }

        if (config.minWidth && width < config.minWidth) {
          errors['minWidth'] = {
            file,
            fileName: file.name,
            actualDimensions: { width, height },
            minWidth: config.minWidth,
            message: `Image width ${width}px is less than minimum ${config.minWidth}px`,
          };
        }

        if (config.maxHeight && height > config.maxHeight) {
          errors['maxHeight'] = {
            file,
            fileName: file.name,
            actualDimensions: { width, height },
            maxHeight: config.maxHeight,
            message: `Image height ${height}px exceeds maximum ${config.maxHeight}px`,
          };
        }

        if (config.minHeight && height < config.minHeight) {
          errors['minHeight'] = {
            file,
            fileName: file.name,
            actualDimensions: { width, height },
            minHeight: config.minHeight,
            message: `Image height ${height}px is less than minimum ${config.minHeight}px`,
          };
        }

        // Exact dimensions
        if (config.exactDimensions) {
          if (
            width !== config.exactDimensions.width ||
            height !== config.exactDimensions.height
          ) {
            errors['exactDimensions'] = {
              file,
              fileName: file.name,
              actualDimensions: { width, height },
              requiredDimensions: config.exactDimensions,
              message: `Image must be exactly ${config.exactDimensions.width}x${config.exactDimensions.height}px`,
            };
          }
        }

        // Aspect ratio
        if (config.aspectRatio) {
          const actualRatio = width / height;
          const tolerance = 0.1; // 10% tolerance
          if (Math.abs(actualRatio - config.aspectRatio) > tolerance) {
            errors['aspectRatio'] = {
              file,
              fileName: file.name,
              actualDimensions: { width, height },
              actualRatio: actualRatio.toFixed(2),
              requiredRatio: config.aspectRatio,
              message: `Image aspect ratio ${actualRatio.toFixed(
                2
              )} doesn't match required ${config.aspectRatio}`,
            };
          }
        }

        resolve(Object.keys(errors).length > 0 ? errors : null);
      };

      img.onerror = () => {
        resolve({
          invalidImage: {
            file,
            fileName: file.name,
            message: `"${file.name}" is not a valid image file`,
          },
        });
      };

      img.src = URL.createObjectURL(file);
    });
  }

  /**
   * Utility methods
   */

  private static getFileExtension(filename: string): string {
    return filename?.slice(filename?.lastIndexOf('.')).toLowerCase();
  }

  private static isImage(file: File): boolean {
    return file.type.startsWith('image/');
  }

  private static formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 Bytes';

    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));

    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  }

  /**
   * Get user-friendly error messages
   */
  static getErrorMessage(errors: ValidationErrors): string[] {
    const messages: string[] = [];

    Object.keys(errors).forEach((key) => {
      const error = errors[key];
      if (error.message) {
        messages.push(error.message);
      } else {
        // Fallback messages
        if (key.includes('maxSize')) {
          messages.push(
            `File too large. Maximum size: ${this.formatFileSize(
              error.maxSize
            )}`
          );
        } else if (key.includes('minSize')) {
          messages.push(
            `File too small. Minimum size: ${this.formatFileSize(
              error.minSize
            )}`
          );
        } else if (key.includes('extension')) {
          messages.push(
            `Invalid file type. Allowed: ${error.allowedExtensions?.join(', ')}`
          );
        } else if (key.includes('mimeType')) {
          messages.push(`Invalid file format`);
        } else if (key === 'maxFiles') {
          messages.push(`Too many files. Maximum: ${error.max}`);
        } else if (key === 'minFiles') {
          messages.push(`Not enough files. Minimum: ${error.min}`);
        } else {
          messages.push('Invalid file');
        }
      }
    });

    return messages;
  }
}
