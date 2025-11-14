<template>
  <div class="notification-bell">
    <button
      @click="toggleDropdown"
      class="bell-button"
      :class="{ 'has-notifications': unreadCount > 0 }"
    >
      <i class="bi bi-bell-fill"></i>
      <span v-if="unreadCount > 0" class="notification-badge">{{ unreadCount }}</span>
    </button>

    <!-- Dropdown -->
    <transition name="dropdown">
      <div v-if="showDropdown" class="notification-dropdown">
        <div class="dropdown-header">
          <h3>Notifications</h3>
          <button v-if="unreadCount > 0" @click="markAllRead" class="mark-all-btn">
            Mark all as read
          </button>
        </div>

        <div class="notifications-list">
          <div
            v-for="notification in notifications"
            :key="notification.id"
            :class="['notification-item', { unread: !notification.isRead }]"
            @click="handleNotificationClick(notification)"
          >
            <div class="notification-icon" :class="`icon-${notification.type}`">
              {{ getIcon(notification.type) }}
            </div>
            <div class="notification-content">
              <h4>{{ notification.title }}</h4>
              <p>{{ notification.message }}</p>
              <span class="notification-time">{{ formatTime(notification.createdAt) }}</span>
            </div>
          </div>

          <div v-if="notifications.length === 0" class="no-notifications">
            <i class="bi bi-inbox"></i>
            <p>No notifications yet</p>
          </div>
        </div>
      </div>
    </transition>
  </div>
</template>

<script>
import { ref, onMounted, onUnmounted } from 'vue'
import { useRouter } from 'vue-router'
import notificationService from '../services/notificationService'

export default {
  name: 'NotificationBell',
  setup() {
    const router = useRouter()
    const showDropdown = ref(false)
    const notifications = ref([])
    const unreadCount = ref(0)
    let pollInterval = null

    const loadNotifications = async () => {
      try {
        notifications.value = await notificationService.getNotifications(20)
        unreadCount.value = await notificationService.getUnreadCount()
      } catch (error) {
        console.error('Error loading notifications:', error)
      }
    }

    const toggleDropdown = () => {
      showDropdown.value = !showDropdown.value
      if (showDropdown.value) {
        loadNotifications()
      }
    }

    const handleNotificationClick = async (notification) => {
      if (!notification.isRead) {
        await notificationService.markAsRead(notification.id)
        notification.isRead = true
        unreadCount.value = Math.max(0, unreadCount.value - 1)
      }

      if (notification.link) {
        router.push(notification.link)
      }

      showDropdown.value = false
    }

    const markAllRead = async () => {
      try {
        await notificationService.markAllAsRead()
        notifications.value.forEach((n) => (n.isRead = true))
        unreadCount.value = 0
      } catch (error) {
        console.error('Error marking all as read:', error)
      }
    }

    const startPolling = () => {
      pollInterval = setInterval(() => {
        loadNotifications()
      }, 30000) // Poll every 30 seconds
    }

    const stopPolling = () => {
      if (pollInterval) {
        clearInterval(pollInterval)
      }
    }

    const getIcon = (type) => {
      const icons = {
        budget_alert: '⚠️',
        transaction: '💳',
        reminder: '📅',
        goal: '🎯',
        report: '📊',
      }
      return icons[type] || '🔔'
    }

    const formatTime = (dateString) => {
      const date = new Date(dateString)
      const now = new Date()
      const diff = now - date
      const minutes = Math.floor(diff / 60000)
      const hours = Math.floor(diff / 3600000)
      const days = Math.floor(diff / 86400000)

      if (minutes < 1) return 'Just now'
      if (minutes < 60) return `${minutes}m ago`
      if (hours < 24) return `${hours}h ago`
      if (days < 7) return `${days}d ago`
      return date.toLocaleDateString()
    }

    onMounted(() => {
      loadNotifications()
      startPolling()
    })

    onUnmounted(() => {
      stopPolling()
    })

    return {
      showDropdown,
      notifications,
      unreadCount,
      toggleDropdown,
      handleNotificationClick,
      markAllRead,
      getIcon,
      formatTime,
    }
  },
}
</script>

<style scoped>
.notification-bell {
  position: relative;
}

.bell-button {
  position: relative;
  background: white;
  border: 2px solid #e5e7eb;
  width: 44px;
  height: 44px;
  border-radius: 12px;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #6b7280;
  transition: all 0.3s ease;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
}

