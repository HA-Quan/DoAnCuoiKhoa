<template>
    <div class="header">
        <div class="header-top">
            <div class="container flex-row">
                <div class="header-left">
                    <div class="logo flex-row" @click="clickLogo">
                        <div class="logo-icon"></div>
                        <div class="logo-title">
                            <p>{{ Resource.NameWeb }}</p>
                        </div>
                    </div>
                </div>
                <div class="header-mid d-flex align-items" v-if="!isManagePage">
                    <div class="form-input-search flex-row">
                        <input type="text" @keypress.enter="search" v-model="keyword"
                            :placeholder="Resource.Placehoder.SearchFil">
                        <div class="icon" @click="search">
                            <div class="list-icon icon-search"></div>
                        </div>

                    </div>
                </div>
                <div class="header-right flex-row align-items">
                    <div class="btn-login align-items d-flex" @click="btnLoginOnClick" v-if="user == null">
                        {{ Resource.Text.Login }}
                    </div>
                    <div class="title" v-if="user != null">
                        {{ user.fullName }}
                    </div>
                    <div class="settings" @click="showOption" v-if="user != null">
                        <img :src="Resource.PrefixImage +user.avatar" alt="">
                        <div class="list-option pad-8 " id="list-option" v-show="isShowOption"
                            @focusout="isShowOption = false;">
                            <!-- <span class="arrow-list"></span> -->
                            <div class="option flex-row" v-for="(option, index) in listAction" :key="index"
                                @click="action(option.comand)">
                                <div class="option-icon align-items">
                                    <div :class="option.icon"></div>
                                </div>
                                <span class="option-title align-items">
                                    {{ option.name }}
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="cart" @click="goToCart">
                        <div class="icon-cart">
                        </div>
                        <div class="amount" v-show="amountCart != 0">
                            {{ amountCart }}
                        </div>
                    </div>
                </div>
            </div>


        </div>
        <div class="header-bottom " v-if="!isManagePage">
            <div class="container flex-row">
                <div class="header-menu align-items">
                    <div class="flex-row align-items pointer">
                        <div class="list-icon icon-menu">

                        </div>
                        <div class="title-menu">
                            <p>{{ Resource.Label.Category }}</p>
                        </div>
                    </div>

                    <div :class="['list-category', {'home': isHomePage == true,}]">
                        <div class="cate-item" v-for="(cate, index) in listCate" :key="index">
                            <div class="item flex-row align-items">
                                <div class="cate-item-left flex-row align-items"
                                    @click="filterProductByCategory(cate.categoryID)">
                                    <!-- <div class="cate-icon" :class="cate.icon"></div> -->
                                    <div class="cate-icon">
                                        <img src="../../assets/icon/ic_laptop.png" alt="">
                                    </div>
                                    <p class="cate-title">{{ cate.categoryName }}</p>
                                </div>
                                <div class="cate-item-right">
                                    <div class="icon-arrow-right"></div>
                                </div>
                            </div>
                            <div class="menu-hover flex-row" v-if="cate.categories.length != 0">
                                <div class="list-holder" v-for="(item, index) in cate.categories" :key="index">
                                    <p class="holder-title">{{ item.title }}</p>
                                    <div class="item-holder" v-for="(category, index) in item.categories" :key="index">
                                        <div class="holder-content" @click="filterProductByCategory(category.categoryID)">
                                            <p>{{ category.categoryName }}</p>
                                            <div class="icon-arrow-right" v-if="category.categories.length != 0"></div>
                                        </div>
                                        <div class="list-sub-item" v-if="category.categories.length != 0">
                                            <div class="sub-item-holder" v-for="(subItem, index) in category.categories"
                                                :key="index" @click="filterProductByCategory(subItem.categoryID)">
                                                {{ subItem.categoryName }}
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="header-bottom-right flex-row align-items">
                    <div class="item-right flex-row" v-for="(item, index) in listItem" :key="index"
                        @click="goToProduct(item.productID)">
                        <div class="pulse-icon">
                            <div class="icon-wrap">
                            </div>
                            <div class="elements">
                                <div class="pulse"></div>
                            </div>
                        </div>
                        <div class="title-item">
                            <span :title="item.productName">{{ item.productName }}</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
