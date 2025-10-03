import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToasterService } from '../../services/toaster-service';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthManagement } from '../../services/auth/auth-management';
import { enErrorCodes, IBusinessError } from '../../../shared/enums/errors';

export const httpErrorHandelrInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const authMangment = inject(AuthManagement);
  const toastService = inject(ToasterService);
  let counter = 0;
  const NewReq = req.clone({
    withCredentials: true,
  });

  return next(NewReq).pipe(
    catchError((error: HttpErrorResponse) => {
      // handle unauthorized
      console.log('error accrued', error);
      if (error.status === 401 && counter == 0) {
        counter++;
          
          return authMangment.refreshToken().pipe(
            switchMap(() => {
              counter = 0;
              const newReq = req.clone({ withCredentials: true });
              return next(newReq);
            }),
            catchError((refreshErr) => {
              counter++;
              // لو فشل الريفرش نطرد المستخدم
              toastService.error(
                'انتهت صلاحية الجلسة، يرجى تسجيل الدخول مرة أخرى'
              );
              authMangment.logout();
              router.navigate(['/login']);
              return throwError(() => refreshErr);
            })
          );
        
      } else {
        
        // router.navigate(['/login']);
        counter = 0
      }

      // handle other errors
      switch (error.status) {
        case 0:
          toastService.error('لا يمكن الاتصال بالخادم');
          break;
        case 400:
          toastService.error('طلب غير صالح');
          break;
        case 403:
          toastService.error('ليس لديك صلاحية الوصول');
          break;
        case 404:
          // toastService.error('العنصر غير موجود');
          break;
        case 500:
          toastService.error('حدث خطأ في الخادم، يرجى المحاولة لاحقًا');
          break;
        default:
          toastService.error('حدث خطأ غير متوقع');
      }

      const myerror : IBusinessError = {
        code: error?.error?.detail as enErrorCodes,
        status: error.status,
        title: error?.error?.title,
        type: error?.error?.type
      }

      return throwError(() => myerror);
    })
  );
};
