<template>

    <v-card>
        <v-card-title>
            <cardOrganization :item="selectedItem" :show="showCard" v-on:update="loadItems" v-on:close="closeCard"></cardOrganization>
            <span class="ml-4">{{title}}</span>
        </v-card-title>

        <v-card-text>
            <v-data-table :headers="headers"
                          :items="items"
                          dense
                          @click:row="editItem"
                          item-class="cursor-pointer"
                          :loading="loadingData">
            </v-data-table>
        </v-card-text>

    </v-card>

</template>

<script>
    import { doFetch } from '../helpers/fetch-helper.js';
    import cardOrganization from '../components/organizations/organization-card';

    var organizationModel = {
        organizationId: 0,
        organizationName: '',
        inn: ''
    };

    export default {
        created: function () {
            this.loadItems();
        },
        data: () => ({
            title: 'Источники',
            items: [],
            headers: [
                { text: 'Наименование', value: 'organizationName' },
                { text: 'ИНН', value: 'inn' },
            ],
            loadingData: false,

            selectedItem: Object.assign({}, organizationModel),
            showCard: false,

        }),
        methods: {

            async loadItems() {
                await doFetch('/api/organization', 'GET', this.$store, null, data => {
                    this.items = data;
                    this.loadingData = false;
                });
            },

            editItem(e) {
                this.selectedItem = JSON.parse(JSON.stringify(e));
                this.showCard = true;
            },

            closeCard() {

                this.showCard = false;

                this.selectedItem = Object.assign({}, organizationModel);
            },
        },
        components: {
            cardOrganization
        }
    };
</script>

<style>
</style>