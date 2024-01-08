<template>
    <div>
        <form @keydown.esc="closeForm" class="modal-detail-wrapper" tabindex="0" @keydown.ctrl.s.prevent.exact="save(false)"
            @keydown.ctrl.shift.s.prevent.exact="save(true)">

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

                <div class="modal-detail-content">
                    <div class="w-full flex-column">

                        <div class="row-form">

                            <label for="" class="label-form d-flex">
                                {{ Resource.Label.Supplier }}
                            </label>

                            <div class="flex combobox-form">
                                <BaseCombobox tabindex="1" :isCheck="errors.supplier != '' ? true : false" :hasIcon="true" 
                                    :items="listSupplier" :initItem="importModel.infoImport.supplierName" fieldName="supplierName"
                                     @getValue="setSupplier" :placehoder="Resource.Placehoder.Supplier" />
                                <div class="error-text">{{ errors.supplier }}</div>
                            </div>
                        </div>
                        <div class="row-form">
                            <label for="" class="label-form flex-row space-between ">
                                {{ Resource.Label.Product }}
                                <BaseButton @click="addProduct" class="button-blue btn ml-10">
                                    {{ Resource.Button.AddProduct }}
                                </BaseButton>
                            </label>
                            <div class="list-product" v-show="importModel.listImportDetail.length != 0">
                                <div class="item" v-for="item in importModel.listImportDetail" :key="item.productID">
                                    <CartItem :initItem="item" @updateValue="updateQuantity"  @updatePrice="updatePrice"
                                    @deleteItem="deleteItem" :isImport="true" />
                                </div>
                            </div>

                        </div>


                        <div class="row-form d-flex align-items space-between">
                            <b> {{ Resource.Label.Total }}</b>
                            <span>
                                {{ formatMoney(totalMoney) }}
                            </span>
                        </div>
                    </div>
                </div>

                <div class="modal-footer">
                    <div class="flex-row flex-end">

                        <BaseButton tabindex="4"  @mousedown="closeForm"
                            @keydown.enter="closeForm" class="sub-button btn">
                            {{ Resource.Button.Cancel }}
                        </BaseButton>

                        <el-tooltip effect="dark" :content="Const.KeyStrokes.CtrlShiftS" placement="top">
                            <BaseButton tabindex="3" @click="save(true)" @keydown.enter="save(true)"
                                v-show="!checkForm() && importModel.listImportDetail.length > 0"
                                class="button-blue btn ml-10">
                                {{ Resource.Button.SaveAndNew }}
                            </BaseButton>
                        </el-tooltip>

                        <el-tooltip effect="dark" :content="Const.KeyStrokes.CtrlS" placement="top">
                            <BaseButton tabindex="2" @click="save(false)" @keydown.enter="save(false)"
                                v-show="!checkForm() && importModel.listImportDetail.length > 0"
                                class="main-button btn ml-10">{{ Resource.Button.Save }}</BaseButton>
                        </el-tooltip>

                        <el-tooltip effect="dark" :content="Const.KeyStrokes.CtrlS" placement="top">
                            <BaseButton tabindex="2" @click="save(false)" @keydown.enter="save(false)"
                                v-show="checkForm() && importModel.listImportDetail.length > 0" style="padding: 0 16px;"
                                class="main-button btn ml-10">{{ Resource.Button.SaveChange }}
                            </BaseButton>
                        </el-tooltip>

                    </div>
                </div>
            </div>
        </form>
    </div>

    <AddProduct :closeFormAddProduct="closeFormAddProduct" @setListProduct="setListProduct" v-if="isShowFormAddProduct" :isImport="true"/>

    <BasePopup @close="closePopup" class="popup-delete" :title="titlePopup" :content="contentPopup" v-if="isShowPopup">

        <template #buttons>
            <BaseButton @click="closePopup" class="main-button-red btn ml-10">{{ Resource.Button.Close }}</BaseButton>
        </template>

    </BasePopup>
    <BaseToastMessage v-show="toastMessage.isShowed" :class="`toast-${toastMessage.type} icon-toast-${toastMessage.type}`">

        <template #message>{{ toastMessage.message }}</template>
    </BaseToastMessage>
