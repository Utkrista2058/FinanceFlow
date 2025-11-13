// src/services/authService.js
// import axios from 'axios';
import api from './api';
const API_URL= '/auth'

// const API_URL = 'http://localhost:7230/api/auth';

class AuthService {
  async register(username, email, password) {
    const response = await api.post(`${API_URL}/register`, {
      username,
      email,
      password
    });
    
    if (response.data.token) {
      localStorage.setItem('user', JSON.stringify(response.data));
    }
    
    return response.data;
  }

  async login(username, password) {
    const response = await api.post(`${API_URL}/login`, {
      username,
      password
    });
    
    if (response.data.token) {
      localStorage.setItem('user', JSON.stringify(response.data));
    }
    
    return response.data;
  }
  async loginWithGoogle(idToken) {
    const response = await api.post(`${API_URL}/google-login`, {
      idToken
    });
    
    if (response.data.token) {
      localStorage.setItem('user', JSON.stringify(response.data));
    }
    
    return response.data;
  }

  logout() {
    localStorage.removeItem('user');
  }

  getCurrentUser() {
    return JSON.parse(localStorage.getItem('user'));
  }

  getToken() {
    const user = this.getCurrentUser();
    return user?.token;
  }
}

export default new AuthService();