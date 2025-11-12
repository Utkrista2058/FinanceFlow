// import { createRouter, createWebHistory } from 'vue-router'
// import authService from '../services/authService';
// import Income from '../views/Income.vue';  
// import Category from '../views/Category.vue'
// import Expense from '../views/Expense.vue';
// import Dashboard from '../views/Dashboard.vue';

// const routes = [
//   {path: '/', redirect: '/login' },
  
//     {
//     path: '/login',
//     name: 'Login',
//     component: () => import('../views/Login.vue')
//   },
//     {
//     path: '/register',
//     name: 'Register',
//     component: () => import('../views/Register.vue')
//   },
//   {
//     path: '/categories',
//     name: 'Categories',
//     component: () => import('../views/Category.vue'),
//     meta: { requiresAuth: true }
//   },
//   { path: '/category', component: Category },
//   {path: '/income', component: Income },
//   {path: '/expense', component: Expense },
//   {path: '/dashboard', component: Dashboard },
//   // add other routes here
// ]

// const router = createRouter({
//   history: createWebHistory(),
//   routes,
// });

// // Navigation guard
// router.beforeEach((to, from, next) => {
//   const user = authService.getCurrentUser();
  
//   if (to.meta.requiresAuth && !user) {
//     next('/login');
//   } else if ((to.path === '/login' || to.path === '/register') && user) {
//     next('/categories');
//   } else {
//     next();
//   }
// });

// export default router;


import { createRouter, createWebHistory } from 'vue-router'
import authService from '../services/authService';

// Import Layouts
import AppLayout from '../layouts/AppLayout.vue';
import AuthLayout from '../layouts/AuthLayout.vue';

// Import Views
import Income from '../views/Income.vue';  
import Category from '../views/Category.vue';
import Expense from '../views/Expense.vue';
import Dashboard from '../views/Dashboard.vue';

const routes = [
  // Root redirect
  {
    path: '/',
    redirect: (to) => {
      const user = authService.getCurrentUser();
      return user ? '/dashboard' : '/login';
    }
  },
  
  // Auth Routes (WITHOUT sidebar) - using AuthLayout
  {
    path: '/',
    component: AuthLayout,
    children: [
      {
        path: 'login',
        name: 'Login',
        component: () => import('../views/Login.vue'),
        meta: { guest: true }
      },
      {
        path: 'register',
        name: 'Register',
        component: () => import('../views/Register.vue'),
        meta: { guest: true }
      }
    ]
  },

  // App Routes (WITH sidebar) - using AppLayout
  {
    path: '/',
    component: AppLayout,
    meta: { requiresAuth: true },
    children: [
      {
        path: 'dashboard',
        name: 'Dashboard',
        component: Dashboard
      },
      {
        path: 'income',
        name: 'Income',
        component: Income
      },
      {
        path: 'expense',
        name: 'Expense',
        component: Expense
      },
      {
        path: 'category',
        name: 'Category',
        component: Category
      },
      {
        path: 'categories',
        name: 'Categories',
        component: Category
      }
    ]
  },

  // Catch-all redirect
  {
    path: '/:pathMatch(.*)*',
    redirect: '/dashboard'
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes,
});

// Navigation guard
router.beforeEach((to, from, next) => {
  const user = authService.getCurrentUser();
  
  // Check if route requires authentication
  if (to.meta.requiresAuth && !user) {
    next('/login');
  } 
  // Check if route is for guests only (login/register)
  else if (to.meta.guest && user) {
    next('/dashboard');
  } 
  else {
    next();
  }
});

export default router;

