<template>
    <div class="filter-page">
        <BaseLoading v-if="isLoading"/>
        <div class="container" v-else>
            <ul class="product-category d-flex align-items space-around">
                <li v-for="(cate, index) in Const.CategoryDemand" :key="index">
                    <div class="icon">
                        <div :class="cate.Icon"></div>
                    </div>
                    <p class="text">
                        {{ cate.Text }}
                    </p>
                </li>
            </ul>
            <div class="filter-product">
                <div class="filter-box">
                    <div class="item-filter d-flex align-items">
                        <div class="filter-left">
                            {{ Resource.Text.FilterPrice }}
                        </div>
                        <div class="filter-right d-flex align-items flex-wrap">
                            <div class="list-item">
                                <span v-for="priceFilter in listPrice" :key="priceFilter.id" class="item">
                                    {{ priceFilter.name }} ({{ priceFilter.count }})
                                </span>
                            </div> 
                        </div>
                    </div>
                    <div class="item-filter d-flex align-items">
                        <div class="filter-left">
                            {{ Resource.Text.FilterCriteria }}
                        </div>
                        <div class="filter-right d-flex align-items flex-wrap">
                            <div class="list-item" v-for="(criteriaFilter, index) in listCriteria" :key="index"
                                :class="{'hidden' : criteriaFilter.selectModels.length == 0}">
                                <span class="item">
                                    <span style="margin-right: 20px;">{{ criteriaFilter.title }}</span>
                                    <i class="icon-arrow-down"></i>
                                </span>
                                <ul>
                                    <li v-for="subCriteria in criteriaFilter.selectModels" :key="subCriteria.id">
                                        <span class="content-sub">{{ subCriteria.name }}</span>
                                        <span> ({{ subCriteria.count }})</span>
                                         
                                    </li>
                                </ul>
                            </div> 
                        </div>
                    </div>
                    <div class="item-filter d-flex align-items">
                        <div class="filter-left">
                            {{ Resource.Text.SortBy }}
                        </div>
                        <div class="filter-right d-flex align-items flex-wrap">
                            <div :class="['item-sort', {'active' : sortItem.Value == sort}]" 
                            v-for="(sortItem, index) in Const.ListSort" :key="index"  @click="sortProduct(sortItem.Value)" > 
                                <span>{{ sortItem.Label }}</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="product-list d-flex flex-wrap">
                <ProductItem v-for="product in listProduct" :key="product.productID" :initProduct="product"/>
            </div>
            <div class="paging d-flex">
                <BasePaging :pageSize="pageSize" :pageNow="pageNumber" :totalCount="totalCount"
                @prePage="goToPrePage" @nextPage="goToNextPage" @updatePageNow="updatePageNow" :isManager="false" />
            </div>
        </div>
    </div>
