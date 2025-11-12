// import axios from 'axios';
import api from './api';
// const apiClient = axios.create({
//   baseURL: 'https://localhost:7230/api', // your backend base URL
//   headers: { 'Content-Type': 'application/json' }
// });

// export default {
//   getIncomes() { return apiClient.get('/incomes'); },
//   getIncome(id) { return apiClient.get(`/incomes/${id}`); },
//   createIncome(income) { return apiClient.post('/incomes', income); },
//   updateIncome(id, income) { return apiClient.put(`/incomes/${id}`, income); },
//   deleteIncome(id) { return apiClient.delete(`/incomes/${id}`); }
// };

export default {
  getIncomes() { return api.get('/incomes'); },
  getIncome(id) { return api.get(`/incomes/${id}`); },
  createIncome(income) { return api.post('/incomes', income); },
  updateIncome(id, income) { return api.put(`/incomes/${id}`, income); },
  deleteIncome(id) { return api.delete(`/incomes/${id}`); }
};