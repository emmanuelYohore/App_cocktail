<template>
  <div class="edit-cocktail-container">
    <h2 class="text-center mb-4">Modifier le cocktail</h2>

    <div class="card">
      <div class="card-body">
        <form @submit.prevent="updateCocktail">
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
            <label for="cocktailImage" class="form-label">Image du cocktail*</label>
            <input 
              type="file" 
              class="form-control" 
              id="cocktailImage" 
              @change="handleImageUpload"
              accept="image/*"
            >
            <div class="form-text">Sélectionnez une nouvelle image pour remplacer l'ancienne</div>

         
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
              Enregistrer les modifications
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import CocktailService from '@/services/cocktail.service';

export default {
  name: 'EditCocktail',
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
  methods: {
    async fetchCocktailDetails() {
      try {
        const response = await CocktailService.getCocktail(this.$route.params.id);
        this.cocktail = response.data;
        if (this.cocktail.cocktailImage) {
          this.imagePreview = this.cocktail.cocktailImage;
        }
      } catch (error) {
        console.error('Erreur lors de la récupération des détails du cocktail:', error);
        this.error = 'Impossible de charger les détails du cocktail. Veuillez réessayer.';
      }
    },
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
        this.error = "L'image est trop volumineuse. Taille maximale: 5MB.";
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
    async updateCocktail() {
      this.submitted = true;

      if (!this.cocktail.name) {
        return;
      }

      this.submitting = true;
      this.error = null;

      try {
        const cocktailData = {
          Name: this.cocktail.name,
          Description: this.cocktail.description
        };

        await CocktailService.updateCocktail(this.$route.params.id, cocktailData, this.selectedImage);
        this.$router.push({ name: 'CocktailDetails', params: { id: this.$route.params.id } });
      } catch (error) {
        console.error('Erreur lors de la mise à jour du cocktail:', error);
        this.error = 'Une erreur est survenue lors de la mise à jour du cocktail. Veuillez réessayer.';
      } finally {
        this.submitting = false;
      }
    }
  },
  created() {
    this.fetchCocktailDetails();
  }
};
</script>

<style scoped lang="scss">
.edit-cocktail-container {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
}
</style>
