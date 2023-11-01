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

                        <div class="row-form flex-row">
                            <div class="flex mr-10 block">
                                <label for="" class="label-form d-flex">
                                    {{ Resource.Label.ReceiverName }}
                                    <span class="required">*</span>
                                </label>

                                <div class="flex text-field-form">
                                    <InputString :transmissionString="orderModel.order.fullName"
                                        :hasError="errors.fullName != ''"
                                        :placeholder="Resource.Placehoder.InfoGuest.FullName" maxlength="100" tabindex="1"
                                        :nameProperty="Resource.InfoGuestProperty.FullName" @updateValue="updateValue"
                                        @eventBlur="focusFullName = false; validateFullName();" :isRef="focusFullName" />

                                    <div class="error-text">{{ errors.fullName }}</div>
                                </div>
                            </div>
                            <div class="flex mr-10 block">
                                <label for="" class="label-form d-flex">
                                    {{ Resource.Label.Phone }}
                                    <span class="required">*</span>
                                </label>

                                <div class="flex text-field-form">
                                    <InputString :transmissionString="orderModel.order.phone" :hasError="errors.phone != ''"
                                        :placeholder="Resource.Placehoder.InfoGuest.Phone" maxlength="20" tabindex="2"
                                        :nameProperty="Resource.InfoGuestProperty.Phone" @updateValue="updateValue"
                                        @eventBlur="focusPhone = false; validatePhone();" :isRef="focusPhone" />

                                    <div class="error-text">{{ errors.phone }}</div>
                                </div>
                            </div>

                        </div>

                        <div class="row-form d-flex">
                            <div class="flex mr-10 block">
                                <label for="" class="label-form d-flex">
                                    {{ Resource.Label.Email }}
                                    <span class="required">*</span>
                                </label>

                                <div class="flex text-field-form">
                                    <InputString :transmissionString="orderModel.order.email" :hasError="errors.email != ''"
                                        :placeholder="Resource.Placehoder.InfoGuest.Email" maxlength="150" tabindex="3"
                                        :nameProperty="Resource.InfoGuestProperty.Email" @updateValue="updateValue"
                                        @eventBlur="focusEmail = false; validateEmail();" :isRef="focusEmail" />

                                    <div class="error-text">{{ errors.email }}</div>
                                </div>
                            </div>
                            <div class="flex mr-10 block">
                                <label for="" class="label-form d-flex">
                                    {{ Resource.Label.DeliveryMethod }}
                                    <span class="required">*</span>
                                </label>

                                <div class="flex-row flex radio-form">
                                    <div class="flex">
                                        <BaseRadio tabindex="4"
                                            @check="orderModel.order.deliveryMethod = Const.DeliveryMethod.AtStore.Value"
                                            :checkedProp="orderModel.order.deliveryMethod == Const.DeliveryMethod.AtStore.Value"
                                            :label="Const.DeliveryMethod.AtStore.Label">
                                            <template #radiomark>
                                                <div class="radio-checkmark"></div>
                                            </template>
                                        </BaseRadio>
                                    </div>
                                    <div class="flex">
                                        <BaseRadio tabindex="5"
                                            @check="orderModel.order.deliveryMethod = Const.DeliveryMethod.AtHome.Value"
                                            :checkedProp="orderModel.order.deliveryMethod == Const.DeliveryMethod.AtHome.Value"
                                            :label="Const.DeliveryMethod.AtHome.Label">
                                            <template #radiomark>
                                                <div class="radio-checkmark"></div>
                                            </template>
                                        </BaseRadio>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row-form flex">
                            <label for="" class="label-form d-flex">
                                {{ Resource.Label.Address }}
                                <span class="required">*</span>
                            </label>
                            <div class="flex-column text-field-form"
                                v-if="orderModel.order.deliveryMethod == Const.DeliveryMethod.AtStore.Value">
                                <BaseRadio v-for="(address, index) in Const.ListAddress" :key="index" class="mt-10"
                                    :tabindex="address.Index"
                                    @check="orderModel.order.address = address.Label; errors.address = '';"
                                    :checkedProp="orderModel.order.address == address.Label" :label="address.Label">
                                    <template #radiomark>
                                        <div class="radio-checkmark"></div>
                                    </template>
                                </BaseRadio>
                                <div class="error-text">{{ errors.address }}</div>
                            </div>
                            <div class="flex text-field-form" v-else>
                                <InputString :transmissionString="orderModel.order.address" :hasError="errors.address != ''"
                                    :placeholder="Resource.Placehoder.InfoGuest.Address" maxlength="255" tabindex="6"
                                    :nameProperty="Resource.InfoGuestProperty.Address" @updateValue="updateValue"
                                    @eventBlur="focusAddress = false; validateAddress();" :isRef="focusAddress" />
                                <div class="error-text">{{ errors.address }}</div>
                            </div>
                        </div>

                        <div class="row-form d-flex">
                            <div class="flex mr-10 block">
                                <label for="" class="label-form d-flex">
                                    {{ Resource.Label.PaymentMethod }}
                                    <span class="required">*</span>
                                </label>

                                <div class="flex-row text-field-form">
                                    <div class="">
                                        <BaseRadio tabindex="12"
                                            @check="orderModel.order.paymentMethod = Const.PaymentMethod.ShipCod.Value"
                                            :checkedProp="orderModel.order.paymentMethod == Const.PaymentMethod.ShipCod.Value"
                                            :label="Const.PaymentMethod.ShipCod.Label">
                                            <template #radiomark>
                                                <div class="radio-checkmark"></div>
                                            </template>
                                        </BaseRadio>
                                    </div>
                                    <div class="">
                                        <BaseRadio tabindex="13"
                                            @check="orderModel.order.paymentMethod = Const.PaymentMethod.PaymentOnline.Value"
                                            :checkedProp="orderModel.order.paymentMethod == Const.PaymentMethod.PaymentOnline.Value"
                                            :label="Const.PaymentMethod.PaymentOnline.Label">
                                            <template #radiomark>
                                                <div class="radio-checkmark"></div>
                                            </template>
                                        </BaseRadio>
                                    </div>
                                </div>
                            </div>
                            <div class="flex mr-10 block">
                                <label for="" class="label-form d-flex">
                                    {{ Resource.Label.PaymentStatus }}
                                    <span class="required">*</span>
                                </label>

                                <div class="flex-row text-field-form">
                                    <div class="flex">
                                        <BaseRadio tabindex="14"
                                            @check="orderModel.order.paymentStatus = Const.PaymentStatus.Unpaid.Value"
                                            :checkedProp="orderModel.order.paymentStatus == Const.PaymentStatus.Unpaid.Value"
                                            :label="Const.PaymentStatus.Unpaid.Label">
                                            <template #radiomark>
                                                <div class="radio-checkmark"></div>
                                            </template>
                                        </BaseRadio>
                                    </div>
                                    <div class="flex">
                                        <BaseRadio tabindex="15"
                                            @check="orderModel.order.paymentStatus = Const.PaymentStatus.Paid.Value"
                                            :checkedProp="orderModel.order.paymentStatus == Const.PaymentStatus.Paid.Value"
                                            :label="Const.PaymentStatus.Paid.Label">
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
                                    {{ Resource.Label.StatusOrder }}
                                    <span class="required">*</span>
                                </label>

                                <div class="flex combobox-form">
                                    <BaseCombobox tabindex="16" :hasIcon="true" :isReadonly="true"
                                        :items="Const.StatusOrder" :initItem="configStatusOrder(orderModel.order.status)"
                                        fieldName="Label" @getValue="setStatusOrder" />
                                </div>
                            </div>

                            <div class="flex mr-10 block">
                                <label for="" class="label-form d-flex">
                                    {{ Resource.Label.PromotionCode }}
                                </label>

                                <div class="text-field-form">
                                    <div class="flex-row">
                                        <div style="width: 100%;">
                                            <InputString :transmissionString="orderModel.order.promotionCode" maxlength="50"
                                                :hasError="errors.promotion != ''"
                                                :placeholder="Resource.Placehoder.InfoGuest.PromotionCode" tabindex="18"
                                                :nameProperty="Resource.InfoGuestProperty.PromotionCode"
                                                @updateValue="updateValue" />
                                        </div>
                                        <BaseButton @click="applyVoucher" class="main-button-red btn ml-10"
                                            :class="{ 'disabled': orderModel.order.promotionCode == '' }">
                                            {{ Resource.Button.Apply }}
                                        </BaseButton>
                                    </div>

                                    <div class="error-text">{{ errors.promotion }}</div>
                                </div>
                            </div>
                        </div>
                        <div class="row-form">

                            <label for="" class="label-form d-flex">
                                {{ Resource.Label.Note }}
                            </label>

                            <div class="flex text-aria-form">

                                <div class="flex-row border">
                                    <textarea :placeholder="Resource.Placehoder.InfoGuest.Note" class="aria-form flex"
                                        rows="4" v-model.trim="orderModel.order.note" tabindex="17"></textarea>
                                </div>
                            </div>
                        </div>
                        <div class="row-form">
                            <label for="" class="label-form flex-row space-between ">
                                {{ Resource.Label.Product }}
                                <BaseButton @click="addProduct" class="button-blue btn ml-10">{{ Resource.Button.AddProduct
                                }}</BaseButton>
                            </label>
                            <div class="list-product" v-show="orderModel.listOrderDetail.length != 0">
                                <div class="item" v-for="item in orderModel.listOrderDetail" :key="item.productID">
                                    <CartItem :initItem="item" @updateValue="updateQuantity" @deleteItem="deleteItem" />
                                </div>
                            </div>

                        </div>


                        <div class="row-form d-flex align-items space-between">
                            <b> {{ Resource.Label.TemporaryTotalMoney }}</b>
                            <span>
                                {{ formatMoney(totalMoney) }}
                            </span>
                        </div>
                        <div class="row-form d-flex align-items space-between">
                            <b> {{ Resource.Label.DiscountMoney }}</b>
                            <span>
                                {{ formatMoney(decreaseMoney) }}
                            </span>
                        </div>
                        <div class="row-form d-flex align-items space-between">
                            <b> {{ Resource.Label.FinalMMoney }}</b>
                            <span>
                                {{ formatMoney(orderModel.order.total) }}
                            </span>
                        </div>
                    </div>
                </div>

                <div class="modal-footer">
                    <div class="flex-row flex-end">

                        <BaseButton tabindex="21" @keydown.tab.prevent.exact="focusFullName = true;" @mousedown="closeForm"
                            @keydown.enter="closeForm" class="sub-button btn">
                            {{ Resource.Button.Cancel }}
                        </BaseButton>

                        <el-tooltip effect="dark" :content="Const.KeyStrokes.CtrlShiftS" placement="top">
                            <BaseButton tabindex="20" @click="save(true)" @keydown.enter="save(true)" v-show="!checkForm()"
                                class="button-blue btn ml-10">{{ Resource.Button.SaveAndNew }}
                            </BaseButton>
                        </el-tooltip>

                        <el-tooltip effect="dark" :content="Const.KeyStrokes.CtrlS" placement="top">
                            <BaseButton tabindex="19" @click="save(false)" @keydown.enter="save(false)"
                                v-show="!checkForm()" class="main-button btn ml-10">{{ Resource.Button.Save }}</BaseButton>
                        </el-tooltip>

                        <el-tooltip effect="dark" :content="Const.KeyStrokes.CtrlS" placement="top">
                            <BaseButton tabindex="19" @click="save(false)" @keydown.enter="save(false)" v-show="checkForm()"
                                style="padding: 0 16px;" class="main-button btn ml-10">{{ Resource.Button.SaveChange }}
                            </BaseButton>
                        </el-tooltip>

                    </div>
                </div>
            </div>
        </form>
    </div>

    <BasePopup @close="closePopup" class="popup-delete" :title="this.titlePopup" :content="contentPopup" v-if="isShowPopup">

        <template #buttons>
            <BaseButton @click="closePopup" class="main-button-red btn ml-10">{{ Resource.Button.Close }}</BaseButton>
        </template>

    </BasePopup>
