<template>

    <v-card>
        <v-card-title>
            <cardEstate :item="selectedItem" :show="showCard" v-on:update="loadItems" v-on:close="closeCard"></cardEstate>
            <span class="ml-4">Объекты</span>
        </v-card-title>

        <v-card-text>
            <v-data-table :headers="headers"
                          :items="items"
                          dense
                          @click:row="editItem"
                          :loading="loadingData"
                          item-class="cursor-pointer"
                          >
            </v-data-table>
        </v-card-text>

    </v-card>

</template>

<script>
    import { doFetch } from '../helpers/fetch-helper.js';
    import cardEstate from '../components/estates/estate-card';

    export default {
        created: function () {
            this.loadItems();
        },
        data: () => ({
            loadingData: false,

            items: [],

            headers: [
                { text: 'Наименование', value: 'estateName' },
                { text: 'Адрес', value: 'estateAddress' },
            ],

            showCard: false,

            selectedItem: {
                estateId: 0,
                estateName: '',
                estateAddress: '',
            },

        }),
        methods: {
            async loadItems() {

                this.loadingData = true;

                await doFetch('/api/estates', 'GET', this.$store, null, data => {
                    this.items = data;
                    this.loadingData = false;
                });
            },
            closeCard() {

                this.showCard = false;

                this.selectedItem = {
                    estateId: 0,
                    estateName: '',
                    estateAddress: '',
                };
            },
            editItem(e) {

                this.selectedItem = {
                    estateId: e.estateId,
                    estateName: e.estateName,
                    estateAddress: e.estateAddress,
                };

                this.showCard = true;
            }

        },
        components: {
            cardEstate
        }
    };
</script>

<style>
</style>