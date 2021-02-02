import Vuex from 'vuex';
Vue.use(Vuex);

export default new Vuex.Store({
    state: {
        token: undefined,
        error: '',
        showError: false
    },
    mutations: {

        setToken(state, newToken) {
            state.token = newToken;
        },

        setError(state, error) {
            state.error = error;
        },

        setShowError(state, showError) {
            state.showError = showError;
        }
    }
});
