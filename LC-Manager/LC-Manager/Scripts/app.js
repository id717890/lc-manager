$(document).ready(function () {


	function eventsFunc() {

		if ($(window).width() <= 1200) {
			$('.aside').addClass('small');
		}

		$(".check-legend").click(function () {

			if ($(this).hasClass('active')) {
				$(this).removeClass('active');
			} else {
				$(this).addClass('active');
			}
		});

		$(".btn-hide-aside").click(function (e) {
			e.preventDefault();
			if ($('.aside').hasClass('small')) {
				$('.aside').removeClass('small');
			} else {
				$('.aside').addClass('small');
			}
		});


		function searchUI() {
			var placeholderItem = "";
			var activeSelector;
			$(".search-form input").focusin(function () {
				activeSelector = $(this).closest('.search-form');
				activeSelector.addClass('active');
				placeholderItem = $(this).attr('placeholder');
				$(this).attr('placeholder', '');

			});
			$(".search-form input").focusout(function () {
				activeSelector = $(this).closest('.search-form');
				activeSelector.removeClass('active');
				$(this).attr('placeholder', placeholderItem);

			})
		}


		function tabsUI() {
			$(".tabs__links a").click(function (e) {
				e.preventDefault();
				var tab = $(".tabs__content > div");
				var thisTab = $(this).attr('href');

				tab.removeClass('active');
				$(thisTab).addClass('active');

				$(".tabs__links li").removeClass('active');
				$(this).closest('li').addClass('active');
			})
		}
		function tabsFormUI() {
			$(".form-terminal__tabs-link a").click(function (e) {
				e.preventDefault();
				var tab = $(".form-terminal__tabs-content > div");
				var thisTab = $(this).attr('href');

				tab.removeClass('active');
				$(thisTab).addClass('active');

				$(".form-terminal__tabs-link li a").removeClass('active');
				$(this).addClass('active');
			})
		}
		searchUI();
		tabsUI();
		tabsFormUI();
		$('.select').niceSelect();


	}
	eventsFunc()
	//setTimeout(eventsFunc,2000);


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
		// Tooltip Element
		var idAftreDate = this._chart.canvas.id;
		var tooltipEl = document.getElementById('chartjs-tooltip-' + this._chart.id);
		if (!tooltipEl) {
			tooltipEl = document.createElement('div');
			tooltipEl.id = 'chartjs-tooltip';
			tooltipEl.innerHTML = "<table></table>"
			document.body.appendChild(tooltipEl);
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

		if (idAftreDate == 'structura-of-client-base') {
			afterData = ' чел.'
		}
		if (idAftreDate == 'active-of-client-base') {
			afterData = ' руб.';
		}

		if (idAftreDate == 'structura-of-client-base2') {
			afterData = ' чел.'
		}
		if (idAftreDate == 'active-of-client-base2') {
			afterData = ' руб.';
		}

		if (idAftreDate == 'structura-of-client-base3') {
			afterData = ' чел.'
		}
		if (idAftreDate == 'active-of-client-base3') {
			afterData = ' руб.';
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
	updateDataset2 = function (e, datasetIndex) {

		var index = datasetIndex;
		var ci = e.view.weightChart2;
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
	updateDataset3 = function (e, datasetIndex) {

		var index = datasetIndex;
		var ci = e.view.weightChart3;
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

	var weightChartVar;




	//сегментаци по возрасту#0
	var config = {
		type: 'doughnutLabels',
		data: {
			labels: [
				" До 25 лет",
				" 25-25 лет",
				" 35-45 лет",
				" 45+ лет",
				" Не указано",
			],
			datasets: [{
				data: [
					20,
					20,
					35,
					15,
					10,
				],
				backgroundColor: [
					"#11B9A3",
					"#E5C861",
					"#58AEDC",
					"#D4614A",
					"#373636",
				],
			}],
		},
		options: {
			responsive: true,
			legend: false,
			segmentShowStroke: true,
			animation: {
				animateScale: true,
				animateRotate: true
			},
			tooltips: {
				mydata: 122,
				enabled: false,
				mode: 'index',
				position: 'nearest',
				custom: customTooltips
			},
			cutoutPercentage: 55,

		}
	};
	var ctx = document.getElementById("segmentation-of-years").getContext("2d");
	var myChart0 = new Chart(ctx, config);
	document.getElementById('segmentation-of-years-legend').innerHTML = myChart0.generateLegend();
	//сегментаци по возрасту//

	//структура клиентской базы#2
	var config = {
		type: 'doughnut',
		data: {
			labels: [
				" Мужчины",
				" Женщины",
				" Пол не указан",
				" С покупками",
				" Без покупок",
			],
			datasets: [{
				data: [
					3000,
					3000,
					6000,
					6000,
					5546,
				],
				backgroundColor: [
					"#11B9A3",
					"#E5C861",
					"#58AEDC",
					"#D4614A",
					"#373636",
				],
				borderWidth: 0,
			}],
		},
		options: {
			responsive: true,
			legend: false,
			legendCallback: function (chart) {
				var text = [];
				afterData = ' чел.';
				text.push('<ul class="' + chart.id + '-legend">');
				for (var i = 0; i < chart.data.datasets[0].data.length; i++) {
					text.push('<li><span class="check-inside-chart" style="background-color:' + chart.data.datasets[0].backgroundColor[i] + '"></span>');
					if (chart.data.labels[i]) {
						text.push(chart.data.labels[i] + ': <span class="text-inside-chart">' + chart.data.datasets[0].data[i] + afterData + '</span>');

					}
					text.push('</li>');
				}
				text.push('</ul>');
				return text.join("");
			},
			segmentShowStroke: true,
			animation: {
				animateScale: true,
				animateRotate: true
			},
			tooltips: {
				enabled: false,
				mode: 'index',
				position: 'nearest',
				custom: customTooltips
			},
			cutoutPercentage: 55,
			tooltipTemplate: "<%= value %>%",
		}
	};
	var ctx = document.getElementById("structura-of-client-base").getContext("2d");
	var myChart1 = new Chart(ctx, config);
	document.getElementById('structura-of-client-base-legend').innerHTML = myChart1.generateLegend();
	//структура клиентской базы//

	//АКТИВНОСТЬ КЛИЕНТСКОЙ БАЗЫ#3
	var afterData = '';
	var config = {
		type: 'doughnut',
		data: {
			labels: [
				" Мужчины",
				" Женщины",
				" Пол не указан",
				" Повторные",
				" Покупок на клиента",
			],
			datasets: [{
				data: [
					1500000,
					1500000,
					150000,
					1500000,
					150000,
				],
				backgroundColor: [
					"#11B9A3",
					"#E5C861",
					"#58AEDC",
					"#D4614A",
					"#373636",
				],
				borderWidth: 0,
			}],
		},
		options: {
			responsive: true,
			legend: false,
			legendCallback: function (chart) {
				var text = [];
				afterData = ' руб.';

				text.push('<ul class="' + chart.id + '-legend">');
				for (var i = 0; i < chart.data.datasets[0].data.length; i++) {
					text.push('<li><span class="check-inside-chart" style="background-color:' + chart.data.datasets[0].backgroundColor[i] + '"></span>');
					if (chart.data.labels[i]) {
						text.push(chart.data.labels[i] + ': <span class="text-inside-chart">' + chart.data.datasets[0].data[i] + afterData + '</span>');
					}
					text.push('</li>');
				}
				text.push('</ul>');
				return text.join("");
			},
			segmentShowStroke: true,
			animation: {
				animateScale: true,
				animateRotate: true
			},
			tooltips: {
				enabled: false,
				mode: 'index',
				position: 'nearest',
				custom: customTooltips
			},
			cutoutPercentage: 55,
			tooltipTemplate: "<%= value %>%",
		}
	};
	var ctx = document.getElementById("active-of-client-base").getContext("2d");
	var myChart2 = new Chart(ctx, config);
	document.getElementById('active-of-client-base-legend').innerHTML = myChart2.generateLegend();
	//АКТИВНОСТЬ КЛИЕНТСКОЙ БАЗЫ//

	//line


	lineChartData = {

		labels: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"],
		datasets: [
			{
				label: "Выручка",
				borderColor: '#58AEDC',
				pointBackgroundColor: '#58AEDC',
				pointRadius: 2,
				backgroundColor: '#58AEDC',
				data: [650, 359, 290, 481, 156, 255, 640, 481, 156, 255, 640, 255],
				fill: false,
				borderWidth: 2,
			},
			{
				label: "Клиенты",
				borderColor: '#567BA5',
				pointBackgroundColor: '#567BA5',
				pointRadius: 2,
				backgroundColor: '#567BA5',
				data: [750, 500, 678, 300, 96, 270, 400, 381, 156, 255, 240, 378, 300],
				fill: false,
				borderWidth: 2,
			}, {
				label: "Средний чек",
				borderColor: '#11B9A3',
				pointBackgroundColor: '#11B9A3',
				pointRadius: 2,
				backgroundColor: '#11B9A3',
				data: [600, 159, 190, 381, 156, 455, 340, 234, 111, 234, 687, 241, 234],
				fill: false,
				borderWidth: 2,
			},
			{
				label: "Средний % скидки",
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
	document.getElementById("line-chart-legend").innerHTML = weightChart1.generateLegend();


	//конец графика лайн

	//сегментаци по возрасту#0
	var config = {
		type: 'doughnutLabels',
		data: {
			labels: [
				" До 25 лет",
				" 25-25 лет",
				" 35-45 лет",
				" 45+ лет",
				" Не указано",
			],
			datasets: [{
				data: [
					20,
					20,
					35,
					15,
					10,
				],
				backgroundColor: [
					"#11B9A3",
					"#E5C861",
					"#58AEDC",
					"#D4614A",
					"#373636",
				],
			}],
		},
		options: {
			responsive: true,
			legend: false,
			segmentShowStroke: true,
			animation: {
				animateScale: true,
				animateRotate: true
			},
			tooltips: {
				mydata: 122,
				enabled: false,
				mode: 'index',
				position: 'nearest',
				custom: customTooltips
			},
			cutoutPercentage: 55,

		}
	};
	var ctx = document.getElementById("segmentation-of-years2").getContext("2d");
	var myChart0 = new Chart(ctx, config);
	document.getElementById('segmentation-of-years2-legend').innerHTML = myChart0.generateLegend();
	//сегментаци по возрасту//

	//структура клиентской базы#2
	var config = {
		type: 'doughnut',
		data: {
			labels: [
				" Мужчины",
				" Женщины",
				" Пол не указан",
				" С покупками",
				" Без покупок",
			],
			datasets: [{
				data: [
					3000,
					3000,
					6000,
					6000,
					5546,
				],
				backgroundColor: [
					"#11B9A3",
					"#E5C861",
					"#58AEDC",
					"#D4614A",
					"#373636",
				],
				borderWidth: 0,
			}],
		},
		options: {
			responsive: true,
			legend: false,
			legendCallback: function (chart) {
				var text = [];
				afterData = ' чел.';
				text.push('<ul class="' + chart.id + '-legend">');
				for (var i = 0; i < chart.data.datasets[0].data.length; i++) {
					text.push('<li><span class="check-inside-chart" style="background-color:' + chart.data.datasets[0].backgroundColor[i] + '"></span>');
					if (chart.data.labels[i]) {
						text.push(chart.data.labels[i] + ': <span class="text-inside-chart">' + chart.data.datasets[0].data[i] + afterData + '</span>');

					}
					text.push('</li>');
				}
				text.push('</ul>');
				return text.join("");
			},
			segmentShowStroke: true,
			animation: {
				animateScale: true,
				animateRotate: true
			},
			tooltips: {
				enabled: false,
				mode: 'index',
				position: 'nearest',
				custom: customTooltips
			},
			cutoutPercentage: 55,
			tooltipTemplate: "<%= value %>%",
		}
	};
	var ctx = document.getElementById("structura-of-client-base2").getContext("2d");
	var myChart1 = new Chart(ctx, config);
	document.getElementById('structura-of-client-base2-legend').innerHTML = myChart1.generateLegend();
	//структура клиентской базы//

	//АКТИВНОСТЬ КЛИЕНТСКОЙ БАЗЫ#3
	var afterData = '';
	var config = {
		type: 'doughnut',
		data: {
			labels: [
				" Мужчины",
				" Женщины",
				" Пол не указан",
				" Повторные",
				" Покупок на клиента",
			],
			datasets: [{
				data: [
					1500000,
					1500000,
					150000,
					1500000,
					150000,
				],
				backgroundColor: [
					"#11B9A3",
					"#E5C861",
					"#58AEDC",
					"#D4614A",
					"#373636",
				],
				borderWidth: 0,
			}],
		},
		options: {
			responsive: true,
			legend: false,
			legendCallback: function (chart) {
				var text = [];
				afterData = ' руб.';

				text.push('<ul class="' + chart.id + '-legend">');
				for (var i = 0; i < chart.data.datasets[0].data.length; i++) {
					text.push('<li><span class="check-inside-chart" style="background-color:' + chart.data.datasets[0].backgroundColor[i] + '"></span>');
					if (chart.data.labels[i]) {
						text.push(chart.data.labels[i] + ': <span class="text-inside-chart">' + chart.data.datasets[0].data[i] + afterData + '</span>');
					}
					text.push('</li>');
				}
				text.push('</ul>');
				return text.join("");
			},
			segmentShowStroke: true,
			animation: {
				animateScale: true,
				animateRotate: true
			},
			tooltips: {
				enabled: false,
				mode: 'index',
				position: 'nearest',
				custom: customTooltips
			},
			cutoutPercentage: 55,
			tooltipTemplate: "<%= value %>%",
		}
	};
	var ctx = document.getElementById("active-of-client-base2").getContext("2d");
	var myChart2 = new Chart(ctx, config);
	document.getElementById('active-of-client-base2-legend').innerHTML = myChart2.generateLegend();
	//АКТИВНОСТЬ КЛИЕНТСКОЙ БАЗЫ//

	//line
	var lineChartOptions2 = {
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
				legendHtml.push('<td class="check-legend active" onclick="updateDataset2(event, ' + '\'' + chart.legend.legendItems[i].datasetIndex + '\'' + ')" ><div class="check-legend-line " style="background-color:' + chart.data.datasets[i].backgroundColor + ';"></div><div class="chart-legend-line" style="color:#383737;font-size:16rem;">');
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

	lineChartData = {

		labels: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"],
		datasets: [
			{
				label: "Выручка",
				borderColor: '#58AEDC',
				pointBackgroundColor: '#58AEDC',
				pointRadius: 2,
				backgroundColor: '#58AEDC',
				data: [650, 359, 290, 481, 156, 255, 640, 481, 156, 255, 640, 255],
				fill: false,
				borderWidth: 2,
			},
			{
				label: "Клиенты",
				borderColor: '#567BA5',
				pointBackgroundColor: '#567BA5',
				pointRadius: 2,
				backgroundColor: '#567BA5',
				data: [750, 500, 678, 300, 96, 270, 400, 381, 156, 255, 240, 378, 300],
				fill: false,
				borderWidth: 2,
			}, {
				label: "Средний чек",
				borderColor: '#11B9A3',
				pointBackgroundColor: '#11B9A3',
				pointRadius: 2,
				backgroundColor: '#11B9A3',
				data: [600, 159, 190, 381, 156, 455, 340, 234, 111, 234, 687, 241, 234],
				fill: false,
				borderWidth: 2,
			},
			{
				label: "Средний % скидки",
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
				legendHtml.push('<td class="check-legend active" onclick="updateDataset(event, ' + '\'' + chart.legend.legendItems[i].datasetIndex + '\'' + ')" ><div class="check-legend-line " style="background-color:' + chart.data.datasets[i].backgroundColor + ';"></div><div class="chart-legend-line" style="color:#383737;font-size:16rem;">');
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

	var ctx = document.getElementById("line-chart2").getContext("2d");
	window.weightChart2 = new Chart(ctx, {
		type: 'line',
		data: lineChartData,
		options: lineChartOptions2,
	});
	document.getElementById("line-chart2-legend").innerHTML = weightChart2.generateLegend();

	//конец графика лайн

	//сегментаци по возрасту#0
	var config = {
		type: 'doughnutLabels',
		data: {
			labels: [
				" До 25 лет",
				" 25-25 лет",
				" 35-45 лет",
				" 45+ лет",
				" Не указано",
			],
			datasets: [{
				data: [
					20,
					20,
					35,
					15,
					10,
				],
				backgroundColor: [
					"#11B9A3",
					"#E5C861",
					"#58AEDC",
					"#D4614A",
					"#373636",
				],
			}],
		},
		options: {
			responsive: true,
			legend: false,
			segmentShowStroke: true,
			animation: {
				animateScale: true,
				animateRotate: true
			},
			tooltips: {
				mydata: 122,
				enabled: false,
				mode: 'index',
				position: 'nearest',
				custom: customTooltips
			},
			cutoutPercentage: 55,

		}
	};
	var ctx = document.getElementById("segmentation-of-years3").getContext("2d");
	var myChart0 = new Chart(ctx, config);
	document.getElementById('segmentation-of-years3-legend').innerHTML = myChart0.generateLegend();
	//сегментаци по возрасту//

	//структура клиентской базы#2
	var config = {
		type: 'doughnut',
		data: {
			labels: [
				" Мужчины",
				" Женщины",
				" Пол не указан",
				" С покупками",
				" Без покупок",
			],
			datasets: [{
				data: [
					3000,
					3000,
					6000,
					6000,
					5546,
				],
				backgroundColor: [
					"#11B9A3",
					"#E5C861",
					"#58AEDC",
					"#D4614A",
					"#373636",
				],
				borderWidth: 0,
			}],
		},
		options: {
			responsive: true,
			legend: false,
			legendCallback: function (chart) {
				var text = [];
				afterData = ' чел.';
				text.push('<ul class="' + chart.id + '-legend">');
				for (var i = 0; i < chart.data.datasets[0].data.length; i++) {
					text.push('<li><span class="check-inside-chart" style="background-color:' + chart.data.datasets[0].backgroundColor[i] + '"></span>');
					if (chart.data.labels[i]) {
						text.push(chart.data.labels[i] + ': <span class="text-inside-chart">' + chart.data.datasets[0].data[i] + afterData + '</span>');

					}
					text.push('</li>');
				}
				text.push('</ul>');
				return text.join("");
			},
			segmentShowStroke: true,
			animation: {
				animateScale: true,
				animateRotate: true
			},
			tooltips: {
				enabled: false,
				mode: 'index',
				position: 'nearest',
				custom: customTooltips
			},
			cutoutPercentage: 55,
			tooltipTemplate: "<%= value %>%",
		}
	};
	var ctx = document.getElementById("structura-of-client-base3").getContext("2d");
	var myChart1 = new Chart(ctx, config);
	document.getElementById('structura-of-client-base3-legend').innerHTML = myChart1.generateLegend();
	//структура клиентской базы//

	//АКТИВНОСТЬ КЛИЕНТСКОЙ БАЗЫ#3
	var afterData = '';
	var config = {
		type: 'doughnut',
		data: {
			labels: [
				" Мужчины",
				" Женщины",
				" Пол не указан",
				" Повторные",
				" Покупок на клиента",
			],
			datasets: [{
				data: [
					1500000,
					1500000,
					150000,
					1500000,
					150000,
				],
				backgroundColor: [
					"#11B9A3",
					"#E5C861",
					"#58AEDC",
					"#D4614A",
					"#373636",
				],
				borderWidth: 0,
			}],
		},
		options: {
			responsive: true,
			legend: false,
			legendCallback: function (chart) {
				var text = [];
				afterData = ' руб.';

				text.push('<ul class="' + chart.id + '-legend">');
				for (var i = 0; i < chart.data.datasets[0].data.length; i++) {
					text.push('<li><span class="check-inside-chart" style="background-color:' + chart.data.datasets[0].backgroundColor[i] + '"></span>');
					if (chart.data.labels[i]) {
						text.push(chart.data.labels[i] + ': <span class="text-inside-chart">' + chart.data.datasets[0].data[i] + afterData + '</span>');
					}
					text.push('</li>');
				}
				text.push('</ul>');
				return text.join("");
			},
			segmentShowStroke: true,
			animation: {
				animateScale: true,
				animateRotate: true
			},
			tooltips: {
				enabled: false,
				mode: 'index',
				position: 'nearest',
				custom: customTooltips
			},
			cutoutPercentage: 55,
			tooltipTemplate: "<%= value %>%",
		}
	};
	var ctx = document.getElementById("active-of-client-base3").getContext("2d");
	var myChart2 = new Chart(ctx, config);
	document.getElementById('active-of-client-base3-legend').innerHTML = myChart2.generateLegend();
	//АКТИВНОСТЬ КЛИЕНТСКОЙ БАЗЫ//

	//line

	var lineChartOptions3 = {
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
				legendHtml.push('<td class="check-legend active" onclick="updateDataset3(event, ' + '\'' + chart.legend.legendItems[i].datasetIndex + '\'' + ')" ><div class="check-legend-line " style="background-color:' + chart.data.datasets[i].backgroundColor + ';"></div><div class="chart-legend-line" style="color:#383737;font-size:16rem;">');
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
	lineChartData = {

		labels: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"],
		datasets: [
			{
				label: "Выручка",
				borderColor: '#58AEDC',
				pointBackgroundColor: '#58AEDC',
				pointRadius: 2,
				backgroundColor: '#58AEDC',
				data: [650, 359, 290, 481, 156, 255, 640, 481, 156, 255, 640, 255],
				fill: false,
				borderWidth: 2,
			},
			{
				label: "Клиенты",
				borderColor: '#567BA5',
				pointBackgroundColor: '#567BA5',
				pointRadius: 2,
				backgroundColor: '#567BA5',
				data: [750, 500, 678, 300, 96, 270, 400, 381, 156, 255, 240, 378, 300],
				fill: false,
				borderWidth: 2,
			}, {
				label: "Средний чек",
				borderColor: '#11B9A3',
				pointBackgroundColor: '#11B9A3',
				pointRadius: 2,
				backgroundColor: '#11B9A3',
				data: [600, 159, 190, 381, 156, 455, 340, 234, 111, 234, 687, 241, 234],
				fill: false,
				borderWidth: 2,
			},
			{
				label: "Средний % скидки",
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


	var ctx = document.getElementById("line-chart3").getContext("2d");
	window.weightChart3 = new Chart(ctx, {
		type: 'line',
		data: lineChartData,
		options: lineChartOptions3,
	});
	document.getElementById("line-chart3-legend").innerHTML = weightChart3.generateLegend();

	//конец графика лайн

});

