<template>
  <div class="add-ingredients-container">
    <div v-if="loading" class="text-center my-5">
      <div class="spinner-border" role="status">
        <span class="visually-hidden">Chargement...</span>
      </div>
    </div>

    <div v-else-if="error" class="alert alert-danger">
      {{ error }}
    </div>

    <div v-else>
      <div class="card mb-4">
        <div class="card-header">
          <h2 class="mb-0">Ajouter des ingrédients à "{{ cocktailName }}"</h2>
        </div>
        <div class="card-body">
          <p>Veuillez ajouter au moins un ingrédient à votre cocktail.</p>

          <div v-if="ingredients.length > 0" class="mb-4">
            <h5>Ingrédients ajoutés:</h5>
            <ul class="list-group">
              <li v-for="(ingredient, index) in ingredients" :key="index" class="list-group-item d-flex justify-content-between align-items-center">
                <div>
                  <strong>{{ ingredient.name.join(', ') }}</strong>
                  <span class="badge bg-secondary ms-2">{{ ingredient.category }}</span>
                  <span class="ms-2">{{ ingredient.quantity }} {{ ingredient.unit }}</span>
                </div>
                <button @click="removeIngredient(index)" class="btn btn-danger btn-sm">
                  <i class="bi bi-trash"></i>
                </button>
              </li>
            </ul>
          </div>

          <form @submit.prevent="addIngredient" class="ingredient-form">
            <div class="row mb-3">
              <div class="col-md-6">
                <label for="ingredientName" class="form-label">Nom(s) de l'ingrédient*</label>
                <div v-for="(name, i) in newIngredient.name" :key="i" class="d-flex mb-2">
                  <input
                    type="text"
                    class="form-control"
                    v-model="newIngredient.name[i]"
                    :class="{ 'is-invalid': submitted && !isValidName(i) }"
                    placeholder="Ex: Rhum, Rum"
                  >
                  <button 
                    type="button" 
                    class="btn btn-outline-danger ms-2" 
                    @click="removeName(i)"
                    :disabled="newIngredient.name.length <= 1"
                  >
                    <i class="bi bi-dash"></i>
                  </button>
                </div>
                <div v-if="submitted && !isValidName()" class="text-danger">
                  Veuillez saisir au moins un nom d'ingrédient
                </div>
                <button 
                  type="button" 
                  class="btn btn-outline-primary btn-sm mt-2" 
                  @click="addName"
                >
                  <i class="bi bi-plus"></i> Ajouter un nom alternatif
                </button>
              </div>

              <div class="col-md-6">
                <label for="category" class="form-label">Catégorie*</label>
                <select 
                  class="form-select" 
                  id="category" 
                  v-model="newIngredient.category" 
                  required
                  :class="{ 'is-invalid': submitted && !newIngredient.category }"
                >
                  <option value="" disabled>Choisir une catégorie</option>
                  <option v-for="(cat, index) in categories" :key="index" :value="cat">{{ cat }}</option>
                </select>
                <div class="invalid-feedback">La catégorie est obligatoire</div>
              </div>
            </div>

            <div class="row mb-3">
              <div class="col-md-6">
                <label for="quantity" class="form-label">Quantité*</label>
                <input 
                  type="number" 
                  class="form-control" 
                  id="quantity" 
                  v-model.number="newIngredient.quantity" 
                  step="0.01"
                  min="0.01"
                  required
                  :class="{ 'is-invalid': submitted && !isValidQuantity() }"
                >
                <div class="invalid-feedback">La quantité doit être supérieure à 0</div>
              </div>

              <div class="col-md-6">
                <label for="unit" class="form-label">Unité*</label>
                <select 
                  class="form-select" 
                  id="unit" 
                  v-model="newIngredient.unit" 
                  required
                  :class="{ 'is-invalid': submitted && !newIngredient.unit }"
                >
                  <option value="" disabled>Choisir une unité</option>
                  <option value="ml">ml (millilitre)</option>
                  <option value="cl">cl (centilitre)</option>
                  <option value="goutte(s)">goutte(s)</option>
                  <option value="cuillère(s) à café">cuillère(s) à café</option>
                  <option value="cuillère(s) à soupe">cuillère(s) à soupe</option>
                  <option value="tranche(s)">tranche(s)</option>
                  <option value="pincée(s)">pincée(s)</option>
                  <option value="unité(s)">unité(s)</option>
                </select>
                <div class="invalid-feedback">L'unité est obligatoire</div>
              </div>
            </div>
            
            <div class="d-flex justify-content-between">
              <button 
                type="submit" 
                class="btn btn-primary" 
                :disabled="addingIngredient"
              >
                <span v-if="addingIngredient" class="spinner-border spinner-border-sm me-1" role="status" aria-hidden="true"></span>
                Ajouter cet ingrédient
              </button>
              
              <button 
                type="button" 
                class="btn btn-success" 
                :disabled="submitting || ingredients.length === 0" 
                @click="finishAddingIngredients"
              >
                <span v-if="submitting" class="spinner-border spinner-border-sm me-1" role="status" aria-hidden="true"></span>
                Terminer
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import CocktailService from '@/services/cocktail.service';
import IngredientService from '@/services/ingredient.service';
import ApiService from '@/services/api.service';

