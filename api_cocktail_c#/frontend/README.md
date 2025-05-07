# frontend

## Project setup
```
npm install -g @vue/cli
npm install axios vue-sweetalert2 vuelidate bootstrap@5 @popperjs/core
npm install
```
frontend/
├── public/
│   ├── index.html
│   └── favicon.ico
├── src/
│   ├── assets/              # Images, styles, etc.
│   ├── components/          # Composants réutilisables
│   │   ├── common/          # Composants génériques (Header, Footer, etc.)
│   │   └── auth/            # Composants liés à l'authentification
│   ├── views/               # Pages principales
│   │   ├── Home.vue
│   │   ├── Login.vue
│   │   ├── Register.vue
│   │   └── Profile.vue
│   ├── services/            # Services API
│   │   ├── api.service.js   # Configuration Axios
│   │   ├── auth.service.js  # Service d'authentification
│   │   └── user.service.js  # Service utilisateur
│   ├── store/               # Vuex store
│   │   ├── index.js
│   │   └── modules/
│   │       └── auth.js      # Module d'authentification
│   ├── router/              # Vue Router
│   │   └── index.js         # Configuration des routes
│   ├── App.vue
│   └── main.js
└── package.json

### Compiles and hot-reloads for development
```
npm run serve
```

### Compiles and minifies for production
```
npm run build
```

### Lints and fixes files
```
npm run lint
```

### Customize configuration
See [Configuration Reference](https://cli.vuejs.org/config/).
