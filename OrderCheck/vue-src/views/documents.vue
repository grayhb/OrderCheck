<template>
    <v-card>
        <v-card-title>

            <cardDocument :item="selectedItem"
                          :show="showCard"
                          :organizations="organizations"
                          :estates="estates"
                          v-on:update="loadItems"
                          @delete="deleteItem"
                          v-on:close="closeCard"></cardDocument>

            <span class="ml-4">Документы</span>
            <v-spacer></v-spacer>
            <strong v-if="!loadingData" class="red--text text--lighten-1">Долг: {{amountDebt}} ₽</strong>

        </v-card-title>

        <v-card-text>

            <v-progress-linear indeterminate v-if="loadingData"></v-progress-linear>
            <div v-else>
                <div class="d-flex mb-3 align-center">
                    <v-select :items="years" v-model="selectedYear" outlined dense hide-details :elevation="0" style="max-width:130px"></v-select>
                    <v-spacer></v-spacer>
                    <span class="mr-2">Расход за год</span>
                    <v-chip>{{amountPaidLocale}} ₽</v-chip>
                </div>
                <v-expansion-panels multiple hover>
                    <v-expansion-panel v-for="month in monthPanels" :key="month.id">
                        <v-expansion-panel-header>{{month.name}}</v-expansion-panel-header>
                        <v-expansion-panel-content>
                            <v-list flat>
                                <v-list-item-group>
                                    <v-list-item v-for="item in getItems(month)" @click="editItem(item)" :key="item.guid">
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
                                            <v-list-item-action-text class="red--text">Долг: {{item.debt.toLocaleString()}} ₽</v-list-item-action-text>
                                            <v-list-item-action-text class="green--text "><b>Оплачено: {{item.paid.toLocaleString()}} ₽</b></v-list-item-action-text>
                                        </v-list-item-action>
                                    </v-list-item>
                                </v-list-item-group>
                            </v-list>

                        </v-expansion-panel-content>
                    </v-expansion-panel>
                </v-expansion-panels>

                <span v-if="monthPanels.length === 0" class="d-flex justify-center my-3">Записи не найдены</span>


                <chart-line :data="chartData" class="mt-4"></chart-line>

            </div>
        </v-card-text>
    </v-card>
</template>

<script>
    import { doFetch } from '../helpers/fetch-helper.js';
    import cardDocument from '../components/documents/document-card';
    import chartLine from '../components/documents/chart-line';

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
        components: {
            chartLine,
            cardDocument,
        },
        created() {
            this.loadItems();
        },
        data: () => ({
            loadingData: false,

            items: [],
            organizations: [],
            estates: [],

            years: [],

            showCard: false,

            selectedItem: Object.assign({}, modelItem),
            selectedYear: null,

            amountPaid: 0,

            chartValues: [],
        }),
        computed: {
            amountDebt() {
                let debt = 0;
                this.items.filter(e => !e.haveCheck).map(e => debt += e.debt);
                return debt.toLocaleString();
            },

            currentMonths() {

                let monthIndexes = this.items
                    .filter(e => e.dateEnd.getFullYear() === this.selectedYear)
                    .map(e => Number(e.dateEnd.getMonth()));

                monthIndexes = [...new Set(monthIndexes)];

                monthIndexes.sort((a, b) => b - a);

                return monthIndexes;
            },
            monthPanels() {

                this.amountPaid = 0;
                this.chartValues = [];

                let panels = this.currentMonths.map(monthIndex => {

                    let d = new Date(this.selectedYear, monthIndex, 1);

                    let monthName = d.toLocaleString('default', { month: 'long' }).toLocaleUpperCase();

                    return {
                        id: monthIndex,
                        name: monthName,
                        month: d.getMonth(),
                        year: d.getFullYear()
                    };
                });

                panels.forEach(e => {

                    let chartValue = 0;

                    this.getItems(e).forEach(d => {
                        if (d.paid && d.paid > 0) {
                            this.amountPaid += d.paid;
                            chartValue += d.paid;
                        }
                    });

                    this.chartValues.unshift(chartValue);
                });


                // сортируем панели от новых к старым
                panels.sort((a, b) => {
                    return b.id - a.id;
                });

                return panels;
            },

            amountPaidLocale() {
                return this.amountPaid.toLocaleString();
            },

            chartData() {
                return [
                    {
                        label: 'Расход',
                        backgroundColor: '#349eeb',
                        data: this.chartValues
                    }
                ]
            }

        },
        methods: {

            async loadItems() {

                this.loadingData = true;

                await doFetch('/api/document', 'GET', this.$store, null, data => {

                    this.items = data.map(e => {
                        e.dateStart = new Date(e.dateStart);
                        e.dateEnd = new Date(e.dateEnd);
                        return e;
                    });

                    this.initYears();
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

            initYears() {

                this.years = [...new Set(this.items.map(e => {
                    let d = new Date(e.dateStart);
                    return d.getFullYear();
                }))];

                this.years.sort();

                this.selectedYear = this.years[this.years.length - 1];
            },

            getItems(m) {

                return this.items.filter(e => {
                    return e.dateStart.getFullYear() === m.year && e.dateStart.getMonth() === m.month;
                });

            },

            deleteItem() {

                try {
                    let itemIndex = this.items.findIndex(e => e.guid === this.selectedItem.guid);
                    this.items.splice(itemIndex, 1);
                }
                catch (ex) {
                    console.log(ex);
                }

            }

        },


    };
</script>

<style>
</style>