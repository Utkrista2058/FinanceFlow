import api from './api';

export default {
  async downloadExcelReport() {
    return api.get('/Report/export-excel', {
      responseType: 'blob' 
    });
  }
};