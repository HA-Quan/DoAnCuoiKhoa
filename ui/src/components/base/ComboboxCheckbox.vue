<template>
    <div @keydown="handleKeyCommand" class="combobox pointer" @focusin="isOpenCombobox = true; setIndexOption()"
        @focusout="focusOut">
        <div :class="['combobox-select flex-row', { 'border-red': isCheck == true }, { disabled: disabled }]">
            <div class="popover flex">
                <input class="input" type="text" :disabled="disabled" :tabindex="tabindex" :placeholder="placehoder"
                    :readonly="isReadonly" />
            </div>
            <div @click="toggleOptions" class="icon-dropdown"></div>
        </div>
        <div v-if="isOpenCombobox && items.length > 0 && !disabled" class="combobox-options pointer">
            <div class="combobox-header d-flex" @click="selectAll">
                <div class="select-all">
                    <BaseCheckbox class="combobox-checkbox" :class="{ 'check-temp': selectedOptions.length > 0 }"
                        :checkedProp="checkList(items, selectedOptions) && items.length !== 0" :notClick="true">
                        <template #checkmark>
                            <div class="checkbox-checkmark"></div>
                        </template>
                    </BaseCheckbox>
                </div>
                <span class="combobox-value">
                    {{ Resource.All }}
                </span>
            </div>
            <div class="combobox-values">
                <div ref="options" v-for="(option, index) in items" @click="handleOptionClicked(option, index)" :key="index"
                    :class="[
                        'combobox-value-wrapper',
                        {
                            'combobox-value-wrapper-selected':
                                selectedOptions.some((obj) => obj[fieldName] == option[fieldName]),
                            'combobox-value-wrapper-hover': hoveredOptionIndex === index,
                        },
                    ]">
                    <div class="select-all">
                        <BaseCheckbox class="combobox-checkbox" :notClick="true" :checkedProp="selectedOptions.some(
                            (obj) => obj[fieldName] == option[fieldName]
                        )">
                            <template #checkmark>
                                <div class="checkbox-checkmark"></div>
                            </template>
                        </BaseCheckbox>
                    </div>
                    <span class="combobox-value">
                        {{ option[fieldName] }}
                    </span>

                </div>
            </div>
        </div>
    </div>
