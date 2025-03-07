import { Routes } from '@angular/router';
import {RegistrationComponent} from './registration/registration.component';
import {LandingPageComponent} from './landing-page/landing-page.component';

export const routes: Routes = [
  {path: '', redirectTo: 'landing-page', pathMatch: 'full'},
  {path: 'landing-page', component: LandingPageComponent},
  {path: 'register',component: RegistrationComponent}

];