</template>
<script>
import axios from 'axios';
import Resource from '@/utils/resource';
import Const from '@/utils/const';
import CommonFn from '@/utils/commonFuncion';
export default {
    name: 'TheHeader',
    props: ['isManagePage', 'user'],
    data() {
        return {
            Resource: Resource,
            Const: Const,
            isShowOption: false,
            listAction: [],
            listItem: [
            ],
            listCate: [
            ],
            amountCart: 0,
            keyword: '',
            isHomePage: false
        };
    },
    watch: {
        '$route.name': {
            handler: function (newVal) {
                if (newVal != undefined) {
                    this.isHomePage = newVal == "HomePage" ? true : false;
                }
            }
        },
    },
    methods: {
        clickLogo() {
            this.$router.push({ name: 'HomePage' });
        },

        btnLoginOnClick() {
            this.$router.push({ name: 'LoginForm' });
        },

        showOption() {
            this.isShowOption = !this.isShowOption;
        },

        action(comand) {
            let me = this;
            me[comand]();
        },

        infoOnClick() {
            this.$router.push({ name: 'InfoUser' });
        },

        changePasswordOnClick() {
            this.$router.push({ name: 'ChangePassword' });
        },
        manageAccountOnClick() {
            this.$router.push({ name: 'AccountList' });
        },

        manageProductOnClick() {
            this.$router.push({ name: 'ProductList' });
        },

        manageOrderOnClick() {
            this.$router.push({ name: 'OrderList' });
        },

        manageImportOnClick() {
            this.$router.push({ name: 'ImportProductList' });
        },

        manageGiftOnClick() {
            this.$router.push({ name: 'GiftList' });
        },

        managePromotionOnClick() {
            this.$router.push({ name: 'PromotionList' });
        },

        manageNewsOnClick() {
            this.$router.push({ name: 'OrderList' });
        },

        statisticOnClick() {
            this.$router.push({ name: 'StatisticPage' });
        },

        goToCart() {
            this.$router.push({ name: 'CartPage' });
        },

        listOrderOnClick() {
            this.$router.push({ name: 'ManagerPage' });
        },

        goToProduct(productID) {
            this.$router.push({ name: 'ProductDetailPage', query: { productId: productID } });
        },

        historyShoppingOnClick() {
            this.$router.push({ name: 'HistoryShopping' });
        },

        replyRateOnClick() {
            this.$router.push({ name: 'RateList' });
        },

        filterProductByCategory(categoryID) {
            this.$router.push(
                {
                    name: 'FilterPage',
                    query: {
                        categoryID: categoryID
                    }
                }
            )
        },

        search() {
            this.$router.push(
                {
                    name: 'FilterPage',
                    query: {
                        keyword: this.keyword
                    }
                }
            );
            this.keyword = '';
        },

        logout() {
            CommonFn.eraseCookie('RefreshToken');
            CommonFn.eraseCookie('Token');
            this.$store.dispatch('setUser', null);
            axios.defaults.headers.common["Authorization"] = "Bearer ";
            this.$router.push({ path: '/' });

        },

        async getTopProduct() {
            try {
                let url = `Product/topProduct?number=${3}`;
                await axios.get(url)

                    .then((response) => {
                        this.listItem = response.data;
                    })

                    .catch((error) => {
                        console.log(error);
                    });
            } catch (error) {
                console.log(error);
            }
        },

        async getCategory() {
            try {
                let url = `Category/getAll`;
                await axios.get(url)

                    .then((response) => {       
                        this.controlCategory(response.data);
                    })

                    .catch((error) => {
                        console.log(error);
                    });
            } catch (error) {
                console.log(error);
            }
        },

        controlCategory(listAllCategory) {
            this.listCate = [];
            listAllCategory.forEach(item => {
                let category = {
                    categoryID: null,
                    categoryName: null,
                    icon: null,
                    categories: []
                };
                if (item.parentID == "00000000-0000-0000-0000-000000000000") {
                    category.categoryID = item.categoryID;
                    category.categoryName = item.categoryName;
                    if (item.description != null) {
                        let record = {
                            categoryID: category.categoryID,
                            title: item.description,
                            categories: []
                        };
                        category.categories.push(record);
                    }
                    if (item.categoryName == "Laptop Mới") {
                        category.icon = "laptop-new";
                        category.categories.push(Const.DemandCategory);

                    }
                    else if (item.categoryName == "Laptop Cũ") {
                        category.icon = "laptop-used";
                        category.categories.push(Const.DemandCategory);
                    }
                    this.listCate.push(category);
                }
            });
            this.listCate.forEach(item => {
                if (item.categories.length != 0) {
                    item.categories[0].categories = this.getSubCategory(listAllCategory, item.categories[0]);
                }
            });
        },

        getSubCategory(allCategory, category) {
            allCategory.forEach(item => {
                let record = {
                    categoryID: null,
                    categoryName: null,
                    categories: []
                };
                if (category.categoryID == item.parentID) {
                    record.categoryID = item.categoryID;
                    record.categoryName = item.categoryName;
                    record.categories = this.getSubCategory(allCategory, record);
                    category.categories.push(record);
                }
            });
            return category.categories;
        },

        configAction() {
            if (this.user != null) {
                Const.ListActionSetting.find(
                    (item) => {
                        if (item.Role == this.user.role) {
                            this.listAction = item.Value;
                        }
                    }
                );
            }
        }

    },
    created() {
        // window.addEventListener('click', function(e) {
        //     if (!document.getElementById('list-option').contains(e.target)) {
        //         this.isOpenCombobox = false
        //     }
        // });

        if(this.$route.name == "HomePage"){
            this.isHomePage = true;
        }
        this.getTopProduct();
        this.getCategory();
        this.amountCart = JSON.parse(localStorage.getItem('cart')) != null ? JSON.parse(localStorage.getItem('cart')).length : 0;
        this.configAction();
    },
    mounted() {
        this.emitter.on("updateAmountCart", amount => {
            this.amountCart = amount;
        });

    },
}
</script>
<style scoped>
.list-sub-item {
    left: 50%;
    position: absolute;
    display: none;
    top: -8px;
    width: 210px;
    background: #fff;
    box-shadow: 0 0 4px 0 #b5b5b5;
    margin-left: 10px;
    padding: 5px 10px;
    z-index: 9;
    border-radius: 3px;
}

