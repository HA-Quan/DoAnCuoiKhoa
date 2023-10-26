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

            <div class="modal-detail-title">{{ isTitle }}</div>

            <div class="search-box flex-row">
                <input @keypress.enter="search" v-model="keyword" :placeholder="Resource.Placehoder.Search"
                    class="search-input" type="text" @focus="showIconX = true" @blur="showIconX = false" />
                <div v-show="keyword && showIconX" @mousedown="keyword = ''" class="icon-X"></div>
                <div @click="search" class="search-input-icon"></div>
            </div>
            <BaseLoading v-if="isLoading" />
            <div class="modal-detail-content" v-else>

                <div class="w-full d-flex flex-wrap">
                    <ProductItem v-for="product in listProduct" :key="product.productID" :initProduct="product" />
                </div>
            </div>

            <div class="modal-footer">
                <div class="flex-row flex-end">

                    <BaseButton @mousedown="closeForm" class="sub-button btn">
                        {{ Resource.Button.Close }}
                    </BaseButton>

                    <BaseButton @click="addProduct" class="main-button btn ml-10">
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

export default {
    name: 'AddProduct',
    components: {
        ProductItem, BaseLoading, BasePaging
    },
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

            productIdUpdate: null, // id sản phẩm muốn cập nhật

            listProductID: [], // danh sách ID của các sản phẩm 

            listSize: Enum.ListSize, // danh sách gía trị số sản phẩm hiển thị trong 1 trang


        }
    },
    computed: {
        totalMoney: function () {
            let total = 0;
            this.cart.forEach(element => {
                let money = element.price;
                total += money * element.amount;
            });
            return total;
        }
    },

    watch: {
        'decreaseMoney': {
            handler: function (newVal) {
                if (newVal != undefined) {
                    this.orderModel.order.total = this.totalMoney - newVal;
                }
            }
        },
    },

    methods: {
        /**
         * Lấy dữ liệu sản phẩm 
         * Author: HAQUAN(26/08/2023)
         */
        async getData() {
            try {
                this.isLoading = true;
                let url = `Product/filter?byManager=${false}&keyword=${this.keyword}&pageSize=${this.pageSize}&pageNumber=${this.pageNumber}`;
                await axios.get(url)

                    .then((response) => {
                        console.log(response);
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

    },
    created() {
        this.cart = JSON.parse(localStorage.getItem('cart'));
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
    width: 1024px;
    height: fit-content;
    border-radius: 4px;
    z-index: 5;
    background-color: #fff;
    box-shadow: 0 0 16px rgb(23 27 42 / 24%);
}

.modal-detail-content::-webkit-scrollbar {
    width: 8px;
    /* height: 8px; */
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
    padding: 24px 0 6px 24px;
    font-weight: 700;
    min-height: 24px;
}

.modal-detail-content {
    padding: 24px 24px 0;
    position: relative;
    max-height: 530px;
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
    background: url(../assets/icon/icon-x.png);
    background-size: 22px 22px;
    height: 22px;
    width: 22px;
    position: relative;
    left: -16px;
    margin-left: 12px;
    cursor: pointer;
}

.modal-footer {
    padding: 12px 24px;
    border-top: 1px solid #e0e0e0;
}</style>
    