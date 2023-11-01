<template>
    <div class="register-form">
        <div class="register-box">
            <div class="title">
                <h2>Register</h2>
            </div>
            <div class="content">
                <div class="row-form">
                    <label for="">Fullname</label>
                    <div class="text-field-form">
                        <!-- <div :class="['input-form border', { 'error': error.fullname }]">
                            <input type="text" ref="txtFullname" v-model.trim="registerModel.fullName" maxlength="100"
                                @blur="validateFullname">
                        </div> -->
                        <InputString :transmissionString="registerModel.fullName" :hasError="error.fullname != ''"
                            maxlength="100" tabindex="1" :nameProperty="Resource.AccountProperty.FullName"
                            @updateValue="updateValue" @eventBlur="focusFullname = false; validateFullname();"
                            :isRef="focusFullname" />
                        <div class="error-text">{{ error.fullname }}</div>
                    </div>
                </div>

                <div class="row-form">
                    <label for="">Email</label>
                    <div class="text-field-form">
                        <!-- <div :class="['input-form border', { 'error': error.email }]">
                            <input type="text" ref="txtEmail" v-model.trim="registerModel.email" maxlength="150"
                                @blur="this.validateEmail">
                        </div> -->

                        <InputString :transmissionString="registerModel.email" :hasError="error.email != ''"
                            maxlength="150" tabindex="2" :nameProperty="Resource.AccountProperty.Email"
                            @updateValue="updateValue" @eventBlur="focusEmail = false; validateEmail();"
                            :isRef="focusEmail" />
                        <div class="error-text">{{ error.email }}</div>
                    </div>
                </div>
                <div class="row-form">
                    <label for="">Username</label>
                    <div class="text-field-form">
                        <!-- <div :class="['input-form border', { 'error': error.username }]">
                            <input type="text" ref="txtUsername" v-model.trim="registerModel.userName" maxlength="50"
                                @blur="validateUsername">
                        </div> -->
                        <InputString :transmissionString="registerModel.username" :hasError="error.username != ''"
                            maxlength="50" tabindex="3" :nameProperty="Resource.AccountProperty.Username"
                            @updateValue="updateValue" @eventBlur="focusUsername = false; validateUsername();"
                            :isRef="focusUsername" />
                        <div class="error-text">{{ error.username }}</div>
                    </div>
                </div>
                <div class="row-form">
                    <label for="">Password</label>
                    <div class="text-field-form">
                        <!-- <div :class="['input-form border', { 'error': error.password }]">
                            <input type="password" ref="txtPassword" v-model.trim="registerModel.password" maxlength="255"
                                @blur="validatePassword">
                            <div v-show="registerModel.password" class="icon" :class="showPassword ? 'hide' : 'visible'"
                                @click="hidePassword"></div>
                        </div> -->

                        <InputPassword :transmissionString="registerModel.password" :hasError="error.password != ''"
                            maxlength="255" tabindex="4" :nameProperty="Resource.AccountProperty.Password"
                            @updateValue="updateValue" @eventBlur="focusPassword = false; validatePassword();"
                            :isFocus="focusPassword" />
                        <div class="error-text">{{ error.password }}</div>
                    </div>
                </div>
                <div class="row-form">
                    <label for="">Confirm Password</label>
                    <div class="text-field-form">
                        <!-- <div :class="['input-form border', { 'error': error.verifyPassword }]">
                            <input type="password" ref="txtVetifyPassword" v-model.trim="verifyPassword" maxlength="255"
                                @blur="validateVerifyPassword">
                            <div v-show="verifyPassword" class="icon" :class="showVetifyPassword ? 'hide' : 'visible'"
                                @click="hideVetifyPassword"></div>
                        </div> -->

                        <InputPassword :transmissionString="registerModel.verifyPassword"
                            :hasError="error.verifyPassword != ''" maxlength="255" tabindex="5"
                            :nameProperty="Resource.AccountProperty.VerifyPassword" @updateValue="updateValue"
                            @eventBlur="focusVerifyPassword = false; validateVerifyPassword();"
                            :isFocus="focusVerifyPassword" />
                        <div class="error-text">{{ error.verifyPassword }}</div>
                    </div>
                </div>

            </div>
            <div class="footer">
                <div class="submit">
                    <button @click="singUp">
                        Register
                    </button>
                </div>
                <div class="login">
                    <a @click="goToLogin">Login</a>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
