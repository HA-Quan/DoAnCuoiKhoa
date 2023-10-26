import { createApp } from 'vue'
import App from './App.vue'
import axios from 'axios';
import ElementPlus from 'element-plus'
import ElDatePicker from "element-plus";
import vi from "element-plus/es/locale/lang/vi";
import 'element-plus/dist/index.css'
import mitt from 'mitt';
import router from './router/mainRouter'
import CommonFn from './utils/commonFuncion';

import store from './utils/vuex';

const app = createApp(App);

const emitter = mitt();
app.config.globalProperties.emitter = emitter;

axios.defaults.baseURL = 'https://localhost:7101/api/';

axios.defaults.headers.common["Authorization"] = "Bearer " + CommonFn.getCookie('Token');

// Sử dụng tiếng Việt
app.use(ElDatePicker, {
  locale: vi,
});
app.use(store); 
app.use(ElementPlus);
app.use(router).mount("#app");
