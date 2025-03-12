import {environment} from '../enviroment';

export const API_ENDPOINTS =
  {
    authentication: {
      register:`${environment.apiBaseUrl}/Auth/Register`,
      exists: `${environment.apiBaseUrl}/Auth/Exists`,
      login: `${environment.apiBaseUrl}/Auth/Login`,
    }
  }
