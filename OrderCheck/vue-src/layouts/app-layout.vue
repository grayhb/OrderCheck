<template>
    <v-app id="inspire">

        <v-app-bar app absolute
                   elevate-on-scroll>

            <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>

            <v-toolbar-title>Документовед</v-toolbar-title>

            <v-spacer></v-spacer>

        </v-app-bar>

        <v-navigation-drawer v-model="drawer"
                             fixed
                             temporary>

            <v-list>
                <v-list-item v-for="[icon, text, to] in links"
                             :key="icon"
                             :to="to"
                             link>
                    <v-list-item-icon>
                        <v-icon>{{ icon }}</v-icon>
                    </v-list-item-icon>

                    <v-list-item-content>
                        <v-list-item-title>{{ text }}</v-list-item-title>
                    </v-list-item-content>
                </v-list-item>
            </v-list>

            <v-seporator></v-seporator>

            <v-list>
                <v-list-item link to="profile">
                    <v-list-item-icon>
                        <v-icon></v-icon>
                    </v-list-item-icon>

                    <v-list-item-content>
                        <v-list-item-title>Профиль</v-list-item-title>
                    </v-list-item-content>
                </v-list-item>

                <v-list-item link @click="logout">
                    <v-list-item-icon>
                        <v-icon></v-icon>
                    </v-list-item-icon>

                    <v-list-item-content>
                        <v-list-item-title>Выход</v-list-item-title>
                    </v-list-item-content>
                </v-list-item>

            </v-list>


        </v-navigation-drawer>

        <v-main>
            <v-container>
                <v-row>
                    <v-col>
                        <router-view></router-view>

                        <v-snackbar v-model="showError"
                                    timeout="15000"
                                    vertical>

                            {{ errorText }}

                            <template v-slot:action="{ attrs }">
                                <v-btn color="red"
                                       text
                                       v-bind="attrs"
                                       @click="closeError">
                                    Close
                                </v-btn>
                            </template>
                        </v-snackbar>

                    </v-col>
                </v-row>
            </v-container>
        </v-main>

        <v-footer app>

        </v-footer>

    </v-app>
</template>

<script>

    import { getToken, deleteToken } from '../helpers/jwt-helpers';

    export default {
        created: function () {

            this.token = getToken();
            this.checkToken();

            this.$vuetify.lang.current = 'ru';
        },
        computed: {
            routes() {
                return this.$router.options.routes;
            },
            showError() {
                return this.$store.state.showError;
            },
            errorText() {
                return this.$store.state.error;
            }
        },
        data: () => ({
            title: 'Управление задачами',
            // потом исправить
            isAdmin: true,
            drawer: null,
            token: null,
            links: [
                ['mdi-safe-square', 'Документы', 'documents'],
                ['mdi-mail', 'Источники', 'organizations'],
                ['mdi-palette-swatch', 'Объекты', 'estates'],
            ]
        }),
        methods: {

            checkToken() {
                if (!this.token) {
                    window.location.replace("/login");
                }
                else {
                    this.$store.commit('setToken', this.token);
                }
            },

            logout() {
                deleteToken();
                window.location.replace("/login");
            },

            closeError() {
                this.$store.commit('setShowError', false);
            }
        }
    }
</script>

<style scoped>
    .content {
        padding: 10px;
    }
</style>