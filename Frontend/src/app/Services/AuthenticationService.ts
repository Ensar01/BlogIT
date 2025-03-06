import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from '../../enviroment';
import {API_ENDPOINTS} from '../api-endpoints';

@Injectable({
  providedIn: 'root'
  })
export  class AuthenticationService {
  constructor(private http: HttpClient) {
  }

  register(userData: any): Observable<any> {
    const url = API_ENDPOINTS.authentication.register;
    return this.http.post(url, userData);
  }
}
