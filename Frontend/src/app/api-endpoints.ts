import {environment} from '../enviroment';

export const API_ENDPOINTS =
  {
    authentication: {
      register:`${environment.apiBaseUrl}/Authentication/Register`,
      checkAvailability: `${environment.apiBaseUrl}/Authentication/CheckAvailability`,
    }
  }
