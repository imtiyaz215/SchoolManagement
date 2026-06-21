import { api } from '../lib/api';

export interface LoginRequest {
  email: string;
  password: string;
  schoolId?: string;
}

export interface LoginResponse {
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
  user: { id: string; email: string; fullName: string; role: string; schoolId: string };
}

export const authService = {
  login: (req: LoginRequest) => api.post<LoginResponse>('/auth/login', req).then((r) => r.data),
  me: () => api.get('/auth/me').then((r) => r.data),
  logout: () => {
    localStorage.clear();
    window.location.href = '/login';
  },
};
