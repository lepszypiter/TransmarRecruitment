import { Injectable } from '@angular/core';
import { ApiService } from './api.service';

@Injectable({ providedIn: 'root' })
export class AuthService {
  constructor(private api: ApiService) {}

  async login(email: string, password: string) {
    const res: any = await this.api.post('/api/auth/login', { email, password });
    if (res?.token) {
      localStorage.setItem('token', res.token);
      return res.user;
    }
    throw new Error(res?.message || 'Login failed');
  }

  logout() {
    localStorage.removeItem('token');
  }

  currentUser() {
    return null;
  }
}
