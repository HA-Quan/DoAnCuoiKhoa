<template>
    <BaseLoading v-if="isLoading" />
    <div v-else class="body flex-column " style="outline: none;" tabindex="0" @keydown.ctrl.shift.o.prevent.exact="addAccount">
        <div class="title-box">{{ Resource.Label.Account }}</div>

        <div class="toolbar flex-row">
            <div class="toolbar-left flex-row">
                <div class="search-box flex-row">
                    <input @keypress.enter="search" v-model="keyword" :placeholder="Resource.Placehoder.SearchAccount"
                        class="search-input" type="text" @focus="showIconX = true" @blur="showIconX = false" />
                    <div v-show="keyword && showIconX" @mousedown="keyword = ''" class="icon-X"></div>
                    <div @click="search" class="search-input-icon"></div>
                </div>

                <BaseButton @click="toggleFilterBox" class="filter flex-row btn">
                    <div class="icon-filter" :class="{ 'active': this.isFilter }"></div>
                    <div class="text-button">{{ Resource.Button.Filter }}</div>
                </BaseButton>

                <div v-show="isFilter" @click="filterProductDefault" class="cancel-seleted pointer">{{ Resource.Text.StopFilter }}
                </div>

                <div class="filter-box" v-show="isShowFilterBox">
                    <span class="arrow-filter-box"></span>

                    <div class="box-header">
                        <span class="title">{{ Resource.Label.FilterAccount }}</span>
                        <div @click="closeFilterBox" style="left: 0px;" class="icon-X"></div>
                    </div>

                    <div class="box-content flex-column">

                        <div class="row-box">
                            <label for="">{{ Resource.Label.Role }}</label>
                            <div class="flex mt-10">
                                <BaseCombobox :hasIcon="false" :isReadonly="true"
                                    :items="Const.RoleFilter" :initItem="configRole(this.filter.role)"
                                    fieldName="Label" @getValue="setRoleFilter" />
                            </div>
                        </div>

                        <div class="row-box">
                            <label for="">{{ Resource.Label.Status }}</label>
                            <div class="flex mt-10">
                                <BaseCombobox :hasIcon="false" :isReadonly="true"
                                    :items="Const.StatusFilter" :initItem="configStatus(this.filter.status)"
                                    fieldName="Label" @getValue="setStatusFilter" />
                            </div>
                        </div>
                    </div>

                    <div class="box-footer flex-end">
                        <BaseButton @click="closeFilterBox" class="sub-button btn">{{ Resource.Button.Cancel }}</BaseButton>
                        <BaseButton @click="filterAccount" class="main-button btn ml-10">{{ Resource.Button.Apply }}
                        </BaseButton>
                    </div>
                </div>
            </div>

            <div class="toolbar-right flex-row">
                <div class="flex-row">
                    <el-tooltip effect="dark" :content="Const.KeyStrokes.CtrlShiftO" placement="top">
                        <BaseButton v-show="selectedAccounts.length == 0" @click="addAccount"
                            class="main-button flex-row btn">
                            <div class="icon-sum"></div>
                            <div class="text-button">{{ Resource.Button.AddAccount }}</div>
                        </BaseButton>
                    </el-tooltip>
                </div>

                <div v-show="selectedAccounts.length > 0" class="multiple-action flex-row">
                    <div class="selected-count d-flex align-items">
                        {{ Resource.Label.Selected }} 
                        <strong>{{ selectedAccounts.length }}</strong>
                    </div>

                    <div @click="unselectAll" class="cancel-seleted d-flex align-items pointer">{{ Resource.Label.StopSelected }}</div>
                    <div class="action-container d-flex">
                        <BaseButton @click="deleteMultipleOnClick" class="action flex-row btn button-red">
                            <div class="text-btn">{{ Resource.Button.Delete }}</div>
                        </BaseButton>
                    </div>
                </div>
            </div>
        </div>

        <div class="box list-account flex-column space-between">
            <div class="table">
                <table cellpadding="0" cellspacing="0">
                    <thead>
                        <tr>
                            <th class="select-all">
                                <BaseCheckbox class="table-checkbox" :class="{ 'check-temp': selectedAccounts.length > 0 }"
                                    @check="selectAll"
                                    :checkedProp="this.checkList(this.listAccount, this.selectedAccounts) && listAccount.length !== 0">
                                    <template #checkmark>
                                        <div class="checkbox-checkmark"></div>
                                    </template>
                                </BaseCheckbox>
                            </th>

                            <th class="col-180 pointer" @click="sortData(Enum.SortAccount.FullNameAsc)">
                                <div class="flex-row">
                                    <p class="title-table">{{ Resource.Label.FullName }}</p>
                                    <div class="icon-sort" :class="{
                                        'desc': this.sort == Enum.SortAccount.FullNameDesc,
                                        'asc': this.sort == Enum.SortAccount.FullNameAsc
                                    }"></div>
                                </div>
                            </th>

                            <th class="col-180 pointer" @click="sortData(Enum.SortAccount.UsernameAsc)">
                                <div class="flex-row">
                                    <p class="title-table">{{ Resource.Label.Username }}</p>
                                    <div class="icon-sort" :class="{
                                        'desc': this.sort == Enum.SortAccount.UsernameDesc,
                                        'asc': this.sort == Enum.SortAccount.UsernameAsc
                                    }"></div>
                                </div>
                            </th>

                            <th class="col-180 ">
                                <div class="flex-row">
                                    <p class="title-table ">{{ Resource.Label.Email }}</p>
                                    
                                </div>
                            </th>

                            <th class="col-120 ">
                                <div class="flex-row">
                                    <p class="title-table">{{ Resource.Label.Phone }}</p>
                                </div>
                            </th>
                            <th class="col-var">
                                <div class="flex-row">
                                    <p class="title-table">{{ Resource.Label.Address }}</p>
                                </div>
                            </th>
                            <th class="col-100 pointer" @click="sortData(Enum.SortAccount.RoleAsc)">
                                <div class="flex-row">
                                    <p class="title-table ">{{ Resource.Label.Role }}</p>
                                    <div class="icon-sort" :class="{
                                        'desc': this.sort == Enum.SortAccount.RoleDesc,
                                        'asc': this.sort == Enum.SortAccount.RoleAsc
                                    }"></div>
                                </div>
                            </th>
                            <th class="col-120 pointer" @click="sortData(Enum.SortAccount.StatusAsc)">
                                <div class="flex-row">
                                    <p class="title-table ">{{ Resource.Label.Status }}</p>
                                    <div class="icon-sort" :class="{
                                        'desc': this.sort == Enum.SortAccount.StatusDesc,
                                        'asc': this.sort == Enum.SortAccount.StatusAsc
                                    }"></div>
                                </div>
                            </th>

                            <th class="col-150 pointer" @click="sortData(Enum.SortAccount.TimeAsc)">
                                <div class="flex-row">
                                    <p class="title-table ">{{ Resource.Label.CreatedDate }}</p>
                                    <div class="icon-sort" :class="{
                                        'desc': this.sort == Enum.SortAccount.TimeDesc,
                                        'asc': this.sort == Enum.SortAccount.TimeAsc
                                    }"></div>
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
                        <tr v-for="(account, index) in this.listAccount" @click.exact="focusIndex = index"
                            :class="{ 'selected-tr': focusIndex == index }" :key="account.accountID"
                            @dblclick="editAcount(account.accountID)">

                            <td class="select-all">
                                <BaseCheckbox class="table-checkbox" @check="selectRow(account)" :checkedProp="selectedAccounts.some(
                                    (obj) => obj.accountID == account.accountID
                                )">
                                    <template #checkmark>
                                        <div class="checkbox-checkmark"></div>
                                    </template>
                                </BaseCheckbox>
                            </td>

                            <td >
                                {{ account.fullName }}
                                
                            </td>

                            <td>
                                {{ account.username }}
                            </td>

                            <td >
                                {{ account.email }}
                            </td>

                            <td class="col-center">
                                {{ account.phone }}
                            </td>
                            <td >
                                {{ account.address }}
                            </td>
                            <td >
                                {{ configRole(account.role) }}
                            </td>
                            <td >
                                {{ configStatus(account.status) }}
                            </td>

                            <td>
                                {{ convertTime(account.createdDate) }}
                            </td>

                            <td>
                                <div class="row-actions">

                                    <el-tooltip effect="dark" :content="Resource.Tooltip.Edit" placement="top"
                                        hide-after="0" class="d-flex">
                                        <div @click="editAcount(account.accountID)" class="item">
                                            <div class="icon-edit"></div>
                                        </div>
                                    </el-tooltip>

                                    <el-tooltip effect="dark" :content="Resource.Tooltip.Delete" placement="top"
                                        hide-after="0" class="d-flex">
                                        <div @click="deleteSingle(account)" class="item">
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
                 @setPageSize="setPageSize" @prePage="goToPrePage" @nextPage="goToNextPage" @updatePageNow="updatePageNow"/>
            
        </div>

    </div>
    <FormAccountDetail @refreshData="getData" @showToast="showToast" :formMode="formMode" :accountIdUpdate="accountIdUpdate"
        :closeFormDetail="closeFormDetail" v-if="isShowFormDetail" />

    <BasePopup @close="closePopup" class="popup-delete" :title="this.titlePopup"
        :prefix="prefixPopup" :suffix="suffixPopup" :content="nameAccountDelete" v-show="isShowPopup">
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
import BaseCombobox from "../../components/base/BaseCombobox.vue";
import BaseCheckbox from "../../components/base/BaseCheckbox.vue";
import BaseToastMessage from "../../components/base/BaseToastMessage.vue";
import BasePopup from "../../components/base/BasePopup.vue";
import BaseButton from "../../components/base/BaseButton.vue";
import BasePaging from '@/components/base/BasePaging.vue';
import FormAccountDetail from '../ManageAccount/FormAccountDetail.vue';
import CommonFn from '@/utils/commonFuncion';
import moment from 'moment';
import 'moment/locale/vi'
export default {
    name: "AccountList",
    components: {
        BaseLoading, BaseButton, BaseCombobox, BaseCheckbox, BaseToastMessage, BasePopup, BasePaging, FormAccountDetail
    },
    data() {
        return {
            Resource: Resource,
            Enum: Enum,
            Const: Const,
            focusIndex: Enum.FirstIndexFocus, // chỉ số của tài khoản focus vào

            showIconX: false, // cờ thể hiện trạng thái ẩn hiện của iconX

            titlePopup: Resource.Title.DeleteAccount, // tiêu đề của popup

            isShowFormDetail: false, // cờ điền khiển đóng mở form thêm mới

            isShowPopup: false, // cờ điền khiển đóng mở cảnh báo xóa

            isLoading: false, // cờ điều khiển bật tắt trạng thái loading

            isShowFilterBox: false, // cờ điền khiển đóng mở bộ lọc

            formMode: "", // chức năng của form

            accountNow: {}, //tài khoản đang được chọn hiện tại 

            listAccount: [], // danh sách tài khoản thi đua đang được hiển thị

            selectedAccounts: [], // danh sách tài khoản đang được chọn

            keyword: "", // từ khóa để tìm kiếm tài khoản

            totalCount: 0, // tổng số tài khoản

            pageNumber: Enum.FirstPage, // trang đang hiển thị

            pageSize: Enum.PageSize, // số tài khoản hiển thị trong 1 trang

            nameAccountDelete: null, // tên tài khoản muốn xóa

            prefixPopup: null, //

            suffixPopup: null,//

            accountIdUpdate: null, // id tài khoản muốn cập nhật

            toastMessage: { // cảnh báo
                message: "", // nội dung cảnh báo
                type: "", // loại cảnh báo
                isShowed: false, // cờ điều khiển bật tắt cảnh báo
            },

            deleteMultiple: false, // cờ thể hiện có phải đang xóa nhiều hay không

            listAccountID: [], // danh sách ID của các tài khoản 

            listSize: Enum.ListSize, // danh sách gía trị số tài khoản hiển thị trong 1 trang

            filter: { // danh sách các điều kiện để lọc tài khoản
                
            },
            filterDefault: { 
                role: Enum.Role.All,
                status: ""
            },
            
            isFilter: false, // cờ thể hiện có đang lọc hay là không

            sort: Enum.SortAccount.All, // điều kiện sắp xếp

        };
    },
    methods: {

        /**
         * Lấy dữ liệu tài khoản 
         * Author: HAQUAN(26/08/2023)
         */
        async getData() {
            try {
                debugger
                this.isLoading = true;
                let url = `Account/filter?role=${this.filter.role}&status=${this.filter.status}&keyword=${this.keyword}&sort=${this.sort}&pageSize=${this.pageSize}&pageNumber=${this.pageNumber}`;
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
                        if (this.listAccount.length == 0 && this.pageNumber > Enum.FirstPage) { // nếu không lấy được tài khoản nào thì
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
            this.listAccount = value.data.data;
            this.totalCount = value.data.totalCount;
            this.focusIndex = Enum.FirstIndexFocus;
            this.accountNow = {};

            // cập nhật danh sách những tài khoản đã chọn
            if (this.selectedAccounts.length > 0) {
                for (let i = 0; i < this.selectedAccounts.length; i++) {
                    this.listAccount.find(
                        (item) => {
                            if (item.accountID == this.selectedAccounts[i].accountID) {
                                this.selectedAccounts[i] = item;
                            }
                        }
                    );
                }
            }
        },


        /**
        * Set giá trị text danh mục tài khoản về id danh mục tài khoản
        * @param {string} value :  giá trị danh mục tài khoản dạng text người dùng chọn
        * Author: HAQUAN(29/08/2023) 
        */
        setRoleFilter(value) {
            Const.RoleFilter.find((item) => {
                if (item.Label == value) {
                    this.filter.role = item.Value;
                }
            });
        },

        configRole(value) {
            let result = value;
            Const.RoleFilter.find((item) => {
                if (item.Value === value) {
                    result = item.Label;
                }
            });
            return result;

        },

        setStatusFilter(value) {
            Const.StatusFilter.find((item) => {
                if (item.Label == value) {
                    this.filter.status = item.Value;
                }
            });
        },

        configStatus(value) {
            let result = value;
            Const.StatusFilter.find((item) => {
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
         * Thêm mới tài khoản 
         * Author: HAQUAN(28/08/2023)
         */
        addAccount() {
            try {
                this.formMode = Enum.Mode.Add;
                this.showFormDetail();
            } catch (error) {
                console.log(error);
            }
        },

        /**
         * Sửa thông tin tài khoản 
         * Author: HAQUAN(28/10/2022)
         */
        editAcount(accountID) {
            try {
                this.formMode = Enum.Mode.Edit;
                this.accountIdUpdate = accountID;
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
         * Tìm kiếm tài khoản 
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
        * Sắp xếp tài khoản
        * @param {byte} sortCondition :  giá trị điều kiện sắp xếp
        * Author: HAQUAN(28/10/2022)
        */
        sortData(sortCondition) {
            if (this.sort == sortCondition) {
                this.sort++;
            }
            else if(this.sort - sortCondition == 1){
                this.sort = Enum.SortAccount.All;
            }
            else {
                this.sort = sortCondition;
            }
            this.getData();
        },

        /**
         * Xóa 1 tài khoản 
         * @param {*} account :  tài khoản đang được chọn để xóa
         * Author: HAQUAN(28/10/2022)
         */
        deleteSingle(account) {
            try {
                this.deleteMultiple = false;
                this.nameAccountDelete = account.username;
                this.prefixPopup = Resource.Text.CanDeleteAccount;
                this.suffixPopup = Resource.Text.SelectedNo;
                this.accountNow = account;
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

                // Nếu chỉ có 1 tài khoản được chọn, xóa một tài khoản
                if (this.selectedAccounts.length == 1) {
                    this.deleteSingle(this.selectedAccounts[0]);
                }

                // Ngược lại nếu có nhiều hơn một tài khoản thì xóa nhiều
                else if (this.selectedAccounts.length > 1) {

                    // Thực hiện ánh xạ danh sách tài khoản đang được chọn
                    // sang danh sách ID tài khoản 
                    this.listAccountID = [];
                    this.selectedAccounts.filter((account) => {
                        this.listAccountID.push(account.accountID)
                    });

                    this.titlePopup = Resource.Title.DeleteAccount;
                    this.nameAccountDelete = this.selectedAccounts.length + " " + Resource.Text.Account;
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
         * Hàm thực hiện xóa tài khoản 
         * Author: HAQUAN(28/10/2022)
         */
        deleteAcount() {
            if (this.deleteMultiple) {
                this.requestDeleteMultiple();
            }
            else {
                this.requestDeleteSingle(this.accountNow.accountID)
            }
        },

        /**
         * Gửi yêu cầu xóa 1 tài khoản đến Server
         * Author: HAQUAN(28/10/2022)
         */
        async requestDeleteSingle(accountID) {
            try {
                let url = `Account/${accountID}`
                await axios.delete(url).then((response) => {
                    console.log(response);
                    // nếu là xóa 1 thì bỏ tài khoản đã xóa khỏi danh sách các tài khoản đang được chọn
                    this.selectedAccounts = this.selectedAccounts.filter(
                        (account) => account.accountID !== accountID
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
         * Gửi yêu cầu xóa nhiều tài khoản đến Server
         * Author: HAQUAN(28/10/2022)
         */
        async requestDeleteMultiple() {
            try {
                await axios({
                    url: 'Account/multiple',
                    method: 'delete',
                    data: this.listAccountID,
                }).then((response) => {
                    console.log(response);
                    // Sau khi xóa thành công, nếu là xóa nhiều thì gán danh sách các tài khoản đã chọn về rỗng
                    this.selectedAccounts = [];

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
            this.accountNow = {};
        },

        /**
         * Đóng popup 
         *  Author: HAQUAN(28/10/2022)
         */
        closePopup() {
            this.isShowPopup = false;
        },

        /**
         * Đóng bộ lọc tài khoản 
         *  Author: HAQUAN(28/10/2022)
         */
        closeFilterBox() {
            this.isShowFilterBox = false;
        },

        /**
         * Thực hiện lọc tài khoản theo giá trị mặc định
         *  Author: HAQUAN(28/10/2022)
         */
        filterProductDefault() {
            this.isFilter = false;
            this.isShowFilterBox = false;
            this.filter = Object.assign({},this.filterDefault);
            this.pageNumber = Enum.FirstPage;

            this.getData();
        },

        /**
         * Thực hiện lọc tài khoản theo giá trị đã chọn
         *  Author: HAQUAN(28/10/2022)
         */
        filterAccount() {
            this.isShowFilterBox = false;
            this.isFilter = true;
            this.pageNumber = Enum.FirstPage;

            this.getData();
        },

        /**
         * Xử lý khi một hàng được chọn
         * @param {*} accountNow: tài khoản đang được chọn
         * Author: HAQUAN(28/10/2022)
         */
        selectRow(accountNow) {
            try {

                // Nếu như hàng được chọn đã được chọn trước đó thì loại bỏ khỏi danh sách
                if (this.selectedAccounts.find((item) => item.accountID === accountNow.accountID)) {
                    this.selectedAccounts = this.selectedAccounts.filter(
                        (account) => account.accountID !== accountNow.accountID
                    );
                }

                // Nếu như hàng được chọn chưa được chọn trước đó thì thêm vào danh sách
                else {
                    this.selectedAccounts.push(accountNow);
                }
            } catch (error) {
                console.log(error);
            }
        },

        /**
         * Xử lý bỏ chọn tất cả các tài khoản đã chọn 
         * Author: HAQUAN(28/10/2022)
         */
        unselectAll() {
            this.focusIndex = Enum.FirstIndexFocus;
            this.selectedAccounts = [];
            this.accountNow = {};
        },

        /**
         * Kiểm tra 2 danh sách tài khoản
         * @param {*} firstList: danh sách tài khoản cần so sánh
         * @param {*} secondList: danh sách tài khoản cần so sánh
         * Author: HAQUAN(28/10/2022)
         */
        checkList(firstList, secondList) {

            if (firstList.length == 0 || secondList.length == 0) {
                return false;
            }

            try {
                let count = 0;
                for (let item of firstList) {
                    if (secondList.some((obj) => obj.accountID == item.accountID)) {
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
         * Xử lý chọn tất cả các tài khoản đang được hiển thị
         * Author: HAQUAN(28/10/2022)
         */
        selectAll() {
            try {

                // Kiểm tra xem những tài khoản đang hiển thị có hết trong danh sách những tài khoản đã được chọn hay không
                if (this.checkList(this.listAccount, this.selectedAccounts)) { // Nếu phải thì bỏ hết những tài khoản đó ra khỏi danh sách đã chọn
                    for (let account of this.listAccount) {
                        this.selectedAccounts = this.selectedAccounts.filter(
                            (item) => item.accountID != account.accountID
                        );
                    }
                }
                else { // Ngược lại thì thêm những tài khoản chưa ở trong danh sách đã chọn vào danh sách các tài khoản đã chọn

                    for (let item of this.listAccount) {
                        if (!this.selectedAccounts.some((obj) => obj.accountID == item.accountID)) {
                            this.selectedAccounts.push(item);
                        }
                    }
                }

            } catch (error) {
                console.log(error);
            }
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
        convertTime(value) {
            return moment(value).format('L');
        },
    },
    created() {
    // Lấy danh sách tài khoản
    this.filterProductDefault();
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
@import url(../../css/base/icon.css);
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
    background: url(../../assets/icon/icon-x.png) ;
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
.list-account {
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
    /* height: 100%; */
    height: 470px;
}
.table thead {
    position: sticky;
    top: 0;
    z-index: 3;
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

.checkbox:not(.check-temp) input:not(:checked):hover ~ .checkbox-checkmark,
.checkbox:not(.check-temp) input:not(:checked):focus ~ .checkbox-checkmark {
  top: 9px;
  left: 0px;
  width: 22px;
  height: 22px;
  background-image: url(https://cegovapp.misacdn.net/r/cegov/img/ic_checkbox_hover.d04300f4.svg);
}

.checkbox > .checkbox-input:checked ~ .checkbox-checkmark {
  background-image: url(https://cegovapp.misacdn.net/r/cegov/img/ic_checkbox_active.14fc601f.svg);
}
.checkbox-checkmark:after {
  content: "";
  position: absolute;
  display: none;
}
.checkbox .checkbox-input:checked ~ .checkbox-checkmark:after {
  display: block;
}

.checkbox .checkbox-input:checked:disabled ~ .checkbox-checkmark {
  width: 24px;
  height: 24px;
  background-image: url(https://cegovapp.misacdn.net/r/cegov/img/ic_checkbox_disabled_active.c6062b71.svg);
}

.checkbox .checkbox-input:disabled ~ .checkbox-checkmark {
  width: 24px;
  height: 24px;
  background-image: url(https://cegovapp.misacdn.net/r/cegov/img/ic_checkbox_disabled.7177f2a0.svg);
}
.col-180{
    max-width: 180px ;
    min-width: 180px;
}
.col-100{
    max-width: 100px;
    min-width: 100px;
}
.col-120{
    min-width: 120px;
    max-width: 120px;
}
.col-360{
    min-width: 360px;
    max-width: 360px;
}
.col-var {
  width: 100%;
  min-width: 100px;
}
.body{
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
  right: 0;
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
  right: 4px;
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
thead tr th:first-child, tbody tr td:first-child{
    position: sticky;
    left: 0;
}
thead tr th:last-child, tbody tr td:last-child{
    position: sticky;
    right: 0;
    border-left: 1px solid #e0e1ef;
}
</style>