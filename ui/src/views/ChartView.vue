<template>
    <div class="box">
        <div class="w-full d-flex">
            <div class="flex">
                <canvas id="chart-line" class="line-chart"></canvas>
                <canvas id="chart-bar" class="line-chart"></canvas>
            </div>
            <div class="flex">
                <div class="statistic flex-row pad-0-16">
                    <div class="flex mt-20">
                        <p>{{ resource.Label.TotalCapital }}: {{ totalCapital }}</p>
                    </div>
                    <div class="flex mt-20">
                        <p>{{ resource.Label.TotalRevenue }}: {{ totalRevenue }}</p>
                    </div>
                </div>

                <div class="pie-chart d-flex justify-content">
                    <canvas id="chart-pie"></canvas>
                </div>

            </div>
        </div>
    </div>
</template>
<script>
import Chart from 'chart.js/auto'
import Resource from '@/utils/resource';
export default {
    name: "ChartView",
    props: ['lineData', 'pieData'],   
    data() {
        return {
            resource: Resource,
        };
    },
    computed: {
        totalCapital: function () {
            let total = 0;
            this.lineData.datasets[0].data.forEach(element => {
                total += element;
            });
            return this.formatMoney(total);
        },
        totalRevenue: function () {
            let total = 0;
            this.lineData.datasets[1].data.forEach(element => {
                total += element;
            });
            return this.formatMoney(total);
        }
    },

    methods: {
        formatMoney(money) {
            if (!isNaN(money)) {
                var valueMoney = parseInt(money);
                return valueMoney.toString().replace(/(\d)(?=(\d{3})+(?:\.\d+)?$)/g, "$1.") + " VNĐ";
            } else {
                return valueMoney + " VNĐ";
            }
        },

        handleStatistic() {
            var displayLine = document.getElementById('chart-line');
            var displayBar = document.getElementById('chart-bar');
            var displayPie = document.getElementById('chart-pie');
            var lineChart = new Chart(displayLine, {
                type: 'line',
                data: this.lineData
            });
            var barChart = new Chart(displayBar, {
                type: 'bar',
                data: this.lineData,
            });
            var pieChart = new Chart(displayPie, {
                type: 'pie',
                data: this.pieData,
            });
            lineChart;
            barChart;
            pieChart
        },
    },
    created() {
        
    },
    mounted() {
        this.handleStatistic();
    },
}
</script>
<style scoped>
.box {
    padding: 12px 12px 0;
    background-color: #fff;
    box-shadow: 0 0 11px rgb(0 0 0 / 8%);
    position: relative;
    border-radius: 4px;
    width: 100%;
    max-height: 511px;
    overflow: auto;
}
.box::-webkit-scrollbar {
  width: 5px;
}

.box::-webkit-scrollbar-thumb {
  background: #cccccc;
  border-radius: 4px;
}
.w-full {
    width: 100%;
    height: 100%;
}
.pie-chart {
    height: 65%;
    padding-bottom: 20px;
}
.statistic {
    height: 15%;
    font-size: 16px;
    font-weight: 600;
    color: red;
}
</style>