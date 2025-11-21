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

<script>
import { onMessageListener } from './firebase/config'

export default {
  name: 'App',

  mounted() {
    // Listen for notifications when app loads
    this.listenForNotifications()
  },

  methods: {
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
</script>
