<template>
    <div class="product-detail">
        <div class="container">
            <h1 class="name-product">
                <p>
                    <span v-show="product.condition != 2">
                        [{{ configCondition(product.condition) }}]
                    </span>
                    <span :title="product.productName">{{ product.productName }}</span>

                </p>
            </h1>
            <div class="main-info-product d-flex">
                <div class="main-product-left">
                        <div class="product-image" @click="goToProduct(product.productID)">
                            <img :src="`${Resource.PrefixImage + product.mainImage}`" alt="">
                        </div>
                    <div class="gift-product">
                        <div class="title flex-row">
                            <i class="icon-gift"></i>
                            <span>{{ Resource.Text.InfoGift }}</span>
                        </div>
                        <div class="content">
                            <p class="detail-gift" v-for="(gift, index) in listGifts" :key="index">
                                <i class="icon-check">

                                </i>
                                <span>{{ gift.description }}</span>

                            </p>
                        </div>
                    </div>
                </div>
                <div class="main-product-mid">
                    <div class="d-flex">
                        <div class="rate">
                            5Sao
                        </div>
                        <div class="number-sell">
                            <span style="font-weight: 500;"> {{ product.quantity - product.inventory }}</span>
                            <span style="margin-left: 5px;color: #767676;">{{ Resource.Label.NumberSell }}</span>
                        </div>
                    </div>
                    <div class="price-sell">
                        {{ product.status == Enum.StatusProduct.Selling ? formatMoney(getNewPrice(product.price,
                            product.discount))
                            : Resource.Text.Contact }}
                    </div>
                    <div class="main-price d-flex align-items"
                        v-show="product.discount > 0 && product.status == Enum.StatusProduct.Selling">
                        <strong>{{ Resource.Text.PriceOld }}:</strong>
                        <del class="price-old"> {{ formatMoney(product.price) }}</del>
                        <div class="sale-off">
                            -{{ product.discount }}%
                        </div>
                    </div>
                    <div class="product-status">
                        <div class="item">
                            <div class="content">
                                {{ this.configCondition(product.condition) }}
                            </div>
                            <div class="tooltip">
                                {{ this.configTooltipCondition(product.condition) }}
                            </div>
                        </div>
                    </div>
                    <div class="config-product">
                        <div class="d-flex align-items flex-wrap select-config">
                            <span class="name">{{ Resource.Label.Chip }}</span>
                            <ul>
                                <li>
                                    <span>{{ product.chipDetail }}</span>
                                </li>
                            </ul>
                        </div>

                        <div class="d-flex align-items flex-wrap select-config">
                            <span class="name">{{ Resource.Label.Memory }}</span>
                            <ul>
                                <li>
                                    <span>{{ product.memoryDetail }}</span>
                                </li>
                            </ul>
                        </div>

                        <div class="d-flex align-items flex-wrap select-config">
                            <span class="name">{{ Resource.Label.Storage }}</span>
                            <ul>
                                <li>
                                    <span>{{ product.storageDetail }}</span>
                                </li>
                            </ul>
                        </div>

                        <div class="d-flex align-items flex-wrap select-config">
                            <span class="name">{{ Resource.Label.Card }}</span>
                            <ul>
                                <li>
                                    <span>{{ product.cardDetail }}</span>
                                </li>
                            </ul>
                        </div>

                        <div class="d-flex align-items flex-wrap select-config">
                            <span class="name">{{ Resource.Label.Display }}</span>
                            <ul>
                                <li>
                                    <span>{{ product.displayDetail }}</span>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="btn-action d-flex align-items flex-wrap"
                        v-if="product.status == Enum.StatusProduct.Selling">
                        <div class="btn" @click="buyNow">
                            <span>{{ Resource.Button.BuyNow }}</span>
                        </div>
                        <div class="btn" @click="addToCart">
                            <span>{{ Resource.Button.AddToCart }}</span>
                        </div>
                    </div>
                    <div class="btn-contact" v-else>
                        {{ Resource.Text.Contact }}
                    </div>
                </div>
                <div class="main-product-right">
                    <div class="product-warranty">
                        <h3 class="title">{{ Resource.Text.InfoWarranty }}</h3>
                        <div class="content">
                            <p v-for="(content, index) in Resource.Warranty" :key="index">
                                {{ content }}
                            </p>
                        </div>
                    </div>
                    <div class="product-warranty">
                        <h3 class="title">{{ Resource.Text.AssuredShopping }}</h3>
                        <div class="content">
                            <p v-for="(content, index) in Resource.AssuredShopping" :key="index">
                                {{ content }}
                            </p>
                        </div>
                    </div>
                    <img class="banner-detail" src="../../assets/icon/banner-right-detail.png">
                </div>
            </div>
            <div class="detail-info-product d-flex">
                <div class="more-info-product">
                    <h3 class="title">
                        <span>{{ Resource.Text.Description }}</span>
                    </h3>
                    <div :class="['content', { 'more-content': moreContent == true }]">
                    {{ product.description }}</div>
                    <div class="btn-more d-flex" @click="moreContent = !moreContent;">
                        <span class="mr-10">{{ Resource.Button.MoreAll }}</span>
                        <div :class="['icon-arrow', { 'icon-arrow-up': moreContent == true }]"></div>
                    </div>
                </div>
                <div class="detail-property-product">
                    <div class="property-product">
                        <h3 class="title">
                            <span>{{ Resource.Text.Property }}</span>
                        </h3>
                        <div :class="['content', { 'more-content': moreDetailProperty == true }]">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>
                                            <p>{{ Resource.LabelProduct.Chip }}</p>
                                        </td>
                                        <td>
                                            <p>{{ product.chipDetail }}</p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <p>{{ Resource.LabelProduct.Memory }}</p>
                                        </td>
                                        <td>
                                            <p>{{ product.memoryDetail }}</p>
                                        </td>
                                    </tr>
                                    <tr v-show="product.amountRam != null">
                                        <td>
                                            <p>{{ Resource.LabelProduct.AmountMemory }}</p>
                                        </td>
                                        <td>
                                            <p>{{ product.amountRam }}</p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <p>{{ Resource.LabelProduct.Storage }}</p>
                                        </td>
                                        <td>
                                            <p>{{ product.storageDetail }}</p>
                                        </td>
                                    </tr>
                                    <tr v-show="product.amountDisk != null">
                                        <td>
                                            <p>{{ Resource.LabelProduct.AmountStorage }}</p>
                                        </td>
                                        <td>
                                            <p>{{ product.amountDisk }}</p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <p>{{ Resource.LabelProduct.Display }}</p>
                                        </td>
                                        <td>
                                            <p>{{ product.displayDetail }}</p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <p>{{ Resource.LabelProduct.Card }}</p>
                                        </td>
                                        <td>
                                            <p>{{ product.cardDetail }}</p>
                                        </td>
                                    </tr>
                                    <tr v-show="product.weight != null">
                                        <td>
                                            <p>{{ Resource.LabelProduct.Weight }}</p>
                                        </td>
                                        <td>
                                            <p>{{ product.weight }}</p>
                                        </td>
                                    </tr>
                                    <tr v-show="product.dimension != null">
                                        <td>
                                            <p>{{ Resource.LabelProduct.Dimension }}</p>
                                        </td>
                                        <td>
                                            <p>{{ product.dimension }}</p>
                                        </td>
                                    </tr>
                                    <tr v-show="product.color != null">
                                        <td>
                                            <p>{{ Resource.LabelProduct.Color }}</p>
                                        </td>
                                        <td>
                                            <p>{{ product.color }}</p>
                                        </td>
                                    </tr>
                                    <tr v-show="product.material != null">
                                        <td>
                                            <p>{{ Resource.LabelProduct.Material }}</p>
                                        </td>
                                        <td>
                                            <p>{{ product.material }}</p>
                                        </td>
                                    </tr>
                                    <tr v-show="product.operatingSystem != null">
                                        <td>
                                            <p>{{ Resource.LabelProduct.OperatingSystem }}</p>
                                        </td>
                                        <td>
                                            <p>{{ product.operatingSystem }}</p>
                                        </td>
                                    </tr>
                                    <tr v-show="product.touchpad != null">
                                        <td>
                                            <p>{{ Resource.LabelProduct.Touchpad }}</p>
                                        </td>
                                        <td>
                                            <p>{{ product.touchpad }}</p>
                                        </td>
                                    </tr>
                                    <tr v-show="product.keyboard != null">
                                        <td>
                                            <p>{{ Resource.LabelProduct.Keyboard }}</p>
                                        </td>
                                        <td>
                                            <p>{{ product.keyboard }}</p>
                                        </td>
                                    </tr>
                                    <tr v-show="product.speakers != null">
                                        <td>
                                            <p>{{ Resource.LabelProduct.Speakers }}</p>
                                        </td>
                                        <td>
                                            <p>{{ product.speakers }}</p>
                                        </td>
                                    </tr>
                                    <tr v-show="product.battery != null">
                                        <td>
                                            <p>{{ Resource.LabelProduct.Battery }}</p>
                                        </td>
                                        <td>
                                            <p>{{ product.battery }}</p>
                                        </td>
                                    </tr>
                                    <tr v-show="product.camera != null">
                                        <td>
                                            <p>{{ Resource.LabelProduct.Camera }}</p>
                                        </td>
                                        <td>
                                            <p>{{ product.camera }}</p>
                                        </td>
                                    </tr>
                                    <tr v-show="product.connectivityNetwork != null">
                                        <td>
                                            <p>{{ Resource.LabelProduct.ConnectivityNetwork }}</p>
                                        </td>
                                        <td>
                                            <p>{{ product.connectivityNetwork }}</p>
                                        </td>
                                    </tr>
                                    <tr v-show="product.standardPorts != null">
                                        <td>
                                            <p>{{ Resource.LabelProduct.StandardPorts }}</p>
                                        </td>
                                        <td>
                                            <p>{{ product.standardPorts }}</p>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div class="btn-box" @click="moreDetailProperty = !moreDetailProperty;">
                            <span>{{ Resource.Button.MoreConfigDetail }}</span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="same-product">

            </div>
        </div>
    </div>
    <BaseToastMessage v-show="toastMessage.isShowed" :class="`toast-${toastMessage.type} icon-toast-${toastMessage.type}`">
        <template #message>{{ toastMessage.message }}</template>
    </BaseToastMessage>
