<template>
  <div class="create-cocktail-container">
    <h2 class="text-center mb-4">Création d'un cocktail</h2>

    <div class="card">
      <div class="card-body">
        <form @submit.prevent="createCocktail">
          <div class="mb-3">
            <label for="name" class="form-label">Nom du cocktail*</label>
            <input 
              type="text" 
              class="form-control" 
              id="name" 
              v-model="cocktail.name" 
              required
              :class="{ 'is-invalid': submitted && !cocktail.name }"
            >
            <div class="invalid-feedback">Le nom du cocktail est obligatoire</div>
          </div>

          <div class="mb-3">
            <label for="description" class="form-label">Description*</label>
            <textarea 
              class="form-control" 
              id="description" 
              v-model="cocktail.description" 
              rows="3"
            ></textarea>
          </div>

          <div class="mb-3">
            <label for="cocktailImage" class="form-label">Image du cocktail</label>
            <input 
              type="file" 
              class="form-control" 
              id="cocktailImage" 
              @change="handleImageUpload"
              accept="image/*"
            >
            <div class="form-text">Sélectionnez une image pour illustrer votre cocktail</div>
            
            <div v-if="imagePreview" class="mt-3">
              <p>Aperçu :</p>
              <img :src="imagePreview" alt="Aperçu du cocktail" class="img-thumbnail" style="max-height: 200px;">
            </div>
          </div>

          <div v-if="error" class="alert alert-danger">
            {{ error }}
          </div>

          <div class="d-grid gap-2 d-md-flex justify-content-md-end">
            <router-link to="/" class="btn btn-secondary me-md-2">Annuler</router-link>
            <button 
              type="submit" 
              class="btn btn-primary" 
              :disabled="submitting"
            >
              <span v-if="submitting" class="spinner-border spinner-border-sm me-1" role="status" aria-hidden="true"></span>
              Créer et ajouter des ingrédients
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex';
import CocktailService from '@/services/cocktail.service';

export default {
  name: 'CreateCocktail',
  data() {
    return {
      cocktail: {
        name: '',
        description: ''
      },
      selectedImage: null,
      imagePreview: null,
      submitting: false,
      submitted: false,
      error: null
    };
  },
  computed: {
    ...mapGetters('auth', ['currentUser'])
  },
  methods: {
    handleImageUpload(event) {
      const file = event.target.files[0];
      if (!file) {
        this.selectedImage = null;
        this.imagePreview = null;
        return;
      }

      if (!file.type.match('image.*')) {
        this.error = 'Veuillez sélectionner une image valide.';
        this.selectedImage = null;
        this.imagePreview = null;
        return;
      }

      const maxSize = 5 * 1024 * 1024; // 5MB
      if (file.size > maxSize) {
        this.error = 'L\'image est trop volumineuse. Taille maximale: 5MB.';
        this.selectedImage = null;
        this.imagePreview = null;
        return;
      }

      this.selectedImage = file;
      this.error = null;
      
      const reader = new FileReader();
      reader.onload = e => {
        this.imagePreview = e.target.result;
      };
      reader.readAsDataURL(file);
    },
    
    async createCocktail() {
      this.submitted = true;
      
      if (!this.cocktail.name) {
        return;
      }
      
      this.submitting = true;
      this.error = null;
      
      try {
        const cocktailData = {
          Name: this.cocktail.name,
          Description: this.cocktail.description,
          UserId: this.currentUser.userId || this.currentUser._id || this.currentUser.id
        };
        
        const response = await CocktailService.createCocktail(cocktailData, this.selectedImage);
        

        this.$router.push({ 
          name: 'AddIngredients', 
          params: { id: response.data.cocktailId } 
        });
      } catch (error) {
        console.error('Erreur lors de la création du cocktail:', error);
        this.error = 'Une erreur est survenue lors de la création du cocktail. Veuillez réessayer.';
      } finally {
        this.submitting = false;
      }
    }
  }
};
</script>

<style scoped lang="scss">
.create-cocktail-container {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
}
</style>