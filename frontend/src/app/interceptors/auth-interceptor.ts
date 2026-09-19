import { inject } from '@angular/core';
import {
  HttpErrorResponse,
  HttpInterceptorFn
} from '@angular/common/http';

import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../services/auth';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.accessToken();

  const authReq = token
    ? req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      })
    : req;

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {

      if (
        error.status !== 401 ||
        req.url.includes('/auth/refresh')
      ) {
        return throwError(() => error);
      }

      return authService.refresh().pipe(
        switchMap(result => {
          authService.accessToken.set(result.accessToken);

          const retryReq = req.clone({
            setHeaders: {
              Authorization: `Bearer ${result.accessToken}`
            }
          });

          return next(retryReq);
        })
      );
    })
  );
};