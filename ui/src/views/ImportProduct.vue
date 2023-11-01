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

            <div class="modal-detail-title">{{ Resource.Title.ImportProduct }}</div>

            <div class="modal-detail-content">

                <div class="w-full flex-column">
                    <div class="row-form flex">
                        <label for="" class="label-form d-flex">
                            {{ Resource.Label.Supplier }}
                            <span class="required">*</span>
                        </label>

                        <div class="flex combobox-form">
                            <BaseCombobox tabindex="1" :isCheck="errors.supplier != '' ? true : false" :hasIcon="true"
                                :items="listSupplier" :initItem="infoImport.supplierName" fieldName="supplierName"
                                @focusOut="validateSupplier" @getValue="setSupplier"
                                :placehoder="Resource.Placehoder.Supplier" />

                        </div>
                        <div class="error-text">{{ errors.supplier }}</div>
                    </div>

                    <div class="row-form flex">
                        <label for="" class="label-form d-flex">
                            {{ Resource.Label.Quantity }}
                            <span class="required">*</span>
                        </label>

                        <div class="flex text-field-form">
                            <InputNumber :transmissionNumber="infoImport.amount" :decimalPlaces="0" tabindex="2"
                                :hasIcon="true" :hasError="errors.amount != ''"
                                @eventBlur="focusAmount = false; validateAmount();" :isRef="focusAmount"
                                :nameProperty="Resource.ImportProductProperty.Amount" @update="updateValue" />
                            <div class="error-text">{{ errors.amount }}</div>
                        </div>
                    </div>
                    <div class="row-form flex">
                        <label for="" class="label-form d-flex">
                            {{ Resource.Label.Price }}
                            <span class="required">*</span>
                        </label>

                        <div class="flex text-field-form">
                            <InputNumber :transmissionNumber="infoImport.price" :decimalPlaces="0" tabindex="3"
                                :hasError="errors.price != ''" @eventBlur="focusPrice = false; validatePrice();"
                                :isRef="focusPrice" :nameProperty="Resource.ImportProductProperty.Price"
                                @update="updateValue" />
                            <div class="error-text">{{ errors.price }}</div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal-footer">
                <div class="flex-row flex-end">

                    <BaseButton @mousedown="closeForm" class="sub-button btn">
                        {{ Resource.Button.Close }}
                    </BaseButton>

                    <BaseButton @click="importProduct" class="main-button btn ml-10">
                        {{ Resource.Button.ImportProduct }}
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
import Const from '@/utils/const';
import CommonFn from '@/utils/commonFuncion';
import BaseCombobox from '@/components/base/BaseCombobox.vue';
import InputNumber from '@/components/base/BaseInputNumber.vue';
import BaseButton from '@/components/base/BaseButton.vue';
export default {
    name: 'ImportProduct',
    components: {
        BaseCombobox, InputNumber, BaseButton
    },
    props: ["closeFormImport", "productId"],
    data() {
        return {
            Resource: Resource,

            importProductInit: { // thông tin nhập hàng mặc định
                amount: 0,
                price: 0
            },

            infoImport: {}, // thông tin nhập hàng

            errors: { // danh sách lỗi validate của các trường thông tin
                supplier: '',
                amount: '',
                price: '',
            },

            focusAmount: false,
            focusPrice: false,
            listSupplier: [], // danh sách nhà cung cấp

        }
    },

    methods: {

        updateValue(value, nameProperty) {
            this.infoImport[nameProperty] = value;
        },

        /**
        * Set giá trị nhà cung cấp cho thông tin nhập hàng
        * @param {string} value :  nhà cung cấp người dùng chọn
        * Author: HAQUAN(29/08/2023) 
        */
        setSupplier(value) {
            let check = false;
            this.listSupplier.find((item) => {
                if (item.supplierName == value) {
                    this.infoImport.supplierID = item.supplierID;
                    this.infoImport.supplierName = item.supplierName;
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

        /**
        * Lưu thông tin nhập hàng
        *  Author: HAQUAN(29/08/2023)  
        */
        importProduct() {
            try {
                let valid = this.validate();
                if (valid) {
                    this.infoImport.productID = this.productId;
                    this.infoImport.createdBy = this.$store.getters.user.accountID;
                    this.sendRequestImport();

                }
            } catch (error) {
                console.log(error);
            }
        },

        /**
        * Gửi request thêm mới nhập hàng
        *  Author: HAQUAN(29/08/2023) 
        */
        async sendRequestImport() {
            try {
                await axios.post('ImportProduct/', this.infoImport)
                    .then((response) => {
                        console.log(response);
                        // Hiển thị toast thông báo thành công
                        this.$emit("showToast", Resource.Message.ImportProductSucces, Const.TypeToast.Success);
                        this.$emit("refreshData");
                        this.closeFormImport();
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
        * Đóng form 
        * Author: HAQUAN(29/08/2023) 
         */
        closeForm() {
            this.closeFormImport();
        },

        /**
        * Validate không để trống nhà cung cấp
        * Author: HAQUAN(29/08/2023) 
        */
        validateSupplier() {
            try {
                if (!this.infoImport.supplierID) {
                    this.errors.supplier= Resource.Error.Supplier;
                    return false;
                }   
                this.errors.supplier = '';
                return true;
            } catch (error) {
                console.log(error);
            }
        },

        /**
        * Validate số lượng
        * Author: HAQUAN(29/08/2023) 
        */
        validateAmount() {
            try {
                if (!this.infoImport.amount) {
                    this.errors.amount = Resource.Error.Quantity;
                    return false;
                }
                this.errors.amount = '';
                return true;
            } catch (error) {
                console.log(error);
            }
        },

        /**
        * Validate giá tiền
        * Author: HAQUAN(29/08/2023) 
        */
        validatePrice() {
            try {
                if (!this.infoImport.price) {
                    this.errors.price = Resource.Error.Price;
                    return false;
                }
                this.errors.price = '';
                return true;
            } catch (error) {
                console.log(error);
            }
        },

        /**
        * Validate thông tin nhập hàng trước khi lưu
        * Author: HAQUAN(29/08/2023)  
        */
        validate() {
            try {
                let valid = true;

                if (!this.validateSupplier()) {
                    valid = false;
                }

                if (!this.validatePrice()) {
                    this.focusPrice = true;
                    valid = false;
                }

                if (!this.validateAmount()) {
                    this.focusAmount = true;
                    valid = false;
                }

                return valid;
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
                            this.importProduct();
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

    },
    created() {
        this.infoImport = { ...this.importProductInit };
        this.infoImport.createdBy = this.$store.getters.user.accountID;
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
    width: 500px;
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
    height: 20px;
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

.modal-footer {
    padding: 12px 24px;
    border-top: 1px solid #e0e0e0;
}
</style>
    