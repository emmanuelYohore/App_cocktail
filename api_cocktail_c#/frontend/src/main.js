import { createApp } from 'vue';
import App from './App.vue';
import router from './router';
import store from './store';
import ApiService from './services/api.service';
import VueSweetalert2 from 'vue-sweetalert2';
import 'sweetalert2/dist/sweetalert2.min.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';


ApiService.init();
const app = createApp(App);

app.use(store)
   .use(router)
   .use(VueSweetalert2)
   .mount('#app');
   