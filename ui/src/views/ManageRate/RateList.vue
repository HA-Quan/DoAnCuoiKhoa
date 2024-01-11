<template>
    <BaseLoading v-if="isLoading" />
    <div v-else class="body flex-column " style="outline: none;" tabindex="0">
        <div class="title-box">{{ Resource.Label.ListRate }}</div>
        <div class="toolbar flex-row">
            <div class="toolbar-left flex-row">
                <div class="d-flex mr-10">
                    <BaseButton v-for="(item, index) in Const.StatusRate" :key="index" @click="updateStatus(item.Value)"
                        :class="['flex-row btn btn-tag', { 'selected': statusRate == item.Value }]">
                        <div class="text-button">{{ item.Label }}</div>
                    </BaseButton>
                </div>
            </div>
        </div>

        <div class="box list-order">
            <div class="table">
                <div class="order-item" v-for="(rate, index) in listRate" :key="index">
                    <a class="order-content w-100">
                        <div class="row-oneline">
                            <div class="order-detail d-flex flex-column">
                                <div class="d-flex">
                                    <div class="product-img" @click="goToProduct(rate.productID)">
                                        <div class="image">
                                            <img :src="`${Resource.PrefixImage + rate.productImage}`" alt="">
                                        </div>
                                    </div>
                                    <div class="product-info">
                                        <div>
                                            <p class="name-product">
                                                <span v-show="rate.condition != 2">
                                                    [{{ configCondition(rate.condition) }}]
                                                </span>
                                                <span>{{ rate.productName }}</span>
                                            </p>
                                        </div>
                                        <div class="mb-14 d-flex">
                                            <span class="fw-bold average-star">{{ rate.rateAverage.toFixed(1) }}</span>
                                            <StarRating :max-rating="5" :rating="rate.rateAverage" :read-only="true"
                                                :increment="0.1" :show-rating="false" :star-size="20" active-color="#ee4d2d"
                                                inactive-color="#fff">
                                            </StarRating>
                                            <span class="count-rate">{{ rate.rateCount + " " + Resource.Text.Rate }}</span>
                                        </div>
                                        <div class="mb-6 d-flex">
                                            <span class="fw-bold">{{ Resource.Label.RateBy }}:</span>
                                            <span>{{ rate.rateBy }}</span>
                                        </div>
                                        <div class="mb-6 d-flex">
                                            <span class="fw-bold">{{ Resource.Label.NumberStar }}:</span>
                                            <StarRating :max-rating="rate.numStar" :rating="rate.numStar" :read-only="true"
                                                :show-rating="false" :star-size="20" active-color="#ee4d2d"
                                                inactive-color="#fff">
                                            </StarRating>
                                        </div>
                                        <div class="mb-6 d-flex">
                                            <span class="fw-bold">{{ Resource.Label.RateDate }}:</span>
                                            <span>{{ convertTime(rate.rateDate) }}</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="info-order d-flex flex-column">
                                <div class="mb-10">
                                    <label for="" class="fw-bold">{{ Resource.Label.RateComment }}</label>
                                    <div class="flex-column rate-comment">
                                        <span style="margin-bottom: 6px;">{{ rate.comment }}</span>
                                        <div class="list-image d-flex">
                                            <a :data-fancybox="rate.rateID" style="position: relative;" class="mr-10" @click="showImg(rate.rateID)"
                                                :href="Resource.PrefixImage + item" v-for="(item, index) in rate.commentImage" :key="index">
                                                <img :src="Resource.PrefixImage + item" alt="">
                                            </a>
                                        </div>
                                    </div>
                                </div>
                                <div class="mb-10">
                                    <label for="" class="fw-bold">{{ Resource.Label.ReplyComment }}</label>
                                    <div class="reply-comment">
                                        <textarea :placeholder="Resource.Placehoder.ReplyComment" class="aria-form flex"
                                            rows="2" v-model.trim="rate.replyComment"
                                            :disabled="statusRate == Enum.StatusRate.Replyed">
                                        </textarea>
                                    </div>
                                    <div class="flex-row flex-end"
                                        v-show="statusRate == Enum.StatusRate.NotReply && rate.replyComment">
                                        <BaseButton @click="replyComment(rate.rateID, rate.replyComment)"
                                            class="main-button flex-row btn btn-send">
                                            <div class="text-button">{{ Resource.Button.SendReply }}</div>
                                            <div class="icon-send"></div>
                                        </BaseButton>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </a>
                </div>
            </div>
            <BasePaging :listSize="listSize" :pageSize="pageSize" :pageNow="pageNumber" :totalCount="totalCount"
                @setPageSize="setPageSize" @prePage="goToPrePage" @nextPage="goToNextPage" @updatePageNow="updatePageNow" />

        </div>

    </div>

    <BaseToastMessage v-show="toastMessage.isShowed" :class="`toast-${toastMessage.type} icon-toast-${toastMessage.type}`">

        <template #message>{{ toastMessage.message }}</template>
    </BaseToastMessage>
