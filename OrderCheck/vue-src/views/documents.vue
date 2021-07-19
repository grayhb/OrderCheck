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
            <div v-else>
                <div class="d-flex mb-3 align-center">
                    <v-combobox :items="years" v-model="selectedYear" outlined dense hide-details :elevation="0" style="max-width:130px"></v-combobox>
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

                    <v-sparkline :labels="chartLabels"
                                 :value="chartValues"
                                 label-size="4"
                                 line-width="1"
                                 class="mt-2"
                                 padding="12"></v-sparkline>

            </div>
        </v-card-text>

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

            years: [],

            showCard: false,

            selectedItem: Object.assign({}, modelItem),
            selectedYear: null,

            amountPaid: 0,

            chartValues: [],
            chartLabels: []
        }),
        computed: {
            amountDebt() {
                let debt = 0;
                this.items.filter(e => !e.haveCheck).map(e => debt += e.debt);
                return debt.toLocaleString();
            },

            monthPanels() {

                this.amountPaid = 0;
                this.chartLabels = [];
                this.chartValues = [];

                let monthArray = [...new Set(this.items.map(e => {
                    let d = new Date(e.dateStart);
                    let y = d.getFullYear();

                    if (y === this.selectedYear)
                        return y + '-' + d.getMonth();
                }))];

                if (!monthArray[0])
                    return [];

                // сортируем от старых к новым
                monthArray.sort((a, b) => {
                    return a.localeCompare(b);
                });

                let monthIndex = 0;

                let panels = monthArray.map(e => {

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

                panels.forEach(e => {

                    let chartValue = 0;

                    this.getItems(e).forEach(d => {
                        if (d.paid && d.paid > 0) {
                            this.amountPaid += d.paid;
                            chartValue += d.paid;
                        }
                    });

                    this.chartLabels.push(e.name.split(' ')[0]);
                    this.chartValues.push(chartValue);
                });

                // добавляем пустые месяцы
                while (this.chartValues.length < 12) {
                    this.chartValues.push(0);
                    this.chartLabels.push(new Date(this.selectedYear, this.chartValues.length - 1, 1).toLocaleString('default', { month: 'long' }).toLocaleUpperCase());
                }

                // сортируем панели от новых к старым
                panels.sort((a, b) => {
                    return b.id - a.id;
                });

                return panels;
            },

            amountPaidLocale() {
                return this.amountPaid.toLocaleString();
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

                this.selectedYear = this.years[0];
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