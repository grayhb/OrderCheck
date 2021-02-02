import ru from 'vuetify/src/locale/ru.ts';

import store from './store'
import layout from "./layouts/app-layout.vue";
import router from "./routers/app-router";

import { getToken } from './helpers/jwt-helpers';

if (!getToken()) {
    window.location.replace("/login");
}
else {
    new Vue({
        vuetify: new Vuetify({
            lang: {
                locales: { ru },
                current: 'ru',
            },
        }),
        router,
        store,
        render: h => h(layout)
    }).$mount("#app");
}