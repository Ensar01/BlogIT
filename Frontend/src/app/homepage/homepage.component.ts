import { Component } from '@angular/core';
import {AuthService} from '../services/authService';
import {HTTP_INTERCEPTORS} from '@angular/common/http';
import {Interceptor} from '../core/interceptor';

@Component({
  selector: 'app-homepage',
  standalone: true,
  imports: [],
  templateUrl: './homepage.component.html',
  styleUrl: './homepage.component.css',
})
export class HomepageComponent {
  constructor(private authService: AuthService) {}



  logoutUser() {
    this.authService.logout().subscribe(
      response => {
        console.log(response);
      },
      error => console.log(error)
    )
  };
}
