<template>
  <div @keydown="handleKeyCommand" class="checkbox" :class="disabled ? 'disabled' : ''">
    <input @change="check" :tabindex="tabindex" type="checkbox" class="checkbox-input" :checked="isChecked"
      :disabled="disabled" />
    <slot name="checkmark"></slot>
    <label v-if="label" :tooltip="tooltip" class="checkbox-label">{{ label }} </label>
  </div>
</template>
<script>
import Enum from '@/utils/enum'
export default {
  props: {
    label: { // nội dung, tiêu đề của checkbox
      type: String
    },

    tooltip: { // tooltip
      type: String,
      default: null,
    },

    checkedProp: { // trạng thái của checkbox được truyền 
      type: Boolean
    },

    disabled: { // trạng thái của checkbox 
      type: Boolean
    },

    tabindex: { // tabindex của checkbox
      type: Number,
      default: null,
    },

    notClick: {
      type: Boolean,
      default: false
    }

  },

  data() {
    return {
      isChecked: false, // biến điều khiển trạng thái của checkbox
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
      if (!this.notClick) {

        this.isChecked = !this.isChecked;
        this.$emit("check");
      }
    },
  },
};
</script>
<style scoped>
.checkbox {
  display: inline-block;
  position: relative;
  padding-left: 20px;
  min-height: 20px;
}

.checkbox-input {
  position: absolute;
  opacity: 0;
  cursor: pointer;
  height: 16px;
  width: 16px;
  cursor: pointer;
  top: 2px;
  z-index: 2;
  left: 2px;
}


.checkbox-label {
  padding-right: 16px;
  padding-left: 12px;
}
</style>
