<template>
  <div class="menu">
    <div class="navbar">
      <router-link :to="option.link" v-for="(option, index) in listOption" :key="index" 
      @click="optionIsActive = option.link">
        <div :class="['nav-option',{ active: option.link == optionIsActive }, option.icon]" 
        v-if="chekcRole(option.role)">
          <div class="nav-option-content">
            <div class="title-option">{{ option.content }}</div>
            <div v-show="isZoomOut" class="icon-arrow-right"></div>
            <div v-show="!isZoomOut" class="tooltip-icon">
              {{ option.content }}
            </div>
          </div>

        </div>

      </router-link>
    </div>
    <div class="menu-bottom" @click="zoomMenu">
      <span style="position: absolute;padding-left: 20px;" v-if="isZoomOut">Thu gọn</span>
    </div>
  </div>
</template>
<script>
import Const from '@/utils/const'
import Resource from '@/utils/resource'
export default {
  data() {
    return {
      Resource: Resource,
      Const: Const,
      isZoomOut: true, // cờ điều khiển trạng thái của menu

      optionIsActive: null, // biến thể hiện option nào của menu đang được chọn

      listOption: Const.ListMenu // dang sách các option
        
    };
  },
  methods: {
    /**
     * Hàm thu phóng Menu
     * Author: HAQuan(27/09/2022)
     */
    zoomMenu() {
      this.isZoomOut = !this.isZoomOut;
      this.$emit("zoomMenu");
    },

    chekcRole(roles){
      if(this.$store.getters.user != null){
        return roles.some((obj) => obj == this.$store.getters.user.role);
      }
      else {
        return false;
      }
    }
  },
  watch: {
        '$route.fullPath': {
            handler: function (newVal) {
                if (newVal != undefined) {
                    this.optionIsActive = newVal;
                }
            }
        },
        
    },
 
}
</script>

<style scoped>
.menu {
  background-color: #fff;
  color: black;
  transition: width .2s;
  box-shadow: inset 0 1.5px 2px 0 rgb(0 0 0 / 10%);
  box-sizing: border-box;
  position: relative;
}

.navbar {
  margin-top: 8px;
}

.menu-bottom {
  position: absolute;
  bottom: 18px;
  margin: 0 8px;
  width: 92%;
  border-radius: 8px;
  height: 40px;
  line-height: 40px;
  background: #edf1f5;
  cursor: pointer;
}

.zoom-in .menu-bottom {
  width: 75%;
}

.menu-bottom::before {
  content: "";
  display: inline-block;
  margin-top: 8px;
  margin-left: 16px;
  left: 0;
  height: 24px;
  width: 24px;
  background: url(../../assets/icon/double-left-arrow.png) ;
  background-size: 24px 24px;
  transform: rotate(0deg);
  transition-duration: .4s;
}


.zoom-in .menu-bottom::before {
  transform: rotate(180deg) !important;
  margin-left: 10px;
}

.menu-top,
.brand {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.nav-option {
  font-weight: 600;
  display: block;
  color: #000;
  position: relative;
  padding: 12px 0 12px 44px;
  margin: 8px;
  cursor: pointer;
  min-height: 43px;
}

.nav-option::before {
  content: "";
  display: block;
  height: 24px;
  width: 24px;
  position: absolute;
  top: 8px;
  left: 12px;
  background-repeat: no-repeat;
  background-color: transparent;
}

.zoom-in .nav-option {
  padding: 0;
  font-size: 0;
  border: none;
  height: 40px;
  margin: 4px 8px 8px;
  border-radius: 6px;
  color: transparent;
  width: 40px;
}

.zoom-in .nav-option::before {
  top: 8px;
  left: 8px;
}

/* .nav-option:not(:first-child) {
  margin-top: 0px;
} */

.nav-option.active {
  color: #ff6d00;
  background: #fbe9e7;
  border-radius: 8px;
}


.icon-arrow-right {
  background: url(../../assets/icon/right-arrow.png);
  background-size: 12px 12px;
  height: 12px;
  width: 12px;
  position: absolute;
  right: 6px;
  top: 17px;
}

.nav-option:hover {
  background: #eff7ff;
  color: #ff6d00;
  border-radius: 8px;
  cursor: pointer;
}

.nav-option:hover .icon-arrow-right {
  background: url(../../assets/icon/arrow.png);
  background-size: 12px 12px;
  height: 12px;
  width: 12px;
  position: absolute;
  right: 6px;
  top: 17px;
}

.icon-news::before{
  background: url(../../assets/icon/icon-news.png);
  background-size: 24px 24px;
  height: 24px;
  width: 24px;
  position: absolute;
  left: 4px;
  top: 8px;
}

.icon-product::before{
  background: url(../../assets/icon/icon-product.png);
  background-size: 24px 24px;
  height: 24px;
  width: 24px;
  position: absolute;
  left: 4px;
  top: 8px;
}
.icon-order::before{
  background: url(../../assets/icon/icon-order.png);
  background-size: 24px 24px;
  height: 24px;
  width: 24px;
  position: absolute;
  left: 4px;
  top: 8px;
}
.tooltip-icon {
  display: inline-block;
  position: fixed;
  background-color: #fff;
  color: #182d4b;
  border: 1px solid hsla(0, 0%, 62%, .30980392156862746);
  padding: 8px 15px;
  border-radius: 8px;
  margin-top: 0;
  left: 60px;
  visibility: hidden;
  font-size: 13px;
  letter-spacing: .5px;
  box-shadow: 0 1.5px 2px 0 rgb(0 0 0 / 10%);
}

.tooltip-icon::before {
  content: "";
  display: block;
  position: absolute;
  left: -4px;
  top: 10px;
  transform: rotate(45deg);
  width: 10px;
  height: 10px;
  background-color: inherit;
  border-left: 1px solid hsla(0, 0%, 62%, .30980392156862746);
  border-bottom: 1px solid hsla(0, 0%, 62%, .30980392156862746);
}

.nav-option:hover .tooltip-icon{
  visibility: visible;
}
</style>


