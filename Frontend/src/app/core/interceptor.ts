import {
  HttpClient,
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest
} from '@angular/common/http';
import {Injectable, Injector} from '@angular/core';
import {Router} from '@angular/router';
import {catchError, Observable} from 'rxjs';
import {AuthService} from '../services/authService';

@Injectable()
export class Interceptor implements HttpInterceptor {

  constructor(private injector: Injector, private router: Router)  {
  }
  errCtr = 0
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(catchError(x=> this.handleAuthError(x)));
  }

  private handleAuthError(err: HttpErrorResponse):Observable<any> {
    if(err && err.status === 401 && this.errCtr!=1) {
      this.errCtr++
      let authService = this.injector.get(AuthService);
    }
  }
}
