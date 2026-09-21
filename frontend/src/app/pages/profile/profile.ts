import { Component, signal } from '@angular/core';
import { AuthService } from '../../services/auth';
import { inject } from '@angular/core';
import { User } from './User';
import { ReactiveFormsModule, FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-profile',
  imports: [ReactiveFormsModule],
  templateUrl: './profile.html',
  styleUrl: './profile.scss',
})
export class Profile {
  authService = inject(AuthService);
  
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
}