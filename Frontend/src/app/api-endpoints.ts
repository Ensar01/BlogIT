import {environment} from '../enviroment';

export const API_ENDPOINTS =
  {
    authentication: {
      register:`${environment.apiBaseUrl}/Auth/Register`,
      checkAvailability: `${environment.apiBaseUrl}/Auth/CheckAvailability`,
    }
  }
