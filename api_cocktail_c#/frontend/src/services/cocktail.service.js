import ApiService from './api.service';

const CocktailService = {
  getAllCocktails() {
    return ApiService.get('/api/cocktails');
  },

  getCocktail(id) {
    return ApiService.get(`/api/cocktails/${id}`);
  },

  createCocktail(cocktailData, image) {
    const formData = new FormData();
    formData.append('Name', cocktailData.Name);
    formData.append('Description', cocktailData.Description);
    formData.append('UserId', cocktailData.UserId);
    
    if (image) {
      formData.append('CocktailImage', image);
    }
    
    return ApiService.post('/api/cocktails', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });
  },

  updateCocktail(id, cocktailData, image) {
    const formData = new FormData();
    
    if (cocktailData.Name) {
      formData.append('Name', cocktailData.Name);
    }
    
    if (cocktailData.Description) {
      formData.append('Description', cocktailData.Description);
    }
    
    if (image) {
      formData.append('CocktailImage', image);
    }
    
    return ApiService.put(`/api/cocktails/${id}`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });
  },

  deleteCocktail(id) {
    return ApiService.delete(`/api/cocktails/${id}`);
  }
};

export default CocktailService;