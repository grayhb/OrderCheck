﻿<template>

    <v-dialog v-model="show" max-width="800px" persistent>

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
                       small
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
                        <v-col>
                            <v-form v-model="validForm" ref="form">
                                <v-text-field v-model="item.estateName"
                                              :rules="[rules.required]"
                                              label="Наименование"></v-text-field>

                                <v-text-field v-model="item.estateAddress"
                                              label="Адрес"></v-text-field>
                            </v-form>
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
    </v-dialog>

</template>

<script>
    import { doFetch } from '../../helpers/fetch-helper.js';

    export default {
        props: ['item', 'show'],
        data: () => ({
            loadingSave: false,
            validForm: false,
            rules: {
                required: value => !!value || 'Обязательное поле',
            }
        }),
        methods: {
            close() {
                this.$emit('close')
            },
            async saveItem() {

                if (!this.validForm)
                    return;

                let formData = new FormData();
                formData.append('estateName', this.item.estateName);
                formData.append('estateAddress', this.item.estateAddress);

                let method = 'POST';

                if (this.item.estateId !== 0) {
                    method = 'PUT';
                    formData.append('estateId', this.item.estateId)
                }

                this.loadingSave = true;

                await doFetch('/api/estates', method, this.$store, formData, data => {
                    this.item = data;
                    this.loadingSave = false;
                    this.$emit('update')
                    this.close();
                });
            },
            async deleteItem() {
                let formData = new FormData();
                formData.append('estateId', this.item.estateId);

                await doFetch('/api/estates', 'DELETE', this.$store, formData, data => {
                    this.$emit('update')
                    this.close();
                });
            }
        }
    };
</script>

<style>
</style>