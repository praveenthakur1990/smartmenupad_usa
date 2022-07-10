$(function () {
    $("#frmQRScannedData").validate({
        rules: {
            ddlMonth: {
                required: true
            },
            ddlYear: {
                required: true
            }
        }
    })
    ajaxindicatorstart(returnLoadingText())
    bindMonthGraphData(0, 0)
})

function getQRScannedData() {
    if ($("#frmQRScannedData").valid()) {
        bindMonthGraphData($("#ddlMonth").val(), $("#ddlYear").val());
    }
}

function bindMonthGraphData(monthId, year) {
    var categoriesArr = [], pickupArr = [], orderedArr = []; totalCountArr = []; totalSumPickedCount = 0, totalSumOrderedCount = 0;
    $.ajax({
        url: "/Dashboard/GetOrderGraphData",
        method: "GET",
        data: { 'monthId': monthId, 'year': year },
        success: function (result) {
            ajaxindicatorstop()
            if (result.data.length > 0) {
                for (var i = 0; i < result.data.length; i++) {
                    categoriesArr.push(result.data[i].OrderDate);
                    pickupArr.push(result.data[i].TotalPickUpCount);
                    orderedArr.push(result.data[i].TotalDeliveredCount);
                    totalCountArr.push(result.data[i].TotalCount);
                    //totalSumPickedCount += parseInt(result.data[i].TotalPickUpCount);
                    //totalSumOrderedCount += parseInt(result.data[i].TotalDeliveredCount);
                }
                Highcharts.chart('sales-chart', {
                    title: {
                        text: ''
                    },
                    xAxis: {
                        categories: categoriesArr
                    },
                    yAxis: {
                        title: {
                            text: 'Total Count'
                        }
                    },
                    labels: {
                        items: [{
                            html: '',
                            style: {
                                left: '50px',
                                top: '18px',
                                color: ( // theme
                                    Highcharts.defaultOptions.title.style &&
                                    Highcharts.defaultOptions.title.style.color
                                ) || 'black'
                            }
                        }]
                    },
                    series: [{
                        type: 'column',
                        name: 'Picked Up',
                        data: pickupArr
                    }, {
                        type: 'column',
                        name: 'Delivery',
                        data: orderedArr
                    }, {
                        type: 'column',
                        name: 'Total',
                        data: totalCountArr
                    },

                        //   {
                        //    type: 'pie',
                        //    name: 'Total Sales',
                        //    data: [{
                        //        name: 'Picked',
                        //        y: totalSumPickedCount,
                        //        color: Highcharts.getOptions().colors[0] // Jane's color
                        //    }, {
                        //        name: 'Ordered',
                        //        y: totalSumOrderedCount,
                        //        color: Highcharts.getOptions().colors[1] // John's color
                        //    }],
                        //    center: [100, 80],
                        //    size: 100,
                        //    showInLegend: false,
                        //    dataLabels: {
                        //        enabled: false
                        //    }
                        //}
                    ]
                });

                Highcharts.chart('paymentMode-pie-chart', {
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: 0,
                        plotShadow: false
                    },
                    title: {
                        text: 'Payment<br>Mode<br> month of ' + result.reportMonth,
                        align: 'center',
                        verticalAlign: 'middle',
                        y: 60
                    },
                    tooltip: {
                        pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                    },
                    accessibility: {
                        point: {
                            valueSuffix: ''
                        }
                    },
                    plotOptions: {
                        pie: {
                            dataLabels: {
                                enabled: true,
                                distance: -50,
                                style: {
                                    fontWeight: 'bold',
                                    color: 'white'
                                }
                            },
                            startAngle: -90,
                            endAngle: 90,
                            center: ['50%', '75%'],
                            size: '110%'
                        }
                    },
                    series: [{
                        type: 'pie',
                        name: 'Mode',
                        innerSize: '50%',
                        data: [
                            ['Cash Order', result.cashModeCounter],
                            ['Card Order', result.cardModeCounter]
                        ]
                    }]
                });
            }
        }
    });
}