import Vuex from 'vuex';

const store = new Vuex.Store({
    state: {
        user: null
    },
    getters: {
        user: store => store.user
    },
    actions: {
        setUser(context, user){
            context.commit('setUser', user);
        } 
    },
    mutations: {
        setUser(state, user){
            state.user = user;
        } 
    },

});
export default store;