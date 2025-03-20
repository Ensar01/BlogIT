import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandlerFn,
  HttpInterceptorFn,
  HttpRequest
} from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, Observable, throwError, switchMap, of } from 'rxjs';
import { AuthService } from '../services/authService';

// Globalna varijabla za praćenje errora
let errCtr = 0;

export const Interceptor: HttpInterceptorFn = (req: HttpRequest<any>, next: HttpHandlerFn) => {
  const router = inject(Router);
  const authService = inject(AuthService);

  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      if (err && err.status === 401 && errCtr != 1) {
        errCtr++;

        return authService.refreshToken().pipe(
          switchMap((x: any) => {
            console.log("Token refreshed successfully");
            // Ponovi originalni zahtjev
            return next(req);
          }),
          catchError((refreshErr: any) => {
            return authService.logout().pipe(
              switchMap(() => {
                router.navigateByUrl('/login');
                errCtr = 0;
                return throwError(() => err.message);
              })
            );
          })
        );
      } else {
        errCtr = 0;
        return throwError(() => err);
      }
    })
  );
};
