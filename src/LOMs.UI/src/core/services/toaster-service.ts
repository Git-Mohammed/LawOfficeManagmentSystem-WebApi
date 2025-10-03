import { inject, Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';
@Injectable({
  providedIn: 'root',
})
export class ToasterService {
  private snackBar = inject(MatSnackBar);

  private template(mes: string, lbl?: string, config?: MatSnackBarConfig) {
    this.snackBar.open(mes, lbl, config);
  }

  success(mes: string, lbl?: string) {
    this.template(mes, lbl,  {
      duration: 9000,
      panelClass: ['snackbar-success'],
      direction: 'rtl',
      horizontalPosition: 'right',
    });
  }

  error(mes: string, lbl?: string) {
    this.template(mes, lbl, {
      duration: 9000,
      panelClass: ['snackbar-error'],
      direction: 'rtl',
      horizontalPosition: 'right',
    });
  }
}
