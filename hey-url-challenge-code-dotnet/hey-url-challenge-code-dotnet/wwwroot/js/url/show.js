﻿google.charts.load('current', { packages: ['corechart'] });
google.charts.setOnLoadCallback(drawCharts);
function drawTotalClicksChart() {
    var data = google.visualization.arrayToDataTable([
        ['day', 'clicks'],
    ].concat(window.pages.url.show.dailyClicks));
    var options = {
        title: 'total clicks',
        haxis: {
            title: 'day of month'
        },
        vaxis: {
            title: 'clicks'
        }
    };
    var chart = new google.visualization.AreaChart(
        document.getElementById('total-clicks-chart')
    );
    chart.draw(data, options);
}
function drawBrowsersChart() {
    var data = google.visualization.arrayToDataTable([
        ['browser', 'clicks'],
    ].concat(window.pages.url.show.browserClicks));
    var options = {
        title: 'Browsers'
    };
    var chart = new google.visualization.PieChart(
        document.getElementById('browsers-chart')
    );
    chart.draw(data, options);
}
function drawPlatformsChart() {
    var data = google.visualization.arrayToDataTable([
        ['platform', 'clicks'],
    ].concat(window.pages.url.show.platformClicks));
    var options = {
        title: 'Platform'
    };
    var chart = new google.visualization.PieChart(
        document.getElementById('platforms-chart')
    );
    chart.draw(data, options);
}
function drawCharts() {
    drawTotalClicksChart();
    drawBrowsersChart();
    drawPlatformsChart();
}