import { Component } from '@angular/core';
import {NgOptimizedImage} from '@angular/common';
import {Router, RouterLink} from "@angular/router";

@Component({
  selector: 'app-landing-page',
  standalone:true,
    imports: [
        NgOptimizedImage,
        RouterLink
    ],
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.css'
})
export class LandingPageComponent {
  constructor(private router: Router) {
  }

  openRegistrationPage() {
    this.router.navigate(['/register']);
  }
}
