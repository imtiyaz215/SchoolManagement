import { useEffect, useState, type ReactNode } from 'react';
import { authService, type LoginRequest, type LoginResponse } from '../services/auth';
import { AuthContext, type AuthCtx } from './authContext';

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<LoginResponse['user'] | null>(() =>
    JSON.parse(localStorage.getItem('user') ?? 'null')
  );

  useEffect(() => {
    if (user || !localStorage.getItem('access_token')) return;
    authService.me()
      .then((me) => {
        const stored = JSON.parse(localStorage.getItem('user') ?? '{}');
        const merged = { ...stored, ...me };
        localStorage.setItem('user', JSON.stringify(merged));
        setUser(merged);
      })
      .catch(() => {
        localStorage.clear();
        setUser(null);
      });
  }, [user]);

  const login = async (req: LoginRequest) => {
    const res = await authService.login(req);
    localStorage.setItem('access_token', res.accessToken);
    localStorage.setItem('refresh_token', res.refreshToken);
    localStorage.setItem('user', JSON.stringify(res.user));
    setUser(res.user);
  };

  const logout = () => {
    authService.logout();
    setUser(null);
  };

  const value: AuthCtx = { user, login, logout };
  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}
