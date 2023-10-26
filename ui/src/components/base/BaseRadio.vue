<template>
  <div class="radio">
    <input @change="check" @keydown="handleKeyCommand" :tabindex="tabindex" type="radio" class="radio-input" :checked="isChecked" />
    <slot name="radiomark"></slot>
    <label v-if="label" :tooltip="tooltip" class="radio-label">{{ label }} </label>
  </div>
</template>
<script>
import Enum from '@/utils/enum'
export default {
  props: {
    label: { // nội dung, tiêu đề của radio
      type: String
    },

    tooltip: { // tooltip
      type: String,
      default: null,
    },

    checkedProp: { // trạng thái của radio được truyền 
      type: Boolean
    },
    
    tabindex: { // tabindex của radio
      type: String
    },

  },

  data() {
    return {
      isChecked: false, // biến điều khiển trạng thái của radio
    };
  },

  created() {
    this.isChecked = this.checkedProp;
  },

  watch: {
    isChecked(newValue) {
      this.$emit("getValue", newValue);
    },
    checkedProp(newValue) {
      this.isChecked = newValue;
    },
  },

  methods: {
    /**
     * Xử lý khi dùng bàn phím
     * Author: HAQUAN(28/10/2022)
     */
    handleKeyCommand() {
      let keyCode = event.keyCode;
      if (keyCode === Enum.KeyCode.Enter) {
        this.check();
      }
    },

    /**
     * Xử lý khi trạng thái của checkbox thay đổi
     * Author: HAQUAN(28/10/2022)
     */
    check() {
      this.isChecked = !this.isChecked;
      this.$emit("check");
    },
  },
};
</script>
<style scoped>
.radio {
    display: inline-block;
    position: relative;
    padding-left: 20px;
    min-height: 20px;
  }
  .radio-input {
    position: absolute;
    opacity: 0;
    cursor: pointer;
    height: 24px;
    width: 24px;
    cursor: pointer;
    top: -2px;
    z-index: 2;
    left: 2px;
  }
  
  /* .radio .radio-input:checked:disabled ~ .radio-checkmark::after {
    background-image: url(../../assets/icons/ic_Checkbox_Active_Disable.png);
  }
  .radio .radio-checkmark:after {
    background-image: url(../../assets/icons/ic_Checkbox_Inactive.png);
  }
  .radio .radio-input:disabled ~ .radio-checkmark::after {
    background-image: url(../../assets/icons/ic_Checkbox_Inactive_Disable.png);
  } */
  .radio-label {
    padding-right: 16px;
    padding-left: 12px;
  }
  
  
</style>
  