.list-sub-item::before {
    content: '';
    position: absolute;
    width: 0;
    border: solid 5px transparent;
    border-right-color: #d2d2d2;
    left: -11px;
    top: 16px;
}

.item-holder:hover .list-sub-item {
    display: block;
}

.list-sub-item .sub-item-holder {
    line-height: 1.6;
    cursor: pointer;
}

.list-sub-item .sub-item-holder:hover {
    color: #ff9300;
}

.header-top {
    background: #000;
    padding: 10px 0;
}

.header-top .container {
    justify-content: space-between;
}

.header-left {
    min-width: 230px;
}

.logo {
    align-items: center;
    cursor: pointer;
}

.logo-icon {
    height: 62px;
    width: 62px;
    background: transparent url(../../assets/image/logo.png) no-repeat;
    background-position: center;
    background-size: 60px 60px;
    border-radius: 20px;
}

.logo-title {
    margin-left: 12px;
    color: yellow;
    font-size: 2em;
    font-weight: 900;
}

.form-input-search {
    position: relative;
    border-radius: 20px;
    background-color: #fff;
    width: 350px;
}

.form-input-search input {
    width: 100%;
    height: 42px;
    border: none;
    outline: none;
    padding: 5px 10px;
    border-radius: 20px;
    font-size: 14px;
    font-style: italic;
}

.icon {
    height: 42px;
    min-width: 48px;
    background: #ff9300;
    border-top-right-radius: 20px;
    border-bottom-right-radius: 20px;
    display: flex;
    align-items: center;
}

.list-icon {
    background: url(../../assets/icon/list-icon.png) no-repeat;
}

.list-icon.icon-search {
    width: 30px;
    height: 30px;
    display: block;
    background-position: -20px -22px;
    margin-left: 8px;
}

.list-icon.icon-menu {
    width: 35px;
    height: 32px;
    /* display: block; */
    background-position: -304px -91px;
}

