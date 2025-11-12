// import axios from 'axios';
import api from './api';
// const apiClient = axios.create({
//   baseURL: 'https://localhost:7230/api',
//   headers: { 'Content-Type': 'application/json' }
// });

// export default {
//   getExpenses() { return apiClient.get('/expense'); },
//   getExpense(id) { return apiClient.get(`/expense/${id}`); },
//   createExpense(expense) { return apiClient.post('/expense', expense); },
//   updateExpense(id, expense) { return apiClient.put(`/expense/${id}`, expense); },
//   deleteExpense(id) { return apiClient.delete(`/expense/${id}`); }
// };
export default {
  getExpenses() { return api.get('/expense'); },
  getExpense(id) { return api.get(`/expense/${id}`); },
  createExpense(expense) { return api.post('/expense', expense); },
  updateExpense(id, expense) { return api.put(`/expense/${id}`, expense); },
  deleteExpense(id) { return api.delete(`/expense/${id}`); }
};

