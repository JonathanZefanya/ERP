// Check if Highcharts is already loaded
if (typeof Highcharts === 'undefined') {
    console.error('Highcharts library is not loaded!');
} else {
    initializeCharts();
}

function initializeCharts() {
    // PIE Chart
    $(document).ready(function () {
        loadPieChart();
    });

    // BAR Chart
    $(document).ready(function () {
        loadBarChart();
    });
}

function loadPieChart() {
    var titleMessage = "Highest Earning by Item Total: ";
    $.ajax({
        type: "GET",
        url: "/Dashboard/GetHighestEarningByItem",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            var keys = Object.keys(result);
            var weeklydata = new Array();
            var totalspent = 0.0;
            for (var i = 0; i < keys.length; i++) {
                var arrL = new Array();
                arrL.push(keys[i]);
                arrL.push(result[keys[i]]);
                totalspent += result[keys[i]];
                weeklydata.push(arrL);
            }
            createPIECharts(weeklydata, titleMessage, totalspent.toFixed(2));
        },
        error: function (xhr, status, error) {
            console.error("Error loading pie chart data:", error);
        }
    });
}

function loadBarChart() {
    var titleMessage = "Monthly Sales Amount in last 12 months is: ";
    $.ajax({
        type: "GET",
        url: "/Dashboard/GetMonthlySales",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            var keys = Object.keys(result);
            var weeklydata = new Array();
            var totalspent = 0.0;
            for (var i = 0; i < keys.length; i++) {
                var arrL = new Array();
                arrL.push(keys[i]);
                arrL.push(result[keys[i]]);
                totalspent += result[keys[i]];
                weeklydata.push(arrL);
            }
            createBarCharts(weeklydata, titleMessage, totalspent.toFixed(2));
        },
        error: function (xhr, status, error) {
            console.error("Error loading bar chart data:", error);
        }
    });
}

function createPIECharts(sum, titleText, totalspent) {
    if ($('#containerHighestEarning').length === 0) {
        console.error("Container 'containerHighestEarning' not found");
        return;
    }

    Highcharts.chart('containerHighestEarning', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie',
            backgroundColor: 'transparent',
            height: 600, // Increased height
            style: {
                fontFamily: "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif"
            }
        },
        title: {
            text: titleText + ' ' + totalspent,
            style: {
                fontSize: '20px',
                fontWeight: '700',
                color: '#2c3e50'
            }
        },
        tooltip: {
            pointFormat: '<b>{point.name}</b><br/>' +
                        'Amount: <b>{point.y:.2f}</b><br/>' +
                        'Percentage: <b>{point.percentage:.1f}%</b>',
            backgroundColor: 'rgba(255, 255, 255, 0.95)',
            borderWidth: 2,
            borderColor: '#667eea',
            borderRadius: 10,
            shadow: true,
            style: {
                fontSize: '13px'
            }
        },
        accessibility: {
            point: {
                valueSuffix: '%'
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                size: '85%', // Larger pie chart
                innerSize: '40%', // Donut style
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b><br/>{point.percentage:.1f}%',
                    distance: 20,
                    style: {
                        fontSize: '13px',
                        textOutline: 'none',
                        fontWeight: '600'
                    },
                    connectorColor: '#999',
                    connectorWidth: 2
                },
                showInLegend: true,
                colors: [
                    '#667eea', '#764ba2', '#f093fb', '#4facfe',
                    '#43e97b', '#fa709a', '#fee140', '#30cfd0',
                    '#a8edea', '#fed6e3', '#c471ed', '#12c2e9',
                    '#ff6b6b', '#4ecdc4', '#45b7d1', '#96ceb4'
                ],
                states: {
                    hover: {
                        brightness: 0.1,
                        halo: {
                            size: 10
                        }
                    }
                }
            }
        },
        legend: {
            align: 'right',
            verticalAlign: 'middle',
            layout: 'vertical',
            itemStyle: {
                fontSize: '13px',
                fontWeight: '600'
            },
            itemMarginBottom: 10,
            symbolRadius: 8,
            symbolHeight: 14,
            symbolWidth: 14
        },
        series: [{
            name: 'Earning',
            colorByPoint: true,
            data: sum
        }],
        credits: {
            enabled: false
        }
    });
}

function createBarCharts(sum, titleText, totalspent) {
    if ($('#containerMonthlySales').length === 0) {
        console.error("Container 'containerMonthlySales' not found");
        return;
    }

    Highcharts.chart('containerMonthlySales', {
        chart: {
            type: 'column',
            backgroundColor: 'transparent',
            height: 450,
            style: {
                fontFamily: "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif"
            }
        },
        title: {
            text: titleText + ' ' + totalspent,
            style: {
                fontSize: '20px',
                fontWeight: '700',
                color: '#2c3e50'
            }
        },
        xAxis: {
            type: 'category',
            labels: {
                rotation: -45,
                style: {
                    fontSize: '12px',
                    fontWeight: '600'
                }
            },
            lineColor: '#e0e0e0',
            tickColor: '#e0e0e0'
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Sales Amount',
                style: {
                    fontSize: '14px',
                    fontWeight: '700'
                }
            },
            gridLineColor: '#f0f0f0'
        },
        legend: {
            enabled: false
        },
        tooltip: {
            pointFormat: '<b>Total Sales: {point.y:.2f}</b>',
            backgroundColor: 'rgba(255, 255, 255, 0.95)',
            borderWidth: 2,
            borderColor: '#667eea',
            borderRadius: 10,
            shadow: true,
            style: {
                fontSize: '13px'
            }
        },
        plotOptions: {
            column: {
                borderRadius: 8,
                dataLabels: {
                    enabled: false
                },
                colorByPoint: true,
                colors: [
                    '#667eea', '#764ba2', '#f093fb', '#4facfe',
                    '#43e97b', '#fa709a', '#fee140', '#30cfd0',
                    '#a8edea', '#fed6e3', '#c471ed', '#12c2e9'
                ],
                states: {
                    hover: {
                        brightness: 0.1
                    }
                }
            }
        },
        series: [{
            name: 'Monthly Sales',
            type: 'column',
            data: sum
        }],
        credits: {
            enabled: false
        }
    });
}
