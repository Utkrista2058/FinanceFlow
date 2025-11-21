<template>
  <div id="app">
    <!-- Sidebar Navigation -->
    <div class="sidebar" :class="{ collapsed: isSidebarCollapsed }">
      <div class="sidebar-header">
        <div class="brand-container">
          <div class="brand-icon">
            <i class="bi bi-wallet2"></i>
          </div>
          <transition name="fade">
            <div v-if="!isSidebarCollapsed" class="brand-text">
              <h4 class="mb-0">FinanceFlow</h4>
              <p class="text-muted small mb-0">Your Money Manager</p>
            </div>
          </transition>
        </div>
        <button class="toggle-btn" @click="toggleSidebar">
          <i class="bi" :class="isSidebarCollapsed ? 'bi-chevron-right' : 'bi-chevron-left'"></i>
        </button>
      </div>

      <div class="sidebar-menu">
        <router-link to="/dashboard" class="menu-item" :class="{ collapsed: isSidebarCollapsed }">
          <div class="menu-icon">
            <i class="bi bi-speedometer2"></i>
          </div>
          <transition name="fade">
            <span v-if="!isSidebarCollapsed" class="menu-text">Dashboard</span>
          </transition>
          <div class="menu-indicator"></div>
        </router-link>

        <router-link to="/income" class="menu-item" :class="{ collapsed: isSidebarCollapsed }">
          <div class="menu-icon">
            <i class="bi bi-cash-coin"></i>
          </div>
          <transition name="fade">
            <span v-if="!isSidebarCollapsed" class="menu-text">Income</span>
          </transition>
          <div class="menu-indicator"></div>
        </router-link>

        <router-link to="/expense" class="menu-item" :class="{ collapsed: isSidebarCollapsed }">
          <div class="menu-icon">
            <i class="bi bi-credit-card"></i>
          </div>
          <transition name="fade">
            <span v-if="!isSidebarCollapsed" class="menu-text">Expenses</span>
          </transition>
          <div class="menu-indicator"></div>
        </router-link>

        <router-link to="/category" class="menu-item" :class="{ collapsed: isSidebarCollapsed }">
          <div class="menu-icon">
            <i class="bi bi-tags"></i>
          </div>
          <transition name="fade">
            <span v-if="!isSidebarCollapsed" class="menu-text">Categories</span>
          </transition>
          <div class="menu-indicator"></div>
        </router-link>

        <div class="menu-divider" v-if="!isSidebarCollapsed"></div>

        <div class="menu-item inactive" :class="{ collapsed: isSidebarCollapsed }">
          <div class="menu-icon">
            <i class="bi bi-gear"></i>
          </div>
          <transition name="fade">
            <span v-if="!isSidebarCollapsed" class="menu-text">Settings</span>
          </transition>
        </div>
      </div>

      <div class="sidebar-footer" v-if="!isSidebarCollapsed">
        <div class="user-card">
          <div class="user-avatar">
            <i class="bi bi-person-circle"></i>
          </div>
          <div class="user-info">
            <p class="user-name mb-0">Utkrista Parajuli</p>
            <p class="user-email mb-0">utkristaparajuli@gmail.com</p>
          </div>
          <button @click="logout" class="logout-btn">
            <i class="bi bi-box-arrow-right"></i> Logout
          </button>
        </div>
      </div>
    </div>

    <!-- Main Content Area -->
    <div class="main-content" :class="{ expanded: isSidebarCollapsed }">
      <!-- Top Bar -->
      <div class="top-bar">
        <div class="top-bar-left">
          <button class="mobile-menu-btn" @click="toggleSidebar">
            <i class="bi bi-list"></i>
          </button>
          <h5 class="page-title mb-0">{{ currentPageTitle }}</h5>
        </div>
        <!-- <div class="top-bar-right">
          <button class="top-bar-icon-btn">
            <i class="bi bi-bell"></i>
            <span class="notification-badge">3</span>
          </button>
          <button class="top-bar-icon-btn">
            <i class="bi bi-question-circle"></i>
          </button>
        </div> -->
        <div class="top-bar-right">
          <NotificationBell />

          <button class="top-bar-icon-btn">
            <i class="bi bi-question-circle"></i>
          </button>
        </div>
      </div>

      <!-- Router View -->
      <div class="content-wrapper">
        <transition name="page" mode="out-in">
          <router-view />
        </transition>
      </div>

      <!-- Footer -->
      <footer class="app-footer">
        <p class="mb-0 text-muted small">
          © Utkrista Parajuli. Made with <i class="bi bi-heart-fill text-danger"></i> for better
          personal financial management
        </p>
      </footer>
    </div>

    <!-- Mobile Overlay -->
    <transition name="fade">
      <div
        v-if="!isSidebarCollapsed && isMobile"
        class="mobile-overlay"
        @click="toggleSidebar"
      ></div>
    </transition>
  </div>
