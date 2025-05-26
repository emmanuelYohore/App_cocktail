<template>
  <div class="cocktail-details">
    <div v-if="loading" class="text-center my-5">
      <div class="spinner-border" role="status">
        <span class="visually-hidden">Chargement...</span>
      </div>
    </div>

    <div v-else-if="error" class="alert alert-danger">
      {{ error }}
    </div>

    <div v-else-if="!cocktail" class="alert alert-warning">
      Le cocktail demandé n'existe pas.
    </div>

    <div v-else>
      <div class="card mb-4">
        <div class="row g-0">
          <div class="col-md-4">
            <div v-if="cocktail.cocktailImage" class="cocktail-image">
              <img :src="getImageUrl(cocktail.cocktailImage)" class="img-fluid rounded-start" alt="Photo du cocktail">
            </div>
            <div v-else class="cocktail-image bg-light d-flex justify-content-center align-items-center">
              <span class="text-muted">Pas d'image</span>
            </div>
          </div>
          <div class="col-md-8">
            <div class="card-body">
              <h2 class="card-title">{{ cocktail.name }}</h2>
              <p class="card-text">{{ cocktail.description || 'Aucune description disponible' }}</p>
              <p class="card-text">
                <small class="text-muted">
                  Créé par: {{ creator ? creator.firstName : 'Utilisateur inconnu' }} le {{ formatDate(cocktail.createdAt) }}
                </small>
              </p>

              <div class="rating-summary mb-3" v-if="ratings && ratings.length > 0">
                <h5>Note moyenne: {{ averageRating }}/5</h5>
                <div class="stars">
                  <i v-for="star in 5" :key="`avg-${star}`" 
                    :class="['bi', star <= Math.round(averageRating) ? 'bi-star-fill' : 'bi-star']"></i>
                </div>
                <p class="mt-1">{{ ratings.length }} avis</p>
              </div>
              <div v-else class="text-muted">Aucune note pour l'instant</div>
            </div>
          </div>
        </div>
      </div>

      <div class="card mb-4">
        <div class="card-header">
          <h4>Ingrédients</h4>
        </div>
        <div class="card-body">
          <div v-if="ingredients && ingredients.length > 0">
            <ul class="list-group">
              <li v-for="ingredient in ingredients" :key="ingredient.ingredientId" class="list-group-item">
                <div class="d-flex justify-content-between align-items-center">
                  <div>
                    <strong>{{ ingredient.name.join(', ') }}</strong>
                    <span class="badge bg-secondary ms-2">{{ ingredient.category }}</span>
                  </div>
                  <div>
                    {{ ingredient.quantity }} {{ ingredient.unit }}
                  </div>
                </div>
              </li>
            </ul>
          </div>
          <div v-else class="text-muted">Aucun ingrédient ajouté pour ce cocktail.</div>
        </div>
      </div>

      <div class="card mb-4">
        <div class="card-header d-flex justify-content-between align-items-center">
          <h4>Commentaires et évaluations</h4>
          <button v-if="isAuthenticated" 
                 class="btn btn-primary btn-sm" 
                 @click="showRatingForm = !showRatingForm">
            {{ showRatingForm ? 'Annuler' : 'Ajouter un avis' }}
          </button>
        </div>
        <div class="card-body">
          <div v-if="showRatingForm" class="mb-4">
            <form @submit.prevent="submitRating">
              <div class="mb-3">
                <label for="rating" class="form-label">Votre note:</label>
                <div class="rating-input">
                  <i v-for="star in 5" 
                     :key="`input-${star}`" 
                     :class="['bi', star <= newRating.value ? 'bi-star-fill' : 'bi-star']"
                     @click="setRating(star)"
                     @mouseover="hoverRating = star"
                     @mouseleave="hoverRating = 0"
                     :style="{ color: (hoverRating >= star || newRating.value >= star) ? '#ffc107' : '#e4e5e9' }"></i>
                </div>
                <div class="mt-2">
                  <span>Note sélectionnée: {{ newRating.value || '0' }}/5</span>
                </div>
              </div>
              <div class="mb-3">
                <label for="comment" class="form-label">Votre commentaire:</label>
                <textarea class="form-control" id="comment" v-model="newRating.comment" rows="3"></textarea>
              </div>
              <button type="submit" class="btn btn-success" :disabled="submitting || newRating.value === 0">
                <span v-if="submitting" class="spinner-border spinner-border-sm me-1" role="status" aria-hidden="true"></span>
                Envoyer
              </button>
            </form>
          </div>

          <div v-if="ratings && ratings.length > 0">
            <div v-for="rating in ratings" :key="rating.ratingId" class="comment-card mb-3">
              <div class="card">
                <div class="card-header d-flex justify-content-between align-items-center">
                  <div>
                    <span class="fw-bold">{{ getUserName(rating.userId) || 'Utilisateur' }}</span>
                    <small class="text-muted ms-2">{{ formatDate(rating.createdAt) }}</small>
                  </div>
                  <div class="stars">
                    <i v-for="star in 5" 
                       :key="`rating-${rating.ratingId}-star-${star}`" 
                       :class="['bi', star <= rating.value ? 'bi-star-fill' : 'bi-star']"></i>
                  </div>
                </div>
                <div class="card-body">
                  <p class="card-text">{{ rating.comment || 'Aucun commentaire' }}</p>
                </div>
              </div>
            </div>
          </div>
          <div v-else class="text-muted">Aucun commentaire pour l'instant.</div>
        </div>
      </div>
      
      <div class="d-flex gap-2 mt-4">
        <router-link to="/" class="btn btn-secondary">Retour à l'accueil</router-link>
      </div>      <div v-if="currentUser && currentUser.userId === cocktail.userId" class="d-flex justify-content-end mt-3">
        <script>
          console.log('currentUser.userId:', currentUser?.userId);
          console.log('cocktail.userId:', cocktail?.userId);
        </script>
        <router-link 
          :to="{ name: 'EditCocktail', params: { id: cocktail.cocktailId } }" 
          class="btn btn-warning me-2"
        >
          Modifier
        </router-link>
        <button 
          class="btn btn-danger" 
          @click="confirmDelete"
        >
          Supprimer
        </button>
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
  name: 'CocktailDetails',
  data() {
    return {
      cocktailId: null,
      cocktail: null,
      creator: null,
      ingredients: [],
      ratings: [],
      users: {},
      loading: true,
      error: null,
      showRatingForm: false,
      newRating: {
        value: 0,
        comment: ''
      },
      submitting: false,
      hoverRating: 0
    };
  },
  computed: {
    ...mapGetters('auth', ['isAuthenticated', 'currentUser']),
    averageRating() {
      if (!this.ratings || this.ratings.length === 0) return 0;
      const sum = this.ratings.reduce((total, rating) => total + rating.value, 0);
      return (sum / this.ratings.length).toFixed(1);
    }
  },
  methods: {
    async fetchCocktailDetails() {
      this.loading = true;
      this.error = null;

      console.log('ID du cocktail récupéré:', this.cocktailId);

      try {
        const cocktailResponse = await CocktailService.getCocktail(this.cocktailId);
        this.cocktail = cocktailResponse.data;
        
        try {
          const ingredientsResponse = await ApiService.get(`/api/cocktailingredients/cocktail/${this.cocktailId}`);
          this.ingredients = ingredientsResponse.data;
        } catch (error) {
          console.error('Erreur lors de la récupération des ingrédients:', error);
        }
        
        const ratingsResponse = await RatingService.getRatingsByCocktailId(this.cocktailId);
        this.ratings = ratingsResponse.data;
        
        if (this.cocktail.userId) {
          try {
            const creatorResponse = await UserService.getUser(this.cocktail.userId);
            this.creator = creatorResponse.data;
          } catch (error) {
            console.error('Erreur lors de la récupération du créateur:', error);
          }
        }
        
        for (const rating of this.ratings) {
          if (rating.userId && !this.users[rating.userId]) {
            try {
              const userResponse = await UserService.getUser(rating.userId);
              this.users[rating.userId] = userResponse.data;
            } catch (error) {
              console.error(`Erreur lors de la récupération de l'utilisateur ${rating.userId}:`, error);
            }
          }
        }
      } catch (error) {
        console.error('Erreur lors de la récupération des détails du cocktail:', error);
        this.error = 'Impossible de charger les détails du cocktail. Veuillez réessayer.';
      } finally {
        this.loading = false;
      }
    },
    formatDate(dateString) {
      if (!dateString) return '';
      const date = new Date(dateString);
      return date.toLocaleDateString('fr-FR', { 
        year: 'numeric', 
        month: 'long', 
        day: 'numeric' 
      });
    },
    getUserName(userId) {
      return this.users[userId]?.firstName || 'Utilisateur';
    },
    async submitRating() {
      if (this.newRating.value === 0) {
        alert('Veuillez sélectionner une note.');
        return;
      }
      
      this.submitting = true;
      
      try {
       
        const userId = this.currentUser.userId || this.currentUser._id || this.currentUser.id;
        
        if (!userId) {
          console.error("Impossible de récupérer l'ID de l'utilisateur");
          alert("Une erreur est survenue. Veuillez vous reconnecter et réessayer.");
          this.submitting = false;
          return;
        }
        
        const rating = {
          userId: userId,
          cocktailId: this.cocktailId,
          value: this.newRating.value,  
          comment: this.newRating.comment,
          createdAt: new Date().toISOString(),
          updatedAt: new Date().toISOString()
        };
        
        console.log("Envoi de l'évaluation:", rating);
        
        await RatingService.createRating(rating);
        
       
        const ratingsResponse = await RatingService.getRatingsByCocktailId(this.cocktailId);
        this.ratings = ratingsResponse.data;
        

        this.newRating = { value: 0, comment: '' };
        this.showRatingForm = false;
        
      } catch (error) {
        console.error('Erreur lors de l\'ajout de l\'évaluation:', error);
        alert('Une erreur est survenue lors de l\'ajout de votre évaluation. Veuillez réessayer.');
      } finally {
        this.submitting = false;
      }
    },
    setRating(star) {
      this.newRating.value = star;
    },
    getImageUrl(imagePath) {
      if (!imagePath) return '';
      
      if (imagePath.startsWith('http://') || imagePath.startsWith('https://')) {
        return imagePath;
      }
      
      const cleanPath = imagePath.split(':')[0];
      
      const baseUrl = ApiService.getBaseUrl();
      
      return `${baseUrl}${cleanPath.startsWith('/') ? '' : '/'}${cleanPath}`;
    },
    confirmDelete() {
      if (confirm('Êtes-vous sûr de vouloir supprimer ce cocktail ?')) {
        this.deleteCocktail();
      }
    },
    async deleteCocktail() {
      try {
        await CocktailService.deleteCocktail(this.cocktailId);
        alert('Cocktail supprimé avec succès.');
        this.$router.push('/');
      } catch (error) {
        console.error('Erreur lors de la suppression du cocktail:', error);
        alert('Une erreur est survenue lors de la suppression du cocktail. Veuillez réessayer.');
      }
    }
  },
  created() {
    this.cocktailId = this.$route.params.id;
    if (this.cocktailId) {
      this.fetchCocktailDetails();
    } else {
      this.error = 'ID de cocktail non spécifié.';
      this.loading = false;
    }

    // Debugging currentUser and cocktail
    console.log('Utilisateur connecté:', this.currentUser);
    console.log('Cocktail:', this.cocktail);
  }
};
</script>

<style scoped lang="scss">
.cocktail-details {
  padding: 20px;
  
  .cocktail-image {
    height: 300px;
    width: 100%;
    
    img {
      width: 100%;
      height: 100%;
      object-fit: cover;
    }
  }
  
  .stars {
    color: #ffc107;
    font-size: 1.2rem;
    cursor: default;
    
    i {
      margin-right: 3px;
    }
  }
  
  .rating-input {
    font-size: 2rem;
    color: #ffc107;
    
    i {
      cursor: pointer;
      margin-right: 5px;
      
      &:hover {
        transform: scale(1.1);
      }
    }
  }
  
  .comment-card {
    .card-header {
      background-color: rgba(0, 0, 0, 0.03);
    }
  }
}
</style>