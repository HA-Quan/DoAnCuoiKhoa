<template>
    <BaseLoading v-if="isLoading" />
    <div v-else class="body flex-column " style="outline: none;" tabindex="0"
        @keydown.ctrl.shift.o.prevent.exact="addOrder">
        <div class="title-box">{{ Resource.Label.Order }}</div>

        <div class="toolbar flex-row">
            <div class="toolbar-left flex-row">
                <div class="search-box flex-row">
                    <input @keypress.enter="search" v-model="keyword" :placeholder="Resource.Placehoder.SearchAccount"
                        class="search-input" type="text" @focus="showIconX = true" @blur="showIconX = false" />
                    <div v-show="keyword && showIconX" @mousedown="keyword = ''" class="icon-X"></div>
                    <div @click="search" class="search-input-icon"></div>
                </div>

                <BaseButton @click="toggleFilterBox" class="filter flex-row btn">
                    <div class="icon-filter" :class="{ 'active': isFilter }"></div>
                    <div class="text-button">{{ Resource.Button.Filter }}</div>
                </BaseButton>

                <div v-show="isFilter" @click="filterOrderDefault" class="cancel-seleted pointer">{{
                    Resource.Text.StopFilter }}
                </div>

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
                                <label for="">{{ Resource.Label.PaymentStatus }}</label>
                                <div class="flex mt-10">
                                    <BaseCombobox :hasIcon="false" :isReadonly="true" :items="Const.PaymentStatusFilter"
                                        :initItem="configPaymentStatus(filter.statusPayment)" fieldName="Label"
                                        @getValue="setPaymentStatusFilter" />
                                </div>
                            </div>
                            <div class="flex">
                                <label for="">{{ Resource.Label.StatusOrder }}</label>
                                <div class="flex mt-10">
                                    <BaseCombobox :hasIcon="false" :isReadonly="true" :items="Const.StatusOrderFilter"
                                        :initItem="configStatusOrder(filter.statusOrder)" fieldName="Label"
                                        @getValue="setStatusOrder" />
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

            <div class="toolbar-right flex-row">
                <div class="flex-row">
                    <el-tooltip effect="dark" :content="Const.KeyStrokes.CtrlShiftO" placement="top">
                        <BaseButton v-show="selectedOrders.length == 0" @click="addOrder" class="main-button flex-row btn">
                            <div class="icon-sum"></div>
                            <div class="text-button">{{ Resource.Button.AddNewOrder }}</div>
                        </BaseButton>
                    </el-tooltip>
                </div>

                <div v-show="selectedOrders.length > 0" class="multiple-action flex-row">
                    <div class="selected-count d-flex align-items">
                        {{ Resource.Label.Selected }}
                        <strong>{{ selectedOrders.length }}</strong>
                    </div>

                    <div @click="unselectAll" class="cancel-seleted d-flex align-items pointer">{{
                        Resource.Label.StopSelected }}</div>
                    <div class="action-container d-flex">
                        <BaseButton @click="deleteMultipleOnClick" class="action flex-row btn button-red">
                            <div class="text-btn">{{ Resource.Button.Delete }}</div>
                        </BaseButton>
                    </div>
                </div>
            </div>
        </div>

        <div class="box list-order flex-column space-between">
            <div class="table">
                <table cellpadding="0" cellspacing="0">
                    <thead>
                        <tr>
                            <th class="select-all">
                                <BaseCheckbox class="table-checkbox" :class="{ 'check-temp': selectedOrders.length > 0 }"
                                    @check="selectAll"
                                    :checkedProp="checkList(listOrder, selectedOrders) && listOrder.length !== 0">
                                    <template #checkmark>
                                        <div class="checkbox-checkmark"></div>
                                    </template>
                                </BaseCheckbox>
                            </th>

                            <th class="col-150">
                                <div class="flex-row">
                                    <p class="title-table">{{ Resource.Label.OrderBy }}</p>
                                </div>
                            </th>

                            <th class="col-150">
                                <div class="flex-row">
                                    <p class="title-table">{{ Resource.Label.ReceiverName }}</p>
                                </div>
                            </th>

                            <th class="col-180 ">
                                <div class="flex-row">
                                    <p class="title-table ">{{ Resource.Label.DeliveryMethod }}</p>

                                </div>
                            </th>

                            <th class="col-120 ">
                                <div class="flex-row">
                                    <p class="title-table">{{ Resource.Label.PaymentMethod }}</p>
                                </div>
                            </th>
                            <th class="col-var">
                                <div class="flex-row">
                                    <p class="title-table">{{ Resource.Label.PaymentStatus }}</p>
                                </div>
                            </th>
                            <th class="col-100 ">
                                <div class="flex-row">
                                    <p class="title-table ">{{ Resource.Label.Total }}</p>
                                </div>
                            </th>
                            <th class="col-120 ">
                                <div class="flex-row">
                                    <p class="title-table ">{{ Resource.Label.Status }}</p>
                                </div>
                            </th>

                            <th class="col-120">
                                <div class="flex-row">
                                    <p class="title-table">{{ Resource.Label.Mode }}</p>
                                </div>
                            </th>

                        </tr>
                    </thead>

                    <tbody v-if="totalCount > 0">
                        <tr v-for="(order, index) in listOrder" @click.exact="focusIndex = index"
                            :class="{ 'selected-tr': focusIndex == index }" :key="order.orderID"
                            @dblclick="editOrder(order.orderID)">

                            <td class="select-all">
                                <BaseCheckbox class="table-checkbox" @check="selectRow(order)" :checkedProp="selectedOrders.some(
                                    (obj) => obj.orderID == order.orderID
                                )">
                                    <template #checkmark>
                                        <div class="checkbox-checkmark"></div>
                                    </template>
                                </BaseCheckbox>
                            </td>

                            <td>
                                {{ order.orderBy }}

                            </td>

                            <td>
                                {{ order.fullName }}
                            </td>

                            <td>
                                {{ configDeliveryMethod(order.deliveryMethod) }}
                            </td>

                            <td class="col-center">
                                {{ configPaymentMethod(order.paymentMethod) }}
                            </td>
                            <td>
                                {{ configPaymentStatus(order.paymentStatus) }}
                            </td>
                            <td>
                                {{ formatMoney(order.total) }}
                            </td>
                            <td>
                                {{ configStatusOrder(order.status) }}
                            </td>

                            <td style="position: relative;">
                                <div class="row-actions">

                                    <el-tooltip effect="dark" :content="Resource.Tooltip.Edit" placement="top"
                                        hide-after="0" class="d-flex">
                                        <div @click="editOrder(order.orderID)" class="item">
                                            <div class="icon-edit"></div>
                                        </div>
                                    </el-tooltip>

                                    <el-tooltip effect="dark" :content="Resource.Tooltip.Delete" placement="top"
                                        hide-after="0" class="d-flex">
                                        <div @click="deleteSingle(order)" class="item">
                                            <div class="icon-more"></div>
                                        </div>
                                    </el-tooltip>

                                </div>
                            </td>
                        </tr>
                    </tbody>

                    <div class="empty-text" v-else>{{ Resource.Message.NoData }}</div>
                </table>
            </div>
            <BasePaging :listSize="listSize" :pageSize="pageSize" :pageNow="pageNumber" :totalCount="totalCount"
                @setPageSize="setPageSize" @prePage="goToPrePage" @nextPage="goToNextPage" @updatePageNow="updatePageNow" />

        </div>

    </div>
    <FormOrderDetail @refreshData="getData" @showToast="showToast" :formMode="formMode" :orderIdUpdate="orderIdUpdate"
        :closeFormDetail="closeFormDetail" v-if="isShowFormDetail" />

    <BasePopup @close="closePopup" class="popup-delete" :title="titlePopup" :prefix="prefixPopup" :suffix="suffixPopup"
        :content="nameOrderDelete" v-show="isShowPopup">
        <template #buttons>

            <BaseButton @click="closePopup" class="btn sub-button">
                {{ Resource.Button.No }}
            </BaseButton>

            <BaseButton @click="deleteAcount(); closePopup();" class="main-button-red btn ml-10">
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
import BaseCombobox from "../components/base/BaseCombobox.vue";
import BaseCheckbox from "../components/base/BaseCheckbox.vue";
import BaseToastMessage from "../components/base/BaseToastMessage.vue";
import BasePopup from "../components/base/BasePopup.vue";
import BaseButton from "../components/base/BaseButton.vue";
import BasePaging from '@/components/base/BasePaging.vue';
import FormOrderDetail from './FormOrderDetail.vue';
export default {
    name: "AccountList",
    components: {
        BaseLoading, BaseButton, BaseCombobox, BaseCheckbox, BaseToastMessage, BasePopup, BasePaging, FormOrderDetail
    },
    data() {
        return {
            Resource: Resource,
            Enum: Enum,
            Const: Const,
            focusIndex: Enum.FirstIndexFocus, // chỉ số của đơn hàng focus vào

            showIconX: false, // cờ thể hiện trạng thái ẩn hiện của iconX

            titlePopup: Resource.Title.DeleteOrder, // tiêu đề của popup

            isShowFormDetail: false, // cờ điền khiển đóng mở form thêm mới

            isShowPopup: false, // cờ điền khiển đóng mở cảnh báo 

            isLoading: false, // cờ điều khiển bật tắt trạng thái loading

            isShowFilterBox: false, // cờ điền khiển đóng mở bộ lọc

            formMode: "", // chức năng của form

            orderNow: {}, //đơn hàng đang được chọn hiện tại 

            listOrder: [], // danh sách đơn hàng thi đua đang được hiển thị

            selectedOrders: [], // danh sách đơn hàng đang được chọn

            keyword: "", // từ khóa để tìm kiếm đơn hàng

            totalCount: 0, // tổng số đơn hàng

            pageNumber: Enum.FirstPage, // trang đang hiển thị

            pageSize: Enum.PageSize, // số đơn hàng hiển thị trong 1 trang

            nameOrderDelete: null, // tên đơn hàng muốn xóa

            prefixPopup: null, //

            suffixPopup: null,//

            orderIdUpdate: null, // id đơn hàng muốn cập nhật

            toastMessage: { // cảnh báo
                message: "", // nội dung cảnh báo
                type: "", // loại cảnh báo
                isShowed: false, // cờ điều khiển bật tắt cảnh báo
            },

            deleteMultiple: false, // cờ thể hiện có phải đang xóa nhiều hay không

            listOrderID: [], // danh sách ID của các đơn hàng 

            listSize: Enum.ListSize, // danh sách gía trị số đơn hàng hiển thị trong 1 trang

            filter: { // danh sách các điều kiện để lọc đơn hàng

            },
            filterDefault: {
                deliveryMethod: "",
                paymentMethod: "",
                statusPayment: "",
                statusOrder: Enum.StatusOrder.All,
                timeStart: "",
                timeEnd: ""
            },

            errors: {
                timeStart: '',
                timeEnd: ''
            },

            isFilter: false, // cờ thể hiện có đang lọc hay là không

            sort: Enum.SortAccount.All, // điều kiện sắp xếp

        };
    },
    methods: {

        /**
         * Lấy dữ liệu đơn hàng 
         * Author: HAQUAN(26/08/2023)
         */
        async getData() {
            try {
                debugger
                this.isLoading = true;
                let url = `OrderProduct/filter?timeStart=${this.filter.timeStart}&timeEnd=${this.filter.timeEnd}&keyword=${this.keyword}&sort=${this.sort}&timeStart=${this.filter.timeStart}&deliveryMethod=${this.filter.deliveryMethod}&paymentMethod=${this.filter.paymentMethod}&statusPayment=${this.filter.statusPayment}&status=${this.filter.statusOrder}&pageNumber=${this.pageNumber}`;
                await axios.get(url)

                    .then((response) => {
                        console.log(response);
                        this.setInitPage(response);
                    })

                    .catch((error) => {
                        console.log(error);
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
            this.focusIndex = Enum.FirstIndexFocus;
            this.orderNow = {};

            // cập nhật danh sách những đơn hàng đã chọn
            if (this.selectedOrders.length > 0) {
                for (let i = 0; i < this.selectedOrders.length; i++) {
                    this.listOrder.find(
                        (item) => {
                            if (item.orderID == this.selectedOrders[i].orderID) {
                                this.selectedOrders[i] = item;
                            }
                        }
                    );
                }
            }
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
        * Set giá trị text danh mục đơn hàng về id danh mục đơn hàng
        * @param {string} value :  giá trị danh mục đơn hàng dạng text người dùng chọn
        * Author: HAQUAN(29/08/2023) 
        */
        setDeliveryMethodFilter(value) {
            Const.DeliveryMethodFilter.find((item) => {
                if (item.Label == value) {
                    this.filter.role = item.Value;
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
                    this.filter.role = item.Value;
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

        setPaymentStatusFilter(value) {
            Const.PaymentStatusFilter.find((item) => {
                if (item.Label == value) {
                    this.filter.role = item.Value;
                }
            });
        },

        configPaymentStatus(value) {
            let result = value;
            Const.PaymentStatusFilter.find((item) => {
                if (item.Value === value) {
                    result = item.Label;
                }
            });
            return result;

        },

        setStatusOrder(value) {
            Const.StatusOrderFilter.find((item) => {
                if (item.Label == value) {
                    this.filter.status = item.Value;
                }
            });
        },

        configStatusOrder(value) {
            let result = value;
            Const.StatusOrderFilter.find((item) => {
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
         * Thêm mới đơn hàng 
         * Author: HAQUAN(28/08/2023)
         */
        addOrder() {
            try {
                this.formMode = Enum.Mode.Add;
                this.showFormDetail();
            } catch (error) {
                console.log(error);
            }
        },

        /**
         * Sửa thông tin đơn hàng 
         * Author: HAQUAN(28/10/2022)
         */
        editOrder(orderID) {
            try {
                this.formMode = Enum.Mode.Edit;
                this.orderIdUpdate = orderID;
                this.showFormDetail();
            } catch (error) {
                console.log(error);
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
         * Tìm kiếm đơn hàng 
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

        /**
         * Xóa 1 đơn hàng 
         * @param {*} order :  đơn hàng đang được chọn để xóa
         * Author: HAQUAN(28/10/2022)
         */
        deleteSingle(order) {
            try {
                this.deleteMultiple = false;
                this.nameOrderDelete = order.fullName;
                this.prefixPopup = Resource.Text.CanDeleteOrder;
                this.suffixPopup = Resource.Text.SelectedNo;
                this.orderNow = order;
                this.isShowPopup = true;
            } catch (error) {
                console.log(error);
            }
        },

        /**
         * Xử lý sự kiện khi nút xóa nhiều được click
         * Author: HAQUAN(28/10/2022)
         */
        deleteMultipleOnClick() {
            try {

                // Nếu chỉ có 1 đơn hàng được chọn, xóa một đơn hàng
                if (this.selectedOrders.length == 1) {
                    this.deleteSingle(this.selectedOrders[0]);
                }

                // Ngược lại nếu có nhiều hơn một đơn hàng thì xóa nhiều
                else if (this.selectedOrders.length > 1) {

                    // Thực hiện ánh xạ danh sách đơn hàng đang được chọn
                    // sang danh sách ID đơn hàng 
                    this.listOrderID = [];
                    this.selectedOrders.filter((order) => {
                        this.listOrderID.push(order.orderID)
                    });

                    this.titlePopup = Resource.Title.DeleteOrder;
                    this.nameOrderDelete = this.selectedOrders.length + " " + Resource.Text.Order;
                    this.prefixPopup = Resource.Text.Delete;
                    this.suffixPopup = Resource.Text.Selected;

                    this.deleteMultiple = true;
                    this.isShowPopup = true;
                }
            } catch (error) {
                console.log(error);
            }
        },

        /**
         * Hàm thực hiện xóa đơn hàng 
         * Author: HAQUAN(28/10/2022)
         */
        deleteAcount() {
            if (this.deleteMultiple) {
                this.requestDeleteMultiple();
            }
            else {
                this.requestDeleteSingle(this.orderNow.orderID)
            }
        },

        /**
         * Gửi yêu cầu xóa 1 đơn hàng đến Server
         * Author: HAQUAN(28/10/2022)
         */
        async requestDeleteSingle(orderID) {
            try {
                let url = `OrderProduct/${orderID}`
                await axios.delete(url).then((response) => {
                    console.log(response);
                    // nếu là xóa 1 thì bỏ đơn hàng đã xóa khỏi danh sách các đơn hàng đang được chọn
                    this.selectedOrders = this.selectedOrders.filter(
                        (order) => order.orderID !== orderID
                    );

                    // Hiển thị thông báo xóa thành công
                    this.showToast(Resource.Message.DeleteSucces, Const.TypeToast.Success);
                    this.getData();
                })

                    .catch((error) => {
                        console.log(error);
                    });

            } catch (e) {
                console.log(e);
            }
        },

        /**
         * Gửi yêu cầu xóa nhiều đơn hàng đến Server
         * Author: HAQUAN(28/10/2022)
         */
        async requestDeleteMultiple() {
            try {
                await axios({
                    url: 'OrderProduct/multiple',
                    method: 'delete',
                    data: this.listOrderID,
                }).then((response) => {
                    console.log(response);
                    // Sau khi xóa thành công, nếu là xóa nhiều thì gán danh sách các đơn hàng đã chọn về rỗng
                    this.selectedOrders = [];

                    // Hiển thị thông báo xóa thành công
                    this.showToast(Resource.Message.DeleteSucces, Const.TypeToast.Success);
                    this.getData();
                })

                    .catch((error) => {
                        console.log(error);
                    });

            } catch (e) {
                console.log(e);
            }
        },

        /**
         * Ẩn/hiện bộ lọc
         * Author: HAQUAN(28/10/2022)
         */
        toggleFilterBox() {
            this.isShowFilterBox = !this.isShowFilterBox;
        },

        /**
         * Mở form thêm mới
         * Author: HAQUAN(28/10/2022)
         */
        showFormDetail() {
            this.isShowFormDetail = true;
        },

        /**
         * Đóng form thêm 
         * Author: HAQUAN(28/10/2022)
         */
        closeFormDetail() {
            this.isShowFormDetail = false;
            this.orderNow = {};
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
         * Xử lý khi một hàng được chọn
         * @param {*} orderNow: đơn hàng đang được chọn
         * Author: HAQUAN(28/10/2022)
         */
        selectRow(orderNow) {
            try {

                // Nếu như hàng được chọn đã được chọn trước đó thì loại bỏ khỏi danh sách
                if (this.selectedOrders.find((item) => item.orderID === orderNow.orderID)) {
                    this.selectedOrders = this.selectedOrders.filter(
                        (order) => order.orderID !== orderNow.orderID
                    );
                }

                // Nếu như hàng được chọn chưa được chọn trước đó thì thêm vào danh sách
                else {
                    this.selectedOrders.push(orderNow);
                }
            } catch (error) {
                console.log(error);
            }
        },

        /**
         * Xử lý bỏ chọn tất cả các đơn hàng đã chọn 
         * Author: HAQUAN(28/10/2022)
         */
        unselectAll() {
            this.focusIndex = Enum.FirstIndexFocus;
            this.selectedOrders = [];
            this.orderNow = {};
        },

        /**
         * Kiểm tra 2 danh sách đơn hàng
         * @param {*} firstList: danh sách đơn hàng cần so sánh
         * @param {*} secondList: danh sách đơn hàng cần so sánh
         * Author: HAQUAN(28/10/2022)
         */
        checkList(firstList, secondList) {

            if (firstList.length == 0 || secondList.length == 0) {
                return false;
            }

            try {
                let count = 0;
                for (let item of firstList) {
                    if (secondList.some((obj) => obj.orderID == item.orderID)) {
                        count++;
                    }
                }

                if (count == firstList.length) {
                    return true;
                }

            } catch (error) {
                console.log(error);
            }
            return false;
        },

        /**
         * Xử lý chọn tất cả các đơn hàng đang được hiển thị
         * Author: HAQUAN(28/10/2022)
         */
        selectAll() {
            try {

                // Kiểm tra xem những đơn hàng đang hiển thị có hết trong danh sách những đơn hàng đã được chọn hay không
                if (this.checkList(this.listOrder, this.selectedOrders)) { // Nếu phải thì bỏ hết những đơn hàng đó ra khỏi danh sách đã chọn
                    for (let order of this.listOrder) {
                        this.selectedOrders = this.selectedOrders.filter(
                            (item) => item.orderID != order.orderID
                        );
                    }
                }
                else { // Ngược lại thì thêm những đơn hàng chưa ở trong danh sách đã chọn vào danh sách các đơn hàng đã chọn

                    for (let item of this.listOrder) {
                        if (!this.selectedOrders.some((obj) => obj.orderID == item.orderID)) {
                            this.selectedOrders.push(item);
                        }
                    }
                }

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

        /**
        * Validate ngày bắt đầu
        * Author: HAQUAN(22/10/2023) 
        */
        validateTimeStart() {
            try {
                if (new Date(this.filter.timeStart) > new Date()) {
                    this.errors.timeStart = Resource.Error.DateStart;
                    return false;
                }
                this.errors.timeStart = "";
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
                if (new Date(this.filter.timeEnd) > new Date()) {
                    this.errors.timeEnd = Resource.Error.DateEnd;
                    return false;
                }
                this.errors.timeEnd = "";
                return true;
            } catch (error) {
                console.log(error);
            }
        },
    },
    created() {
        // Lấy danh sách đơn hàng
        this.filterOrderDefault();
        document.title = Resource.Title.Management;
    },

    watch: {
        pageSize(newValue) {
            if (newValue) {
                // this.pageNumber = Enum.FirstPage;
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

.toolbar-left {
    position: relative;
}

.toolbar-left .filter {
    position: relative;
    padding: 0;
    outline: none;
    border: 1px solid #e0e0e0;
    color: #000;
    background: #fff;
}

.icon-filter {
    background: url(../assets/icon/filter.png);
    background-size: 24px 24px;
    height: 24px;
    width: 24px;
    position: relative;
    margin: 0 5px 0 12px;
}

.icon-filter.active {
    background: url(../assets/icon/filter-active.png);
    background-size: 24px 24px;
}

.btn .text-button {
    display: inline-block;
    white-space: nowrap;
    padding: 0 16px 0 0;
    font-weight: 400;
    font-size: 14px;
}

.item {
    background-color: white;
    border-radius: 4px;
    height: 38.8px;
    width: 38.8px;
    display: flex;
    align-items: center;
    justify-content: center;
    cursor: pointer;
}

.icon-multiple-action {
    background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAABHNCSVQICAgIfAhkiAAAANVJREFUSEvtlEEOgjAURGd0Y6IRbyI3UI/gUryE3sRLiMdQb1BvYgku9RsaSQg2lIaQbui2v//NzG9L9LzYc38MAGfCYSKaniUmETnlVQpEoF97qvoZq4MolRuAlQ8AwF0nXLcCeDZuLA8zg94dzFM5EVj6gAR4ZAkPrWbwA8SeANUa4NPYVdt5yFEq8iY2+Y7F1f5bYQBND00nNKIK5XW5NidWB+arGGFhs1xGMbuIebVjwVWA44dQ+QQKWz6r58JE5LoZ1f3CiU15WdPZgUvMAHAlhC8V+kcZr8aZGQAAAABJRU5ErkJggg==);
    height: 24px;
    width: 24px;
}

.muliptle-action-box {
    position: absolute;
    background: #fff;
    top: 22.5%;
    right: 198px;
    display: block;
    z-index: 11;
    border-radius: 4px;
    box-shadow: 0 0 16px rgb(23 27 42 / 24%);
}

.more-action {
    display: block;
    padding: 12px 4px 12px 8px;
    text-decoration: none;
    white-space: nowrap;
    background-color: transparent;
    border: 0;
    min-width: 80px;
    border-radius: 4px;
}

.icon-sum {
    background: transparent url(../assets/icon/icon-sum.png);
    background-size: 24px 24px;
    height: 24px;
    width: 24px;
    position: relative;
    margin: 0 5px 0 12px;
}

.list-order {
    background-color: #fff;
    position: relative;
    border-radius: 4px;
    width: 100%;
    height: 100%;
}

.box {
    background-color: #fff;
    border-radius: 4px;
    box-shadow: 0 0 11px rgb(0 0 0 / 8%);
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
    border-bottom: 1px solid #e0e1ef;
    border-right: 1px solid #e0e1ef;
    z-index: 2;
    height: 48px;
    cursor: pointer;
    text-align: left;
    padding: 0 12px;
    white-space: nowrap;
}

.table th:last-child {
    border-right: none;
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

.select-all {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 53px !important;
    max-width: 53px !important;
    min-width: 53px !important;
}

.checkbox .checkbox-checkmark {
    position: absolute;
    top: 50%;
    transform: translateY(-50%);
    left: 2px;
    height: 16px;
    width: 16px;
    z-index: 1;
    background-position: 0px 0px;
    background-size: cover;
    background-repeat: no-repeat;
    background-image: url(https://cegovapp.misacdn.net/r/cegov/img/checkbox_grey.404fc483.svg);
}

.checkbox.check-temp .checkbox-checkmark {
    display: inline-block;
    background: url(https://cegovapp.misacdn.net/r/cegov/img/CeGo_Sprites.f3e906b1.svg);
    background-position: -544px -144px;
    width: 16px;
    height: 16px;
    position: absolute;
    top: 10px;
    left: 2px;
}

.checkbox:not(.check-temp) input:not(:checked):hover~.checkbox-checkmark,
.checkbox:not(.check-temp) input:not(:checked):focus~.checkbox-checkmark {
    top: 9px;
    left: 0px;
    width: 22px;
    height: 22px;
    background-image: url(https://cegovapp.misacdn.net/r/cegov/img/ic_checkbox_hover.d04300f4.svg);
}

.checkbox>.checkbox-input:checked~.checkbox-checkmark {
    background-image: url(https://cegovapp.misacdn.net/r/cegov/img/ic_checkbox_active.14fc601f.svg);
}

.checkbox-checkmark:after {
    content: "";
    position: absolute;
    display: none;
}

.checkbox .checkbox-input:checked~.checkbox-checkmark:after {
    display: block;
}

.checkbox .checkbox-input:checked:disabled~.checkbox-checkmark {
    width: 24px;
    height: 24px;
    background-image: url(https://cegovapp.misacdn.net/r/cegov/img/ic_checkbox_disabled_active.c6062b71.svg);
}

.checkbox .checkbox-input:disabled~.checkbox-checkmark {
    width: 24px;
    height: 24px;
    background-image: url(https://cegovapp.misacdn.net/r/cegov/img/ic_checkbox_disabled.7177f2a0.svg);
}

.col-180 {
    max-width: 180px;
    min-width: 180px;
}

.col-100 {
    max-width: 100px;
    min-width: 100px;
}

.col-150 {
    max-width: 150px;
    min-width: 150px;
}

.col-120 {
    min-width: 120px;
    max-width: 120px;
}

.col-360 {
    min-width: 360px;
    max-width: 360px;
}

.col-var {
    width: 100%;
    min-width: 100px;
}

.body {
    height: 100%;
}

.table tbody {
    overflow: auto;
}

tbody tr {
    height: 43px !important;
}

tbody .selected-tr td {
    background: #e0ebff;
}

.table td {
    /* font-size: 13px; */
    line-height: calc(38/14);
    padding: 0 12px;
    height: 43px;
    vertical-align: middle;
    background-color: #fff;
    cursor: pointer;
    border-right: 1px solid #ddd;
    border-bottom: 1px solid #ddd;
    text-overflow: ellipsis;
    white-space: nowrap;
    overflow: hidden;
}

.table td:last-child {
    border-right: none;
}

tr .row-actions {
    position: absolute;
    height: 40px;
    width: 100px;
    background: #fbe9e7;
    white-space: nowrap;
    justify-content: space-around;
    display: none;
    z-index: 1;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
}

tr.selected-tr .row-actions {
    display: flex;
    background: #e0ebff;
}

tr:hover td {
    background: #fbe9e7;
}

tr:hover .row-actions {
    background: #fbe9e7;
    display: flex;
}

.row-actions .item {
    border-radius: 50%;
    border: 1px solid #e0e0e0;
    background-color: #fff;
    width: 36px;
    height: 36px;
    cursor: pointer;
    position: relative;
    display: inline-flex;
    align-items: center;
    justify-content: center;
    margin: auto;
    margin-left: 4px;
}

.mode-more {
    width: 200px;
    background-color: #fff;
    background-clip: padding-box;
    border-radius: 4px;
    box-shadow: 0 0 16px rgb(23 27 42 / 24%);
    display: block;
    margin: 0;
    padding: 8px 0;
    z-index: 100000;
    position: absolute;
    right: 0px;
    list-style: none;
    box-sizing: border-box;
    overflow-y: auto;
}

.mode-more-option {
    display: block;
    padding: 12px 4px 12px 0;
    text-decoration: none;
    white-space: nowrap;
    background-color: transparent;
    border: 0;
    margin: 0 8px;
    min-width: 80px;
    border-radius: 4px;
    position: relative;
}

.mode-more-option.disabled {
    color: #d2d2d2;
}

.mode-more-option:focus,
.mode-more-option:hover {
    background-color: #eff1f6;
}

.mode-more-option.disabled:focus,
.mode-more-option.disabled:hover {
    background-color: #fff;
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
    left: 0;
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
    right: 225px;
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
    line-height: 3;
}

.cancel-seleted:hover,
.cancel-seleted:focus,
.cancel-seleted:active {
    text-decoration: underline;
    color: rgb(26, 115, 232) !important;
}

.icon-edit {
    content: "";
    position: absolute;
    background: transparent url(../assets/icon/edit.png);
    background-size: 24px 24px;
    height: 24px;
    width: 24px;
    top: 4px;
    left: 5px;
    background-position: 0 -72px;
}

.icon-more {
    content: "";
    position: absolute;
    background: transparent url(../assets/icon/delete.png);
    background-size: 24px 24px;
    height: 24px;
    width: 24px;
    top: 4px;
    left: 5px;
    background-position: 0 -72px;
}
.w-100{
    width: 100% !important;
}
</style>