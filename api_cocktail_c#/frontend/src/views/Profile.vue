<template>
  <div class="profile-container">
    <div class="card">
      <div class="card-header d-flex justify-content-between align-items-center">
        <h2>Mon Profil</h2>
        <button @click="logout" class="btn btn-outline-danger">Déconnexion</button>
      </div>
      <div class="card-body">
        <div v-if="currentUser">
          <div class="mb-3">
            <label class="form-label">Prénom</label>
            <p class="form-control">{{ currentUser.firstName }}</p>
          </div>
          
          <div class="mb-3">
            <label class="form-label">Nom</label>
            <p class="form-control">{{ currentUser.lastName }}</p>
          </div>
          
          <div class="mb-3">
            <label class="form-label">Email</label>
            <p class="form-control">{{ currentUser.email }}</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';

export default {
  name: 'Profile',
  computed: {
    ...mapGetters('auth', ['currentUser'])
  },
  methods: {
    ...mapActions('auth', ['logout'])
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