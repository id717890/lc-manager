$(document).ready(function () {

	//first chart numbers on chart
	Chart.defaults.doughnutLabels = Chart.helpers.clone(Chart.defaults.doughnut);
	var helpers = Chart.helpers;
	var defaults = Chart.defaults;

	Chart.controllers.doughnutLabels = Chart.controllers.doughnut.extend({
		updateElement: function (arc, index, reset) {
			var _this = this;
			var chart = _this.chart,
				chartArea = chart.chartArea,
				opts = chart.options,
				animationOpts = opts.animation,
				arcOpts = opts.elements.arc,
				centerX = (chartArea.left + chartArea.right) / 2,
				centerY = (chartArea.top + chartArea.bottom) / 2,
				startAngle = opts.rotation, // non reset case handled later
				endAngle = opts.rotation, // non reset case handled later
				dataset = _this.getDataset(),
				circumference = reset && animationOpts.animateRotate ? 0 : arc.hidden ? 0 : _this.calculateCircumference(dataset.data[index]) * (opts.circumference / (2.0 * Math.PI)),
				innerRadius = reset && animationOpts.animateScale ? 0 : _this.innerRadius,
				outerRadius = reset && animationOpts.animateScale ? 0 : _this.outerRadius,
				custom = arc.custom || {},
				valueAtIndexOrDefault = helpers.getValueAtIndexOrDefault;

			helpers.extend(arc, {
				// Utility
				_datasetIndex: _this.index,
				_index: index,

				// Desired view properties
				_model: {
					x: centerX + chart.offsetX,
					y: centerY + chart.offsetY,
					startAngle: startAngle,
					endAngle: endAngle,
					circumference: circumference,
					outerRadius: outerRadius,
					innerRadius: innerRadius,
					label: valueAtIndexOrDefault(dataset.label, index, chart.data.labels[index])
				},

				draw: function () {
					var ctx = this._chart.ctx,
						vm = this._view,
						sA = vm.startAngle,
						eA = vm.endAngle,
						opts = this._chart.config.options;

					var labelPos = this.tooltipPosition();
					var segmentLabel = vm.circumference / opts.circumference * 100;

					ctx.beginPath();

					ctx.arc(vm.x, vm.y, vm.outerRadius, sA, eA);
					ctx.arc(vm.x, vm.y, vm.innerRadius, eA, sA, true);

					ctx.closePath();
					vm.borderWidth = 0;
					ctx.strokeStyle = vm.borderColor;
					ctx.lineWidth = vm.borderWidth;

					ctx.fillStyle = vm.backgroundColor;

					ctx.fill();
					ctx.lineJoin = 'bevel';

					if (vm.borderWidth) {
						ctx.stroke();
					}
					opts.defaultFontSize = 11;
					opts.defaultFontFamily = 'a_FuturicaBook';
					if (vm.circumference > 0.15) { // Trying to hide label when it doesn't fit in segment
						ctx.beginPath();
						ctx.font = helpers.fontString(opts.defaultFontSize, opts.defaultFontStyle, opts.defaultFontFamily);
						ctx.fillStyle = "#fff";
						ctx.textBaseline = "middle";
						ctx.textAlign = "center";

						// Round percentage in a way that it always adds up to 100%
						ctx.fillText(segmentLabel.toFixed(0) + "%", labelPos.x, labelPos.y);
					}
				}
			});

			var model = arc._model;
			model.backgroundColor = custom.backgroundColor ? custom.backgroundColor : valueAtIndexOrDefault(dataset.backgroundColor, index, arcOpts.backgroundColor);
			model.hoverBackgroundColor = custom.hoverBackgroundColor ? custom.hoverBackgroundColor : valueAtIndexOrDefault(dataset.hoverBackgroundColor, index, arcOpts.hoverBackgroundColor);
			model.borderWidth = custom.borderWidth ? custom.borderWidth : valueAtIndexOrDefault(dataset.borderWidth, index, arcOpts.borderWidth);
			model.borderColor = custom.borderColor ? custom.borderColor : valueAtIndexOrDefault(dataset.borderColor, index, arcOpts.borderColor);

			// Set correct angles if not resetting
			if (!reset || !animationOpts.animateRotate) {
				if (index === 0) {
					model.startAngle = opts.rotation;
				} else {
					model.startAngle = _this.getMeta().data[index - 1]._model.endAngle;
				}

				model.endAngle = model.startAngle + model.circumference;
			}

			arc.pivot();
		}
	});
	//first chart numbers on chart end
	var afterData = '';
	//styled tooltips three chart (first row)
	var customTooltips = function (tooltip) {

		var position = this._chart.canvas.getBoundingClientRect();
		// Display, position, and set styles for font
		tooltipEl.style.opacity = 1;
		tooltipEl.style.left = tooltip.caretX + 'px';
		tooltipEl.style.top = tooltip.caretY + 'px';
		tooltipEl.style.padding = tooltip.yPadding + 'px ' + tooltip.xPadding + 'px';
	};
	//styled tooltips three chart (first row) end
	var customTooltipsLine = function (tooltip) {
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

			titleLines.forEach(function (title) {
				//innerHtml += '<tr><th>' + title + '</th></tr>';
			});
			innerHtml += '</thead><tbody>';

			bodyLines.forEach(function (body, i) {
				var parts;
				var colors = tooltip.labelColors[i];

				parts = body[0];
				parts = parts.split(":");
				var newbody = '<span class="line-tool">' + parts[0].trim() + '</span> :  <span style="color:' + colors.backgroundColor + '">' + parts[1].trim() + '</span>';


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
	//cumstom for line graph

	// Show/hide chart by click legend
	updateDataset1 = function (e, datasetIndex) {

		var index = datasetIndex;
		var ci = e.view.weightChart1;
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
	// Show/hide chart by click legend //
	//line

	lineChartData = {

		labels: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"],
		datasets: [
			{
				label: "Покупки",
				borderColor: '#58AEDC',
				pointBackgroundColor: '#58AEDC',
				pointRadius: 2,
				backgroundColor: '#58AEDC',
				data: [650, 359, 290, 481, 156, 255, 640, 481, 156, 255, 640, 255],
				fill: false,
				borderWidth: 2,
			},
			{
				label: "Средний чек",
				borderColor: '#567BA5',
				pointBackgroundColor: '#567BA5',
				pointRadius: 2,
				backgroundColor: '#567BA5',
				data: [750, 500, 678, 300, 96, 270, 400, 381, 156, 255, 240, 378, 300],
				fill: false,
				borderWidth: 2,
			}, {
				label: "Начислено",
				borderColor: '#11B9A3',
				pointBackgroundColor: '#11B9A3',
				pointRadius: 2,
				backgroundColor: '#11B9A3',
				data: [600, 159, 190, 381, 156, 455, 340, 234, 111, 234, 687, 241, 234],
				fill: false,
				borderWidth: 2,
			},
			{
				label: "Списано",
				borderColor: '#E5C861',
				pointBackgroundColor: '#E5C861',
				pointRadius: 2,
				backgroundColor: '#E5C861',
				data: [150, 300, 690, 181, 656, 155, 440, 150, 300, 690, 181, 656, 155, 440],
				fill: false,
				borderWidth: 2,
			}
		]
	};

	var lineChartOptions = {
		responsive: true,
		maintainAspectRatio: false,
		tooltips: {
			enabled: false,
			mode: 'index',
			position: 'nearest',
			custom: customTooltipsLine,
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
				legendHtml.push('<td class="check-legend active" onclick="updateDataset1(event, ' + '\'' + chart.legend.legendItems[i].datasetIndex + '\'' + ')" ><div class="check-legend-line " style="background-color:' + chart.data.datasets[i].backgroundColor + ';"></div><div class="chart-legend-line" style="color:#383737;font-size:16rem;">');
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
	var ctx = document.getElementById("line-chart").getContext("2d");
	window.weightChart1 = new Chart(ctx, {
		type: 'line',
		data: lineChartData,
		options: lineChartOptions,
	});
	var aElem=document.querySelectorAll("#line-chart-leg");
for(i=0;i<aElem.length;i++){
   aElem[i].innerHTML = weightChart1.generateLegend();
}
});

