<template>
    <div :class="['input-form border', { 'error': hasError }]">
        <input type="password" ref="txtPassword" v-model.trim="password" :maxlength="maxlength"
            @blur="evenBlur" :tabindex="tabindex" :placeholder="placeholder">
        <div v-show="password" class="icon" :class="showPassword ? 'hide' : 'visible'"
            @click="hidePassword"></div>
    </div>
</template>
<script>
export default {
    data() {
        return {
            password: "",
            showPassword: false
        };
    },
    props: {
        transmissionString: { // giá trị truyền vào
            type: String
        },

        hasError: {
            type: Boolean,
            default: false,
        },

        placeholder: {
            type: String
        },

        tabindex: {
            type: String
        },

        maxlength: {
            type: String
        },

        isFocus: {
            type: Boolean,
            default: false
        },

        nameProperty: {
            type: String
        },

    },

    methods: {
        evenBlur() {
            this.$emit('eventBlur');
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

    },
    created() {
        this.password = this.transmissionString;
    },
    watch: {
        transmissionString() {
            this.password = this.transmissionString;
        },
        isFocus() {
            if (this.isFocus) {
                this.$refs.txtPassword.focus();
            }
        },
        'password': {
            handler: function (newVal) {
                if (newVal != undefined) {
                    this.$emit("updateValue", newVal, this.nameProperty);
                }
            }
        },
    }
};
</script>
<style scoped>

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
.icon{
    height: 24px;
    width: 24px;
    position: relative;
    left: -16px;
    margin-left: 12px;
    cursor: pointer;
}

.icon.visible{
    background: transparent url(../../assets/icon/visible.png) no-repeat;
    background-position: center;
    background-size: 24px 24px;
}

.icon.hide{
    background: transparent url(../../assets/icon/hide.png) no-repeat;
    background-position: center;
    background-size: 24px 24px;
}
</style>
  