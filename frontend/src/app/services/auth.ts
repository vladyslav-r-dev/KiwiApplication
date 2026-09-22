import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LoginResult } from '../pages/login/login-result';
import { LoginRequest } from '../pages/login/login';
import { signal } from '@angular/core';
import { RefreshResult } from '../pages/login/refresh-result';
import { User } from '../pages/profile/User';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

    accessToken = signal<string | null>(null);
    sessionRestored = signal(false);

  constructor(private httpClient: HttpClient) {
  }

  register(data: RegisterRequest) {
    return this.httpClient.post(
      'http://localhost:5086/register',
      data
    );
  }

  login(data: LoginRequest) {
    return this.httpClient.post<LoginResult>(
      'http://localhost:5086/auth/login',
      data,
      { withCredentials: true }
    );
  }

  getMe(){
    return this.httpClient.get<User>(
      'http://localhost:5086/auth/me'
    );
  }

  logout() {
    return this.httpClient.post(
      'http://localhost:5086/auth/logout',
      {},
      { withCredentials: true }
    );
  }

  refresh(){
    return this.httpClient.post<RefreshResult>(
      'http://localhost:5086/auth/refresh',
      {},
      { withCredentials: true }
    );
  }

  updateMe(data: { name: string; lastName: string }) {
    return this.httpClient.put<User>(
      'http://localhost:5086/auth/me',
      data,
      { withCredentials: true }
    );
  }

  getAllUsers() {
  return this.httpClient.get<AdminUser[]>(
    'http://localhost:5086/admin/users'
  );
}

   restoreSession() {
  this.refresh().subscribe({
    next: result => {
      this.accessToken.set(result.accessToken);
      this.sessionRestored.set(true);
    },
    error: () => {
      this.accessToken.set(null);
      this.sessionRestored.set(true);
    }
   });
  }

  getAllBookings() {
  return this.httpClient.get<AdminBooking[]>(
    'http://localhost:5086/admin/bookings'
    );
  }

  deleteBooking(id: number) {
  return this.httpClient.delete(
    `http://localhost:5086/bookings/${id}`
    );
  }

  googleLogin(idToken: string) {
  return this.httpClient.post<LoginResult>(
    'http://localhost:5086/auth/google',
    { idToken },
    { withCredentials: true }
  );
  }
}

export interface AdminUser {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  role: string;
}

export interface AdminBooking {
  bookingId: number;
  userId: number;
  flightId: number;
  email: string;
  status: string;
  price: number;
}

export interface RegisterRequest{
  name: string;
  lastName: string;
  email: string;
  password: string;
  passport: string;
}
