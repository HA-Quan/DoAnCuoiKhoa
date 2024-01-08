<template>
    <div class="container-login">
        <div class="login-box">
            <h2>Login</h2>
            <div class="input-box">
                <input type="text" v-model.trim="this.loginModel.username" maxlength="50" required @keydown="error =''">
                <label>Username</label>
            <div class="error-text">{{ error }}</div>
                
            </div>
            <div class="input-box">
                <div v-show="loginModel.password" class="icon" :class="showPassword ? 'hide' : 'visible'"
                    @click="hidePassword"></div>
                <input type="password" ref="password" v-model.trim="this.loginModel.password" maxlength="255" required @keydown="error =''">
                <label>Password</label>
            <div class="error-text">{{ error }}</div>
            </div>
            <div class="remember-forgot">
                <label>
                    <input type="checkbox">
                    Remember me
                    <a href="#">Forgot Password?</a>
                </label>
            </div>
            <button type="submit" @click="login">
                Login
            </button>
            <div class="register">
                <p>Don't have an account?
                    <a @click="goToRegister">Register</a>
                </p>
            </div>
        </div>
    </div>
</template>
<script>
import axios from 'axios';
import CommonFn from '@/utils/commonFuncion';
export default {
    name: 'LoginForm', 
    data() {
        return {
            loginModel: {
                username: null,
                password: null,
            },
            error: '',
            showPassword: false,
        };
    },
    methods: {
        hidePassword() {
            this.showPassword = !this.showPassword;
            if (this.showPassword) {
                this.$refs.password.type = "text";
            }
            else {
                this.$refs.password.type = "password";
            }
        },
        goToRegister() {
            this.$router.push({ path: '/register' });
        },
        async login() {
            try {
                let url = `Account/login`;
                await axios.post(url, this.loginModel)
                    .then((response) => {
                        CommonFn.setCookie('RefreshToken', response.data.refreshToken, 60);
                        CommonFn.setCookie('Token', response.data.accessToken, 60);
                        axios.defaults.headers.common["Authorization"] = "Bearer " + response.data.accessToken;
                        this.$store.dispatch('setUser', response.data.infoUser);
                        this.$router.push({ name: 'HomePage'});
                    })
                    .catch((error) => {
                        this.error = error.response.data.userMsg[0];
                    });
            } catch (error) {
                console.log(error);
            }
        }
    }
}
</script>
<style scoped>
.container-login {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
    font-family: Arial, Helvetica, sans-serif;
    display: flex;
    justify-content: center;
    align-items: center;
    width: 100%;
    height: 100vh;
    background: url(../../assets/image/3047902.jpg) no-repeat;
    background-size: cover;
    background-position: center;
}

.login-box {
    position: relative;
    width: 400px;
    height: 450px;
    background: #fff;
    border-radius: 10px;
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: column;
    /* box-sizing: border-box; */
}

h2 {
    font-size: 2em;
    color: #000;
    text-align: center;
}

.input-box {
    position: relative;
    width: 310px;
    margin: 30px 0;
    border-bottom: 2px solid #000;
}

.input-box label {
    position: absolute;
    top: 50%;
    left: 5px;
    transform: translateY(-50%);
    font-size: 1em;
    color: #000;
    transition: .5s;
}

.input-box input:focus~label,
.input-box input:valid~label {
    top: -5px;
}

.input-box input {
    width: 100%;
    height: 40px;
    background: transparent;
    font-size: 1em;
    color: #000;
    border: none;
    outline: none;
    padding: 0 35px 0 5px;
}

.remember-forgot {
    margin: -15px 0 15px;
    font-size: .9em;
    color: #000;
    display: flex;
    justify-content: space-between;
    position: relative;

}

.remember-forgot label input {
    margin-right: 3px;
}

.remember-forgot input {
    position: absolute;
    top: -6%;
    left: -10%;
}

.remember-forgot a {
    margin-left: 30px;
    color: #000;
    text-decoration: none;
}

.remember-forgot a:hover {
    text-decoration: underline;
}

button {
    width: 80%;
    height: 40px;
    background: #82cdf6;
    border: none;
    outline: none;
    border-radius: 40px;
    cursor: pointer;
    font-size: 1em;
    color: #000000;
    font-weight: 500;
}

button:hover {
    background: #78c3eb;
}

.register {
    font-size: .9em;
    color: #000;
    text-align: center;
    margin: 25px 0 10px;
}

.register p a {
    color: #000;
    text-decoration: none;
    font-weight: 600;
}

.register a:hover {
    text-decoration: underline;
    cursor: pointer;
}

.input-box .icon {
    position: absolute;
    right: 8px;
    top: 22%;
    width: 24px;
    height: 24px;
    cursor: pointer;
}

.icon.visible {
    background: transparent url(../../assets/icon/visible.png) no-repeat;
    background-position: center;
    background-size: 24px 24px;
}

.icon.hide {
    background: transparent url(../../assets/icon/hide.png) no-repeat;
    background-position: center;
    background-size: 24px 24px;
}
.error-text {
    color: #ef5350;
    margin-top: 6px;
}
</style>