export default {
  name: 'AddIngredients',
  data() {
    return {
      cocktailId: null,
      cocktailName: '',
      ingredients: [],
      newIngredient: {
        name: [''],
        category: '',
        quantity: 1,
        unit: ''
      },
      categories: ['Spiritueux', 'Jus', 'Sirop', 'Autre'],
      loading: true,
      error: null,
      submitted: false,
      addingIngredient: false,
      submitting: false
    };
  },
  methods: {
    async fetchCocktailDetails() {
      this.loading = true;
      this.error = null;
      
      try {
        const response = await CocktailService.getCocktail(this.cocktailId);
        this.cocktailName = response.data.name;
      } catch (error) {
        console.error('Erreur lors de la récupération du cocktail:', error);
        this.error = 'Impossible de charger les détails du cocktail. Veuillez réessayer.';
      } finally {
        this.loading = false;
      }
    },
    isValidName(index = null) {
      if (index !== null) {
        return this.newIngredient.name[index]?.trim() !== '';
      }
      return this.newIngredient.name.some(name => name.trim() !== '');
    },
    isValidQuantity() {
      return this.newIngredient.quantity > 0;
    },
    addName() {
      this.newIngredient.name.push('');
    },
    removeName(index) {
      if (this.newIngredient.name.length > 1) {
        this.newIngredient.name.splice(index, 1);
      }
    },
    validateIngredientForm() {
      this.submitted = true;
      
      if (!this.isValidName() || 
          !this.newIngredient.category || 
          !this.isValidQuantity() || 
          !this.newIngredient.unit) {
        return false;
      }
      
      return true;
    },
    async addIngredient() {
      if (!this.validateIngredientForm()) {
        return;
      }
      
      this.addingIngredient = true;
      
      try {
        const cleanedNames = this.newIngredient.name.filter(name => name.trim() !== '');
        
        //nouvel ingrédient
        const ingredient = {
          name: cleanedNames,
          category: this.newIngredient.category,
          quantity: this.newIngredient.quantity,
          unit: this.newIngredient.unit
        };
        
        // Enregistrer l'ingrédient dans la base de données
        this.ingredients.push(ingredient);
        
        this.newIngredient = {
          name: [''],
          category: '',
          quantity: 1,
          unit: ''
        };
        this.submitted = false;
        
      } catch (error) {
        console.error('Erreur lors de l\'ajout de l\'ingrédient:', error);
        alert('Une erreur est survenue. Veuillez réessayer.');
      } finally {
        this.addingIngredient = false;
      }
    },
    removeIngredient(index) {
      this.ingredients.splice(index, 1);
    },
    async finishAddingIngredients() {
      if (this.ingredients.length === 0) {
        alert('Veuillez ajouter au moins un ingrédient.');
        return;
      }
      
      this.submitting = true;
      
      try {
        // Enregistrer chaque ingrédient dans la base de données
        for (const ingredient of this.ingredients) {
          ingredient.createdAt = new Date();
          ingredient.updatedAt = new Date();
          
          const response = await IngredientService.createIngredient(ingredient);
          const ingredientId = response.data.ingredientId;
          
          const cocktailIngredient = {
            cocktailId: this.cocktailId,
            ingredientId: ingredientId,
            createdAt: new Date(),
            updatedAt: new Date()
          };
          
          await ApiService.post('/api/cocktailingredients', cocktailIngredient);
        }
        
        this.$router.push({ path: `/cocktails/${this.cocktailId}` });
      } catch (error) {
        console.error('Erreur lors de l\'enregistrement des ingrédients:', error);
        this.error = 'Une erreur est survenue lors de l\'enregistrement des ingrédients. Veuillez réessayer.';
      } finally {
        this.submitting = false;
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
  }
};
</script>

<style scoped lang="scss">
.add-ingredients-container {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
}
</style>