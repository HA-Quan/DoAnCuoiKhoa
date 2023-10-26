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
                        <div :class="['input-form border', { 'error': error.fullname }]">
                            <input type="text" ref="txtFullname" v-model.trim="registerModel.fullName" maxlength="100"
                                @blur="validateFullname">
                        </div>
                        <div class="error-text">{{ error.fullname }}</div>
                    </div>
                </div>

                <div class="row-form">
                    <label for="">Email</label>
                    <div class="text-field-form">
                        <div :class="['input-form border', { 'error': error.email }]">
                            <input type="text" ref="txtEmail" v-model.trim="registerModel.email" maxlength="150"
                                @blur="this.validateEmail">
                        </div>
                        <div class="error-text">{{ error.email }}</div>
                    </div>
                </div>
                <div class="row-form">
                    <label for="">Username</label>
                    <div class="text-field-form">
                        <div :class="['input-form border', { 'error': error.username }]">
                            <input type="text" ref="txtUsername" v-model.trim="registerModel.userName" maxlength="50"
                                @blur="validateUsername">
                        </div>
                        <div class="error-text">{{ error.username }}</div>
                    </div>
                </div>
                <div class="row-form">
                    <label for="">Password</label>
                    <div class="text-field-form">
                        <div :class="['input-form border', { 'error': error.password }]">
                            <input type="password" ref="txtPassword" v-model.trim="registerModel.password" maxlength="255"
                                @blur="validatePassword">
                            <div v-show="registerModel.password" class="icon" :class="showPassword ? 'hide' : 'visible'"
                                @click="hidePassword"></div>
                        </div>
                        <div class="error-text">{{ error.password }}</div>
                    </div>
                </div>
                <div class="row-form">
                    <label for="">Confirm Password</label>
                    <div class="text-field-form">
                        <div :class="['input-form border', { 'error': error.verifyPassword }]">
                            <input type="password" ref="txtVetifyPassword" v-model.trim="verifyPassword" maxlength="255"
                                @blur="validateVetifyPassword">
                            <div v-show="verifyPassword" class="icon" :class="showVetifyPassword ? 'hide' : 'visible'"
                                @click="hideVetifyPassword"></div>
                        </div>
                        <div class="error-text">{{ error.verifyPassword }}</div>
                    </div>
                </div>
                <!-- <div class="input-box">
                    <input type="text" v-model.trim="registerModel.fullname" maxlength="100">
                    <label>Fullname</label>
                </div>
                <div class="input-box">
                    <input type="text" v-model.trim="registerModel.email" maxlength="150">
                    <label>Email</label>
                </div>
                <div class="input-box">
                    <input type="text" v-model.trim="registerModel.username" maxlength="255">
                    <label>Username</label>
                </div>
                <div class="input-box">
                    <div v-show="registerModel.password" class="icon" :class="showPassword ? 'hide' : 'visible'"
                        @click="hidePassword"></div>
                    <input type="password" ref="password" v-model.trim="registerModel.password" maxlength="255">
                    <label>Password</label>
                </div>
                <div class="input-box">
                    <div v-show="verifyPassword" class="icon" :class="showVerifyPassword ? 'hide' : 'visible'"
                        @click="hideVerifyPassword"></div>
                    <input type="password" ref="password" v-model.trim="verifyPassword" maxlength="255">
                    <label>Confirm Password</label> 
                </div>-->
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

