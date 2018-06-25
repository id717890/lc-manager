function legendRub(chart) {
    var text = [];
    afterData = ' руб.';

    text.push('<ul class="' + chart.id + '-leg">');
    for (var i = 0; i < chart.data.datasets[0].data.length; i++) {
        text.push('<li><span class="check-inside-chart" style="background-color:' + chart.data.datasets[0].backgroundColor[i] + '"></span>');
        if (chart.data.labels[i]) {
            if (chart.data.labels[i].indexOf('Стоимость') == -1) {
                text.push(chart.data.labels[i] + ': <span class="text-inside-chart">' + chart.data.datasets[0].data[i].toLocaleString('ru') + afterData + '</span>');
            } else {
                text.push(chart.data.labels[i] + ': <span class="text-inside-chart orange-text">' + chart.data.datasets[0].data[i].toLocaleString('ru') + afterData + '</span>');
            }

        }
        text.push('</li>');
    }
    text.push('</ul>');
    return text.join("");
}
function legendSht(chart) {
    var text = [];
    afterData = ' шт.';

    text.push('<ul class="' + chart.id + '-leg">');
    for (var i = 0; i < chart.data.datasets[0].data.length; i++) {
        text.push('<li><span class="check-inside-chart" style="background-color:' + chart.data.datasets[0].backgroundColor[i] + '"></span>');
        if (chart.data.labels[i]) {
            text.push(chart.data.labels[i] + ': <span class="text-inside-chart">' + chart.data.datasets[0].data[i].toLocaleString('ru') + afterData + '</span>');
        }
        text.push('</li>');
    }
    text.push('</ul>');
    return text.join("");
}
function legendChel(chart) {
    var text = [];
    afterData = ' чел.';

    text.push('<ul class="' + chart.id + '-leg">');
    for (var i = 0; i < chart.data.datasets[0].data.length; i++) {
        text.push('<li><span class="check-inside-chart" style="background-color:' + chart.data.datasets[0].backgroundColor[i] + '"></span>');
        if (chart.data.labels[i]) {
            text.push(chart.data.labels[i] + ': <span class="text-inside-chart">' + chart.data.datasets[0].data[i].toLocaleString('ru') + afterData + '</span>');
        }
        text.push('</li>');
    }
    text.push('</ul>');
    return text.join("");
}


updateDatasetTable = function (e, datasetIndex) {

    var index = datasetIndex;
    var ci = e.view.myLine;
    var meta = ci.getDatasetMeta(index);
    if (e.currentTarget.classList.contains('active')) {
        e.currentTarget.classList.remove('active');
    }
    else {
        e.currentTarget.classList.add('active');
    }
    // See controller.isDatasetVisible comment
    meta.hidden = meta.hidden === null ? !ci.data.datasets[index].hidden : null;

    // We hid a dataset ... rerender the chart
    ci.update();
};
//first chart numbers on chart end
var afterData = '';
//styled tooltips three chart (first row)
var customTooltips = function (tooltip) {
    // Tooltip Element
    var idAftreDate = this._chart.canvas.id;
    var classAftreDate = this._chart.canvas.className;
    var tooltipEl = document.getElementById('chartjs-tooltip-' + this._chart.id);
    if (!tooltipEl) {
        /*tooltipEl = document.createElement('div');
        tooltipEl.id = 'chartjs-tooltip';
        tooltipEl.innerHTML = "<table></table>"
        document.body.appendChild(tooltipEl);*/
    } else {
        tooltipEl.innerHTML = "<table></table>"
    }
    // Hide if no tooltip
    if (tooltip.opacity === 0) {
        tooltipEl.style.opacity = 0;
        return;
    }

    // Set caret Position
    tooltipEl.classList.remove('above', 'below', 'no-transform');
    if (tooltip.yAlign) {
        tooltipEl.classList.add(tooltip.yAlign);
    } else {
        tooltipEl.classList.add('no-transform');
    }

    function getBody(bodyItem) {
        return bodyItem.lines;
    }

    if (idAftreDate == 'left-pie') {
        afterData = ' чел.'
    }
    if (idAftreDate == 'right-pie') {
        afterData = ' руб.';
    }
    if (classAftreDate == "afterSht") {
        afterData = ' шт.';
    }


    // Set Text
    if (tooltip.body) {
        var titleLines = tooltip.title || [];
        var bodyLines = tooltip.body.map(getBody);

        var innerHtml = '<thead>';

        titleLines.forEach(function (title) {
            innerHtml += '<tr><th>' + title + '</th></tr>';
        });
        innerHtml += '</thead><tbody>';

        bodyLines.forEach(function (body, i) {
            var colors = tooltip.labelColors[i];
            var style = 'background:' + colors.backgroundColor;
            style += '; border-color:' + colors.borderColor;
            style += '; border-width: 2px';
            var span = '<span class="chartjs-tooltip-key" style="' + style + '"></span>';
            innerHtml += '<tr><td>' + span + body + afterData + '</td></tr>';
        });
        innerHtml += '</tbody>';

        var tableRoot = tooltipEl.querySelector('table');
        tableRoot.innerHTML = innerHtml;
    }

    var position = this._chart.canvas.getBoundingClientRect();
    // Display, position, and set styles for font
    tooltipEl.style.opacity = 1;
    tooltipEl.style.left = tooltip.caretX + 'px';
    tooltipEl.style.top = tooltip.caretY + 'px';
    tooltipEl.style.padding = tooltip.yPadding + 'px ' + tooltip.xPadding + 'px';
};

