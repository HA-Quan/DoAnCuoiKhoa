<template>
  <div class="popup-wrapper" ref="popup">
    <div class="popup">
      <div class="buttons-header">
        <div @click="handleClose" class="button close"></div>
      </div>
      <div class="top-popup">
          <div class="popup-title">{{ title }}</div>
          <div style="padding-right: 25px;" v-if="prefix==null && suffix==null">
            <p>
              {{ content }} 
            </p>
          </div>
          <div style="padding-right: 25px;" v-else>
            <p>
              {{ prefix}} <strong>{{ content }}</strong> {{ suffix }}
            </p>
          </div>
          
      </div>
      <div class="popup-controller">
        <slot name="buttons"></slot>
      </div>
    </div>
  </div>
</template>
<script>
import Resource from '@/utils/resource'
export default {
  data() {
    return {
      Resource: Resource,
    };
  },
  props: {
    title: { // tiêu đề của popup
      type: String,
    },

    content: { // nội dung popup
      type: String,
      default: null,
    },

    prefix: { // tiền tố popup
      type: String,
      default: null,
    },

    suffix: { // hậu tố popup
      type: String,
      default: null,
    },
  },

  methods: {
    /**
    * Hàm xử lý khi click nút close
    * Author: HAQUAN(28/09/2022)
    */
    handleClose() {
      this.$emit("close");
    },
  },
};
</script>
<style scoped>
.popup-wrapper {
  background-color: rgba(0, 0, 0, 0.25);
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  z-index: 15; 
}
.popup {
  position: relative;
  transition: all .2s;
  z-index: 100;
  width: 444px;
  box-shadow: 0 0 16px rgb(23 27 42 / 24%);
  background: #fff;
  animation: rebound-data-v-b33a4368 .3s;
  border-radius: 3px;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
}

.buttons-header {
  position: absolute;
  top: 16px;
  right: 16px;
}

.buttons-header .button {
  cursor: pointer;
  display: inline-block;
  /* padding: 10px 8px; */
}

.buttons-header .button::before {
  content: "";
  display: block;
  background: transparent url(https://cegovapp.misacdn.net/rc/cegov/img/sprites.06b14dc5.svg) no-repeat;
  height: 20px;
  width: 20px;
}

.buttons-header .button.close:before {
  background-position: -242px -26px;
}

.buttons-header .button.close:hover::before {
  background-position: -266px -26px;
}

.top-popup{
  padding: 20px 16px 16px 16px;
  min-height: 144px;
}

.popup-title {
  font-size: 20px;
  padding-bottom: 20px;
  font-weight: 700;
}

.popup-controller{
  position: relative;
  height: 60px;
  padding: 0 24px 24px 16px;
  display: flex;
  justify-content: flex-end;
}

</style>