</template>
<script>
import Const from '@/utils/const'
import Enum from '@/utils/enum'
import Resource from '@/utils/resource'
import BaseCombobox from "../../components/base/BaseCombobox.vue";
import BaseButton from "../../components/base/BaseButton.vue";
import BasePopup from "../../components/base/BasePopup.vue";
import CartItem from '@/components/base/CartItem.vue';
import CommonFn from '@/utils/commonFuncion';
import axios from 'axios';
import AddProduct from '../Modal/AddProduct.vue';
import BaseToastMessage from '@/components/base/BaseToastMessage.vue';
export default {
    components: {
        BaseCombobox, BaseButton, BasePopup, CartItem, AddProduct, BaseToastMessage
    },
    props: ["closeFormDetail", "importProductIdUpdate", "formMode"],
    data() {
        return {
            Enum: Enum,
            Resource: Resource,
            Const: Const,
            isTitle: "", // tiêu đề của form

            titlePopup: Resource.Title.Management, // tiêu đề của popup

            contentPopup: "", // nội dung cảnh báo

            isShowPopup: false, // cờ điền khiển đóng mở cảnh báo lỗi

            isShowFormAddProduct: false,

            errors: { // danh sách lỗi validate của các trường thông tin
                supplier: ''
            },

            importModelInit: {
                infoImport: {
                    supplierID: '',
                    supplierName: '',
                    amount: 0,
                    price: 0,
                },
                listImportDetail: [],
            },

            importModel: {}, // thông tin đơn nhập hàng trong form thêm mới

            listSupplier: [], //Danh sách nhà cung cấp
           
            decreaseMoney: 0,

            toastMessage: { // cảnh báo
                message: "", // nội dung cảnh báo
                type: "", // loại cảnh báo
                isShowed: false, // cờ điều khiển bật tắt cảnh báo
            },
        };
    },

    computed: {
        totalMoney: function () {
            let total = 0;
            this.importModel.listImportDetail.forEach(element => {
                total += element.priceImport * element.quantity;
            });
            return total;
        }
    },

    watch: {
        // 'decreaseMoney': {
        //     handler: function (newVal) {
        //         if (newVal != undefined) {
        //             this.importModel.infoImport.total = this.totalMoney - newVal;
        //         }
        //     }
        // },
        'importModel.listImportDetail.length': {
            handler: function (newVal) {
                this.importModel.infoImport.amount = newVal;
            }
        },
    },

    methods: {
        /**
        * Set giá trị nhà cung cấp cho thông tin nhập hàng
        * @param {string} value :  nhà cung cấp người dùng chọn
        * Author: HAQUAN(29/08/2023) 
        */
        setSupplier(value) {
            let check = false;
            this.listSupplier.find((item) => {
                if (item.supplierName == value) {
                    this.importModel.infoImport.supplierID = item.supplierID;
                    this.importModel.infoImport.supplierName = item.supplierName;
                    this.errors.supplier = '';
                    check = true;
                }
            });
            if (!check) {
                if (value == "") {
                    this.errors.supplier = Resource.Error.Supplier;
                }
                else {
                    // Lấy danh sách nhà cung cấp có chứa giá trị value
                    let checkListSupplier = this.listSupplier.filter((item) => {
                        if (item.supplierName.toLowerCase().includes(value.toLowerCase())) {
                            return item;
                        }
                    });

                    // Nếu danh sách không có phần tử nào, hiển thị lỗi nhà cung cấp không có trong danh sách
                    if (checkListSupplier.length == 0) {
                        this.errors.supplier = Resource.Error.SupplierNotFound;
                    }
                }
            }
        },

        /**
         * Lấy dữ liệu nhà cung cấp
         * Author: HAQUAN(26/08/2023)
         */
        async getData() {
            try {
                await axios.get('Supplier/getAll')
                    .then((response) => {
                        console.log(response);
                        this.listSupplier = response.data;
                    })

                    .catch((error) => {
                        console.log(error);
                    })
            } catch (error) {
                console.log(error);
            }
        },

        formatMoney(money) {
            return money.toLocaleString('vi-VN') + ' đ';
        },

        updateQuantity(productID, quantity) {
            this.importModel.listImportDetail.find(
                (item) => {
                    if (item.productID == productID) {
                        item.quantity = quantity;
                    }
                }
            );
            this.importModel.infoImport.price = this.totalMoney;
        },

        updatePrice(productID, price) {
            this.importModel.listImportDetail.find(
                (item) => {
                    if (item.productID == productID) {
                        item.priceImport = price;
                    }
                }
            );
            this.importModel.infoImport.price = this.totalMoney;
        },

        deleteItem(productID) {
            this.importModel.listImportDetail = this.importModel.listImportDetail.filter(
                (item) => {
                    if (item.productID != productID) {
                        return item;
                    }
                }
            );
            this.importModel.infoImport.price = this.totalMoney;
        },
        
        /**
         * Kiểm tra xem có phải form sửa hay không
         * Author: HAQUAN(29/08/2023) 
         */
        checkForm() {
            if (this.formMode === Enum.Mode.Edit) {
                return true;
            }
            return false;
        },

        /**
         * Đóng form 
         * Author: HAQUAN(29/08/2023) 
         */
        closeForm() {
            this.closeFormDetail();
        },

        /**
         * Mở form thêm sản phẩm vào đơn nhập hàng 
         * Author: HAQUAN(29/08/2023) 
         */
        addProduct() {
            this.isShowFormAddProduct = true;
        },

        setListProduct(listProduct) {
            try {
                listProduct.forEach(
                    (e) => {
                        if (!this.importModel.listImportDetail.some((p) => p.productID == e.productID)) {
                            this.importModel.listImportDetail.push(e);
                        }
                    }
                );
                this.importModel.infoImport.price = this.totalMoney;
                this.showToast(Resource.Message.AddProductSucces, Const.TypeToast.Success);
            } catch (error) {
                console.log(error);
            }

        },

        closeFormAddProduct() {
            this.isShowFormAddProduct = false;
        },

        /**
         * Lưu thông tin đơn nhập hàng
         * @param {*} control: kiểu lưu đơn nhập hàng, nếu false thì lưu và đóng form, còn true thì lưu và reset dữ liệu, không đóng form
         *  Author: HAQUAN(29/08/2023)  
         */
        save(control) {
            try {
                let valid = this.validate();
                if (valid) {
                    // Nếu như là chỉnh sửa thông tin đơn nhập hàng
                    if (this.formMode == Enum.Mode.Edit) {
                        this.sendRequestUpdate();
                    }

                    else {
                        this.sendRequestInsert(control);
                    }

                }
            } catch (error) {
                console.log(error);
            }
        },

        /**
         * Gửi request chỉnh sửa thông tin đơn nhập hàng đến server
         *  Author: HAQUAN(29/08/2023) 
         */
        async sendRequestUpdate() {
            try {
                this.importModel.infoImport.modifiedBy = this.$store.getters.user.accountID;
                await axios.put("ImportProduct/" + this.importModel.infoImport.importID, this.importModel)
                    .then((response) => {
                        console.log(response);

                        // Hiển thị toast thông báo thành công
                        this.$emit("showToast", Resource.Message.UpdateOrderSucces, Const.TypeToast.Success);
                        this.closeFormDetail();
                        this.$emit("refreshData");
                    })

                    .catch((error) => {
                        console.log(error);
                        this.handleErrorResponse(error);
                    });
            } catch (error) {
                console.log(error);
            }
        },

        /**
         * Gửi request thêm mới đơn nhập hàng
         * @param {*} control: kiểu lưu đơn nhập hàng, nếu false thì lưu và đóng form, còn true thì lưu và reset dữ liệu, không đóng form
         *  Author: HAQUAN(29/08/2023) 
         */
        async sendRequestInsert(control) {
            try {
                await axios.post('ImportProduct/', this.importModel).
                    then((response) => {
                        console.log(response);

                        // Hiển thị toast thông báo thành công
                        this.$emit("showToast", Resource.Message.AddAccountSucces, Const.TypeToast.Success);
                        this.$emit("refreshData");
                        if (!control) {
                            this.closeFormDetail();
                        }
                        else {
                            this.importModel = { ...this.importModelInit };
                            this.importModel.infoImport.createdBy = this.$store.getters.user.accountID;
                        }
                    })
                    .catch((error) => {
                        console.log(error);
                        this.handleErrorResponse(error);
                    });
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

        validateSupplier() {
            if (!this.importModel.infoImport.supplierName) {
                this.errors.supplier = Resource.Error.Supplier;
                return false;
            }
            this.errors.supplier = '';
            return true;
        },
       
        validate() {
            try {
                let valid = true;
                if (!this.validateSupplier()) {
                    valid = false;
                }
                return valid;

            } catch (error) {
                console.log(error);
                return false;
            }
        },

        /**
         * Lấy thông tin đơn nhập hàng theo ID
         * @param {*} importID : ID của đơn nhập hàng muốn lấy
         * Author: HAQUAN(29/08/2023) 
         */
        async getImportByID(importID) {
            try {
                this.isLoading = true;
                let url = `ImportProduct/${importID}`;
                await axios.get(url)
                    .then((response) => {
                        this.importModel.infoImport = response.data.data.infoImport;
                        this.importModel.listImportDetail = response.data.data.listImportDetail;
                        console.log(this.importModel.listImportDetail);
                    })

                    .catch((error) => {
                        console.log(error);
                        this.handleErrorResponse(error);
                    })

                    .finally(() => {
                        this.isLoading = false;
                    })
            } catch (error) {
                console.log(error);
            }
        },

        /**
         * Đóng popup 
         *  Author: HAQUAN(29/08/2023) 
         */
        closePopup() {
            this.isShowPopup = false;
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
        this.getData();
        this.importModel = { ...this.importModelInit };
        this.importModel.infoImport.createdBy = this.$store.getters.user.accountID;
        if (this.formMode == Enum.Mode.Edit) {
            this.isTitle = Resource.Title.EditImport;
            this.getImportByID(this.importProductIdUpdate);
        }
    },
    mounted() {
        if (this.formMode == Enum.Mode.Add) {
            this.isTitle = Resource.Title.AddImport;
        }
    },

};
</script>
<style scoped>
@import url(../../css/base/radio.css);

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

.row-form {
    margin-bottom: 16px;
}

.label-form {
    color: #000;
    font-size: 14px;
    font-weight: 400;
    min-height: 20px;
    margin-bottom: 8px;
    padding: 0;
    width: auto;
}

.required {
    color: #e54848;
    margin-left: 5px;
}

.text-field-form {
    background: #fff;
    padding: 0;
    width: 100%;
    border-radius: 3.5px;
}

.text-aria-form {
    background: #fff;
    align-items: center;
    border-radius: 3.5px;
}

.aria-form {
    border: none;
    padding: 6px 12px;
    resize: none;
    background: 0 0;
}

.border {
    border: 1px solid #e0e0e0;
    border-radius: 3.5px;
}

.border.disabled {
    background-color: #ebebeb !important;
    color: #666;
}

:focus-visible {
    outline: none;
}

.border:not(.disabled):not(.error):focus-within,
.border:not(.disabled):not(.error):hover {
    border: 1px solid #1a73e8;
}

.border.error {
    border: 1px solid #ef5350;
}

.error-text {
    color: #ef5350;
    margin-top: 6px;
}

.border.error:focus-within {
    border: 1px solid #ef5350;
}

.checkbox-form {
    height: 34px;
    margin-bottom: 9px;
}

.combobox-form {
    width: 100%;
    background-color: transparent;
}

.modal-footer {
    padding: 12px 24px;
    border-top: 1px solid #e0e0e0;
}

.radio-form {
    min-height: 35px;
    padding: 0;
    position: relative;
    align-items: center;
}

.list-product {
    padding: 12px 12px;
    border: 1px solid #c6c6c6;
}

.list-product .item:not(:last-child) {
    padding-bottom: 12px;
    border-bottom: 1px solid #c6c6c6;
}

.list-product .item:not(:first-child) {
    padding-top: 12px;
}
</style>
  