var tableTooltipsLine = function (tooltip) {
    // Tooltip Element
    var tooltipEl = document.getElementById('chartjs-tooltip-' + this._chart.id);

    if (!tooltipEl) {

    } else {
        tooltipEl.innerHTML = "<table></table>"
    }

    // Hide if no tooltip
    if (tooltip.opacity === 0) {
        tooltipEl.style.opacity = 0;
        return;
    }

    // Set caret Position
    tooltipEl.classList.remove('above', 'below', 'no-transform');
    if (tooltip.yAlign) {
        tooltipEl.classList.add(tooltip.yAlign);
    } else {
        tooltipEl.classList.add('no-transform');
    }

    function getBody(bodyItem) {
        return bodyItem.lines;
    }

    // Set Text
    if (tooltip.body) {
        var titleLines = tooltip.title || [];
        var bodyLines = tooltip.body.map(getBody);

        var innerHtml = '<thead>';
        //Title toolTipe
        /*titleLines.forEach(function (title) {
            innerHtml += '<tr><th>' + title + '</th></tr>';
        });*/
        innerHtml += '</thead><tbody>';

        bodyLines.forEach(function (body, i) {
            var parts;
            var colors = tooltip.labelColors[i];

            parts = body[0];
            parts = parts.split(":");
            var end = " б.";
            switch (parts[0]) {
				case "Покупки":
				case "Выручка":
                case "Средний чек": {
                    end = " руб.";
                    break;
                }
            }
            var newbody = '<span class="line-tool">' + parts[0].trim() + '</span> :  <span style="color:' + colors.backgroundColor + '">' + parts[1].trim().toLocaleString('ru') + end + '</span>';


            var style = 'background:' + colors.backgroundColor;
            style += '; border-color:' + colors.borderColor;
            style += '; border-width: 2px';
            innerHtml += '<tr><td>' + newbody + '</td></tr>';
        });
        innerHtml += '</tbody>';

        var tableRoot = tooltipEl.querySelector('table');
        tableRoot.innerHTML = innerHtml;
    }

    var position = this._chart.canvas.getBoundingClientRect();

    // Display, position, and set styles for font

    tooltipEl.style.opacity = 1;
    tooltipEl.style.left = tooltip.caretX + 'px';
    tooltipEl.style.top = tooltip.caretY + 'px';
    tooltipEl.style.fontFamily = 'a_Futurica';
    tooltipEl.style.fontSize = 15 + 'rem';
    tooltipEl.style.fontStyle = tooltip._fontStyle;
    tooltipEl.style.padding = 10 + 'px ' + 10 + 'px';
};

var lineChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    tooltips: {
        enabled: false,
        mode: 'index',
        position: 'nearest',
        custom: tableTooltipsLine,
    },
    elements: {
        line: {
            fill: false,
            tension: 0
        },

    },
    scales: {
        xAxes: [{
            ticks: {
                fontFamily: 'a_Futurica',
            }
        }],
        yAxes: [{
            ticks: {
                fontFamily: 'a_Futurica',
                maxTicksLimit: 5
            }
        }]
    },
    legendCallback: function (chart) {
        var legendHtml = [];
        legendHtml.push('<table>');
        legendHtml.push('<tr>');
        for (var i = 0; i < chart.data.datasets.length; i++) {
            legendHtml.push('<td class="check-legend active" onclick="updateDatasetTable(event, ' + '\'' + chart.legend.legendItems[i].datasetIndex + '\'' + ')" ><div class="check-legend-line " style="background-color:' + chart.data.datasets[i].backgroundColor + ';"></div><div class="chart-legend-line" style="color:#383737;font-size:16rem;">');
            if (chart.data.datasets[i].label) {
                legendHtml.push(chart.data.datasets[i].label + '</div></td>');
            }
        }
        legendHtml.push('</tr>');
        legendHtml.push('</table>');
        return legendHtml.join("");
    },
    legend: {
        display: false
    }
};


function drawChart() {

    if (typeDiagram === "line") {
        var ctx = document.getElementById("canvas").getContext("2d");
        window.myLine = Chart.Line(ctx, {
            type: 'line',
            data: lineChartData,
            options: lineChartOptions,
        });
        document.getElementById("line-chart-leg").innerHTML = myLine.generateLegend();
        $('.linejs-tooltip').attr('id', 'chartjs-tooltip-' + myLine.id);
    }

    if (typeDiagram === "two-pie") {

        var lctx = document.getElementById("left-pie").getContext("2d");
        var myChart0 = new Chart(lctx, leftPieConfig);
        document.getElementById('left-pie-leg').innerHTML = myChart0.generateLegend();
        $('#chartjs-tooltip-left').attr('id', 'chartjs-tooltip-' + myChart0.id);
        var rctx = document.getElementById("right-pie").getContext("2d");
        var myChart1 = new Chart(rctx, rightPieConfig);
        document.getElementById('right-pie-leg').innerHTML = myChart1.generateLegend();
        $('#chartjs-tooltip-right').attr('id', 'chartjs-tooltip-' + myChart1.id);
    }
}