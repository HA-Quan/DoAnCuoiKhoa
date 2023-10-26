<template>
    <BaseLoading v-if="isLoading" />
    <div v-else class="body flex-column box">
        <div class="title-box">{{ Resource.Label.ChangePassword }}</div>
        <div class="box-info d-flex">
            <div class="info">
                <table>
                    <tbody>
                        <tr>
                            <td>
                                <p>{{ Resource.Label.PasswordOld }}:</p>
                            </td>
                            <td>
                                <div class="text-field-form">
                                    <div :class="['input-form border', { 'error': errors.passwordOld }]">
                                        <input type="password" ref="txtPasswordOld" v-model.trim="passwordModel.passwordOld"
                                            maxlength="255" @blur="validatePasswordOld">
                                        <div v-show="passwordModel.passwordOld" class="icon"
                                            :class="showPasswordOld ? 'hide' : 'visible'" @click="hidePasswordOld"></div>
                                    </div>
                                    <div class="error-text">{{ errors.passwordOld }}</div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <p>{{ Resource.Label.Password }}:</p>
                            </td>
                            <td>
                                <div class="text-field-form">
                                    <div :class="['input-form border', { 'error': errors.password }]">
                                        <input type="password" ref="txtPassword" v-model.trim="passwordModel.password"
                                            maxlength="255" @blur="validatePassword(); validateVerifyPassword()">
                                        <div v-show="passwordModel.password" class="icon"
                                            :class="showPassword ? 'hide' : 'visible'" @click="hidePassword"></div>
                                    </div>
                                    <div class="error-text">{{ errors.password }}</div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <p>{{ Resource.Label.VerifyPassword }}:</p>
                            </td>
                            <td>
                                <div class="text-field-form">
                                    <div :class="['input-form border', { 'error': errors.verifyPassword }]">
                                        <input type="password" ref="txtVerifyPassword"
                                            v-model.trim="passwordModel.verifyPassword" maxlength="255"
                                            @blur="blurVerifyPassword = true; validateVerifyPassword()">
                                        <div v-show="passwordModel.verifyPassword" class="icon"
                                            :class="showVerifyPassword ? 'hide' : 'visible'" @click="hideVerifyPassword">
                                        </div>
                                    </div>
                                    <div class="error-text">{{ errors.verifyPassword }}</div>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>

                <div class="d-flex justify-content mt-10">
                    <BaseButton @click="btnCancelOnclick" class="btn sub-button ml-10">
                        {{ Resource.Button.Cancel }}
                    </BaseButton>
                    <BaseButton @click="btnSaveOnclick" class="btn main-button-red ml-10">
                        {{ Resource.Button.UpdatePassword }}
                    </BaseButton>
                </div>
            </div>
        </div>

    </div>

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
import BaseToastMessage from '@/components/base/BaseToastMessage.vue';
import BaseButton from '@/components/base/BaseButton.vue';
export default {
    name: "ChangePassword",
    components: {
        BaseLoading, BaseToastMessage, BaseButton
    },
    data() {
        return {
            Resource: Resource,
            Enum: Enum,
            Const: Const,

            isLoading: false, // cờ điều khiển bật tắt trạng thái loading

            accountNow: {}, //tài khoản đang được chọn hiện tại 

            toastMessage: { // cảnh báo
                message: "", // nội dung cảnh báo
                type: "", // loại cảnh báo
                isShowed: false, // cờ điều khiển bật tắt cảnh báo
            },

            contentPopup: null,

            errors: {
                passwordOld: '',
                password: '',
                verifyPassword: '',
            },

            passwordModel: {
                passwordOld: '',
                password: '',
                verifyPassword: '',
            },

            showPasswordOld: false,
            showPassword: false,
            showVerifyPassword: false,
            blurVerifyPassword: false
        };
    },
    methods: {
        hidePasswordOld() {
            this.showPasswordOld = !this.showPasswordOld;
            if (this.showPasswordOld) {
                this.$refs.txtPasswordOld.type = "text";
            }
            else {
                this.$refs.txtPasswordOld.type = "password";
            }
        },
        hidePassword() {
            this.showPassword = !this.showPassword;
            if (this.showPassword) {
                this.$refs.txtPassword.type = "text";
            }
            else {
                this.$refs.txtPassword.type = "password";
            }
        },
        hideVerifyPassword() {
            this.showVerifyPassword = !this.showVerifyPassword;
            if (this.showVerifyPassword) {
                this.$refs.txtVerifyPassword.type = "text";
            }
            else {
                this.$refs.txtVerifyPassword.type = "password";
            }
        },
        btnCancelOnclick() {
            this.errors.password = '';
            this.errors.passwordOld = '';
            this.errors.verifyPassword = '';
            this.passwordModel.password = '';
            this.passwordModel.passwordOld = '';
            this.passwordModel.verifyPassword = '';
            this.blurVerifyPassword = false;
        },

        btnSaveOnclick() {
            try {
                let valid = this.validate();
                if (valid) {
                    this.sendRequestUpdate();
                }
            } catch (error) {
                console.log(error);
            }
        },

        /**
        * Gửi request chỉnh sửa thông tin tài khoản đến server
        *  Author: HAQUAN(29/08/2023) 
        */
        async sendRequestUpdate() {
            try {
                let passwordModel = {
                    passwordOld: this.passwordModel.passwordOld,
                    passwordNew: this.passwordModel.password
                };
                await axios.put("Account/change-password" + this.$store.getters.user.accountID, passwordModel)
                    .then((response) => {
                        this.accountNow = response.data;
                        this.$store.dispatch('setUser', response.data);
                        this.passwordModel.password = '';
                        this.passwordModel.passwordOld = '';
                        this.passwordModel.verifyPassword = '';
                        // Hiển thị toast thông báo thành công
                        this.showToast(Resource.Message.ChangePasswordSucces, Const.TypeToast.Success);
                    })

                    .catch((error) => {
                        console.log(error);
                        if (error.response.data.errorCode == Enum.Error.PasswordOldInvalid) {
                            this.errors.passwordOld = Resource.Error.PasswordOldInvalid;
                        }
                    })
                    .finally(() => {
                        this.blurVerifyPassword = false;
                    });
            } catch (error) {
                console.log(error);
            }
        },

        /**
         * Lấy dữ liệu tài khoản 
         * Author: HAQUAN(26/08/2023)
         */
        async getData() {
            try {
                this.isLoading = true;
                let url = `Account/${this.$store.getters.user.accountID}`;
                await axios.get(url)

                    .then((response) => {
                        console.log(response);
                        this.accountNow = response.data;
                    })

                    .catch((error) => {
                        console.log(error);
                    })

                    .finally(() => {
                        this.isLoading = false;
                    });
            } catch (error) {
                console.log(error);
            }
        },

        validatePasswordOld() {
            if (!this.passwordModel.passwordOld) {
                this.errors.passwordOld = Resource.Error.PasswordOld;
                return false;
            }
            this.errors.passwordOld = null;
            return true;
        },

        validatePassword() {
            // var validPassword = '/^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z]{8,}$/';
            if (!this.passwordModel.password) {
                this.errors.password = Resource.Error.Password;
                return false;
            }
            if (this.passwordModel.password.includes(' ')) {
                this.errors.password = Resource.Error.PasswordNotSpace;
                return false;
            }
            // if (!new RegExp(validPassword).test(this.registerModel.password)) {
            //     this.error.password = "Mật khẩu phải có ít nhất 8 ký tự, có ít nhất một chữ thường, một chữ hoa, một chữ số và một kí tự đặc biệt!";
            //     return false;
            // }
            this.errors.password = null;
            return true;
        },

        validateVerifyPassword() {
            if (this.blurVerifyPassword && this.passwordModel.password != this.passwordModel.verifyPassword) {
                this.errors.verifyPassword = Resource.Error.VerifyPassword;
                return false;
            }
            this.errors.verifyPassword = null;
            return true;
        },

        validate() {
            try {
                let valid = true;
                if (!this.validateVerifyPassword()) {
                    valid = false;
                    this.$refs.txtVerifyPassword.focus();
                }
                if (!this.validatePassword()) {
                    valid = false;
                    this.$refs.txtPassword.focus();
                }

                return valid;

            } catch (error) {
                console.log(error);
                return false;
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


    },

    created() {
        // Lấy danh sách tài khoản
        this.getData();
        document.title = Resource.Title.Management;
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

.box {
    height: 100%;
}

.box-info {
    /* width: 100%; */
    height: 100%;
    background: #fff;
    padding-top: 10px;
}

.info {
    padding-left: 10px;
}

table {
    border-collapse: collapse;
    width: 100%;
    max-width: 100%;
}

table tr {
    height: 41px;
}

table td {
    padding: 5px 0;
}

table td:nth-child(odd) {
    font-weight: 700;
    width: 30%;
    min-width: 150px;
}

.text-field-form {
    background: #fff;
    padding: 0;
    width: 100%;
    border-radius: 3.5px;
}

.input-form {
    display: flex;
    flex-direction: row;
    align-items: center;
}

.input-form input {
    border: none;
    padding: 9px 12px;
    background: 0 0;
    min-height: 34px;
    width: 240px;
}

.border {
    border: 1px solid #e0e0e0;
    border-radius: 3.5px;
}

.border.error {
    border: 1px solid #ef5350;
}

:focus-visible {
    outline: none;
}

.border:not(.error):focus-within,
.border:not(.error):hover {
    border: 1px solid #1a73e8;
}

.error-text {
    color: #ef5350;
    margin-top: 6px;
}

.icon {
    height: 24px;
    width: 24px;
    position: relative;
    left: -16px;
    margin-left: 12px;
    cursor: pointer;
}

.icon.visible {
    background: transparent url(../assets/icon/visible.png) no-repeat;
    background-position: center;
    background-size: 24px 24px;
}

.icon.hide {
    background: transparent url(../assets/icon/hide.png) no-repeat;
    background-position: center;
    background-size: 24px 24px;
}</style>