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
                                    {{ Resource.Label.GiftCode }}
                                    <span class="required">*</span>
                                </label>

                                <div class="flex text-field-form">
                                    <InputString :transmissionString="giftModel.giftCode" :hasError="errors.giftCode != ''"
                                        :placeholder="Resource.Placehoder.GiftCode" maxlength="50" tabindex="1"
                                        :nameProperty="Resource.GiftProperty.GiftCode" @updateValue="updateValue"
                                        @eventBlur="focusCode = false; validateCode();" :isRef="focusCode" />

                                    <div class="error-text">{{ errors.giftCode }}</div>
                                </div>
                            </div>
                        </div>

                        <div class="row-form d-flex">
                            <div class="flex mr-10 block">
                                <label for="" class="label-form d-flex">
                                    {{ Resource.Label.DescriptionGift }}
                                    <span class="required">*</span>
                                </label>

                                <div class="flex text-aria-form">

                                    <div :class="['flex-row border',{ 'error': errors.description != '' }]">
                                        <textarea :placeholder="Resource.Placehoder.GiftInfo" class="aria-form flex" rows="4" ref="infoGift"
                                             v-model.trim="giftModel.description" tabindex="2" @blur="validateDescription" >
                                        </textarea>
                                    </div>
                                    <div class="error-text">{{ errors.description }}</div>
                                </div>
                            </div>
                        </div>

                        <div class="row-form d-flex">

                            <div class="flex mr-10 block">
                                <label for="" class="label-form d-flex">
                                    {{ Resource.Label.Status }}
                                </label>

                                <div class="flex-row text-field-form">
                                    <div class="flex">
                                        <BaseRadio tabindex="3"
                                            @check="giftModel.status = Const.UseStatus.Using.Value"
                                            :checkedProp="giftModel.status == Const.UseStatus.Using.Value"
                                            :label="Const.UseStatus.Using.Label">
                                            <template #radiomark>
                                                <div class="radio-checkmark"></div>
                                            </template>
                                        </BaseRadio>
                                    </div>
                                    <div class="flex">
                                        <BaseRadio tabindex="4"
                                            @check="giftModel.status = Const.UseStatus.StopUsing.Value"
                                            :checkedProp="giftModel.status == Const.UseStatus.StopUsing.Value"
                                            :label="Const.UseStatus.StopUsing.Label">
                                            <template #radiomark>
                                                <div class="radio-checkmark"></div>
                                            </template>
                                        </BaseRadio>
                                    </div>
                                </div>
                            </div>
                            
                        </div>

                        
                    </div>
                </div>

                <div class="modal-footer">
                    <div class="flex-row flex-end">

                        <BaseButton tabindex="7" @keydown.tab.prevent.exact="focusCode = true;" @mousedown="closeForm"
                            @keydown.enter="closeForm" class="sub-button btn">
                            {{ Resource.Button.Cancel }}
                        </BaseButton>

                        <el-tooltip effect="dark" :content="Const.KeyStrokes.CtrlShiftS" placement="top">
                            <BaseButton tabindex="6" @click="save(true)" @keydown.enter="save(true)"
                                v-show="!checkForm()"
                                class="button-blue btn ml-10">
                                {{ Resource.Button.SaveAndNew }}
                            </BaseButton>
                        </el-tooltip>

                        <el-tooltip effect="dark" :content="Const.KeyStrokes.CtrlS" placement="top">
                            <BaseButton tabindex="5" @click="save(false)" @keydown.enter="save(false)"
                                v-show="!checkForm()"
                                class="main-button btn ml-10">{{ Resource.Button.Save }}</BaseButton>
                        </el-tooltip>

                        <el-tooltip effect="dark" :content="Const.KeyStrokes.CtrlS" placement="top">
                            <BaseButton tabindex="5" @click="save(false)" @keydown.enter="save(false)"
                                v-show="checkForm()" style="padding: 0 16px;"
                                class="main-button btn ml-10">{{ Resource.Button.SaveChange }}
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
import BaseRadio from '@/components/base/BaseRadio.vue';
import CommonFn from '@/utils/commonFuncion';
import axios from 'axios';
import BaseToastMessage from '@/components/base/BaseToastMessage.vue';
export default {
    components: {
        BaseButton, BasePopup, InputString, BaseRadio, BaseToastMessage
    },
    props: ["closeFormDetail", "giftIdUpdate", "formMode"],
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
                giftCode: '',
                description: '',
            },

            giftInit: {
                giftCode: '',
                description: '',
                status: true,
            },

            giftModel: {}, // thông tin quà tặng trong form thêm mới

            focusCode: false,

            toastMessage: { // cảnh báo
                message: "", // nội dung cảnh báo
                type: "", // loại cảnh báo
                isShowed: false, // cờ điều khiển bật tắt cảnh báo
            },
        };
    },

    methods: {

        updateValue(value, nameProperty) {
            this.giftModel[nameProperty] = value;
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
         * Lưu thông tin quà tặng
         * @param {*} control: kiểu lưu quà tặng, nếu false thì lưu và đóng form, còn true thì lưu và reset dữ liệu, không đóng form
         *  Author: HAQUAN(29/08/2023)  
         */
        save(control) {
            try {
                let valid = this.validate();
                if (valid) {
                    // Nếu như là chỉnh sửa thông tin quà tặng
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
         * Gửi request chỉnh sửa thông tin quà tặng đến server
         *  Author: HAQUAN(29/08/2023) 
         */
        async sendRequestUpdate() {
            try {
                this.giftModel.modifiedBy = this.$store.getters.user.accountID;
                await axios.put("Gift/" + this.giftModel.giftID, this.giftModel)
                    .then((response) => {
                        console.log(response);

                        // Hiển thị toast thông báo thành công
                        this.$emit("showToast", Resource.Message.UpdateGiftSucces, Const.TypeToast.Success);
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
         * Gửi request thêm mới quà tặng
         * @param {*} control: kiểu lưu quà tặng, nếu false thì lưu và đóng form, còn true thì lưu và reset dữ liệu, không đóng form
         *  Author: HAQUAN(29/08/2023) 
         */
        async sendRequestInsert(control) {
            try {
                await axios.post('Gift/', this.giftModel).
                    then((response) => {
                        console.log(response);

                        // Hiển thị toast thông báo thành công
                        this.$emit("showToast", Resource.Message.AddGiftSucces, Const.TypeToast.Success);
                        this.$emit("refreshData");
                        if (!control) {
                            this.closeFormDetail();
                        }
                        else {
                            this.giftModel = { ...this.giftInit };
                            this.giftModel.createdBy = this.$store.getters.user.accountID;
                            this.focusCode = true;
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
                else if(error.response.data.errorCode == Enum.Error.Duplicode) {
                    this.contentPopup = error.response.data.userMsg;
                    this.errors.giftCode = error.response.data.userMsg;
                    this.isShowPopup = true;

                }
            } catch (error) {
                console.log(error);
            }
        },

        validateCode() {
            if (!this.giftModel.giftCode) {
                this.errors.giftCode = Resource.Error.GiftCodeNotEmpty;
                return false;
            }
            this.errors.giftCode = '';
            return true;
        },

        validateDescription() {
            if (!this.giftModel.description) {
                this.errors.description = Resource.Error.GiftInfoNotEmpty;
                return false;
            }
            this.errors.description = '';
            return true;
        },

        validate() {
            try {
                let valid = true;
                if (!this.validateDescription()) {
                    valid = false;
                    this.$refs.infoGift.focus();
                }
                if (!this.validateCode()) {
                    valid = false;
                    this.focusCode = true;
                }
                return valid;

            } catch (error) {
                console.log(error);
                return false;
            }
        },

        /**
         * Lấy thông tin quà tặng theo ID
         * @param {*} giftID : ID của quà tặng muốn lấy
         * Author: HAQUAN(29/08/2023) 
         */
        async getGiftByID(giftID) {
            try {
                this.isLoading = true;
                let url = `Gift/${giftID}`;
                await axios.get(url)
                    .then((response) => {
                        this.giftModel = response.data;
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
            this.focusCode = true;
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

        printBill() {
            window.print();
        }

    },
    created() {
        this.giftModel = { ...this.giftInit };
        this.giftModel.createdBy = this.$store.getters.user.accountID;
        if (this.formMode == Enum.Mode.Edit) {
            this.isTitle = Resource.Title.EditGift;
            this.getGiftByID(this.giftIdUpdate);
        }
    },
    mounted() {
        if (this.formMode == Enum.Mode.Add) {
            this.isTitle = Resource.Title.AddGift;
        }
        // focus vào ô input tên quà tặng
        this.focusCode = true;

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

</style>
  