</template>

<script>
import AuthService from '../services/authService'
import NotificationBell from '../components/NotificationBell.vue'
import { onMessageListener } from '../firebase/config.js'

export default {
  name: 'App',
  components: {
    NotificationBell,
  },
  data() {
    return {
      isSidebarCollapsed: false,
      isMobile: false,
    }
  },
  computed: {
    currentPageTitle() {
      const route = this.$route.path
      const titles = {
        '/dashboard': 'Dashboard',
        '/income': 'Income Management',
        '/expense': 'Expense Tracking',
        '/category': 'Category Management',
      }
      return titles[route] || 'FinanceFlow'
    },
  },
  mounted() {
    this.checkMobile()
    window.addEventListener('resize', this.checkMobile)
    this.listenForNotifications()
  },

  beforeUnmount() {
    window.removeEventListener('resize', this.checkMobile)
  },
  methods: {
    toggleSidebar() {
      this.isSidebarCollapsed = !this.isSidebarCollapsed
    },
    checkMobile() {
      this.isMobile = window.innerWidth <= 768
      if (this.isMobile) {
        this.isSidebarCollapsed = true
      }
    },
    logout() {
      AuthService.logout()
      this.$router.push('/login') // redirect to login page
    },
    // toggleSidebar() {
    //   this.isSidebarCollapsed = !this.isSidebarCollapsed
    // },
    // checkMobile() {
    //   this.isMobile = window.innerWidth <= 768
    //   if (this.isMobile) {
    //     this.isSidebarCollapsed = true
    //   }
    // },
    listenForNotifications() {
      onMessageListener()
        .then((payload) => {
          console.log('📬 Notification received:', payload)

          // Option 1: Update NotificationBell component (emit event)
          this.$root.$emit('new-notification', payload)

          // Option 2: Show browser notification
          if (Notification.permission === 'granted') {
            new Notification(payload.notification.title, {
              body: payload.notification.body,
              icon: '/favicon.ico',
              badge: '/favicon.ico',
            })
          }

          // Option 3: Show toast notification (if you have a toast library)
          // this.$toast.info(payload.notification.title)
        })
        .catch((err) => {
          console.log('Notification listener error:', err)
        })
    },
  },
}
</script>

<style>
/* Global Styles */
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  background: linear-gradient(135deg, #0f0f1e 0%, #1a1a2e 100%);
  color: #cfd2e1;
  overflow-x: hidden;
}

#app {
  display: flex;
  min-height: 100vh;
  position: relative;
}

/* Sidebar Styles */
.sidebar {
  width: 280px;
  background: linear-gradient(180deg, #1e1e2f 0%, #27293d 100%);
  border-right: 1px solid rgba(255, 255, 255, 0.05);
  display: flex;
  flex-direction: column;
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  position: fixed;
  left: 0;
  top: 0;
  height: 100vh;
  z-index: 1000;
  box-shadow: 4px 0 20px rgba(0, 0, 0, 0.3);
}

.sidebar.collapsed {
  width: 80px;
}

.sidebar-header {
  padding: 1.5rem 1rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.05);
  display: flex;
  align-items: center;
  justify-content: space-between;
  position: relative;
}

.brand-container {
  display: flex;
  align-items: center;
  gap: 1rem;
  overflow: hidden;
}

