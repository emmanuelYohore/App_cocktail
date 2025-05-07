import { createRouter, createWebHistory } from 'vue-router';
import Home from '../views/Home.vue';
import Login from '../views/Login.vue';
import Register from '../views/Register.vue';
import Profile from '../views/Profile.vue';
import CocktailDetails from '../views/CocktailDetails.vue';
import CreateCocktail from '../views/CreateCocktail.vue';
import AddIngredients from '../views/AddIngredients.vue';
import AuthService from '../services/auth.service';

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/login',
    name: 'Login',
    component: Login,
    meta: { guestOnly: true }
  },
  {
    path: '/register',
    name: 'Register',
    component: Register,
    meta: { guestOnly: true }
  },
  {
    path: '/profile',
    name: 'Profile',
    component: Profile,
    meta: { requiresAuth: true }
  },
  {
    path: '/cocktails/:id',
    name: 'CocktailDetails',
    component: CocktailDetails
  },
  {
    path: '/create-cocktail',
    name: 'CreateCocktail',
    component: CreateCocktail,
    meta: { requiresAuth: true }
  },
  {
    path: '/cocktails/:id/add-ingredients',
    name: 'AddIngredients',
    component: AddIngredients,
    meta: { requiresAuth: true }
  }
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
});

router.beforeEach((to, from, next) => {
  const isAuthenticated = AuthService.isAuthenticated();
  
  if (to.meta.requiresAuth && !isAuthenticated) {
    next('/login');
  } else if (to.meta.guestOnly && isAuthenticated) {
    next('/profile');
  } else {
    next();
  }
});

export default router;
