import {environment} from '../enviroment';

export const API_ENDPOINTS =
  {
    authentication: {
      register:`${environment.apiBaseUrl}/Auth/Register`,
      login: `${environment.apiBaseUrl}/Auth/Login`,
      refreshToken: `${environment.apiBaseUrl}/Auth/RefreshToken`,
      logout: `${environment.apiBaseUrl}/Auth/Logout`
    },
    user: {
      exists: `${environment.apiBaseUrl}/User/Exists`,
    }
  }
