<template>
    <div class="homepage">
        <div class="container">
            <div class="slide d-flex">
                SLIDE
            </div>
            <div class="sale d-flex">
                SALE
            </div>
            <div class="product-category-home">
                <div class="title flex-row align-items space-between">
                    <h3 class="name">Sản phẩm hot</h3>
                    <div class="list-cate flex-row">
                        <p class="item-cate">Acer</p>
                        <p class="item-cate">Lenovo</p>
                        <p class="item-cate">Asus</p>
                        <p class="item-cate">Dell</p>
                        <p class="item-cate">Gigabyte</p>
                        <p class="item-cate">Apple</p>
                        <div class="more-all flex-row align-items">
                            <p>Xem tất cả</p>
                            <div class="icon-arrow-down"></div>
                        </div>
                    </div>
                </div>
                <div class="list-product">
                    <ProductItem v-for="product in listHotProduct" :key="product.productID" :initProduct="product"/>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
import axios from 'axios';
import ProductItem from '@/components/base/ProductItem.vue';
export default {
    name: "HomePage",
    components: {
        ProductItem
    },
    data(){
        return{
            listHotProduct: null
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
.homepage .slide {
    margin-left: 240px;
    height: 410px;
    margin-top: 10px;
    background-color: aliceblue;
}
.homepage .sale {
    margin: 20px 0;
    margin-right: -10px;
    background-color: antiquewhite;
    height: 175px;
}
.product-category-home{
    margin-top: 20px;
}

.product-category-home .title .name{
    color: #555;
    font-size: 21px;
    text-transform: uppercase;
    height: 45px;
    padding: 0 10px;
    line-height: 45px;
    position: relative;
    width: 260px;
    font-weight: 700;
}
.product-category-home .title .item-cate {
    background: #ebebeb;
    border-radius: 15px;
    padding: 10px 20px;
    margin-right: 10px;
}

.product-category-home .title .item-cate:hover {
    color: #fe6402;
    cursor: pointer;
}

.product-category-home .list-product {
    margin: 10px 0;
    margin-right: -10px;
    display: flex;
    flex-direction: row;
    
}

 .icon-arrow-down {
    width: 8px;
    height: 8px;
    background-image: url(../assets/icon/arrow-right.png);
    background-size: 8px 8px;
    margin-left: 6px;
    transform: rotate(90deg);
    margin-top: 2px;
}

.more-all:hover{
    color: #fe6402;
    cursor: pointer;
}
</style>