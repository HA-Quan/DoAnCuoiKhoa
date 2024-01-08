<template>
    <div class="box">
        <div class="w-full flex-column">
            <div class="w-full flex-row">
                <div class="flex">
                    <canvas id="chart-line" class="line-chart"></canvas>
                    <canvas id="chart-bar" class="line-chart"></canvas>
                </div>
                <div class="flex">
                    <div class="statistic f-flex pad-0-16">
                        <div class="flex mt-10">
                            <p>{{ resource.Label.TotalCapital }}: {{ formatMoney(totalCapital) }}</p>
                        </div>
                        <div class="flex mt-10">
                            <p>{{ resource.Label.TotalRevenue }}: {{ formatMoney(totalRevenue) }}</p>
                        </div>
                        <div class="flex mt-10">
                            <p>{{ resource.Label.TotalProfit }}: {{ formatMoney(totalRevenue - totalCapital) }}</p>
                        </div>
                    </div>

                    <div class="pie-chart d-flex justify-content">
                        <canvas id="chart-pie"></canvas>
                    </div>

                </div>
            </div>

            <div class="w-full flex-row mt-20">
                <div class="flex flex-row">
                    <div class="flex">
                        <label for="">
                            {{ resource.Label.TopMember }}
                        </label>
                        <div class="table">
                            <table cellpadding="0" cellspacing="0">
                                <thead>
                                    <tr>
                                        <th class=" ">

                                        </th>
                                        <th class=" ">
                                            {{ resource.Label.Username }}
                                        </th>
                                        <th class=" ">
                                            {{ resource.Label.Total }}
                                        </th>

                                    </tr>
                                </thead>

                                <tbody v-if="data.topMembers != undefined && data.topMembers.length > 0">
                                    <tr v-for="(item, index) in data.topMembers" :key="index">
                                        <td>
                                            {{ index + 1 }}
                                        </td>

                                        <td>
                                            {{ item.username }}
                                        </td>

                                        <td>
                                            {{ formatMoney(item.totalBuy) }}
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="flex">
                        <label for="">
                            {{ resource.Label.TopStaff }}
                        </label>
                        <div class="table">
                            <table cellpadding="0" cellspacing="0">
                                <thead>
                                    <tr>
                                        <th class=" ">

                                        </th>
                                        <th class=" ">
                                            {{ resource.Label.Username }}
                                        </th>
                                        <th class=" ">
                                            {{ resource.Label.Total }}
                                        </th>

                                    </tr>
                                </thead>

                                <tbody v-if="data.topStaffs != undefined && data.topStaffs.length > 0">
                                    <tr v-for="(item, index) in data.topStaffs" :key="index">
                                        <td>
                                            {{ index + 1 }}
                                        </td>

                                        <td>
                                            {{ item.username }}
                                        </td>

                                        <td>
                                            {{ formatMoney(item.totalBuy) }}
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

                <div class="flex">
                    <label for="">
                        {{ resource.Label.TopProduct }}
                    </label>
                    <div class="table">
                        <table cellpadding="0" cellspacing="0">
                            <thead>
                                <tr>
                                    <th class=" ">

                                    </th>
                                    <th class=" ">
                                        {{ resource.Label.ProductName }}
                                    </th>
                                    <th class=" ">
                                        {{ resource.Label.Quantity }}
                                    </th>
                                    <th class=" ">
                                        {{ resource.Label.Total }}
                                    </th>
                                </tr>
                            </thead>

                            <tbody v-if="data.topProducts != undefined && data.topProducts.length > 0">
                                <tr v-for="(item, index) in data.topProducts" :key="index">
                                    <td>
                                        {{ index + 1}}
                                    </td>

                                    <td>
                                        {{ item.productName }}
                                    </td>

                                    <td>
                                        {{ item.amountBuy }}
                                    </td>

                                    <td>
                                        {{ formatMoney(item.total) }}
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
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
    props: ['statisticModel'],
    data() {
        return {
            resource: Resource,
            data: {},
            lineData: {
                labels: [],
                datasets: [
                    {
                        label: "Tiền hàng",
                        borderColor: "#FC2525",
                        pointBackgroundColor: "white",
                        borderWidth: 1,
                        pointBorderColor: "black",
                        backgroundColor: "rgba(255, 0, 0, 0.25)",
                        data: []
                    },
                    {
                        label: "Doanh thu",
                        borderColor: "#05CBE1",
                        pointBackgroundColor: "white",
                        pointBorderColor: "black",
                        borderWidth: 1,
                        backgroundColor: "rgba(0, 231, 255, 0.25)",
                        data: []
                    },
                    {
                        label: "Tiền lãi",
                        borderColor: "#0cf017",
                        pointBackgroundColor: "white",
                        pointBorderColor: "black",
                        borderWidth: 1,
                        backgroundColor: "rgba(0, 231, 255, 0.25)",
                        data: []
                    }
                ]
            },
            pieData: {
                labels: [],
                datasets: [
                    {
                        backgroundColor: ["#7db6ed", "#414146", "#92e9e2", "#f45b5b", "#e5d453", "#f15c81", "#8186ea", "#91ee7e"],
                        data: [40, 20, 10]
                    }
                ]
            },

            lineChart: null,
            barChart: null,
            pieChart: null
        };
    },
    watch: {
        statisticModel: {
            handler: function (newVal) {
                this.data = newVal;
                console.log(this.data);
                this.updateStatistic();
            }
        }
    },
    computed: {
        totalCapital: function () {
            let total = 0;
            this.lineData.datasets[0].data.forEach(element => {
                total += element;
            });
            return total;
        },
        totalRevenue: function () {
            let total = 0;
            this.lineData.datasets[1].data.forEach(element => {
                total += element;
            });
            return total;
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
            this.lineChart = new Chart(displayLine, {
                type: 'line',
                data: this.lineData
            });
            this.barChart = new Chart(displayBar, {
                type: 'bar',
                data: this.lineData,
            });
            this.pieChart = new Chart(displayPie, {
                type: 'pie',
                data: this.pieData,
            });
            // lineChart;
            // barChart;
            // pieChart
        },
        updateStatistic() {
            this.lineData.labels = this.data.areaChart.map(i => i.label);
            this.lineData.datasets[0].data = this.data.areaChart.map(i => i.capital);
            this.lineData.datasets[1].data = this.data.areaChart.map(i => i.revenue);
            this.lineData.datasets[2].data = this.data.areaChart.map(i => i.revenue - i.capital);
            this.pieData.labels = this.data.pieChart.map(i => i.label);
            this.pieData.datasets[0].data = this.data.pieChart.map(i => i.value);
        },
    },
    created() {
        this.data = this.statisticModel;
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
.table {
    position: relative;
    overflow: auto;
    border-bottom: 1px solid #ddd;
    height: 100%;
}

.table thead {
    position: sticky;
    top: 0;
    z-index: 3;
}

.table th {
    font-weight: 600;
    background: #fff;
    border-bottom: 1px solid #e0e1ef;
    border-right: 1px solid #e0e1ef;
    z-index: 2;
    height: 48px;
    text-align: left;
    padding: 0 12px;
    white-space: nowrap;
}

.table th:last-child {
    border-right: none;
}
.table td {
    line-height: calc(38/14);
    padding: 0 12px;
    height: 43px;
    vertical-align: middle;
    background-color: #fff;
    border-right: 1px solid #ddd;
    border-bottom: 1px solid #ddd;
    text-overflow: ellipsis;
    white-space: nowrap;
    overflow: hidden;
}

.table td:last-child {
    border-right: none;
}
label{
    padding: 8px 8px 0 0;
    color: #666;
    font-size: 14px;
    font-weight: 500;
}
</style>