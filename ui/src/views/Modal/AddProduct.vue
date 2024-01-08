<template>
    <form @keydown.esc="closeForm" class="modal-detail-wrapper">

        <div class="modal-detail">

            <div class="buttons-header">

                <el-tooltip effect="dark" :content="Resource.Tooltip.Help" placement="top">
                    <div class="button help"></div>
                </el-tooltip>

                <el-tooltip effect="dark" :content="Resource.Tooltip.Close" placement="top">
                    <div @mousedown="closeForm" class="button close"></div>
                </el-tooltip>

            </div>

            <div class="modal-detail-title">{{ Resource.Label.AddProduct }}</div>

            <div class="search-box flex-row">
                <input @keypress.enter="search" v-model="keyword" :placeholder="Resource.Placehoder.Search"
                    class="search-input" type="text" @focus="showIconX = true" @blur="showIconX = false" />
                <div v-show="keyword && showIconX" @mousedown="keyword = ''" class="icon-X"></div>
                <div @click="search" class="search-input-icon"></div>
            </div>
            <BaseLoading v-if="isLoading" />
            <div class="modal-detail-content" v-else>
                <div class="box" v-show="selectedProducts.length > 0">
                    <div class="table flex-column">
                        <div class="title">
                            <p>{{ Resource.Label.ListProductSelected }}</p>
                        </div>
                        <table cellpadding="0" cellspacing="0">
                            <thead>
                                <tr>
                                    <th class="col-360">
                                        <div class="flex-row">
                                            <p class="title-table">{{ Resource.Label.ProductName }}</p>
                                        </div>
                                    </th>
                                    <th class="col-120" v-if="!isImport">
                                        <div class="flex-row justify-content">
                                            <p class="title-table ">{{ Resource.Label.Quantity }}</p>
                                        </div>
                                    </th>
                                    <th class="col-120" v-if="!isImport">
                                        <div class="flex-row justify-content">
                                            <p class="title-table">{{ Resource.Label.PriceSell }}</p>
                                        </div>
                                    </th>
                                    <th class="col-60">

                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(product, index) in selectedProducts" :key="index">
                                    <td>
                                        <span :title="product.productName">{{ product.productName }}</span>
                                    </td>
                                    <td class="col-center" v-if="!isImport">
                                        <InputNumber :transmissionNumber="product.amount" :decimalPlaces="0"
                                            :nameProperty="product.productID" :max="product.quantity" @update="updateValue"
                                            :isCart="true" />
                                    </td>
                                    <td class="col-center" v-if="!isImport">
                                        {{ formatMoney(product.price) }}
                                    </td>
                                    <td class="col-center">
                                        <span class="delete" @click="deleteProduct(product.productID)">{{
                                            Resource.Button.Delete }}</span>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="w-full d-flex flex-wrap">
                    <ProductItem v-for="product in listProduct" :key="product.productID" :initProduct="product"
                        :notClick="true" @click="selectProduct(product)" v-show="!selectedProducts.some(
                            (obj) => obj.productID == product.productID
                        )" />
                </div>
                <div class="paging d-flex">
                    <BasePaging :pageSize="pageSize" :pageNow="pageNumber" :totalCount="totalCount" @prePage="goToPrePage"
                        @nextPage="goToNextPage" @updatePageNow="updatePageNow" :isManager="false" />
                </div>
            </div>

            <div class="modal-footer">
                <div class="flex-row flex-end">

                    <BaseButton @mousedown="closeForm" class="sub-button btn">
                        {{ Resource.Button.Close }}
                    </BaseButton>

                    <BaseButton @click="addProduct" class="main-button-red btn ml-10" v-show="selectedProducts.length>0">
                        {{ Resource.Button.AddProduct }}
                    </BaseButton>

                </div>
            </div>
        </div>
    </form>
</template>
    
