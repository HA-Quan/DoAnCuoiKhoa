<template>
    <div>
        <div class="view" @click="focusout">

        </div>
        <div class="image">
            <img :src="link" alt="">
            <div class="icon-close" @click="focusout">

            </div>
        </div>

    </div>
</template>
<script>
export default {
    name: "ViewImage",
    components: {
    },
    props: ['link'],
    data() {
        return {

        }
    },
    methods: {
        focusout() {
            this.$emit("focusoutImage");
        }
    },
    created() {

    }
}
</script>
<style scoped>
.view {
    position: fixed;
    top: 0;
    left: 0;
    width: 100vw;
    height: 100vh;
    background: #000;
    z-index: 6;
}

.image {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    z-index: 7;
}

img {
    background-size: auto;
    max-height: 70vh;
}
.icon-close{
    top: 5px;
    right: 5px;
    position: absolute;
    width: 24px;
    height: 24px;
    background-image: url(../assets/icon/icon-x.png);
    background-size: 24px 24px;
}
.icon-close:hover{
    cursor: pointer;
}
</style>