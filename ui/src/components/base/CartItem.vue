<template>
    <div class="item-row d-flex">
        <div class="item-image">
            <div class="image">
                <img :src="`${item.mainImage}`" alt="">
            </div>
        </div>
        <div class="item-info">
            <p class="name-product">
                <span v-show="item.condition != 2">
                    [{{ configCondition(item.condition) }}]
                </span>
                <span>{{ item.productName }}</span>
            </p>
            <div class="price-product d-flex align-items">
                <div class="price-sell">
                    {{ formatMoney(item.price) }}
                </div>
                <del class="price-old" v-if="item.discount">
                    {{ formatMoney(item.priceOld) }}
                </del>
                <div class="sale-off" v-if="item.discount">
                    Giảm {{ item.discount }} %
                </div>
            </div>
            <div class="quantity-product d-flex align-items">
                <p>{{ Resource.Label.SelectQuantity }}</p>
                <div class="amount-control">
                    <InputNumber :transmissionNumber="item.amount" :decimalPlaces="0" :max="item.quantity"
                        @update="updateValue" :isCart="true"/>
                </div>
                <span class="btn-delete" @click="deleteItem">
                    {{ Resource.Button.Delete }}
                </span>
            </div>
            <div class="info-gift" v-if="item.listGift.length > 0">
                <div class="title">
                    - {{ Resource.Text.InfoGift }}
                </div>
                <div class="content">
                    <li v-for="(gift, index) in item.listGift" :key="index">
                        <span>{{ gift.description }}</span>
                    </li>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
import InputNumber from '@/components/base/BaseInputNumber.vue';
import Const from '@/utils/const';
import Resource from '@/utils/resource';
export default {
    name: "CartItem",
    components: {
        InputNumber
    },
    props: ["initItem"],
    data() {
        return {
            Const: Const,
            Resource: Resource,
            item: {}
        }
    },
    methods: {
        configCondition(value) {
            let result = value;
            Const.Condition.find((item) => {
                if (item.Value === value) {
                    result = item.Label;
                }
            });
            return result;
        },
        formatMoney(money) {
            return money.toLocaleString('vi-VN') + ' đ';
        },
        getNewPrice(oldPrice, saleoff) {
            let money = oldPrice * (100 - saleoff) / 1000000;
            money = money - money % 1;
            money = money * 10000;
            return this.formatMoney(money);
        },
        updateValue(value){
            if(value == 0){
                this.$emit('deleteItem', this.item.productID); 
            }
            else {
                this.item.amount = value;  
                this.$emit('updateValue', this.item.productID, value); 
            }
            
        },
        deleteItem(){
            this.$emit('deleteItem', this.item.productID); 
        }
    },
    created() {
        this.item = {...this.initItem};
    }

}
</script>
<style scoped>
.box-cart .item-row {
    width: 100%;
    margin-bottom: 20px;
}
.item-row .item-image {
    width: 200px;
    margin-right: 15px;
}

/* .item-image .image {
    width: 120px;
    height: 120px;
} */

.image img{
    width: 180px;
    height: auto;
}

.item-row .item-info {
    width: calc(100% - 200px);
}

.item-info .name-product {
    padding-bottom: 7px;
    font-weight: 700;
}

.item-info .price-product {
    padding-bottom: 5px;
}

.price-product .price-sell {
    color: #d70018;
    font-weight: 700;
}

.price-product .price-old {
    padding: 0 10px;
}

.price-product .sale-off {
    padding: 3px 5px;
    background: #d70018;
    color: #fff;
    border-radius: 4px;
}

.quantity-product .amount-control {
    margin-left: 10px;
}
.btn-delete{
    margin-left: 10px;
    cursor: pointer;
}
.btn-delete:hover{
    color: #fe6402;
}
.info-gift{
    margin-top: 10px;
    padding: 5px;
    background-color: #f6f6f6;
    border-radius: 10px;
}
.info-gift .title{
    padding: 5px 0 5px 10px;
    font-weight: 700;
}
.info-gift .content li{
    position: relative;
    padding-left: 10px;
    padding-bottom: 7px;
}
.content li::marker{
    color: #d70018;
}
</style>