<template>
  <div>
    <form @keydown.esc="closeForm" class="modal-detail-wrapper" tabindex="0" @keydown.ctrl.s.prevent.exact="save(false)"
      @keydown.ctrl.shift.s.prevent.exact="save(true)">

      <div class="modal-detail">

        <div class="buttons-header">

          <el-tooltip effect="dark" :content="Resource.Tooltip.Help" placement="top">
            <div class="button help"></div>
          </el-tooltip>

          <el-tooltip effect="dark" :content="Resource.Tooltip.Close" placement="top">
            <div @mousedown="closeForm" class="button close"></div>
          </el-tooltip>

        </div>

        <div class="modal-detail-title">{{ isTitle }}</div>

        <div class="modal-detail-content">
          <div class="w-full d-flex">
            <div class="flex flex-column mr-10">
              <div class="row-form flex">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.FullName }}
                  <span class="required">*</span>
                </label>

                <div class="flex text-field-form">
                  <InputString :transmissionString="accountModel.account.fullName" :hasError="errors.fullName != ''"
                    :placeholder="Resource.Placehoder.FullName" maxlength="100" tabindex="1"
                    :nameProperty="Resource.AccountProperty.FullName" @updateValue="updateValue"
                    @eventBlur="focusFullName = false; validateFullName();" :isRef="focusFullName" />

                  <div class="error-text">{{ errors.fullName }}</div>
                </div>
              </div>
              <div class="row-form flex">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.Username }}
                  <span class="required">*</span>
                </label>

                <div class="flex text-field-form">
                  <InputString :transmissionString="accountModel.account.username" :hasError="errors.username != ''"
                    :placeholder="Resource.Placehoder.Username" maxlength="50" tabindex="2"
                    :disabled="formMode == Enum.Mode.Edit" :nameProperty="Resource.AccountProperty.Username"
                    @updateValue="updateValue" @eventBlur="focusUsername = false; validateUsername();"
                    :isRef="focusUsername" />

                  <div class="error-text">{{ errors.username }}</div>
                </div>
              </div>
              <div class="row-form flex" v-if="formMode == Enum.Mode.Add">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.Password }}
                  <span class="required">*</span>
                </label>

                <div class="flex text-field-form">
                  <!-- <InputString :transmissionString="accountModel.account.password" :hasError="errors.password != ''"
                    :placeholder="Resource.Placehoder.Password" maxlength="255" tabindex="3"
                    :nameProperty="Resource.AccountProperty.Password" @updateValue="updateValue"
                    @eventBlur="focusPassword = false; validatePassword();" :isRef="focusPassword" /> -->

                  <InputPassword :transmissionString="accountModel.account.password" :hasError="errors.password != ''"
                    :placeholder="Resource.Placehoder.Password" maxlength="255" tabindex="3"
                    :nameProperty="Resource.AccountProperty.Password" @updateValue="updateValue"
                    @eventBlur="focusPassword = false; validatePassword();" :isFocus="focusPassword" />

                  <div class="error-text">{{ errors.password }}</div>
                </div>
              </div>

              <div class="row-form flex" v-if="formMode == Enum.Mode.Add">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.VerifyPassword }}
                  <span class="required">*</span>
                </label>

                <div class="flex text-field-form">
                  <!-- <InputString :transmissionString="verifyPassword" :hasError="errors.verifyPassword != ''"
                    :placeholder="Resource.Placehoder.VerifyPassword" maxlength="255" tabindex="4"
                    :nameProperty="Resource.AccountProperty.VerifyPassword" @updateValue="updateValue"
                    @eventBlur="focusVerifyPassword = false; validateVerifyPassword();" :isRef="focusVerifyPassword" /> -->

                  <InputPassword :transmissionString="accountModel.account.verifyPassword"
                    :hasError="errors.verifyPassword != ''" :placeholder="Resource.Placehoder.VerifyPassword"
                    maxlength="255" tabindex="4" :nameProperty="Resource.AccountProperty.VerifyPassword"
                    @updateValue="updateValue" @eventBlur="focusVerifyPassword = false; validateVerifyPassword();"
                    :isFocus="focusVerifyPassword" />
                  <div class="error-text">{{ errors.verifyPassword }}</div>
                </div>
              </div>

              <div class="row-form flex">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.Email }}
                  <span class="required">*</span>
                </label>

                <div class="flex text-field-form">
                  <InputString :transmissionString="accountModel.account.email" :hasError="errors.email != ''"
                    :placeholder="Resource.Placehoder.Email" maxlength="150" tabindex="5"
                    :disabled="formMode == Enum.Mode.Edit" :nameProperty="Resource.AccountProperty.Email"
                    @updateValue="updateValue" @eventBlur="focusEmail = false; validateEmail();" :isRef="focusEmail" />

                  <div class="error-text">{{ errors.email }}</div>
                </div>
              </div>

              <div class="row-form flex">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.Role }}
                  <span class="required">*</span>
                </label>

                <div class="flex combobox-form">
                  <BaseCombobox tabindex="8" :hasIcon="true" :isReadonly="true" :items="Const.Role"
                    :initItem="configRole(accountModel.account.role)" fieldName="Label" @getValue="setRole" />

                </div>
              </div>

              <div class="row-form flex" v-if="formMode == Enum.Mode.Edit">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.Status }}
                </label>

                <div class="flex combobox-form">
                  <BaseCombobox tabindex="9" :hasIcon="true" :isReadonly="true" :items="Const.Status"
                    :initItem="configStatus(accountModel.account.status)" fieldName="Label" @getValue="setStatus" />

                </div>
              </div>


              <div class="row-form flex">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.Phone }}
                </label>

                <div class="flex text-field-form">
                  <InputString :transmissionString="accountModel.account.phone" :hasError="errors.phone != ''"
                    :placeholder="Resource.Placehoder.Phone" maxlength="20" tabindex="6"
                    :nameProperty="Resource.AccountProperty.Phone" @updateValue="updateValue"
                    @eventBlur="focusPhone = false; validatePhone();" :isRef="focusPhone" />

                  <div class="error-text">{{ errors.phone }}</div>
                </div>
              </div>

              <div class="row-form flex">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.Address }}
                </label>

                <div class="flex text-field-form">
                  <InputString :transmissionString="accountModel.account.address"
                    :placeholder="Resource.Placehoder.Address" maxlength="255" tabindex="7"
                    :nameProperty="Resource.AccountProperty.Address" @updateValue="updateValue" />
                </div>
              </div>
            </div>
            <div class="flex mr-10">
              <div class="image d-flex justify-content">
                <img :src="url" ref="avt" alt="">
              </div>
              <div class="action d-flex justify-content">
                <input class="input-file" type="file" ref="avatar" accept="image/*" @change="changeAvatar">
                <BaseButton @click="cancelUploadAvatar" class="sub-button btn ml-10" v-show="accountModel.image">
                  {{ Resource.Button.Cancel }}
                </BaseButton>
              </div>
            </div>

          </div>
        </div>
        <div class="modal-footer">
          <div class="flex-row flex-end">

            <BaseButton tabindex="11" @keydown.tab.prevent.exact="this.focusFullName = true;" @mousedown="closeForm"
              @keydown.enter="closeForm" class="sub-button btn">
              {{ Resource.Button.Cancel }}
            </BaseButton>

            <el-tooltip effect="dark" :content="Const.KeyStrokes.CtrlShiftS" placement="top">
              <BaseButton tabindex="10" @click="save(true)" @keydown.enter="save(true)" v-show="!checkForm()"
                class="button-blue btn ml-10">{{ Resource.Button.SaveAndNew }}
              </BaseButton>
            </el-tooltip>

            <el-tooltip effect="dark" :content="Const.KeyStrokes.CtrlS" placement="top">
              <BaseButton tabindex="9" @click="save(false)" @keydown.enter="save(false)" v-show="!checkForm()"
                class="main-button btn ml-10">{{ Resource.Button.Save }}</BaseButton>
            </el-tooltip>

            <el-tooltip effect="dark" :content="Const.KeyStrokes.CtrlS" placement="top">
              <BaseButton tabindex="9" @click="save(false)" @keydown.enter="save(false)" v-show="checkForm()"
                style="padding: 0 16px;" class="main-button btn ml-10">{{ Resource.Button.SaveChange }}</BaseButton>
            </el-tooltip>

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
</template>
<script>
import Const from '@/utils/const'
import Enum from '@/utils/enum'
import Resource from '@/utils/resource'
import BaseCombobox from "../components/base/BaseCombobox.vue";
import BaseButton from "../components/base/BaseButton.vue";
import BasePopup from "../components/base/BasePopup.vue";
import InputString from '@/components/base/BaseInputString.vue';
import InputPassword from '@/components/base/BaseInputPassword.vue';
import axios from 'axios';
export default {
  components: {
    BaseCombobox, BaseButton, BasePopup, InputString, InputPassword
  },
  props: ["closeFormDetail", "accountIdUpdate", "formMode"],
  data() {
    return {
      Enum: Enum,
      Resource: Resource,
      Const: Const,
      isTitle: "", // tiêu đề của form

      titlePopup: Resource.Title.Management, // tiêu đề của popup

      contentPopup: "", // nội dung cảnh báo

      isShowPopup: false, // cờ điền khiển đóng mở cảnh báo lỗi

      errors: { // danh sách lỗi validate của các trường thông tin
        fullName: "",
        username: "",
        password: "",
        verifyPassword: "",
        email: "",
        phone: "",
      },

      accountModelInit: { // thông tin tài khoản mặc định của form thêm mới
        account: {
          fullName: '', // họ tên người dùng
          username: '', // tên đăng nhập
          avatar: Resource.DefaultAvatar, //ảnh đại diện
          password: '', // mật khẩu
          email: '', // email
          phone: '', // số điện thoại
          address: '', // địa chỉ
          role: Enum.Role.Staff, // quyền hạn
        },
        image: null
      },

      accountModel: {}, // thông tin tài khoản trong form thêm mới

      verifyPassword: "",

      errorDuplicateUsername: false, // Cờ thể hiện xem có đang bị trùng tên đăng nhập hay không

      focusFullName: false,
      focusUsername: false,
      focusPassword: false,
      focusVerifyPassword: false,
      focusEmail: false,
      focusPhone: false,

      url: '',
    };
  },

  methods: {
    changeAvatar() {
      this.url = URL.createObjectURL(this.$refs.avatar.files[0]);
      this.accountModel.image = this.$refs.avatar.files[0];
      console.log(this.accountModel.image);
    },

    cancelUploadAvatar() {
      this.url = Resource.PrefixImage + this.accountModel.account.avatar;
      this.accountModel.image = null;
    },

    updateValue(value, nameProperty) {
      if (nameProperty == Resource.AccountProperty.VerifyPassword) {
        this.verifyPassword = value;
      }
      else {
        this.accountModel.account[nameProperty] = value;
      }
    },

    setStatus(value) {
      Const.Status.find((item) => {
        if (item.Label == value) {
          this.accountModel.account.status = item.Value;
        }
      });
    },

    configStatus(value) {
      let result = value;
      Const.Status.find((item) => {
        if (item.Value === value) {
          result = item.Label;
        }
      });
      return result;

    },

    setRole(value) {
      Const.Role.find((item) => {
        if (item.Label == value) {
          this.accountModel.account.role = item.Value;
        }
      });
    },

    configRole(value) {
      let result = value;
      Const.Role.find((item) => {
        if (item.Value === value) {
          result = item.Label;
        }
      });
      return result;

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
     * Lưu thông tin tài khoản
     * @param {*} control: kiểu lưu tài khoản, nếu false thì lưu và đóng form, còn true thì lưu và reset dữ liệu, không đóng form
     *  Author: HAQUAN(29/08/2023)  
     */
    save(control) {
      try {
        debugger
        let valid = this.validate();
        if (valid) {
          // Nếu như là chỉnh sửa thông tin tài khoản
          if (this.formMode == Enum.Mode.Edit) {
            this.sendRequestUpdate();
          }

          else {
            this.sendRequestInsert(control);
          }

        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Gửi request chỉnh sửa thông tin tài khoản đến server
     *  Author: HAQUAN(29/08/2023) 
     */
    async sendRequestUpdate() {
      try {
        this.accountModel.account.modifiedBy = this.$store.getters.user.accountID;

        await axios.put("Account/" + this.accountModel.account.accountID,
          { account: this.accountModel.account, image: this.accountModel.image }, {
          headers: {
            'Content-Type': 'multipart/form-data'
          },
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

    /**
     * Gửi request thêm mới tài khoản
     * @param {*} control: kiểu lưu tài khoản, nếu false thì lưu và đóng form, còn true thì lưu và reset dữ liệu, không đóng form
     *  Author: HAQUAN(29/08/2023) 
     */
    async sendRequestInsert(control) {
      try {
        await axios.post('Account/', { account: this.accountModel.account, image: this.accountModel.image }, {
          headers: {
            'Content-Type': 'multipart/form-data'
          },
        })
          .then((response) => {
            console.log(response);

            // Hiển thị toast thông báo thành công
            this.$emit("showToast", Resource.Message.AddAccountSucces, Const.TypeToast.Success);
            this.$emit("refreshData");
            if (!control) {
              this.closeFormDetail();
            }
            else {
              this.accountModel.account = { ...this.accountModelInit };
              this.focusProductName = true;
            }
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
    handleErrorResponse(error) {
      try {
        this.errorDuplicateUsername = false;

        // Nếu lỗi trả về là trùng tên tài khoản
        if (error.response.data == Resource.Error.UsernameExists) {
          this.errorDuplicateUsername = true;
        }

        this.contentPopup = error.response.data.userMsg;
        this.titlePopup = Resource.Title.Management;
        this.isShowPopup = true;

      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Validate không để trống tên người dùng
     * Author: HAQUAN(29/08/2023) 
     */
    validateFullName() {
      try {
        if (!this.accountModel.account.fullName) {
          this.errors.fullName = Resource.Error.FullName;
          return false;
        }
        this.errors.fullName = "";
        return true;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Validate không để trống tên đăng nhập
     * Author: HAQUAN(29/08/2023) 
     */
    validateUsername() {
      try {
        if (!this.accountModel.account.username) {
          this.errors.username = Resource.Error.UserName;
          return false;
        }
        this.errors.username = "";
        return true;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Validate password
     * Author: HAQUAN(29/08/2023) 
     */
    validatePassword() {
      try {
        var validPassword = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z]{8,}$/;

        if (!this.accountModel.account.password) {
          this.errors.password = Resource.Error.Password;
          return false;
        }
        if (!new RegExp(validPassword).test(this.accountModel.account.password)) {
          this.errors.password = Resource.Error.InvalidPassword;
          return false;
        }
        this.errors.password = "";
        return true;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Xác thực mật khẩu
     * Author: HAQUAN(29/08/2023) 
     */
    validateVerifyPassword() {
      try {
        if (this.accountModel.account.password != this.verifyPassword) {
          this.errors.verifyPassword = Resource.Error.VerifyPassword;
          return false;
        }
        this.errors.verifyPassword = "";
        return true;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Validate email
     * Author: HAQUAN(29/08/2023) 
     */
    validateEmail() {
      try {
        // eslint-disable-next-line
        var validEmail = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/;
        if (!this.accountModel.account.email) {
          this.errors.email = Resource.Error.Email;
          return false;
        }
        if (!new RegExp(validEmail).test(this.accountModel.account.email)) {
          this.errors.email = "Email không hợp lệ!";
          return false;
        }
        this.errors.email = "";
        return true;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Validate phone
     * Author: HAQUAN(29/08/2023) 
     */
    validatePhone() {
      try {
        var validPhone = /(84|0[3|5|7|8|9])+([0-9]{8})\b/;
        if (this.accountModel.account.phone && !new RegExp(validPhone).test(this.accountModel.account.phone)) {
          this.errors.phone = Resource.Error.Phone;
          return false;
        }
        this.errors.phone = "";
        return true;
      } catch (error) {
        console.log(error);
      }
    },


    /**
     * Validate thông tin tài khoản trước khi lưu
     * Author: HAQUAN(29/08/2023)  
     */
    validate() {
      try {
        let valid = true;
        debugger
        if (!this.validatePhone()) {
          this.focusPhone = true;
          valid = false;
        }

        if (!this.validateEmail()) {
          this.focusEmail = true;
          valid = false;
        }

        if (!this.validateVerifyPassword() && this.formMode == Enum.Mode.Add) {
          this.focusVerifyPassword = true;
          valid = false;
        }

        if (!this.validatePassword() && this.formMode == Enum.Mode.Add) {
          this.focusPassword = true;
          valid = false;
        }

        if (!this.validateUsername()) {
          this.focusUsername = true;
          valid = false;
        }

        if (!this.validateFullName()) {
          this.focusFullName = true;
          valid = false;
        }

        return valid;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Lấy thông tin tài khoản theo ID
     * @param {*} accountID : ID của tài khoản muốn lấy
     * Author: HAQUAN(29/08/2023) 
     */
    async getAccountByID(accountID) {
      try {
        this.isLoading = true;
        let url = `Account/${accountID}`;
        await axios.get(url)
          .then((response) => {
            this.accountModel.account = response.data;
            this.url = Resource.PrefixImage + this.accountModel.account.avatar;
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

      if (this.errorDuplicateUsername) {
        this.errors.username = Resource.Error.UsernameExists;
        this.focusUsername = true;
      }
    },

  },
  created() {
    this.accountModelInit.account.createdBy = this.$store.getters.user.accountID;
    this.accountModel = { ...this.accountModelInit };
    this.url = Resource.PrefixImage + this.accountModel.account.avatar;
    if (this.formMode == Enum.Mode.Edit) {
      this.isTitle = Resource.Title.EditAccount;
      this.getAccountByID(this.accountIdUpdate);
    }
  },
  mounted() {
    if (this.formMode == Enum.Mode.Add) {
      this.isTitle = Resource.Title.AddAccount;
    }
    // focus vào ô input tên người dùng
    this.focusFullName = true;

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
  width: 1024px;
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
  padding: 24px 24px 0;
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

.radio-checkmark {
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  left: 2px;
  height: 24px;
  width: 24px;
  z-index: 1;
  background-position: 0px 0px;
  background-size: cover;
  background-repeat: no-repeat;
  background-image: url(https://cegovapp.misacdn.net/r/cegov/img/ic_radio_inactive.478d13c5.svg);
}

.radio:hover>.radio-checkmark {
  background-image: url(https://cegovapp.misacdn.net/r/cegov/img/ic_radio_hover.9f996444.svg);
}

.radio>.radio-input:checked~.radio-checkmark {
  background-image: url(https://cegovapp.misacdn.net/r/cegov/img/ic_radio_active.68000fa3.svg);
}

.radio-checkmark:after {
  content: "";
  position: absolute;
  display: none;
}

.radio .radio-input:checked~.radio-checkmark:after {
  display: block;
}

.radio .radio-input:checked:disabled~.radio-checkmark {
  background-color: #f5f5f5;
  border-color: #cccccc;
}

input.input-file {
  margin-left: 10px;
  height: 36px;
  background-color: #4d8cf2;
  padding: 8px;
  color: #fff;
  border: 2px solid #4d8cf2;
  border-radius: 4px;
  cursor: pointer;
  max-width: 80px;
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
  margin-top: 50px;
}

img {
  width: 256px;
  height: 256px;
}
</style>
  