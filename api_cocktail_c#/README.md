# App_cocktail

## Documentation de l'API

Base URL : `http://localhost:5000/api/`

### Authentification

- **POST** `/auth/login` : Connexion utilisateur (body : email, password)
- **POST** `/auth/logout` : Déconnexion utilisateur (body : token)

### Utilisateurs

- **GET** `/user` : Liste tous les utilisateurs
- **GET** `/user/{id}` : Détail d'un utilisateur
- **POST** `/user` : Création d'un utilisateur (body : firstName, lastName, email, password)
- **PUT** `/user/{id}` : Modification d'un utilisateur (body : firstName, lastName, email, [password])
- **DELETE** `/user/{id}` : Suppression d'un utilisateur

### Cocktails

- **GET** `/cocktails` : Liste tous les cocktails
- **GET** `/cocktails/{id}` : Détail d'un cocktail
- **POST** `/cocktails` : Création d'un cocktail (body : name, description, userId, [image])
- **PUT** `/cocktails/{id}` : Modification d'un cocktail (body : name, description, [image])
- **DELETE** `/cocktails/{id}` : Suppression d'un cocktail

### Ingrédients

- **GET** `/ingredients` : Liste tous les ingrédients
- **GET** `/ingredients/{id}` : Détail d'un ingrédient
- **POST** `/ingredients` : Création d'un ingrédient (body : name[], category, quantity, unit)
- **PUT** `/ingredients/{id}` : Modification d'un ingrédient (body : name[], category, quantity, unit)
- **DELETE** `/ingredients/{id}` : Suppression d'un ingrédient

### CocktailIngredients (association cocktail/ingrédient)

- **GET** `/cocktailingredients` : Liste toutes les associations
- **GET** `/cocktailingredients/cocktail/{cocktailId}` : Liste les ingrédients d'un cocktail
- **GET** `/cocktailingredients/{id}` : Détail d'une association
- **POST** `/cocktailingredients` : Ajout d'un ingrédient à un cocktail (body : cocktailId, ingredientId)
- **PUT** `/cocktailingredients/{id}` : Modification d'une association
- **DELETE** `/cocktailingredients/{id}` : Suppression d'une association

### Notes (Ratings)

- **GET** `/ratings` : Liste toutes les notes (filtrage possible par cocktailId)
- **GET** `/ratings/{id}` : Détail d'une note
- **POST** `/ratings` : Création d'une note (body : userId, cocktailId, value, comment)
- **PUT** `/ratings/{id}` : Modification d'une note
- **DELETE** `/ratings/{id}` : Suppression d'une note

# frontend

## Project setup
```
npm install -g @vue/cli
npm install

npm run serve
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