</template>
<script>
import axios from 'axios';
import Const from '@/utils/const';
import Resource from '@/utils/resource';
import Enum from '@/utils/enum';
import BaseLoading from '@/components/base/BaseLoading.vue';
import ProductItem from '@/components/base/ProductItem.vue';
import BasePaging from '@/components/base/BasePaging.vue';
export default {
    name: "FilterPage",
    components: {
        BaseLoading, ProductItem, BasePaging
    },
    data(){
        return{
            Const: Const,
            Resource: Resource,

            isLoading: false, // cờ điều khiển bật tắt trạng thái loading

            pageNumber: Enum.FirstPage, // trang đang hiển thị

            pageSize: Enum.PageSize, // số sản phẩm hiển thị trong 1 trang
            filter: { 
                category: Const.GuidEmpty,
                chip: Const.GuidEmpty,
                memory: Const.GuidEmpty,
                storage: Const.GuidEmpty,
                cardType: "",
                display: Const.GuidEmpty,
                trademark: "",
                demand: Enum.DemandType.All,
                priceRange: Enum.PriceRange.All,
            },

            sort: Enum.Sort.Default, // điều kiện sắp xếp

            listProduct: [], // danh sách sản phẩm đang được hiển thị
            listCriteria: [],
            listPrice: [],

            totalCount: 0, // tổng số sản phẩm
        }
    },
    methods:{
        /**
         * Lấy dữ liệu sản phẩm 
         * Author: HAQUAN(26/08/2023)
         */
         async getData() {
            try {
                this.isLoading = true;
                let url = `Product/filter?byManager=${false}&categoryID=${this.filter.category}&chipID=${this.filter.chip}&memoryID=${this.filter.memory}&storageID=${this.filter.storage}&cardType=${this.filter.cardType}&displayID=${this.filter.display}&trademark=${this.filter.trademark}&sort=${this.sort}&demand=${this.filter.demand}&priceRange=${this.filter.priceRange}&pageSize=${this.pageSize}&pageNumber=${this.pageNumber}`;
                await axios.get(url)

                    .then((response) => {
                        console.log(response);
                        this.listProduct = response.data.data;
                        this.totalCount = response.data.totalCount;
                        this.configCriteria(response.data.listCriteriaModel);
                    })

                    .catch((error) => {
                        console.log(error);
                    })

                    .finally(() => {
                        this.isLoading = false;
                        if (this.listProduct.length == 0 && this.pageNumber > Enum.FirstPage) { // nếu không lấy được sản phẩm nào thì
                            this.pageNumber = Enum.FirstPage;                // quay về trang đầu tiên
                            this.getData();
                        }
                    });
            } catch (error) {
                console.log(error);
            }
        },  
        configCriteria(valueList){
            this.listPrice = valueList[0].selectModels;
            this.listCriteria = [];
            for (let index = 1; index < valueList.length; index++) {
                this.listCriteria.push(valueList[index]);
            }
            console.log(this.listCriteria);
        },    

        sortProduct(value){
            this.sort = value;
            this.getData(); 
        },

        /**
         * Chuyển đến trang trước
         * Author: HAQUAN(28/10/2022)
         */
         goToPrePage() {
            try {
                if (this.pageNumber > Enum.FirstPage) {
                    this.pageNumber--;
                    this.getData();
                }
            } catch (error) {
                console.log(error);
            }
        },

        /**
         * Chuyển đến trang tiếp theo
         * Author: HAQUAN(28/10/2022)
         */
        goToNextPage() {
            try {
                let totalPage = Math.ceil(this.totalCount / this.pageSize);
                if (this.pageNumber < totalPage) {
                    this.pageNumber++;
                    this.getData();
                }
            } catch (error) {
                console.log(error);
            }
        },  

        /**
         * Hàm chuyển đến trang chỉ định
         * Author: HAQuan (09/08/2022)
         */
         updatePageNow(pageIsSelected) {
            this.pageNumber = pageIsSelected;
            this.getData();
        },

    },
    created(){
        if(this.$route.query.categoryID != undefined){
            this.filter.category = this.$route.query.categoryID;
        }
        else if (this.$route.query.demandType != undefined){
            this.filter.demand = this.$route.query.demandType;
        }
        this.getData(); 
    }
    
}
</script>
<style scoped>
.product-category{
    margin: 15px 0;
    background: #f1f1f1;
    padding: 10px 0;
    text-align: center;
}
.product-category li{
    width: calc(100% / 8);
    cursor: pointer;
}
li::marker{
    display: none;
    content: "";
}
.product-category li .icon{
    align-items: center;
    border-radius: 50%;
    display: flex;
    justify-content: center;
    height: 56px;
    margin: 0 auto 6px;
    width: 56px;
    background-color: #fff;
}
.icon-office{
    height: 34px;
    width: 34px;
    background: url(../assets/icon/office.png);
    background-size: 34px 34px;
}
.icon-gaming{
    height: 34px;
    width: 34px;
    background: url(../assets/icon/gaming.png);
    background-size: 34px 34px;
}
.icon-graphics{
    height: 34px;
    width: 34px;
    background: url(../assets/icon/dohoakythuat.png);
    background-size: 34px 34px;
}
.icon-high-class{
    height: 34px;
    width: 34px;
    background: url(../assets/icon/laptopmongnhe.png);
    background-size: 34px 34px;
}
.product-category:hover .icon:hover{
    background-color: #feae02;
}
.product-category .filter-product{
    background: #fff;
    padding: 10px;
}
.filter-product .filter-box .item-filter{
    margin-bottom: 10px;
}
.filter-product .filter-box .filter-left {  
    width: 125px;
    margin-right: 10px;
    font-weight: 700;
}
.product-category .filter-box .filter-right {
    width: calc(100% - 120px);
}
.filter-product .filter-box .list-item {
    position: relative;
    margin-bottom: 5px;
}
.filter-box .list-item .item{
    margin-right: 10px;
    font-size: 14px;
    display: inline-block;
    cursor: pointer;
    color: #222;
    background: #f5f5f5;
    border: 1px solid #cdcdcd;
    border-radius: 8px;
    height: 38px;
    line-height: 38px;
    padding: 0 8px;
}
.list-item .item .icon-arrow-down{
    position: relative;
}
.list-item .item .icon-arrow-down::before{
    position: relative;
    content: "";
    width: 12px;
    height: 12px;
    background-image: url(../assets/icon/arrow-right.png);
    transform: rotate(90deg);
    background-size: 12px 12px;
    position: absolute;
    display: block;
    top: 5px;
    right: 0;
}
.filter-box .list-item ul {
    position: absolute;
    z-index: 11;
    background-color: #fff;
    padding: 10px 0;
    width: 200px;
    -webkit-box-shadow: 0 0 5px 0 #969696;
    box-shadow: 0 0 5px 0 #969696;
    left: 0;
    top: 100%;
    -webkit-transform: scale(0);
    transform: scale(0);
    -webkit-transition: .5s all;
    transition: .5s all;
    -webkit-transform-origin: top left;
    transform-origin: top left;
    padding: 20px 10px;
}
.list-item ul li{
    cursor: pointer;
}
.list-item ul li .content-sub:hover{
    color: #ff9300;
}
.filter-box .list-item:hover ul {
    -webkit-transform: scale(1);
    transform: scale(1);
    -webkit-transition: .5s all;
    transition: .5s all;
    -webkit-transform-origin: top right;
    transform-origin: top right;
}
.item-filter .list-item .item:hover {
    background: #ff9300 !important;
    color: #fff !important;
}
.filter-right .item-sort {
    margin-right: 10px;
    font-size: 14px;
    display: inline-block;
    cursor: pointer;
    color: #0f71d3;
    border-radius: 8px;
    height: 38px;
    line-height: 38px;
    padding: 0 8px;
}
.filter-right .item-sort.active {
    background: #ff9300;
    color: #fff;
}
.filter-page .loading{
    position: fixed;
}
.item-sort:hover{
    color: #ff9300
}
.product-list{
    margin: 12px 0;
    margin-right: -12px;
}
.paging{
    align-items: center;
    justify-content: center;
}
</style>