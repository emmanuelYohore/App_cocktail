import ApiService from './api.service';

const UserService = {
  register(userData) {
    return ApiService.post('/api/user', userData);
  },

  getProfile(userId) {
    return ApiService.get(`/api/user/${userId}`);
  },

  getUser(userId) {
    return ApiService.get(`/api/user/${userId}`);
  },

  updateProfile(userId, userData) {
    return ApiService.put(`/api/user/${userId}`, userData);
  }
};

export default UserService;