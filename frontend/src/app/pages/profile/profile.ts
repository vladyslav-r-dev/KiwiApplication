import { Component, signal } from '@angular/core';
import { AuthService } from '../../services/auth';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { User } from './User';
import { ReactiveFormsModule, FormGroup, FormControl } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-profile',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './profile.html',
  styleUrl: './profile.scss',
})
export class Profile {
  authService = inject(AuthService);
  router = inject(Router);
  
  updateProfileForm = new FormGroup({
  name: new FormControl('', { nonNullable: true }),
  lastName: new FormControl('', { nonNullable: true }),
  });

  loading = signal(false);
  error = signal<string | null>(null);
  currentUser = signal<User | null>(null);

  ngOnInit() {
    this.loading.set(true);
    this.authService.getMe().subscribe({
      next: (user) => {
  this.currentUser.set(user);
  this.error.set(null);
  this.loading.set(false);
  this.updateProfileForm.patchValue({
  name: user.name,
  lastName: user.lastName
});
  },
error: (err) => {
  this.error.set(err.message);
  this.loading.set(false);
  }
    });
  }

  saveProfile() {
    if (this.updateProfileForm.valid) {
      const data = this.updateProfileForm.getRawValue();
      
      this.authService.updateMe(data).subscribe({
        next: (user) => {
          this.currentUser.set(user);
          this.updateProfileForm.patchValue({
          name: user.name,
          lastName: user.lastName
          });
          this.error.set(null);
        },
        error: (err) => {
          this.error.set(err.message);
        }
      });
    }
  }

  logout() {
  this.authService.logout().subscribe({
    next: () => {
      this.authService.accessToken.set(null);
      this.router.navigate(['/home']);
    },
    error: (err) => {
      console.error('Logout failed:', err);
    }
  });
}
}