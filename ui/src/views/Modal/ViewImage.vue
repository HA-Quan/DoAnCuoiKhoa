<template>
    <div>
        <div class="view">
            <div class="icon-close" @click="focusout">

            </div>
            <div class="image flex-row align-items">
                <div class="icon-left" @click="preImage">

                </div>
                <img :src="link[index]" alt="" @keypress.left="preImage" @keypress.right="nextImage">

                <div class="icon-right" @click="nextImage">
                </div>
            </div>
        </div>


    </div>
</template>
<script>
import Resource from '@/utils/resource'
export default {
    name: "ViewImage",
    components: {
    },
    props: ['link'],
    data() {
        return {
            index: 0,
            Resource: Resource
        }
    },
    watch: {
        link: {
            handler: function () {
                this.index = 0;
            }
        },
    },
    methods: {
        focusout() {
            this.$emit("focusoutImage");
        },
        preImage() {
            if (this.index > 0) {
                this.index--;
            }
        },
        nextImage() {
            if (this.index < this.link.length - 1) {
                this.index++;
            }
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
    background: #fff;
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
    margin: 0 50px;
}

.icon-close {
    top: 30px;
    right: 30px;
    position: absolute;
    width: 24px;
    height: 24px;
    background-image: url(../../assets/icon/icon-x.png);
    background-size: 24px 24px;
}

.icon-left {
    width: 64px;
    height: 64px;
    background-image: url(../../assets/icon/arrow-right.png);
    transform: rotate(-180deg);
    background-size: 64px 64px;
    cursor: pointer;
}

.icon-right {
    width: 64px;
    height: 64px;
    background-image: url(../../assets/icon/arrow-right.png);
    background-size: 64px 64px;
    cursor: pointer;
}

.icon-close:hover {
    cursor: pointer;
}
</style>