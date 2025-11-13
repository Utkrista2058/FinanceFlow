import { initializeApp } from 'firebase/app';
import { getAuth, GoogleAuthProvider } from 'firebase/auth';

const firebaseConfig = {
  apiKey: "AIzaSyA9hsv_Vq0pkrMcwxSGc0RGWPxpINncE_Q",
  authDomain: "financeflow-69a60.firebaseapp.com",
  projectId: "financeflow-69a60",
  storageBucket: "financeflow-69a60.firebasestorage.app",
  messagingSenderId: "296943535197",
  appId: "1:296943535197:web:8a0bf2d438108b0beeb9b4",
  measurementId: "G-43MELL5LNV"
};

const app = initializeApp(firebaseConfig);
const auth = getAuth(app);
const googleProvider = new GoogleAuthProvider();

export { auth, googleProvider };