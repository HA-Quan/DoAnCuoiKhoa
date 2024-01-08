<template>
  <TheHeader :isManagePage="isManager" :user="this.$store.getters.user"/>
  <TheContent />
  <TheFooter v-if="!isManager"/>
</template>
  
<script>
import TheHeader from '../../components/layout/TheHeader.vue';
import TheContent from '../../components/layout/TheContent.vue';
import TheFooter from '../../components/layout/TheFooter.vue';
export default {
  name: 'MainPage',
  components: {
    TheHeader, TheContent, TheFooter
  },
  data() {
    return {
      isManager: false,
    }
  },
  methods: {
  },
  created() {
    this.$router.beforeEach((to) => {
      this.isManager = to.meta.isManager;
    })

  }
}
</script>
  
<style></style>
  