
import api from './api';

export default {
 getCategories() { return api.get('/categories'); },
getCategory(id) { return api.get(`/categories/${id}`); },
createCategory(category) { return api.post('/categories', category); },
updateCategory(id, category) { return api.put(`/categories/${id}`, category); },
deleteCategory(id) { return api.delete(`/categories/${id}`); }


};

