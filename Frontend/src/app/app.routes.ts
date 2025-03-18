import { Routes } from '@angular/router';
import {RegistrationComponent} from './registration/registration.component';
import {LandingPageComponent} from './landing-page/landing-page.component';
import {LoginComponent} from './login/login.component';
import {HomepageComponent} from './homepage/homepage.component';

export const routes: Routes = [
  {path: '', redirectTo: 'landing-page', pathMatch: 'full'},
  {path: 'landing-page', component: LandingPageComponent},
  {path: 'register',component: RegistrationComponent},
  {path: 'login',component: LoginComponent},
  {path: 'homepage', component: HomepageComponent},
];
