<template>
    <div class="product-item" v-if="this.product != null">
        <div class="product-image" @click="goToProduct(product.productID)">
            <img :src="`${Resource.PrefixImage + product.mainImage}`" alt="">
        </div>
        <div class="product-info">
            <div class="title" @click="goToProduct(product.productID)">
                <p>
                    <span v-show="product.condition != Enum.Condition.Used">
                        [{{ configCondition(product.condition) }}]
                    </span>
                    <span :title="product.productName">{{ product.productName }}</span>

                </p>    
            </div>
            <div class="info-detail">
                <table>
                    <tbody>
                        <tr>
                            <td class="min-w-48">{{ Resource.Label.Chip }}</td>
                            <td>
                                <span>
                                    {{ product.chipDetail }}
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td class="min-w-48">{{ Resource.Label.Memory }}</td>
                            <td>
                                <span>
                                    {{ product.memoryDetail }}
                                </span>
                            </td>
                        </tr>
                        <tr >
                            <td class="min-w-48">{{ Resource.Label.Storage }}</td>
                            <td>
                                <span :title="product.storageDetail">
                                    {{ product.storageDetail }}
                                </span>
                            </td>
                        </tr>
                        <tr >
                            <td class="min-w-48">{{ Resource.Label.CardHome }}</td>
                            <td>
                                <span :title="product.cardDetail">
                                    {{ product.cardDetail }}
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td class="min-w-48">{{ Resource.Label.DisplayHome }}</td>
                            <td>
                                <span :title="product.displayDetail">
                                    {{ product.displayDetail }}
                                </span>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="price">
                <div class="price-top flex-row align-items justify-content"
                    v-show="product.discount > 0 && product.status == Enum.StatusProduct.Selling">
                    <div class="old-price">
                        {{ formatMoney(product.price) }}
                    </div>
                    <div class="sale-off">
                        -{{ product.discount }}%
                    </div>
                </div>
                <div class="price-bottom">
                    <span class="price-now">
                        {{ product.status == Enum.StatusProduct.Selling ? this.getNewPrice(product.price, product.discount)
                            : Resource.Text.Contact }}
                    </span>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
import Const from '@/utils/const';
import Enum from '@/utils/enum';
import Resource from '@/utils/resource';
export default {
    name: "ProductItem",
    // props: ["initProduct"],
    props: {
        initProduct: {
            type: Object
        },
        notClick: {
            type: Boolean,
            default: false,
        }
    },
    data() {
        return {
            product: null,
            Enum: Enum,
            Resource: Resource
        }
    },
    methods: {
        goToProduct(productID) {
            if (!this.notClick) {
                this.$router.push({ name: 'ProductDetailPage', query: { productId: productID } });
            }
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

        configCondition(value) {
            let result = value;
            Const.Condition.find((item) => {
                if (item.Value === value) {
                    result = item.Label;
                }
            });
            return result;
        },
    },
    created() {
        this.product = this.initProduct;
    }

}
</script>
<style scoped>
.product-item {
    padding: 10px;
    width: calc(100% / 5 - 12px);
    margin-right: 12px;
    background: #fff;
    position: relative;
}

.product-item .product-image {
    display: block;
    position: relative;
    height: 205px;
}

.product-item .product-image:hover {
    cursor: pointer;
}

.product-item .product-image img {
    width: auto;
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    max-width: 100%;
    max-height: 100%;
    margin: auto;
}

.product-info .title {
    text-align: center;
    /* font-size: 14px; */
    margin: 5px 0;
    display: -webkit-box;
    -webkit-line-clamp: 2;
    -webkit-box-orient: vertical;
    overflow: hidden;
    height: 37px;
    font-weight: 500;
}

.product-info .title:hover {
    cursor: pointer;
    color: #fe6402;
}

.product-info .info-detail {
    display: -webkit-box;
    -webkit-line-clamp: 5;
    -webkit-box-orient: vertical;
    overflow: hidden;
    font-size: 13px;
    line-height: 25px;
    color: #666;
    text-align: left;
    font-weight: 400;
    margin: 9px 0;
}

.product-info .info-detail table {
    border-collapse: collapse;
    width: 100%;
}
.price{
    height: 65px;
    position: relative;
}
.price .price-top {
    height: 20px;
}

.price-top .old-price {
    margin-right: 7px;
    font-weight: 400;
    font-size: 14px;
    color: #222;
    text-decoration: line-through;
}

.price-top .sale-off {
    text-align: center;
    background: #d00012;
    border-radius: 5px;
    font-size: 11px;
    font-weight: 700;
    color: #fff;
    width: 32px;
    height: 20px;
    line-height: 20px;
}
.price-bottom{
    position: absolute;
    bottom: 0%;
}
.price-bottom .price-now {
    color: #fff;
    font-weight: 700;
    background: #ff9300;
    width: 160px;
    height: 35px;
    line-height: 35px;
    font-size: 20px;
    display: block;
    text-align: center;
    border-radius: 16px;
    margin: 10px auto 0;
}
.min-w-48{
    min-width: 48px;
}
table td {
    overflow: hidden;
    max-width: 155px;
    white-space: nowrap;
    text-overflow: ellipsis;
}
</style>