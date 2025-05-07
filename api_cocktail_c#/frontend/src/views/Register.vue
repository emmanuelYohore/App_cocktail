<template>
  <div class="register-container">
    <div class="card">
      <div class="card-header">
        <h2>Créer un compte</h2>
      </div>
      <div class="card-body">
        <form @submit.prevent="handleRegister">
          <div v-if="error" class="alert alert-danger">{{ error }}</div>
          
          <div class="mb-3">
            <label for="firstName" class="form-label">Prénom</label>
            <input 
              type="text" 
              class="form-control" 
              id="firstName" 
              v-model="user.firstName" 
              required
              minlength="3"
            />
          </div>
          
          <div class="mb-3">
            <label for="lastName" class="form-label">Nom</label>
            <input 
              type="text" 
              class="form-control" 
              id="lastName" 
              v-model="user.lastName" 
              required
              minlength="3"
            />
          </div>
          
          <div class="mb-3">
            <label for="email" class="form-label">Email</label>
            <input 
              type="email" 
              class="form-control" 
              id="email" 
              v-model="user.email" 
              required
            />
          </div>
          
          <div class="mb-3">
            <label for="password" class="form-label">Mot de passe</label>
            <input 
              type="password" 
              class="form-control" 
              id="password" 
              v-model="user.password" 
              required
              minlength="6"
            />
          </div>
          
          <button 
            type="submit" 
            class="btn btn-primary w-100" 
            :disabled="loading"
          >
            <span v-if="loading" class="spinner-border spinner-border-sm"></span>
            S'inscrire
          </button>
          
          <div class="mt-3 text-center">
            <router-link to="/login">Déjà inscrit ? Connectez-vous</router-link>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
import UserService from '@/services/user.service';

export default {
  name: 'Register',
  data() {
    return {
      user: {
        firstName: '',
        lastName: '',
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
    ...mapActions('auth', ['register']),
    async handleRegister() {
      this.loading = true;
      try {
        await UserService.register(this.user);
        this.$router.push('/login');
        this.$swal({
          icon: 'success',
          title: 'Inscription réussie',
          text: 'Vous pouvez maintenant vous connecter'
        });
      } catch (error) {
        console.error('Erreur lors de l\'inscription:', error);
      } finally {
        this.loading = false;
      }
    }
  }
};
</script>

<style scoped lang="scss">
.register-container {
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