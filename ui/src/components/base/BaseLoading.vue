<template>
    <div class="loading">
    <div class="loading-content">
      <img class="loading-icon" src="../../assets/icon/messenger-typing-preloader.svg" alt="" />
      <span class="loading-text">{{ Resource.Text.Loading }}</span>
    </div>
  </div>
</template>
<script>
import Resource from '@/utils/resource';
export default {
    name: "BaseLoading",
    components: {
    },
    data(){
        return{
            Resource: Resource
        }
    },
}
</script>
<style scoped>
.loading {
  background-color: #cccccc;
  width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
}
.loading-content {
  padding: 5px 30px;
  border-radius: 4px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-direction: column;
  background-color: white;
}
.loading-icon {
  height: 50px;
}

</style>