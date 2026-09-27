import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { toObservable } from '@angular/core/rxjs-interop';
import { catchError, filter, map, of, switchMap, take } from 'rxjs';
import { AuthService } from '../services/auth.service';

export const adminGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return toObservable(authService.sessionRestored).pipe(
    filter((restored) => restored),
    take(1),

    switchMap(() => authService.getMe()),

    map((user) => {
      if (user.role === 'Admin') {
        return true;
      }

      return router.createUrlTree(['/']);
    }),

    catchError(() => {
      return of(router.createUrlTree(['/']));
    }),
  );
};
