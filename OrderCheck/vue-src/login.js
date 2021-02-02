import layout from "./layouts/login.vue";

new Vue({
    vuetify: new Vuetify(),
    render: h => h(layout)
}).$mount("#app");

