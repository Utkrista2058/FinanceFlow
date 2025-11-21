<!-- <template>

      <div class="content-wrapper">
        <transition name="page" mode="out-in">
          <router-view />
        </transition>
      </div>


      </template>

<script></script>
 -->
<template>
  <!-- Router View -->
  <div class="content-wrapper">
    <transition name="page" mode="out-in">
      <router-view />
    </transition>
  </div>
</template>

<!-- <script>
import { onMessageListener } from './firebase/config'
import axios from 'axios'


export default {
  name: 'App',
  data() {
    return {
      fcmTokenRegistered: false
    }
  },

  mounted() {
    // Listen for notifications when app loads
    this.listenForNotifications()
    this.registerDevice()
  },

  methods: {
    async registerDevice() {
      try {
        // Check if user is logged in
        const token = localStorage.getItem('token')
        const userId = localStorage.getItem('userId') // Make sure you store userId on login

        if (!token || !userId) {
          console.log('User not logged in, skipping notification registration')
          return
        }

        // Request permission and get FCM token
        const fcmToken = await requestNotificationPermission()

        if (fcmToken) {
          console.log('Got FCM token, registering with backend...')

          // Register with backend
          const response = await axios.post(
            'https://financeflow-2esa.onrender.com/api/Notifications/register',
            {
              userId: parseInt(userId),
              fcmToken: fcmToken
            },
            {
              headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
              }
            }
          )

          console.log('✅ Device registered:', response.data)
          this.fcmTokenRegistered = true
        } else {
          console.log('No FCM token obtained')
        }
      } catch (error) {
        console.error('❌ Error registering device:', error)
      }
    },
    listenForNotifications() {
      onMessageListener()
        .then((payload) => {
          console.log(' Notification received:', payload)

          // Show notification in the app
          this.showNotification(payload)
        })
        .catch((err) => {
          console.error('Notification listener error:', err)
        })
    },

    showNotification(payload) {
      // Option 1: Browser notification (if you want)
      if (Notification.permission === 'granted') {
        new Notification(payload.notification.title, {
          body: payload.notification.body,
          icon: '/favicon.ico',
        })
      }

      // Option 2: Console log for now (you can add toast later)
      console.log('Title:', payload.notification.title)
      console.log('Body:', payload.notification.body)

      // Option 3: If you have a toast library, use it
      // this.$toast.info(payload.notification.title);
    },
  },
}
</script> -->
<script>
import { onMessageListener, requestNotificationPermission } from './firebase/config'
import authService from './services/authService'
import api from './services/api'

export default {
  name: 'App',

  data() {
    return {
      fcmTokenRegistered: false,
    }
  },

  mounted() {
    // Listen for notifications when app loads
    this.listenForNotifications()

    // Register device for push notifications if user is logged in
    this.registerDevice()
  },

  methods: {
    async registerDevice() {
      try {
        // Check if user is logged in
        const token = authService.getToken()
        const userId = authService.getUserId()

        if (!token || !userId) {
          console.log('User not logged in, skipping notification registration')
          return
        }

        // Request permission and get FCM token
        console.log('Requesting notification permission...')
        const fcmToken = await requestNotificationPermission()

        if (fcmToken) {
          console.log('✅ Got FCM token:', fcmToken.substring(0, 20) + '...')

          // Register with backend using the api service
          const response = await api.post('/Notifications/register', {
            userId: parseInt(userId),
            fcmToken: fcmToken,
          })

          console.log('✅ Device registered successfully:', response.data)
          this.fcmTokenRegistered = true
        } else {
          console.log('⚠️ No FCM token obtained - user may have denied permission')
        }
      } catch (error) {
        console.error('❌ Error registering device:', error)
      }
    },

    listenForNotifications() {
      onMessageListener()
        .then((payload) => {
          console.log('📬 Notification received:', payload)

          // Show notification in the app
          this.showNotification(payload)
        })
        .catch((err) => {
          console.error('Notification listener error:', err)
        })
    },

    showNotification(payload) {
      // Browser notification
      if (Notification.permission === 'granted') {
        new Notification(payload.notification.title, {
          body: payload.notification.body,
          icon: '/favicon.ico',
        })
      }

      console.log('📬 Title:', payload.notification.title)
      console.log('📬 Body:', payload.notification.body)
    },
  },
}
</script>
