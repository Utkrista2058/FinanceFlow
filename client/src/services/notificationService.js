import api from './api'

export default {
  async getNotifications(limit = 50) {
    const response = await api.get(`/notification?limit=${limit}`)
    return response.data
  },

  async getUnreadCount() {
    const response = await api.get('/notification/unread-count')
    return response.data.count
  },

  async markAsRead(notificationId) {
    const response = await api.patch(`/notification/${notificationId}/read`)
    return response.data
  },

  async markAllAsRead() {
    const response = await api.patch('/notification/mark-all-read')
    return response.data
  },
}