</template>
<script>
import { Fancybox } from "@fancyapps/ui";
import "@fancyapps/ui/dist/fancybox/fancybox.css";
import axios from 'axios';
import Const from '@/utils/const'
import Enum from '@/utils/enum'
import Resource from '@/utils/resource'
import BaseLoading from '@/components/base/BaseLoading.vue';
import BaseToastMessage from "../../components/base/BaseToastMessage.vue";
import BaseButton from "../../components/base/BaseButton.vue";
import BasePaging from '@/components/base/BasePaging.vue';
import CommonFn from '@/utils/commonFuncion';
import StarRating from 'vue-star-rating';
import moment from 'moment';
import 'moment/locale/vi'
export default {
    name: "RateList",
    components: {
        BaseLoading, BaseButton, BaseToastMessage, BasePaging, StarRating
    },
    data() {
        return {
            Resource: Resource,
            Enum: Enum,
            Const: Const,

            isLoading: false, // cờ điều khiển bật tắt trạng thái loading

            listRate: [], // danh sách đánh giá thi đua đang được hiển thị

            totalCount: 0, // tổng số đánh giá

            pageNumber: Enum.FirstPage, // trang đang hiển thị

            pageSize: Enum.PageSize, // số đánh giá hiển thị trong 1 trang

            toastMessage: { // cảnh báo
                message: '', // nội dung cảnh báo
                type: '', // loại cảnh báo
                isShowed: false, // cờ điều khiển bật tắt cảnh báo
            },

            listSize: Enum.ListSize, // danh sách gía trị số đánh giá hiển thị trong 1 trang

            statusRate: Enum.StatusRate.NotReply,

            accountID: '',

            countRateNotReply: 0,

        };
    },
    methods: {

        goToProduct(productID) {
            this.$router.push({ name: 'ProductDetailPage', query: { productId: productID } });
        },

        configCondition(value) {
            let result = value;
            Const.Condition.find((item) => {
                if (item.Value === value) {
                    result = item.Label;
                }
            });
            return result;
        },

        /**
         * Lấy dữ liệu đánh giá 
         * Author: HAQUAN(26/08/2023)
         */
        async getRateList() {
            try {
                this.isLoading = true;
                let url = `Rate/byStatus?status=${this.statusRate}&pageSize=${this.pageSize}&pageNumber=${this.pageNumber}`;
                await axios.get(url)

                    .then((response) => {
                        debugger
                        console.log(response);
                        this.setInitPage(response);
                    })

                    .catch((error) => {
                        console.log(error);
                        this.handleErrorResponse(error);
                    })

                    .finally(() => {
                        this.isLoading = false;
                        if (this.listRate.length == 0 && this.pageNumber > Enum.FirstPage) { // nếu không lấy được đánh giá nào thì
                            this.pageNumber = Enum.FirstPage;                // quay về trang đầu tiên
                            this.getRateList();
                        }
                    });
            } catch (error) {
                console.log(error);
            }
        },

        async getCountRateNotReply() {
            try {
                this.isLoading = true;
                let url = `Rate/countRateNotReply`;
                await axios.get(url)

                    .then((response) => {
                        this.CountRateNotReplyy = response.data;
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
         * Set dữ liệu mẫu lúc đầu của page
         * @param {*} value :  giá trị API trả về
         * Author: HAQUAN(28/10/2022)
         */
        setInitPage(value) {
            this.listRate = value.data.data;
            this.totalCount = value.data.totalCount;
        },

        /**
        * Hàm format giá trị tiền tệ
        * Author: HAQuan (28/08/2023)
        */
        formatMoney(money) {
            if (!isNaN(money)) {
                var valueMoney = parseInt(money);
                return valueMoney.toString().replace(/(\d)(?=(\d{3})+(?:\.\d+)?$)/g, "$1.");
            } else {
                return valueMoney;
            }
        },

        /**
         * Set giá trị pageSize
         * @param {int} value :  giá trị pageSize được chọn
         * Author: HAQUAN(26/10/2022) 
         */
        setPageSize(value) {
            this.pageSize = value;
        },

        /**
         * Chuyển đến trang trước
         * Author: HAQUAN(28/10/2022)
         */
        goToPrePage() {
            try {
                if (this.pageNumber > Enum.FirstPage) {
                    this.pageNumber--;
                    this.getRateList();
                }
            } catch (error) {
                console.log(error);
            }
        },

        /**
         * Chuyển đến trang tiếp theo
         * Author: HAQUAN(28/10/2022)
         */
        goToNextPage() {
            try {
                let totalPage = Math.ceil(this.totalCount / this.pageSize);
                if (this.pageNumber < totalPage) {
                    this.pageNumber++;
                    this.getRateList();
                }
            } catch (error) {
                console.log(error);
            }
        },

        /**
         * Hàm chuyển đến trang chỉ định
         * Author: HAQuan (09/08/2022)
         */
        updatePageNow(pageIsSelected) {
            this.pageNumber = pageIsSelected;
            this.getRateList();
        },

        async replyComment(rateID, content) {
            try {
                debugger
                let replyModel = {
                    rateId: rateID,
                    replyComment: content
                };
                this.isLoading = true;
                let url = `Rate/replyRate`
                await axios.post(url, replyModel).then((response) => {
                    console.log(response);
                    this.showToast(Resource.Message.CancelOrderSucces, Const.TypeToast.Success);
                    this.getData();
                })

                    .catch((error) => {
                        console.log(error);
                        this.handleErrorResponse(error);
                    })
                    .finally(() => {
                        this.isLoading = false;
                    });

            } catch (e) {
                console.log(e);
            }
        },

        updateStatus(value) {
            this.statusRate = value;
            this.getRateList();

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


        getData() {
            this.getCountRateNotReply();
            this.getRateList();
        },

        convertTime(value) {
            return moment(value).format('L');
        },

        showImg(rateID){
            Fancybox.bind(`[data-fancybox="${rateID}"]`, {
            // Your custom options
        });
        }
    },
    created() {
        this.accountID = this.$store.getters.user.accountID;
        this.getData();
        Fancybox.bind("[data-fancybox]", {
            // Your custom options
        });
        // Lấy danh sách đánh giá
        document.title = Resource.Title.Management;
    },

    watch: {
        pageSize(newValue) {
            if (newValue) {
                this.getRateList();
            }
        },
    },
}
</script>
<style scoped>
.title-box {
    margin: 0 0 16px;
    font-size: 20px;
    font-weight: 700;
    line-height: 35px;
}

.toolbar {
    margin: 0 0 16px;
    justify-content: space-between;
}

.btn-tag {
    width: 120px;
    outline: none;
    border-left: 1px solid #e0e0e0;
    color: #000;
    background: #fff;
}

.btn-tag .text-button {
    padding: 0 !important;
}

.icon-filter {
    background: url(../../assets/icon/filter.png);
    background-size: 24px 24px;
    height: 24px;
    width: 24px;
    position: relative;
    margin: 0 5px 0 12px;
}

.icon-filter.active {
    background: url(../../assets/icon/filter-active.png);
    background-size: 24px 24px;
}

.btn .text-button {
    display: inline-block;
    white-space: nowrap;
    padding: 0 16px 0 0;
    font-weight: 400;
    font-size: 14px;
}

.list-order {
    /* background-color: #fff; */
    position: relative;
    border-radius: 4px;
    /* width: 100%;
    height: 100%; */
}

.box {
    background-color: #fff;
    border-radius: 4px;
    box-shadow: 0 0 11px rgb(0 0 0 / 8%);
}

.empty-text {
    color: #9e9fab;
    display: flex;
    align-items: center;
    padding: 0 16px;
    height: 40px;
    position: absolute;
    width: 150px;
    white-space: nowrap;
}

.body {
    height: 100%;
}

[tooltip] {
    position: relative;
}

[tooltip]:hover::before,
[tooltip]:hover::after {
    display: block;
}

[tooltip]:after {
    width: fit-content;
    padding: 0 8px;
    background-color: #000;
    color: #fff;
    content: attr(tooltip);
    display: none;
    position: absolute;
    top: 5%;
    left: 0;
    z-index: 10;
    border-radius: 10px;
}

.w-100 {
    width: 100% !important;
}

.order-content {
    display: inline-block;
    padding: 10px;
    margin-bottom: 8px;
    background-color: #e8e8e8;
    border-radius: 0.3rem !important;
}

.row-oneline {
    display: flex;
    flex-wrap: wrap;
}

.info-order {
    padding: 0 10px;
    width: 50%;
    min-width: 400px;
}

.table {
    height: 506px;
    overflow: auto;
}

.table::-webkit-scrollbar {
    width: 10px;
    height: 10px;
    border-radius: 10px;
    background: #fff;
}

.table::-webkit-scrollbar:hover {
    cursor: pointer;

}

.table::-webkit-scrollbar-thumb {
    border-radius: 10px;
    background-color: #b5b5b5;
}

.table::-webkit-scrollbar-thumb:hover {
    background-color: #8f8f8f;
}

.mb-10 {
    margin-bottom: 10px;
}

.mb-14 {
    margin-bottom: 14px;
}

.mb-6 {
    margin-bottom: 6px;
}

.image img {
    width: 180px;
    height: auto;
}

.product-img {
    width: 200px;
    margin-right: 15px;
    cursor: pointer;
}

.fw-bold {
    font-weight: bold;
    margin-right: 5px;
}

.name-product {
    /* padding-bottom: 5px; */
    font-weight: 700;
    font-size: 18px;
}

.order-detail {
    width: 50%;
    max-width: calc(100% - 400px);
}

.list-product {
    background-color: #e8e8e8;
    border-radius: 6px;
    padding: 10px;
    margin-bottom: 5px;
}

.btn-cancel-order {
    width: 40%;
    background-color: #f3dbba;
    font-weight: 500;
    color: #000;
    cursor: pointer;
}

.btn-cancel-order:hover {
    background-color: #ff9300;
    color: #fff;
}

.btn-tag:hover {
    color: orangered;
    background-color: rgb(246, 242, 229);
}

.btn-tag.selected {
    color: orangered;
    border-bottom: 4px solid orangered;
}

.btn-buy-repeat {
    outline: 0;
    color: #fff;
    height: 48px;
    width: 90px;
    background: #ee4d2d;
    cursor: pointer;
}

.btn-buy-repeat:hover {
    background: #f05d40;
}

.btn-rate {
    margin-right: 10px;
    outline: 0;
    box-shadow: 0 1px 1px 0 rgba(0, 0, 0, .03);
    border: 1px solid #ee4d2d;
    color: #ee4d2d;
    height: 48px;
    width: 120px;
    background: rgba(255, 87, 34, .1);
    cursor: pointer;
}

.btn-rate:hover {
    background: rgba(255, 197, 178, .181);
}

.rate-comment {
    background-color: #fff;
    border-radius: 6px;
    padding: 8px;
    margin-top: 8px;
}

.reply-comment {
    background-color: #fff;
    border-radius: 6px;
    padding: 8px;
    margin-top: 8px;
    display: flex;
    margin-bottom: 8px;
}

.aria-form {
    border: none;
    outline: none;
}

.icon-send {
    background: transparent url(../../assets/icon/send.png);
    background-size: 24px 24px;
    height: 24px;
    width: 24px;
    position: relative;
    margin: 0 12px;
}

.btn-send .text-button {
    padding: 0 0 0 16px !important;
    font-weight: 600 !important;
}

.average-star {
    color: #ee4d2d;
    border-bottom: solid 2px #ee4d2d;
    font-size: 16px;
}

.count-rate {
    color: #767676;
    font-size: 16px;
    padding-left: 10px;
    border-left: solid 1px #767676;
    margin-left: 10px;
}
.list-image > a > img {
    width: 90px;
    height: 90px;
    cursor: pointer;
}
</style>