.brand-icon {
  width: 48px;
  height: 48px;
  border-radius: 12px;
  background: linear-gradient(135deg, #4f5bd5 0%, #6c7df5 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
  color: white;
  flex-shrink: 0;
  box-shadow: 0 4px 15px rgba(79, 91, 213, 0.4);
}
.logout-btn {
  margin-left: auto;
  background: none;
  border: none;
  color: #ff6b6b;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 4px;
  font-weight: 500;
}

.logout-btn:hover {
  color: #ff4757;
}

.brand-text h4 {
  color: #fff;
  font-weight: 700;
  font-size: 1.2rem;
}

.brand-text .text-muted {
  font-size: 0.75rem;
  color: #8b8fa3;
}

.toggle-btn {
  width: 32px;
  height: 32px;
  border-radius: 8px;
  background: rgba(255, 255, 255, 0.05);
  border: none;
  color: #cfd2e1;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.3s ease;
  flex-shrink: 0;
}

.toggle-btn:hover {
  background: rgba(79, 91, 213, 0.2);
  color: #6c7df5;
}

/* Menu Styles */
.sidebar-menu {
  flex: 1;
  padding: 1rem 0.5rem;
  overflow-y: auto;
  overflow-x: hidden;
}

.sidebar-menu::-webkit-scrollbar {
  width: 4px;
}

.sidebar-menu::-webkit-scrollbar-thumb {
  background: rgba(79, 91, 213, 0.3);
  border-radius: 2px;
}

.menu-item {
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 0.875rem 1rem;
  margin-bottom: 0.5rem;
  border-radius: 12px;
  color: #8b8fa3;
  text-decoration: none;
  transition: all 0.3s ease;
  cursor: pointer;
  position: relative;
  overflow: hidden;
}

.menu-item.collapsed {
  justify-content: center;
  padding: 0.875rem;
}

.menu-item:hover {
  background: rgba(79, 91, 213, 0.1);
  color: #cfd2e1;
}

.menu-item.router-link-active {
  background: linear-gradient(90deg, rgba(79, 91, 213, 0.2) 0%, rgba(108, 125, 245, 0.1) 100%);
  color: #fff;
}

.menu-item.router-link-active .menu-icon {
  color: #6c7df5;
  transform: scale(1.1);
}

.menu-item.router-link-active .menu-indicator {
  opacity: 1;
  transform: scaleY(1);
}

.menu-icon {
  width: 24px;
  height: 24px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.25rem;
  transition: all 0.3s ease;
  flex-shrink: 0;
}

.menu-text {
  font-weight: 500;
  font-size: 0.95rem;
  white-space: nowrap;
}

.menu-indicator {
  position: absolute;
  left: 0;
  top: 50%;
  transform: translateY(-50%) scaleY(0);
  width: 4px;
  height: 24px;
  background: linear-gradient(180deg, #4f5bd5 0%, #6c7df5 100%);
  border-radius: 0 4px 4px 0;
  opacity: 0;
  transition: all 0.3s ease;
}

.menu-divider {
  height: 1px;
  background: rgba(255, 255, 255, 0.05);
  margin: 1rem 0;
}

.menu-item.inactive {
  opacity: 0.5;
  cursor: not-allowed;
}

.menu-item.inactive:hover {
  background: transparent;
  color: #8b8fa3;
}

/* Sidebar Footer */
.sidebar-footer {
  padding: 1rem;
  border-top: 1px solid rgba(255, 255, 255, 0.05);
}

.user-card {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding: 0.75rem;
  background: rgba(255, 255, 255, 0.03);
  border-radius: 12px;
  transition: all 0.3s ease;
}

.user-card:hover {
  background: rgba(255, 255, 255, 0.05);
}

.user-avatar {
  width: 40px;
  height: 40px;
  border-radius: 10px;
  background: linear-gradient(135deg, #4f5bd5 0%, #6c7df5 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
  color: white;
  flex-shrink: 0;
}

.user-info {
  overflow: hidden;
}

.user-name {
  color: #fff;
  font-weight: 600;
  font-size: 0.9rem;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.user-email {
  color: #8b8fa3;
  font-size: 0.75rem;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

/* Main Content */
.main-content {
  flex: 1;
  margin-left: 280px;
  transition: margin-left 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

.main-content.expanded {
  margin-left: 80px;
}

/* Top Bar */
.top-bar {
  height: 70px;
  background: rgba(30, 30, 47, 0.8);
  backdrop-filter: blur(10px);
  border-bottom: 1px solid rgba(255, 255, 255, 0.05);
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 2rem;
  position: sticky;
  top: 0;
  z-index: 100;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.2);
}

.top-bar-left {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.mobile-menu-btn {
  width: 40px;
  height: 40px;
  border-radius: 10px;
  background: rgba(255, 255, 255, 0.05);
  border: none;
  color: #cfd2e1;
  font-size: 1.5rem;
  cursor: pointer;
  display: none;
  align-items: center;
  justify-content: center;
  transition: all 0.3s ease;
}

.mobile-menu-btn:hover {
  background: rgba(79, 91, 213, 0.2);
  color: #6c7df5;
}

.page-title {
  color: #fff;
  font-weight: 600;
}

.top-bar-right {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.top-bar-icon-btn {
  width: 40px;
  height: 40px;
  border-radius: 10px;
  background: rgba(255, 255, 255, 0.05);
  border: none;
  color: #cfd2e1;
  font-size: 1.2rem;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.3s ease;
  position: relative;
}

.top-bar-icon-btn:hover {
  background: rgba(79, 91, 213, 0.2);
  color: #6c7df5;
  transform: translateY(-2px);
}

.notification-badge {
  position: absolute;
  top: -4px;
  right: -4px;
  background: linear-gradient(135deg, #ff4757 0%, #ff3838 100%);
  color: white;
  font-size: 0.7rem;
  font-weight: 600;
  padding: 2px 6px;
  border-radius: 10px;
  box-shadow: 0 2px 8px rgba(255, 71, 87, 0.4);
}

/* Content Wrapper */
.content-wrapper {
  flex: 1;
  padding: 2rem;
  overflow-x: hidden;
}

/* Footer */
.app-footer {
  padding: 1.5rem 2rem;
  background: rgba(30, 30, 47, 0.5);
  border-top: 1px solid rgba(255, 255, 255, 0.05);
  text-align: center;
}

/* Mobile Overlay */
.mobile-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.7);
  backdrop-filter: blur(3px);
  z-index: 999;
}

/* Animations */
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

.page-enter-active {
  animation: pageIn 0.4s ease;
}

.page-leave-active {
  animation: pageOut 0.3s ease;
}

@keyframes pageIn {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes pageOut {
  from {
    opacity: 1;
  }
  to {
    opacity: 0;
  }
}

/* Responsive Design */
@media (max-width: 768px) {
  .sidebar {
    transform: translateX(-100%);
  }

  .sidebar.collapsed {
    transform: translateX(0);
    width: 280px;
  }

  .main-content {
    margin-left: 0;
  }

  .main-content.expanded {
    margin-left: 0;
  }

  .mobile-menu-btn {
    display: flex;
  }

  .content-wrapper {
    padding: 1rem;
  }

  .top-bar {
    padding: 0 1rem;
  }

  .page-title {
    font-size: 1rem;
  }
}

@media (max-width: 480px) {
  .top-bar-right .top-bar-icon-btn:last-child {
    display: none;
  }
}

/* Scrollbar Styles */
::-webkit-scrollbar {
  width: 8px;
  height: 8px;
}

::-webkit-scrollbar-track {
  background: rgba(30, 30, 47, 0.5);
}

::-webkit-scrollbar-thumb {
  background: rgba(79, 91, 213, 0.3);
  border-radius: 4px;
}

::-webkit-scrollbar-thumb:hover {
  background: rgba(79, 91, 213, 0.5);
}
.top-bar-right .notification-bell .bell-button {
  background: rgba(255, 255, 255, 0.05);
  border-color: transparent;
}

.top-bar-right .notification-bell .bell-button:hover {
  background: rgba(79, 91, 213, 0.2);
  border-color: transparent;
}

.top-bar-right .notification-bell .notification-dropdown {
  right: 0;
  top: calc(100% + 12px);
}
</style>
