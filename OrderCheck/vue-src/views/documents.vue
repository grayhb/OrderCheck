<template>
    <v-card>
        <v-card-title>

            <cardDocument :item="selectedItem"
                          :show="showCard"
                          :organizations="organizations"
                          :estates="estates"
                          v-on:update="loadItems"
                          v-on:close="closeCard"></cardDocument>

            <span class="ml-4">Документы</span>
            <v-spacer></v-spacer>
            <strong v-if="!loadingData" class="red--text text--lighten-1">Долг: {{amountDebt}} ₽</strong>

        </v-card-title>

        <v-card-text>

            <v-progress-linear indeterminate v-if="loadingData"></v-progress-linear>

            <v-expansion-panels multiple v-else>
                <v-expansion-panel v-for="month in months" :key="month.id">
                    <v-expansion-panel-header>{{month.name}}</v-expansion-panel-header>
                    <v-expansion-panel-content>
                        <v-list flat>
                            <v-list-item-group>
                                <v-list-item v-for="item in getItems(month)" @click="editItem(item)">
                                    <v-list-item-content>
                                        <v-list-item-title>
                                            {{ item.organization.organizationName }}
                                        </v-list-item-title>
                                        <v-list-item-subtitle>
                                            <v-chip v-if="item.haveCheck"
                                                    color="success"
                                                    small>
                                                оплачено
                                            </v-chip>
                                            <v-chip v-else
                                                    color="error"
                                                    small>
                                                не оплачено
                                            </v-chip>
                                        </v-list-item-subtitle>
                                    </v-list-item-content>
                                    <v-list-item-action>
                                        <v-list-item-action-text><b>Долг: {{item.debt.toLocaleString()}} ₽</b></v-list-item-action-text>
                                        <v-list-item-action-text>Оплачено: {{item.paid.toLocaleString()}} ₽</v-list-item-action-text>
                                    </v-list-item-action>
                                </v-list-item>
                            </v-list-item-group>
                        </v-list>

                    </v-expansion-panel-content>
                </v-expansion-panel>
            </v-expansion-panels>
        </v-card-text>

        <!--<v-card-text>
            <v-data-table :headers="headers"
                          :items="items"
                          dense
                          @click:row="editItem"
                          :loading="loadingData"
                          item-class="cursor-pointer">
            </v-data-table>
        </v-card-text>-->

    </v-card>
</template>

<script>
    import { doFetch } from '../helpers/fetch-helper.js';
    import cardDocument from '../components/documents/document-card';

    var modelItem = {
        guid: '',
        docFile: null,
        checkFile: null,
        dateStart: null,
        dateEnd: null,
        note: '',
        organization: null,
        estate: null,
        organizationId: 0,
        estateId: 0,
        paid: 0,
        debt: 0,
    };

    export default {
        created: function () {
            this.loadItems();
        },
        data: () => ({
            loadingData: false,

            items: [],
            organizations: [],
            estates: [],

            months: [],

            headers: [
                { text: 'Дата', value: 'dateStartFormated' },
                { text: 'Организация', value: 'organization.organizationName' },
                { text: 'Чек', value: 'haveCheck' },
            ],

            showCard: false,

            selectedItem: Object.assign({}, modelItem),

        }),
        computed: {
            amountDebt() {
                let debt = 0;
                this.items.filter(e => !e.haveCheck).map(e => debt += e.debt);
                return debt.toLocaleString();
            }
        },
        methods: {
            async loadItems() {

                this.loadingData = true;

                await doFetch('/api/document', 'GET', this.$store, null, data => {

                    this.items = data.map(e => {

                        e.dateStartFormated = new Date(e.dateStart).toLocaleDateString('ru-RU');
                        e.dateEndFormated = new Date(e.dateEnd).toLocaleDateString('ru-RU');
                        //e.haveCheck = e.haveCheck ? 'есть' : 'нет';
                        return e;
                    });

                    this.initMonths();

                });

                await doFetch('/api/organization', 'GET', this.$store, null, data => {
                    this.organizations = data;

                    if (data.length === 1)
                        modelItem.organizationId = data[0].organizationId;
                });

                await doFetch('/api/estates', 'GET', this.$store, null, data => {
                    this.estates = data;

                    if (data.length === 1)
                        modelItem.estateId = data[0].estateId;
                });

                this.selectedItem = Object.assign({}, modelItem);

                this.loadingData = false;

            },
            closeCard() {

                this.showCard = false;

                this.selectedItem = Object.assign({}, modelItem);
            },
            editItem(e) {

                this.selectedItem = Object.assign({}, e);

                this.selectedItem.organization = this.organizations.find(org => org.organizationId == e.organizationId);
                this.selectedItem.estate = this.estates.find(estate => estate.estateId == e.estateId);

                this.showCard = true;
            },
            initMonths() {


                let monthArray = [...new Set(this.items.map(e => {
                    let d = new Date(e.dateStart);
                    return d.getFullYear() + '-' + d.getMonth();
                }))];

                let monthIndex = 0;

                this.months = monthArray.map(e => {

                    let splitedDate = e.split('-');

                    let d = new Date(splitedDate[0], splitedDate[1], 1);
                    let monthName = d.toLocaleString('default', { month: 'long' }).toLocaleUpperCase();

                    return {
                        id: monthIndex++,
                        name: monthName + ' ' + splitedDate[0],
                        month: d.getMonth(),
                        year: d.getFullYear()
                    };
                });


            },

            getItems(m) {

                return this.items.filter(e => {
                    let d = new Date(e.dateStart);
                    return d.getFullYear() === m.year && d.getMonth() === m.month;
                });

            }

        },
        components: {
            cardDocument
        },

    };
</script>

<style>
</style>