</template>
<script>
import Enum from '@/utils/enum'
import BaseCheckbox from './BaseCheckbox.vue';
import Resource from '@/utils/resource';
export default {
    components: {
        BaseCheckbox
    },
    props: {
        isReadonly: { type: Boolean, default: false }, // cờ thể hiện xem combobox có thể tìm kiếm được hay k?

        isCheck: { type: Boolean, default: false }, // cờ kiểm tra xem combobox có đang ở trạng thái lỗi hay không

        items: { //danh sách item trong combobox
            type: Array,
            default() {
                return [];
            }
        },

        initItem: { // item mặc định ban đầu
            type: Array,
            default() {
                return [];
            }
        },


        disabled: { // trạng thái của combobox 
            type: Boolean
        },

        tabindex: { // tabindex của combobox
            type: Number,
            default: null,
        },

        fieldName: { // tên trường
            type: String,
        },

        placehoder: { // tên trường
            type: String,
        }
    },

    watch: {
        initItem: {
            handler: function (newVal) {
                this.selectedOptions = newVal;
            }
        },
        'isOpenCombobox': {
            handler: function (newVal) {
                if (newVal == false) {
                    // console.log(this.selectedOptions);
                    this.$emit("getValue", this.selectedOptions);
                }
            }
        },
    },


    data() {
        return {
            Resource: Resource,
            isOpenCombobox: false, // cờ điều khiển đóng mở combobox

            selectedOptions: [], // list giá trị combobox đang được chọn

            hoveredOptionIndex: 0, // vị trí phần tử đang trỏ
        };
    },

    created() {
        // xử lý sự kiện khi click chuột ra ngoài vùng combobox sẽ đóng combobox
        window.addEventListener('click', (e) => {
            if (!this.$el.contains(e.target)) {
                this.isOpenCombobox = false
            }
        })
        this.selectedOptions = this.initItem;
        this.setIndexOption();
    },

    methods: {

        /**
         * Xử lý khi người dùng focusout khỏi combobox
         * Author:HAQUAN(28/10/2022) 
         */
        focusOut() {
            // this.isOpenCombobox = false;
            this.$emit("focusOut");
        },

        /**
         * Xử lý khi một option được chọn
         * @param {*} option: option đang được chọn
         * Author: HAQUAN(28/10/2022)
         */
        selectOption(option) {
            try {

                // Nếu như option được chọn đã được chọn trước đó thì loại bỏ khỏi danh sách
                if (this.selectedOptions.find((item) => item[this.fieldName] === option[this.fieldName])) {
                    this.selectedOptions = this.selectedOptions.filter(
                        (op) => op[this.fieldName] !== option[this.fieldName]
                    );
                }

                // Nếu như hàng được chọn chưa được chọn trước đó thì thêm vào danh sách
                else {
                    this.selectedOptions.push(option);
                }
            } catch (error) {
                console.log(error);
            }
        },

        /**
         * Xử lý chọn tất cả các option đang được hiển thị
         * Author: HAQUAN(28/10/2022)
         */
        selectAll() {
            try {

                // Kiểm tra xem những option đang hiển thị có hết trong danh sách những option đã được chọn hay không
                if (this.checkList(this.items, this.selectedOptions)) { // Nếu phải thì bỏ hết những option đó ra khỏi danh sách đã chọn
                    for (let op of this.items) {
                        this.selectedOptions = this.selectedOptions.filter(
                            (item) => item[this.fieldName] != op[this.fieldName]
                        );
                    }
                }
                else { // Ngược lại thì thêm những option chưa ở trong danh sách đã chọn vào danh sách các option đã chọn

                    for (let item of this.items) {
                        if (!this.selectedOptions.some((obj) => obj[this.fieldName] == item[this.fieldName])) {
                            this.selectedOptions.push(item);
                        }
                    }
                }

            } catch (error) {
                console.log(error);
            }
        },

        /**
         * Kiểm tra 2 danh sách sản phẩm
         * @param {*} firstList: danh sách sản phẩm cần so sánh
         * @param {*} secondList: danh sách sản phẩm cần so sánh
         * Author: HAQUAN(28/10/2022)
         */
        checkList(firstList, secondList) {

            if (firstList.length == 0 || secondList.length == 0) {
                return false;
            }

            try {
                let count = 0;
                for (let item of firstList) {
                    if (secondList.some((obj) => obj[this.fieldName] == item[this.fieldName])) {
                        count++;
                    }
                }

                if (count == firstList.length) {
                    return true;
                }

            } catch (error) {
                console.log(error);
            }
            return false;
        },

        /**
         * Xét index cho item đang được chọn trong combobox
         * Author:HAQUAN(28/10/2022) 
         */
        setIndexOption() {
            this.hoveredOptionIndex = 0;
        },


        /**
         * Xử lý khi dùng bàn phím
         * Author: HAQUAN(28/10/2022) 
         */
        handleKeyCommand() {
            let keyCode = event.keyCode;
            switch (keyCode) {
                case Enum.KeyCode.Enter:
                    if (this.hoveredOptionIndex >= 0 && this.isOpenCombobox === true) {
                        this.selectOption(this.items[this.hoveredOptionIndex]);
                    }
                    this.isOpenCombobox = false;
                    break;

                case Enum.KeyCode.ArrowDown:
                    if (this.hoveredOptionIndex < this.items.length - 1) {
                        this.hoveredOptionIndex++;
                        this.scrollToElement();
                    }
                    if (!this.isOpenCombobox) {
                        this.toggleOptions();
                    }
                    break;

                case Enum.KeyCode.ArrowUp:
                    if (this.hoveredOptionIndex > 0) {
                        this.hoveredOptionIndex--;
                        this.scrollToElement();
                    }
                    break;
                default:
                    break;
            }
        },

        /**
         * Xử lý khi click vào 1 option của combobox
         * Author: HAQUAN(28/10/2022) 
         */
        handleOptionClicked(option, index) {
            this.selectOption(option);
            this.hoveredOptionIndex = index;
        },

        /**
         * Ẩn/hiện danh sách options
         * Author: HAQUAN(28/10/2022) 
         */
        toggleOptions() {
            this.isOpenCombobox = !this.isOpenCombobox;
            if (this.isOpenCombobox) {
                this.setIndexOption();
                this.scrollToElement();
            }
        },

        /**
         * Cuộn đến phần tử được hover
         * Author: HAQUAN(28/09/2022) 
         */
        scrollToElement() {
            console.log("optional");
            // this.$refs.options[this.hoveredOptionIndex].scrollIntoView({ behavior: "smooth", inline: "end", block: "end" });
        },
    },
};
</script>
<style scoped>
.combobox {
    width: 100%;
    background-color: transparent;
    position: relative;
}

.combobox .disabled {
    background-color: #ebebeb !important;
    color: #666;
}

.combobox-small .combobox-options {
    z-index: 2;
    bottom: 100%;
    top: unset;
}

.combobox-select {
    background: #fff;
    border: 1px solid #e0e0e0;
    border-radius: 3.5px;
}

