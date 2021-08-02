<template>

    <v-dialog v-model="show" max-width="600px" persistent>

        <template v-slot:activator="{ on, attrs }">
            <v-btn color="primary"
                   dark
                   fab
                   class=""
                   small
                   depressed
                   v-bind="attrs"
                   v-on="on">
                <v-icon>
                    mdi-plus
                </v-icon>
            </v-btn>
        </template>

        <v-card>
            <v-card-title>
                <v-spacer></v-spacer>
                <v-btn fab
                       x-small
                       depressed
                       @click="close">
                    <v-icon>
                        mdi-close
                    </v-icon>
                </v-btn>
            </v-card-title>

            <v-card-text class="card-body">
                <v-container>
                    <v-row>

                        <v-col cols="4">
                            <v-avatar color="grey"
                                      size="50"
                                      @click="overlayDoc = true"
                                      tile>
                                <v-img :src="urlDocThumbImage"></v-img>
                            </v-avatar>
                        </v-col>

                        <v-col cols="8">
                            <v-file-input v-model="item.docFile"
                                          accept="image/*"
                                          @change="getQrInfo"
                                          label="Файл документа"></v-file-input>
                        </v-col>

                        <v-col cols="4">
                            <v-avatar color="grey"
                                      size="50"
                                      @click="overlayCheck = true"
                                      tile>
                                <v-img :src="urlCheckThumbImage"></v-img>
                            </v-avatar>
                        </v-col>

                        <v-col cols="8">
                            <v-file-input v-model="item.checkFile"
                                          accept="image/*"
                                          label="Файл чека"></v-file-input>
                        </v-col>

                        <v-col cols="12">
                            <v-select v-model="item.estateId"
                                      :items="estates"
                                      item-value="estateId"
                                      item-text="estateName"
                                      dense
                                      label="Объект"></v-select>
                        </v-col>
                        <v-col cols="12">
                            <v-select v-model="item.organizationId"
                                      :items="organizations"
                                      item-value="organizationId"
                                      item-text="organizationName"
                                      dense
                                      label="Организация"></v-select>

                        </v-col>

                        <v-col cols="6">
                            <v-text-field v-model="item.debt"
                                          label="Долг"
                                          value="0.00"
                                          suffix="руб."></v-text-field>
                        </v-col>

                        <v-col cols="6">
                            <div class="d-flex align-center">
                                
                                <v-btn x-small fab depressed class="mr-2" @click="copyDebtToPaid">
                                    <v-icon>
                                        mdi-arrow-right-bold
                                    </v-icon>
                                </v-btn>

                                <v-text-field v-model="item.paid"
                                              label="Оплачено"
                                              value="0.00"
                                              suffix="руб."></v-text-field>
                            </div>
                        </v-col>

                        <v-col cols="6">
                            <v-menu v-model="menuDateStart"
                                    :close-on-content-click="false"
                                    :nudge-right="40"
                                    transition="scale-transition"
                                    offset-y
                                    min-width="auto">
                                <template v-slot:activator="{ on, attrs }">
                                    <v-text-field v-model="dateStartISO"
                                                  label="Начало"
                                                  prepend-icon="mdi-calendar"
                                                  readonly
                                                  clearable
                                                  v-bind="attrs"
                                                  v-on="on"
                                                  @click:clear="item.dateStart = null"></v-text-field>
                                </template>
                                <v-date-picker no-title
                                               v-model="item.dateStart"
                                               @change="onDateStartChange"
                                               @input="menuDateStart = false"></v-date-picker>
                            </v-menu>
                        </v-col>
                        <v-col cols="6">
                            <v-menu v-model="menuDateEnd"
                                    :close-on-content-click="false"
                                    :nudge-right="40"
                                    transition="scale-transition"
                                    offset-y
                                    min-width="auto">
                                <template v-slot:activator="{ on, attrs }">
                                    <v-text-field v-model="dateEndISO"
                                                  label="Окончание"
                                                  prepend-icon="mdi-calendar"
                                                  readonly
                                                  clearable
                                                  v-bind="attrs"
                                                  v-on="on"
                                                  @click:clear="item.dateEnd = null"></v-text-field>
                                </template>
                                <v-date-picker no-title
                                               v-model="item.dateEnd"
                                               @input="menuDateEnd = false"></v-date-picker>
                            </v-menu>
                        </v-col>
                        <v-col cols="12">
                            <v-textarea v-model="item.note"
                                        rows="2"
                                        label="Примечание"></v-textarea>
                        </v-col>
                    </v-row>
                </v-container>
            </v-card-text>

            <v-card-actions>
                <v-btn color="red" text v-if="item.estateId !== 0" @click="deleteItem">Удалить</v-btn>
                <v-spacer></v-spacer>
                <v-btn color="primary"
                       text
                       :loading="loadingSave"
                       @click="saveItem">Сохранить</v-btn>

            </v-card-actions>
        </v-card>

        <v-overlay :value="overlayDoc">

            <v-col cols="12">
                <v-img :src="urlDocImage" height="80vh" width="95vw" contain></v-img>
            </v-col>

            <v-col cols="12" class="text-center">
                <v-btn color="primary"
                       @click="overlayDoc = false">
                    Закрыть
                </v-btn>
            </v-col>

        </v-overlay>

        <v-overlay :value="overlayCheck">

            <v-col cols="12">
                <v-img :src="urlCheckImage" height="80vh" width="95vw" contain></v-img>
            </v-col>

            <v-col cols="12" class="text-center">
                <v-btn color="primary"
                       @click="overlayCheck = false">
                    Закрыть
                </v-btn>
            </v-col>

        </v-overlay>

    </v-dialog>