.cart {
    min-width: 44px;
    height: 44px;
    border-radius: 35px;
    background: #ff9300;
    position: relative;
    margin-left: 20px;
}

.cart .icon-cart {
    width: 24px;
    height: 24px;
    background: transparent url(../../assets/icon/icon-cart.png) no-repeat;
    background-position: center;
    background-size: 24px 24px;
    position: absolute;
    top: 30%;
    left: 18%;
}

.cart:hover {
    cursor: pointer;
}

.btn-login {
    height: 32px;
    background: #ff9300;
    border-radius: 15px;
    padding: 10px;
    font-weight: 700;
    min-width: 92px;
}

.btn-login:hover {
    cursor: pointer;
}

.settings {
    min-width: 44px;
    height: 44px;
    border-radius: 50%;
    /* border-radius: 18px; */
    background: #fff;
    position: relative;
}

.settings:hover {
    cursor: pointer;
}

.settings img {
    width: 44px;
    height: 44px;
    border-radius: 50%;
    position: absolute;
}

.settings .list-option {
    width: 200px;
    background: #fff;
    position: absolute;
    top: 115%;
    right: 5%;
    border-radius: 10px;
    box-shadow: 0 0 16px rgb(23 27 42 / 24%);
    z-index: 10;
}

/* .list-option .arrow-list {
    position: absolute;
    background: #fff;
    height: 16px;
    top: -6px;
    right: 8px;
    transform: rotate(45deg);
    border-left: 8px solid transparent;
    border-right: 8px solid transparent;
} */

.list-option .option {
    height: 32px;
    padding: 0 8px;
    cursor: pointer;
    border-radius: 4px;
}

.list-option::before {
    border-bottom: 8px solid #fff;
    border-left: 8px solid transparent;
    border-right: 8px solid transparent;
    position: absolute;
    top: -6px;
    right: 8px;
    content: "";
}

.list-option .option:hover {
    background: #d2f0f0;
}

.option-icon {
    width: 32px;
    height: 32px;
}

.icon-logout {
    width: 24px;
    height: 24px;
    background: transparent url(../../assets/icon/logout.png) no-repeat;
    background-position: center;
    background-size: 24px 24px;
}

.icon-user {
    width: 24px;
    height: 24px;
    background: transparent url(../../assets/icon/user.png) no-repeat;
    background-position: center;
    background-size: 24px 24px;
}

.icon-change-password {
    width: 24px;
    height: 24px;
    background: transparent url(../../assets/icon/reset-password.png) no-repeat;
    background-position: center;
    background-size: 24px 24px;
}

.icon-shopping-list {
    width: 24px;
    height: 24px;
    background: transparent url(../../assets/icon/shopping-list.png) no-repeat;
    background-position: center;
    background-size: 24px 24px;
}

.icon-history-shopping {
    width: 24px;
    height: 24px;
    background: transparent url(../../assets/icon/history-shopping.png) no-repeat;
    background-position: center;
    background-size: 24px 24px;
}

.header-bottom {
    box-shadow: 0 1px 4px rgba(0, 0, 0, 0.15);
}

.header-menu {
    background: #ff9300;
    min-width: 230px;
    max-width: 230px;
    height: 42px;
    position: relative;
}

.title-menu p {
    width: calc(100% -35px);
    margin-left: 10px;
    text-transform: uppercase;
    font-weight: bolder;
}

.title-menu p {
    width: calc(100% -35px);
    margin-left: 10px;
    text-transform: uppercase;
    font-weight: bolder;
}

.header-bottom-right {
    width: calc(100% -230px) !important;
    justify-content: space-between;
}

.title-item {
    padding-left: 20px;
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
}

.item-right {
    max-width: calc(980px / 3);
    padding: 0 20px;
    /* font-size: 13px; */
    border-left: 1px solid #eee;
    height: 20px;
    position: relative;
    cursor: pointer;
}

.item-right:first-child {
    border: none;
}

.item-right:hover .title-item {
    color: #ff9300;
}

.pulse-icon {
    position: absolute;
    display: inline-block;
    top: 3px;
    left: 15px;
}

.pulse-icon .icon-wrap {
    width: 6px;
    height: 6px;
    border-radius: 6px;
    background: #ff9300;
    position: absolute;
    top: 3px;
    left: 3px;
}

