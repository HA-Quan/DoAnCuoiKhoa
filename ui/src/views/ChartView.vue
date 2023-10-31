<template>
    <div class="box">
        <div class="w-full d-flex">
            <div class="flex">
                <canvas id="chart-line" class="line-chart"></canvas>
                <canvas id="chart-bar" class="line-chart"></canvas>
            </div>
            <div class="flex">
                <div class="statistic flex-column pad-0-16">
                    <div class="mt-20">
                        <p>{{ resource.Label.TotalCapital }}: {{ formatMoney(statisticModel.totalCapital) }}</p>
                    </div>
                    <div class="mt-20">
                        <p>{{ resource.Label.TotalRevenue }}: {{ formatMoney(statisticModel.totalRevenue) }}</p>
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
    props: ['statisticModel'],
    data() {
        return {
            resource: Resource,
            lineData: {
                labels: [],
                datasets: [
                    {
                        label: "Tiền vốn",
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
      handler: function () {
        this.handleStatistic(2);
      }
    },

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
        handleStatistic(value) {
            try {
                debugger
                this.lineData.labels = this.statisticModel.areaChart.map(i => i.label);
                this.lineData.datasets[0].data = this.statisticModel.areaChart.map(i => i.capital);
                this.lineData.datasets[1].data = this.statisticModel.areaChart.map(i => i.revenue);
                this.pieData.labels = this.statisticModel.pieChart.map(i => i.label);
                this.pieData.datasets[0].data = this.statisticModel.pieChart.map(i => i.value);

                var displayLine = document.getElementById('chart-line');
                var displayBar = document.getElementById('chart-bar');
                var displayPie = document.getElementById('chart-pie');
                if(value == 1){
                    this.lineChart = new Chart(displayLine, {
                    type: 'line',
                    data: this.lineData,
                });
                this.barChart = new Chart(displayBar, {
                    type: 'bar',
                    data: this.lineData,
                });
                this.pieChart = new Chart(displayPie, {
                    type: 'pie',
                    data: this.pieData,
                });
                }
                else {
                    this.lineChart.config._config.data = this.lineData;
                    // this.lineChart.update();
                    this.barChart.config._config.data = this.lineData;
                    // this.barChart.update();
                    this.pieChart.config._config.data = this.pieData;
                    // this.pieChart.update();
                }
                

                // if(this.lineChart==null){
                //     this.lineChart = new Chart(displayLine, {
                //     type: 'line',
                //     data: this.lineData
                // });
                // }
                // else{
                //     this.lineChart.destroyed();
                //     this.lineChart = new Chart(displayLine, {
                //     type: 'line',
                //     data: this.lineData
                // });
                // }

                // if(this.barChart==null){
                //     this.barChart = new Chart(displayBar, {
                //     type: 'bar',
                //     data: this.lineData
                // });
                // }
                // else{
                //     this.barChart.destroyed();
                //     this.barChart = new Chart(displayBar, {
                //     type: 'line',
                //     data: this.lineData
                // });
                // }

                // if(this.pieChart==null){
                //     this.pieChart = new Chart(displayLine, {
                //     type: 'line',
                //     data: this.pieData
                // });
                // }
                // else{
                //     this.pieChart.destroyed();
                //     this.pieChart = new Chart(displayPie, {
                //     type: 'pie',
                //     data: this.pieData
                // });
                // }
                
                // lineChart;
                // barChart;
                // pieChart;
            }
            catch (error) {
                console.log(error);
            }

        },
    },
    created() {
        
    },
    mounted() {
        this.handleStatistic(1);
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
    height: 100%;
    overflow: auto;
}

.w-full {
    width: 100%;
    height: 100%;
}

.pie-chart {
    height: 80%;
    padding-bottom: 20px;
}

.statistic {
    height: 20%;
    font-size: 16px;
    font-weight: 600;
    color: red;
}
</style>