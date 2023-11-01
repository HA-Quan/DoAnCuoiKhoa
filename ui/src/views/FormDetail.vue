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
          <div class="w-full flex-column">

            <div class="row-form flex">
              <label for="" class="label-form d-flex">
                {{ Resource.Label.ProductName }}
                <span class="required">*</span>
              </label>

              <div class="flex text-field-form">
                <InputString :transmissionString="product.productName" :hasError="errors.productName != ''"
                  :placeholder="Resource.Placehoder.ProductName" maxlength="255" tabindex="1"
                  :nameProperty="Resource.ProductProperty.ProductName" @updateValue="updateValue"
                  @eventBlur="focusProductName = false; validateRequiredName();" :isRef="focusProductName" />

                <div class="error-text">{{ errors.productName }}</div>
              </div>
            </div>

            <div class="row-form d-flex">
              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.Avatar }}
                </label>
                <div class="d-flex text-field-form space-between">
                  <div class="content">
                    <InputString :transmissionString="nameFileAvatar" :isReadonly="true"
                      :hasError="errors.avatarProduct != ''" />
                  </div>
                  <div class="action">
                    <input class="input-file" type="file" ref="avatar" accept=".jpg" @change="handleUploadAvatar"
                      tabindex="2">
                    <BaseButton @click="seeAvatar" @keydown.enter="seeAvatar" class="main-button btn ml-10">{{
                      Resource.Button.See }}</BaseButton>
                  </div>

                </div>
                <div class="error-text">{{ errors.avatarProduct }}</div>
              </div>
              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.ImageDetail }}
                </label>
                <div class="d-flex text-field-form space-between">
                  <div class="content">
                    <InputString :transmissionString="listImageTxt" :isReadonly="true" />
                  </div>
                  <div class="action">
                    <input class="input-file" multiple type="file" ref="imageDetail" accept=".jpg"
                      @change="handleUploadImageDetail" tabindex="3">
                    <BaseButton @click="seeImageDetail" @keydown.enter="seeImageDetail" class="main-button btn ml-10">{{
                      Resource.Button.See }}</BaseButton>
                  </div>
                </div>
              </div>
            </div>
            <div class="row-form d-flex">
              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.Category }}
                  <span class="required">*</span>
                </label>

                <div class="flex combobox-form">
                  <BaseCombobox tabindex="4" :isCheck="errors.category != '' ? true : false" :hasIcon="true"
                    :items="listCategory" :initItem="product.categoryName" fieldName="categoryName"
                    @focusOut="validateCategory" @getValue="setCategory" :disabled="formMode == Enum.Mode.Edit"
                    :placehoder="Resource.Placehoder.Category" />

                </div>
                <div class="error-text">{{ errors.category }}</div>
              </div>
              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.Demand }}
                  <span class="required">*</span>
                </label>

                <div class="flex combobox-form">
                  <ComboboxCheckbox tabindex="5" :isCheck="errors.demand != '' ? true : false" :items="Const.DemandType"
                    :initItem="configDemandType(product.demandTypeString)" fieldName="Label" @getValue="setDemandType"
                    :placehoder="Resource.Placehoder.DemandType" :isReadonly="true"
                    :disabled="formMode == Enum.Mode.Edit" />

                </div>
                <div class="error-text">{{ errors.demand }}</div>
              </div>
            </div>

            <div class="row-form d-flex">
              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.ChipType }}
                  <span class="required">*</span>
                </label>

                <div class="flex combobox-form">
                  <BaseCombobox tabindex="6" :isCheck="errors.chipType != '' ? true : false" :hasIcon="true"
                    :items="listChip" :initItem="configChip(product.chipID)" fieldName="chipType" @getValue="setChip"
                    :disabled="formMode == Enum.Mode.Edit" :placehoder="Resource.Placehoder.Chip"
                    @focusOut="validateChipType" />

                </div>
                <div class="error-text">{{ errors.chipType }}</div>
              </div>
              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.ChipDetail }}
                  <span class="required">*</span>
                </label>

                <div class="flex text-field-form">
                  <InputString :transmissionString="product.chipDetail" :hasError="errors.chipDetail != ''"
                    :placeholder="Resource.Placehoder.ChipDetail" maxlength="100" tabindex="7"
                    @eventBlur="focusChipDetail = false; validateChipDetail();"
                    :nameProperty="Resource.ProductProperty.ChipDetail" @updateValue="updateValue"
                    :isRef="focusChipDetail" />

                  <div class="error-text">{{ errors.chipDetail }}</div>
                </div>
              </div>
              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.MemoryType }}
                  <span class="required">*</span>
                </label>

                <div class="flex combobox-form">
                  <BaseCombobox tabindex="8" :isCheck="errors.memoryType != '' ? true : false" :hasIcon="true"
                    :items="listMemory" :initItem="configMemory(product.memoryID)" fieldName="memoryType"
                    @focusOut="validateMemoryType" @getValue="setMemory" :disabled="formMode == Enum.Mode.Edit"
                    :placehoder="Resource.Placehoder.Memory" />

                </div>
                <div class="error-text">{{ errors.memoryType }}</div>
              </div>
              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.MemoryDetail }}
                  <span class="required">*</span>
                </label>

                <div class="flex text-field-form">
                  <InputString :transmissionString="product.memoryDetail" :hasError="errors.memoryDetail != ''"
                    :placeholder="Resource.Placehoder.MemoryDetail" maxlength="100" tabindex="9"
                    @eventBlur="focusMemoryDetail = false; validateMemoryDetail();"
                    :nameProperty="Resource.ProductProperty.MemoryDetail" @updateValue="updateValue"
                    :isRef="focusMemoryDetail" />

                  <div class="error-text">{{ errors.memoryDetail }}</div>
                </div>
              </div>
            </div>
            <div class="row-form d-flex">
              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.StorageType }}
                  <span class="required">*</span>
                </label>

                <div class="flex combobox-form">
                  <BaseCombobox tabindex="10" :isCheck="errors.storageType != '' ? true : false" :hasIcon="true"
                    :items="listStorage" :initItem="configStorage(product.storageID)" fieldName="storageType"
                    @focusOut="validateStorageType" @getValue="setStorage" :disabled="formMode == Enum.Mode.Edit"
                    :placehoder="Resource.Placehoder.Storage" />

                </div>
                <div class="error-text">{{ errors.storageType }}</div>
              </div>
              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.StorageDetail }}
                  <span class="required">*</span>
                </label>

                <div class="flex text-field-form">
                  <InputString :transmissionString="product.storageDetail" :hasError="errors.storageDetail != ''"
                    :placeholder="Resource.Placehoder.StorageDetail" maxlength="100" tabindex="11"
                    @eventBlur="focusStorageDetail = false; validateStorageDetail();" :isRef="focusStorageDetail"
                    :nameProperty="Resource.ProductProperty.StorageDetail" @updateValue="updateValue" />

                  <div class="error-text">{{ errors.storageDetail }}</div>
                </div>
              </div>
              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.DisplayType }}
                  <span class="required">*</span>
                </label>

                <div class="flex combobox-form">
                  <BaseCombobox tabindex="12" :isCheck="errors.displayType != '' ? true : false" :hasIcon="true"
                    :items="listDisplay" :initItem="configDisplay(product.displayID)" fieldName="displayType"
                    @focusOut="validateDisplayType" @getValue="setDisplay" :disabled="formMode == Enum.Mode.Edit"
                    :placehoder="Resource.Placehoder.Display" />

                </div>
                <div class="error-text">{{ errors.displayType }}</div>
              </div>
              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.DisplayDetail }}
                  <span class="required">*</span>
                </label>

                <div class="flex text-field-form">
                  <InputString :transmissionString="product.displayDetail" :hasError="errors.displayDetail != ''"
                    :placeholder="Resource.Placehoder.DisplayDetail" maxlength="255" tabindex="13"
                    @eventBlur="focusDisplayDetail = false; validateDisplayDetail();"
                    :nameProperty="Resource.ProductProperty.DisplayDetail" @updateValue="updateValue"
                    :isRef="focusDisplayDetail" />

                  <div class="error-text">{{ errors.displayDetail }}</div>
                </div>
              </div>
            </div>

            <div class="row-form d-flex">
              <div class="flex mr-10">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.CardType }}
                  <span class="required">*</span>
                </label>
                <div class="flex-row flex radio-form">

                  <BaseRadio tabindex="14" @check="product.cardType = Const.CardType.Onboard.Value"
                    :checkedProp="product.cardType == Const.CardType.Onboard.Value" :label="Const.CardType.Onboard.Label">

                    <template #radiomark>
                      <div class="radio-checkmark"></div>
                    </template>

                  </BaseRadio>

                  <BaseRadio tabindex="15" @check="product.cardType = Const.CardType.Removable.Value"
                    :checkedProp="product.cardType == Const.CardType.Removable.Value"
                    :label="Const.CardType.Removable.Label">

                    <template #radiomark>
                      <div class="radio-checkmark"></div>
                    </template>
                  </BaseRadio>
                </div>
              </div>
              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.CardDetail }}
                  <span class="required">*</span>
                </label>

                <div class="flex text-field-form">
                  <InputString :transmissionString="product.cardDetail" :hasError="errors.cardDetail != ''"
                    :placeholder="Resource.Placehoder.CardDetail" maxlength="255" tabindex="16"
                    @eventBlur="focusCardDetail = false; validateCardDetail();"
                    :nameProperty="Resource.ProductProperty.CardDetail" @updateValue="updateValue"
                    :isRef="focusCardDetail" />

                  <div class="error-text">{{ errors.cardDetail }}</div>
                </div>
              </div>
              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.Condition }}
                  <span class="required">*</span>
                </label>

                <div class="flex combobox-form">
                  <BaseCombobox tabindex="17" :isCheck="errors.condition != '' ? true : false" :hasIcon="true"
                    :items="Const.Condition" :initItem="configCondition(product.condition)" fieldName="Label"
                    :placehoder="Resource.Placehoder.Condition" @focusOut="validateCondition" @getValue="setCondition"
                    :disabled="formMode == Enum.Mode.Edit" :isReadonly="true" />

                </div>
                <div class="error-text">{{ errors.condition }}</div>
              </div>
              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.AmountRam }}
                </label>

                <div class="flex text-field-form">
                  <InputNumber :transmissionNumber="product.amountRam" :decimalPlaces="0" tabindex="18" :hasIcon="true"
                    :max="10" :nameProperty="Resource.ProductProperty.AmountRam" @update="updateValue" />
                </div>
              </div>
            </div>

            <div class="row-form d-flex" v-if="formMode == Enum.Mode.Edit">
              <div class="flex mr-10">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.TotalQuantity }}
                </label>
                <div class="flex text-field-form">
                  <InputNumber :transmissionNumber="product.quantity" :decimalPlaces="0" :isReadonly="true" />
                </div>
              </div>
              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.AmountSell }}
                </label>

                <div class="flex text-field-form">
                  <InputNumber :transmissionNumber="product.quantity - product.inventory" :decimalPlaces="0"
                    isReadonly="true" />
                </div>
              </div>
              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.Inventory }}
                </label>

                <div class="flex text-field-form">
                  <InputNumber :transmissionNumber="product.inventory" :decimalPlaces="0" isReadonly="true" />
                </div>
              </div>
              <div class="flex mr-10 block flex-end-center">
                <BaseButton tabindex="19" @click="isShowFormImport = true" @keydown.enter="isShowFormImport = true"
                  class="main-button btn ml-10">{{ Resource.Button.ImportProduct }}
                </BaseButton>
              </div>
            </div>

            <div class="row-form d-flex" v-if="formMode == Enum.Mode.Edit">
              <div class="flex mr-10">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.Price }}
                </label>
                <div class="flex text-field-form">
                  <InputNumber :transmissionNumber="product.price" :decimalPlaces="0" tabindex="20"
                    :nameProperty="Resource.ProductProperty.Price" @update="updateValue" />
                </div>
              </div>
              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.Discount }}
                </label>

                <div class="flex text-field-form">
                  <InputNumber :transmissionNumber="product.discount" :decimalPlaces="0" tabindex="21" :max="100"
                    :nameProperty="Resource.ProductProperty.Discount" @update="updateValue" />
                </div>
              </div>
              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.PriceSell }}
                </label>

                <div class="flex text-field-form">
                  <InputNumber :transmissionNumber="getNewPrice(product.price, product.discount)" :decimalPlaces="0"
                    tabindex="22" :nameProperty="Resource.ProductProperty.Discount" @update="updateDiscount" />
                </div>
              </div>
              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.Status }}
                </label>

                <div class="flex combobox-form">
                  <BaseCombobox tabindex="23" :hasIcon="true" :items="Const.StatusProduct"
                    :initItem="configStatus(product.status)" fieldName="Label" @getValue="setStatus" :isReadonly="true" />

                </div>
              </div>
            </div>

            <div class="row-form d-flex">
              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.AmountDisk }}
                </label>

                <div class="flex text-field-form">
                  <InputNumber :transmissionNumber="product.amountDisk" :decimalPlaces="0" tabindex="24" :hasIcon="true"
                    :max="10" :nameProperty="Resource.ProductProperty.AmountDisk" @update="updateValue" />
                </div>
              </div>
              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.Weight }}
                </label>

                <div class="flex text-field-form">
                  <InputNumber :transmissionNumber="product.weight" :decimalPlaces="2" tabindex="25" :hasIcon="true"
                    :max="10" :nameProperty="Resource.ProductProperty.Weight" @update="updateValue" />
                </div>
              </div>

              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.Dimension }}
                </label>

                <div class="flex text-field-form">
                  <InputString :transmissionString="product.dimension" maxlength="255" tabindex="26"
                    :nameProperty="Resource.ProductProperty.Dimension" @updateValue="updateValue" />
                </div>
              </div>

              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.Material }}
                </label>

                <div class="flex text-field-form">
                  <InputString :transmissionString="product.material" maxlength="100" tabindex="27"
                    :nameProperty="Resource.ProductProperty.Material" @updateValue="updateValue" />
                </div>
              </div>

            </div>

            <div class="row-form d-flex">
              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.Color }}
                </label>

                <div class="flex text-field-form">
                  <InputString :transmissionString="product.color" maxlength="50" tabindex="28"
                    :nameProperty="Resource.ProductProperty.Color" @updateValue="updateValue" />
                </div>
              </div>
              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.OperatingSystem }}
                </label>

                <div class="flex text-field-form">
                  <InputString :transmissionString="product.operatingSystem" maxlength="100" tabindex="29"
                    :nameProperty="Resource.ProductProperty.OperatingSystem" @updateValue="updateValue" />
                </div>
              </div>

              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.Touchpad }}
                </label>

                <div class="flex text-field-form">
                  <InputString :transmissionString="product.touchpad" maxlength="255" tabindex="30"
                    :nameProperty="Resource.ProductProperty.Touchpad" @updateValue="updateValue" />
                </div>
              </div>

              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.Keyboard }}
                </label>

                <div class="flex text-field-form">
                  <InputString :transmissionString="product.keyboard" maxlength="255" tabindex="31"
                    :nameProperty="Resource.ProductProperty.Keyboard" @updateValue="updateValue" />
                </div>
              </div>

            </div>

            <div class="row-form d-flex">
              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.Speakers }}
                </label>

                <div class="flex text-field-form">
                  <InputString :transmissionString="product.speakers" maxlength="255" tabindex="32"
                    :nameProperty="Resource.ProductProperty.Speakers" @updateValue="updateValue" />
                </div>
              </div>
              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.Battery }}
                </label>

                <div class="flex text-field-form">
                  <InputString :transmissionString="product.battery" maxlength="255" tabindex="33"
                    :nameProperty="Resource.ProductProperty.Battery" @updateValue="updateValue" />
                </div>
              </div>

              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.Camera }}
                </label>

                <div class="flex text-field-form">
                  <InputString :transmissionString="product.camera" maxlength="255" tabindex="34"
                    :nameProperty="Resource.ProductProperty.Camera" @updateValue="updateValue" />
                </div>
              </div>

              <div class="flex mr-10 block">
                <label for="" class="label-form d-flex">
                  {{ Resource.Label.ConnectivityNetwork }}
                </label>

                <div class="flex text-field-form">
                  <InputString :transmissionString="product.connectivityNetwork" tabindex="35"
                    :nameProperty="Resource.ProductProperty.ConnectivityNetwork" @updateValue="updateValue" />
                </div>
              </div>

            </div>

            <div class="row-form">

              <label for="" class="label-form d-flex">
                {{ Resource.Label.StandardPorts }}
              </label>

              <div class="flex text-aria-form">

                <div class="flex-row border">
                  <textarea :placeholder="Resource.Placehoder.StandardPorts" class="aria-form flex" rows="4"
                    v-model.trim="product.standardPorts" tabindex="36"></textarea>
                </div>
              </div>
            </div>

            <div class="row-form">

              <label for="" class="label-form d-flex">
                {{ Resource.Label.Description }}
              </label>

              <div class="flex text-aria-form">

                <div class="flex-row border">
                  <textarea :placeholder="Resource.Placehoder.Description" class="aria-form flex" rows="4"
                    v-model.trim="product.description" tabindex="37"></textarea>
                </div>
              </div>
            </div>

            <div class="flex row-form">
              <label for="" class="label-form d-flex">
                {{ Resource.Label.Gift }}
              </label>

              <div class="flex combobox-form">
                <ComboboxCheckbox tabindex="38" :items="listGift" :initItem="this.listGifts" fieldName="description"
                  @getValue="setListGifts" :placehoder="Resource.Placehoder.Gift" :isReadonly="true" />

              </div>
            </div>
          </div>
        </div>
        <div class="modal-footer">
          <div class="flex-row flex-end">

            <BaseButton tabindex="41" @keydown.tab.prevent.exact="this.focusProductName = true;" @mousedown="closeForm"
              @keydown.enter="closeForm" class="sub-button btn">
              {{ Resource.Button.Cancel }}
            </BaseButton>

            <el-tooltip effect="dark" :content="Const.KeyStrokes.CtrlShiftS" placement="top">
              <BaseButton tabindex="40" @click="save(true)" @keydown.enter="save(true)" v-show="!checkForm()"
                class="button-blue btn ml-10">{{ Resource.Button.SaveAndNew }}
              </BaseButton>
            </el-tooltip>

            <el-tooltip effect="dark" :content="Const.KeyStrokes.CtrlS" placement="top">
              <BaseButton tabindex="39" @click="save(false)" @keydown.enter="save(false)" v-show="!checkForm()"
                class="main-button btn ml-10">{{ Resource.Button.Save }}</BaseButton>
            </el-tooltip>

            <el-tooltip effect="dark" :content="Const.KeyStrokes.CtrlS" placement="top">
              <BaseButton tabindex="39" @click="save(false)" @keydown.enter="save(false)" v-show="checkForm()"
                style="padding: 0 16px;" class="main-button btn ml-10">{{ Resource.Button.SaveChange }}</BaseButton>
            </el-tooltip>

          </div>
        </div>
      </div>
    </form>
  </div>

  <ImportProduct @refreshData="reloadData" @showToast="showToast"
    :productId="product.productID" :closeFormImport="closeFormImport" v-if="isShowFormImport"/>

  <BasePopup @close="closePopup" class="popup-delete" :title="this.titlePopup" :content="contentPopup" v-if="isShowPopup">

    <template #buttons>
      <BaseButton @click="closePopup" class="main-button-red btn ml-10">{{ Resource.Button.Close }}</BaseButton>
    </template>

  </BasePopup>

  <BaseToastMessage v-show="toastMessage.isShowed" :class="`toast-${toastMessage.type} icon-toast-${toastMessage.type}`">

    <template #message>{{ toastMessage.message }}</template>
  </BaseToastMessage>

  <ViewImage :link="Resource.PrefixImage + product.mainImage" v-show="isShowMainImage"
    @focusoutImage="isShowMainImage = false;" />