.combobox-select.border-red {
    border: 1px solid #ef5350;
}

.combobox:not(.disabled).combobox-select:focus-within,
.combobox:not(.disabled).combobox-select:hover {
    border-color: #2979ff;
}

.combobox-select .input {
    margin-left: 12px;
    border: 0;
    width: calc(100% - 12px);
    padding: 0;
    height: 35px;
    outline: none;
}

.popover {
    border: 0;
    width: 100%;
    padding: 0;
    flex: 1;
}

.combobox-options {
    position: absolute;
    width: 100%;
    padding: 8px;
    top: calc(100% + 1px);
    left: 0;
    border: 1px solid #cccccc;
    border-radius: 4px;
    box-shadow: rgb(0 0 0 / 16%) 0px 3px 6px;
    /* margin-top: 8px; */
    background-color: white;
    z-index: 2;
    max-height: 190px;
    overflow: auto;
}

.combobox-options::-webkit-scrollbar {
    width: 5px;
}

.combobox-options::-webkit-scrollbar-thumb {
    background: #cccccc;
    border-radius: 4px;
}

.combobox-options::-webkit-scrollbar-thumb:hover {
    background: #8f8f8f;
}

.combobox-value {
    padding-right: 10px;
}

.combobox-value-wrapper:hover {
    background-color: #dfebff;
    cursor: pointer;
}

.combobox-value-wrapper {
    display: flex;
    align-items: center;
    border-radius: 4px;
    min-height: 36px;
}

.combobox-header {
    display: flex;
    align-items: center;
    border-radius: 4px;
    min-height: 36px;
}

.combobox-value-wrapper-hover {
    background-color: #dfebff;
}

.combobox-value-wrapper-selected .icon-active {
    display: block;
}

.icon-dropdown {
    border-radius: 0 4px 4px 0;
    width: 34px;
    height: 34px;
    position: relative;
}

.icon-dropdown::before {
    content: "";
    position: absolute;
    height: 10px;
    width: 10px;
    display: inline-block;
    background: transparent url(../../assets/icon/right-arrow.png);
    background-size: 10px 10px;
    transform: rotate(90deg);
    top: 13px;
    left: 13px;
    opacity: .8;
}

.icon-dropdown:hover {
    cursor: pointer;
}

.icon-active {
    background: transparent url(https://cegovapp.misacdn.net/r/cegov/img/sprites.06b14dc5.svg) no-repeat -192px -24px;
    height: 24px;
    width: 24px;
    position: relative;
    display: none;
}

.checkbox .checkbox-checkmark {
    position: absolute;
    top: 50%;
    transform: translateY(-50%);
    left: 2px;
    height: 16px;
    width: 16px;
    z-index: 1;
    background-position: 0px 0px;
    background-size: cover;
    background-repeat: no-repeat;
    background-image: url(https://cegovapp.misacdn.net/r/cegov/img/checkbox_grey.404fc483.svg);
}

.checkbox.check-temp .checkbox-checkmark {
    display: inline-block;
    background: url(https://cegovapp.misacdn.net/r/cegov/img/CeGo_Sprites.f3e906b1.svg);
    background-position: -544px -144px;
    width: 16px;
    height: 16px;
    position: absolute;
    top: 10px;
    left: 2px;
}

.checkbox:not(.check-temp) input:not(:checked):hover~.checkbox-checkmark,
.checkbox:not(.check-temp) input:not(:checked):focus~.checkbox-checkmark {
    top: 9px;
    left: 0px;
    width: 22px;
    height: 22px;
    background-image: url(https://cegovapp.misacdn.net/r/cegov/img/ic_checkbox_hover.d04300f4.svg);
}

.checkbox>.checkbox-input:checked~.checkbox-checkmark {
    background-image: url(https://cegovapp.misacdn.net/r/cegov/img/ic_checkbox_active.14fc601f.svg);
}

.checkbox-checkmark:after {
    content: "";
    position: absolute;
    display: none;
}

.checkbox .checkbox-input:checked~.checkbox-checkmark:after {
    display: block;
}

.checkbox .checkbox-input:checked:disabled~.checkbox-checkmark {
    width: 24px;
    height: 24px;
    background-image: url(https://cegovapp.misacdn.net/r/cegov/img/ic_checkbox_disabled_active.c6062b71.svg);
}

.checkbox .checkbox-input:disabled~.checkbox-checkmark {
    width: 24px;
    height: 24px;
    background-image: url(https://cegovapp.misacdn.net/r/cegov/img/ic_checkbox_disabled.7177f2a0.svg);
}

.select-all {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 36px !important;
    max-width: 36px !important;
    min-width: 36px !important;
}
</style>
  