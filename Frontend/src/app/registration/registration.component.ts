import { Component, OnInit, OnDestroy} from '@angular/core';
import {NgOptimizedImage} from '@angular/common';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {RouterLink} from '@angular/router';


@Component({
  selector: 'app-registration',
  imports: [
    NgOptimizedImage,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './registration.component.html',
  standalone: true,
  styleUrl: './registration.component.css'
})

export class RegistrationComponent {
  registerForm = new FormGroup({
    firstName: new FormControl('', Validators.required),
    lastName: new FormControl('', Validators.required),
    username: new FormControl('', Validators.required),
    email: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required)
  })



}
