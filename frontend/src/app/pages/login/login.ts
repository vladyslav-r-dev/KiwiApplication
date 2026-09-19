import { Component} from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {

  logout(){
    this.authService.logout().subscribe({
      next: (response) => {
        console.log('Logout successful:', response);
        this.authService.accessToken.set(null);
        this.router.navigate(['/login']);
      },
      error: (error) => {
        console.error('Logout failed:', error);
      }
    });
  }

onLogin() {
  const loginData = this.loginForm.getRawValue();
  this.authService.login(loginData).subscribe({
    next: (result) => {
      this.authService.accessToken.set(result.accessToken);
      this.router.navigate(['/flights']);
    },
    error: (error) => {
      console.error('Login failed:', error);
    }
  });
}
    constructor(private authService: AuthService, private router: Router) {}
  loginForm = new FormGroup({
  email: new FormControl('', { nonNullable: true }),
  password: new FormControl('', { nonNullable: true }),
});

}

export interface LoginRequest {
  email: string;
  password: string;
}
