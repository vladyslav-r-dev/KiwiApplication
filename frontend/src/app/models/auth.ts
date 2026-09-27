export interface LoginResult {
  accessToken: string;
}
export interface RefreshResult {
  accessToken: string;
}
export interface LoginRequest {
  email: string;
  password: string;
}
export interface RegisterRequest {
  name: string;
  lastName: string;
  email: string;
  password: string;
  passport: string;
}
