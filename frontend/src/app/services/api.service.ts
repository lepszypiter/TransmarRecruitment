import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ApiService {
  base = (window as any).__env?.API_URL || 'http://localhost:4000';

  token() {
    return localStorage.getItem('token');
  }

  headers() {
    const h: any = { 'Content-Type': 'application/json' };
    const t = this.token();
    if (t) h['Authorization'] = `Bearer ${t}`;
    return h;
  }

  async get(path: string) {
    const res = await fetch(this.base + path, { headers: this.headers() });
    return res.json();
  }

  async post(path: string, body: any) {
    const res = await fetch(this.base + path, { method: 'POST', headers: this.headers(), body: JSON.stringify(body) });
    return res.json();
  }

  async put(path: string, body: any) {
    const res = await fetch(this.base + path, { method: 'PUT', headers: this.headers(), body: JSON.stringify(body) });
    return res.json();
  }

  async delete(path: string) {
    const res = await fetch(this.base + path, { method: 'DELETE', headers: this.headers() });
    if (res.status === 204) return null;
    return res.json();
  }
}
