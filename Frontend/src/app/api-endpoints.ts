import {environment} from '../enviroment';

export const API_ENDPOINTS =
  {
    authentication: {
      register:`${environment.apiBaseUrl}/Auth/Register`,
      login: `${environment.apiBaseUrl}/Auth/Login`,
    },
    user: {
      exists: `${environment.apiBaseUrl}/User/Exists`,
    }
  }