import axios from 'axios';
import InputPassword from '@/components/base/BaseInputPassword.vue';
import InputString from '@/components/base/BaseInputString.vue';
import Resource from '@/utils/resource';
export default {
    name: 'RegisterForm',
    components: {
        InputPassword, InputString
    },
    data() {
        return {
            Resource: Resource,
            registerModel: {
                username: '',
                password: '',
                email: '',
                fullName: ''
            },
            error: {
                username: '',
                password: '',
                email: '',
                fullname: '',
                verifyPassword: ''
            },
            verifyPassword: '',
            showPassword: false,
            showVetifyPassword: false,

            focusFullname: false,
            focusEmail: false,
            focusUsername: false,
            focusPassword: false,
            focusVerifyPassword: false,

        };
    },
    
    created(){
        this.focusFullname = true;
    },
    methods: {
        goToLogin() {
            this.$router.push({ path: '/login' });
        },

        updateValue(value, nameProperty) {
            if (nameProperty == Resource.AccountProperty.VerifyPassword) {
                this.verifyPassword = value;
            }
            else {
                this.registerModel[nameProperty] = value;
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

        hideVetifyPassword() {
            this.showVetifyPassword = !this.showVetifyPassword;
            if (this.showVetifyPassword) {
                this.$refs.txtVetifyPassword.type = "text";
            }
            else {
                this.$refs.txtVetifyPassword.type = "password";
            }
        },

        async singUp() {
            try {
                if (await this.validate()) {
                    await axios.post("Account/SingUp", this.registerModel)
                        .then((response) => {
                            console.log(response);
                        })
                        .catch((error) => {
                            console.log(error);
                        });
                }
            } catch (error) {
                console.log(error);
            }
        },

        validate() {
            try {
                let valid = true;
                if (!this.validateVerifyPassword()) {
                    this.focusVerifyPassword = true;
                    valid = false;
                }

                if (!this.validatePassword()) {
                    this.focusPassword = true;
                    valid = false;
                }

                if (!this.validateUsername()) {
                    this.focusUsername = true;
                    valid = false;
                }

                if (!this.validateEmail()) {
                    this.focusEmail = true;
                    valid = false;
                }

                if (!this.validateFullname()) {
                    this.focusFullname = true;
                    valid = false;
                }
                return valid;
            } catch (error) {
                console.log(error);
            }

        },
        validateFullname() {
            if (!this.registerModel.fullName) {
                this.error.fullname = Resource.Error.FullNameNotEmpty;
                return false;
            }
            this.error.fullname = '';
            return true;
        },
        validateEmail() {
            // eslint-disable-next-line
            var validEmail = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/;
            if (!this.registerModel.email) {
                this.error.email = Resource.Error.Email;
                return false;
            }
            if (!new RegExp(validEmail).test(this.registerModel.email)) {
                this.error.email = Resource.Error.InvalidEmail;
                return false;
            }
            this.error.email = '';
            return true;
        },

        validateUsername() {
            if (!this.registerModel.username) {
                this.error.username = Resource.Error.UserName;
                return false;
            }
            this.error.username = '';
            return true;
        },

        validatePassword() {
            var validPassword = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z]{8,}$/;
            if (!this.registerModel.password) {
                this.error.password = Resource.Error.Password;
                return false;
            }

            if (!new RegExp(validPassword).test(this.registerModel.password)) {
                this.error.password = Resource.Error.InvalidPassword;
                return false;
            }
            this.error.password = '';
            return true;
        },

        validateVerifyPassword() {
            if (this.registerModel.password != this.verifyPassword) {
                this.error.verifyPassword = Resource.Error.VerifyPassword;
                return false;
            }
            this.error.verifyPassword = '';
            return true;
        }
    }
}
</script>
<style scoped>
.register-form {
    top: 0;
    left: 0;
    width: 100vw;
    height: 100vh;
    position: fixed;
    background: url(../assets/image/3047902.jpg) no-repeat;
    background-size: cover;
    background-position: center;
}

.register-box {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    width: 450px;
    height: fit-content;
    border-radius: 10px;
    background: #fff;
    display: flex;
    flex-direction: column;
    padding: 20px 40px;
}

.title {
    width: 100%;
    height: 60px;
}

.title h2 {
    font-size: 2em;
    color: #000;
    text-align: center;
}

.content {
    display: flex;
    flex-direction: column;
}

.row-form {
    margin-bottom: 16px;
    display: flex;
    flex-direction: column;
}

label {
    color: #000;
    font-size: 14px;
    font-weight: 400;
    height: 20px;
    margin-bottom: 8px;
    padding: 0;
    width: auto;
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
    width: 100%;
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
}

button {
    width: 50%;
    height: 40px;
    background: #82cdf6;
    border: none;
    outline: none;
    border-radius: 40px;
    cursor: pointer;
    font-size: 1em;
    color: #000000;
    font-weight: 600;
}

button:hover {
    background: #78c3eb;
}

.footer {
    margin-top: 20px;
    border-top: 1px solid #e0e0e0;
    padding-top: 20px;
    display: flex;
    flex-direction: column;
}

.submit {
    display: flex;
    justify-content: center;
}

.login {
    margin-top: 20px;
    display: flex;
    justify-content: center;
}

.login a {
    color: #000;
    text-decoration: none;
    font-weight: 600;
}

.login a:hover {
    text-decoration: underline;
    cursor: pointer;
}
</style>