<template>
  <div class="profile-container">
    <div class="card">
      <div class="card-header d-flex justify-content-between align-items-center">
        <h2>Mon Profil</h2>
        <button @click="logout" class="btn btn-outline-danger">Déconnexion</button>
      </div>
      <div class="card-body">
        <div v-if="currentUser">
          <form @submit.prevent="handleUpdate" class="mb-4">
            <div class="mb-3">
              <label class="form-label">Prénom*</label>
              <input type="text" class="form-control" v-model="user.firstName" required minlength="3" />
            </div>
            <div class="mb-3">
              <label class="form-label">Nom*</label>
              <input type="text" class="form-control" v-model="user.lastName" required minlength="3" />
            </div>
            <div class="mb-3">
              <label class="form-label">Email*</label>
              <input type="email" class="form-control" v-model="user.email" required />
            </div>
            <div class="mb-3">
              <label class="form-label">Nouveau mot de passe*</label>
              <input type="password" class="form-control" v-model="user.password" minlength="6" placeholder="" />
            </div>
            <button type="submit" class="btn btn-primary me-2" :disabled="loading">Mettre à jour</button>
            <button type="button" class="btn btn-danger" @click="confirmDelete" :disabled="loading">Supprimer mon compte</button>
          </form>
          <div v-if="successMessage" class="alert alert-success">{{ successMessage }}</div>
          <div v-if="errorMessage" class="alert alert-danger">{{ errorMessage }}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
import UserService from '@/services/user.service';

export default {
  name: 'Profile',
  data() {
    return {
      user: {
        firstName: '',
        lastName: '',
        email: '',
        password: ''
      },
      loading: false,
      successMessage: '',
      errorMessage: ''
    };
  },
  computed: {
    ...mapGetters('auth', ['currentUser'])
  },
  watch: {
    currentUser: {
      immediate: true,
      handler(newVal) {
        if (newVal) {
          this.user.firstName = newVal.firstName;
          this.user.lastName = newVal.lastName;
          this.user.email = newVal.email;
          this.user.password = '';
        }
      }
    }
  },
  methods: {
    ...mapActions('auth', ['logout']),
    async handleUpdate() {
      this.loading = true;
      this.successMessage = '';
      this.errorMessage = '';
      try {
        const userId = this.currentUser.userId || this.currentUser._id || this.currentUser.id;
        const updateData = { ...this.user };
        if (!updateData.password) delete updateData.password;
        await UserService.updateProfile(userId, updateData);
        this.successMessage = 'Profil mis à jour avec succès.';
      } catch (error) {
        this.errorMessage = "Erreur lors de la mise à jour du profil.";
      } finally {
        this.loading = false;
      }
    },
    async confirmDelete() {
      if (confirm('Êtes-vous sûr de vouloir supprimer votre compte ? Cette action est irréversible.')) {
        await this.handleDelete();
      }
    },
    async handleDelete() {
      this.loading = true;
      this.errorMessage = '';
      try {
        const userId = this.currentUser.userId || this.currentUser._id || this.currentUser.id;
        await UserService.deleteUser(userId);
        this.$swal({ icon: 'success', title: 'Compte supprimé', text: 'Votre compte a bien été supprimé.' });
        await this.logout();
        this.$router.push('/register');
      } catch (error) {
        this.errorMessage = "Erreur lors de la suppression du compte.";
      } finally {
        this.loading = false;
      }
    }
  }
};
</script>

<style scoped lang="scss">
.profile-container {
  padding: 20px;
  max-width: 800px;
  margin: 0 auto;
  
  .card {
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  }
}
</style>