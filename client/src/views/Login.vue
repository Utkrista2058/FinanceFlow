<!-- 
<template>
  <div class="login-container">
    <div class="animated-background">
      <div class="floating-shape shape-1"></div>
      <div class="floating-shape shape-2"></div>
      <div class="floating-shape shape-3"></div>
      <div class="floating-shape shape-4"></div>
    </div>

    <div class="login-card">
      <div class="login-header">
        <div class="icon-wrapper">
          <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M15 3h4a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2h-4"></path>
            <polyline points="10 17 15 12 10 7"></polyline>
            <line x1="15" y1="12" x2="3" y2="12"></line>
          </svg>
        </div>
        <h2>Welcome Back</h2>
        <p>Sign in to continue your journey</p>
      </div>

      <form @submit.prevent="handleLogin" class="login-form">
        <div class="form-group">
          <label>
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"></path>
              <circle cx="12" cy="7" r="4"></circle>
            </svg>
            Username
          </label>
          <input
            v-model="username"
            type="text"
            placeholder="Enter your username"
            required
            @focus="focusedField = 'username'"
            @blur="focusedField = ''"
          />
        </div>

        <div class="form-group">
          <label>
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <rect x="3" y="11" width="18" height="11" rx="2" ry="2"></rect>
              <path d="M7 11V7a5 5 0 0 1 10 0v4"></path>
            </svg>
            Password
          </label>
          <input
            v-model="password"
            type="password"
            placeholder="Enter your password"
            required
            @focus="focusedField = 'password'"
            @blur="focusedField = ''"
          />
        </div>

        <div v-if="error" class="error-message">
          <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <circle cx="12" cy="12" r="10"></circle>
            <line x1="12" y1="8" x2="12" y2="12"></line>
            <line x1="12" y1="16" x2="12.01" y2="16"></line>
          </svg>
          {{ error }}
        </div>

        <button type="submit" class="btn-login" :disabled="loading">
          <span class="btn-content">
            <span v-if="!loading">
              Sign In
              <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <line x1="5" y1="12" x2="19" y2="12"></line>
                <polyline points="12 5 19 12 12 19"></polyline>
              </svg>
            </span>
            <span v-else class="loading-spinner">
              <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <circle cx="12" cy="12" r="10"></circle>
              </svg>
              Signing in...
            </span>
          </span>
          <div class="btn-shine"></div>
        </button>
      </form>

      <div class="divider">
        <span>or</span>
      </div>

      <div class="login-footer">
        <p>Don't have an account? <router-link to="/register">Sign up now</router-link></p>
      </div>
    </div>
  </div>
</template>

<script>
import authService from '../services/authService';

export default {
  name: 'Login',
  data() {
    return {
      username: '',
      password: '',
      error: '',
      loading: false,
      focusedField: ''
    };
  },
  methods: {
    async handleLogin() {
      this.error = '';
      this.loading = true;

      try {
        await authService.login(this.username, this.password);
        this.$router.push('/dashboard');
      } catch (error) {
        this.error = error.response?.data?.message || 'Login failed';
      } finally {
        this.loading = false;
      }
    }
  }
};
</script>

<style scoped>
* {
  box-sizing: border-box;
}

.login-container {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  padding: 2rem;
  position: relative;
  overflow: hidden;
}

.animated-background {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  overflow: hidden;
  z-index: 0;
}

.floating-shape {
  position: absolute;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.1);
  animation: float 20s infinite ease-in-out;
}

.shape-1 {
  width: 300px;
  height: 300px;
  top: -150px;
  left: -150px;
  animation-delay: 0s;
}

.shape-2 {
  width: 200px;
  height: 200px;
  top: 50%;
  right: -100px;
  animation-delay: 5s;
}

.shape-3 {
  width: 150px;
  height: 150px;
  bottom: -75px;
  left: 20%;
  animation-delay: 10s;
}

.shape-4 {
  width: 250px;
  height: 250px;
  top: 20%;
  right: 15%;
  animation-delay: 15s;
}

@keyframes float {
  0%, 100% {
    transform: translate(0, 0) rotate(0deg);
  }
  25% {
    transform: translate(50px, 50px) rotate(90deg);
  }
  50% {
    transform: translate(0, 100px) rotate(180deg);
  }
  75% {
    transform: translate(-50px, 50px) rotate(270deg);
  }
}

.login-card {
  background: white;
  border-radius: 24px;
  padding: 3rem;
  max-width: 450px;
  width: 100%;
  box-shadow: 0 30px 80px rgba(0, 0, 0, 0.3);
  position: relative;
  z-index: 1;
  animation: slideUp 0.6s ease-out;
}

