<template>
  <div id="app">
    <header>
      <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
        <div class="container">
          <router-link class="navbar-brand" to="/">Cocktail App</router-link>
          
          <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
            <span class="navbar-toggler-icon"></span>
          </button>
          
          <div class="collapse navbar-collapse" id="navbarNav">
            <ul class="navbar-nav ms-auto">
              <li class="nav-item">
                <router-link class="nav-link" to="/">Accueil</router-link>
              </li>
              <template v-if="!isAuthenticated">
                <li class="nav-item">
                  <router-link class="nav-link" to="/login">Connexion</router-link>
                </li>
                <li class="nav-item">
                  <router-link class="nav-link" to="/register">Inscription</router-link>
                </li>
              </template>
              <template v-else>
                <li class="nav-item">
                  <span class="nav-link text-light">
                    <i class="bi bi-person-circle me-1"></i>
                    Bonjour, {{ currentUser ? (currentUser.firstName || 'Utilisateur') : 'Utilisateur' }}
                  </span>
                </li>
                <li class="nav-item">
                  <router-link class="nav-link" to="/profile">Mon Profil</router-link>
                </li>
                <li class="nav-item">
                  <a class="nav-link" href="#" @click.prevent="logout">Déconnexion</a>
                </li>
              </template>
            </ul>
          </div>
        </div>
      </nav>
    </header>
    
    <main class="container mt-4">
      <router-view />
    </main>
    
    <footer class="bg-light text-center text-muted py-3 mt-5">
      <div class="container">
        <p class="mb-0">&copy; {{ new Date().getFullYear() }} Cocktail App</p>
      </div>
    </footer>
  </div>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';

export default {
  name: 'App',
  computed: {
    ...mapGetters('auth', ['isAuthenticated', 'currentUser'])
  },
  methods: {
    ...mapActions('auth', ['logout'])
  }
};
</script>