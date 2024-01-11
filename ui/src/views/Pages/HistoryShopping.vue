<template>
    <BaseLoading v-if="isLoading" />
    <div v-else class="body flex-column " style="outline: none;" tabindex="0">
        <div class="title-box">{{ Resource.Label.HistoryShopping }}</div>
        <div class="toolbar flex-row">
            <div class="toolbar-left flex-row">
                <div class="d-flex mr-10">
                    <BaseButton v-for="(item, index) in Const.StatusOrder" :key="index" @click="updateStatus(item.Value)"
                        :class="['flex-row btn btn-tag', { 'selected': filter.statusOrder == item.Value }]">
                        <div class="text-button">{{ item.Label }}</div>
                    </BaseButton>
                </div>
            </div>
            <div class="toolbar-right flex-row">
                <div v-show="isFilter" @click="filterOrderDefault" class="cancel-seleted pointer">{{
                    Resource.Text.StopFilter }}
                </div>

                <BaseButton @click="toggleFilterBox" class="filter flex-row btn">
                    <div class="icon-filter" :class="{ 'active': isFilter }"></div>
                    <div class="text-button">{{ Resource.Button.Filter }}</div>
                </BaseButton>

                <div class="filter-box" v-show="isShowFilterBox">
                    <span class="arrow-filter-box"></span>

                    <div class="box-header">
                        <span class="title">{{ Resource.Label.FilterOrder }}</span>
                        <div @click="closeFilterBox" style="left: 0px;" class="icon-X"></div>
                    </div>

                    <div class="box-content flex-column">

                        <div class="row-box d-flex">
                            <div class="flex mr-10">
                                <label for="">{{ Resource.Label.DeliveryMethod }}</label>
                                <div class="flex mt-10">
                                    <BaseCombobox :hasIcon="false" :isReadonly="true" :items="Const.DeliveryMethodFilter"
                                        :initItem="configDeliveryMethod(filter.deliveryMethod)" fieldName="Label"
                                        @getValue="setDeliveryMethodFilter" />
                                </div>
                            </div>
                            <div class="flex">
                                <label for="">{{ Resource.Label.PaymentMethod }}</label>
                                <div class="flex mt-10">
                                    <BaseCombobox :hasIcon="false" :isReadonly="true" :items="Const.PaymentMethodFilter"
                                        :initItem="configPaymentMethod(filter.paymentMethod)" fieldName="Label"
                                        @getValue="setPaymentMethodFilter" />
                                </div>
                            </div>
                        </div>

                        <div class="row-box d-flex">
                            <div class="flex mr-10">
                                <label for="">{{ Resource.Label.TimeStart }}</label>
                                <div class="flex mt-10">
                                    <el-date-picker v-model="filter.timeStart" type="date"
                                        :placeholder="Resource.Placehoder.TimeStart" :format="Const.DateFormat"
                                        value-format="YYYY-MM-DD" :class="[{ 'input-error': errors.timeStart }]"
                                        style="width: 100%;" @change="validateTimeStart()">
                                    </el-date-picker>
                                    <div class="error-text">{{ errors.timeStart }}</div>
                                </div>
                            </div>
                            <div class="flex">
                                <label for="">{{ Resource.Label.TimeEnd }}</label>
                                <div class="flex mt-10">
                                    <el-date-picker v-model="filter.timeEnd" type="date"
                                        :placeholder="Resource.Placehoder.TimeEnd" :format="Const.DateFormat"
                                        value-format="YYYY-MM-DD" :class="[{ 'input-error': errors.timeEnd }]"
                                        style="width: 100%;" @change="validateTimeEnd()">
                                    </el-date-picker>
                                    <div class="error-text">{{ errors.timeEnd }}</div>
                                </div>

                            </div>
                        </div>
                    </div>

                    <div class="box-footer flex-end">
                        <BaseButton @click="closeFilterBox" class="sub-button btn">{{ Resource.Button.Cancel }}</BaseButton>
                        <BaseButton @click="filterOrder" class="main-button btn ml-10">{{ Resource.Button.Apply }}
                        </BaseButton>
                    </div>
                </div>
            </div>
        </div>

        <div class="box list-order">
            <div class="table">
                <div class="order-item" v-for="(orderItem, index) in listOrder" :key="index">
                    <a class="order-content w-100">
                        <div class="row-oneline">
                            <div class="info-order d-flex flex-column">
                                <div class="mb-10">
                                    <span class="fw-bold">{{ Resource.Label.OrderCode }}:</span>
                                    <span>{{ orderItem.order.orderID.toUpperCase() }}</span>
                                </div>
                                <div class="mb-10">
                                    <span class="fw-bold">{{ Resource.Label.DeliveryMethod }}:</span>
                                    <span>{{ configDeliveryMethod(orderItem.order.deliveryMethod) }}</span>
                                </div>
                                <div class="mb-10">
                                    <span class="fw-bold">{{ Resource.Label.DeliveryAddress }}:</span>
                                    <span>{{ orderItem.order.address }}</span>
                                </div>
                                <div class="mb-10">
                                    <span class="fw-bold">{{ Resource.Label.PaymentMethod }}:</span>
                                    <span>{{ configPaymentMethod(orderItem.order.paymentMethod) }}</span>
                                </div>
                                <div class="mb-10">
                                    <span class="fw-bold">{{ Resource.Label.Total }}:</span>
                                    <span>{{ formatMoney(orderItem.order.total) + "VNĐ" }}</span>
                                </div>
                                <div class="mb-10">
                                    <span class="fw-bold">{{ Resource.Label.TimeOrder }}:</span>
                                    <span>{{ convertTime(orderItem.order.createdDate) }}</span>
                                </div>
                                <div style="display: flex;width: 100%;justify-content: center;padding-top: 10px;">
                                    <BaseButton @click="cancelOrder(orderItem.order.orderID)"
                                        v-if="orderItem.order.status == Enum.StatusOrder.NotApproved"
                                        class="btn btn-cancel-order">
                                        {{ Resource.Button.CancelOrder }}
                                    </BaseButton>
                                </div>

                            </div>
                            <div class="order-detail d-flex flex-column">
                                <span class="fw-bold mb-10">{{ Resource.Text.Product }}</span>
                                <div class="list-product" v-for="(item, index) in orderItem.listOrderDetail" :key="index">
                                    <div class="d-flex">
                                        <div class="product-img" @click="goToProduct(item.productID)">
                                            <div class="image">
                                                <img :src="`${Resource.PrefixImage + item.mainImage}`" alt="">
                                            </div>
                                        </div>
                                        <div class="product-info">
                                            <div>
                                                <p class="name-product">
                                                    <span v-show="item.condition != 2">
                                                        [{{ configCondition(item.condition) }}]
                                                    </span>
                                                    <span>{{ item.productName }}</span>
                                                </p>
                                            </div>
                                            <div class="mb-10">
                                                <span class="fw-bold">{{ Resource.Label.PriceSell }}:</span>
                                                <span style="color: #d70018; font-weight: bold;">{{ formatMoney(item.price)
                                                    + "Đ" }}</span>
                                            </div>
                                            <div class="mb-10">
                                                <span class="fw-bold">{{ Resource.Label.Quantity }}:</span>
                                                <span>{{ item.amount }}</span>
                                            </div>
                                            <div
                                                style="padding-top: 10px;">
                                                <BaseButton @click="addRate(item.productID)"
                                                v-if="orderItem.order.status == Enum.StatusOrder.Delivered && item.rateID == Const.GuidEmpty"
                                                    class="btn btn-rate">
                                                    {{ Resource.Button.Rate }}
                                                </BaseButton>
                                                <BaseButton @click="openFormRate(item.rateID)"
                                                v-if="orderItem.order.status == Enum.StatusOrder.Delivered && item.rateID != Const.GuidEmpty"
                                                    class="btn btn-rate">
                                                    {{ Resource.Button.ViewRate }}
                                                </BaseButton>
                                                <BaseButton @click="goToProduct(item.productID)"
                                                v-if="orderItem.order.status == Enum.StatusOrder.Delivered || orderItem.order.status == Enum.StatusOrder.Cancel "
                                                    class="btn btn-buy-repeat">
                                                    {{ Resource.Button.BuyRepeat }}
                                                </BaseButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </a>
                </div>
            </div>
            <BasePaging :listSize="listSize" :pageSize="pageSize" :pageNow="pageNumber" :totalCount="totalCount"
                @setPageSize="setPageSize" @prePage="goToPrePage" @nextPage="goToNextPage" @updatePageNow="updatePageNow" />

        </div>

    </div>

    <FormRate @refreshData="getData" @showToast="showToast" :formMode="formMode" :rateIDInit="rateIDSeleectd" :accountID="accountID"
        :productID="productID" :closeFormDetail="closeFormDetail" v-if="isShowFormRate" />

    <BasePopup @close="closePopup" class="popup-delete" :title="titlePopup" :prefix="prefixPopup" :suffix="suffixPopup"
        :content="nameOrderDelete" v-show="isShowPopup">
        <template #buttons>

            <BaseButton @click="closePopup" class="btn sub-button">
                {{ Resource.Button.No }}
            </BaseButton>

            <BaseButton @click="deleteOrder(); closePopup();" class="main-button-red btn ml-10">
                {{ Resource.Button.Delete }}
            </BaseButton>

        </template>

    </BasePopup>

    <BaseToastMessage v-show="toastMessage.isShowed" :class="`toast-${toastMessage.type} icon-toast-${toastMessage.type}`">

        <template #message>{{ toastMessage.message }}</template>
    </BaseToastMessage>
