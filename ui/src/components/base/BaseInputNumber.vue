<template>
    <div :class="['flex-row border align-items-center ',
        { 'error': hasError }, {}]" v-if="!isCart">
        <input class="input-form flex text-right" :class="hasIcon ? ' padding-r' : ''" @keyup="updateValue"
            @blur="this.evenBlur();" :tabindex="tabindex" ref="refName">
        <div class="up-down" v-if="hasIcon">
            <div class="icon-up" @keydown.up="clickOnIconUp" @click="clickOnIconUp">
            </div>
            <div class="icon-down" @keydown.down="clickOnIconDown" @click="clickOnIconDown">
            </div>
        </div>
    </div>
    <div class="flex-row border align-items" v-else>
        <div class="icon-minus" @keydown.down="clickOnIconDown" @click="clickOnIconDown"></div>
        <input class="input-form quantity flex text-right" @keyup="updateValue">
        <div class="icon-sum" @keydown.up="clickOnIconUp" @click="clickOnIconUp"></div>
    </div>
</template>
<script>
import AutoNumeric from "autonumeric";
export default {
    data() {
        return {

        };
    },
    props: {
        transmissionNumber: { // giá trị truyền vào
            type: Number,
            default: 0,
        },

        max: { // giá trị lớn nhất có thể
            type: Number,
            default: 10000000000000,
        },

        decimalPlaces: { // số chữ số phần thập phân
            type: Number,
            default: 0,
        },

        hasIcon: { // cờ kiểm tra xem ô nhập liệu có icon tăng giảm hay không
            type: Boolean,
            default: false,
        },

        hasError: {
            type: Boolean,
            default: false,
        },

        tabindex: {
            type: String
        },

        isRef: {
            type: Boolean,
            default: false
        },

        nameProperty: {
            type: String
        },

        isCart: {
            type: Boolean,
            default: false,
        }

    },

    methods: {
        /**
         * Hàm update giá trị   
         * Author:HAQuan (30/08/2023)
         */
        updateValue() {
            let rawTextValue = this.valueNumeric.rawValue;

            let rawNumberValue = Number(rawTextValue);
            if (!this.isCart) {
                this.$emit('update', rawNumberValue, this.nameProperty);
            }
            else {
                this.$emit('update', rawNumberValue);
            }


        },

        /**
         * Hàm lấy giá trị dạng number từ giá trị dạng text khi truyền vào 
         * Author:HAQuan (30/08/2023)
         */
        getRawValue() {
            let rawTextValue = this.valueNumeric.rawValue;

            let rawNumberValue = Number(rawTextValue);

            return rawNumberValue;
        },

        /**
         * Hàm xử lý khi click vào icon down
         * Author:HAQuan (30/08/2023)
         */
        clickOnIconDown() {
            let rawNumberValue = this.getRawValue();
            if (rawNumberValue >= 1) {
                if (!this.isCart) {
                    this.$emit('update', rawNumberValue - 1, this.nameProperty);
                }
                else {
                    this.$emit('update', rawNumberValue - 1);
                }
            }
        },

        /**
         * Hàm xử lý khi click vào icon up
         * Author:HAQuan (30/08/2023)
         */
        clickOnIconUp() {
            let rawNumberValue = this.getRawValue();
            if (rawNumberValue < this.max) {
                if (!this.isCart) {
                    this.$emit('update', rawNumberValue + 1, this.nameProperty);
                }
                else {
                    this.$emit('update', rawNumberValue + 1);
                }
            }
        },

        evenBlur() {
            this.$emit('eventBlur');
        }

    },

    mounted() {
        // Khởi tạo 1 biến autonumeric
        const numeric = new AutoNumeric(this.$el.querySelector(".input-form"), {
            digitGroupSeparator: ".",
            decimalCharacter: ",",
            maximumValue: this.max,
            minimumValue: 0,
            decimalPlaces: this.decimalPlaces,
        });

        this.valueNumeric = numeric;

        if (this.transmissionNumber !== 0) {
            this.valueNumeric.set(this.transmissionNumber);
        }


    },
    watch: {
        transmissionNumber() {
            let rawNumberValue = this.getRawValue();
            if (this.transmissionNumber != rawNumberValue) {
                this.valueNumeric.set(this.transmissionNumber);
            }
        },
        isRef() {
            if (this.isRef) {
                this.$refs.refName.focus();
            }
        },
    }
};
</script>
<style scoped>
.input-form {
    border: none;
    padding: 9px 12px;
    background: 0 0;
    min-height: 34px;
}

input:focus {
    outline: none;
}

.flex-row {
    display: flex;
    position: relative;
}

.up-down {
    position: absolute;
    top: 50%;
    right: 13px;
    transform: translateY(-50%);
    height: 17px;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    cursor: pointer;
}

.icon-up {
    background: url(../../assets/icon/arrow-right.png);
    background-size: 8px 8px;
    width: 8px;
    height: 8px;
    transform: rotate(-90deg);
}

.icon-down {
    background: url(../../assets/icon/arrow-right.png);
    background-size: 8px 8px;
    width: 8px;
    height: 8px;
    transform: rotate(90deg);
}

.icon-minus {
    margin-left: 10px;
    background: url(../../assets/icon/minus.png);
    background-size: 16px 16px;
    width: 16px;
    height: 16px;
    cursor: pointer;
}

.icon-sum {
    margin-right: 10px;
    background: url(../../assets/icon/plus.png);
    background-size: 16px 16px;
    width: 16px;
    height: 16px;
    cursor: pointer;
}

.input-form.quantity {
    width: 40px;
    text-align: center;
}</style>
  