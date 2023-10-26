<template>
    <button type="button">
        <slot></slot>
    </button>
</template>
<script>
    
</script>
<style scoped>
.btn {
  min-height: 36px;
  min-width: 80px;
  border: none;
  box-shadow: none;
  position: relative;
  overflow: hidden;
  padding: 0 16px;
  border-radius: 4px;
  white-space: nowrap;
  align-items: center;
  justify-items: center;
  justify-content: center;
  cursor: pointer;
}

.btn .text-button {
  display: inline-block;
  white-space: nowrap;
  padding: 0 16px 0 0;
  font-weight: 400;
  font-size: 14px;
}

.text-btn {
  display: inline-block;
  white-space: nowrap;
  /* padding: 0 16px; */
  font-weight: 400;
  font-size: 14px;
}

.button-red {
  border: 1px solid #ef5350;
  color: #ef5350;
  background: #fff;
}

.button-red:not(.disabled):hover {
  background: #fef1f1;
}

.button-red:not(.disabled):focus,
.button-red:not(.disabled):active {
  background: #d63734;
  color: #fff!important;
}

.button-blue {
  border: 1px solid #2979ff;
  color: #2979ff;
  background: #fff;
}

.button-blue.disabled {
  border: 1px solid #d2d2d2;
  background-color: #fff !important;
  color: #d2d2d2;
}

.button-blue:not(.disabled):hover{
  background: #eef4ff;
}

.button-blue:not(.disabled):focus,
.button-blue:not(.disabled):active {
  background: #0062cc;
  color: #fff!important;
}

.main-button {
  padding: 0;
  outline: none;
  color: #fff;
  background: #2979ff;
}

.main-button:not(.disabled):hover,
.main-button:not(.disabled):focus,
.main-button:not(.disabled):active {
  background: #1a73e8;
}

.main-button-red {
  padding: 0 16px;
  outline: none;
  color: #fff;
  background: #ef5350;
}

.main-button-red:not(.disabled):hover,
.main-button-red:not(.disabled):focus,
.main-button-red:not(.disabled):active {
  background: #e54340;
}

.main-button-red.disabled{
  background: #f7c3c1;
  cursor: no-drop;
}

.sub-button {
  border: 1px solid #e0e0e0;
  color: #000;
  background: #fff;
}

.sub-button.disabled {
  border: 1px solid #d2d2d2;
  background-color: #fff !important;
  color: #d2d2d2;
}

.sub-button:not(.disabled):hover {
  background: #f4f5f8;
}

.sub-button:not(.disabled):focus,
.sub-button:not(.disabled):active {
  background: #ebebeb;
}

</style>