<template>
    <BaseLoading v-if="isLoading" />
    <div v-else class="body flex-column box">
        <div class="title-box">{{ Resource.Label.Info }}</div>
        <div class="box-info flex-row">
            <div class="info flex">
                <table>
                    <tbody>
                        <tr v-show="!modeUpdate" :class="{ 'border-bottom': !modeUpdate }">
                            <td>
                                <p>{{ Resource.Label.Username }}:</p>
                            </td>
                            <td>
                                <p>{{ accountNow.username }}</p>
                            </td>
                        </tr>
                        <tr :class="{ 'border-bottom': !modeUpdate }">
                            <td>
                                <p>{{ Resource.Label.FullName }}:</p>
                            </td>
                            <td>
                                <p v-if="!modeUpdate">{{ accountNow.fullName }}</p>
                                <div v-else>
                                    <InputString :transmissionString="account.fullName" :hasError="errors.fullName != ''"
                                        :placeholder="Resource.Placehoder.FullName" maxlength="100"
                                        :nameProperty="Resource.AccountProperty.FullName" @updateValue="updateValue"
                                        @eventBlur="focusFullName = false; validateFullName();" :isRef="focusFullName" />

                                    <div class="error-text">{{ errors.fullName }}</div>
                                </div>
                            </td>
                        </tr>
                        <tr :class="{ 'border-bottom': !modeUpdate }">
                            <td>
                                <p>{{ Resource.Label.Email }}:</p>
                            </td>
                            <td>
                                <p v-if="!modeUpdate">{{ accountNow.email }}</p>
                                <div v-else>
                                    <InputString :transmissionString="account.email" :hasError="errors.email != ''"
                                        :placeholder="Resource.Placehoder.Email" maxlength="150"
                                        :nameProperty="Resource.AccountProperty.Email" @updateValue="updateValue"
                                        @eventBlur="focusEmail = false; validateEmail();" :isRef="focusEmail" />

                                    <div class="error-text">{{ errors.email }}</div>
                                </div>
                            </td>
                        </tr>
                        <tr :class="{ 'border-bottom': !modeUpdate }">
                            <td>
                                <p>{{ Resource.Label.Phone }}:</p>
                            </td>
                            <td>
                                <p v-if="!modeUpdate">{{ accountNow.phone }}</p>
                                <div v-else>
                                    <InputString :transmissionString="account.phone" :hasError="errors.phone != ''"
                                        :placeholder="Resource.Placehoder.Phone" maxlength="20"
                                        :nameProperty="Resource.AccountProperty.Phone" @updateValue="updateValue"
                                        @eventBlur="focusPhone = false; validatePhone();" :isRef="focusPhone" />

                                    <div class="error-text">{{ errors.phone }}</div>
                                </div>
                            </td>
                        </tr>
                        <tr :class="{ 'border-bottom': !modeUpdate }">
                            <td>
                                <p>{{ Resource.Label.Address }}:</p>
                            </td>
                            <td>
                                <p v-if="!modeUpdate">{{ accountNow.address }}</p>
                                <div v-else>
                                    <InputString :transmissionString="account.address"
                                        :placeholder="Resource.Placehoder.Address" maxlength="255"
                                        :nameProperty="Resource.AccountProperty.Address" @updateValue="updateValue" />
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <div v-if="!modeUpdate" class="d-flex justify-content mt-20">
                    <BaseButton @click="modeUpdate = true" class=" main-button btn pad-0-16">
                        {{ Resource.Button.UpdateInfo }}
                    </BaseButton>
                </div>
                <div v-else class="d-flex justify-content mt-10">
                    <BaseButton @click="btnCancelOnclick" class="btn sub-button ml-10">
                        {{ Resource.Button.Cancel }}
                    </BaseButton>
                    <BaseButton @click="btnSaveOnclick" class="btn main-button-red ml-10">
                        {{ Resource.Button.SaveChange }}
                    </BaseButton>
                </div>
            </div>
            <div class="avatar flex">
                <div class="image d-flex justify-content">
                    <img :src="url" ref="avt" alt="">
                </div>
                <div class="d-flex justify-content mt-20">
                    <input class="input-file" type="file" ref="avatar" accept="image/*" @change="changeAvatar">

                    <BaseButton @click="cancelUploadAvatar" class="sub-button btn ml-10"
                        v-show="url != Resource.PrefixImage + account.avatar">
                        {{ Resource.Button.Cancel }}
                    </BaseButton>
                    <BaseButton @click="uploadAvatar" class="main-button-red btn ml-10"
                        v-show="url != Resource.PrefixImage + account.avatar">
                        {{ Resource.Button.UpdateAvatar }}
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
import InputString from '@/components/base/BaseInputString.vue';
import CommonFn from '@/utils/commonFuncion';
export default {
    name: "InfoAccount",
    components: {
        BaseLoading, BaseToastMessage, BaseButton, InputString
    },
    data() {
        return {
            Resource: Resource,
            Enum: Enum,
            Const: Const,

            isLoading: false, // cờ điều khiển bật tắt trạng thái loading

            accountNow: {}, //tài khoản đang được chọn hiện tại 

            account: {},

            toastMessage: { // cảnh báo
                message: "", // nội dung cảnh báo
                type: "", // loại cảnh báo
                isShowed: false, // cờ điều khiển bật tắt cảnh báo
            },

            modeUpdate: false,

            errors: {
                fullName: '',
                email: '',
                phone: '',
            },

            focusFullName: false,
            focusEmail: false,
            focusPhone: false,
            url: '',
        };
    },
    methods: {
        changeAvatar() {
            this.url = URL.createObjectURL(this.$refs.avatar.files[0]);
        },
        cancelUploadAvatar() {
            this.url = Resource.PrefixImage + this.account.avatar;
        },

        updateValue(value, nameProperty) {
            this.account[nameProperty] = value;
        },

        btnCancelOnclick() {
            this.modeUpdate = false;
            this.account = { ...this.accountNow };
            this.errors.fullName = '';
            this.errors.email = '';
            this.errors.phone = '';
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
                this.account.modifiedBy = this.$store.getters.user.accountID;
                await axios.put("Account/" + this.$store.getters.user.accountID, this.account)
                    .then((response) => {
                        console.log(response);
                        this.accountNow = response.data;
                        this.$store.dispatch('setUser', this.accountNow);
                        this.modeUpdate = false;
                        // Hiển thị toast thông báo thành công
                        this.showToast(Resource.Message.UpdateInfoSucces, Const.TypeToast.Success);
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
        * Gửi request upload avatar đến server
        *  Author: HAQUAN(29/08/2023) 
        */
        async uploadAvatar() {
            let data = {
                accountID: this.accountNow.accountID,
                image: this.$refs.avatar.files[0]
            };
            try {
                await axios.post('Account/update-avatar', data, {
                    headers: {
                        'Content-Type': 'multipart/form-data'
                    },
                }).then((response) => {
                    console.log(response);
                    this.getData();
                    // Hiển thị toast thông báo thành công
                    this.showToast(Resource.Message.UpdateAvatarSucces, Const.TypeToast.Success);

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
                        this.account = { ...this.accountNow };
                        this.url = Resource.PrefixImage + this.account.avatar;

                    })

                    .catch((error) => {
                        console.log(error);
                        this.handleErrorResponse(error);
                    })

                    .finally(() => {
                        this.isLoading = false;
                    });
            } catch (error) {
                console.log(error);
            }
        },

        validate() {
            try {
                let valid = true;
                if (!this.validatePhone()) {
                    valid = false;
                    this.focusPhone = true;
                }
                if (!this.validateEmail()) {
                    valid = false;
                    this.focusEmail = true;
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

        validateFullName() {
            if (!this.account.fullName) {
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
                // eslint-disable-next-line
                var validEmail = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/;
                if (!this.account.email) {
                    this.errors.email = Resource.Error.Email;
                    return false;
                }
                if (!new RegExp(validEmail).test(this.account.email)) {
                    this.errors.email = Resource.Error.InvalidEmail;
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
                if (!this.account.phone) {
                    this.errors.phone = Resource.Error.PhoneNotEmpty;
                    return false;
                }
                if (!new RegExp(validPhone).test(this.account.phone)) {
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

/* .box-info .item-info {
    margin: 15px 0;
}

.box-info .item-info p {
    font-weight: 400;
    margin-left: 10px;
    text-transform: capitalize;
    font-size: 16px;
}

.box-info .item-info span {
    font-weight: 600;
} */
table {
    border-collapse: collapse;
    width: 100%;
    max-width: 100%;
}

table tr {
    height: 41px;
}

.border-bottom {
    border-bottom: solid 1px #a9a6a6;
}

table td {
    padding: 5px 0;
}

table td:nth-child(odd) {
    font-weight: 700;
    width: 30%;
}

img {
    width: 205px;
    height: 205px;
}

input.input-file {
    margin-left: 10px;
    height: 36px;
    background-color: #4d8cf2;
    padding: 8px;
    color: #fff;
    border: 2px solid #4d8cf2;
    border-radius: 4px;
    cursor: pointer;
    max-width: 80px;
}

input.input-file::before {
    content: 'Upload file';
}

input.input-file::-webkit-file-upload-button {
    visibility: hidden;
}

input.input-file+label {
    background-color: #4d8cf2;
    padding: 8px;
    color: #fff;
    border: 2px solid #4d8cf2;
    border-radius: 4px;
}

input.input-file+label:hover {
    background-color: #3b73ce;
    cursor: pointer;
}

input.input-file:hover {
    background-color: #3b73ce;
}
</style>