.bell-button:hover {
  background: #f9fafb;
  border-color: #667eea;
  color: #667eea;
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.2);
}

.bell-button.has-notifications {
  animation: ring 2s ease-in-out infinite;
}

@keyframes ring {
  0%,
  100% {
    transform: rotate(0deg);
  }
  10%,
  30% {
    transform: rotate(-10deg);
  }
  20%,
  40% {
    transform: rotate(10deg);
  }
}

.bell-button i {
  font-size: 1.25rem;
}

.notification-badge {
  position: absolute;
  top: -6px;
  right: -6px;
  background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%);
  color: white;
  border-radius: 50%;
  width: 20px;
  height: 20px;
  font-size: 0.7rem;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 700;
  border: 2px solid white;
  animation: pulse 2s ease-in-out infinite;
}

@keyframes pulse {
  0%,
  100% {
    transform: scale(1);
  }
  50% {
    transform: scale(1.1);
  }
}

/* Dropdown */
.notification-dropdown {
  position: absolute;
  top: calc(100% + 12px);
  right: 0;
  width: 400px;
  max-height: 600px;
  background: white;
  border-radius: 16px;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.15);
  overflow: hidden;
  z-index: 1000;
  border: 1px solid #e5e7eb;
}

.dropdown-enter-active,
.dropdown-leave-active {
  transition: all 0.3s ease;
}

.dropdown-enter-from {
  opacity: 0;
  transform: translateY(-10px);
}

.dropdown-leave-to {
  opacity: 0;
  transform: translateY(-10px);
}

.dropdown-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.25rem 1.5rem;
  border-bottom: 1px solid #f3f4f6;
  background: #fafbfc;
}

.dropdown-header h3 {
  margin: 0;
  font-size: 1.125rem;
  color: #1f2937;
  font-weight: 600;
}

.mark-all-btn {
  background: none;
  border: none;
  color: #667eea;
  cursor: pointer;
  font-size: 0.875rem;
  font-weight: 600;
  padding: 0.375rem 0.75rem;
  border-radius: 8px;
  transition: all 0.2s;
}

.mark-all-btn:hover {
  background: #ede9fe;
}

.notifications-list {
  max-height: 500px;
  overflow-y: auto;
}

.notifications-list::-webkit-scrollbar {
  width: 6px;
}

.notifications-list::-webkit-scrollbar-track {
  background: #f3f4f6;
}

.notifications-list::-webkit-scrollbar-thumb {
  background: #d1d5db;
  border-radius: 3px;
}

.notification-item {
  display: flex;
  gap: 1rem;
  padding: 1rem 1.5rem;
  border-bottom: 1px solid #f3f4f6;
  cursor: pointer;
  transition: all 0.2s ease;
}

.notification-item:hover {
  background: #f9fafb;
}

.notification-item.unread {
  background: linear-gradient(90deg, #eff6ff 0%, #f0f9ff 100%);
  border-left: 3px solid #667eea;
}

.notification-icon {
  width: 44px;
  height: 44px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
  flex-shrink: 0;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.notification-icon.icon-budget_alert {
  background: linear-gradient(135deg, #fa709a 0%, #fee140 100%);
}

.notification-icon.icon-transaction {
  background: linear-gradient(135deg, #43e97b 0%, #38f9d7 100%);
}

.notification-icon.icon-reminder {
  background: linear-gradient(135deg, #4facfe 0%, #00f2fe 100%);
}

.notification-content {
  flex: 1;
  min-width: 0;
}

.notification-content h4 {
  margin: 0 0 0.375rem 0;
  font-size: 0.9375rem;
  color: #1f2937;
  font-weight: 600;
  line-height: 1.3;
}

.notification-content p {
  margin: 0 0 0.5rem 0;
  font-size: 0.875rem;
  color: #6b7280;
  line-height: 1.4;
  overflow: hidden;
  text-overflow: ellipsis;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  line-clamp: 2;
  -webkit-box-orient: vertical;
}

.notification-time {
  font-size: 0.75rem;
  color: #9ca3af;
  font-weight: 500;
}

.no-notifications {
  padding: 4rem 2rem;
  text-align: center;
  color: #9ca3af;
}

.no-notifications i {
  font-size: 3rem;
  margin-bottom: 1rem;
  display: block;
  opacity: 0.5;
}

.no-notifications p {
  margin: 0;
  font-size: 0.9375rem;
}

@media (max-width: 768px) {
  .notification-dropdown {
    width: 320px;
    right: -100px;
  }
}
</style>