export default {
    name: 'RegisterForm',
    data() {
        return {
            registerModel: {
                userName: null,
                password: null,
                email: null,
                fullName: null
            },
            error: {
                username: null,
                password: null,
                email: null,
                fullname: null,
                verifyPassword: null
            },
            verifyPassword: null,
            showPassword: false,
            showVetifyPassword: false,
        };
    },
    methods: {
        goToLogin() {
            this.$router.push({ path: '/login' });
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
                console.log(1);
                if (await this.validate()) {
                    await axios.post("Account/SingUp", this.registerModel)
                        .then((response) => {
                            console.log(2);
                            console.log(response);
                        })
                        .catch((error) => {
                            console.log(3);
                            console.log(error);
                        });
                }
                else {
                    console.log(5);
                }
            } catch(error) {
                console.log(error);
            }
        },

        validate() {
            try {
                let valid = true;
                if (!this.validateVetifyPassword()) {
                    this.$refs.txtVetifyPassword.focus();
                    valid = false;
                }

                if (!this.validatePassword()) {
                    this.$refs.txtPassword.focus();
                    valid = false;
                }

                if (!this.validateUsername()) {
                    this.$refs.txtUsername.focus();
                    valid = false;
                }

                if (!this.validateEmail()) {
                    this.$refs.txtEmail.focus();
                    valid = false;
                }

                if (!this.validateFullname()) {
                    this.$refs.txtFullname.focus();
                    valid = false;
                }
                console.log(4);
                return valid;
            } catch (error) {
                console.log(error);
            }

        },
        validateFullname() {
            if (!this.registerModel.fullName) {
                this.error.fullname = "Họ tên không được để trống!";
                return false;
            }
            this.error.fullname = null;
            return true;
        },
        validateEmail() {
            var validEmail = '^[\\w-.]+@([\\w-]+.)+[\\w-]{2,4}$';
            if (!this.registerModel.email) {
                this.error.email = "Email không được để trống!";
                return false;
            }
            if (!new RegExp(validEmail).test(this.registerModel.email)) {
                this.error.email = "Email không hợp lệ!";
                return false;
            }
            this.error.email = null;
            return true;
        },

        validateUsername() {
            if (!this.registerModel.userName) {
                this.error.username = "Tên đăng nhập không được để trống!";
                return false;
            }
            if (this.registerModel.userName.includes(' ')) {
                this.error.username = "Tên đăng nhập không được chứa dấu cách!";
                return false;
            }
            this.error.username = null;
            return true;
        },

        validatePassword() {
            // var validPassword = '/^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z]{8,}$/';
            if (!this.registerModel.password) {
                this.error.password = "Mật khẩu không được để trống!";
                return false;
            }
            if (this.registerModel.password.includes(' ')) {
                this.error.password = "Mật khẩu không được chứa dấu cách!";
                return false;
            }
            // if (!new RegExp(validPassword).test(this.registerModel.password)) {
            //     this.error.password = "Mật khẩu phải có ít nhất 8 ký tự, có ít nhất một chữ thường, một chữ hoa, một chữ số và một kí tự đặc biệt!";
            //     return false;
            // }
            this.error.password = null;
            return true;
        },

        validateVetifyPassword() {
            if (this.registerModel.password != this.verifyPassword) {
                this.error.verifyPassword = "Mật khẩu xác nhận không khớp!";
                return false;
            }
            this.error.verifyPassword = null;
            return true;
        }
    }
}
</script>
<style scoped>
.register-form{
    top: 0;
    left: 0;
    width: 100vw;
    height: 100vh;
    position: fixed;
    background: url(../assets/image/3047902.jpg) no-repeat;
    background-size: cover;
    background-position: center;
}

.register-box{
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

.title{
    width: 100%;
    height: 60px;
}

.title h2{
    font-size: 2em;
    color: #000;
    text-align: center;
}

.content{
    display: flex;
    flex-direction: column;
}

.row-form{
    margin-bottom: 16px;
    display: flex;
    flex-direction: column;
}

label{
    color: #000;
    font-size: 14px;
    font-weight: 400;
    height: 20px;
    margin-bottom: 8px;
    padding: 0;
    width: auto;
}

.text-field-form{
    background: #fff;
    padding: 0;
    width: 100%;
    border-radius: 3.5px;
}

.input-form{
    display: flex;
    flex-direction: row;
    align-items: center;
}

.input-form input{
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

.icon{
    height: 24px;
    width: 24px;
    position: relative;
    left: -16px;
    margin-left: 12px;
    cursor: pointer;
}

.icon.visible{
    background: transparent url(../assets/icon/visible.png) no-repeat;
    background-position: center;
    background-size: 24px 24px;
}

.icon.hide{
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

.footer{
    margin-top: 20px;
    border-top: 1px solid #e0e0e0;
    padding-top: 20px;
    display: flex;
    flex-direction: column;
}

.submit{
    display: flex;
    justify-content: center;
}
.login{
    margin-top: 20px;
    display: flex;
    justify-content: center;
}

.login a{
    color: #000;
    text-decoration: none;
    font-weight: 600;
}

.login a:hover{
    text-decoration: underline;
    cursor: pointer;
}
</style>