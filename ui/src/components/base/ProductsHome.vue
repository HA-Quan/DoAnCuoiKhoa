<template>
    <div class="product-category-home" v-if="listHotProduct.length > 0 ">
        <div class="title flex-row align-items space-between">
            <h3 class="name">{{ demand.Label }}</h3>
            <div class="list-cate flex-row">
                <p class="item-cate" v-for="(item, index) in Resource.ListTrademark" :key="index" @click="filterProductByTrademark(item)">
                    {{ item }}
                </p>
                <div class="more-all flex-row align-items" @click="filterProductByDemand(demand.Value)">
                    <p>{{ Resource.Text.SeeAll }}</p>
                    <div class="icon-arrow-down"></div>
                </div>
            </div>
        </div>
        <div class="list-product">
            <ProductItem v-for="product in listHotProduct" :key="product.productID" :initProduct="product" />
        </div>
    </div>
</template>
<script>
import axios from 'axios'
import Const from '@/utils/const';
import Resource from '@/utils/resource';
import ProductItem from '@/components/base/ProductItem.vue';
export default {
    name: "ProductsHome",
    components: {
        ProductItem
    },
    props: ["demand"],
    data() {
        return {
            Const: Const,
            Resource: Resource,
            listHotProduct: []
        }
    },
    // watch: {
    //     initItem: {
    //         handler: function (newVal) {
    //             this.item = newVal;
    //         }
    //     },
    // },
    methods: {
        async getData() {
            try {
                let url = `Product/by-demand?demand=${this.demand.Value}&number=${5}`;
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
        filterProductByDemand(demand) {
            this.$router.push(
                {
                    name: 'FilterPage',
                    query: {
                        demandType: demand
                    }
                }
            )
        },
        filterProductByTrademark(trademark) {
            this.$router.push(
                {
                    name: 'FilterPage',
                    query: {
                        trademark: trademark
                    }
                }
            )
        },
    },
    created() {
        this.getData();
    }

}
</script>
<style scoped>
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
    /* width: 260px; */
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
    background-image: url(../../assets/icon/arrow-right.png);
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