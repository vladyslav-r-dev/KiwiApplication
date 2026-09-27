import { LoginResult, LoginRequest, RefreshResult, RegisterRequest } from '../models/auth';
import { User, AdminUser } from '../models/user';
import { computed, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  accessToken = signal<string | null>(null);
  sessionRestored = signal(false);

  isAuthenticated = computed(() => this.accessToken() !== null);

  constructor(private httpClient: HttpClient) {}

  register(data: RegisterRequest) {
    return this.httpClient.post('http://localhost:5086/register', data);
  }

  login(data: LoginRequest) {
    return this.httpClient.post<LoginResult>('http://localhost:5086/auth/login', data, {
      withCredentials: true,
    });
  }

  getMe() {
    return this.httpClient.get<User>('http://localhost:5086/auth/me');
  }

  logout() {
    return this.httpClient.post('http://localhost:5086/auth/logout', {}, { withCredentials: true });
  }

  refresh() {
    return this.httpClient.post<RefreshResult>(
      'http://localhost:5086/auth/refresh',
      {},
      { withCredentials: true },
    );
  }

  updateMe(data: { name: string; lastName: string }) {
    return this.httpClient.put<User>('http://localhost:5086/auth/me', data, {
      withCredentials: true,
    });
  }

  getAllUsers() {
    return this.httpClient.get<AdminUser[]>('http://localhost:5086/admin/users');
  }

  restoreSession() {
    this.refresh().subscribe({
      next: (result) => {
        this.accessToken.set(result.accessToken);
        this.sessionRestored.set(true);
      },
      error: () => {
        this.accessToken.set(null);
        this.sessionRestored.set(true);
      },
    });
  }

  googleLogin(idToken: string) {
    return this.httpClient.post<LoginResult>(
      'http://localhost:5086/auth/google',
      { idToken },
      { withCredentials: true },
    );
  }
}
