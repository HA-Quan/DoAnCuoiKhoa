<template>
    <div class="box statistic flex-column space-between">
        <canvas id="chart-line" class="line-chart"></canvas>
        <canvas id="chart-bar" class="line-chart"></canvas>
        <canvas id="chart-pie" class="pie-chart"></canvas>
    </div>
</template>
<script>
import Chart from 'chart.js/auto'
export default {
    name: "ChartView",
    props: ['lineData', 'pieData'],   
    data() {
        return {
            
        };
    },
    methods: {
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
.statistic {
    background-color: #fff;
    position: relative;
    border-radius: 4px;
    width: 100%;
    height: 100%;
}

.box {
    background-color: #fff;
    border-radius: 4px;
    box-shadow: 0 0 11px rgb(0 0 0 / 8%);
}
</style>