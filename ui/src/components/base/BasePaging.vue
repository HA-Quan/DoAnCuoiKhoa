<template>
    <div class="pagination flex-row">
        <div class="pagination-left" v-if="isManager">
            <span>{{ Resource.Text.Total }}: <strong>{{ totalCount }}</strong>
                {{ Resource.Text.Record }}
            </span>
        </div>
        <div class="pagination-mid flex-row align-items">
            <div @click="goToPrePage" :class="['move prev', { disabled: this.pageNow <= Enum.FirstPage },]">
            </div>
            <div class="paging-item" v-for="(indexPageNow, index) in configPaging" :key="index"
                :class="indexPageNow == this.pageNow ? 'active' : ''" @click="updatePageNow(indexPageNow)">
                {{ indexPageNow }}
            </div>

            <div @click="goToNextPage"
                :class="['move next', { disabled: this.pageNow >= Math.ceil(this.totalCount / this.pageSize) },]">
            </div>
        </div>

        <div class="pagination-right d-flex align-items" v-if="isManager">
            <span>{{ Resource.Text.RecordInPage }}</span>

            <div class="page-size">
                <BaseCombobox class="combobox-small" :isCheck="false" :hasIcon="true" :items="listSize"
                    :initItem="this.pageSize" @getValue="setPageSize" :fieldName="'value'" />
            </div>

            <span><strong>{{ totalCount ? (pageNow - 1) * pageSize + 1 : 0 }}</strong> - <strong>{{ pageNow *
                pageSize < totalCount ? pageNow * pageSize : totalCount }}</strong>
                        {{ Resource.Text.Record }}
            </span>
        </div>

    </div>
</template>
<script>
import Resource from '@/utils/resource';
import Enum from '@/utils/enum';
import BaseCombobox from './BaseCombobox.vue';
export default {
    name: "BasePaging",
    components: {
        BaseCombobox
    },  
    props: {
        listSize: {
            type: Array,
            default() {
                return [];
            }
        },

        pageSize: {
            type: Number
        },
        
        pageNow: {
            type: Number
        },

        totalCount: {
            type: Number
        },

        isManager: {
            type: Boolean,
            default: true
        }
    },
    data() {
        return {
            Resource: Resource,
            Enum: Enum,
            totalPage: 1,
            maxPageDisplay: 5,
            magicNumber: 4,
            pageIsSelected: 0, //chỉ số trang được chọn
        }
    },
    methods: {
        /**
         * Set giá trị pageSize
         * @param {int} value :  giá trị pageSize được chọn
         * Author: HAQUAN(26/10/2022) 
         */
        setPageSize(value) {
            this.$emit("setPageSize", value);
        },
        /**
         * Chuyển đến trang trước
         * Author: HAQUAN(28/10/2022)
         */
        goToPrePage() {
            this.$emit("prePage");
        },

        /**
         * Chuyển đến trang tiếp theo
         * Author: HAQUAN(28/10/2022)
         */
        goToNextPage() {
            this.$emit("nextPage");
        },
        /**
        * Hàm cập nhật paging 
        * Author: HAQuan(08/08/2022)
        */
        updatePageNow(indexPageNow) {
            if (indexPageNow != "...") {
                this.pageIsSelected = indexPageNow;
                this.$emit("updatePageNow", this.pageIsSelected);
            }
        },
    },
    computed: {
        /**
        * Hàm cấu hình paging
        * Author: HAQuan(08/08/2022)
        */
        configPaging() {
            if (this.totalPage <= this.maxPageDisplay) {
                //Nếu tổng số trang < 5 thì hiện tất cả
                let config = [];
                for (let i = 1; i <= this.totalPage; i++) {
                    config.push(i);
                }
                return config;
            }
            if (this.pageNow < this.magicNumber) {
                //Hiển thị 3 trang đầu và 2 trang cuối
                return [1, 2, 3, "...", this.totalPage - 1, this.totalPage];
            } else if (
                this.pageNow >= this.magicNumber &&
                this.pageNow <= this.totalPage - this.magicNumber + 1
            ) {
                //Hiển thị 2 trang đầu 2 trang cuối và trang hiện tại
                return [1, 2, "...", this.pageNow, "...", this.totalPage - 1, this.totalPage];
            } else if (this.pageNow >= this.totalPage - this.magicNumber) {
                //Hiển thị 2 trang đầu và 3 trang cuối 
                return [1, 2, "...", this.totalPage - 2, this.totalPage - 1, this.totalPage];
            }
            return [];
        },
    },
    created() {
        this.totalPage = Math.ceil(this.totalCount / this.pageSize);
    },
}
</script>
<style scoped>
.pagination {
    padding: 0 16px;
    height: 40px;
    min-height: 40px;
    border-bottom-left-radius: 10px;
    border-bottom-right-radius: 10px;
    align-items: center;
    justify-content: space-between;
}

.page-size {
    width: 85px;
    margin: 4px 16px;
    line-height: 32px;
}

.move {
    height: 36px;
    width: 36px;
    position: relative;
    cursor: pointer;
}

.move::before {
    content: "";
    position: absolute;
    background: transparent url(../../assets/icon/right-arrow.png);
    background-size: 24px 24px;
    height: 24px;
    width: 24px;
    top: 8px;
    left: 6px;
}

.move.prev {
    margin-left: 25px;
    margin-top: 4px;
    transform: rotate(-180deg);
    height: 36px;
    width: 36px;
    position: relative;
}

.move.prev.disabled,
.move.next.disabled {
    cursor: no-drop;
}

.move.disabled {
    opacity: .2;
}

.paging-item {
    width: 32px;
    height: 32px;
    background: #666;
    border-radius: 50%;
    color: #fff;
    line-height: 30px;
    text-align: center;
    margin: 0 5px;
}

.paging-item.active {
    background: #ff9300;
}

.paging-item:hover {
    cursor: pointer;
}
</style>