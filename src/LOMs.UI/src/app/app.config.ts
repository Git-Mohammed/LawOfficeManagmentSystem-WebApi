import {
  ApplicationConfig,
  importProvidersFrom,
  provideBrowserGlobalErrorListeners,
  provideZoneChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { loaderInterceptor } from '../core/interceptors/loader/loader-interceptor-interceptor';
import { httpErrorHandelrInterceptor } from '../core/interceptors/http-error-handler/http-error-handelr-interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideHttpClient(withFetch(), withInterceptors([loaderInterceptor, httpErrorHandelrInterceptor])),
    provideRouter(routes),
    provideAnimations(),
  ],
};
