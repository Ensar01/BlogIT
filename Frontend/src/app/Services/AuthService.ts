import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {catchError, debounce, debounceTime, Observable, of, switchMap} from 'rxjs';
import {environment} from '../../enviroment';
import {API_ENDPOINTS} from '../api-endpoints';

@Injectable({
  providedIn: 'root'
  })
export  class AuthService {
  constructor(private http: HttpClient) {
  }

  register(userData: any): Observable<any> {
    const url = API_ENDPOINTS.authentication.register;
    return this.http.post(url, userData);
  }
  login(userData: any): Observable<any> {
    const url = API_ENDPOINTS.authentication.login;
    return this.http.post(url, userData,{ withCredentials: true });
  }
  refreshToken(): Observable<any> {
    const url = API_ENDPOINTS.authentication.refreshToken;
    return this.http.post(url,{},{withCredentials: true});
  }
  logout(): Observable<any>{
    const url = API_ENDPOINTS.authentication.logout;
    return this.http.post(url,{},{withCredentials: true});
  }
  exists(email?: string, username?:string): Observable<boolean> {

    const url = API_ENDPOINTS.user.exists;
    const params: any = {};
    if(email) params.email = email;
    if(username) params.username = username;

    return this.http.get<boolean>(url, { params });
  }

  genericAsyncValidator(type: 'username' | 'email', value: string): Observable<any> {
    return this.exists(type === 'username' ? undefined : value, type==='email'? undefined : value)
      .pipe(
        switchMap(exists => {
        return exists ? of ({ [`${type}Taken`]: true }) : of(null);
      }),
        catchError(() => of(null))
      );
  }
}