<script>
import axios from 'axios';
import Resource from '@/utils/resource';
import Enum from '@/utils/enum';
import ProductItem from '@/components/base/ProductItem.vue';
import BaseLoading from '@/components/base/BaseLoading.vue';
import BasePaging from '@/components/base/BasePaging.vue';
import BaseButton from '@/components/base/BaseButton.vue';
import InputNumber from '@/components/base/BaseInputNumber.vue'
export default {
    name: 'AddProduct',
    components: {
        ProductItem, BaseLoading, BasePaging, BaseButton, InputNumber
    },
    props: ["closeFormAddProduct", "isImport"],
    data() {
        return {
            Resource: Resource,
            showIconX: false, // cờ thể hiện trạng thái ẩn hiện của iconX

            isLoading: false, // cờ điều khiển bật tắt trạng thái loading

            productNow: {}, //sản phẩm đang được chọn hiện tại 

            listProduct: [], // danh sách sản phẩm đang được hiển thị

            selectedProducts: [], // danh sách sản phẩm đang được chọn

            keyword: "", // từ khóa để tìm kiếm sản phẩm

            totalCount: 0, // tổng số sản phẩm

            pageNumber: Enum.FirstPage, // trang đang hiển thị

            pageSize: Enum.PageSize, // số sản phẩm hiển thị trong 1 trang

            listSize: Enum.ListSize, // danh sách gía trị số sản phẩm hiển thị trong 1 trang

            // listGift : []

        }
    },
    computed: {
        
    },

    watch: {
    
    },

    methods: {

        formatMoney(money) {
            return money.toLocaleString('vi-VN') + ' đ';
        },

        getNewPrice(oldPrice, saleoff) {
            let money = oldPrice * (100 - saleoff) / 1000000;
            money = money - money % 1;
            money = money * 10000;
            return money;
        },

        updateValue(value, productID) {
            if (value == 0) {
                this.deleteProduct(productID);
            }
            else {
                this.selectedProducts.find(
                    (p) => {
                        if(p.productID == productID){
                            p.amount = value;
                        }
                    }
                )
            }
        },

        async addProduct(){
            this.isLoading = true;
            for(let i = 0; i<this.selectedProducts.length; i++){
                await this.getListGiftByID(this.selectedProducts[i]);
            }
            
            this.$emit("setListProduct", this.selectedProducts);
            this.isLoading = false;
            this.closeFormAddProduct();
        },

        /**
         * Lấy danh sách quà theo ID sản phẩm
         * @param {*} productID : ID của sản phẩm muốn lấy danh sách quà tặng kèm
         * Author: HAQUAN(29/08/2023) 
         */
         async getListGiftByID(product) {
            try {
                let url = `Gift/gifts${product.productID}`;
                await axios.get(url)
                    .then((response) => {
                        product.listGift = response.data;
                        product.discount = 0;
                        // this.listGift = response.data;
                        // return response.data;
                    })

                    .catch((error) => {
                        console.log(error);
                        this.handleErrorResponse(error);
                    })
            } catch (error) {
                console.log(error);
            }
        },

        /**
         * Lấy dữ liệu sản phẩm 
         * Author: HAQUAN(26/08/2023)
         */
        async getData() {
            try {
                this.isLoading = true;
                let url = `Product/filter?byManager=${false}&keyword=${this.keyword}&status=${Enum.StatusProduct.Selling}&pageSize=${this.pageSize}&pageNumber=${this.pageNumber}`;
                if(this.isImport){
                    url = `Product/filter?byManager=${false}&keyword=${this.keyword}&pageSize=${this.pageSize}&pageNumber=${this.pageNumber}`;
                }
                await axios.get(url)

                    .then((response) => {
                        this.listProduct = response.data.data;
                        this.totalCount = response.data.totalCount;
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

        /**
         * Tìm kiếm sản phẩm 
         * Author: HAQUAN(28/10/2022)
         */
        search() {
            try {
                this.pageNumber = Enum.FirstPage;
                this.getData();
            } catch (error) {
                console.log(error);
            }
        },

        selectProduct(product) {
            let item = { ...product };
            if(this.isImport){
                item.priceImport = 0;
                item.quantity = 1;
            }
            else {
                item.price = this.getNewPrice(product.price, product.discount);
                item.amount = 1;
            }
            this.selectedProducts.push(item);
        },

        deleteProduct(productID) {
            if (this.selectedProducts.find((item) => item.productID === productID)) {
                this.selectedProducts = this.selectedProducts.filter(
                    (p) => p.productID !== productID
                );
            }
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

        /**
         * Đóng form 
         * Author: HAQUAN(29/08/2023) 
         */
        closeForm() {
            this.closeFormAddProduct();
        },

    },
    created() {
        this.getData();
    }
}
</script>
    
<style scoped>
.modal-detail-wrapper {
    top: 0;
    left: 0;
    width: 100vw;
    height: 100vh;
    position: fixed;
    background-color: rgba(0, 0, 0, 0.25);
    z-index: 5;
}

.modal-detail {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    width: 1200px;
    height: fit-content;
    border-radius: 4px;
    z-index: 5;
    background-color: #fff;
    box-shadow: 0 0 16px rgb(23 27 42 / 24%);
}

.modal-detail-content::-webkit-scrollbar {
    width: 8px;
    border-radius: 10px;
    background: #fff;
}

.modal-detail-content::-webkit-scrollbar:hover {
    cursor: pointer;

}

.modal-detail-content::-webkit-scrollbar-thumb {
    border-radius: 10px;
    background-color: #b5b5b5;
}

.modal-detail::-webkit-scrollbar-thumb:hover {
    background-color: #8f8f8f;
}

.modal-detail ::placeholder {
    color: #bbbbbb;
}

.buttons-header {
    position: absolute;
    top: 14px;
    right: 16px;
}

.buttons-header .button {
    cursor: pointer;
    display: inline-block;
    padding: 10px 8px;
}

.buttons-header .button::before {
    content: "";
    display: block;
    background: transparent url(https://cegovapp.misacdn.net/rc/cegov/img/sprites.06b14dc5.svg) no-repeat;
    height: 20px;
    width: 20px;
}

.buttons-header .button.help:before {
    background-position: -50px -50px;
}

.buttons-header .button.close:before {
    background-position: -242px -26px;
}

.buttons-header .button.close:hover::before {
    background-position: -266px -26px;
}

.modal-detail-title {
    font-size: 20px;
    padding: 6px 0 6px 24px;
    font-weight: 700;
    min-height: 24px;
}

.modal-detail-content {
    padding: 12px 24px 0;
    position: relative;
    max-height: 505px;
    overflow: auto;
    position: relative;
}

.w-full {
    width: 100%;
    height: 100%;
}

.search-box {
    width: 265px;
    background: #fff;
    padding: 0;
    margin-right: 10px;
    border: 1px solid #e0e0e0;
    border-radius: 3.5px;
    display: flex;
    align-items: center;
    margin-left: 20px;
}

.search-input {
    border: none;
    padding: 9px 12px;
    background: 0 0;
    min-height: 34px;
    border-radius: 3.5px;
    font-size: 14px;
    width: 100%;
}

.search-input:focus {
    outline: none;
}

.search-box:focus-within {
    border-color: #2979ff;
}

.icon-X {
    background: url(../../assets/icon/icon-x.png);
    background-size: 22px 22px;
    height: 22px;
    width: 22px;
    position: relative;
    left: -16px;
    margin-left: 12px;
    cursor: pointer;
}

.paging {
    align-items: center;
    justify-content: center;
}

.modal-footer {
    padding: 12px 24px;
    border-top: 1px solid #e0e0e0;
}

.search-input-icon {
    height: 24px;
    width: 24px;
    position: relative;
    right: 8px;
    cursor: pointer;
}

.search-input-icon::before {
    content: "";
    position: absolute;
    background: transparent url(https://cegovapp.misacdn.net/r/cegov/img/sprites.06b14dc5.svg) no-repeat;
    background-position: -289px -24px;
    height: 24px;
    width: 24px;
    top: 0;
    left: 0;
}

table {
    width: 100%;
}

.table {
    position: relative;
    overflow: auto;
    border-bottom: 1px solid #ddd;
    height: 100%;
}

.table thead {
    position: sticky;
    top: 0;
    z-index: 3;
}

.table th {
    font-weight: 600;
    background: #fff;
    border-right: 1px solid #ddd;
    border-bottom: 1px solid #ddd;
    z-index: 2;
    height: 48px;
    text-align: left;
    padding: 0 12px;
    white-space: nowrap;
}

.table td {
    line-height: calc(38/14);
    padding: 0 12px;
    height: 43px;
    vertical-align: middle;
    background-color: #fff;
    border-right: 1px solid #ddd;
    border-bottom: 1px solid #ddd;
    text-overflow: ellipsis;
    white-space: nowrap;
    overflow: hidden;
}

.table th:last-child {
    border-right: none;
}

.table td:last-child {
    border-right: none;
}

.table tr:last-child {
    border-bottom: none;
}

/* .col-360 {
    min-width: 360px;
    max-width: 360px;
} */

.col-120 {
    min-width: 120px;
    max-width: 120px;
}

.col-60 {
    min-width: 60px;
    max-width: 60px;
}

.table .title {
    padding-right: 24px;
    font-size: 16px;
    font-weight: 600;
}

td .delete:hover {
    color: red;
    cursor: pointer;
}</style>
    