</template>
<script>
import Const from '@/utils/const'
import Enum from '@/utils/enum'
import Resource from '@/utils/resource'
import BaseCombobox from "../components/base/BaseCombobox.vue";
import BaseButton from "../components/base/BaseButton.vue";
import BasePopup from "../components/base/BasePopup.vue";
import InputString from '@/components/base/BaseInputString.vue';
import BaseRadio from '@/components/base/BaseRadio.vue';
import CartItem from '@/components/base/CartItem.vue';
import axios from 'axios';
import moment from 'moment';
export default {
    components: {
        BaseCombobox, BaseButton, BasePopup, InputString, BaseRadio, CartItem
    },
    props: ["closeFormDetail", "orderIdUpdate", "formMode"],
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
                fullName: '',
                email: '',
                phone: '',
                address: '',
                promotion: ''
            },

            orderModelInit: {
                order: {
                    fullName: '',
                    email: '',
                    phone: '',
                    address: '',
                    deliveryMethod: Const.DeliveryMethod.AtStore.Value,
                    paymentMethod: Const.PaymentMethod.ShipCod.Value,
                    status: 1,
                    total: 0,
                    paymentStatus: false,
                    note: '',
                    promotionCode: ''
                },
                listOrderDetail: [],
            },

            orderModel: {}, // thông tin đơn hàng trong form thêm mới

            focusFullName: false,
            focusPhone: false,
            focusEmail: false,
            focusAddress: false,
            promotion: null,
            decreaseMoney: 0
        };
    },

    computed: {
        totalMoney: function () {
            let total = 0;
            this.orderModel.listOrderDetail.forEach(element => {
                let money = element.price * (100 - element.discount) / 1000000;
                money = money - money % 1;
                money = money * 10000;
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

        formatMoney(money) {
            return money.toLocaleString('vi-VN') + ' đ';
        },

        updateValue(value, nameProperty) {
            this.orderModel.order[nameProperty] = value;
        },

        updateQuantity(productID, quantity) {
            this.orderModel.listOrderDetail.find(
                (item) => {
                    if (item.productID == productID) {
                        item.amount = quantity;
                    }
                }
            );
            this.orderModel.order.total = this.totalMoney - this.decreaseMoney;
        },

        deleteItem(productID) {
            this.orderModel.listOrderDetail = this.orderModel.listOrderDetail.filter(
                (item) => {
                    if (item.productID != productID) {
                        return item;
                    }
                }
            );
        },

        setStatusOrder(value) {
            Const.StatusOrder.find((item) => {
                if (item.Label == value) {
                    this.orderModel.order.status = item.Value;
                }
            });
        },

        configStatusOrder(value) {
            let result = value;
            Const.StatusOrder.find((item) => {
                if (item.Value === value) {
                    result = item.Label;
                }
            });
            return result;
        },

        async applyVoucher() {
            if (this.orderModel.order.promotionCode) {
                await this.getVoucherByCode(this.orderModel.order.promotionCode);
                if (this.promotion != null) {
                    if (moment(this.promotion.dayStart) > moment() || moment(this.promotion.dayExpired) < moment()) {
                        this.errors.promotion = Resource.Error.PromotionOverTime;
                    }

                    if (this.promotion.quantity == this.promotion.numUsed) {
                        this.errors.promotion = Resource.Error.PromotionOver;
                    }

                    if (this.promotion.proviso > this.totalMoney) {
                        this.errors.promotion = Resource.Error.Unconditional;
                    }

                    if (this.errors.promotion == '') {
                        this.decreaseMoney = this.promotion.discount;
                    }
                    else {
                        this.decreaseMoney = 0;
                    }
                }

            }
        },

        /**
        * Lấy thông tin voucher
       * @param {*} code : mã code của voucher
       * Author: HAQUAN(29/08/2023) 
       */
        async getVoucherByCode(code) {
            try {
                let url = `Promotion/byCode?code=${code}`;
                await axios.get(url)
                    .then((response) => {
                        this.promotion = response.data;
                        console.log(this.promotion);
                        this.errors.promotion = '';
                    })

                    .catch((error) => {
                        console.log(error);
                        this.promotion = null;
                        this.decreaseMoney = 0;
                        this.errors.promotion = Resource.Error.PromotionNotFound;
                    })
            } catch (error) {
                console.log(error);
            }
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
         * Lưu thông tin đơn hàng
         * @param {*} control: kiểu lưu đơn hàng, nếu false thì lưu và đóng form, còn true thì lưu và reset dữ liệu, không đóng form
         *  Author: HAQUAN(29/08/2023)  
         */
        save(control) {
            try {
                let valid = this.validate();
                if (valid) {
                    // Nếu như là chỉnh sửa thông tin đơn hàng
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
         * Gửi request chỉnh sửa thông tin đơn hàng đến server
         *  Author: HAQUAN(29/08/2023) 
         */
        async sendRequestUpdate() {
            try {

                await axios.put("OrderProduct/" + this.orderModel.order.orderID, this.orderModel)
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
         * Gửi request thêm mới đơn hàng
         * @param {*} control: kiểu lưu đơn hàng, nếu false thì lưu và đóng form, còn true thì lưu và reset dữ liệu, không đóng form
         *  Author: HAQUAN(29/08/2023) 
         */
        async sendRequestInsert(control) {
            try {
                await axios.post('OrderProduct/', this.orderModel).
                then((response) => {
                    console.log(response);

                    // Hiển thị toast thông báo thành công
                    this.$emit("showToast", Resource.Message.AddAccountSucces, Const.TypeToast.Success);
                    this.$emit("refreshData");
                    if (!control) {
                        this.closeFormDetail();
                    }
                    else {
                        this.orderModel = { ...this.orderModelInit };
                        this.orderModel.order.createdBy = this.$store.getters.user.accountID;
                        this.orderModel.order.orderBy = this.$store.getters.user.username;
                        this.focusFullName = true;
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
        handleErrorResponse(error) {
            try {
                this.contentPopup = error.response.data.userMsg;
                this.titlePopup = Resource.Title.Management;
                this.isShowPopup = true;

            } catch (error) {
                console.log(error);
            }
        },

        validateFullName() {
            if (!this.orderModel.order.fullName) {
                this.errors.fullName = Resource.Error.FullNameNotEmpty;
                return false;
            }
            this.errors.fullName = '';
            return true;
        },

        /**
         * Validate email
         * Author: HAQUAN(29/08/2023) 
        */
        validateEmail() {
            try {
                var validEmail = '^[\\w-.]+@([\\w-]+.)+[\\w-]{2,4}$';
                if (!this.orderModel.order.email) {
                    this.errors.email = Resource.Error.Email;
                    return false;
                }
                if (!new RegExp(validEmail).test(this.orderModel.order.email)) {
                    this.errors.email = "Email không hợp lệ!";
                    return false;
                }
                this.errors.email = '';
                return true;
            } catch (error) {
                console.log(error);
            }
        },

        /**
        * Validate phone
         * Author: HAQUAN(29/08/2023) 
        */
        validatePhone() {
            try {
                // var validPhone = '/(84|0[3|5|7|8|9])+([0-9]{8})\b/g';
                if (!this.orderModel.order.phone) {
                    this.errors.phone = Resource.Error.PhoneNotEmpty;
                    return false;
                }
                // if (!new RegExp(validPhone).test(this.orderModel.order.phone)) {
                //     this.errors.phone = Resource.Error.Phone;
                //     return false;
                // }
                this.errors.phone = '';
                return true;
            } catch (error) {
                console.log(error);
                this.errors.phone = Resource.Error.PhoneFormat;
                return false;
            }
        },

        validateAddress() {
            if (!this.orderModel.order.address) {
                this.errors.address = Resource.Error.AddressNotEmpty;
                return false;
            }
            this.errors.address = '';
            return true;
        },

        validate() {
            try {
                let valid = true;
                if (!this.validateAddress()) {
                    valid = false;
                    this.focusAddress = true;
                }
                if (!this.validateEmail()) {
                    valid = false;
                    this.focusEmail = true;
                }
                if (!this.validatePhone()) {
                    valid = false;
                    this.focusPhone = true;
                }
                if (!this.validateFullName()) {
                    valid = false;
                    this.focusFullName = true;
                }
                return valid;

            } catch (error) {
                console.log(error);
                return false;
            }
        },

        /**
         * Lấy thông tin đơn hàng theo ID
         * @param {*} orderID : ID của đơn hàng muốn lấy
         * Author: HAQUAN(29/08/2023) 
         */
        async getOrderByID(orderID) {
            try {
                this.isLoading = true;
                let url = `OrderProduct/${orderID}`;
                await axios.get(url)
                    .then((response) => {
                        this.orderModel.order = response.data.order;
                        this.orderModel.listOrderDetail = response.data.listOrderDetail;
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

    },
    created() {
        this.orderModel = { ...this.orderModelInit };
        this.orderModel.order.createdBy = this.$store.getters.user.accountID;
        this.orderModel.order.orderBy = this.$store.getters.user.username;
        if (this.formMode == Enum.Mode.Edit) {
            this.isTitle = Resource.Title.EditOrder;
            this.getOrderByID(this.orderIdUpdate);
        }
    },
    mounted() {
        if (this.formMode == Enum.Mode.Add) {
            this.isTitle = Resource.Title.AddOrder;
        }
        // focus vào ô input tên đơn hàng
        this.focusFullName = true;

    },

};
</script>
<style scoped>
@import url(../css/base/radio.css);

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
.list-product .item:not(:last-child){
    padding-bottom: 12px;
    border-bottom: 1px solid #c6c6c6;
}
.list-product .item:not(:first-child){
    padding-top: 12px;
}
</style>
  