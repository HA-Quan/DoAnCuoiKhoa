<template>
  <div class="toast-message">
    <div class="toast-content">
      <div>
        <div class="content-toast-message">
          <slot name="message"></slot>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: "BaseToastMessage",
};
</script>
<style scoped>
.toast-message {
  padding: 18px;
  border: 1px solid #cccccc;
  min-width: 400px;
  border-radius: 4px;
  background-size: 20px;
  background-repeat: no-repeat;
  background-position-y: center;
  background-position-x: 18px;
  box-shadow: rgba(0, 0, 0, 0.16) 0px 3px 6px;
  padding-left: 50px;
  padding-right: 8px;
  position: fixed;
  top: 10px;
  left: 50%;
  transform: translate(-50%);
  background-color: white;
  z-index: 11;
  height: 56px;
  min-height: unset;
  animation: animation 5s alternate;
}
.toast-close {
  width: 40px;
  height: 40px;
  margin-left: 40px;
}

.toast-content {
  display: flex;
  justify-content: space-between;
  align-items: center;
}
.toast-title {
  font-weight: bold;
}
.content-toast-message {
  font-size: 13px;
}

@keyframes animation {
  from {
    opacity: 1;
  }
  to {
    opacity: 0;
  }
  
}
.toast-success {
  border-left: 5px solid #03ae66;
}
.toast-fail {
  border-left: 5px solid #fa3939;
}
.toast-warning {
  border-left: 5px solid #f9b914;
}

.icon-toast-success {
  background-image: url(https://cegovapp.misacdn.net/r/cegov/img/toast-success.c555a628.svg);
}

</style>
