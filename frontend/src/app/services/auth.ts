import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LoginResult } from '../pages/login/login-result';
import { LoginRequest } from '../pages/login/login';
import { signal } from '@angular/core';
import { RefreshResult } from '../pages/login/refresh-result';

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
    return this.httpClient.get(
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
}


interface RegisterRequest{
  name: string;
  lastName: string;
  email: string;
  password: string;
  passport: string;
}
