import AuthService from '@/services/auth.service';
import UserService from '@/services/user.service';
import router from '@/router';

export default {
  namespaced: true,
  state: {
    user: AuthService.getAuthUser(),
    token: localStorage.getItem('token') || '',
    status: '',
    error: null
  },
  getters: {
    isAuthenticated: state => !!state.token,
    authStatus: state => state.status,
    currentUser: state => state.user,
    error: state => state.error
  },
  mutations: {
    AUTH_REQUEST(state) {
      state.status = 'loading';
      state.error = null;
    },
    AUTH_SUCCESS(state, { user, token }) {
      state.status = 'success';
      state.token = token;
      state.user = user;
      state.error = null;
    },
    AUTH_ERROR(state, error) {
      state.status = 'error';
      state.error = error;
    },
    LOGOUT(state) {
      state.status = '';
      state.token = '';
      state.user = null;
    }
  },
  actions: {
    login({ commit }, credentials) {
      commit('AUTH_REQUEST');
      return AuthService.login(credentials)
        .then(response => {
          const { token, ...user } = response.data;
          localStorage.setItem('token', token);
          localStorage.setItem('user', JSON.stringify(user));
          commit('AUTH_SUCCESS', { user, token });
          return user;
        })
        .catch(err => {
          commit('AUTH_ERROR', err.response?.data?.message || 'Erreur de connexion');
          localStorage.removeItem('token');
          localStorage.removeItem('user');
          throw err;
        });
    },
    logout({ commit }) {
      return AuthService.logout()
        .then(() => {
          commit('LOGOUT');
          router.push('/login');
        })
        .catch(error => {
          console.error('Erreur lors de la déconnexion:', error);
          commit('LOGOUT');
          localStorage.removeItem('token');
          localStorage.removeItem('user');
          router.push('/login');
        });
    },
    register({ commit }, userData) {
      commit('AUTH_REQUEST');
      return UserService.register(userData)
        .then(response => {
          return response.data;
        })
        .catch(err => {
          commit('AUTH_ERROR', err.response?.data?.message || 'Erreur d\'inscription');
          throw err;
        });
    }
  }
};