</template>
<script>
import axios from 'axios';
import Resource from '@/utils/resource.js';
import Enum from '@/utils/enum.js';
import Const from '@/utils/const.js';
import BaseLoading from '@/components/base/BaseLoading.vue';
import BaseCombobox from "../../components/base/BaseCombobox.vue";
import BaseToastMessage from "../../components/base/BaseToastMessage.vue";
import BasePopup from "../../components/base/BasePopup.vue";
import BaseButton from "../../components/base/BaseButton.vue";
import BasePaging from '@/components/base/BasePaging.vue';
import FormRate from '../ManageRate/FormRate.vue';
import CommonFn from '@/utils/commonFuncion';
import moment from 'moment';
import 'moment/locale/vi'
export default {
    name: "HistoryShopping",
    components: {
        BaseLoading, BaseButton, BaseCombobox, BaseToastMessage, BasePopup, BasePaging, FormRate
    },
    data() {
        return {
            Resource: Resource,
            Enum: Enum,
            Const: Const,

            titlePopup: Resource.Title.DeleteOrder, // tiêu đề của popup

            isShowPopup: false, // cờ điền khiển đóng mở cảnh báo 

            isLoading: false, // cờ điều khiển bật tắt trạng thái loading

            isShowFormRate: false,

            isShowFilterBox: false, // cờ điền khiển đóng mở bộ lọc

            listOrder: [], // danh sách đơn hàng thi đua đang được hiển thị

            formMode: '', // chức năng của form

            totalCount: 0, // tổng số đơn hàng

            pageNumber: Enum.FirstPage, // trang đang hiển thị

            pageSize: Enum.PageSize, // số đơn hàng hiển thị trong 1 trang

            nameOrderDelete: null, // tên đơn hàng muốn xóa

            prefixPopup: null, //

            suffixPopup: null,//

            toastMessage: { // cảnh báo
                message: '', // nội dung cảnh báo
                type: '', // loại cảnh báo
                isShowed: false, // cờ điều khiển bật tắt cảnh báo
            },

            listSize: Enum.ListSize, // danh sách gía trị số đơn hàng hiển thị trong 1 trang

            filter: { // danh sách các điều kiện để lọc đơn hàng

            },
            filterDefault: {
                deliveryMethod: '',
                paymentMethod: '',
                statusOrder: Enum.StatusOrder.Delivered,
                timeStart: '',
                timeEnd: ''
            },

            errors: {
                timeStart: '',
                timeEnd: ''
            },

            isFilter: false, // cờ thể hiện có đang lọc hay là không,

            accountID: null,

            productID: '',

            rateIDSeleectd: Const.GuidEmpty,

        };
    },
    methods: {

        goToProduct(productID) {
            this.$router.push({ name: 'ProductDetailPage', query: { productId: productID } });
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

        /**
         * Mở form thêm mới
         * Author: HAQUAN(28/10/2022)
         */
         showFormDetail() {
            this.isShowFormRate = true;
        },

        /**
         * Đóng form thêm 
         * Author: HAQUAN(28/10/2022)
         */
         closeFormDetail() {
            this.isShowFormRate = false;
        },

        /**
         * Lấy dữ liệu đơn hàng 
         * Author: HAQUAN(26/08/2023)
         */
        async getData() {
            try {
                this.isLoading = true;
                let url = `OrderProduct/getByAccountID${this.accountID}?timeStart=${this.filter.timeStart}&timeEnd=${this.filter.timeEnd}&deliveryMethod=${this.filter.deliveryMethod}&paymentMethod=${this.filter.paymentMethod}&status=${this.filter.statusOrder}&pageSize=${this.pageSize}&pageNumber=${this.pageNumber}`;
                await axios.get(url)

                    .then((response) => {
                        this.setInitPage(response);
                    })

                    .catch((error) => {
                        console.log(error);
                        this.handleErrorResponse(error);
                    })

                    .finally(() => {
                        this.isLoading = false;
                        if (this.listOrder.length == 0 && this.pageNumber > Enum.FirstPage) { // nếu không lấy được đơn hàng nào thì
                            this.pageNumber = Enum.FirstPage;                // quay về trang đầu tiên
                            this.getData();
                        }
                    });
            } catch (error) {
                console.log(error);
            }
        },

        /**
         * Set dữ liệu mẫu lúc đầu của page
         * @param {*} value :  giá trị API trả về
         * Author: HAQUAN(28/10/2022)
         */
        setInitPage(value) {
            this.listOrder = value.data.data;
            this.totalCount = value.data.totalCount;
        },

        /**
        * Hàm format giá trị tiền tệ
        * Author: HAQuan (28/08/2023)
        */
        formatMoney(money) {
            if (!isNaN(money)) {
                var valueMoney = parseInt(money);
                return valueMoney.toString().replace(/(\d)(?=(\d{3})+(?:\.\d+)?$)/g, "$1.");
            } else {
                return valueMoney;
            }
        },

        /**
        * Set giá trị text hình thức giao hàng về giá trị lưu trong db
        * @param {string} value :  giá trị hình thức giao hàng dạng text người dùng chọn
        * Author: HAQUAN(29/08/2023) 
        */
        setDeliveryMethodFilter(value) {
            Const.DeliveryMethodFilter.find((item) => {
                if (item.Label == value) {
                    this.filter.deliveryMethod = item.Value;
                }
            });
        },

        configDeliveryMethod(value) {
            let result = value;
            Const.DeliveryMethodFilter.find((item) => {
                if (item.Value === value) {
                    result = item.Label;
                }
            });
            return result;

        },

        setPaymentMethodFilter(value) {
            Const.PaymentMethodFilter.find((item) => {
                if (item.Label == value) {
                    this.filter.paymentMethod = item.Value;
                }
            });
        },

        configPaymentMethod(value) {
            let result = value;
            Const.PaymentMethodFilter.find((item) => {
                if (item.Value === value) {
                    result = item.Label;
                }
            });
            return result;

        },

        /**
         * Set giá trị pageSize
         * @param {int} value :  giá trị pageSize được chọn
         * Author: HAQUAN(26/10/2022) 
         */
        setPageSize(value) {
            this.pageSize = value;
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
        
         addRate(productIDRate) {
            try {
                this.formMode = Enum.Mode.Add;
                this.productID = productIDRate;
                this.showFormDetail();
            } catch (error) {
                console.log(error);
            }
        },

        openFormRate(rateID) {
            try {
                this.formMode = Enum.Mode.Edit;
                this.rateIDSeleectd = rateID;
                this.showFormDetail();
            } catch (error) {
                console.log(error);
            }
        },

        async cancelOrder(orderID) {
            try {
                this.isLoading = true;
                let url = `OrderProduct/cancelOrder${orderID}`
                await axios.put(url).then((response) => {
                    console.log(response);
                    this.showToast(Resource.Message.CancelOrderSucces, Const.TypeToast.Success);
                    this.getData();
                })

                    .catch((error) => {
                        console.log(error);
                        this.handleErrorResponse(error);
                    })
                    .finally(() => {
                        this.isLoading = false;
                        if (this.listOrder.length == 0 && this.pageNumber > Enum.FirstPage) { // nếu không lấy được đơn hàng nào thì
                            this.pageNumber = Enum.FirstPage;                // quay về trang đầu tiên
                            this.getData();
                        }
                    });

            } catch (e) {
                console.log(e);
            }
        },

        updateStatus(value) {
            this.filter.statusOrder = value;
            this.getData();

        },

        /**
         * Ẩn/hiện bộ lọc
         * Author: HAQUAN(28/10/2022)
         */
        toggleFilterBox() {
            this.isShowFilterBox = !this.isShowFilterBox;
        },

        /**
         * Đóng popup 
         *  Author: HAQUAN(28/10/2022)
         */
        closePopup() {
            this.isShowPopup = false;
        },

        /**
         * Đóng bộ lọc đơn hàng 
         *  Author: HAQUAN(28/10/2022)
         */
        closeFilterBox() {
            this.isShowFilterBox = false;
        },

        /**
         * Thực hiện lọc đơn hàng theo giá trị mặc định
         *  Author: HAQUAN(28/10/2022)
         */
        filterOrderDefault() {
            this.isFilter = false;
            this.isShowFilterBox = false;
            this.filter = Object.assign({}, this.filterDefault);
            this.pageNumber = Enum.FirstPage;

            this.getData();
        },

        /**
         * Thực hiện lọc đơn hàng theo giá trị đã chọn
         *  Author: HAQUAN(28/10/2022)
         */
        filterOrder() {
            this.isShowFilterBox = false;
            this.isFilter = true;
            this.pageNumber = Enum.FirstPage;

            this.getData();
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

        /**
        * Xử lý các lỗi trả về từ server
        * @param {*} error Đối tượng lỗi
        * Author: HAQUAN(29/08/2023) 
        */
        async handleErrorResponse(error) {
            try {
                // Nếu lỗi trả về là lỗi đăng nhập (401)
                if (error.response.status == Enum.Error.Login) {
                    var tokenMoel = {
                        refreshToken: CommonFn.getCookie('RefreshToken'),
                        accessToken: CommonFn.getCookie('Token')
                    };
                    await axios.post('Account/renewToken', tokenMoel).
                        then((response) => {
                            CommonFn.setCookie('RefreshToken', response.data.refreshToken, 60);
                            CommonFn.setCookie('Token', response.data.accessToken, 60);
                            this.$store.dispatch('setUser', response.data.infoUser);
                            axios.defaults.headers.common["Authorization"] = "Bearer " + response.data.accessToken;
                        }).
                        catch((error) => {
                            console.log(error);
                            this.$store.dispatch('setUser', null);
                            this.$router.push({ name: 'LoginForm' });

                        });
                }
            } catch (error) {
                console.log(error);
            }
        },

        /**
        * Validate ngày bắt đầu
        * Author: HAQUAN(22/10/2023) 
        */
        validateTimeStart() {
            try {
                if (moment(this.filter.timeStart) > moment()) {
                    this.errors.timeStart = Resource.Error.DateStart;
                    return false;
                }
                this.errors.timeStart = '';
                return true;
            } catch (error) {
                console.log(error);
            }
        },
        /**
        * Validate ngày kết thúc
        * Author: HAQUAN(22/10/2023) 
        */
        validateTimeEnd() {
            try {
                if (moment(this.filter.timeEnd) > moment()) {
                    this.errors.timeEnd = Resource.Error.DateEnd;
                    return false;
                }
                this.errors.timeEnd = '';
                return true;
            } catch (error) {
                console.log(error);
            }
        },

        convertTime(value) {
            return moment(value).format('L');
        },
    },
    created() {
        this.accountID = this.$store.getters.user.accountID;
        // Lấy danh sách đơn hàng
        this.filterOrderDefault();
        document.title = Resource.Title.Management;
    },

    watch: {
        pageSize(newValue) {
            if (newValue) {
                this.getData();
            }
        },
    },
}
</script>
<style scoped>
.title-box {
    margin: 0 0 16px;
    font-size: 20px;
    font-weight: 700;
    line-height: 35px;
}

.toolbar {
    margin: 0 0 16px;
    justify-content: space-between;
}

.toolbar-right {
    position: relative;
}

.toolbar-right .filter {
    position: relative;
    padding: 0;
    outline: none;
    border: 1px solid #e0e0e0;
    color: #000;
    background: #fff;
}

.btn-tag {
    width: 120px;
    outline: none;
    border-left: 1px solid #e0e0e0;
    color: #000;
    background: #fff;
}

.btn-tag .text-button {
    padding: 0 !important;
}

.icon-filter {
    background: url(../../assets/icon/filter.png);
    background-size: 24px 24px;
    height: 24px;
    width: 24px;
    position: relative;
    margin: 0 5px 0 12px;
}

.icon-filter.active {
    background: url(../../assets/icon/filter-active.png);
    background-size: 24px 24px;
}

.btn .text-button {
    display: inline-block;
    white-space: nowrap;
    padding: 0 16px 0 0;
    font-weight: 400;
    font-size: 14px;
}

.list-order {
    /* background-color: #fff; */
    position: relative;
    border-radius: 4px;
    /* width: 100%;
    height: 100%; */
}

.box {
    /* background-color: #fff; */
    border-radius: 4px;
    box-shadow: 0 0 11px rgb(0 0 0 / 8%);
}

.empty-text {
    color: #9e9fab;
    display: flex;
    align-items: center;
    padding: 0 16px;
    height: 40px;
    position: absolute;
    width: 150px;
    white-space: nowrap;
}

.body {
    height: 100%;
}

[tooltip] {
    position: relative;
}

[tooltip]:hover::before,
[tooltip]:hover::after {
    display: block;
}

[tooltip]:after {
    width: fit-content;
    padding: 0 8px;
    background-color: #000;
    color: #fff;
    content: attr(tooltip);
    display: none;
    position: absolute;
    top: 5%;
    left: 0;
    z-index: 10;
    border-radius: 10px;
}

.filter-box {
    position: absolute;
    background: #fff;
    top: calc(100% + 10px);
    right: 0;
    width: 560px;
    display: block;
    z-index: 10;
    border-radius: 4px;
    box-shadow: 0 0 16px rgb(23 27 42 / 24%);
}

.arrow-filter-box {
    position: absolute;
    background: #fff;
    height: 16px;
    top: -6px;
    right: 30px;
    transform: rotate(45deg);
    border-left: 8px solid transparent;
    border-right: 8px solid transparent;
    border-bottom: 8px solid var(--primary);
    transition: border-color .2s linear;
    box-shadow: 0 -20px 20px 0 rgb(0 0 0 / 8%);
}

.filter-box .box-header {
    width: 100%;
    padding: 12px 22px 8px 24px;
    background-color: var(--primary);
    display: flex;
    align-items: center;
    justify-content: space-between;
    border-top-right-radius: 4px;
    border-top-left-radius: 4px;
}

.filter-box .box-header .title {
    font-size: 18px;
    font-weight: 700;
}

.filter-box .box-content {
    width: 100%;
    padding: 12px 24px 10px;
    background-color: var(--primary);
    display: flex;
}

.filter-box .box-content .row-box {
    margin-bottom: 14px;
}

.filter-box .box-content .row-box label {
    padding: 8px 8px 0 0;
    color: #666;
    font-size: 14px;
    font-weight: 500;
}

.filter-box .box-footer {
    width: 100%;
    padding: 12px 24px;
    background-color: var(--gray);
    border-bottom-right-radius: 4px;
    border-bottom-left-radius: 4px;
    border-top: 1px solid #ddd;
    display: flex;
}

.cancel-seleted {
    color: rgb(41, 121, 255) !important;
    margin: 0 10px 0 20px;
    border-radius: 4px;
    min-width: 39px;
    line-height: calc(38/13);
}

.cancel-seleted:hover,
.cancel-seleted:focus,
.cancel-seleted:active {
    text-decoration: underline;
    color: rgb(26, 115, 232) !important;
}

.w-100 {
    width: 100% !important;
}

.order-content {
    display: inline-block;
    padding: 10px;
    margin-bottom: 8px;
    background-color: #fff;
    border-radius: 0.3rem !important;
}

.row-oneline {
    display: flex;
    flex-wrap: wrap;
}

.info-order {
    padding: 0 10px;
    width: 40%;
    min-width: 400px;
}

.table {
    height: 471px;
    overflow: auto;
}

.table::-webkit-scrollbar {
    width: 10px;
    height: 10px;
    border-radius: 10px;
    background: #fff;
}

.table::-webkit-scrollbar:hover {
    cursor: pointer;

}

.table::-webkit-scrollbar-thumb {
    border-radius: 10px;
    background-color: #b5b5b5;
}

.table::-webkit-scrollbar-thumb:hover {
    background-color: #8f8f8f;
}

.mb-10 {
    margin-bottom: 10px;
}

.image img {
    width: 180px;
    height: auto;
}

.product-img {
    width: 200px;
    margin-right: 15px;
    cursor: pointer;
}

.fw-bold {
    font-weight: bold;
    margin-right: 5px;
}

.name-product {
    padding-bottom: 20px;
    font-weight: 700;
    font-size: 18px;
}

.order-detail {
    width: 60%;
    max-width: calc(100% - 400px);
}

.list-product {
    background-color: #e8e8e8;
    border-radius: 6px;
    padding: 10px;
    margin-bottom: 5px;
}

.btn-cancel-order {
    width: 40%;
    background-color: #f3dbba;
    font-weight: 500;
    color: #000;
    cursor: pointer;
}

.btn-cancel-order:hover {
    background-color: #ff9300;
    color: #fff;
}

.btn-tag:hover {
    color: orangered;
    background-color: rgb(246, 242, 229);
}

.btn-tag.selected {
    color: orangered;
    border-bottom: 4px solid orangered;
}
.btn-buy-repeat{
    outline: 0;
    color: #fff;
    height: 48px;
    width: 90px;
    background: #ee4d2d;
    cursor: pointer;
}
.btn-buy-repeat:hover{
    background: #f05d40;
}
.btn-rate{
    margin-right: 10px;
    outline: 0;
    box-shadow: 0 1px 1px 0 rgba(0,0,0,.03);
    border: 1px solid #ee4d2d;
    color: #ee4d2d;
    height: 48px;
    width: 120px;
    background: rgba(255,87,34,.1);
    cursor: pointer;
}
.btn-rate:hover{
    background: rgba(255,197,178,.181);
}
</style>
