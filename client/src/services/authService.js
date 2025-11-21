// // src/services/authService.js
// // import axios from 'axios';
// import api from './api';
// const API_URL= '/auth'

// // const API_URL = 'http://localhost:7230/api/auth';

// class AuthService {
//   async register(username, email, password) {
//     const response = await api.post(`${API_URL}/register`, {
//       username,
//       email,
//       password
//     });

//     if (response.data.token) {
//       localStorage.setItem('user', JSON.stringify(response.data));
//     }

//     return response.data;
//   }

//   async login(username, password) {
//     const response = await api.post(`${API_URL}/login`, {
//       username,
//       password
//     });

//     if (response.data.token) {
//       localStorage.setItem('user', JSON.stringify(response.data));
//     }

//     return response.data;
//   }
//   async loginWithGoogle(idToken) {
//     const response = await api.post(`${API_URL}/google-login`, {
//       idToken
//     });

//     if (response.data.token) {
//       localStorage.setItem('user', JSON.stringify(response.data));
//     }

//     return response.data;
//   }

//   logout() {
//     localStorage.removeItem('user');
//   }

//   getCurrentUser() {
//     return JSON.parse(localStorage.getItem('user'));
//   }

//   getToken() {
//     const user = this.getCurrentUser();
//     return user?.token;
//   }
// }

// export default new AuthService();

// src/services/authService.js
import api from './api'
const API_URL = '/auth'

class AuthService {
  // Helper function to decode JWT and extract userId
  getUserIdFromToken(token) {
    try {
      const base64Url = token.split('.')[1]
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/')
      const jsonPayload = decodeURIComponent(
        atob(base64)
          .split('')
          .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
          .join(''),
      )

      const payload = JSON.parse(jsonPayload)

      // Your JWT has this claim for userId
      return payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']
    } catch (error) {
      console.error('Error decoding token:', error)
      return null
    }
  }

  async register(username, email, password) {
    const response = await api.post(`${API_URL}/register`, {
      username,
      email,
      password,
    })

    if (response.data.token) {
      localStorage.setItem('user', JSON.stringify(response.data))

      // Store userId separately for easy access
      const userId = this.getUserIdFromToken(response.data.token)
      if (userId) {
        localStorage.setItem('userId', userId)
      }
    }

    return response.data
  }

  async login(username, password) {
    const response = await api.post(`${API_URL}/login`, {
      username,
      password,
    })

    if (response.data.token) {
      localStorage.setItem('user', JSON.stringify(response.data))

      // Store userId separately for easy access
      const userId = this.getUserIdFromToken(response.data.token)
      if (userId) {
        localStorage.setItem('userId', userId)
      }
    }

    return response.data
  }

  async loginWithGoogle(idToken) {
    const response = await api.post(`${API_URL}/google-login`, {
      idToken,
    })

    if (response.data.token) {
      localStorage.setItem('user', JSON.stringify(response.data))

      // Store userId separately for easy access
      const userId = this.getUserIdFromToken(response.data.token)
      if (userId) {
        localStorage.setItem('userId', userId)
      }
    }

    return response.data
  }

  logout() {
    localStorage.removeItem('user')
    localStorage.removeItem('userId')
  }

  getCurrentUser() {
    return JSON.parse(localStorage.getItem('user'))
  }

  getToken() {
    const user = this.getCurrentUser()
    return user?.token
  }

  getUserId() {
    return localStorage.getItem('userId')
  }
}

export default new AuthService()
