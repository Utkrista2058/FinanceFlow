import { initializeApp } from 'firebase/app'
import { getAuth, GoogleAuthProvider } from 'firebase/auth'
import { getMessaging, getToken, onMessage } from 'firebase/messaging'

const firebaseConfig = {
  apiKey: 'AIzaSyA9hsv_Vq0pkrMcwxSGc0RGWPxpINncE_Q',
  authDomain: 'financeflow-69a60.firebaseapp.com',
  projectId: 'financeflow-69a60',
  storageBucket: 'financeflow-69a60.firebasestorage.app',
  messagingSenderId: '296943535197',
  appId: '1:296943535197:web:8a0bf2d438108b0beeb9b4',
  measurementId: 'G-43MELL5LNV',
}

const app = initializeApp(firebaseConfig)
const auth = getAuth(app)
const googleProvider = new GoogleAuthProvider()
const messaging = getMessaging(app)

export const requestNotificationPermission = async () => {
  try {
    const permission = await Notification.requestPermission()

    if (permission === 'granted') {
      console.log('Notification permission granted')

      // Get FCM token
      const token = await getToken(messaging, {
        vapidKey:
          'BPv0E-eIP-LaC2BZkGncvcca7m938M5e0MQGucRTS3btEhzhjRBUQQpRoO8XqcURbumapuPXWiXBciiSQZxiLSo', // You'll get this from Firebase Console
      })

      if (token) {
        console.log('FCM Token:', token)
        return token
      } else {
        console.log('No registration token available')
        return null
      }
    } else {
      console.log('Notification permission denied')
      return null
    }
  } catch (error) {
    console.error('Error getting notification permission:', error)
    return null
  }
}

// Listen for foreground messages
export const onMessageListener = () =>
  new Promise((resolve) => {
    onMessage(messaging, (payload) => {
      console.log('Message received in foreground:', payload)
      resolve(payload)
    })
  })

export { auth, googleProvider, messaging }
