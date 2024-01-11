<template>
    <div class="container-cart" v-if="cart.length > 0 || step == Enum.Step.Done">
        <div class="title d-flex align-items">
            <div class="go-back d-flex align-items">
                <div class="icon-arrow">
                </div>
                <span>{{ Resource.Text.GoBack }}</span>

            </div>
            <h2>{{ Resource.Text.Cart }}</h2>
        </div>
        <div class="order-cart">
            <div class="step d-flex align-items space-between" v-show="step > Enum.Step.SelectProduct">
                <div class="process-item" :class="{ 'active': item.Value <= step }" v-for="(item, index) in Const.ListStep"
                    :key="index">
                    <div class="icon d-flex align-items justify-content">
                        <div :class="item.Icon"></div>
                    </div>
                    <p>{{ item.Label }}</p>
                </div>
            </div>
            <div class="box-cart" v-if="step == Enum.Step.SelectProduct">
                <CartItemVue v-for="item in cart" :key="item.productID" :initItem="item" @updateValue="updateQuantity"
                    @deleteItem="confirmDelete" />
            </div>
            <div class="box-cart info-order" v-else-if="step == Enum.Step.InfoOrder">
                <div class="title">
                    {{ Resource.Label.InfoGuest }}
                </div>
                <div class="flex text-field-form">
                    <InputString :transmissionString="orderModel.order.fullName" :hasError="errors.fullName != ''"
                        :placeholder="Resource.Placehoder.InfoGuest.FullName" maxlength="100" tabindex="1"
                        :nameProperty="Resource.InfoGuestProperty.FullName" @updateValue="updateValue"
                        @eventBlur="focusFullName = false; validateFullName();" :isRef="focusFullName" />

                    <div class="error-text">{{ errors.fullName }}</div>
                </div>
                <div class="flex text-field-form">
                    <InputString :transmissionString="orderModel.order.phone" :hasError="errors.phone != ''"
                        :placeholder="Resource.Placehoder.InfoGuest.Phone" maxlength="20" tabindex="2"
                        :nameProperty="Resource.InfoGuestProperty.Phone" @updateValue="updateValue"
                        @eventBlur="focusPhone = false; validatePhone();" :isRef="focusPhone" />

                    <div class="error-text">{{ errors.phone }}</div>
                </div>
                <div class="flex text-field-form">
                    <InputString :transmissionString="orderModel.order.email" :hasError="errors.email != ''"
                        :placeholder="Resource.Placehoder.InfoGuest.Email" maxlength="150" tabindex="3"
                        :nameProperty="Resource.InfoGuestProperty.Email" @updateValue="updateValue"
                        @eventBlur="focusEmail = false; validateEmail();" :isRef="focusEmail" />

                    <div class="error-text">{{ errors.email }}</div>
                </div>
                <div class="title">
                    {{ Resource.Label.SelectDeliveryMethod }}
                </div>
                <div class="flex-row radio-form">
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
                        <BaseRadio tabindex="5" @check="orderModel.order.deliveryMethod = Const.DeliveryMethod.AtHome.Value"
                            :checkedProp="orderModel.order.deliveryMethod == Const.DeliveryMethod.AtHome.Value"
                            :label="Const.DeliveryMethod.AtHome.Label">
                            <template #radiomark>
                                <div class="radio-checkmark"></div>
                            </template>
                        </BaseRadio>
                    </div>
                </div>
                <div class="content-ship flex-column"
                    v-if="orderModel.order.deliveryMethod == Const.DeliveryMethod.AtStore.Value">
                    <div class="title">
                        {{ Resource.Label.SelectAddress }}
                    </div>
                    <BaseRadio v-for="(address, index) in Const.ListAddress" :key="index" :tabindex="address.Index"
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
                <div class="flex text-field-form">
                    <InputString :transmissionString="orderModel.order.note"
                        :placeholder="Resource.Placehoder.InfoGuest.Note" tabindex="12"
                        :nameProperty="Resource.InfoGuestProperty.Note" @updateValue="updateValue" />
                </div>
                <div class="title">
                    {{ Resource.Label.SelectPaymentMethod }}
                </div>
                <div class="flex-row radio-form">
                    <div class="flex">
                        <BaseRadio tabindex="13" @check="orderModel.order.paymentMethod = Const.PaymentMethod.ShipCod.Value"
                            :checkedProp="orderModel.order.paymentMethod == Const.PaymentMethod.ShipCod.Value"
                            :label="Const.PaymentMethod.ShipCod.Label">
                            <template #radiomark>
                                <div class="radio-checkmark"></div>
                            </template>
                        </BaseRadio>
                    </div>
                    <div class="flex">
                        <BaseRadio tabindex="14"
                            @check="orderModel.order.paymentMethod = Const.PaymentMethod.PaymentOnline.Value"
                            :checkedProp="orderModel.order.paymentMethod == Const.PaymentMethod.PaymentOnline.Value"
                            :label="Const.PaymentMethod.PaymentOnline.Label">
                            <template #radiomark>
                                <div class="radio-checkmark"></div>
                            </template>
                        </BaseRadio>
                    </div>
                </div>
                <div class="text-field-form">
                    <div class="flex-row">
                        <div style="width: 100%;">
                            <InputString :transmissionString="orderModel.order.promotionCode" maxlength="50"
                                :hasError="errors.promotion != ''"
                                :placeholder="Resource.Placehoder.InfoGuest.PromotionCode" tabindex="15"
                                :nameProperty="Resource.InfoGuestProperty.PromotionCode" @updateValue="updateValue" />
                        </div>
                        <BaseButton @click="applyVoucher" class="main-button-red btn ml-10"
                            :class="{ 'disabled': orderModel.order.promotionCode == '' }">
                            {{ Resource.Button.Apply }}
                        </BaseButton>
                    </div>

                    <div class="error-text">{{ errors.promotion }}</div>
                </div>
            </div>
            <div class="box-cart payment" v-else-if="step == Enum.Step.Payment">
                <div class="image">
                    <img :src="`https://img.vietqr.io/image/MB-0050135109007-print.png?accountName=LE%20VAN%20THANG&amount=${orderModel.order.total / 10}&addInfo=HAQ%20CK`"
                        alt="">
                </div>
            </div>
            <div class="box-cart order-succes" v-else-if="step == Enum.Step.Done">
                <div class="note">
                    <p>{{ Resource.Text.DoneNote1 }}</p>
                    <i>{{ Resource.Text.DoneNote2 }}</i>
                </div>
                <div class="box-info">
                    <h4>{{ Resource.Title.OrderSucces }}</h4>
                    <div class="item-info">
                        <p>
                            {{ Resource.Label.OrderCode }}:
                            <span>
                                {{ orderModel.order.orderID.toUpperCase() }}
                            </span>
                        </p>
                    </div>
                    <div class="item-info">
                        <p>
                            {{ Resource.Label.Receiver }}:
                            <span>
                                {{ orderModel.order.fullName }}
                            </span>
                        </p>
                    </div>
                    <div class="item-info">
                        <p>
                            {{ Resource.Label.Phone }}:
                            <span>
                                {{ orderModel.order.phone }}
                            </span>
                        </p>
                    </div>
                    <div class="item-info">
                        <p>
                            {{ Resource.Label.Email }}:
                            <span>
                                {{ orderModel.order.email }}
                            </span>
                        </p>
                    </div>
                    <div class="item-info">
                        <p>
                            {{ Resource.Label.Address }}:
                            <span>
                                {{ orderModel.order.address }}
                            </span>
                        </p>
                    </div>
                    <div class="item-info">
                        <p>
                            {{ Resource.Label.PaymentMethod }}:
                            <span>
                                {{ orderModel.order.paymentMethod == Const.PaymentMethod.ShipCod.Value ?
                                    Const.PaymentMethod.ShipCod.Label : Const.PaymentMethod.PaymentOnline.Label }}
                            </span>
                        </p>
                    </div>
                    <div class="item-info">
                        <p>
                            {{ Resource.Label.FinalMMoney }}:
                            <span>
                                {{ formatMoney(orderModel.order.total) }}
                            </span>
                        </p>
                    </div>

                </div>

            </div>
            <div class="btn-cart">
                <div class="total-money d-flex align-items space-between " v-show="step < Enum.Step.Payment">
                    <b> {{ Resource.Label.TemporaryTotalMoney }}</b>
                    <span>
                        {{ formatMoney(totalMoney) }}
                    </span>
                </div>
                <div class="discount-money d-flex align-items space-between " v-show="step == Enum.Step.InfoOrder">
                    <b> {{ Resource.Label.DiscountMoney }}</b>
                    <span>
                        {{ formatMoney(decreaseMoney) }}
                    </span>
                </div>
                <div class="final-money d-flex align-items space-between "
                    v-show="step == Enum.Step.InfoOrder || step == Enum.Step.Payment">
                    <b> {{ Resource.Label.FinalMMoney }}</b>
                    <span>
                        {{ formatMoney(orderModel.order.total) }}
                    </span>
                </div>
                <div v-if="step < Enum.Step.Done">
                    <div class="btn-submit-order pointer" @click="nextStep">{{ Resource.Button.OrderProduct }}</div>
                    <div class="btn-select-more pointer" @click="goHome">{{ Resource.Button.SelectMore }}</div>
                </div>
                <div v-else>
                    <div class="btn-continue-shoping" @click="goHome">
                        {{ Resource.Button.ContinueShoping }}
                    </div>
                </div>

            </div>

        </div>
    </div>
    <div class="cart-empty" v-else>
        <span>{{ Resource.CartEmpty.Prefix }}</span>
        <span class="btn-click">{{ Resource.CartEmpty.Content }}</span>
        <span>{{ Resource.CartEmpty.Suffix }}</span>
    </div>
    <BasePopup @close="isShowPopup = false" class="popup-delete" :title="Resource.Title.VerifyDelete" :prefix="prefixPopup"
        :suffix="suffixPopup" :content="contentPopup" v-show="isShowPopup">
        <template #buttons>

            <BaseButton @click="isShowPopup = false" class="btn sub-button">
                {{ Resource.Button.No }}
            </BaseButton>

            <BaseButton @click="deleteItem(); isShowPopup = false;" class="main-button-red btn ml-10">
                {{ Resource.Button.DeleteProduct }}
            </BaseButton>

        </template>

    </BasePopup>
