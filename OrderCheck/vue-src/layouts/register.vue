<template>
    <v-app id="inspire" style="padding: 10px;">
        <v-form ref="registerForm"
              v-model="valid"
              action="registration"
              method="POST">

            <v-card class="mx-auto" style="margin-top: 20px;"
                    max-width="500">

                <v-card-title>
                    <span class="title font-weight-light">Регистрация</span>
                </v-card-title>

                <v-card-text>
                    <v-text-field v-model="email"
                                  label="Email"
                                  name="Email"
                                  :rules="[rules.required, rules.email]"
                                  required></v-text-field>

                    <v-text-field v-model="displayName"
                                  label="Ваше имя"
                                  name="DisplayName"
                                  :rules="[rules.required]"
                                  required></v-text-field>

                    <v-text-field v-model="password"
                                  :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
                                  :rules="[rules.required, rules.password]"
                                  :type="showPassword ? 'text' : 'password'"
                                  label="Пароль"
                                  counter
                                  required
                                  name="password"
                                  @click:append="showPassword = !showPassword"></v-text-field>
                </v-card-text>

                <v-card-actions>
                    <v-btn class=""
                           block
                           @click="onSubmit"
                           :disabled="!valid"
                           color="success">
                        отправить
                    </v-btn>

                </v-card-actions>

            </v-card>

            <v-snackbar v-model="snackbar"
                        vertical
                        :timeout="timeout">
                {{ snackbarText }}

                <template v-slot:action="{ attrs }">
                    <v-btn color="red"
                           text
                           v-bind="attrs"
                           @click="snackbar = false">
                        Закрыть
                    </v-btn>
                </template>
            </v-snackbar>

        </v-form>
    </v-app>
</template>

<script>

    import { setToken } from '../helpers/jwt-helpers';

    export default {
        created: function () {
            this.$vuetify.lang.current = 'ru';
        },

        data: () => ({

            valid: false,
            showPassword: false,
            loading:false,

            email: '',
            displayName: '',
            password: '',

            rules: {
                required: value => !!value || 'Обязательное поле',
                password: v => /^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{6,}$/.test(v) || 'Минимум 6 символов, минимум одна буква, одна цифра и один специальный символ',
                email: value => {
                    const pattern = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
                    return pattern.test(value) || 'Недопустимый email'
                },
            },

            snackbar: false,
            snackbarText: '',
            timeout: 15000,

        }),
        methods: {

            async onSubmit() {

                this.loading = true;

                let self = this;

                const formData = new FormData();

                formData.append('Email', this.email);
                formData.append('DisplayName', this.displayName);
                formData.append('password', this.password);

                await fetch('/registration', {
                    method: 'POST',
                    credentials: 'include',
                    body: formData
                })
                    .then(async (response) => {

                        self.loading = false;

                        const json = await response.json();

                        if (response.ok) {
                            setToken(json.token);
                            window.location.replace("/");
                        }
                        else {
                            if (response.status === 400) {
                                self.snackbarText = json.error;
                                self.snackbar = true;
                            }
                        }

                    });

            }
        }
    }
</script>

<style scoped>
</style>