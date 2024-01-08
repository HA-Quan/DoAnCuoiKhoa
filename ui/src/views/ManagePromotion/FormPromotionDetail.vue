<template>
    <div>
        <form @keydown.esc="closeForm" class="modal-detail-wrapper" tabindex="0" @keydown.ctrl.s.prevent.exact="save(false)"
            @keydown.ctrl.shift.s.prevent.exact="save(true)">

            <div class="modal-detail">

                <div class="buttons-header">

                    <el-tooltip effect="dark" :content="Resource.Tooltip.Help" placement="top">
                        <div @mousedown="printBill" class="button help"></div>
                    </el-tooltip>

                    <el-tooltip effect="dark" :content="Resource.Tooltip.Close" placement="top">
                        <div @mousedown="closeForm" class="button close"></div>
                    </el-tooltip>

                </div>

                <div class="modal-detail-title">{{ isTitle }}</div>

                <div class="modal-detail-content">
                    <div class="w-full flex-column">

                        <div class="row-form flex-row">
                            <div class="flex mr-10 block">
                                <label for="" class="label-form d-flex">
                                    {{ Resource.Label.PromotionCode }}
                                    <span class="required">*</span>
                                </label>

                                <div class="flex text-field-form">
                                    <InputString :transmissionString="promotion.promotionCode"
                                        :hasError="errors.promotionCode != ''"
                                        :placeholder="Resource.Placehoder.InfoGuest.PromotionCode" maxlength="20"
                                        tabindex="1" :nameProperty="Resource.PromotionProperty.PromotionCode"
                                        @updateValue="updateValue"
                                        @eventBlur="focusPromotionCode = false; validatePromotionCode();"
                                        :isRef="focusPromotionCode" />

                                    <div class="error-text">{{ errors.promotionCode }}</div>
                                </div>
                            </div>
                            <div class="flex mr-10 block">
                                <label for="" class="label-form d-flex">
                                    {{ Resource.Label.ValueVoucher }}
                                    <span class="required">*</span>
                                </label>

                                <div class="flex text-field-form">
                                    <InputNumber :transmissionNumber="promotion.discount" :decimalPlaces="0" tabindex="2"
                                        :hasError="errors.discount != ''" 
                                        :nameProperty="Resource.PromotionProperty.Discount"
                                        :placeholder="Resource.Placehoder.Discount" @update="updateValue"
                                        @eventBlur="focusDiscount = false; validateDiscount();" :isRef="focusDiscount" />

                                    <div class="error-text">{{ errors.discount }}</div>
                                </div>
                            </div>

                        </div>

                        <div class="row-form d-flex">
                            <div class="flex mr-10 block">
                                <label for="" class="label-form d-flex">
                                    {{ Resource.Label.Proviso }}
                                    <span class="required">*</span>
                                </label>

                                <div class="flex text-field-form">
                                    <InputNumber :transmissionNumber="promotion.proviso" :decimalPlaces="0" tabindex="3"
                                        :hasError="errors.proviso != ''" 
                                        :nameProperty="Resource.PromotionProperty.Proviso"
                                        :placeholder="Resource.Placehoder.Proviso" @update="updateValue"
                                        @eventBlur="focusProviso = false; validateProviso();" :isRef="focusProviso" />

                                    <div class="error-text">{{ errors.proviso }}</div>
                                </div>
                            </div>
                            <div class="flex mr-10 block">
                                <label for="" class="label-form d-flex">
                                    {{ Resource.Label.Status }}
                                    <span class="required">*</span>
                                </label>

                                <div class="flex-row flex radio-form">
                                    <div class="flex">
                                        <BaseRadio tabindex="4" @check="promotion.status = Const.UseStatus.Using.Value"
                                            :checkedProp="promotion.status == Const.UseStatus.Using.Value"
                                            :label="Const.UseStatus.Using.Label">
                                            <template #radiomark>
                                                <div class="radio-checkmark"></div>
                                            </template>
                                        </BaseRadio>

                                    </div>
                                    <div class="flex">
                                        <BaseRadio tabindex="5" @check="promotion.status = Const.UseStatus.StopUsing.Value"
                                            :checkedProp="promotion.status == Const.UseStatus.StopUsing.Value"
                                            :label="Const.UseStatus.StopUsing.Label">
                                            <template #radiomark>
                                                <div class="radio-checkmark"></div>
                                            </template>
                                        </BaseRadio>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row-form d-flex">
                            <div class="flex mr-10 block">
                                <label for="" class="label-form d-flex">
                                    {{ Resource.Label.DayStart }}
                                    <span class="required">*</span>
                                </label>
                                <div class="flex text-field-form">
                                    <el-date-picker v-model="promotion.dayStart" type="date"
                                        :placeholder="Resource.Placehoder.DayStart" :format="Const.DateFormat"
                                        value-format="YYYY-MM-DD" :class="[{ 'input-error': errors.dayStart }]"
                                        style="width: 100%;" @change="validateDayStart()" tabindex="6">
                                    </el-date-picker>
                                    <div class="error-text">{{ errors.dayStart }}</div>
                                </div>
                            </div>

                            <div class="flex mr-10 block">
                                <label for="" class="label-form d-flex">
                                    {{ Resource.Label.DayExpired }}
                                    <span class="required">*</span>
                                </label>
                                <div class="flex text-field-form">
                                    <el-date-picker v-model="promotion.dayExpired" type="date"
                                        :placeholder="Resource.Placehoder.DayExpired" :format="Const.DateFormat"
                                        value-format="YYYY-MM-DD" :class="[{ 'input-error': errors.dayExpired }]"
                                        style="width: 100%;" @change="validateDayExpired()" tabindex="7">
                                    </el-date-picker>
                                    <div class="error-text">{{ errors.dayExpired }}</div>
                                </div>
                            </div>

                        </div>

                        <div class="row-form d-flex">
                            <div class="flex mr-10 block" style="max-width: 49%;">
                                <label for="" class="label-form d-flex">
                                    {{ Resource.Label.Quantity }}
                                    <span class="required">*</span>
                                </label>
                                <div class="flex text-field-form">
                                    <InputNumber :transmissionNumber="promotion.quantity" :decimalPlaces="0" tabindex="8"
                                        :hasError="errors.quantity != ''" :hasIcon="true"
                                        :nameProperty="Resource.PromotionProperty.Quantity"
                                        :placeholder="Resource.Placehoder.Quantity" @update="updateValue"
                                        @eventBlur="focusQuantity = false; validateQuantity();" :isRef="focusQuantity" />

                                    <div class="error-text">{{ errors.quantity }}</div>
                                </div>

                            </div>
                            <div class="flex mr-10 block" v-if="formMode == Enum.Mode.Edit">
                                <label for="" class="label-form d-flex">
                                    {{ Resource.Label.NumUsed }}
                                </label>
                                <div class="flex text-field-form">
                                    <InputNumber :transmissionNumber="promotion.numUsed" :decimalPlaces="0"
                                        :isReadonly="true" />
                                </div>

                            </div>

                        </div>

                    </div>
                </div>

                <div class="modal-footer">
                    <div class="flex-row flex-end">

                        <BaseButton tabindex="11" @keydown.tab.prevent.exact="focusPromotionCode = true;"
                            @mousedown="closeForm" @keydown.enter="closeForm" class="sub-button btn">
                            {{ Resource.Button.Cancel }}
                        </BaseButton>

                        <el-tooltip effect="dark" :content="Const.KeyStrokes.CtrlShiftS" placement="top">
                            <BaseButton tabindex="10" @click="save(true)" @keydown.enter="save(true)" v-show="!checkForm()"
                                class="button-blue btn ml-10">
                                {{ Resource.Button.SaveAndNew }}
                            </BaseButton>
                        </el-tooltip>

                        <el-tooltip effect="dark" :content="Const.KeyStrokes.CtrlS" placement="top">
                            <BaseButton tabindex="9" @click="save(false)" @keydown.enter="save(false)" v-show="!checkForm()"
                                class="main-button btn ml-10">{{ Resource.Button.Save }}</BaseButton>
                        </el-tooltip>

                        <el-tooltip effect="dark" :content="Const.KeyStrokes.CtrlS" placement="top">
                            <BaseButton tabindex="9" @click="save(false)" @keydown.enter="save(false)" v-show="checkForm()"
                                style="padding: 0 16px;" class="main-button btn ml-10">{{ Resource.Button.SaveChange }}
                            </BaseButton>
                        </el-tooltip>

                    </div>
                </div>
            </div>
        </form>
    </div>

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
import BaseButton from "../../components/base/BaseButton.vue";
import BasePopup from "../../components/base/BasePopup.vue";
import InputString from '@/components/base/BaseInputString.vue';
import InputNumber from '@/components/base/BaseInputNumber.vue';
import BaseRadio from '@/components/base/BaseRadio.vue';
import CommonFn from '@/utils/commonFuncion';
import axios from 'axios';
import BaseToastMessage from '@/components/base/BaseToastMessage.vue';
import moment from 'moment';
import 'moment/locale/vi'
export default {
    components: {
        BaseButton, BasePopup, InputString, InputNumber, BaseRadio, BaseToastMessage
    },
    props: ["closeFormDetail", "promotionIdUpdate", "formMode"],
    data() {
        return {
            Enum: Enum,
            Resource: Resource,
            Const: Const,
            isTitle: "", // tiêu đề của form

            titlePopup: Resource.Title.Management, // tiêu đề của popup

            contentPopup: "", // nội dung cảnh báo

            isShowPopup: false, // cờ điền khiển đóng mở cảnh báo lỗi

            errors: { // danh sách lỗi validate của các trường thông tin
                promotionCode: '',
                discount: '',
                dayStart: '',
                dayExpired: '',
                proviso: '',
                quantity: '',
            },

            promotionInit: {
                promotionCode: '',
                discount: '',
                dayStart: '',
                dayExpired: '',
                proviso: '',
                quantity: '',
                status: true
            },

            promotion: {}, // thông tin voucher trong form thêm mới

            focusPromotionCode: false,
            focusDiscount: false,
            focusDayStart: false,
            focusDayExpired: false,
            focusProviso: false,
            focusQuantity: false,

            toastMessage: { // cảnh báo
                message: "", // nội dung cảnh báo
                type: "", // loại cảnh báo
                isShowed: false, // cờ điều khiển bật tắt cảnh báo
            },
        };
    },

    methods: {

        updateValue(value, nameProperty) {
            this.promotion[nameProperty] = value;
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
         * Lưu thông tin voucher
         * @param {*} control: kiểu lưu voucher, nếu false thì lưu và đóng form, còn true thì lưu và reset dữ liệu, không đóng form
         *  Author: HAQUAN(29/08/2023)  
         */
        save(control) {
            try {
                let valid = this.validate();
                if (valid) {
                    // Nếu như là chỉnh sửa thông tin voucher
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
         * Gửi request chỉnh sửa thông tin voucher đến server
         *  Author: HAQUAN(29/08/2023) 
         */
        async sendRequestUpdate() {
            try {
                this.promotion.modifiedBy = this.$store.getters.user.accountID;
                await axios.put("Promotion/" + this.promotionIdUpdate, this.promotion)
                    .then((response) => {
                        console.log(response);

                        // Hiển thị toast thông báo thành công
                        this.$emit("showToast", Resource.Message.UpdatePromotionSucces, Const.TypeToast.Success);
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
         * Gửi request thêm mới voucher
         * @param {*} control: kiểu lưu voucher, nếu false thì lưu và đóng form, còn true thì lưu và reset dữ liệu, không đóng form
         *  Author: HAQUAN(29/08/2023) 
         */
        async sendRequestInsert(control) {
            try {
                await axios.post('Promotion/', this.promotion).
                    then((response) => {
                        console.log(response);

                        // Hiển thị toast thông báo thành công
                        this.$emit("showToast", Resource.Message.AddPromotionSucces, Const.TypeToast.Success);
                        this.$emit("refreshData");
                        if (!control) {
                            this.closeFormDetail();
                        }
                        else {
                            this.promotion = { ...this.promotionInit };
                            this.promotion.createdBy = this.$store.getters.user.accountID;
                            this.focusPromotionCode = true;
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

        validatePromotionCode() {
            if (!this.promotion.promotionCode) {
                this.errors.promotionCode = Resource.Error.PromotionCodeNotEmpty;
                return false;
            }
            this.errors.promotionCode = '';
            return true;
        },

        validateDiscount() {
            if (!this.promotion.discount) {
                this.errors.discount = Resource.Error.DiscountNotEmpty;
                return false;
            }
            this.errors.discount = '';
            return true;
        },

        validateDayStart() {
            if (!this.promotion.dayStart) {
                this.errors.dayStart = Resource.Error.DayStartNotEmpty;
                return false;
            }
            else if (this.promotion.dayExpired) {
                if (moment(this.promotion.dayStart) > moment(this.promotion.dayExpired)) {
                    this.errors.dayStart = Resource.Error.DayStartFail;
                    return false;
                }
                this.errors.dayExpired = '';
            }
            this.errors.dayStart = '';
            return true;
        },

        validateDayExpired() {
            if (!this.promotion.dayExpired) {
                this.errors.dayExpired = Resource.Error.DayExpiredNotEmpty;
                return false;
            }
            else if (this.promotion.dayStart) {
                if (moment(this.promotion.dayStart) > moment(this.promotion.dayExpired)) {
                    this.errors.dayExpired = Resource.Error.DayExpiredFail;
                    return false;
                }
                this.errors.dayStart = '';
            }
            this.errors.dayExpired = '';
            return true;
        },

        validateProviso() {
            if (!this.promotion.proviso) {
                this.errors.proviso = Resource.Error.ProvisoNotEmpty;
                return false;
            }
            this.errors.proviso = '';
            return true;
        },

        validateQuantity() {
            if (!this.promotion.quantity) {
                this.errors.quantity = Resource.Error.Quantity;
                return false;
            }
            this.errors.quantity = '';
            return true;
        },

        validate() {
            try {
                let valid = true;
                if (!this.validateQuantity()) {
                    valid = false;
                    this.focusQuantity = true;
                }
                if (!this.validateProviso()) {
                    valid = false;
                    this.focusProviso = true;
                }
                if (!this.validateDayExpired()) {
                    valid = false;
                    this.focusDayExpired = true;
                }
                if (!this.validateDayStart()) {
                    valid = false;
                    this.focusDayStart = true;
                }
                if (!this.validateDiscount()) {
                    valid = false;
                    this.focusDiscount = true;
                }
                if (!this.validatePromotionCode()) {
                    valid = false;
                    this.focusPromotionCode = true;
                }
                return valid;

            } catch (error) {
                console.log(error);
                return false;
            }
        },

        /**
         * Lấy thông tin voucher theo ID
         * @param {*} promotionID : ID của voucher muốn lấy
         * Author: HAQUAN(29/08/2023) 
         */
        async getPromotionByID(promotionID) {
            try {
                this.isLoading = true;
                let url = `Promotion/${promotionID}`;
                await axios.get(url)
                    .then((response) => {
                        this.promotion = response.data;
                        console.log(response.data);
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
        this.promotion = { ...this.promotionInit };
        this.promotion.createdBy = this.$store.getters.user.accountID;
        if (this.formMode == Enum.Mode.Edit) {
            this.isTitle = Resource.Title.EditPromotion;
            this.getPromotionByID(this.promotionIdUpdate);
        }
    },
    mounted() {
        if (this.formMode == Enum.Mode.Add) {
            this.isTitle = Resource.Title.AddPromotion;
        }
        // focus vào ô input mã voucher
        this.focusPromotionCode = true;

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
  