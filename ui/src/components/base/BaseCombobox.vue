<template>
  <div @keydown="handleKeyCommand" class="combobox pointer" @focusin="isOpenCombobox = true;setIndexOption()" @focusout="focusOut">
    <div :class="['combobox-select flex-row', { 'border-red': this.isCheck == true }, { disabled: disabled }]">
      <div class="popover flex">
        <input class="input" type="text" @input="searchCombobox" :disabled="disabled" v-model="this.valueCombobox"
          :tabindex="tabindex" :placeholder="placehoder" :readonly="isReadonly" />
      </div>
      <div @click="toggleOptions" class="icon-dropdown"></div>
    </div>
    <div v-if="isOpenCombobox && this.listItem.length > 0 && !disabled" class="combobox-options pointer">
      <div class="combobox-values">
        <div ref="options" @mousedown="handleOptionClicked(option, index)" v-for="(option, index) in this.listItem"
          :key="index" :class="[  
            'combobox-value-wrapper',
            {
              'combobox-value-wrapper-selected':
                option[fieldName] == selectedOption,
              'combobox-value-wrapper-hover': hoveredOptionIndex === index,
            },
          ]">
          <span class="combobox-value">
            {{ option[fieldName] }}
          </span>
          <div v-show="this.hasIcon == true" class="icon-active"></div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import Enum from '@/utils/enum'
// import Resource from '@/utils/resource';
export default {
  props: {
    isReadonly: { type: Boolean, default: false }, // cờ thể hiện xem combobox có thể tìm kiếm được hay k?

    isCheck: { type: Boolean, default: false }, // cờ kiểm tra xem combobox có đang ở trạng thái lỗi hay không

    hasIcon: { type: Boolean, default: false }, // cờ kiểm tra xem combobox có icon hay không

    items: { //danh sách item trong combobox
      type: Array,
      default() {
        return [];
      }
    },

    initItem: { // item mặc định ban đầu
      type: String,
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
        this.valueCombobox = newVal;
        this.items.find(
          (item) => {
            if (item[this.fieldName] == newVal) {
              this.selectedOption = newVal;
              this.setIndexOption();
            }
          }
        );
      }
    },

    'valueCombobox': {
      handler: function (newVal) {
        if (newVal != undefined) {
          this.$emit("getValue", newVal);
        }
      }
    },
  },

  data() {
    return {
      isOpenCombobox: false, // cờ điều khiển đóng mở combobox

      valueCombobox: "", // giá trị combobox

      selectedOption: "", // giá trị combobox đang được chọn

      hoveredOptionIndex: 0, // vị trí phần tử đang được chọn

      listItem: [], // danh sách item trong combobox 
    };
  },

  created() {
    // xử lý sự kiện khi click chuột ra ngoài vùng combobox sẽ đóng combobox
    window.addEventListener('click', (e) => {
      if (!this.$el.contains(e.target)) {
        this.isOpenCombobox = false
      }
    })
    this.valueCombobox = this.initItem;
    this.selectedOption = this.initItem;
    this.listItem = this.items;
    this.setIndexOption();
  },

  methods: {

    /**
     * Xử lý khi người dùng focusout khỏi combobox
     * Author:HAQUAN(28/10/2022) 
     */
    focusOut() {
      this.isOpenCombobox = false;
      this.$emit("focusOut");
    },

    /**
     * Xử lý khi người dùng search các option của combobox
     * Author:HAQUAN(28/10/2022) 
     */
    searchCombobox() {
      this.isOpenCombobox = true;
      this.listItem = this.items.filter((item) => {
        if (item[this.fieldName].toLowerCase().includes(this.valueCombobox.toLowerCase())) {
          return item;
        }
      });

      this.setIndexOption();

      if (this.valueCombobox == "") {
        this.selectedOption = this.valueCombobox;
        this.hoveredOptionIndex = 0;
      }
    },

    /**
     * Xét index cho item đang được chọn trong combobox
     * Author:HAQUAN(28/10/2022) 
     */
    setIndexOption() {
      this.hoveredOptionIndex = this.listItem.findIndex(
        (item) => {
          return item[this.fieldName] == this.selectedOption;
        }
      );
      if(this.hoveredOptionIndex < 0 ){
        this.hoveredOptionIndex = 0;
      }
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
            this.handleOptionSelected(this.listItem[this.hoveredOptionIndex]);
          }
          this.isOpenCombobox = false;
          break;

        case Enum.KeyCode.ArrowDown:
          if (this.hoveredOptionIndex < this.listItem.length - 1) {
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
      this.handleOptionSelected(option);
      this.isOpenCombobox = false;
      this.hoveredOptionIndex = index;
    },

    /**
     * Xử lý khi option được chọn
     * Author: HAQUAN(28/10/2022) 
     */
    handleOptionSelected(option) {
      this.selectedOption = option[this.fieldName];
      this.valueCombobox = option[this.fieldName];
    },

    /**
     * Ẩn/hiện danh sách options
     * Author: HAQUAN(28/10/2022) 
     */
    toggleOptions() {
      this.isOpenCombobox = !this.isOpenCombobox;
      if (this.isOpenCombobox) {
        this.listItem = this.items;
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
      // me.$refs.options[this.hoveredOptionIndex].scrollIntoView({behavior: "smooth" , inline: "end", block: "end"});
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
  justify-content: space-between;
  display: flex;
  align-items: center;
  border-radius: 4px;
  padding: 0 8px;
  min-height: 36px;
}

.combobox-value-wrapper-hover {
  background-color: #dfebff;
}

/* .combobox-value-wrapper-selected {
  background-color: #dfebff;
} */

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
</style>
