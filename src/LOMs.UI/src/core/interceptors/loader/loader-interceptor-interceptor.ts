import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { finalize } from 'rxjs';
import { LoaderService } from '../../services/loader/loader-service';

export const loaderInterceptor: HttpInterceptorFn = (req, next) => {
  const loaderService = inject(LoaderService);
  loaderService.loading();

  return next(req).pipe(
    finalize(() => {
      debugger
      loaderService.hideLoader();
    })
  );
};
