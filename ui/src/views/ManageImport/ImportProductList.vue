<template>
    <BaseLoading v-if="isLoading" />
    <div v-else class="body flex-column " style="outline: none;" tabindex="0"
        @keydown.ctrl.shift.o.prevent.exact="addImport">
        <div class="title-box">{{ Resource.Label.ImportList }}</div>

        <div class="toolbar flex-row">
            <div class="toolbar-left flex-row">
                <BaseButton @click="toggleFilterBox" class="filter flex-row btn">
                    <div class="icon-filter" :class="{ 'active': isFilter }"></div>
                    <div class="text-button">{{ Resource.Button.Filter }}</div>
                </BaseButton>

                <div v-show="isFilter" @click="filterImportDefault" class="cancel-seleted pointer">{{
                    Resource.Text.StopFilter }}
                </div>

                <div class="filter-box" v-show="isShowFilterBox">
                    <span class="arrow-filter-box"></span>

                    <div class="box-header">
                        <span class="title">{{ Resource.Label.FilterImport }}</span>
                        <div @click="closeFilterBox" style="left: 0px;" class="icon-X"></div>
                    </div>

                    <div class="box-content flex-column">
                        <div class="row-box">
                            <label for="">{{ Resource.Label.Supplier }}</label>
                            <div class="flex mt-10">
                                <BaseCombobox :isCheck="errors.supplier != '' ? true : false" :hasIcon="false"
                                    :items="listSupplier" :initItem="configSupplier(filter.supplier)"
                                    fieldName="supplierName" @getValue="setSupplierFilter" />
                                <div class="error-text">{{ errors.supplier }}</div> 
                            </div>
                        </div>
                        <div class="row-box">
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
                        <div class="row-box">
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
                        <BaseButton v-show="selectedImports.length == 0" @click="addImport"
                            class="main-button flex-row btn">
                            <div class="icon-sum"></div>
                            <div class="text-button">{{ Resource.Button.AddNewImport }}</div>
                        </BaseButton>
                    </el-tooltip>
                </div>

                <div v-show="selectedImports.length > 0" class="multiple-action flex-row">
                    <div class="selected-count d-flex align-items">
                        {{ Resource.Label.Selected }}
                        <strong>{{ selectedImports.length }}</strong>
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

        <div class="box list-import flex-column space-between">
            <div class="table">
                <table cellpadding="0" cellspacing="0">
                    <thead>
                        <tr>
                            <th class="select-all">
                                <BaseCheckbox class="table-checkbox" :class="{ 'check-temp': selectedImports.length > 0 }"
                                    @check="selectAll"
                                    :checkedProp="checkList(listImport, selectedImports) && listImport.length !== 0">
                                    <template #checkmark>
                                        <div class="checkbox-checkmark"></div>
                                    </template>
                                </BaseCheckbox>
                            </th>

                            <th class="col-var">
                                <div class="flex-row pointer">
                                    <p class="title-table">{{ Resource.Label.ImportBy }}</p>
                                </div>
                            </th>

                            <th class="col-180 pointer" @click="sortData(Enum.Sort.NameAsc)">
                                <div class="flex-row">
                                    <p class="title-table">{{ Resource.Label.Supplier }}</p>
                                    <div class="icon-sort" :class="{
                                        'desc': this.sort == Enum.Sort.NameDesc,
                                        'asc': this.sort == Enum.Sort.NameAsc
                                    }"></div>
                                </div>
                            </th>

                            <th class="col-120 pointer" @click="sortData(Enum.Sort.QuantityAsc)">
                                <div class="flex-row">
                                    <p class="title-table ">{{ Resource.Label.Quantity }}</p>
                                    <div class="icon-sort" :class="{
                                        'desc': this.sort == Enum.Sort.QuantityDesc,
                                        'asc': this.sort == Enum.Sort.QuantityAsc
                                    }"></div>
                                </div>
                            </th>

                            <th class="col-150 pointer" @click="sortData(Enum.Sort.PriceAsc)">
                                <div class="flex-row">
                                    <p class="title-table ">{{ Resource.Label.Total }}</p>
                                    <div class="icon-sort" :class="{
                                        'desc': this.sort == Enum.Sort.PriceDesc,
                                        'asc': this.sort == Enum.Sort.PriceAsc
                                    }"></div>
                                </div>
                            </th>
                            <th class="col-150 pointer" @click="sortData(Enum.Sort.TimeAsc)">
                                <div class="flex-row">
                                    <p class="title-table ">{{ Resource.Label.TimeImport }}</p>
                                    <div class="icon-sort" :class="{
                                        'desc': this.sort == Enum.Sort.TimeDesc,
                                        'asc': this.sort == Enum.Sort.TimeAsc
                                    }"></div>
                                </div>
                            </th>

                            <th class="col-150">
                                <div class="flex-row">
                                    <p class="title-table">{{ Resource.Label.Mode }}</p>
                                </div>
                            </th>

                        </tr>
                    </thead>

                    <tbody v-if="totalCount > 0">
                        <tr v-for="(importProduct, index) in listImport" @click.exact="focusIndex = index"
                            :class="{ 'selected-tr': focusIndex == index }" :key="importProduct.importID"
                            @dblclick="editImport(importProduct.importID)">

                            <td class="select-all">
                                <BaseCheckbox class="table-checkbox" @check="selectRow(importProduct)" :checkedProp="selectedImports.some(
                                    (obj) => obj.importID == importProduct.importID
                                )">
                                    <template #checkmark>
                                        <div class="checkbox-checkmark"></div>
                                    </template>
                                </BaseCheckbox>
                            </td>

                            <td>
                                {{ configImportBy(importProduct.createdBy) }}

                            </td>

                            <td>
                                {{ importProduct.supplierName }}
                            </td>

                            <td>
                                {{ importProduct.amount }}
                            </td>

                            <td>
                                {{ formatMoney(importProduct.price) }}
                            </td>
                            <td>
                                {{ convertTime(importProduct.createdDate) }}
                            </td>

                            <td>
                                <div class="row-actions">

                                    <el-tooltip effect="dark" :content="Resource.Tooltip.Edit" placement="top"
                                        hide-after="0" class="d-flex">
                                        <div @click="editImport(importProduct.importID)" class="item">
                                            <div class="icon-edit"></div>
                                        </div>
                                    </el-tooltip>

                                    <el-tooltip effect="dark" :content="Resource.Tooltip.Delete" placement="top"
                                        hide-after="0" class="d-flex">
                                        <div @click="deleteSingle(importProduct)" class="item">
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
    <FormImportDetail @refreshData="getData" @showToast="showToast" :formMode="formMode"
        :importProductIdUpdate="importProductIdUpdate" :closeFormDetail="closeFormDetail" v-if="isShowFormDetail" />

    <BasePopup @close="closePopup" class="popup-delete" :title="titlePopup" :prefix="prefixPopup" :suffix="suffixPopup"
        :content="nameImportDelete" v-show="isShowPopup">
        <template #buttons>

            <BaseButton @click="closePopup" class="btn sub-button">
                {{ Resource.Button.No }}
            </BaseButton>

            <BaseButton @click="deleteImport(); closePopup();" class="main-button-red btn ml-10">
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
import BaseCombobox from "@/components/base/BaseCombobox.vue";
import BaseCheckbox from "@/components/base/BaseCheckbox.vue";
import BaseToastMessage from "@/components/base/BaseToastMessage.vue";
import BasePopup from "@/components/base/BasePopup.vue";
import BaseButton from "@/components/base/BaseButton.vue";
import BasePaging from '@/components/base/BasePaging.vue';
import FormImportDetail from './FormImportDetail.vue';
import CommonFn from '@/utils/commonFuncion';
import moment from 'moment';
import 'moment/locale/vi'
export default {
    name: "ImportProductList",
    components: {
        BaseLoading, BaseButton, BaseCombobox, BaseCheckbox, BaseToastMessage, BasePopup, BasePaging, FormImportDetail
    },
    data() {
        return {
            Resource: Resource,
            Enum: Enum,
            Const: Const,
            focusIndex: Enum.FirstIndexFocus, // chỉ số của đơn nhập hàng focus vào

            titlePopup: Resource.Title.DeleteImportProduct, // tiêu đề của popup

            isShowFormDetail: false, // cờ điền khiển đóng mở form thêm mới

            isShowPopup: false, // cờ điền khiển đóng mở cảnh báo 

            isLoading: false, // cờ điều khiển bật tắt trạng thái loading

            isShowFilterBox: false, // cờ điền khiển đóng mở bộ lọc

            formMode: '', // chức năng của form

            importProductNow: {}, //đơn nhập hàng đang được chọn hiện tại 

            listImport: [], // danh sách đơn nhập hàng thi đua đang được hiển thị

            selectedImports: [], // danh sách đơn nhập hàng đang được chọn

            totalCount: 0, // tổng số đơn nhập hàng

            pageNumber: Enum.FirstPage, // trang đang hiển thị

            pageSize: Enum.PageSize, // số đơn nhập hàng hiển thị trong 1 trang

            nameImportDelete: null, // tên đơn nhập hàng muốn xóa

            prefixPopup: null, //

            suffixPopup: null,//

            importProductIdUpdate: null, // id đơn nhập hàng muốn cập nhật

            toastMessage: { // cảnh báo
                message: '', // nội dung cảnh báo
                type: '', // loại cảnh báo
                isShowed: false, // cờ điều khiển bật tắt cảnh báo
            },

            deleteMultiple: false, // cờ thể hiện có phải đang xóa nhiều hay không

            listImportID: [], // danh sách ID của các đơn nhập hàng 

            listSize: Enum.ListSize, // danh sách gía trị số đơn nhập hàng hiển thị trong 1 trang

            filter: { // danh sách các điều kiện để lọc đơn nhập hàng

            },
            filterDefault: {
                supplier: Const.GuidEmpty,
                timeStart: '',
                timeEnd: ''
            },

            errors: {
                supplier: '',
                timeStart: '',
                timeEnd: ''
            },

            listAccount: [],

            listSupplier: [], //Danh sách nhà cung cấp

            isFilter: false, // cờ thể hiện có đang lọc hay là không

            sort: Enum.Sort.Default, // điều kiện sắp xếp

        };
    },
    methods: {

        /**
         * Lấy dữ liệu người dùng
         * Author: HAQUAN(26/08/2023)
         */
        async getAccount() {
            try {
                await axios({
                    url: 'Account',
                    method: 'get',
                }).then((response) => {
                    this.listAccount = response.data;
                })

                    .catch((error) => {
                        console.log(error);
                    })

            } catch (error) {
                console.log(error);
            }
        },

        /**
         * Lấy dữ liệu nhà cung cấp
         * Author: HAQUAN(26/08/2023)
         */
        async getSupplier() {
            try {
                await axios({
                    url: 'Supplier',
                    method: 'get',
                }).then((response) => {
                    let x = { supplierID: Const.GuidEmpty, supplierName: "Tất cả" };
                    this.listSupplier.push(x);

                    response.data.forEach((element) => {
                        this.listSupplier.push({
                            supplierID: element.supplierID,
                            supplierName: element.supplierName,
                        });
                    });
                })

                    .catch((error) => {
                        console.log(error);
                    })

            } catch (error) {
                console.log(error);
            }
        },


        /**
         * Lấy dữ liệu đơn nhập hàng 
         * Author: HAQUAN(26/08/2023)
         */
        async getData() {
            try {
                debugger
                this.isLoading = true;
                let url = `ImportProduct/filter?timeStart=${this.filter.timeStart}&timeEnd=${this.filter.timeEnd}&supplier=${this.filter.supplier}&sort=${this.sort}&pageSize=${this.pageSize}&pageNumber=${this.pageNumber}`;
                await axios.get(url)

                    .then((response) => {
                        console.log(response);
                        this.setInitPage(response);
                    })

                    .catch((error) => {
                        console.log(error);
                        this.handleErrorResponse(error);
                    })

                    .finally(() => {
                        this.isLoading = false;
                        if (this.listImport.length == 0 && this.pageNumber > Enum.FirstPage) { // nếu không lấy được đơn nhập hàng nào thì
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
            this.listImport = value.data.data;
            this.totalCount = value.data.totalCount;
            this.focusIndex = Enum.FirstIndexFocus;
            this.importProductNow = {};

            // cập nhật danh sách những đơn nhập hàng đã chọn
            if (this.selectedImports.length > 0) {
                for (let i = 0; i < this.selectedImports.length; i++) {
                    this.listImport.find(
                        (item) => {
                            if (item.importID == this.selectedImports[i].importID) {
                                this.selectedImports[i] = item;
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
        * Set giá trị text danh mục sản phẩm về id danh mục sản phẩm
        * @param {string} value :  giá trị danh mục sản phẩm dạng text người dùng chọn
        * Author: HAQUAN(29/08/2023) 
        */
        setSupplierFilter(value) {
            let check = false;
            this.listSupplier.find((item) => {
                if (item.supplierName == value) {
                    this.filter.supplier = item.supplierID;
                    this.errors.supplier = "";
                    check = true;
                }
            });
            if (!check) {
                if (value == "") {
                    this.errors.supplier = Resource.Error.Supplier;
                }
                else {
                    this.errors.supplier = Resource.Error.SupplierNotFound;
                }
            }
        },

        configSupplier(value) {
            let result = value;
            this.listSupplier.find((item) => {
                if (item.supplierID === value) {
                    result = item.supplierName;
                }
            });
            return result;

        },

        configImportBy(value) {
            let result = value;
            this.listAccount.find((item) => {
                if (item.accountID === value) {
                    result = item.username;
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
         * Thêm mới đơn nhập hàng 
         * Author: HAQUAN(28/08/2023)
         */
        addImport() {
            try {
                this.formMode = Enum.Mode.Add;
                this.showFormDetail();
            } catch (error) {
                console.log(error);
            }
        },

        /**
         * Sửa thông tin đơn nhập hàng 
         * Author: HAQUAN(28/10/2022)
         */
        editImport(importProductID) {
            try {
                this.formMode = Enum.Mode.Edit;
                this.importProductIdUpdate = importProductID;
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
         * Xóa 1 đơn nhập hàng 
         * @param {*} importProduct :  đơn nhập hàng đang được chọn để xóa
         * Author: HAQUAN(28/10/2022)
         */
        deleteSingle(importProduct) {
            try {
                this.deleteMultiple = false;
                this.nameImportDelete = importProduct.importID.toUpperCase();
                this.prefixPopup = Resource.Text.CanDeleteImport;
                this.suffixPopup = Resource.Text.SelectedNo;
                this.importProductNow = importProduct;
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

                // Nếu chỉ có 1 đơn nhập hàng được chọn, xóa một đơn nhập hàng
                if (this.selectedImports.length == 1) {
                    this.deleteSingle(this.selectedImports[0]);
                }

                // Ngược lại nếu có nhiều hơn một đơn nhập hàng thì xóa nhiều
                else if (this.selectedImports.length > 1) {

                    // Thực hiện ánh xạ danh sách đơn nhập hàng đang được chọn
                    // sang danh sách ID đơn nhập hàng 
                    this.listImportID = [];
                    this.selectedImports.filter((importProduct) => {
                        this.listImportID.push(importProduct.importID)
                    });

                    this.titlePopup = Resource.Title.DeleteOrder;
                    this.nameImportDelete = this.selectedImports.length + " " + Resource.Text.Import;
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
         * Hàm thực hiện xóa đơn nhập hàng 
         * Author: HAQUAN(28/10/2022)
         */
        deleteImport() {
            if (this.deleteMultiple) {
                this.requestDeleteMultiple();
            }
            else {
                this.requestDeleteSingle(this.importProductNow.importID)
            }
        },

        /**
         * Gửi yêu cầu xóa 1 đơn nhập hàng đến Server
         * Author: HAQUAN(28/10/2022)
         */
        async requestDeleteSingle(importProductID) {
            try {
                let url = `ImportProduct/${importProductID}`
                await axios.delete(url).then((response) => {
                    console.log(response);
                    // nếu là xóa 1 thì bỏ đơn nhập hàng đã xóa khỏi danh sách các đơn nhập hàng đang được chọn
                    this.selectedImports = this.selectedImports.filter(
                        (importProduct) => importProduct.importID !== importProductID
                    );

                    // Hiển thị thông báo xóa thành công
                    this.showToast(Resource.Message.DeleteSucces, Const.TypeToast.Success);
                    this.getData();
                })

                    .catch((error) => {
                        console.log(error);
                        this.handleErrorResponse(error);
                    });

            } catch (e) {
                console.log(e);
            }
        },

        /**
         * Gửi yêu cầu xóa nhiều đơn nhập hàng đến Server
         * Author: HAQUAN(28/10/2022)
         */
        async requestDeleteMultiple() {
            try {
                await axios({
                    url: 'ImportProduct/multiple',
                    method: 'delete',
                    data: this.listImportID,
                }).then((response) => {
                    console.log(response);
                    // Sau khi xóa thành công, nếu là xóa nhiều thì gán danh sách các đơn nhập hàng đã chọn về rỗng
                    this.selectedImports = [];

                    // Hiển thị thông báo xóa thành công
                    this.showToast(Resource.Message.DeleteSucces, Const.TypeToast.Success);
                    this.getData();
                })

                    .catch((error) => {
                        console.log(error);
                        this.handleErrorResponse(error);
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
            this.importProductNow = {};
        },

        /**
         * Đóng popup 
         *  Author: HAQUAN(28/10/2022)
         */
        closePopup() {
            this.isShowPopup = false;
        },

        /**
         * Đóng bộ lọc đơn nhập hàng 
         *  Author: HAQUAN(28/10/2022)
         */
        closeFilterBox() {
            this.isShowFilterBox = false;
        },

        /**
         * Thực hiện lọc đơn nhập hàng theo giá trị mặc định
         *  Author: HAQUAN(28/10/2022)
         */
        filterImportDefault() {
            this.isFilter = false;
            this.isShowFilterBox = false;
            this.filter = Object.assign({}, this.filterDefault);
            this.pageNumber = Enum.FirstPage;

            
            this.getData();
        },

        /**
         * Thực hiện lọc đơn nhập hàng theo giá trị đã chọn
         *  Author: HAQUAN(28/10/2022)
         */
        filterOrder() {
            this.isShowFilterBox = false;
            this.isFilter = true;
            this.pageNumber = Enum.FirstPage;

            this.getData();
        },

        /**
        * Sắp xếp đơn nhập hàng
        * @param {byte} sortCondition :  giá trị điều kiện sắp xếp
        * Author: HAQUAN(28/10/2022)
        */
        sortData(sortCondition) {
            if (this.sort == sortCondition) {
                this.sort++;
            }
            else if (this.sort - sortCondition == 1) {
                this.sort = Enum.Sort.Default;
            }
            else {
                this.sort = sortCondition;
            }
            this.getData();
        },

        /**
         * Xử lý khi một hàng được chọn
         * @param {*} importProductNow: đơn nhập hàng đang được chọn
         * Author: HAQUAN(28/10/2022)
         */
        selectRow(importProductNow) {
            try {

                // Nếu như hàng được chọn đã được chọn trước đó thì loại bỏ khỏi danh sách
                if (this.selectedImports.find((item) => item.importID === importProductNow.importID)) {
                    this.selectedImports = this.selectedImports.filter(
                        (importProduct) => importProduct.importID !== importProductNow.importID
                    );
                }

                // Nếu như hàng được chọn chưa được chọn trước đó thì thêm vào danh sách
                else {
                    this.selectedImports.push(importProductNow);
                }
            } catch (error) {
                console.log(error);
            }
        },

        /**
         * Xử lý bỏ chọn tất cả các đơn nhập hàng đã chọn 
         * Author: HAQUAN(28/10/2022)
         */
        unselectAll() {
            this.focusIndex = Enum.FirstIndexFocus;
            this.selectedImports = [];
            this.importProductNow = {};
        },

        /**
         * Kiểm tra 2 danh sách đơn nhập hàng
         * @param {*} firstList: danh sách đơn nhập hàng cần so sánh
         * @param {*} secondList: danh sách đơn nhập hàng cần so sánh
         * Author: HAQUAN(28/10/2022)
         */
        checkList(firstList, secondList) {

            if (firstList.length == 0 || secondList.length == 0) {
                return false;
            }

            try {
                let count = 0;
                for (let item of firstList) {
                    if (secondList.some((obj) => obj.importID == item.importID)) {
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
         * Xử lý chọn tất cả các đơn nhập hàng đang được hiển thị
         * Author: HAQUAN(28/10/2022)
         */
        selectAll() {
            try {

                // Kiểm tra xem những đơn nhập hàng đang hiển thị có hết trong danh sách những đơn nhập hàng đã được chọn hay không
                if (this.checkList(this.listImport, this.selectedImports)) { // Nếu phải thì bỏ hết những đơn nhập hàng đó ra khỏi danh sách đã chọn
                    for (let importProduct of this.listImport) {
                        this.selectedImports = this.selectedImports.filter(
                            (item) => item.importID != importProduct.importID
                        );
                    }
                }
                else { // Ngược lại thì thêm những đơn nhập hàng chưa ở trong danh sách đã chọn vào danh sách các đơn nhập hàng đã chọn

                    for (let item of this.listImport) {
                        if (!this.selectedImports.some((obj) => obj.importID == item.importID)) {
                            this.selectedImports.push(item);
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
        // Lấy danh sách đơn nhập hàng
        this.getAccount();
        this.getSupplier();
        this.filterImportDefault();
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
    background: transparent url(../../assets/icon/icon-sum.png);
    background-size: 24px 24px;
    height: 24px;
    width: 24px;
    position: relative;
    margin: 0 5px 0 12px;
}

.list-import {
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
    height: 470px;
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
    min-width: 180px;
}

.body {
    height: 100%;
}

.table tbody {
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

tbody::-webkit-scrollbar {
    width: 8px;
    border-radius: 10px;
    background: #fff;
}

tbody::-webkit-scrollbar:hover {
    cursor: pointer;

}

tbody::-webkit-scrollbar-thumb {
    border-radius: 10px;
    background-color: #b5b5b5;
}

tbody::-webkit-scrollbar-thumb:hover {
    background-color: #8f8f8f;
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
    width: 360px;
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
    left: 45px;
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

.icon-edit {
    content: "";
    position: absolute;
    background: transparent url(../../assets/icon/edit.png);
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
    background: transparent url(../../assets/icon/delete.png);
    background-size: 24px 24px;
    height: 24px;
    width: 24px;
    top: 4px;
    left: 5px;
    background-position: 0 -72px;
}

.w-100 {
    width: 100% !important;
}

thead tr th:first-child,
tbody tr td:first-child {
    position: sticky;
    left: 0;
}

thead tr th:last-child,
tbody tr td:last-child {
    position: sticky;
    right: 0;
    border-left: 1px solid #e0e1ef;
}
</style>