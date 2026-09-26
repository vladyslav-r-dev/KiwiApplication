import { Component, EventEmitter, Output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register {

  @Output() registerClose = new EventEmitter<void>();

  constructor(private authService: AuthService) {}
  registerForm = new FormGroup({
  name: new FormControl('', { nonNullable: true }),
  lastName: new FormControl('', { nonNullable: true }),
  email: new FormControl('', { nonNullable: true }),
  password: new FormControl('', { nonNullable: true }),
  passport: new FormControl('', { nonNullable: true }),
});


  onSubmit() {
    if (this.registerForm.valid) {
      const registrationData = this.registerForm.getRawValue();
      this.authService.register(registrationData).subscribe({
        next: (response) => {
          this.registerClose.emit();
        },
        error: (error) => {
          console.error('Registration failed:', error);
        }
      });
    } else {
      console.log('Form is invalid');
    }
  }
}