</template>
    
<script>
import axios from 'axios';
import Resource from '@/utils/resource';
import Enum from '@/utils/enum';
import Const from '@/utils/const';
import CartItemVue from '@/components/base/CartItem.vue';
import BasePopup from '@/components/base/BasePopup.vue';
import BaseButton from '@/components/base/BaseButton.vue';
import InputString from '@/components/base/BaseInputString.vue';
import BaseRadio from '@/components/base/BaseRadio.vue';
import CommonFn from '@/utils/commonFuncion';
import moment from 'moment';
export default {
    name: 'CartPage',
    components: {
        CartItemVue, BasePopup, BaseButton, InputString, BaseRadio
    },
    data() {
        return {
            Resource: Resource,
            Const: Const,
            Enum: Enum,
            cart: [],

            isShowPopup: false,
            contentPopup: null,
            prefixPopup: null, //
            suffixPopup: null,//
            productID: null,
            step: Enum.Step.SelectProduct,
            orderModel: {
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

            errors: { // danh sách lỗi validate của các trường thông tin
                fullName: '',
                email: '',
                phone: '',
                address: '',
                promotion: ''
            },

            focusFullName: false,
            focusPhone: false,
            focusEmail: false,
            focusAddress: false,
            promotion: null,
            decreaseMoney: 0,

            timeNow: null,

            formMb: {
                accountNo: "0050135109007",
                sessionId: "625a8f42-0c60-4a42-874b-d7ab635c64f8", // lấy ở api của NG
                deviceIdCommon: "1blaoo31-mbib-0000-0000-2024011012573275", // lấy ở api của NG
                fromDate: "",
                toDate: "",
                // refNo: "",
                // historyNumber: "",
                // historyType: "DATE_RANGE",
                // type: "ACCOUNT"
            },
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
        formatMoney(money) {
            return money.toLocaleString('vi-VN') + ' đ';
        },

        updateQuantity(productID, quantity) {
            this.cart.find(
                (item) => {
                    if (item.productID == productID) {
                        item.amount = quantity;
                    }
                }
            );
            localStorage.setItem('cart', JSON.stringify(this.cart));
        },

        confirmDelete(productID) {
            this.productID = productID;
            this.cart.find(
                (item) => {
                    if (item.productID == productID) {
                        this.contentPopup = item.productName;
                    }
                }
            );
            this.prefixPopup = Resource.Text.CanDeleteProduct;
            this.suffixPopup = Resource.Text.OutCart;
            this.isShowPopup = true;

        },

        deleteItem() {
            this.cart = this.cart.filter(
                (item) => {
                    if (item.productID != this.productID) {
                        return item;
                    }
                }
            );
            this.emitter.emit("updateAmountCart", this.cart.length);
            localStorage.setItem('cart', JSON.stringify(this.cart));
        },

        goHome() {
            this.$router.push({ name: 'HomePage' });
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
                var validPhone = /(84|0[3|5|7|8|9])+([0-9]{8})\b/;
                if (!this.orderModel.order.phone) {
                    this.errors.phone = Resource.Error.PhoneNotEmpty;
                    return false;
                }
                if (!new RegExp(validPhone).test(this.orderModel.order.phone)) {
                    this.errors.phone = Resource.Error.Phone;
                    return false;
                }
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
        updateValue(value, nameProperty) {
            this.orderModel.order[nameProperty] = value;
        },

        nextStep() {
            if (this.$store.getters.user == null) {
                this.$router.push({ name: "LoginForm" })
            }
            else {
                if (this.step == Enum.Step.SelectProduct) {
                    this.step = Enum.Step.InfoOrder;
                    this.orderModel.order.total = this.totalMoney;
                }
                else if (this.step == Enum.Step.InfoOrder && this.validate()) {
                    if (this.orderModel.order.paymentMethod == Const.PaymentMethod.ShipCod.Value) {
                        this.orderModel.listOrderDetail = this.cart;
                        this.sendRequestOrder();
                    }
                    else {
                        this.step = Enum.Step.Payment;
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
        * Đặt hàng
        * Author: HAQUAN(29/08/2023) 
        */
        async sendRequestOrder() {
            try {
                this.orderModel.order.createdBy = this.$store.getters.user.accountID;
                this.orderModel.order.orderBy = this.$store.getters.user.username;
                await axios.post('OrderProduct/', this.orderModel)
                    .then((response) => {
                        this.orderModel.order.orderID = response.data;
                        this.cart = [];
                        this.step = Enum.Step.Done;
                        this.emitter.emit("updateAmountCart", this.cart.length);
                        localStorage.setItem('cart', JSON.stringify(this.cart));
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

        configInfoRequest() {
            this.timeNow = moment();
            this.formMb.fromDate = this.timeNow.subtract(1, 'days').format('DD/MM/YYYY');
            this.formMb.toDate = this.timeNow.format('DD/MM/YYYY');
            this.formMb.refNo = "0337081000-" + this.timeNow.format('YYYYMMDDHHmmss00');
            console.log(this.formMb);
        },

        /**
        * Lấy lịch sử giao dịch
        * Author: HAQUAN(29/08/2023) 
        */
        async sendRequestGetHistoryPayment() {
            try {
                const axiosInstance = axios.create({
                    baseURL: 'https://online.mbbank.com.vn/api/',
                });
                this.configInfoRequest();
                let url = `retail-web-transactionservice/transaction/getTransactionAccountHistory`;
                // this.configInfoRequest();
                // let url = `https://online.mbbank.com.vn/api/retail-transactionms/transactionms/get-account-transaction-history`;
                await axiosInstance.post(url, this.formMb, {
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'Accept': 'application/json',
                        'Authorization': 'Basic RU1CUkVUQUlMV0VCOlNEMjM0ZGZnMzQlI0BGR0AzNHNmc2RmNDU4NDNm',
                    }
                })
                    .then((response) => {
                        console.log(response);
                    })

                    .catch((error) => {
                        console.log(error);
                        this.handleErrorResponse(error);
                    })
            } catch (error) {
                console.log(error);
            }
        },


    },
    created() {
        this.cart = JSON.parse(localStorage.getItem('cart'));
        this.sendRequestGetHistoryPayment();
    }
}
</script>
    
<style scoped>
@import url(../../css/base/radio.css);

.container-cart {
    width: 700px;
    margin: 0 auto;
}

.container-cart .title {
    padding: 10px 0;
    color: #ff9300;
    padding-top: 20px;
}

.container-cart .title h2 {
    text-align: center;
    width: calc(100% - 50px);
    margin-right: 40px;
    font-size: 22px;
}

.title .go-back {
    width: 70px;
    color: #ff9300;
    font-weight: 700;
    cursor: pointer;
}

.go-back .icon-arrow {
    width: 16px;
    height: 16px;
    background: url(../../assets/icon/arrow.png);
    transform: rotate(180deg);
    background-size: 16px 16px;
    /* margin-left: 8px; */
}

.box-cart {
    background: #fff;
    padding: 10px;
    border-radius: 15px;
    -webkit-box-shadow: 0 1px 2px 0 #3c40431a, 0 2px 6px 2px #3c404326;
    box-shadow: 0 1px 2px 0 #3c40431a, 0 2px 6px 2px #3c404326;
}

.btn-click:hover {
    color: #fe6402;
    cursor: pointer;
}

.cart-empty {
    font-size: 18px;
    text-align: center;
}

.btn-cart {
    margin-top: 10px;
    padding: 10px;
    background-color: #fff;
    border-radius: 10px;
}

.total-money,
.final-money,
.discount-money {
    padding: 5px 0;
}

.total-money span,
.final-money span {
    font-weight: 700;
    color: #d70018;
}

.discount-money span {
    font-weight: 700;
}

.btn-submit-order {
    height: 60px;
    width: 100%;
    display: flex;
    background: #FF9300;
    line-height: 60px;
    text-align: center;
    text-transform: uppercase;
    color: #fff;
    border-color: #FF9300;
    margin: 10px 0;
    border-radius: 5px;
    font-size: 18px;
    font-weight: 700;
    align-items: center;
    justify-content: center;
}

.btn-submit-order:hover {
    background: #ff9500be;
}

.btn-continue-shoping {
    height: 60px;
    width: 100%;
    display: flex;
    background: #FF9300;
    line-height: 60px;
    text-align: center;
    text-transform: uppercase;
    color: #fff;
    border-color: #FF9300;
    margin: 10px 0;
    border-radius: 5px;
    font-size: 18px;
    font-weight: 700;
    align-items: center;
    justify-content: center;
    cursor: pointer;
}

.btn-continue-shoping:hover {
    background: #c82333;
    border-color: #bd2130;
}

.btn-select-more {
    height: 60px;
    width: 100%;
    display: flex;
    color: #FF9300;
    line-height: 60px;
    text-align: center;
    text-transform: uppercase;
    background: #fff;
    border: 1px solid #FF9300;
    margin: 10px 0;
    border-radius: 5px;
    font-size: 18px;
    font-weight: 700;
    align-items: center;
    justify-content: center;
}

.btn-select-more:hover {
    background: #f2efefcf;
}

.step {
    padding: 1rem;
    background: #000;
    margin-top: 10px;
    border-radius: 15px 15px 0 0;
}

.process-item {
    width: 25%;
    position: relative;
}

.process-item .icon {
    border-radius: 50%;
    border: 1px solid #777;
    width: 35px;
    height: 35px;
    margin: 0 auto;
    position: relative;
}

.process-item.active .icon {
    border: 1px solid #FF9300;
}

.process-item p {
    text-align: center;
    font-size: 11px;
    margin-top: 5px;
    color: #fff;
}

.process-item.active p {
    color: #FF9300;
}

.process-item:not(:first-child):after {
    content: "";
    width: 40%;
    height: 1px;
    position: absolute;
    top: 30%;
    left: -18%;
    border-top: 1.5px dashed #777;
}

.process-item.active:not(:first-child):after {
    border-color: #FF9300;
}

.icon .icon-cart {
    width: 24px;
    height: 24px;
    background: url(../../assets/icon/icon-cart-order.png);
    background-size: 24px 24px;
}

.icon .icon-info-order {
    width: 24px;
    height: 24px;
    background: url(../../assets/icon/icon-info-order.png);
    background-size: 24px 24px;
}

.icon .icon-payment {
    width: 24px;
    height: 24px;
    background: url(../../assets/icon/icon-payment.png);
    background-size: 24px 24px;
}

.process-item.active .icon .icon-payment {
    width: 24px;
    height: 24px;
    background: url(../../assets/icon/icon-payment-active.png);
    background-size: 24px 24px;
}

.icon .icon-done {
    width: 24px;
    height: 24px;
    background: url(../../assets/icon/icon-box.png);
    background-size: 24px 24px;
}

.process-item.active .icon .icon-done {
    width: 24px;
    height: 24px;
    background: url(../../assets/icon/icon-box-active.png);

    background-size: 24px 24px;
}

.text-field-form {
    background: #fff;
    padding: 0;
    width: 100%;
    border-radius: 3.5px;
}

.info-order .title {
    font-weight: 700;
    padding: 10px 0;
    color: #000;
}

.info-order .text-field-form {
    margin-bottom: 1rem !important;
}

.info-order .text-field-form .border {
    border-radius: 10px;
}

.info-order .text-field-form .error-text {
    margin-left: 12px;
}

.border:not(.disabled):not(.error):focus-within,
.border:not(.disabled):not(.error):hover {
    border: 1px solid #1a73e8;
}

.border.error {
    border: 1px solid #ef5350;
}

::placeholder {
    /* color: #bbbbbb; */
    color: #d70018;
    opacity: 1;
}

.content-ship .title {
    font-weight: bold;
    font-style: italic;
}

.content-ship .radio {
    margin-bottom: 18px;
}

.radio-form {
    margin-bottom: 10px;
}

.payment .image img {
    width: 100%;
}

.order-succes .note {
    padding: 10px 0;
}

.order-succes .note p,
.order-succes .note i {
    margin: 0 5px 10px;
}

.order-succes .box-info {
    color: #155724;
    background: #d4edda;
    padding: 10px;
    border-radius: 15px;
}

.order-succes .box-info h4 {
    font-size: 1.3rem;
    font-weight: 700;
    text-align: center;
    text-transform: uppercase;
}

.box-info .item-info {
    margin: 15px 0;
}

.box-info .item-info p {
    font-weight: 400;
    margin-left: 10px;
    text-transform: capitalize;
    font-size: 18px;
}

.box-info .item-info span {
    font-weight: 600;
}
</style>
    