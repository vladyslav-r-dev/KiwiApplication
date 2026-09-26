import {
  AfterViewInit,
  Component,
  EventEmitter,
  inject,
  Output
} from '@angular/core';

import {
  FormControl,
  FormGroup,
  ReactiveFormsModule
} from '@angular/forms';

import { Router } from '@angular/router';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})

export class Login implements AfterViewInit {

  @Output() close = new EventEmitter<void>();

  loginForm = new FormGroup({
    email: new FormControl('', { nonNullable: true }),
    password: new FormControl('', { nonNullable: true }),
  });

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngAfterViewInit() {
    this.renderGoogleButton();
  }

  private renderGoogleButton() {
    const google = (window as any).google;

    if (!google) {
      setTimeout(() => {
        this.renderGoogleButton();
      }, 200);

      return;
    }
    
    const router = inject(Router);

    google.accounts.id.initialize({
      client_id: '269421523996-9mvqdofsd00fm7k8vb5sg4r841capblj.apps.googleusercontent.com',

      callback: (response: any) => {
        this.handleGoogleLogin(response.credential);
      }
    });

    const button = document.getElementById('googleButton');

    if (!button) {
      return;
    }

    google.accounts.id.renderButton(
      button,
      {
        theme: 'outline',
        size: 'large',
        text: 'signin_with'
      }
    );
  }

  onLogin() {
    const loginData = this.loginForm.getRawValue();

    this.authService.login(loginData).subscribe({
      next: (result) => {

        this.authService.accessToken.set(
          result.accessToken
        );

        this.closeLogin();

        this.router.navigate(['/flights']);
      },

      error: (error) => {
        console.error('Login failed:', error);
      }
    });
  }

  handleGoogleLogin(idToken: string) {
    this.authService.googleLogin(idToken).subscribe({
      next: (result) => {

        this.authService.accessToken.set(
          result.accessToken
        );

        this.closeLogin();

        this.router.navigate(['/flights']);
      },

      error: (error) => {
        console.error('Google login failed:', error);
      }
    });
  }

  logout() {
    this.authService.logout().subscribe({
      next: () => {

        this.authService.accessToken.set(null);

        this.router.navigate(['/']);
      },

      error: (error) => {
        console.error('Logout failed:', error);
      }
    });
  }

  closeLogin() {
    this.close.emit();
  }
}

export interface LoginRequest {
  email: string;
  password: string;
}