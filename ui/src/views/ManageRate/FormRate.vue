<template>
  <div ref="myForm">
    <form @keydown.esc="closeForm" class="modal-detail-wrapper">

      <div class="modal-detail">

        <div class="buttons-header">
          <el-tooltip effect="dark" :content="Resource.Tooltip.Close" placement="top">
            <div @mousedown="closeForm" class="button close"></div>
          </el-tooltip>
        </div>

        <div class="modal-detail-title">{{ isTitle }}</div>

        <div class="modal-detail-content">
          <div class="w-full flex-column">

            <div class="row-form flex">
              <div style="display: flex;justify-content: center;">
                <q-rating v-model="rate.numStar" max="5" size="5em" color="yellow" icon="star_border" icon-selected="star"
                  :readonly="formMode == Enum.Mode.Edit && rate.commentReply" />
              </div>
            </div>

            <div class="row-form">

              <label for="" class="label-form d-flex">
                {{ Resource.Label.RateComment }}
              </label>

              <div class="flex text-aria-form">

                <div class="flex-row border">
                  <textarea :placeholder="Resource.Placehoder.RateComment" class="aria-form flex" rows="4" v-model.trim="rate.comment"
                    :disabled="(role != Enum.Role.Member && formMode == Enum.Mode.Edit && rate.commentReply) || (role != Enum.Role.Member)"></textarea>
                </div>
              </div>
            </div>

            <div class="row-form d-flex">
              <div class="flex block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.AddImageDescribe }}
                </label>
                <div class="cmt-img d-flex">
                  <div class="add-img mr-10">
                    <label for="imageCmt">
                      <div class="icon-add"></div>
                    </label>
                    <input class="input-file" type="file" multiple id="imageCmt" ref="image" accept="image/*"
                      @change="handleUploadImage">
                  </div>
                  <div class="list-image">
                    <a data-fancybox="gallery" style="position: relative;" class="mr-10" :href="item"
                      v-for="(item, index) in listUrlImage" :key="index">
                      <img :src="item" alt="">
                    </a>
                  </div>

                </div>
              </div>

            </div>

            <div class="row-form" v-if="(role == Enum.Role.Member && rate.commentReply) || (role != Enum.Role.Member)">

              <label for="" class="label-form d-flex">
                {{ Resource.Label.ReplyComment }}
              </label>

              <div class="flex text-aria-form">

                <div class="flex-row border">
                  <textarea :placeholder="Resource.Placehoder.ReplyComment" class="aria-form flex" rows="4"
                    v-model.trim="rate.commentReply" :disabled="role == Enum.Role.Member">
                  </textarea>
                </div>
              </div>
            </div>

          </div>
        </div>
        <div class="modal-footer">
          <div class="flex-row flex-end">

            <BaseButton @mousedown="closeForm" class="sub-button btn">
              {{ Resource.Button.Cancel }}
            </BaseButton>

            <BaseButton @click="save()" v-if="!checkForm()" class="button-blue btn ml-10">{{ Resource.Button.SendRate }}
            </BaseButton>
            <BaseButton @click="save()" v-else-if="checkForm() && !rate.commentReply" class="button-blue btn ml-10">{{
              Resource.Button.UpdateRate }}
            </BaseButton>

          </div>
        </div>
      </div>
    </form>
  </div>

  <BasePopup @close="closePopup" class="popup-delete" :title="this.titlePopup" :content="contentPopup" v-if="isShowPopup">

    <template #buttons>
      <BaseButton @click="closePopup" class="main-button-red btn ml-10">{{ Resource.Button.Close }}</BaseButton>
    </template>

  </BasePopup>

  <BaseToastMessage v-show="toastMessage.isShowed" :class="`toast-${toastMessage.type} icon-toast-${toastMessage.type}`">

    <template #message>{{ toastMessage.message }}</template>
  </BaseToastMessage>
