import ApiService from './api.service';

const AuthService = {
  login(credentials) {
    return ApiService.post('/api/auth/login', credentials);
  },

  logout() {
    return ApiService.post('/api/auth/logout', {
      token: localStorage.getItem('token')
    }).then(() => {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
    });
  },

  getAuthUser() {
    const userStr = localStorage.getItem('user');
    return userStr ? JSON.parse(userStr) : null;
  },

  isAuthenticated() {
    return !!localStorage.getItem('token');
  }
};

export default AuthService;