</template>

<script>
    import { doFetch } from '../../helpers/fetch-helper.js';

    export default {
        props: ['item', 'show', 'organizations', 'estates'],
        computed: {
            urlDocThumbImage() {
                if (this.item.guid === '')
                    return '';

                return '/api/document/' + this.item.guid + '?type=doc&size=thumb'
            },
            urlCheckThumbImage() {
                if (this.item.guid === '')
                    return '';

                return '/api/document/' + this.item.guid + '?type=check&size=thumb'
            },
            urlDocImage() {
                if (this.item.guid === '')
                    return '';

                return '/api/document/' + this.item.guid + '?type=doc&size=original'
            },
            urlCheckImage() {
                if (this.item.guid === '')
                    return '';

                return '/api/document/' + this.item.guid + '?type=check&size=original'
            },
            dateStartISO() {
                if (this.item.dateStart)
                    return new Date(this.item.dateStart).toLocaleDateString('ru-RU');
                else
                    return '';
            },
            dateEndISO() {
                if (this.item.dateEnd)
                    return new Date(this.item.dateEnd).toLocaleDateString('ru-RU');
                else
                    return '';
            }
        },
        data: () => ({
            loadingSave: false,
            menuDateStart: false,
            menuDateEnd: false,
            overlayDoc: false,
            overlayCheck: false,
        }),
        methods: {
            close() {
                this.$emit('close')
            },
            async saveItem() {

                let formData = new FormData();
                formData.append('estateId', this.item.estateId);
                formData.append('organizationId', this.item.organizationId);
                formData.append('note', this.item.note);

                formData.append('docFile', this.item.docFile);
                formData.append('checkFile', this.item.checkFile);

                formData.append('paid', this.item.paid);
                formData.append('debt', this.item.debt);

                formData.append('dateStart', new Date(this.item.dateStart).toISOString());
                formData.append('dateEnd', new Date(this.item.dateEnd).toISOString());

                let method = 'POST';

                if (this.item.guid !== '') {
                    method = 'PUT';
                    formData.append('guid', this.item.guid)
                }

                this.loadingSave = true;

                await doFetch('/api/document', method, this.$store, formData, data => {
                    this.item = data;
                    this.$emit('update')
                    this.close();
                });

                this.loadingSave = false;

            },
            async deleteItem() {
                let formData = new FormData();
                formData.append('guid', this.item.guid);

                await doFetch('/api/document', 'DELETE', this.$store, formData, data => {
                    this.$emit('delete');
                    this.close();
                });
            },
            onDateStartChange(e) {
                let splitDate = e.split('-');
                let splitDateEnd = new Date(splitDate[0], splitDate[1], 0).toLocaleDateString('ru-RU').split('.');

                this.item.dateEnd = splitDateEnd[2] + '-' + splitDateEnd[1] + '-' + splitDateEnd[0];
            },

            async getQrInfo() {
                if (!this.item.docFile)
                    return;

                let formData = new FormData();
                formData.append('docFile', this.item.docFile);

                await doFetch('/api/document/info', 'POST', this.$store, formData, data => {
                    let infoRaw = data.info.split('|');
                    infoRaw.forEach(infoField => {

                        // организация
                        if (infoField.indexOf('Name') === 0) {
                            let item = this.organizations.filter(e => e.organizationName === infoField.split('=')[1]);
                            if (item.length > 0)
                                this.item.organizationId = item[0].organizationId;
                        }

                        // сумма
                        if (infoField.indexOf('Sum') === 0) {
                            let sum = infoField.split('=')[1];
                            this.item.debt = sum.slice(0, sum.length - 2);
                        }

                        // период
                        if (infoField.indexOf('paymPeriod') === 0) {
                            let item = infoField.split('=')[1];
                            let monthNumber = Number(item.substr(0, 2));
                            let year = Number(item.substr(2, 2));
                            this.item.dateStart = new Date(Number('20' + year), monthNumber - 1, 1).toISOString();
                            this.onDateStartChange(new Date(Number('20' + year), monthNumber , 1).toISOString());
                        }
                        //
                    });
                });
            },

            copyDebtToPaid() {
                this.item.paid = this.item.debt;
            }
        }
    };
</script>

<style scoped>
    .col {
        padding: 4px !important;
    }
</style>