import { inject } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivateFn,
  RedirectCommand,
  Router,
} from '@angular/router';
import { SessionManagement } from '../../services/session/session-management';
import { ToasterService } from '../../services/toaster-service';
import { AuthManagement } from '../../services/auth/auth-management';
import { catchError, map, of } from 'rxjs';
import { enRole } from '../../../shared/enums/roles';

export const authGuard: CanActivateFn = (router) => {
  const toaster = inject(ToasterService);
  const route = inject(Router);
  const session = inject(SessionManagement);
  const loginPath = route.parseUrl('/login');

  if (session.isAuthenticated()) {
    return checkAuthorization(router, session, toaster, route);
  }

  // No valid tokens
  toaster.error('يرجى تسجيل الدخول');
  return new RedirectCommand(loginPath);
};

function checkAuthorization(
  route: ActivatedRouteSnapshot,
  session: SessionManagement,
  toaster: ToasterService,
  router: Router
): boolean | RedirectCommand {
  const requiredRoles = route.data['roles'] as enRole[];
  if (requiredRoles && requiredRoles.length > 0) {
    const user = session.getSession();
    const hasRole = requiredRoles.some((role) => user?.role == role);

    if (!hasRole) {
      toaster.error('ليس لديك صلاحية للوصول إلى هذه الصفحة');
      return new RedirectCommand(router.parseUrl('/unauthorized'));
    }
  }

  return true;
}
