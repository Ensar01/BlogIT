import { Component, OnInit, OnDestroy} from '@angular/core';
import {NgOptimizedImage} from '@angular/common';
import {FormControl, FormGroup, ReactiveFormsModule} from '@angular/forms';


@Component({
  selector: 'app-registration',
  imports: [
    NgOptimizedImage,
    ReactiveFormsModule
  ],
  templateUrl: './registration.component.html',
  standalone: true,
  styleUrl: './registration.component.css'
})

export class RegistrationComponent {
  registrationForm = new FormGroup({
    username: new FormControl(''),
    email: new FormControl(''),
    password: new FormControl('')
  });



}
