import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { toObservable } from '@angular/core/rxjs-interop';
import { filter, map, take } from 'rxjs';

import { AuthService } from '../services/auth';

export const authGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return toObservable(authService.sessionRestored).pipe(
    filter(restored => restored),
    take(1),
    map(() => {
      if (authService.accessToken()) {
        return true;
      }

      return router.createUrlTree(['/login']);
    })
  );
};