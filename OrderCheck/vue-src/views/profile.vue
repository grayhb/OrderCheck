<template>

    <v-card>

        <v-card-title>
            Профиль
        </v-card-title>

        <v-card-text>

            <v-text-field v-model="profile.email"
                          :loading="loadingData"
                          label="Email"
                          disabled></v-text-field>

            <v-text-field v-model="profile.displayName"
                          :loading="loadingData"
                          label="Ваше имя"
                          ></v-text-field>

        </v-card-text>

        <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn @click="save"
                   color="info"
                   :loading="loadingSave"
                   :disabled="loadingSave">Сохранить</v-btn>
        </v-card-actions>

    </v-card>

</template>

<script>
    import { doFetch } from '../helpers/fetch-helper.js';

    export default {
        created: function () {
            this.load();
        },
        data: () => ({
            loadingData: true,
            loadingSave: false,
            profile: {
                displayName: '',
                email: '',
            }
        }),
        methods: {
            async load() {
                await doFetch('/api/profile', 'POST', this.$store, null, profile => {
                    this.profile = profile;
                    this.loadingData = false;
                });
            },
            async save() {

                if (!this.profile.displayName)
                    return;

                this.loadingSave = true;

                var formData = new FormData();
                formData.append('DisplayName', this.profile.displayName);

                await doFetch('/api/profile', 'PUT', this.$store, formData, data => {
                    this.loadingSave = false;
                });

            }
        }
    };
</script>

<style>
</style>