@keyframes slideUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.login-header {
  text-align: center;
  margin-bottom: 2.5rem;
  animation: fadeIn 0.8s ease-out 0.2s both;
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

.icon-wrapper {
  width: 80px;
  height: 80px;
  margin: 0 auto 1.5rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  animation: pulse 2s infinite;
}

@keyframes pulse {
  0%, 100% {
    transform: scale(1);
    box-shadow: 0 0 0 0 rgba(102, 126, 234, 0.7);
  }
  50% {
    transform: scale(1.05);
    box-shadow: 0 0 0 20px rgba(102, 126, 234, 0);
  }
}

.icon-wrapper svg {
  width: 40px;
  height: 40px;
  color: white;
}

.login-header h2 {
  font-size: 2rem;
  color: #1f2937;
  margin: 0 0 0.5rem 0;
  font-weight: 700;
}

.login-header p {
  color: #6b7280;
  margin: 0;
  font-size: 0.95rem;
}

.login-form {
  animation: fadeIn 0.8s ease-out 0.4s both;
}

.form-group {
  margin-bottom: 1.5rem;
  animation: slideIn 0.5s ease-out both;
}

.form-group:nth-child(1) {
  animation-delay: 0.5s;
}

.form-group:nth-child(2) {
  animation-delay: 0.6s;
}

@keyframes slideIn {
  from {
    opacity: 0;
    transform: translateX(-20px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

.form-group label {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  margin-bottom: 0.5rem;
  color: #374151;
  font-weight: 600;
  font-size: 0.9rem;
}

.form-group label svg {
  width: 18px;
  height: 18px;
  color: #667eea;
}

.form-group input {
  width: 100%;
  padding: 0.875rem 1rem;
  border: 2px solid #e5e7eb;
  border-radius: 12px;
  font-size: 1rem;
  transition: all 0.3s ease;
  background: #f9fafb;
}

.form-group input:focus {
  outline: none;
  border-color: #667eea;
  background: white;
  box-shadow: 0 0 0 4px rgba(102, 126, 234, 0.1);
  transform: translateY(-2px);
}

.error-message {
  background: linear-gradient(135deg, #fee2e2 0%, #fecaca 100%);
  color: #991b1b;
  padding: 1rem;
  border-radius: 12px;
  margin-bottom: 1rem;
  display: flex;
  align-items: center;
  gap: 0.75rem;
  font-size: 0.9rem;
  animation: shake 0.5s ease-in-out;
  border: 2px solid #fca5a5;
}

.error-message svg {
  width: 20px;
  height: 20px;
  flex-shrink: 0;
}

@keyframes shake {
  0%, 100% { transform: translateX(0); }
  25% { transform: translateX(-10px); }
  75% { transform: translateX(10px); }
}

.btn-login {
  width: 100%;
  padding: 1rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  border-radius: 12px;
  font-size: 1rem;
  font-weight: 700;
  cursor: pointer;
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
  margin-top: 0.5rem;
  animation: fadeIn 0.8s ease-out 0.7s both;
}

.btn-content {
  position: relative;
  z-index: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
}

.btn-content svg {
  width: 20px;
  height: 20px;
  transition: transform 0.3s ease;
}

.btn-login:hover:not(:disabled) {
  transform: translateY(-3px);
  box-shadow: 0 15px 35px rgba(102, 126, 234, 0.4);
}

.btn-login:hover:not(:disabled) .btn-content svg {
  transform: translateX(5px);
}

.btn-login:active:not(:disabled) {
  transform: translateY(-1px);
}

.btn-shine {
  position: absolute;
  top: 0;
  left: -100%;
  width: 100%;
  height: 100%;
  background: linear-gradient(90deg, 
    transparent, 
    rgba(255, 255, 255, 0.3), 
    transparent
  );
  transition: left 0.5s ease;
}

.btn-login:hover:not(:disabled) .btn-shine {
  left: 100%;
}

.btn-login:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}

.loading-spinner {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.loading-spinner svg {
  width: 20px;
  height: 20px;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
}

.divider {
  display: flex;
  align-items: center;
  margin: 2rem 0 1.5rem;
  animation: fadeIn 0.8s ease-out 0.8s both;
}

.divider::before,
.divider::after {
  content: '';
  flex: 1;
  height: 1px;
  background: linear-gradient(90deg, transparent, #e5e7eb, transparent);
}

.divider span {
  padding: 0 1rem;
  color: #9ca3af;
  font-size: 0.875rem;
  font-weight: 500;
}

.login-footer {
  text-align: center;
  animation: fadeIn 0.8s ease-out 0.9s both;
}

.login-footer p {
  color: #6b7280;
  margin: 0;
}

.login-footer a {
  color: #667eea;
  text-decoration: none;
  font-weight: 700;
  transition: all 0.3s ease;
  position: relative;
}

.login-footer a::after {
  content: '';
  position: absolute;
  bottom: -2px;
  left: 0;
  width: 0;
  height: 2px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  transition: width 0.3s ease;
}

.login-footer a:hover {
  color: #764ba2;
}

.login-footer a:hover::after {
  width: 100%;
}

@media (max-width: 480px) {
  .login-card {
    padding: 2rem 1.5rem;
  }

  .login-header h2 {
    font-size: 1.5rem;
  }

  .icon-wrapper {
    width: 60px;
    height: 60px;
  }

  .icon-wrapper svg {
    width: 30px;
    height: 30px;
  }
}
</style> -->

<template>
  <div class="login-container">
    <div class="animated-background">
      <div class="floating-shape shape-1"></div>
      <div class="floating-shape shape-2"></div>
      <div class="floating-shape shape-3"></div>
      <div class="floating-shape shape-4"></div>
    </div>

    <div class="login-card">
      <div class="login-header">
        <div class="icon-wrapper">
          <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M15 3h4a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2h-4"></path>
            <polyline points="10 17 15 12 10 7"></polyline>
            <line x1="15" y1="12" x2="3" y2="12"></line>
          </svg>
        </div>
        <h2>Welcome Back</h2>
        <p>Sign in to continue your journey</p>
      </div>

      <form @submit.prevent="handleLogin" class="login-form">
        <div class="form-group">
          <label>
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"></path>
              <circle cx="12" cy="7" r="4"></circle>
            </svg>
            Username
          </label>
          <input
            v-model="username"
            type="text"
            placeholder="Enter your username"
            required
            @focus="focusedField = 'username'"
            @blur="focusedField = ''"
          />
        </div>

        <div class="form-group">
          <label>
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <rect x="3" y="11" width="18" height="11" rx="2" ry="2"></rect>
              <path d="M7 11V7a5 5 0 0 1 10 0v4"></path>
            </svg>
            Password
          </label>
          <input
            v-model="password"
            type="password"
            placeholder="Enter your password"
            required
            @focus="focusedField = 'password'"
            @blur="focusedField = ''"
          />
        </div>

        <div v-if="error" class="error-message">
          <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <circle cx="12" cy="12" r="10"></circle>
            <line x1="12" y1="8" x2="12" y2="12"></line>
            <line x1="12" y1="16" x2="12.01" y2="16"></line>
          </svg>
          {{ error }}
        </div>

        <button type="submit" class="btn-login" :disabled="loading">
          <span class="btn-content">
            <span v-if="!loading">
              Sign In
              <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <line x1="5" y1="12" x2="19" y2="12"></line>
                <polyline points="12 5 19 12 12 19"></polyline>
              </svg>
            </span>
            <span v-else class="loading-spinner">
              <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <circle cx="12" cy="12" r="10"></circle>
              </svg>
              Signing in...
            </span>
          </span>
          <div class="btn-shine"></div>
        </button>
      </form>

      <div class="divider">
        <span>or</span>
      </div>

      <!-- Google Sign-In Button -->
      <button @click="signInWithGoogle" class="btn-google" :disabled="loadingGoogle">
        <span class="btn-content">
          <span v-if="!loadingGoogle" class="google-content">
            <svg viewBox="0 0 24 24" width="20" height="20">
              <path fill="#4285F4" d="M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57c2.08-1.92 3.28-4.74 3.28-8.09z"/>
              <path fill="#34A853" d="M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99 20.53 7.7 23 12 23z"/>
              <path fill="#FBBC05" d="M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.07H2.18C1.43 8.55 1 10.22 1 12s.43 3.45 1.18 4.93l2.85-2.22.81-.62z"/>
              <path fill="#EA4335" d="M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15C17.45 2.09 14.97 1 12 1 7.7 1 3.99 3.47 2.18 7.07l3.66 2.84c.87-2.6 3.3-4.53 6.16-4.53z"/>
            </svg>
            Continue with Google
          </span>
          <span v-else class="loading-spinner">
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <circle cx="12" cy="12" r="10"></circle>
            </svg>
            Connecting...
          </span>
        </span>
      </button>

      <div class="login-footer">
        <p>Don't have an account? <router-link to="/register">Sign up now</router-link></p>
      </div>
    </div>
  </div>
</template>

<script>
import authService from '../services/authService';
import { signInWithPopup } from 'firebase/auth';
import { auth, googleProvider } from '../firebase/config';

export default {
  name: 'Login',
  data() {
    return {
      username: '',
      password: '',
      error: '',
      loading: false,
      loadingGoogle: false,
      focusedField: ''
    };
  },
  methods: {
    async handleLogin() {
      this.error = '';
      this.loading = true;

      try {
        await authService.login(this.username, this.password);
        this.$router.push('/dashboard');
      } catch (error) {
        this.error = error.response?.data?.message || 'Login failed';
      } finally {
        this.loading = false;
      }
    },

    async signInWithGoogle() {
      this.error = '';
      this.loadingGoogle = true;

      try {
        const result = await signInWithPopup(auth, googleProvider);
        const user = result.user;
        
        // Get the ID token to send to your backend
        const idToken = await user.getIdToken();
        
        // Send token to your .NET backend for verification
        // You'll need to create this method in your authService
        await authService.loginWithGoogle(idToken);
        
        // Redirect to dashboard on success
        this.$router.push('/dashboard');
      } catch (error) {
        console.error('Error signing in with Google:', error);
        this.error = error.message || 'Google sign-in failed';
      } finally {
        this.loadingGoogle = false;
      }
    }
  }
};
</script>

<style scoped>
* {
  box-sizing: border-box;
}

.login-container {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  padding: 2rem;
  position: relative;
  overflow: hidden;
}

.animated-background {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  overflow: hidden;
  z-index: 0;
}

.floating-shape {
  position: absolute;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.1);
  animation: float 20s infinite ease-in-out;
}

.shape-1 {
  width: 300px;
  height: 300px;
  top: -150px;
  left: -150px;
  animation-delay: 0s;
}

.shape-2 {
  width: 200px;
  height: 200px;
  top: 50%;
  right: -100px;
  animation-delay: 5s;
}

.shape-3 {
  width: 150px;
  height: 150px;
  bottom: -75px;
  left: 20%;
  animation-delay: 10s;
}

.shape-4 {
  width: 250px;
  height: 250px;
  top: 20%;
  right: 15%;
  animation-delay: 15s;
}

@keyframes float {
  0%, 100% {
    transform: translate(0, 0) rotate(0deg);
  }
  25% {
    transform: translate(50px, 50px) rotate(90deg);
  }
  50% {
    transform: translate(0, 100px) rotate(180deg);
  }
  75% {
    transform: translate(-50px, 50px) rotate(270deg);
  }
}

.login-card {
  background: white;
  border-radius: 24px;
  padding: 3rem;
  max-width: 450px;
  width: 100%;
  box-shadow: 0 30px 80px rgba(0, 0, 0, 0.3);
  position: relative;
  z-index: 1;
  animation: slideUp 0.6s ease-out;
}

@keyframes slideUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.login-header {
  text-align: center;
  margin-bottom: 2.5rem;
  animation: fadeIn 0.8s ease-out 0.2s both;
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

.icon-wrapper {
  width: 80px;
  height: 80px;
  margin: 0 auto 1.5rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  animation: pulse 2s infinite;
}

@keyframes pulse {
  0%, 100% {
    transform: scale(1);
    box-shadow: 0 0 0 0 rgba(102, 126, 234, 0.7);
  }
  50% {
    transform: scale(1.05);
    box-shadow: 0 0 0 20px rgba(102, 126, 234, 0);
  }
}

.icon-wrapper svg {
  width: 40px;
  height: 40px;
  color: white;
}

.login-header h2 {
  font-size: 2rem;
  color: #1f2937;
  margin: 0 0 0.5rem 0;
  font-weight: 700;
}

.login-header p {
  color: #6b7280;
  margin: 0;
  font-size: 0.95rem;
}

.login-form {
  animation: fadeIn 0.8s ease-out 0.4s both;
}

.form-group {
  margin-bottom: 1.5rem;
  animation: slideIn 0.5s ease-out both;
}

.form-group:nth-child(1) {
  animation-delay: 0.5s;
}

.form-group:nth-child(2) {
  animation-delay: 0.6s;
}

@keyframes slideIn {
  from {
    opacity: 0;
    transform: translateX(-20px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

.form-group label {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  margin-bottom: 0.5rem;
  color: #374151;
  font-weight: 600;
  font-size: 0.9rem;
}

.form-group label svg {
  width: 18px;
  height: 18px;
  color: #667eea;
}

.form-group input {
  width: 100%;
  padding: 0.875rem 1rem;
  border: 2px solid #e5e7eb;
  border-radius: 12px;
  font-size: 1rem;
  transition: all 0.3s ease;
  background: #f9fafb;
}

.form-group input:focus {
  outline: none;
  border-color: #667eea;
  background: white;
  box-shadow: 0 0 0 4px rgba(102, 126, 234, 0.1);
  transform: translateY(-2px);
}

.error-message {
  background: linear-gradient(135deg, #fee2e2 0%, #fecaca 100%);
  color: #991b1b;
  padding: 1rem;
  border-radius: 12px;
  margin-bottom: 1rem;
  display: flex;
  align-items: center;
  gap: 0.75rem;
  font-size: 0.9rem;
  animation: shake 0.5s ease-in-out;
  border: 2px solid #fca5a5;
}

.error-message svg {
  width: 20px;
  height: 20px;
  flex-shrink: 0;
}

@keyframes shake {
  0%, 100% { transform: translateX(0); }
  25% { transform: translateX(-10px); }
  75% { transform: translateX(10px); }
}

.btn-login {
  width: 100%;
  padding: 1rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  border-radius: 12px;
  font-size: 1rem;
  font-weight: 700;
  cursor: pointer;
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
  margin-top: 0.5rem;
  animation: fadeIn 0.8s ease-out 0.7s both;
}

.btn-content {
  position: relative;
  z-index: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
}

.btn-content svg {
  width: 20px;
  height: 20px;
  transition: transform 0.3s ease;
}

.btn-login:hover:not(:disabled) {
  transform: translateY(-3px);
  box-shadow: 0 15px 35px rgba(102, 126, 234, 0.4);
}

.btn-login:hover:not(:disabled) .btn-content svg {
  transform: translateX(5px);
}

.btn-login:active:not(:disabled) {
  transform: translateY(-1px);
}

.btn-shine {
  position: absolute;
  top: 0;
  left: -100%;
  width: 100%;
  height: 100%;
  background: linear-gradient(90deg, 
    transparent, 
    rgba(255, 255, 255, 0.3), 
    transparent
  );
  transition: left 0.5s ease;
}

.btn-login:hover:not(:disabled) .btn-shine {
  left: 100%;
}

.btn-login:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}

.loading-spinner {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.loading-spinner svg {
  width: 20px;
  height: 20px;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
}

.divider {
  display: flex;
  align-items: center;
  margin: 2rem 0 1.5rem;
  animation: fadeIn 0.8s ease-out 0.8s both;
}

.divider::before,
.divider::after {
  content: '';
  flex: 1;
  height: 1px;
  background: linear-gradient(90deg, transparent, #e5e7eb, transparent);
}

.divider span {
  padding: 0 1rem;
  color: #9ca3af;
  font-size: 0.875rem;
  font-weight: 500;
}

/* Google Sign-In Button Styles */
.btn-google {
  width: 100%;
  padding: 1rem;
  background: white;
  color: #3c4043;
  border: 2px solid #e5e7eb;
  border-radius: 12px;
  font-size: 1rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
  animation: fadeIn 0.8s ease-out 0.85s both;
  margin-bottom: 1.5rem;
}

.btn-google .google-content {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.75rem;
}

.btn-google:hover:not(:disabled) {
  transform: translateY(-3px);
  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.15);
  border-color: #dadce0;
  background: #f8f9fa;
}

.btn-google:active:not(:disabled) {
  transform: translateY(-1px);
}

.btn-google:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}

.login-footer {
  text-align: center;
  animation: fadeIn 0.8s ease-out 0.9s both;
}

.login-footer p {
  color: #6b7280;
  margin: 0;
}

.login-footer a {
  color: #667eea;
  text-decoration: none;
  font-weight: 700;
  transition: all 0.3s ease;
  position: relative;
}

.login-footer a::after {
  content: '';
  position: absolute;
  bottom: -2px;
  left: 0;
  width: 0;
  height: 2px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  transition: width 0.3s ease;
}

.login-footer a:hover {
  color: #764ba2;
}

.login-footer a:hover::after {
  width: 100%;
}

@media (max-width: 480px) {
  .login-card {
    padding: 2rem 1.5rem;
  }

  .login-header h2 {
    font-size: 1.5rem;
  }

  .icon-wrapper {
    width: 60px;
    height: 60px;
  }

  .icon-wrapper svg {
    width: 30px;
    height: 30px;
  }
}
</style>