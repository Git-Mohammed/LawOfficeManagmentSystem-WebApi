import { CanActivateFn } from '@angular/router';

export const authorizeGuardGuard: CanActivateFn = (route, state) => {
  return true;
};
