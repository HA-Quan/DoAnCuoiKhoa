<template>
    <BaseLoading v-show="isLoading" />
    <div v-show="!isLoading" class="body flex-column " style="outline: none;">
        <div class="title-box">{{ Resource.Label.Statistic }}</div>

        <div class="toolbar flex-row">
            <div class="toolbar-left flex-row">
                <div class="row-box">
                    <div class="flex mt-10">
                        <BaseCombobox :hasIcon="false" :isReadonly="true" :items="listYear"
                            :initItem="configYear(yearStatistic)" fieldName="label" @getValue="setYear" />
                    </div>
                </div>
            </div>

            <div class="toolbar-right flex-row">
                <div class="flex-row">
                    <el-tooltip effect="dark" :content="Const.KeyStrokes.CtrlShiftO" placement="top">
                        <BaseButton @click="addAccount" class="main-button flex-row btn">
                            <div class="icon-sum"></div>
                            <div class="text-button">{{ Resource.Button.ExportFile }}</div>
                        </BaseButton>
                    </el-tooltip>
                </div>
            </div>
        </div>

        <div>
            <!-- <canvas id="chart-line" class="line-chart"></canvas>
            <canvas id="chart-bar" class="line-chart"></canvas>
            <canvas id="chart-pie" class="pie-chart"></canvas> -->
            <!-- <ChartView :lineData="lineData" :pieData="pieData"/> -->
            <!-- <ChartView :statisticModel="statisticModel" v-if="isShow"/> -->
            <ChartView :statisticModel="statisticModel"/>
        </div>

    </div>
</template>
<script>
import axios from 'axios';
import Resource from '@/utils/resource.js';
import Enum from '@/utils/enum.js';
import Const from '@/utils/const.js';
import BaseLoading from '@/components/base/BaseLoading.vue';
import BaseCombobox from "../components/base/BaseCombobox.vue";
import BaseButton from "../components/base/BaseButton.vue";
// import { Bar } from 'vue-chartjs'
// import { Chart as ChartJS, Title, Tooltip, Legend, BarElement ,PointElement, LineElement, CategoryScale, LinearScale } from 'chart.js'

// ChartJS.register(Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale, PointElement, LineElement)
// import Chart from 'chart.js/auto'
import ChartView from './ChartView.vue';
export default {
    name: "StatisticPage",
    components: {
        BaseLoading, BaseButton, BaseCombobox, ChartView
    },
    data() {
        return {
            Resource: Resource,
            Enum: Enum,
            Const: Const,
            isShow: true,
            isLoading: false, // cờ điều khiển bật tắt trạng thái loading
            listYear: [],
            yearStatistic: '',
            statisticModel: {},
            
        };
    },
    methods: {

        /**
         * Lấy danh sách các năm
         * Author: HAQUAN(26/08/2023)
         */
        async getListYear() {
            try {
                await axios.get('Statistic/year')
                    .then((response) => {
                        this.listYear = response.data;
                        this.yearStatistic = this.listYear[0].value;
                    })
                    .catch((error) => {
                        console.log(error);
                    })
            } catch (error) {
                console.log(error);
            }
        },

        /**
         * Lấy dữ liệu thống kê
         * Author: HAQUAN(26/08/2023)
         */
        async getData() {
            try {
                this.isLoading = true;
                await axios.get(`Statistic/chart?time=${this.yearStatistic}`)
                    .then((response) => {
                        this.statisticModel = response.data;
                        // this.handleStatistic();
                    })
                    .catch((error) => {
                        console.log(error);
                    })

                    .finally(() => {
                        this.isLoading = false;

                    });
            } catch (error) {
                console.log(error);
            }
        },

        // handleStatistic() {
        //     var displayLine = document.getElementById('chart-line');
        //     var displayBar = document.getElementById('chart-bar');
        //     var displayPie = document.getElementById('chart-pie');
            
        //     var lineChart = new Chart(displayLine, {
        //         type: 'line',
        //         data: this.lineData
        //     });
            
        //     var barChart = new Chart(displayBar, {
        //         type: 'bar',
        //         data: this.lineData,
        //     });
            
        //     var pieChart = new Chart(displayPie, {
        //         type: 'pie',
        //         data: this.pieData,
        //     });
        //     lineChart;
        //     barChart;
        //     pieChart
        // },

        async setYear(value) {
            this.isShow = false;
            await this.listYear.find((item) => {
                if (item.label == value) {
                    this.yearStatistic = item.value;
                    this.getData();
                }
            });
            this.isShow = true;
        },

        configYear(value) {
            let result = value;
            this.listYear.find((item) => {
                if (item.value === value) {
                    result = item.label;
                }
            });
            return result;
        },

    },
    async created() {
        document.title = Resource.Title.Management;
        // Lấy danh sách các năm
        await this.getListYear();

    },
     mounted() {
        
    },
    // watch: {
    //     yearStatistic() {
    //         this.getData();
    //     }
    // }

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

.toolbar-left {
    position: relative;
}

.btn .text-button {
    display: inline-block;
    white-space: nowrap;
    padding: 0 16px 0 0;
    font-weight: 400;
    font-size: 14px;
}

.icon-sum {
    background: transparent url(../assets/icon/icon-sum.png);
    background-size: 24px 24px;
    height: 24px;
    width: 24px;
    position: relative;
    margin: 0 5px 0 12px;
}

.box {
    background-color: #fff;
    box-shadow: 0 0 11px rgb(0 0 0 / 8%);
    position: relative;
    border-radius: 4px;
    width: 100%;
    height: 100%;
}

</style>