</template>
<script>
import axios from 'axios';
import Resource from '@/utils/resource';
import Const from '@/utils/const';
import Enum from '@/utils/enum';
import BaseToastMessage from '@/components/base/BaseToastMessage.vue';
export default {
    name: "ProductDetailPage",
    components: {
        BaseToastMessage
    },
    data() {
        return {
            Resource: Resource,
            Enum: Enum,
            moreContent: false,
            moreDetailProperty: false,
            product: {},
            listGifts: [],
            toastMessage: { // thông báo báo
                message: "", // nội dung thông báo
                type: "", // loại thông báo
                isShowed: false, // cờ điều khiển bật tắt thông báo
            },
        }
    },
    watch: {
        '$route.query.productId': {
            handler: function (newVal) {
                if (newVal != undefined) {
                    this.getProductByID();
                }
            }
        },

    },
    methods: {
        formatMoney(money) {
            if (!isNaN(money)) {
                var valueMoney = parseInt(money);
                return valueMoney.toString().replace(/(\d)(?=(\d{3})+(?:\.\d+)?$)/g, "$1.") + "Đ";
            } else {
                return valueMoney + "Đ";
            }
        },

        getNewPrice(oldPrice, saleoff) {
            let money = oldPrice * (100 - saleoff) / 1000000;
            money = money - money % 1;
            money = money * 10000;
            return money;
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

        configTooltipCondition(value) {
            let result = value;
            Const.Condition.find((item) => {
                if (item.Value === value) {
                    result = item.Detail;
                }
            });
            return result;
        },

        buyNow() {
            let check = false;
            let cart = JSON.parse(localStorage.getItem('cart'));
            cart.find(
                (item) => {
                    if (item.productID == this.product.productID && item.amount < this.product.inventory) {
                        item.amount += 1;
                        check = true;
                    }
                }
            );
            if (!check) {
                let item = {
                    productID: this.product.productID,
                    productName: this.product.productName,
                    mainImage: this.product.mainImage,
                    condition: this.product.condition,
                    price: this.getNewPrice(this.product.price, this.product.discount),
                    priceOld: this.product.price,
                    discount: this.product.discount,
                    quantity: this.product.inventory,
                    amount: 1,
                    listGift: this.listGifts
                };
                cart.push(item);
                this.emitter.emit("updateAmountCart", cart.length);
            }
            localStorage.setItem('cart', JSON.stringify(cart));
            this.$router.push({ name: 'CartPage' });
        },

        addToCart() {
            let check = false;
            let cart = JSON.parse(localStorage.getItem('cart'));
            cart.find(
                (item) => {
                    if (item.productID == this.product.productID && item.amount < this.product.quantity) {
                        item.amount += 1;
                        check = true;
                    }
                }
            );
            if (!check) {
                let item = {
                    productID: this.product.productID,
                    productName: this.product.productName,
                    mainImage: this.product.mainImage,
                    condition: this.product.condition,
                    price: this.getNewPrice(this.product.price, this.product.discount),
                    priceOld: this.product.price,
                    discount: this.product.discount,
                    quantity: this.product.inventory,
                    amount: 1,
                    listGift: this.listGifts
                };
                cart.push(item);
                this.emitter.emit("updateAmountCart", cart.length);
            }
            localStorage.setItem('cart', JSON.stringify(cart));
            this.showToast(Resource.Message.AddProductToCartSucces, Const.TypeToast.Success)
        },

        async getProductByID() {
            try {
                let url = `Product/productDetail${this.$route.query.productId}`;
                await axios.get(url)
                    .then((response) => {
                        this.product = response.data.product;
                        this.listGifts = response.data.listGifts;
                    })
                    .catch((error) => {
                        console.log(error);
                    });
            } catch (error) {
                console.log(error);
            }
        },
        /**
         * Hiển thị toast message khi thực hiện thành công
         * @param {*} contentToast: nội dung thông báo 
         * Author: HAQUAN(28/10/2022)
         */
        showToast(contentToast, TypeToast) {
            this.toastMessage = {
                message: contentToast,
                type: TypeToast,
                isShowed: true,
            };
            setTimeout(() => {
                this.toastMessage.isShowed = false;
            }, 5000);
        },
    },
    created() {
        this.getProductByID();
    }
}
</script>
<style scoped>
.product-detail .name-product {
    margin-top: 10px;
    font-size: 19px;
    margin-bottom: 20px;
}

.product-detail .main-product-left {
    width: 390px;
    background: #fff;
    padding: 10px;
    border-radius: 15px;
    margin-right: 10px;
}

.product-detail .main-product-mid {
    width: 490px;
    margin-right: 10px;
    background: #fff;
    padding: 10px;
    border-radius: 15px;
}

.product-detail .main-product-right {
    width: calc(100% - 490px - 390px);
    background: #fff;
    padding: 10px;
    border-radius: 15px;
}

.main-product-left .gift-product {
    border: 1px solid #ff9300;
    border-radius: 8px;
    margin-top: 15px;
}

.gift-product .title {
    padding: 10px 5px;
    background: #fff2e1;
    border-radius: 7px 7px 0 0;
    color: #ff9300;
    font-weight: 700;
}

.gift-product .content p {
    color: #007bff;
    font-size: 12px;
    padding: 10px;
    padding-top: 0;
}

.main-product-mid .price-sell {
    font-size: 24px;
    font-weight: 700;
    color: #ef1200;
    padding-top: 10px;
}

.main-product-mid .main-price {
    padding: 10px 0;
}

.main-price .price-old {
    font-weight: 400;
    font-size: 14px;
    padding: 0 5px;
}

.main-price .sale-off {
    color: #ef1200;
    font-weight: 700;
    font-size: 14px;
}

.main-product-mid .product-status {
    margin: 10px 0;
}

.product-status .item {
    position: relative;
    width: 100px;
    cursor: pointer;
}

.product-status .content {
    padding: 10px;
    background: #f2f4f6;
    border-radius: 5px;
}

.product-status .tooltip {
    display: none;
    position: absolute;
    box-shadow: 0px 0px 12px rgb(0 0 0 / 19%);
    padding: 10px;
    margin-left: 5px;
    background: #fff;
    z-index: 10;
    left: 90px;
    border-radius: 5px;
    top: -14px;
    width: 300px;
}

.product-status .item:hover .tooltip {
    display: block;
}

.config-product {
    padding: 10px 0;
}

.select-config {
    margin-bottom: 10px;
}

span.name {
    width: 90px;
}

.select-config ul {
    display: inline-block;
    width: calc(100% - 90px) !important;
    margin-bottom: 0;
}

.select-config ul li {
    margin-bottom: 5px;
    float: left;
    margin-right: 5px;
    border: solid 1px #aaa;
    padding: 7px 10px;
    cursor: pointer;
    position: relative;
    -webkit-box-sizing: border-box;
    box-sizing: border-box;
    overflow: hidden;
    color: #333;
    border-radius: 8px;
    border-color: #ff9300 !important;
    border-width: 1px !important;
}

.select-config ul li::before {
    content: "";
    position: absolute;
    right: 0;
    bottom: 0;
    width: 0;
    height: 0;
    border-bottom: 0 solid transparent;
    border-top: 20px solid transparent;
    border-right: 20px solid #ff9300;
}

.main-product-mid .btn-action {
    margin-right: -10px;
}

.btn-action .btn {
    width: calc(100% / 2 - 10px);
    margin-right: 10px;
    background: linear-gradient(180deg, #e49f08 0%, #ed8d06 79.17%);
    border-radius: 10px;
    height: 50px;
    color: #fff;
    font-size: 20px;
    font-weight: 700;
    display: flex;
    justify-content: center;
    align-items: center;
    cursor: pointer;
}

.btn-action .btn:hover {
    background: linear-gradient(180deg, #fdae02 0%, #ff9401 79.17%);
}

.main-product-right .product-warranty {
    border-radius: 10px;
    border: 1px solid #ff9300;
    margin-bottom: 10px;
}

.product-warranty .title {
    padding: 12px;
    font-weight: 700;
    border-bottom: 1px solid #d9d9d9;
    font-size: 19px;
}

.product-warranty .content {
    padding: 10px;
}

.product-warranty .content p {
    padding-bottom: 5px;
    line-height: 20px;
}

.icon-check {
    position: relative;
}

.icon-check::before {
    content: "";
    width: 14px;
    height: 14px;
    background-image: url(../../assets/icon/check.png);
    background-size: 14px 14px;
    position: absolute;
    display: block;
}

.icon-gift {
    position: relative;
    margin-right: 24px;
}

.icon-gift::before {
    content: "";
    width: 18px;
    height: 18px;
    background-image: url(../../assets/icon/giftbox.png);
    background-size: 18px 18px;
    position: absolute;
    display: block;
}

.detail-gift:first-child {
    margin-top: 10px;
}

.detail-gift span {
    margin-left: 20px;
}

.banner-detail {
    max-width: 100%;
    height: auto;
    background-image: url();
}

.detail-info-product {
    margin: 20px 0;
}

.detail-info-product .more-info-product {
    width: 795px;
    margin-right: 10px;
    background: #fff;
    border-radius: 15px;
}

.more-info-product .title {
    padding: 15px 0;
}

.more-info-product .title span {
    font-size: 20px;
    border-bottom: 1px solid #ff9300;
    padding-bottom: 10px;
    padding-left: 15px;
}

.more-info-product .content {
    padding: 10px;
    width: 100%;
    display: block;
    overflow: hidden;
    position: relative;
    height: 500px;
}

.more-info-product .more-content {
    height: auto !important;
}

.more-info-product .btn-more {
    width: 130px;
    text-align: center;
    margin: 10px auto;
    color: #222;
    padding: 10px;
    border-radius: 20px;
    border: 1px solid #666;
    justify-content: center;
    align-items: center;
}

.detail-info-product .detail-property-product {
    width: calc(100% - 795px);
}

.btn-more .icon-arrow {
    width: 14px;
    height: 14px;
    background: url(../../assets/icon/arrow-right.png);
    background-size: 14px 14px;
    transform: rotate(90deg);
}

.btn-more .icon-arrow-up {
    transform: rotate(-90deg) !important;
}

.property-product {
    background: #fff;
    border-radius: 15px;
    padding: 10px 0;
}

.property-product .title {
    padding: 15px 0;
}

.property-product .title span {
    font-size: 20px;
    border-bottom: 1px solid #ff9300;
    padding-bottom: 10px;
    padding-left: 15px;
}

.property-product .content {
    padding: 10px;
    height: 500px;
    position: relative;
    overflow: hidden;
}

.property-product .more-content {
    height: auto !important;
}

.property-product .content::after {
    content: "";
    position: absolute;
    left: 0;
    right: 0;
    bottom: 0;
    height: 50px;
    background: linear-gradient(transparent, #fff);
}

.property-product .content table {
    border-collapse: collapse;
    width: 100%;
    max-width: 100%;
}

.property-product .content td {
    padding: 5px 0;
}

.property-product .content td:nth-child(odd) {
    font-weight: 700;
    width: 30%;
}

.property-product .btn-box {
    display: block;
    width: 96%;
    text-align: center;
    margin: 10px auto;
    color: #4d91ff;
    padding: 10px;
    border: 1px solid #4d91ff;
    cursor: pointer;
}

.same-product {
    background: #fff;
    border-radius: 15px;
}

.same-product .title {
    font-size: 24px;
    padding: 10px 0;
}

.btn-contact {
    width: 100%;
    display: flex;
    justify-content: center;
    line-height: 50px;
    height: 50px;
    margin-bottom: 10px;
    background: #0084cb;
    border-radius: 10px;
    font-size: 22px;
    font-weight: 700;
    cursor: pointer;
    color: #fff;
}

.btn-contact:hover {
    background: #1097df;
}
.product-image {
    display: block;
    position: relative;
    height: 350px;
}
.product-image img {
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
.number-sell{
    border-left: 1px solid rgba(0,0,0,.14);
    padding: 0 15px;
    font-size: 15px;
}
</style>