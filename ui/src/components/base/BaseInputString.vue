<template>
    <div :class="['flex-row border align-items-center',
        { 'error': hasError },{ disabled: disabled } ]">
        <input :placeholder="placeholder" :maxlength="maxlength" class="input-form flex" type="text"
            @blur="evenBlur(); showIcon = false;" @focus="showIcon = true" :tabindex="tabindex"
            v-model.trim="value" ref="refName" :readonly="isReadonly" :disabled="disabled">

        <div v-show="value && showIcon" @mousedown="value = ''" class="icon-X" style="left=0px;"></div>

    </div>
</template>
<script>
import Resource from '@/utils/resource'
export default {
    data() {
        return {
            Resource: Resource,
            showIcon: false,
            value: "",
        };
    },
    props: {
        transmissionString: { // giá trị truyền vào
            type: String
        },

        hasError: {
            type: Boolean,
            default: false,
        },

        placeholder: {
            type: String
        },

        tabindex: {
            type: String
        },

        maxlength: {
            type: String
        },

        isRef: {
            type: Boolean,
            default: false
        },

        isReadonly: {
            type: Boolean,
            default: false
        },

        nameProperty: {
            type: String
        },

        disabled: {
            type: Boolean,
            default: false,
        }

    },

    methods: {
        evenBlur() {
            this.$emit('eventBlur');
        }

    },
    created() {
        this.value = this.transmissionString;
    },
    watch: {
        transmissionString(){
            this.value = this.transmissionString;
        },
        isRef(){
            if(this.isRef){
                this.$refs.refName.focus();
            }
        },
        'value': {
            handler: function (newVal) {
                if (newVal != undefined) {
                    this.$emit("updateValue", newVal, this.nameProperty);
                }
            }
        },
    }
};
</script>
<style scoped>
.input-form {
  border: none;
  padding: 9px 12px;
  background: 0 0;
  min-height: 34px;
}

input:focus{
  outline: none;
}
</style>
  