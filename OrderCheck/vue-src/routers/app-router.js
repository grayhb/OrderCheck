import Router from "vue-router";
import notFound from '../not-found.vue';
import mainView from '../views/main.vue';
import profile from '../views/profile.vue';
import documents from '../views/documents.vue';
import organizations from '../views/organizations.vue';
import estates from '../views/estates.vue';

Vue.use(Router);

export default new Router({
    routes: [
        {
            path: "/",
            component: documents,
        },
        {  
            path: "*",
            component: notFound,
        },
        { 
            path: "/not-found", 
            name: "not-found",
            component: notFound,
        },
        {
            path: "/profile",
            name: "profile",
            component: profile,
        },
        {
            path: "/documents",
            name: "documents",
            component: documents,
        },
        {
            path: "/organizations",
            name: "organizations",
            component: organizations,
        },
        {
            path: "/estates",
            name: "estates",
            component: estates,  
        },
    ]
});