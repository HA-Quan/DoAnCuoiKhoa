<template>
    <div class="footer">
        <div class="container">
            <div class="footer-box ">
                <div class="title">
                    {{ Resource.Title.SystemShop }}
                </div>
                <div class="box d-flex flex-wrap">
                    <div class="address" v-for="(item, index) in Const.ListAddress" :key="index">
                        <b>{{ Resource.NameWeb }}</b>
                        <span>{{ item.Label }}
                            <a :href="item.Link">{{ Resource.Text.SeeMap }}</a>
                        </span>

                    </div>
                </div>
            </div>

        </div>
    </div>
</template>
<script>
import Const from '@/utils/const';
import Resource from '@/utils/resource';
export default {
    name: "TheFooter",
    data() {
        return {
            Const: Const,
            Resource: Resource
        }
    },
}
</script>
<style scoped>
.footer {
    background: #fff;
    overflow: hidden;
    padding: 20px 0;
}

.footer-box {
    height: 410px;
    background-color: aliceblue;
}

.address {
    padding: 0 15px 0 0;
    margin-bottom: 20px;
    min-width: 270px;
}

b {
    display: block;
    font-size: 15px;
}

a {
    color: blue;
    display: block;
    font-size: 13px;
}

.title {
    font-size: 20px;
    padding: 24px 0 24px 24px;
    font-weight: 700;
    min-height: 24px;
}
.box{
    padding: 0 24px ;
}
</style>