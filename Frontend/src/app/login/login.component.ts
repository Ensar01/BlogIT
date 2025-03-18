import { Component } from '@angular/core';
import {NgIf, NgOptimizedImage} from '@angular/common';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {Router, RouterLink} from '@angular/router';
import {AuthService} from '../services/authService';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    NgOptimizedImage,
    FormsModule,
    NgIf,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
 public loginForm = new FormGroup({
   username: new FormControl('', Validators.required),
   password: new FormControl('', Validators.required),
 })
  constructor(private  authService: AuthService, private router: Router) {}

  loginUser()
  {
    this.loginForm.markAsTouched();
    if(this.loginForm.invalid) {
      return;
    }
    this.authService.login(this.loginForm.value).subscribe(
      response => {
        console.log(response);
        this.router.navigate(['/homepage']);
      },
      error => console.log(error)
    )
  }

}
