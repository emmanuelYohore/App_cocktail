<template>
  <div class="home-container">
    <div class="jumbotron">
      <h1>Bienvenue sur l'application Cocktail</h1>
      <p class="lead">Découvrez et partagez vos cocktails préférés</p>
      <div class="mt-4">
        <router-link to="/login" class="btn btn-primary me-2" v-if="!isAuthenticated">Se connecter</router-link>
        <router-link to="/register" class="btn btn-success" v-if="!isAuthenticated">S'inscrire</router-link>
        <router-link to="/profile" class="btn btn-primary me-2" v-if="isAuthenticated">Mon profil</router-link>
        <router-link to="/create-cocktail" class="btn btn-success" v-if="isAuthenticated">Créer un cocktail</router-link>
      </div>
    </div>

    <h2 class="text-center mb-4">Nos cocktails</h2>
    
    <div v-if="loading" class="text-center">
      <div class="spinner-border" role="status">
        <span class="visually-hidden">Chargement...</span>
      </div>
    </div>

    <div v-else-if="error" class="alert alert-danger">
      {{ error }}
    </div>

    <div v-else-if="cocktails.length === 0" class="text-center">
      <p>Aucun cocktail disponible pour le moment.</p>
    </div>

    <div v-else class="row row-cols-1 row-cols-md-3 g-4">
      <div v-for="cocktail in cocktails" :key="cocktail.cocktailId" class="col">
        <div class="card h-100">
          <div v-if="cocktail.cocktailImage" class="card-img-container">
            <img :src="getImageUrl(cocktail.cocktailImage)" class="card-img-top" alt="Photo du cocktail">
          </div>
          <div v-else class="card-img-container bg-light d-flex justify-content-center align-items-center">
            <span class="text-muted">Pas d'image</span>
          </div>
          <div class="card-body">
            <h5 class="card-title">{{ cocktail.name }}</h5>
            <p class="card-text">{{ cocktail.description || 'Aucune description disponible' }}</p>
            <p class="card-text">
              <small class="text-muted">
                <i class="bi bi-person-circle"></i>
                Par: {{ cocktailCreators[cocktail.cocktailId]?.firstName || 'Utilisateur inconnu' }}
              </small>
            </p>
            <div class="rating-info" v-if="cocktailRatings[cocktail.cocktailId]">
              <strong>Note moyenne:</strong> {{ getAverageRating(cocktail.cocktailId) }}/5
              <div class="stars">
                <i v-for="star in 5" :key="star" 
                   :class="['bi', star <= getAverageRating(cocktail.cocktailId) ? 'bi-star-fill' : 'bi-star']"></i>
              </div>
            </div>
          </div>
          <div class="card-footer">
            <router-link :to="'/cocktails/' + cocktail.cocktailId" class="btn btn-primary btn-sm">
              Voir les détails
            </router-link>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex';
import CocktailService from '@/services/cocktail.service';
import RatingService from '@/services/rating.service';
import UserService from '@/services/user.service';
import ApiService from '@/services/api.service';

export default {
  name: 'Home',
  data() {
    return {
      cocktails: [],
      cocktailRatings: {},
      cocktailCreators: {},
      loading: true,
      error: null
    };
  },
  computed: {
    ...mapGetters('auth', ['isAuthenticated'])
  },
  methods: {
    async fetchCocktails() {
      this.loading = true;
      this.error = null;
      
      try {
        const response = await CocktailService.getAllCocktails();
        this.cocktails = response.data;
        
        
        for (const cocktail of this.cocktails) {
          await this.fetchRatingsForCocktail(cocktail.cocktailId);
          
          if (cocktail.userId) {
            await this.fetchCreatorInfo(cocktail.cocktailId, cocktail.userId);
          }
        }
      } catch (error) {
        console.error('Erreur lors de la récupération des cocktails:', error);
        this.error = 'Impossible de charger les cocktails. Veuillez réessayer.';
      } finally {
        this.loading = false;
      }
    },
    async fetchRatingsForCocktail(cocktailId) {
      try {
        const response = await RatingService.getRatingsByCocktailId(cocktailId);
        this.cocktailRatings[cocktailId] = response.data;
      } catch (error) {
        console.error(`Erreur lors de la récupération des notes pour le cocktail ${cocktailId}:`, error);
      }
    },
    async fetchCreatorInfo(cocktailId, userId) {
      try {
        const response = await UserService.getUser(userId);
        this.cocktailCreators[cocktailId] = response.data;
      } catch (error) {
        console.error(`Erreur lors de la récupération du créateur pour le cocktail ${cocktailId}:`, error);
      }
    },
    getAverageRating(cocktailId) {
      const ratings = this.cocktailRatings[cocktailId];
      if (!ratings || ratings.length === 0) return 0;
      
      const sum = ratings.reduce((total, rating) => total + rating.value, 0);
      return (sum / ratings.length).toFixed(1);
    },
    getImageUrl(imagePath) {
      if (!imagePath) return '';
      
      
      if (imagePath.startsWith('http://') || imagePath.startsWith('https://')) {
        return imagePath;
      }
      
      
      const cleanPath = imagePath.split(':')[0];
      
      const baseUrl = ApiService.getBaseUrl();
      
      return `${baseUrl}${cleanPath.startsWith('/') ? '' : '/'}${cleanPath}`;
    }
  },
  created() {
    this.fetchCocktails();
  }
};
</script>

<style scoped lang="scss">
.home-container {
  padding: 20px;
  
  .jumbotron {
    background-color: #f8f9fa;
    padding: 2rem;
    margin-bottom: 2rem;
    border-radius: 0.3rem;
    text-align: center;
  }

  .card-img-container {
    height: 200px;
    overflow: hidden;
    
    img {
      width: 100%;
      height: 100%;
      object-fit: cover;
    }
  }

  .rating-info {
    margin-top: 10px;
    
    .stars {
      color: #ffc107;
      margin-top: 5px;
    }
  }
}
</style>