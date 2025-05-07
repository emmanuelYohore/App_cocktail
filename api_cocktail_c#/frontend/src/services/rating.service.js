import ApiService from './api.service';

const RatingService = {
  getAllRatings() {
    return ApiService.get('/api/ratings');
  },
  
  getRatingsByCocktailId(cocktailId) {
    return ApiService.get(`/api/ratings?cocktailId=${cocktailId}`);
  },

  getRating(id) {
    return ApiService.get(`/api/ratings/${id}`);
  },

  createRating(rating) {
    return ApiService.post('/api/ratings', rating);
  },

  updateRating(id, rating) {
    return ApiService.put(`/api/ratings/${id}`, rating);
  },

  deleteRating(id) {
    return ApiService.delete(`/api/ratings/${id}`);
  }
};

export default RatingService;