</template>
<script>
import Const from '@/utils/const'
import Enum from '@/utils/enum'
import Resource from '@/utils/resource'
import BaseCombobox from "../components/base/BaseCombobox.vue";
import BaseButton from "../components/base/BaseButton.vue";
import BasePopup from "../components/base/BasePopup.vue";
import InputString from '@/components/base/BaseInputString.vue';
import InputNumber from '@/components/base/BaseInputNumber.vue';
import BaseRadio from '@/components/base/BaseRadio.vue';
import ComboboxCheckbox from '@/components/base/ComboboxCheckbox.vue';
import ImportProduct from './ImportProduct.vue';
import BaseToastMessage from '@/components/base/BaseToastMessage.vue';
import ViewImage from './ViewImage.vue';
import axios from 'axios';
export default {
  components: {
    BaseCombobox, BaseButton, BasePopup, InputString, InputNumber, BaseRadio,
    ComboboxCheckbox, ViewImage, ImportProduct, BaseToastMessage
  },
  props: ["closeFormDetail", "productIdUpdate", "formMode"],
  data() {
    return {
      Enum: Enum,
      Resource: Resource,
      Const: Const,
      isTitle: "", // tiêu đề của form

      titlePopup: Resource.Title.Management, // tiêu đề của popup

      contentPopup: "", // nội dung cảnh báo

      isShowPopup: false, // cờ điền khiển đóng mở cảnh báo lỗi

      isShowFormImport: false, // cờ điền khiển đóng mở form nhập hàng

      isShowMainImage: false, // Cờ điều khiển đóng mở xem ảnh sản phẩm

      errors: { // danh sách lỗi validate của các trường thông tin
        productName: "",
        category: "",
        chipType: "",
        memoryType: "",
        storageType: "",
        displayType: "",
        chipDetail: "",
        memoryDetail: "",
        storageDetail: "",
        displayDetail: "",
        cardDetail: "",
        condition: "",
        demand: "",
        avatarProduct: ""
      },

      productInit: { // thông tin sản phẩm mặc định của form thêm mới
        productName: '', // tên sản phẩm
        categoryID: '', // id danh mục sản phẩm
        categoryName: '', // tên danh mục sản phẩm
        mainImage: '', // avatar sản phẩm
        listImageString: '', // danh sách ảnh chi tiết sản phẩm
        chipID: '', // id chip
        chipDetail: '', // chi tiết chip
        memoryID: '', // id ram
        memoryDetail: '', // chi tiết ram
        storageID: '', // id ổ cứng
        storageDetail: '', // chi tiết ổ cứng
        displayID: '', // id màn hình
        displayDetail: '', // chi tiết màn hình
        cardType: Const.CardType.Onboard.Value, // loại card (true: card rời, false: card liền)
        cardDetail: '', // chi tiết card
        demandTypeString: '', // loại nhu cầu 
        description: '', // thông tin chi tiết
        condition: Const.Condition.New, // tình trạng sản phẩm (0: new 100%, 1: newOutlet, 2: used)
        amountRam: '', // số lượng ram
        amountDisk: '', // số ổ đĩa
        weight: '', // cân nặng
        dimension: '', // kích thước
        color: '', // màu sắc
        material: '', // vật liệu
        operatingSystem: '', // hệ điều hành
        touchpad: '', // bàn di chuột
        keyboard: '', // bàn phím 
        speakers: '', // âm thanh
        battery: '', // pin
        camera: '', // máy ảnh
        connectivityNetwork: '', // giao tiếp mạng
        standardPorts: '', // cổng kết nốt
        createdBy: '5dd9b7cf-7185-4a4c-b922-5a9c12ce1a89',

        quantity: 0, // tổng số lượng
        inventory: 0, // số lượng còn trong kho
        price: 0, // giá gốc
        discount: 0, // giảm giá
        status: 1, // trạng thái sản phẩm (0: tạm ngừng bán, 1: đang bán, 2: hàng sắp về)
        // numberView: 0, // lượt xem
      },

      product: {}, // thông tin sản phẩm trong form thêm mới

      listCategory: [], //Danh sách danh mục sản phẩm

      listChip: [], //Danh sách các loại chip

      listMemory: [], //Danh sách các loại Ram

      listStorage: [], //Danh sách ổ cứng

      listDisplay: [], //Danh sách màn hình 

      listImage: [], // Danh sách ảnh sản phẩm

      listGift: [], // Danh sách quà tặng 

      listGifts: [], // Danh sách quà tặng kèm sản phẩm

      nameFileAvatar: Resource.ChooseImage,

      listImageTxt: Resource.ChooseImage,

      errorDuplicateNameProduct: false, // Cờ thể hiện xem có đang bị trùng tên sản phẩm hay không

      focusProductName: false,
      focusChipDetail: false,
      focusMemoryDetail: false,
      focusStorageDetail: false,
      focusDisplayDetail: false,
      focusCardDetail: false,

      toastMessage: { // cảnh báo
        message: "", // nội dung cảnh báo
        type: "", // loại cảnh báo
        isShowed: false, // cờ điều khiển bật tắt cảnh báo
      },
    };
  },

  methods: {

    updateValue(value, nameProperty) {
      this.product[nameProperty] = value;
    },

    updateDiscount(value, nameProperty) {
      let discount = value * 100 / this.product.price;
      discount = discount - discount % 1;
      discount = 100 - discount;
      this.product[nameProperty] = discount;
    },

    handleUploadAvatar() {
      this.nameFileAvatar = this.$refs.avatar.files[0].name;
    },

    handleUploadImageDetail() {
      this.listImageTxt = "Đã upload " + this.$refs.imageDetail.files.length + " tệp.";
    },

    seeAvatar() {
      this.isShowMainImage = true;
    },

    seeImageDetail() {
      console.log(this.$refs.imageDetail.files);
    },

    getNewPrice(oldPrice, saleoff) {
      let money = oldPrice * (100 - saleoff) / 1000000;
      money = money - money % 1;
      money = money * 10000;
      return money;
    },

    updateImage(value) {
      this.product.mainImage = value;
    },
    updateImageDetail(value) {
      this.product.listImageString = value;
    },

    reloadData(){
      this.getProductByID(this.productIdUpdate);
      this.$emit("refreshData");
    },

    /**
    * Set giá trị text danh mục sản phẩm về id danh mục sản phẩm
    * @param {string} value :  giá trị danh mục sản phẩm dạng text người dùng chọn
    * Author: HAQUAN(29/08/2023) 
    */
    setCategory(value) {
      let check = false;
      this.listCategory.find((item) => {
        if (item.categoryName == value) {
          this.product.categoryID = item.categoryID;
          this.product.categoryName = item.categoryName;
          this.errors.category = "";
          check = true;
        }
      });
      if (!check) {
        // this.product.categoryName = value;
        if (value == "") {
          this.errors.category = Resource.Error.Category;
        }
        else {
          // Lấy danh sách danh mục sản phẩm có chứa giá trị value
          let checkListCategory = this.listCategory.filter((item) => {
            if (item.categoryName.toLowerCase().includes(value.toLowerCase())) {
              return item;
            }
          });

          // Nếu danh sách không có phần tử nào, hiển thị lỗi danh mục sản phẩm không có trong danh sách
          if (checkListCategory.length == 0) {
            this.errors.category = Resource.Error.CategoryNotFound;
          }
        }
      }
    },

    /**
    * Set giá trị text loại chip về id loại chip
    * @param {string} value :  giá trị loại chip dạng text người dùng chọn
    * Author: HAQUAN(29/08/2023) 
    */
    setChip(value) {
      let check = false;
      this.listChip.find((item) => {
        if (item.chipType == value) {
          this.product.chipID = item.chipID;
          this.errors.chipType = "";
          check = true;
        }
      });
      if (!check) {
        // this.product.chipID = value;
        if (value == "") {
          this.errors.chipType = Resource.Error.Chip;
        }
        else {
          this.errors.chipType = Resource.Error.ChipNotFound;
        }
      }
    },

    /**
    * Hiển thị giá trị text loại chip tương ứng với id 
    * @param {string} value : id loại chip
    * Author: HAQUAN(29/08/2023) 
    */
    configChip(value) {
      let result = value;
      this.listChip.find((item) => {
        if (item.chipID === value) {
          result = item.chipType;
        }
      });
      return result;
    },
    /**
    * Set giá trị text loại ram về id loại ram
    * @param {string} value :  giá trị loại ram dạng text người dùng chọn
    * Author: HAQUAN(29/08/2023) 
    */
    setMemory(value) {
      let check = false;
      this.listMemory.find((item) => {
        if (item.memoryType == value) {
          this.product.memoryID = item.memoryID;
          this.errors.memoryType = "";
          check = true;
        }
      });
      if (!check) {
        // this.product.memoryID = value;
        if (value == "") {
          this.errors.memoryType = Resource.Error.Memory;
        }
        else {
          this.errors.memoryType = Resource.Error.MemoryNotFound;
        }
      }
    },

    /**
    * Hiển thị giá trị text loại ram tương ứng với id 
    * @param {string} value : id loại ram
    * Author: HAQUAN(29/08/2023) 
    */
    configMemory(value) {
      let result = value;
      this.listMemory.find((item) => {
        if (item.memoryID === value) {
          result = item.memoryType;
        }
      });
      return result;
    },
    /**
    * Set giá trị text loại ổ cứng về id loại ổ cứng
    * @param {string} value :  giá trị loại ổ cứng dạng text người dùng chọn
    * Author: HAQUAN(29/08/2023) 
    */
    setStorage(value) {
      let check = false;
      this.listStorage.find((item) => {
        if (item.storageType == value) {
          this.product.storageID = item.storageID;
          this.errors.storageType = "";
          check = true;
        }
      });
      if (!check) {
        // this.product.storageID = value;
        if (value == "") {
          this.errors.storageType = Resource.Error.Storage;
        }
        else {
          this.errors.storageType = Resource.Error.StorageNotFound;
        }
      }
    },

    /**
    * Hiển thị giá trị text loại ổ cứng tương ứng với id 
    * @param {string} value : id loại ổ cứng
    * Author: HAQUAN(29/08/2023) 
    */
    configStorage(value) {
      let result = value;
      this.listStorage.find((item) => {
        if (item.storageID === value) {
          result = item.storageType;
        }
      });
      return result;
    },
    /**
    * Set giá trị text loại màn hình về id loại màn hình
    * @param {string} value :  giá trị loại màn hình dạng text người dùng chọn
    * Author: HAQUAN(29/08/2023) 
    */
    setDisplay(value) {
      let check = false;
      this.listDisplay.find((item) => {
        if (item.displayType == value) {
          this.product.displayID = item.displayID;
          this.errors.displayType = "";
          check = true;
        }
      });
      if (!check) {
        // this.product.displayID = value;
        if (value == "") {
          this.errors.displayType = Resource.Error.Display;
        }
        else {
          this.errors.displayType = Resource.Error.DisplayNotFound;
        }
      }
    },

    /**
    * Hiển thị giá trị text loại màn hình tương ứng với id 
    * @param {string} value : id loại màn hình
    * Author: HAQUAN(29/08/2023) 
    */
    configDisplay(value) {
      let result = value;
      this.listDisplay.find((item) => {
        if (item.displayID === value) {
          result = item.displayType;
        }
      });
      return result;
    },

    /**
    * Set giá trị text tình trạng sản phẩm về giá trị lưu trong DB
    * @param {string} value :  giá trị tình trạng sản phẩm dạng text người dùng chọn
    * Author: HAQUAN(29/08/2023) 
    */
    setCondition(value) {
      Const.Condition.find((item) => {
        if (item.Label == value) {
          this.product.condition = item.Value;
          this.errors.condition = "";
        }
      });
    },

    /**
    * Hiển thị giá trị text tình trạng sản phẩm tương ứng với giá trị tình trạng sản phẩm lưu trong DB
    * @param {string} value : giá trị tình trạng sản phẩm lưu trong DB
    * Author: HAQUAN(29/08/2023) 
    */
    configCondition(value) {
      let result;
      Const.Condition.find(
        (item) => {
          if (item.Value == value) {
            result = item.Label;
          }
        }
      );
      return result;
    },

    /**
    * Set giá trị text trạng thái sản phẩm về giá trị lưu trong DB
    * @param {string} value :  giá trị trạng thái sản phẩm dạng text người dùng chọn
    * Author: HAQUAN(29/08/2023) 
    */
    setStatus(value) {
      Const.StatusProduct.find((item) => {
        if (item.Label == value) {
          this.product.status = item.Value;
        }
      });
    },

    /**
    * Hiển thị giá trị text trạng thái sản phẩm tương ứng với giá trị trạng thái sản phẩm lưu trong DB
    * @param {string} value : giá trị trạng thái sản phẩm lưu trong DB
    * Author: HAQUAN(29/08/2023) 
    */
    configStatus(value) {
      let result;
      Const.StatusProduct.find(
        (item) => {
          if (item.Value == value) {
            result = item.Label;
          }
        }
      );
      return result;
    },

    /**
    * Convert danh sách nhu cầu sử dụng về chuỗi string để lưu trong DB
    * @param {array} value : danh sách nhu cầu sử dụng người dùng chọn
    * Author: HAQUAN(29/08/2023) 
    */
    setDemandType(value) {
      let listDemand = [];
      value.forEach(element => {
        listDemand.push(element.Value);
      });
      this.product.demandTypeString = listDemand.toString();
    },

    /**
    * Update danh sách quà tặng kèm sản phẩm
    * @param {array} value : danh sách quà tặng kèm sản phẩm người dùng chọn
    * Author: HAQUAN(29/08/2023) 
    */
    setListGifts(value) {
      this.listGifts = value;
    },

    /**
    * Convert danh sách nhu cầu sử dụng từ chuỗi string sang dạng array
    * @param {string} value : chuỗi string danh sách nhu cầu sử dụng
    * Author: HAQUAN(29/08/2023) 
    */
    configDemandType(value) {
      if (value) {
        return value.split(",").map(Number);
      }
      return [];
    },

    /**
     * Lấy dữ liệu danh mục sản phẩm 
     * Author: HAQUAN(26/08/2023)
     */
    async getCategory() {
      try {
        await axios({
          url: 'Category/getAll',
          method: 'get',
        }).then((response) => {
          response.data.forEach(element => {
            this.listCategory.push(element);
          });
        })
          .catch((error) => {
            console.log(error);
          })

      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Lấy danh sách các loại chip 
     * Author: HAQUAN(26/08/2023)
     */
    async getChip() {
      try {
        await axios({
          url: 'Chip/getAll',
          method: 'get',
        }).then((response) => {
          response.data.forEach(element => {
            this.listChip.push(element);
          });
        })
          .catch((error) => {
            console.log(error);
          })

      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Lấy danh sách các loại ram
     * Author: HAQUAN(26/08/2023)
     */
    async getMemory() {
      try {
        await axios({
          url: 'Memory/getAll',
          method: 'get',
        }).then((response) => {
          response.data.forEach(element => {
            this.listMemory.push(element);
          });
        })
          .catch((error) => {
            console.log(error);
          })

      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Lấy danh sách các loại ổ cứng
     * Author: HAQUAN(26/08/2023)
     */
    async getStorage() {
      try {
        await axios({
          url: 'Storage/getAll',
          method: 'get',
        }).then((response) => {
          response.data.forEach(element => {
            this.listStorage.push(element);
          });
        })
          .catch((error) => {
            console.log(error);
          })

      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Lấy danh sách các loại màn hình
     * Author: HAQUAN(26/08/2023)
     */
    async getDisplay() {
      try {
        await axios({
          url: 'Display/getAll',
          method: 'get',
        }).then((response) => {
          response.data.forEach(element => {
            this.listDisplay.push(element);
          });
        })
          .catch((error) => {
            console.log(error);
          })

      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Lấy dữ liệu quà tặng kèm sản phẩm 
     * Author: HAQUAN(26/08/2023)
     */
    async getGift() {
      try {
        await axios({
          url: 'Gift/getAll',
          method: 'get',
        }).then((response) => {
          response.data.forEach(element => {
            this.listGift.push(element);
          });
        })
          .catch((error) => {
            console.log(error);
          })

      } catch (error) {
        console.log(error);
      }
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
    * Đóng form nhập hàng 
    * Author: HAQUAN(28/10/2022)
    */
    closeFormImport() {
      this.isShowFormImport = false;
    },

    /**
     * Lưu thông tin sản phẩm
     * @param {*} control: kiểu lưu sản phẩm, nếu false thì lưu và đóng form, còn true thì lưu và reset dữ liệu, không đóng form
     *  Author: HAQUAN(29/08/2023)  
     */
    save(control) {
      try {
        let valid = this.validate();
        if (valid) {
          // Nếu như là chỉnh sửa thông tin sản phẩm
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
     * Gửi request chỉnh sửa thông tin sản phẩm đến server
     *  Author: HAQUAN(29/08/2023) 
     */
    async sendRequestUpdate() {
      try {
        this.product.modifiedBy = this.$store.getters.user.accountID;
        let productModel = {
          product: this.product,
          listGifts: this.listGifts
        };
        await axios.put("Product/" + this.product.productID, productModel)
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
     * Gửi request thêm mới sản phẩm
     * @param {*} control: kiểu lưu sản phẩm, nếu false thì lưu và đóng form, còn true thì lưu và reset dữ liệu, không đóng form
     *  Author: HAQUAN(29/08/2023) 
     */
    async sendRequestInsert(control) {
      debugger
      let formData = new FormData();
      formData.append('listImages[' + 0 + ']', this.$refs.avatar.files[0]);
      let list = this.$refs.imageDetail.files;
      for (var i = 0; i < list.length; i++) {
        formData.append('listImages[' + (i + 1) + ']', list[i]);
      }
      formData.append('product', JSON.stringify(this.product));
      formData.append('listGifts', JSON.stringify(this.listGifts));
      try {
        await axios.post('Product/', formData, {
          headers: {
            'Content-Type': 'multipart/form-data',
            "Access-Control-Allow-Origin": "*",
          },
        }).then((response) => {
          console.log(response);

          // Hiển thị toast thông báo thành công
          this.$emit("showToast", Resource.Message.AddProductSucces, Const.TypeToast.Success);
          this.$emit("refreshData");
          if (!control) {
            this.closeFormDetail();
          }
          else {
            this.product = { ...this.productInit };
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
        this.errorDuplicateNameProduct = false;

        // Nếu lỗi trả về là trùng tên sản phẩm
        if (error.response.data == Resource.Error.ProductNameExists) {
          this.errorDuplicateNameProduct = true;
        }

        this.contentPopup = error.response.data.userMsg;
        this.titlePopup = Resource.Title.Management;
        this.isShowPopup = true;

      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Validate không để trống tên sản phẩm
     * Author: HAQUAN(29/08/2023) 
     */
    validateRequiredName() {
      try {
        if (!this.product.productName) {
          this.errors.productName = Resource.Error.ProductName;
          return false;
        }
        this.errors.productName = "";
        return true;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Validate không để trống avatar sản phẩm
     * Author: HAQUAN(29/08/2023) 
     */
    validateAvatarImage() {
      try {
        if (this.$refs.avatar.files.length == 0 && this.formMode == Enum.Mode.Add) {
          this.errors.avatarProduct = Resource.Error.AvatarProduct;
          return false;
        }
        this.errors.avatarProduct = "";
        return true;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Validate không để trống danh mục sản phẩm
     * Author: HAQUAN(29/08/2023) 
     */
    validateCategory() {
      try {
        if (!this.product.categoryID) {
          this.errors.category = Resource.Error.Category;
          return false;
        }
        this.errors.category = "";
        return true;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Validate không để trống loại chip
     * Author: HAQUAN(29/08/2023) 
     */
    validateChipType() {
      try {
        if (!this.product.chipID) {
          this.errors.chipType = Resource.Error.Chip;
          return false;
        }
        this.errors.chipType = "";
        return true;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Validate không để trống loại Ram
     * Author: HAQUAN(29/08/2023) 
     */
    validateMemoryType() {
      try {
        if (!this.product.memoryID) {
          this.errors.memoryType = Resource.Error.Memory;
          return false;
        }
        this.errors.memoryType = "";
        return true;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Validate không để trống loại ổ cứng
     * Author: HAQUAN(29/08/2023) 
     */
    validateStorageType() {
      try {
        if (!this.product.storageID) {
          this.errors.storageType = Resource.Error.Storage;
          return false;
        }
        this.errors.storageType = "";
        return true;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Validate không để trống loại màn hình
     * Author: HAQUAN(29/08/2023) 
     */
    validateDisplayType() {
      try {
        if (!this.product.displayID) {
          this.errors.displayType = Resource.Error.Display;
          return false;
        }
        this.errors.displayType = "";
        return true;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Validate không để trống tình trạng sản phẩm
     * Author: HAQUAN(29/08/2023) 
     */
    validateCondition() {
      try {
        if (this.product.condition == null) {
          this.errors.condition = Resource.Error.Condition;
          return false;
        }
        this.errors.condition = "";
        return true;
      } catch (error) {
        console.log(error);
      }
    },


    /**
     * Validate không để trống chi tiết chip
     * Author: HAQUAN(29/08/2023) 
     */
    validateChipDetail() {
      try {
        if (!this.product.chipDetail) {
          this.errors.chipDetail = Resource.Error.ChipDetail;
          return false;
        }
        this.errors.chipDetail = "";
        return true;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Validate không để trống chi tiết Ram
     * Author: HAQUAN(29/08/2023) 
     */
    validateMemoryDetail() {
      try {
        if (!this.product.memoryDetail) {
          this.errors.memoryDetail = Resource.Error.MemoryDetail;
          return false;
        }
        this.errors.memoryDetail = "";
        return true;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Validate không để trống chi tiết ổ cứng
     * Author: HAQUAN(29/08/2023) 
     */
    validateStorageDetail() {
      try {
        if (!this.product.storageDetail) {
          this.errors.storageDetail = Resource.Error.StorageDetail;
          return false;
        }
        this.errors.storageDetail = "";
        return true;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Validate không để trống chi tiết màn hình
     * Author: HAQUAN(29/08/2023) 
     */
    validateDisplayDetail() {
      try {
        if (!this.product.displayDetail) {
          this.errors.displayDetail = Resource.Error.DisplayDetail;
          return false;
        }
        this.errors.displayDetail = "";
        return true;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Validate không để trống chi tiết card màn hình
     * Author: HAQUAN(29/08/2023) 
     */
    validateCardDetail() {
      try {
        if (!this.product.cardDetail) {
          this.errors.cardDetail = Resource.Error.CardDetail;
          return false;
        }
        this.errors.cardDetail = "";
        return true;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Validate thông tin sản phẩm trước khi lưu
     * Author: HAQUAN(29/08/2023)  
     */
    validate() {
      try {
        let valid = true;

        if (!this.validateRequiredName()) {
          this.focusProductName = true;
          valid = false;
        }

        if (!this.validateAvatarImage()) {
          valid = false;
        }

        if (!this.validateCategory()) {
          valid = false;
        }

        if (!this.validateChipType()) {
          valid = false;
        }

        if (!this.validateMemoryType()) {
          valid = false;
        }

        if (!this.validateStorageType()) {
          valid = false;
        }

        if (!this.validateDisplayType()) {
          valid = false;
        }

        if (!this.validateCondition()) {
          valid = false;
        }

        if (!this.validateChipDetail()) {
          this.focusChipDetail = true;
          valid = false;
        }

        if (!this.validateMemoryDetail()) {
          this.focusMemoryDetail = true;
          valid = false;
        }

        if (!this.validateStorageDetail()) {
          this.focusStorageDetail = true;
          valid = false;
        }

        if (!this.validateDisplayDetail()) {
          this.focusDisplayDetail = true;
          valid = false;
        }

        if (!this.validateCardDetail()) {
          this.focusCardDetail = true;
          valid = false;
        }
        return valid;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Lấy thông tin sản phẩm theo ID
     * @param {*} productID : ID của sản phẩm muốn lấy
     * Author: HAQUAN(29/08/2023) 
     */
    async getProductByID(productID) {
      try {
        this.isLoading = true;
        let url = `Product/${productID}`;
        await axios.get(url)
          .then((response) => {
            this.product = response.data.product;
            this.listGifts = response.data.listGifts;
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

      if (this.errorDuplicateNameProduct) {
        this.errors.productName = Resource.Error.ProductNameExists;
        this.focusProductName = true;
      }
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

    /**
     * Lấy dữ liệu cho các combobox
     *  Author: HAQUAN(29/08/2023) 
     */
    getDataCombobox() {
      this.getCategory();
      this.getChip();
      this.getMemory();
      this.getStorage();
      this.getDisplay();
      this.getGift();
    },


  },
  created() {
    this.product = { ...this.productInit };
    this.product.createdBy = this.$store.getters.user.accountID;
    this.getDataCombobox();
    if (this.formMode == Enum.Mode.Edit) {
      this.isTitle = Resource.Title.EditProduct;
      this.getProductByID(this.productIdUpdate);
    }
  },
  mounted() {
    if (this.formMode == Enum.Mode.Add) {
      this.isTitle = Resource.Title.AddProduct;
    }
    // focus vào ô input tên sản phẩm
    this.focusProductName = true;

  },

};
</script>
<style scoped>
@import url(../css/base/radio.css);

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
}
</style>
