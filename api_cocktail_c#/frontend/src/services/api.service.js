import axios from 'axios';

const ApiService = {
  init() {
    axios.defaults.baseURL = 'http://localhost:5000';

    
    axios.interceptors.request.use(
      config => {
        const token = localStorage.getItem('token');
        if (token) {
          config.headers['Authorization'] = `Bearer ${token}`;
        }
        return config;
      },
      error => Promise.reject(error)
    );
  },

  getBaseUrl() {
    return axios.defaults.baseURL || '';
  },

  get(resource) {
    return axios.get(resource);
  },

  post(resource, data) {
    return axios.post(resource, data);
  },

  put(resource, data) {
    return axios.put(resource, data);
  },

  delete(resource) {
    return axios.delete(resource);
  }
};

export default ApiService;