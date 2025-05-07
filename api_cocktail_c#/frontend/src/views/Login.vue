<template>
  <div class="login-container">
    <div class="card">
      <div class="card-header">
        <h2>Connexion</h2>
      </div>
      <div class="card-body">
        <form @submit.prevent="handleLogin">
          <div v-if="error" class="alert alert-danger">{{ error }}</div>
          
          <div class="mb-3">
            <label for="email" class="form-label">Email</label>
            <input 
              type="email" 
              class="form-control" 
              id="email" 
              v-model="credentials.email" 
              required
            />
          </div>
          
          <div class="mb-3">
            <label for="password" class="form-label">Mot de passe</label>
            <input 
              type="password" 
              class="form-control" 
              id="password" 
              v-model="credentials.password" 
              required
            />
          </div>
          
          <button 
            type="submit" 
            class="btn btn-primary w-100" 
            :disabled="loading"
          >
            <span v-if="loading" class="spinner-border spinner-border-sm"></span>
            Se connecter
          </button>
          
          <div class="mt-3 text-center">
            <router-link to="/register">Pas de compte ? Inscrivez-vous</router-link>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';

export default {
  name: 'Login',
  data() {
    return {
      credentials: {
        email: '',
        password: ''
      },
      loading: false
    };
  },
  computed: {
    ...mapGetters('auth', ['error'])
  },
  methods: {
    ...mapActions('auth', ['login']),
    async handleLogin() {
      this.loading = true;
      try {
        await this.login(this.credentials);
        this.$router.push('/profile');
      } catch (error) {
        console.error('Erreur de connexion:', error);
      } finally {
        this.loading = false;
      }
    }
  }
};
</script>

<style scoped lang="scss">
.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 80vh;
  padding: 20px;

  .card {
    width: 100%;
    max-width: 500px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  }
}
</style>