</template>
<script>
import { Fancybox } from "@fancyapps/ui";
import "@fancyapps/ui/dist/fancybox/fancybox.css";
import Const from '@/utils/const'
import Enum from '@/utils/enum'
import Resource from '@/utils/resource'
import BaseButton from "../../components/base/BaseButton.vue";
import BasePopup from "../../components/base/BasePopup.vue";
import BaseToastMessage from '@/components/base/BaseToastMessage.vue';
import axios from 'axios';
import CommonFn from '@/utils/commonFuncion';
export default {
  components: {
    BaseButton, BasePopup, BaseToastMessage
  },
  props: ["closeFormDetail", "rateIDInit", "accountID", "productID", "formMode"],
  data() {
    return {
      Enum: Enum,
      Resource: Resource,
      Const: Const,
      isTitle: Resource.Title.Rate, // tiêu đề của form
      titlePopup: Resource.Title.Notification, // tiêu đề của popup
      contentPopup: '', // nội dung cảnh báo

      isShowPopup: false, // cờ điền khiển đóng mở cảnh báo lỗi

      rateInit: { // thông tin đánh giá mặc định của form thêm mới
        productID: '',
        accountID: '',
        numStar: 5,
        comment: '',
      },

      rate: {}, // thông tin đánh giá trong form thêm mới

      listImage: null, // Danh sách ảnh đánh giá

      listUrlImage: [],

      toastMessage: { // cảnh báo
        message: '', // nội dung cảnh báo
        type: '', // loại cảnh báo
        isShowed: false, // cờ điều khiển bật tắt cảnh báo
      },
      role: Enum.Role.Member,
    };
  },

  methods: {

    handleUploadImage() {
      if (this.validateAvatarImage()) {
        this.listImage = this.$refs.image.files;
        for (var i = 0; i < this.listImage.length; i++) {
          this.listUrlImage.push(URL.createObjectURL(this.listImage[i]));
        }
      }

    },

    validateAvatarImage() {
      if (this.$refs.image.files.length > 8) {
        this.contentPopup = Resource.Error.Max8Image;
        this.isShowPopup = true;
        return false;
      }
      return true;
    },

    /**
     * Kiểm tra xem có phải form sửa hay không
     * Author: HAQUAN(29/08/2023) 
     */
    checkForm() {
      if (this.formMode === Enum.Mode.Edit) {
        return true;
      }
      return false;
    },

    /**
     * Đóng form 
     * Author: HAQUAN(29/08/2023) 
     */
    closeForm() {
      this.closeFormDetail();
    },

    /**
     * Lưu thông tin đánh giá
     *  Author: HAQUAN(29/08/2023)  
     */
    save() {
      try {
        // Nếu như là chỉnh sửa thông tin đánh giá
        if (this.formMode == Enum.Mode.Edit) {
          this.sendRequestUpdate();
        }
        else {
          this.sendRequestInsert();
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Gửi request chỉnh sửa thông tin đánh giá đến server
     *  Author: HAQUAN(29/08/2023) 
     */
    async sendRequestUpdate() {
      try {
        this.rate.modifiedBy = this.$store.getters.user.accountID;
        await axios.put("Rate/" + this.rate.rateID, {
          rate: this.rate, listImages: this.listImage
        }, {
          headers: {
            'Content-Type': 'multipart/form-data'
          }
        })
          .then((response) => {
            console.log(response);

            // Hiển thị toast thông báo thành công
            this.$emit("showToast", Resource.Message.UpdateProductSucces, Const.TypeToast.Success);
            this.closeFormDetail();
            this.$emit("refreshData");
          })

          .catch((error) => {
            console.log(error);
            this.handleErrorResponse(error);
          });
      } catch (error) {
        console.log(error);
      }
    },

    async sendRequestInsert() {
      try {

        await axios.post('Rate/', {
          rate: this.rate, listImages: this.listImage
        }, {
          headers: {
            'Content-Type': 'multipart/form-data'
          }
        }).then((response) => {
          console.log(response);

          // Hiển thị toast thông báo thành công
          this.$emit("showToast", Resource.Message.AddRateSucces, Const.TypeToast.Success);
          this.$emit("refreshData");
          this.closeFormDetail();

        })
          .catch((error) => {
            console.log(error);
            this.handleErrorResponse(error);
          });
      } catch (error) {
        console.log(error);
      }
    },

    /**
    * Xử lý các lỗi trả về từ server
    * @param {*} error Đối tượng lỗi
    * Author: HAQUAN(29/08/2023) 
    */
    async handleErrorResponse(error) {
      try {
        // Nếu lỗi trả về là lỗi đăng nhập (401)
        if (error.response.status == Enum.Error.Login) {
          var tokenMoel = {
            refreshToken: CommonFn.getCookie('RefreshToken'),
            accessToken: CommonFn.getCookie('Token')
          };
          await axios.post('Account/renewToken', tokenMoel).
            then((response) => {
              CommonFn.setCookie('RefreshToken', response.data.refreshToken, 60);
              CommonFn.setCookie('Token', response.data.accessToken, 60);
              this.$store.dispatch('setUser', response.data.infoUser);
              axios.defaults.headers.common["Authorization"] = "Bearer " + response.data.accessToken;
            }).
            catch((error) => {
              console.log(error);
              this.$store.dispatch('setUser', null);
              this.$router.push({ name: 'LoginForm' });

            });
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Lấy thông tin đánh giá theo ID
     * @param {*} rateID : ID của đánh giá muốn lấy
     * Author: HAQUAN(29/08/2023) 
     */
    async getRateByID(rateID) {
      try {
        this.isLoading = true;
        let url = `Rate/${rateID}`;
        await axios.get(url)
          .then((response) => {
            debugger
            this.rate = response.data;
            let listImgPrd = this.rate.listImg.split(",");
            listImgPrd.forEach(e => {
              this.listUrlImage.push((Resource.PrefixImage + e));
            });

          })

          .catch((error) => {
            console.log(error);
            this.handleErrorResponse(error);
          })

          .finally(() => {
            this.isLoading = false;
          })
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Đóng popup 
     *  Author: HAQUAN(29/08/2023) 
     */
    closePopup() {
      this.isShowPopup = false;
    },

    /**
    * Hiển thị toast message khi thực hiện thành công
    * @param {*} contentToast: nội dung thông báo 
    * Author: HAQUAN(28/10/2022)
    */
    showToast(contentToast, TypeToast) {
      this.toastMessage = {
        message: contentToast,
        type: TypeToast,
        isShowed: true,
      };
      setTimeout(() => {
        this.toastMessage.isShowed = false;
      }, 5000);
    },

  },
  created() {
    this.rate = { ...this.rateInit };
    this.rate.createdBy = this.$store.getters.user.accountID;
    this.role = this.$store.getters.user.role;
    Fancybox.bind("[data-fancybox]", {
      // Your custom options
    });
    if (this.formMode == Enum.Mode.Edit) {
      this.getRateByID(this.rateIDInit);
    }
  },
  mounted() {
    if (this.formMode == Enum.Mode.Add) {
      this.rate.productID = this.productID;
      this.rate.accountID = this.accountID;
    }
  },
};
</script>
<style scoped>
.modal-detail-wrapper {
  top: 0;
  left: 0;
  width: 100vw;
  height: 100vh;
  position: fixed;
  background-color: rgba(0, 0, 0, 0.25);
  z-index: 5;
}

.modal-detail {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 1040px;
  height: fit-content;
  border-radius: 4px;
  z-index: 5;
  background-color: #fff;
  box-shadow: 0 0 16px rgb(23 27 42 / 24%);
}

.modal-detail-content::-webkit-scrollbar {
  width: 8px;
  /* height: 8px; */
  border-radius: 10px;
  background: #fff;
}

.modal-detail-content::-webkit-scrollbar:hover {
  cursor: pointer;

}

.modal-detail-content::-webkit-scrollbar-thumb {
  border-radius: 10px;
  background-color: #b5b5b5;
}

.modal-detail::-webkit-scrollbar-thumb:hover {
  background-color: #8f8f8f;
}

.modal-detail ::placeholder {
  color: #bbbbbb;
}

.buttons-header {
  position: absolute;
  top: 14px;
  right: 16px;
}

.buttons-header .button {
  cursor: pointer;
  display: inline-block;
  padding: 10px 8px;
}

.buttons-header .button::before {
  content: "";
  display: block;
  background: transparent url(https://cegovapp.misacdn.net/rc/cegov/img/sprites.06b14dc5.svg) no-repeat;
  height: 20px;
  width: 20px;
}

.buttons-header .button.help:before {
  background-position: -50px -50px;
}

.buttons-header .button.close:before {
  background-position: -242px -26px;
}

.buttons-header .button.close:hover::before {
  background-position: -266px -26px;
}

.modal-detail-title {
  font-size: 20px;
  padding: 24px 0 6px 24px;
  font-weight: 700;
  min-height: 24px;
}

.modal-detail-content {
  padding: 0 24px;
  position: relative;
  max-height: 530px;
  overflow: auto;
  position: relative;
}

.w-full {
  width: 100%;
  height: 100%;
}

.row-form {
  margin-bottom: 16px;
}

.label-form {
  color: #000;
  font-size: 14px;
  font-weight: 400;
  height: 20px;
  margin-bottom: 8px;
  padding: 0;
  width: auto;
}

.required {
  color: #e54848;
  margin-left: 5px;
}

.text-field-form {
  background: #fff;
  padding: 0;
  width: 100%;
  border-radius: 3.5px;
}

.text-aria-form {
  background: #fff;
  align-items: center;
  border-radius: 3.5px;
}

.aria-form {
  border: none;
  padding: 6px 12px;
  resize: none;
  background: 0 0;
}

.border {
  border: 1px solid #e0e0e0;
  border-radius: 3.5px;
}

.border.disabled {
  background-color: #ebebeb !important;
  color: #666;
}

:focus-visible {
  outline: none;
}

.border:not(.disabled):not(.error):focus-within,
.border:not(.disabled):not(.error):hover {
  border: 1px solid #1a73e8;
}

.border.error {
  border: 1px solid #ef5350;
}

.error-text {
  color: #ef5350;
  margin-top: 6px;
}

.border.error:focus-within {
  border: 1px solid #ef5350;
}

.checkbox-form {
  height: 34px;
  margin-bottom: 9px;
}

.combobox-form {
  width: 100%;
  background-color: transparent;
}

.modal-footer {
  padding: 12px 24px;
  border-top: 1px solid #e0e0e0;
}

.w-140 {
  width: 140px !important;
  min-width: 140px !important;
}

.radio-form {
  min-height: 35px;
  padding: 0;
  position: relative;
  align-items: center;
}

.text-right {
  text-align: right;
}

.padding-r {
  padding-right: 33px;
}


input.input-file {
  /* margin-left: 10px;
  height: 36px;
  background-color: #4d8cf2;
  padding: 8px;
  color: #fff;
  border: 2px solid #4d8cf2;
  border-radius: 4px;
  cursor: pointer;
  max-width: 96px; */
  display: none;
  visibility: hidden;
}

input.input-file::before {
  content: 'Upload file';
}

input.input-file::-webkit-file-upload-button {
  visibility: hidden;
}

input.input-file+label {
  background-color: #4d8cf2;
  padding: 8px;
  color: #fff;
  border: 2px solid #4d8cf2;
  border-radius: 4px;
}

input.input-file+label:hover {
  background-color: #3b73ce;
  cursor: pointer;
}

input.input-file:hover {
  background-color: #3b73ce;
}

.content {
  width: 100%;
}

.action {
  display: flex;
  margin-top: 30px;
  justify-content: space-around;
}

img {
  width: 256px;
  height: 256px;
}

.cmt-img {
  border: 1px solid #e0e0e0;
  height: 110px;
  padding: 10px;
  background-color: #e8e8e8;
}

.add-img {
  background: #fff;
  width: 90px;
  border-radius: 10px;
}

.icon-add {
  height: 90px;
  width: 90px;
  background: transparent url(../../assets/icon/add-image.png) no-repeat;
  background-position: center;
  background-size: 64px 64px;
  cursor: pointer;
}

.list-image {
  display: flex;
}

.list-image>a>img {
  width: 90px;
  height: 90px;
  cursor: pointer;
}
</style>