.pulse-icon .elements {
    position: absolute;
    top: 0px;
    left: 0px;
}

.elements .pulse {
    position: absolute;
    -webkit-animation: pulse-wave 1s linear infinite both;
    animation: pulse-wave 1s linear infinite both;
    border-radius: 50%;
    border: solid 1px #ff9300;
    width: 14px;
    height: 14px;
    top: -1px;
    left: -1px;
}

@keyframes pulse-wave {
    0% {
        opacity: 0;
        -webkit-transform: scale(1);
        transform: scale(1);
    }

    50% {
        opacity: 1;
        -webkit-transform: scale(2);
        transform: scale(2);
    }

    100% {
        opacity: 0;
        -webkit-transform: scale(2.7);
        transform: scale(2.7);
    }
}

.list-holder {
    flex-wrap: wrap;
    width: 30%;
    height: 100%;
    padding-right: 10px;
}


.list-holder .holder-title {
    color: #ff9300;
    font-weight: 700;
    text-transform: uppercase;
    width: 100%;
    display: block;
    line-height: 2;
}

.item-holder {
    width: 100%;
    position: relative;
}

.item-holder .holder-content {
    /* font-size: 13px; */
    line-height: 2;
    position: relative;
    display: flex;
    flex-direction: row;
    align-items: center;
    cursor: pointer;
}

.item-holder .holder-content:hover {
    color: #ff9300;
}

.holder-content .icon-arrow-right {
    width: 9px;
    height: 9px;
    background-image: url(../../assets/icon/right-arrow.png);
    background-size: 9px 9px;
    margin-left: 8px;
}

.holder-content:hover .icon-arrow-right {
    background-image: url(../../assets/icon/arrow.png);
}

.list-category {
    position: absolute;
    top: 43px;
    left: 0;
    width: 100%;
    max-height: 417px;
    z-index: 2;
    background: #fff;
    display: none;
}

.list-category.home {
    display: block;
}

.header-menu:hover .list-category {
    display: block;
}

.cate-item .menu-hover {
    background: #fff;
    border: solid 1px #dedede;
    position: absolute;
    z-index: 2;
    top: 0;
    left: 230px;
    width: calc(1200px - 246px);
    height: 420px;
    overflow: auto;
    font-size: 14px;
    padding: 10px;
    visibility: hidden;
    opacity: 0;
    -webkit-transform: translate(20px, 0);
    transform: translate(20px, 0);
    -webkit-transition: opacity 0.7s, -webkit-transform .7s;
    transition: opacity 0.7s, -webkit-transform .7s;
    transition: opacity 0.7s, transform .7s;
    transition: opacity 0.7s, transform 0.7s, -webkit-transform .7s;
}

.cate-item:hover .menu-hover {
    visibility: visible;
    opacity: 1;
    -webkit-transform: translate(0, 0);
    transform: translate(0, 0);
    color: #000;
}

.cate-item:hover {
    color: #fff;
    background: #ff9300;
}

.item {
    justify-content: space-between;
    padding: 0 10px;
    height: 51px;
    cursor: pointer;
}

/* .item:hover {
    color: #fff;
    background: #ff9300;
} */

.cate-icon {
    margin-right: 6px;
    width: 28px;
    height: 28px;
    background-size: 28px 28px;
}

.cate-icon.laptop-new {
    background-image: url(../../assets/icon/ic_laptop.png);
}

.cate-icon.laptop-used {
    background-image: url(../../assets/icon/ic_lap_used.png);
}

.icon-arrow-right {
    width: 11px;
    height: 11px;
    background-image: url(../../assets/icon/arrow-right.png);
    background-size: 11px 11px;
}

.cart .amount {
    width: 20px;
    height: 20px;
    border-radius: 50%;
    background: white;
    color: red;
    top: 5px;
    right: 5px;
    position: absolute;
    font-weight: 700;
    font-size: 16px;
    text-align: center;
    line-height: calc(20/17);
}
.title {
    margin-right: 16px;
    font-size: 18px;
    font-weight: 600;
    line-height: 35px;
    color: #fff;
}
</style>