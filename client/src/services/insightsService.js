import api from './api';

export default {
  getMonthlyInsights() {
    return api.get('/insights/monthly');
  }
};
