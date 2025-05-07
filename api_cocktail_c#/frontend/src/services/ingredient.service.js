import ApiService from './api.service';

const IngredientService = {
  getAllIngredients() {
    return ApiService.get('/api/ingredients');
  },

  getIngredient(id) {
    return ApiService.get(`/api/ingredients/${id}`);
  },

  createIngredient(ingredient) {
    return ApiService.post('/api/ingredients', ingredient);
  },

  updateIngredient(id, ingredient) {
    return ApiService.put(`/api/ingredients/${id}`, ingredient);
  },

  deleteIngredient(id) {
    return ApiService.delete(`/api/ingredients/${id}`);
  }
};

export default IngredientService;