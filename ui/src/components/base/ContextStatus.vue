<template>
  <div id="context-menu" @click="close">
    <div class="context-menu-content" :style="{ top: positionTop + 'px', left: positionLeft + 'px' }">
      <div class="option" v-for="(item, index) in listStatus" :key="index" @click="updateStatus(item.Value)">
        <div class="text">{{ item.Label }}</div>
      </div>
    </div>
  </div>
</template>

<script>
import Const from '@/utils/const';
export default {
  props: {
    positionTop: { // vị trí tọa độ bên trên của form contextStatus
      type: Number,
      default: 0,
    },

    positionLeft: { // vị trí tọa độ bên trái của form contextStatus
      type: Number,
      default: 0,
    },
    statusInit: {
      type: Number
    }
  },
  data() {
    return {
      Const: Const,
      listStatus: []
    }
  },
  methods: {
    /**
     * hàm đóng form
     * Author : HAQuan (15/11/2023)
     */
    close() {
      this.$emit("close");
    },

    /**
     * hàm xử lý sự kiện khi chọn trạng thái mới
     * Author : HAQuan (15/11/2023)
     */
    updateStatus(value) {
      this.$emit("updateStatus", value);
    },
  },
  created(){
    console.log(this.statusInit);
    this.listStatus = Const.StatusOrder.filter(
          (item) => item.Value != this.statusInit
        );
  },
};
</script>
<style scoped>
#context-menu {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  z-index: 20;
}

#context-menu .context-menu-content {
  width: 100%;
  max-width: 140px;
  height: fit-content;
  position: absolute;
  z-index: 22;
  background-color: #f4f7ff;
  display: block;
  padding: 5px;
  border-radius: 2.5px;
  box-shadow: 0 3px 6px rgba(0, 0, 0, .25);
  border: 1px solid #e0e0e0;
}

#context-menu .context-menu-content .option {
  display: flex;
  gap : 10px;
  align-items: center;
  height: 35px;
  border-radius: 2.5px;
  border: 1px solid #f9fafc;
  cursor: pointer;
  padding-left: 10px;
}

#context-menu .context-menu-content .option .icon {
  margin-left: 10px;
}

#context-menu .context-menu-content .optinon.active,
#context-menu .context-menu-content .option:hover {
  background-color: rgba(26, 164, 200, 0.2)
}

/* #context-menu .overplay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  z-index: 21;
} */

</style>
