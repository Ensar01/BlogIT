import { Component, OnInit, OnDestroy} from '@angular/core';
import {NgOptimizedImage} from '@angular/common';


@Component({
  selector: 'app-registration',
  imports: [
    NgOptimizedImage
  ],
  templateUrl: './registration.component.html',
  standalone: true,
  styleUrl: './registration.component.css'
})

export class RegistrationComponent {
  constructor() { }



}
