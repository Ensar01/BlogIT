import { Component, OnInit, OnDestroy} from '@angular/core';
import {NgIf, NgOptimizedImage} from '@angular/common';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
  AsyncValidator,
  AsyncValidatorFn, AbstractControl
} from '@angular/forms';
import {RouterLink} from '@angular/router';
import {AuthService} from '../services/authService';
import {debounceTime, Observable} from 'rxjs';



@Component({
  selector: 'app-registration',
  imports: [
    NgOptimizedImage,
    ReactiveFormsModule,
    RouterLink,
    NgIf,
  ],
  templateUrl: './registration.component.html',
  standalone: true,
  styleUrl: './registration.component.css'
})

export class RegistrationComponent {

  public registerForm = new FormGroup({
    firstName: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(20)]),
    lastName: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(20)]),
    username: new FormControl('', {updateOn:'blur', validators: [Validators.required, Validators.minLength(6),Validators.maxLength(20)],
      asyncValidators: [this.asyncFieldValidator("username")]}),
    email: new FormControl('', {updateOn: 'blur',validators: [Validators.required, Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$')],
     asyncValidators: [this.asyncFieldValidator("email")]}),
    password: new FormControl('', [Validators.required, Validators.minLength(8),
      Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$/)])
  })
  constructor(private authService: AuthService)  {
  }

  asyncFieldValidator(type: 'username' | 'email'):AsyncValidatorFn {
    return (control: AbstractControl): Observable<any> => {
      const value = control.value;
      debounceTime(1000);
      return this.authService.genericAsyncValidator(type, value);
    };
  }

  registerUser() {
    this.registerForm.markAllAsTouched();
    if (this.registerForm.invalid) {
      return;
    }

    this.authService.register(this.registerForm.value).subscribe(
      response => console.log(response),
      error => console.log(error)
    )
  }
}
