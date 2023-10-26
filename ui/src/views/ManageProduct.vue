<template>
    <div class="manage-product"></div>
</template>
<script>
import axios from 'axios';
export default {
    name: "ManageProduct",
    components: {
        
    },
    data(){
        return{
            
        }
    },
    methods:{
        async getHotProduct() {
            try {
                let url = `Product/topProduct?byType=${1}&number=${5}`;
                await axios.get(url)
                    .then((response) => {
                        this.listHotProduct = response.data;
                    })
                    .catch((error) => {
                        console.log(error);
                    });
            } catch (error) {
                console.log(error);
            }
        },
    },
    created(){
        this.getHotProduct();
    }
}
</script>
<style scoped>


</style>