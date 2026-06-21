import { createContext } from 'react';
import type { LoginRequest, LoginResponse } from '../services/auth';

export interface AuthCtx {
  user: LoginResponse['user'] | null;
  login: (req: LoginRequest) => Promise<void>;
  logout: () => void;
}

export const AuthContext = createContext<AuthCtx | null>(null);
