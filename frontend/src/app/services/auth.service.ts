import { API_URL } from '../config/api.config';
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
    return this.httpClient.post(`${API_URL}/register`, data);
  }

  login(data: LoginRequest) {
    return this.httpClient.post<LoginResult>(`${API_URL}/auth/login`, data, {
      withCredentials: true,
    });
  }

  getMe() {
    return this.httpClient.get<User>(`${API_URL}/auth/me`);
  }

  logout() {
    return this.httpClient.post(`${API_URL}/auth/logout`, {}, { withCredentials: true });
  }

  refresh() {
    return this.httpClient.post<RefreshResult>(
      `${API_URL}/auth/refresh`,
      {},
      { withCredentials: true },
    );
  }

  updateMe(data: { name: string; lastName: string }) {
    return this.httpClient.put<User>(`${API_URL}/auth/me`, data, {
      withCredentials: true,
    });
  }

  getAllUsers() {
    return this.httpClient.get<AdminUser[]>(`${API_URL}/admin/users`);
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
      `${API_URL}/auth/google`,
      { idToken },
      { withCredentials: true },
    );
  }
}
