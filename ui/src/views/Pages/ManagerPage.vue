<template>
    <div class="manager-page" :class="isZoom ? 'zoom-in' : ''">
        <TheMenu @zoomMenu="isZoom = !isZoom" />
        <div class="content">
            <router-view></router-view>
        </div>
    </div>
</template>
<script>
import TheMenu from '@/components/layout/TheMenu.vue';
export default {
    name: "ManagerPage",
    components: {
        TheMenu
    },
    data(){
        return{
            isZoom: false,
        }
    },
    methods:{
        
    },
    created(){
    }
}
</script>
<style scoped>
.manager-page{
    width: 100%;
    height: calc(100vh - 82px);
    display: grid;
    grid-template-columns: 204px calc(100%  - 204px);
    background: #f4f5f8;
}
.manager-page.zoom-in{
    grid-template-columns: 56px calc(100% - 56px);
}
.content {
    padding: 16px;
    box-shadow: inset 0 1.5px 2px 0 rgb(0 0 0 / 10%);
    background: #f4f5f8;
}
</style>