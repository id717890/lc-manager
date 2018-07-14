
$(function () {

    /* КАЛЕНДАРЬ */
    $.datepicker._updateDatepicker_original = $.datepicker._updateDatepicker;
    $.datepicker._updateDatepicker = function (inst) {
        $.datepicker._updateDatepicker_original(inst);
        var afterShow = this._get(inst, 'afterShow');
        if (afterShow)
            afterShow.apply((inst.input ? inst.input[0] : null));
    }; 
    $('#dateFrom, #dateTo, .datepicker').datepicker({
        dateFormat: 'dd.mm.yy',
        yearRange: '1950:2020',
        changeMonth: true,
        changeYear: true,
        showButtonPanel: false,
        showOtherMonths: true,
        selectOtherMonths: true,
        afterShow: function (input) {
            $('.ui-datepicker-month, .ui-datepicker-year').styler();
        }
    });

    $('.ui-datepicker-year, .ui-datepicker-month').styler();   
    /* END КАЛЕНДАРЬ */


    //$.getJSON("/bas/options.txt", function (data) {
    //    $.each(data, function (key, val) {
    //        $("#addactionList").append('<option value="' + key + '">' + val + '</option>');
    //    });
    //    $("#addactionList").prop("selectedIndex", 0);
    //});
    $("#addactionList").change(function () {
        var selected = $(this).val();
        $(".text").hide();
        $.getJSON("/bas/content.txt", function (data) {
            var arr = $.grep(Object.keys(data), function (key) {
                return data[key].shares == selected;
            });
            $.each(arr, function (key) {
                $("#actionoption").html('');
                $("#actionoption").append('<div>' + data[arr[key]].block + '</div>');
                $('select').niceSelect(); 
            });
            $("#actionoption").prop("selectedIndex", 0);
        });
    });
});

var typeDiagram = "";
function format( d ) {
    return '<div><table cellspacing="0" width="100%" role="grid" aria-describedby="clients_info" style="width: 100%;"><tbody>'+d.lorem+'</tbody></table></div>';
}

function showBuysModalWindow(e) {
	var window = document.getElementById("clientBuysModalWindow");
	window.style.display = "block";
	window.onclick = function (event) {
		if (event.target == window) {
			window.style.display = "none";
		}
	}
};

function showBonusesModalWindow() {
	var window = document.getElementById("clientBonusModalWindow");
	window.style.display = "block";
	window.onclick = function (event) {
		if (event.target == window) {
			window.style.display = "none";
		}
	} 
};

function personData(d) {
    //return '<div>123</div>';
    return '<div>'+
            '<div>'+((d.diagram === null)?"":d.diagram)+'</div>'+
            '<div><div class="userlist_info_o">'+
            '<h3><a href="#" id="additionalInformation" onclick="showBuysModalWindow(this); return false" style="text-decoration: none; color: #58afdd;">Дополнительная информация:</a></h3>'+
		    '<div class="client_list_ifo_h"><div>?<p>Указано общее количество и сумма покупок участника за всё время участия в программе, включающее в себя покупки, по которым был возврат и списанные бонусы, которыми участник оплатил часть покупки. <br/>'+
	'Важно!<br/>'+
	'Покупка - это общая сумма оплаченных денег и бонусов.<br/>' +
		'Выручка – это общая сумма оплаченных реальных денег участником за покупку.</p></div><p>Покупки: </p><span>'+
            ((d.buyCount === undefined)?"-":(d.buyCount+' шт.'))+'</span>'+
            '<span>'+((d.buyAmount === undefined)?"-":(d.buyAmount+' руб.'))+'</span>'+
            '</div>'+
		'<div class="client_list_ifo_h"><div>?<p>Указано общее количество и сумма возвратов покупок участника за всё время участия в программе. <br/>'+
	'Важно!<br/>'+
	'Сумма возвратов – это общая сумма возвращенных реальных денег участником + количество бонусов, которые вернутся участнику на счёт, в случае если в покупку, по которой делается возврат была операция списания бонусов.</p></div><p>Возвраты: </p><span>'+
            ((d.refundQty === undefined)?"-":(d.refundQty+' шт.'))+'</span>'+
            '<span>'+((d.refund === undefined)?"-":(d.refund+' руб.'))+'</span>'+
            '</div>'+
		'<div class="client_list_ifo_h"><div>?<p>Указаны дата и сумма последней операции участника программы. <br/>'+
	'Покупка - положительное значение операции<br/>'+
	'Возврат - отрицательное значение операции</p></div><p>Последняя операция: </p><span>'+
            ((d.buyLastDate === undefined)?"-":(d.buyLastDate))+'</span>'+
            '<span>'+((d.buyLastAmount === undefined)?"-":(d.buyLastAmount+' руб.'))+'</span>'+
            '</div>'+
            '<div class="client_list_ifo_h"><div>?<p>Указано общее количество и сумма списанных бонусов в покупки, по которым не было возврата, за всё время участия в программе.</p></div><p>Списание бонусов: </p><span>'+
            ((d.writeOffCount === undefined)?"-":(d.writeOffCount+' раз.'))+'</span>'+
            '<span>'+((d.writeOffAmount === undefined)?"-":(d.writeOffAmount+' б.'))+'</span>'+
            '</div>'+
            '<div class="client_list_ifo_h"><div>?<p>Указаны дата и источник регистрации клиента в качестве участника программы.</p></div><p>Регистрация участника: </p><span>'+
            ((d.posRegister === undefined)?"-":(d.posRegister))+'</span>'+
            '<span>'+((d.dateRegister === undefined)?"-":(d.dateRegister))+'</span>'+
            '</div></div>'+
            '<div class="userlist_info_t"><h3><a href="#" onclick="showBonusesModalWindow(); return false;" style="text-decoration: none; color: #58afdd;">Бонусы не за покупки:</a>' +
            '<a href="#" onclick="showClientChangeModalWindow(' + d.card +'); return false;" style="text-decoration: none; color: #58afdd; display:none;">Редактирование карточки клиента</a></h3>'+
            '<div class="client_list_ifo_h"><div>?<p>123123</p></div><p>Welcome: </p><span>'+
            ((d.welcomeBonusDate === undefined)?"-":(d.welcomeBonusDate))+'</span>'+
            '<span>'+((d.welcomeBonusAmount === undefined)?"-":(d.welcomeBonusAmount+' б.'))+'</span>'+
            '</div>'+
            '<div class="client_list_ifo_h"><div>?<p>123123</p></div><p>Promo: </p><span>'+
            ((d.promoBonusDate === undefined)?"-":(d.promoBonusDate))+'</span>'+
            '<span>'+((d.promoBonusAmount === undefined)?"-":(d.promoBonusAmount+' б.'))+'</span>'+
            '</div>'+
            '<div class="client_list_ifo_h"><div>?<p>123123</p></div><p>Operator: </p><span>'+
            ((d.opperatorBonusDate === undefined)?"-":(d.opperatorBonusDate))+'</span>'+
            '<span>'+((d.operatorBonusAmount === undefined)?"-":(d.operatorBonusAmount+' б.'))+'</span>'+
            '</div>'+
            '<div class="client_list_ifo_h"><div>?<p>123123</p></div><p>Friend: </p><span>'+
            ((d.friendBonusDate === undefined)?"-":(d.friendBonusDate))+'</span>'+
            '<span>'+((d.friendBonusAmount === undefined)?"-":(d.friendBonusAmount+' б.'))+'</span>'+
            '</div>'+
            '<div class="client_list_ifo_h"><div>?<p>123123</p></div><p>Birthday: </p><span>'+
            ((d.birthdayBonusDate === undefined)?"-":(d.birthdayBonusDate))+'</span>'+
            '<span>'+((d.birthdayBonusAmount === undefined)?"-":(d.birthdayBonusAmount+' б.'))+'</span>'+
            '</div></div></div>'+
    '</div>';
}
function stockData(d){
    return '<div>'+
            ((d.period === undefined || !d.period)?"":('<div class="action_first_title">'+d.period+'</div>'))+
            ((d.conditions === undefined || !d.conditions)?"":('<div class="action_first_content">'+d.conditions+'</div>'))+
            ((d.header === undefined || !d.header)?"":('<div class="action_to_title">'+d.header+'</div>'))+
            ((d.clarification === undefined || !d.clarification)?"":('<div class="action_to_content">'+d.clarification+'</div>'))+
            ((d.diagram === undefined || !d.diagram)?"":(d.diagram))+
    '</div>';
}

function bookkeepingData(d) {
    return '<div>' + ((d.diagrams === undefined) ? "" : (d.diagrams)) + '</div>';
}


function mailingData(d) {
    return '<div>' +
        '<div class="messageBox"><div class="hover_detail">?<p>123123</p></div><p class="messageTitle">SMS: <span class="right-number">5 | 300</span></p>' +
        ((d.smsText === undefined || !d.smsText) ? "" : ('<div class="messageText">' + d.smsText + '</div>')) + '</div>' +
        '<div class="messageBox"><div class="hover_detail">?<p>123123</p></div><p class="messageTitle">Push: </p>' +
        ((d.pushText === undefined || !d.pushText) ? "" : ('<div class="messageText">' + d.pushText + '</div>')) + '</div>' +
        '<div class="messageBox"><div class="hover_detail">?<p>123123</p></div><p class="messageTitle">E-mail: </p><div class="maketLink">' +
        ((d.templateEmail === undefined || !d.templateEmail) ? "<p>Посмотреть макет письма</p>" : ('<a href="' + d.templateEmail + '">Посмотреть макет письма</a>')) + '</div></div>' +
        ((d.header === undefined || !d.header) ? "" : ('<div class="action_to_title">' + d.header + '</div>')) +
        ((d.clarification === undefined || !d.clarification) ? "" : ('<div class="action_to_content">' + d.clarification + '</div>')) +
        ((d.diagram === undefined || !d.diagram) ? "" : (d.diagram)) +
        '</div>';
}
function applicationsdata(d) {
    return '<div>' +
        '<div class="table__content__info open"><div class="table__item__info_chat">' + d.content + '</div><div class="chat-form"><form class="chat"><div class="input-chat"><input type="text" placeholder="Написать сообщение"></div><a href="#" class="chat-btn"></a></form></div></div>';
}
var clientsTable;
var table;
var tableSales;
var serchingRow = false;
var serchingRowClients = false;
$(document).ready(function() {
    tableSales = $('table#clients-sales').DataTable({
        "searchDelay": 1000,
        "serverSide": true,
        "processing": true,
        "language": {
            processing: '<div class="wrapper-for-loading-datatable"><img src="/img/loading-element-1.svg" alt="Alternate Text" /><span class="sr-only">Загрузка...</span></div>'
        },
        "ajax":
        {
            method: "POST",
            url: "../Home/SalesGetCheques",
            data: function (params) {
                if (location.hash != '' && params.draw === 1) {
                    var checkNumber = decodeURIComponent(location.hash.replace('#', ''));
                    params.columns[5].search.value = checkNumber;
                }
                params.date_from = $("#dateFrom").val();
                params.date_to = $("#dateTo").val();
            }
        },
        "columns": [
            {
                "className": 'details-control',
                "orderable": false,
                "data": null,
                "defaultContent": '<img src="../img/tableiconopen.png">'
            },
            { "data": "date" },
            { "data": "pos" },
            { "data": "phone" },
            { "data": "operation" },
            { "data": "number" },
            { "data": "summ" },
            { "data": "added" },
            { "data": "redeemed" }
        ],
        "ordering": false,
        initComplete: function () {
            $("#clients-sales_length").appendTo($(".filters"));
            $("#clients-sales_length").css('display', 'flex');

            this.api().columns([4]).every(function () {
                var column = this;
                var select = $('<select><option value=""></option></select>')
                    .appendTo($(column.header()))
                    .bind('keyup change',
                        function () {
                            column.search($(this).val());
                            tableSales.draw();
                        });

                select.append('<option value="Возврат">Возврат</option>');
                select.append('<option value="Покупка">Покупка</option>');
            });
            this.api().columns([2, 3, 5]).every(function() {
                addInputFilter(this);
            });
            this.api().columns([1]).every(function() {
                addInputFilterWoAuto(this, ' datepicker');
            });
            this.api().columns([6]).every(function() {
                var column = this;
                var select = $('<select><option value=""></option></select>')
                    .appendTo($(column.header()))
                    .bind('keyup change',
                        function() {
                            column.search($(this).val());
                            tableSales.draw();
                        });

                select.append('<option value="1-500">1 - 500р.</option>');
                select.append('<option value="500-1000">500 - 1 000р.</option>');
                select.append('<option value="1000-5000">1 000 - 5 000р.</option>');
                select.append('<option value="5000-10000">5 000 - 10 000р.</option>');
                select.append('<option value="10000-99000">10 000 - 99 000р.</option>');

            });
            this.api().columns([7, 8]).every(function() {
                var column = this;
                var select = $('<select><option value=""></option></select>')
                    .appendTo($(column.header()))
                    .bind('keyup change',
                        function() {
                            column.search($(this).val());
                            tableSales.draw();
                        });

                select.append('<option value="1-100">1 - 100</option>');
                select.append('<option value="100-500">100 - 500</option>');
                select.append('<option value="500-1000">500 - 1000</option>');
                select.append('<option value="1000-9000">1000 - 9000</option>');
            });
            niceSelect();
            $(".nice-select ul.list li.focus").removeClass('focus');
            if (location.hash != '') {
                var checkNumber = location.hash.replace('#', '');
                $('#clients-sales th:nth-child(6) input').val(decodeURIComponent(checkNumber));
                serchingRow = true;
                $('#clients-sales th:nth-child(6) input').trigger("change");
            }

        }
    });
    tableSales.on( 'draw', function () {
        if(serchingRow){
            if(!$('#clients-sales tbody tr:first-child td:first-child').hasClass('dataTables_empty')){
                $('#clients-sales tbody tr:first-child td:first-child').trigger('click');
                serchingRow = false;
            }    
        }
    } );
    $('#clients-sales tbody').on('click', 'tr.odd td, tr.even td', function () {
        var tr = $(this).closest('tr');
        var row = tableSales.row(tr);
        showHideRow(tr,row,format);
    });

    $("#OperatorSalesReport").click(function(e) {
        e.preventDefault();
        $("#from").val($("#dateFrom").val());
        $("#to").val($("#dateTo").val());
        $("#date").val(tableSales.column(1).search());
        $("#shop").val(tableSales.column(2).search());
        $("#phone").val(tableSales.column(3).search());
        $("#operation").val(tableSales.column(4).search());
        $("#cheque").val(tableSales.column(5).search());
        $("#sum").val(tableSales.column(6).search());
        $("#charge").val(tableSales.column(7).search());
        $("#redeem").val(tableSales.column(8).search());
        $("#export-sales-to-file-form").submit();
    });
	
	tableTerminalCheques = $('table#TerminalCheques').DataTable({
		//"ajax": "../Home/SalesGetCheques",
		"pageLength": 5,
        "columns": [
        {
            "className":      'details-control',
            "orderable":      false,
            "data":           null,
            "defaultContent": '<img src="../img/tableiconopen.png">'
        },
		{ "data": "date" },
		{ "data": "operation" },
		{ "data": "number" },
        { "data": "summ" },
		{ "data": "added" },
		{ "data": "redeemed" }
        ],
        "ordering": false,
        initComplete: function () {
            this.api().columns([2]).every( function () {
                addSelectFilter(this);
           } );
            this.api().columns([3]).every( function () {
               addInputFilter(this);
           });
            this.api().columns([1]).every( function () {
                addInputFilter(this,' datepicker');
           } );
            this.api().columns([4]).every( function () {
               var column = this;
               var select = $('<select><option value=""></option></select>')
                    .appendTo( $(column.header()) )
                   .bind( 'keyup change', function () {
                        table.draw();
                    } );

                    select.append( '<option value="1000-5000">1 000 - 5 000р.</option>' );
                    select.append( '<option value="5000-10000">5 000 - 10 000р.</option>' );
                    select.append( '<option value="10000-99000">10 000 - 99 000р.</option>' );
           } );
            this.api().columns([5,6]).every( function () {
               var column = this;
               var select = $('<select><option value=""></option></select>')
                    .appendTo( $(column.header()) )
                   .bind( 'keyup change', function () {
                        table.draw();
                    } );

                    select.append( '<option value="100-500">100 - 500</option>' );
                    select.append( '<option value="500-1000">500 - 1000</option>' );
                    select.append( '<option value="1000-9000">1000 - 9000</option>' );
           } );
             niceSelect();
             $(".nice-select ul.list li.focus").removeClass('focus');
             if(location.hash!=''){
                var checkNumber = location.hash.replace('#','');
                $('#TerminalCheques th:nth-child(4) input').val(checkNumber);
                serchingRow = true;
                $('#TerminalCheques th:nth-child(4) input').trigger("change");
           }

        }
    });
	tableTerminalCheques.on( 'draw', function () {
        if(serchingRow){
            if(!$('#TerminalCheques tbody tr:first-child td:first-child').hasClass('dataTables_empty')){
                $('#TerminalCheques tbody tr:first-child td:first-child').trigger('click');
                serchingRow = false;
            }
        }
    });
    $('#TerminalCheques tbody').on('click', 'tr.odd td, tr.even td', function () {
        var tr = $(this).closest('tr');
		var row = tableTerminalCheques.row( tr );
        showHideRow(tr,row,format);
    });
    
    //Clients Table page


    clientsTable = $('table#clientsTable').DataTable({
        "searchDelay": 1000,
        "serverSide": true,
        "processing": true,
        "language": {
            processing: '<div class="wrapper-for-loading-datatable"><img src="/img/loading-element-1.svg" alt="Alternate Text" /><span class="sr-only">Загрузка...</span></div>'
        },
        "ajax":
        {
            method: "POST",
            url: "../Home/ClientData",
            data: function (params) {
                if (location.hash != '' && params.draw === 1) {
                    var phoneNumber = location.hash.replace('#', '');
                    params.columns[2].search.value = phoneNumber;
                }
            }
        },
        "columns": [
            {
                "className": 'details-control',
                "orderable": false,
                "data": null,
                "defaultContent": '<img src="../img/tableiconopen.png">'
            },
            { "data": "name" },
            { "data": "phone" },
            { "data": "email" },
            { "data": "birthdate" },
            { "data": "gender" },
            { "data": "client_type" },
            { "data": "card" },
            { "data": "level" },
            {
                "width": "130px",
                "data": "balance"
            }
        ],
        "ordering": false,
        initComplete: function () {
            $("#clientsTable_length").appendTo($(".client_nav")).addClass("table-length");
            $("#clientsTable_length").css("display", "flex");
            $("#clientsTable_length p").remove();

            this.api().columns([6]).every(function () {
                addSelectFilter(this);
            });
            this.api().columns([1, 2, 3, 7]).every(function () {
                addInputFilter(this);
            });
            this.api().columns([5]).every(function () {
                var column = this;
                var select = $('<select><option value=""></option></select>')
                    .appendTo($(column.header()))
                    .bind('keyup change',
                        function () {
                            column.search($(this).val());
                            clientsTable.draw();
                        });

                select.append('<option value="Мужской">Мужской</option>');
                select.append('<option value="Женский">Женский</option>');
            });
            this.api().columns([8]).every(function () {
                var column = this;
                var select = $('<select><option value=""></option></select>')
                    .appendTo($(column.header()))
                    .bind('keyup change',
                        function () {
                            column.search($(this).val());
                            clientsTable.draw();
                        });

                select.append('<option value="_1.00%_">1%</option>');
                select.append('<option value="_2.00%_">2%</option>');
                select.append('<option value="_3.00%_">3%</option>');
                select.append('<option value="_4.00%_">4%</option>');
                select.append('<option value="_5.00%_">5%</option>');
                select.append('<option value="_7.00%_">7%</option>');
                select.append('<option value="_10.00%_">10%</option>');
                select.append('<option value="_15.00%_">15%</option>');
                select.append('<option value="_20.00%_">20%</option>');
            });
            this.api().columns([4]).every(function () {
                addInputFilter(this, ' datepicker');
            });
            this.api().columns([9]).every(function () {
                var column = this;
                var select = $('<select><option value=""></option></select>')
                    .appendTo($(column.header()))
                    .bind('keyup change',
                    function () {
                        column.search($(this).val());
                        clientsTable.draw();
                    });

                select.append('<option value="1-50">1 - 50</option>');
                select.append('<option value="50-100">50 - 100</option>');
                select.append('<option value="100-250">100 - 250</option>');
                select.append('<option value="250-500">250 - 500</option>');
                select.append('<option value="500-1000">500 - 1 000</option>');
                select.append('<option value="1000-2000">1 000 - 2 000</option>');
                select.append('<option value="2000-3000">2 000 - 3 000</option>');
            });
            niceSelect();
            $(".nice-select ul.list li.focus").removeClass('focus');
            if (location.hash != '') {
                var phoneNumber = location.hash.replace('#', '');
                $('#clientsTable th:nth-child(3) input').val(phoneNumber);
                serchingRowClients = true;
                $('#clientsTable th:nth-child(3) input').trigger('change');
            }
        }
    });
    
    clientsTable.on('draw', function () {
        $("#clientCount").html(clientsTable.page.info().recordsTotal);
        if (serchingRowClients){
            if(!$('#clientsTable tbody tr:first-child td:first-child').hasClass('dataTables_empty')){
                $('#clientsTable tbody tr:first-child td:first-child').trigger('click');
                serchingRowClients = false;
            }    
        }
    });

    $('#clientsTable tbody').on('click', 'tr.odd td, tr.even td', function () {
        var tr = $(this).closest('tr');
        var row = clientsTable.row(tr);
        showHideRow(tr,row,personData);
    });

    $("#export-clients-to-file").on("click", function(e) {
            e.preventDefault();
            $("#fio").val(clientsTable.column(1).search());
            $("#phone").val(clientsTable.column(2).search());
            $("#email").val(clientsTable.column(3).search());
            $("#birthdate").val(clientsTable.column(4).search());
            $("#sex").val(clientsTable.column(5).search());
            $("#type").val(clientsTable.column(6).search());
            $("#card").val(clientsTable.column(7).search());
            $("#level").val(clientsTable.column(8).search());
            $("#balance").val(clientsTable.column(9).search());
            $("#export-clients-to-file-form").submit();
        });

	//Table of client cheques
	tableClientCheques = $('table#ClientCheques').DataTable({

		"pageLength": 5,
		"columns": [
			{
				"className": 'details-control',
				"orderable": false,
				"data": null,
				"defaultContent": '<img src="../img/tableiconopen.png">'

			},
			{ "data": "date" },
			{ "data": "pos" },
			{ "data": "operation" },
			{ "data": "summ" },
			{ "data": "added" },
			{ "data": "redeemed" }
		],
		"ordering": false,
		initComplete: function () {
			this.api().columns([3]).every(function () {
				addSelectFilter(this);
			});
			this.api().columns([2]).every(function () {
				addInputFilter(this);
			});
			this.api().columns([1]).every(function () {
				addInputFilter(this, ' datepicker');
			});
			this.api().columns([4]).every(function () {
				var column = this;
				var select = $('<select><option value=""></option></select>')
					.appendTo($(column.header()))
					.bind('keyup change', function () {
						table.draw();
					});

				select.append('<option value="1000-5000">1 000 - 5 000р.</option>');
				select.append('<option value="5000-10000">5 000 - 10 000р.</option>');
				select.append('<option value="10000-99000">10 000 - 99 000р.</option>');
			});
			this.api().columns([5, 6]).every(function () {
				var column = this;
				var select = $('<select><option value=""></option></select>')
					.appendTo($(column.header()))
					.bind('keyup change', function () {
						table.draw();
					});

				select.append('<option value="100-500">100 - 500</option>');
				select.append('<option value="500-1000">500 - 1000</option>');
				select.append('<option value="1000-9000">1000 - 9000</option>');
			});
			niceSelect();
			$(".nice - select ul.list li.focus").removeClass('focus');
			if (location.hash != '') {
				var checkNumber = location.hash.replace('#', '');
				$('#ClientCheques th:nth-child(4) input').val(checkNumber);
				serchingRow = true;
				$('#ClientCheques th:nth-child(4) input').trigger("change");
			}
		}
	});
	tableClientCheques.on('draw', function () {
		if (serchingRow) {
			if (!$('#ClientCheques tbody tr:first-child td:first-child').hasClass('dataTables_empty')) {
                               $('#ClientCheques tbody tr:first-child td:first-child').trigger('click');
				serchingRow = false;
			}
		}
	});
	$('#ClientCheques tbody').on('click', 'tr.odd td, tr.even td', function () {
		var tr = $(this).closest('tr');
        var row = tableClientCheques.row(tr);
		showHideRow(tr, row, format);
	});

	//Table of client bonuses
	tableClientBonuses = $('table#ClientBonuses').DataTable({

		"pageLength": 5,
		"columns": [
			{
				"className": 'details-control',
				"orderable": false,
				"data": null,
				"defaultContent": '<img src="../img/tableiconopen.png">'

			},
			{ "data": "BonusDate" },
			{ "data": "BonusSource" },
			{ "data": "BonusAdded" },
			{ "data": "BonusRedeemed" },
			{ "data": "BonusBurn" }
		],
		"ordering": false,
		initComplete: function () {
			this.api().columns([3]).every(function () {
				addSelectFilter(this);
			});
			this.api().columns([2]).every(function () {
				addInputFilter(this);
			});
			this.api().columns([1]).every(function () {
				addInputFilter(this, ' datepicker');
			});
			this.api().columns([4]).every(function () {
				var column = this;
				var select = $('<select><option value=""></option></select>')
					.appendTo($(column.header()))
					.bind('keyup change', function () {
						table.draw();
					});

				select.append('<option value="1000-5000">1 000 - 5 000р.</option>');
				select.append('<option value="5000-10000">5 000 - 10 000р.</option>');
				select.append('<option value="10000-99000">10 000 - 99 000р.</option>');
			});
			this.api().columns([5]).every(function () {
				var column = this;
				var select = $('<select><option value=""></option></select>')
					.appendTo($(column.header()))
					.bind('keyup change', function () {
						table.draw();
					});

				select.append('<option value="100-500">100 - 500</option>');
				select.append('<option value="500-1000">500 - 1000</option>');
				select.append('<option value="1000-9000">1000 - 9000</option>');
			});
			niceSelect();
			$(".nice - select ul.list li.focus").removeClass('focus');
			if (location.hash != '') {
				var checkNumber = location.hash.replace('#', '');
				$('#ClientBonuses th:nth-child(4) input').val(checkNumber);
				serchingRow = true;
				$('#ClientBonuses th:nth-child(4) input').trigger("change");
			}
		}
	});
	tableClientBonuses.on('draw', function () {
		if (serchingRow) {
			if (!$('#ClientBonuses tbody tr:first-child td:first-child').hasClass('dataTables_empty')) {
				$('#ClientBonuses tbody tr:first-child td:first-child').trigger('click');
				serchingRow = false;
			}
		}
	});
	//$('#ClientBonuses tbody').on('click', 'tr.odd td, tr.even td', function () {
	//	var tr = $(this).closest('tr');
	//	var row = tableClientBonuses.row(tr);
	//	showHideRow(tr, row, format);
	//});
		
    stockTable = $('#stockTable').DataTable( {
        "ajax": "../bas/stock.txt",
        "columns": [
        {
            "className":      'details-control',
            "orderable":      false,
            "data":           null,
            "defaultContent": '<img src="../img/tableiconopen.png">'
        },
        {   "data": "name", 
            "className":      'colName', },
        { "data": "status" },
        { "data": "margin" },
        { "data": "refresh" },
        { "data": "download" }
        ],
        "ordering": false,
        "lengthChange": false,
        initComplete: function () {
            $("#stockCount").html(this.api().data().count());
            
            this.api().columns([2]).every( function () {
                addSelectFilter(this);         
           } );
            this.api().columns([1]).every( function () {
               addInputFilter(this);             
           });
            niceSelect();
        }
    });

    $('#stockTable tbody').on('click', 'tr.odd td, tr.even td', function () {
        var tr = $(this).closest('tr');
        var row = stockTable.row( tr );
        showHideRow(tr,row,stockData);
    });

    applicationsTable = $('#applicationsTable').DataTable({
        "ajax": "../bas/applications.txt",
        "columns": [
            {
                "className": 'details-control',
                "orderable": false,
                "data": null,
                "defaultContent": '<img src="../img/tableiconopen.png">'
            },
            { "data": "theme" },
            { "data": "date" },
            { "data": "preorety" },
            { "data": "status" },
            { "data": "iconum" }
        ],
        "ordering": false,
        "lengthChange": false,
        initComplete: function () {
            $("#appcol").html(this.api().data().count());

            this.api().columns([3,4]).every(function () {
                addSelectFilter(this);
            });
            this.api().columns([2]).every(function () {
                addInputFilter(this, ' datepicker');
            });
            this.api().columns([1]).every(function () {
                addInputFilter(this);
            });
            niceSelect();
        }
    });

    $('#applicationsTable tbody').on('click', 'tr.odd td, tr.even td', function () {
        var tr = $(this).closest('tr');
        var row = applicationsTable.row(tr);
        showHideRow(tr, row, applicationsdata);
    });

    mailingTable = $('#mailingTable').DataTable({
        "ajax": "/bas/mailing.txt",
        "columns": [
            {
                "className": 'details-control',
                "orderable": false,
                "data": null,
                "defaultContent": '<img src="/img/tableiconopen.png">'
            },
            {
                "data": "name",
                "className": 'colName',
            },
            { "data": "date" },
            { "data": "subject" },
            { "data": "refresh" },
            { "data": "download" }
        ],
        "ordering": false,
        "lengthChange": false,
        initComplete: function () {
            $("#mailingCount").html(this.api().data().count());

            this.api().columns([3]).every(function () {
                addSelectFilter(this);
            });
            this.api().columns([1]).every(function () {
                addInputFilter(this);
            });
            this.api().columns([2]).every(function () {
                addInputFilter(this, ' datepicker');
            });
            niceSelect();
        }
    });

    $('#mailingTable tbody').on('click', 'tr.odd td, tr.even td', function () {
        var tr = $(this).closest('tr');
        var row = mailingTable.row(tr);
        showHideRow(tr, row, mailingData);
    });

    $(function () {
        //$(".datepicker").datepicker();
    });
    
	faqTable = $('#faqTable').DataTable({
		//"ajax": "../bas/shopsgoods.txt",
		//"ajax": "../bas/faq.txt",
		"ajax": "../Home/GetFaq",
		"columns": [
			{
				"className": 'details-control',
				"orderable": false,
				"data": null,
				"defaultContent": '<img src="../img/tableiconopen.png">'
			},
			{ "data": "name" }
		],
		"ordering": false,
		"lengthChange": false,
		initComplete: function () {
			$(".goodshopTablecont").html(this.api().data().count());
			this.api().columns([1, 2]).every(function () {
				addInputFilter(this);
			});
		}
	});

	$('#faqTable tbody').on('click', 'tr.odd td, tr.even td', function () {
		var tr = $(this).closest('tr');
		var row = faqTable.row(tr);
		showHideRow(tr, row, format);
	});

   // bokepingTable = $('#bookkeeping').DataTable({
   //     "ajax": "../bas/bookkeeping.txt",
   //     "columns": [
   //         { "className": 'details-control', "orderable": false, "data": null, "defaultContent": '<img src="/img/tableiconopen.png">' },
			//{ "data": "partner" },
			//{ "data": "buys" },
   //         { "data": "payment" },
   //         { "data": "ipayment" },
   //         { "data": "client" }
   //     ],
   //     "ordering": false,
   //     "lengthChange": false,
   //     initComplete: function () {
   //         $(".bokepingcount").html(this.api().data().count());
   //         this.api().columns([1]).every(function () {
   //             addInputFilter(this);
   //         });
   //         this.api().columns([2, 3, 4, 5]).every(function () {
   //             var column = this;
   //             var select = $('<select><option value=""></option></select>')
   //                 .appendTo($(column.header()))
   //                 .bind('keyup change', function () {
   //                     table.draw();
   //                 });

   //             select.append('<option value="0-5000">0 - 5 000р.</option>');
   //             select.append('<option value="5000-10000">5 000 - 10 000р.</option>');
   //             select.append('<option value="10000-50000">10 000 - 50 000р.</option>');
   //         });
   //         niceSelect();
   //     }
   // });

   // $('#bookkeeping tbody').on('click', 'tr.odd td, tr.even td', function () {
   //     var tr = $(this).closest('tr');
   //     var row = bokepingTable.row(tr);
   //     showHideRow(tr, row, bookkeepingData);
   // });

});

function niceSelect(){
    $('.dataTable select').niceSelect();
    $('.dataTable .nice-select .current').html('Выбрать');
    $('.dataTable .nice-select .list li:first-child').html('Выбрать');
    $('.dataTables_length select').niceSelect();
    $( function() {
        //$(".datepicker").datepicker();
        $(".datepicker").datepicker({
            dateFormat: 'dd.mm.yy',
            yearRange: '1950:2020',
            changeMonth: true,
            changeYear: true,
            showButtonPanel: false,
            showOtherMonths: true,
            selectOtherMonths: true,
            afterShow: function (input) {
                $('.ui-datepicker-month, .ui-datepicker-year').styler();
            }
        });
    });
}

function addSelectFilter(column){
   var select = $('<select><option value=""></option></select>')
   .appendTo( $(column.header()) )
   .bind( 'keyup change', function () {
    var val = $.fn.dataTable.util.escapeRegex(
        $(this).val()
        );
        val = val.replace(/<[^>]+>/g,'');
    column
    .search( val ? '^'+val+'$' : '', true, false )
    .draw();
    } );

   column.data().unique().sort().each( function ( d, j ) {
    select.append( '<option value="'+d+'">'+d+'</option>' )
} );

}

function addInputFilter(column,inputClass){
    inputClass = inputClass || "";
     var input = $('<input type="text" class="zbz-input-clearable '+inputClass+'"/>')
     .appendTo($(column.header()) )
     .on( 'keyup change input', function () {
         if (column.search() !== this.value) {
            column
            .search( this.value )
            .draw();
        }
    } );
    addLiveSearch(input,column);
}
function addInputFilterWoAuto(column, inputClass) {
    inputClass = inputClass || "";
    var input = $('<input type="text" class="zbz-input-clearable ' + inputClass + '"/>')
        .appendTo($(column.header()))
        .on('keyup change input', function () {
            if (column.search() !== this.value) {
                column
                    .search(this.value)
                    .draw();
            }
        });
}
function addLiveSearch(input,column){
    var liveSearchData = [];
    column.data().unique().sort().each(function (d, j) {
        if (d != null) {
            d = d + "";
            liveSearchData.push(d);
        }
       
    } );

    $(input).autocomplete({
        source: liveSearchData,
        select: function (event, ui) {
            $(this).val(ui.item.value);
            $(this).trigger("change");
        },
        search: function (event, ui) {
            $(this).addClass('serching');

        },
        response: function (event, ui) {
            if (ui.content.length == 0) {
                $(this).removeClass('serching');
            }
        },
        close: function (event, ui) {
            $(this).removeClass('serching');
        }
    });
        jQuery.ui.autocomplete.prototype._resizeMenu = function () {
            var ul = this.menu.element;
            ul.outerWidth(this.element.outerWidth());
        }
}
var currentRowOpen, currentTrOpen;
function showHideRow(tr, row, formatData) {
    if (row.child.isShown()) {
            currentRowOpen=false;
            hideRow(tr,row);
            typeDiagram = "";
        }
    else {
            if(currentRowOpen){
                hideRow(currentTrOpen,currentRowOpen);
                typeDiagram = "";
            }
            tr.addClass('shown');
            currentTrOpen = tr;
            currentRowOpen = row;
            row.child( formatData(row.data()),'hideData').show("slow");
            $(tr.next()).children().children().css('display','block');         
            $(tr.next()).children().children().hide(0).slideDown(600,function(){
                if(typeDiagram!=""){
                    drawChart();
                }
            }); 
        }
}

function hideRow(tr, row){
    $(tr.next()).children().children().slideUp(600,function(){
                row.child.hide("slow");
                tr.removeClass('shown');
    });   
}

$("#dateFrom, #dateTo").on('change input', function(){
    tableSales.draw();
});

$.fn.dataTable.ext.search.push(
    function( settings, data, dataIndex ) {
        var filter = true;
     if(settings.sTableId === "clients"){          
        var from = $("#dateFrom").val();         
        var to = $("#dateTo").val();      
        if(filter && !(from == "" && to == "")){
            filter = searchByDate(from,to, data[1]);
        } 
        if(filter && $('#clients th:nth-child(7) select').val()!="" && $('#clients th:nth-child(7) select').val()){
            filter = searchByDiapason($('#clients th:nth-child(7) select').val(),data[6]);
        }
        if(filter && $('#clients th:nth-child(8) select').val()!="" && $('#clients th:nth-child(8) select').val()){
            filter = searchByDiapason($('#clients th:nth-child(8) select').val(),data[7]);
        }
        if(filter && $('#clients th:nth-child(9) select').val()!="" && $('#clients th:nth-child(9) select').val()){
            filter = searchByDiapason($('#clients th:nth-child(9) select').val(),data[8]);
        }
    }
    return filter;
}
);
function searchByDiapason(val, data){
    val = val.replace(/<[^>]+>/g,'');
    val = val.split('-');
    var normValue = parseInt(data.replace(" ", ''));
    if(normValue>=val[0] && normValue<=val[1]){
        return true;
    } else{
        return false;
    }
}
function searchByDate(from, to, data){
    var dateArr = from.split(' ')[0].split('.');
            var fromDate = new Date(dateArr[2] , dateArr[1]-1, dateArr[0], 3 );
            var dateArr = to.split(' ')[0].split('.');
            var toDate = new Date(dateArr[2] , dateArr[1]-1, dateArr[0], 3 );

            var dateArr = data.split(' ')[0].split('.');
            var curDate = new Date(dateArr[2] , dateArr[1]-1, dateArr[0], 3 );
            if(from != "" && to != ""){
                if(fromDate <= curDate && toDate >= curDate){
                    return true;
                }
            } else{
                if(from != ""){
                    if(fromDate <= curDate){
                        return true;
                    }
                }else{
                    if(toDate >= curDate){
                        return true;
                    }
                }
            }
    return false;
}
$('#claerDate').click(function(){
    clearDate();
});

$('#claerAllFilters').click(function(){
    clearDate();
    $('th input').val("");
    $('th select option:first-child').prop('selected', true);
    $('th select, th input').trigger("change");
    $('.dataTable .nice-select .current').html('Выбрать');
    $('.dataTable .nice-select .selected').removeClass('selected');
    $('.dataTable .nice-select .list li:first-child').addClass('selected');

});

function clearDate(){
    $("#dateFrom").val("").removeClass("zbz-input-clearable__x");
    $("#dateTo").val("").removeClass("zbz-input-clearable__x");
    tableSales.draw();
}

function ShowHidePassword(id) {
    element = $('#' + id)
    element.replaceWith(element.clone().attr('type', (element.attr('type') == 'password') ? 'text' : 'password'))
}
 


$(document).ready(function () {
    $(document).on('click', '.pen_wrap_data', function () {
        var button_cen = $('#cencel_but_reg');
        var button_penwd = $('.pen_wrap_data');
        var button_save = $('#save_but_reg');
        var enabled = $('.active_input');
        button_penwd.css('display', 'none');
        button_cen.fadeIn();
        button_save.fadeIn();
        enabled.prop('disabled', false).css('background-color', '#ffffff');
        $(document).on('click', '#cencel_but_reg', function () {
            button_cen.hide();
            button_save.hide();
            button_penwd.css('display', 'block');
            enabled.prop('disabled', true).css('background-color', '#f5f5f5');
        });
    });

    /*учетные данные*/

    /*телефон*/

    $(document).on('click', '.pen_wrap_phone', function () {
        var button_cen = $('#cencel_but_phone');
        var button_penp = $('.pen_wrap_phone');
        var button_save = $('#save_but_phone');
        var enabled = $('.active_input_phone');
        var dispatch = $('#but_sms');
        var area_sms = $('#area_sms');

        button_penp.css('display', 'none');
        button_cen.fadeIn();
        button_save.fadeIn();
        dispatch.fadeIn();
        area_sms.fadeIn();
        enabled.prop('disabled', false).css('background-color', '#ffffff');
        $(document).on('click', '#cencel_but_phone', function () {
            button_cen.hide();
            button_save.hide();
            dispatch.hide();
            area_sms.hide();
            button_penp.css('display', 'block');
            enabled.prop('disabled', true).css('background-color', '#f5f5f5');
        });
    });

    /*email*/

    $(document).on('click', '.pen_wrap_email', function () {
        var button_cen = $('#but_email');
        var button_penm = $('.pen_wrap_email');
        var button_save = $('#but_email_com');
        var enabled = $('.active_email');

        button_cen.fadeIn();
        button_save.fadeIn();
        button_penm.css('display', 'none');
        enabled.prop('disabled', false).css('background-color', '#ffffff');
        $(document).on('click', '#but_email', function () {
            button_cen.hide();
            button_save.hide();
            button_penm.css('display', 'block');
            enabled.prop('disabled', true).css('background-color', '#f5f5f5');
        });
    });

    /*password*/

    $(document).on('click', '.pen_wrap_pass', function () {
        var button_cen = $('#but_pass_cenl');
        var button_hidee_pass_on = $('.butt_hidee_pass_on');
        var button_viev_pass_on = $('.butt_viev_pass_on');
        var button_hidee_pass_to = $('.butt_hidee_pass_to');
        var button_viev_pass_to = $('.butt_viev_pass_to');
        var button_pen = $('.pen_wrap_pass');
        var button_save = $('#but_pass_save');
        var enabled = $('.active_pass_input');
        var hid_text_pass = $('.hidden_text_pass');
        var new_pass = $('.active_block');
        var form_block = $('.form_cred_but');
        var hide_text = $('#show_text');

        button_hidee_pass_on.css('display', 'block');
        button_hidee_pass_to.css('display', 'block');
        button_pen.css('display', 'none');
        button_cen.fadeIn();
        button_save.fadeIn();
        hide_text.addClass('hide_text');
        hid_text_pass.fadeIn().css('display', 'block').css('margin-bottom', '23px');
        new_pass.fadeIn().css('display', 'block');
        form_block.css('height', '100%');
        enabled.prop('disabled', false).css('background-color', '#ffffff');
        $(document).on('click', '.butt_hidee_pass_on', function () {
            button_hidee_pass_on.css('display', 'none');
            button_viev_pass_on.css('display', 'block');
        });
        $(document).on('click', '.butt_viev_pass_on', function () {
            button_hidee_pass_on.css('display', 'block');
            button_viev_pass_on.css('display', 'none');
        });
        $(document).on('click', '.butt_hidee_pass_to', function () {
            button_hidee_pass_to.css('display', 'none');
            button_viev_pass_to.css('display', 'block');
        });
        $(document).on('click', '.butt_viev_pass_to', function () {
            button_hidee_pass_to.css('display', 'block');
            button_viev_pass_to.css('display', 'none');
        });
        $(document).on('click', '#but_pass_cenl', function () {
            button_cen.hide();
            button_save.hide();
            hid_text_pass.hide();
            new_pass.hide();
            button_pen.css('display', 'block');
            hide_text.addClass('show_text');
            button_hidee_pass_on.css('display', 'none');
            button_hidee_pass_to.css('display', 'none');
            button_viev_pass_on.css('display', 'none');
            button_viev_pass_to.css('display', 'none');
            enabled.prop('disabled', true).css('background-color', '#f5f5f5');
            if (enabled.type == 'password') { }
            else {
                enabled.type = "password";
            }

        });
    });
});

$('.content__search__left input').keyup(function() {
    if (event.keyCode === 13) {
        location.href = '../Home/Clients#' + $(this).val();
    }
});
$('.content__search__right input').keyup(function() {
    if (event.keyCode === 13) {
        location.href = '../Home/Sales#' + $(this).val();
    }
});


$('.collapse-detail').click(function () {
    if (!$(this).hasClass('open-collapse')) {
        $('.colapse-box[for="' + $(this).attr('id') + '"]').slideDown('slow');
        $(this).addClass('open-collapse');
    } else {
        $('.colapse-box[for="' + $(this).attr('id') + '"]').slideUp('slow');
        $(this).removeClass('open-collapse');
    }

});

$('.smsText').focus(function () {
    $('.sms-length.sms-grey').addClass('sms-blue');
    $('.sms-length.sms-grey').removeClass('sms-grey');
});
$('.smsText').focusout(function () {
    $('.sms-length.sms-blue').addClass('sms-grey');
    $('.sms-length.sms-blue').removeClass('sms-blue');
});

setInterval(function () {
    if ($('#shopListSelect .nice-select').hasClass('open')) {
        $(".param-info p[for='shopListSelect']").addClass('blue-text');
        $(".param-info p[for='shopListSelect']").removeClass('grey-text');
    } else {
        $(".param-info p[for='shopListSelect']").addClass('grey-text');
        $(".param-info p[for='shopListSelect']").removeClass('blue-text');
    }
    if ($('#ItemListSelect .nice-select').hasClass('open')) {
        $(".param-info p[for='ItemListSelect']").addClass('blue-text');
        $(".param-info p[for='ItemListSelect']").removeClass('grey-text');
    } else {
        $(".param-info p[for='ItemListSelect']").addClass('grey-text');
        $(".param-info p[for='ItemListSelect']").removeClass('blue-text');
    }
    if ($('#clientListSelect .nice-select').hasClass('open')) {
        $(".param-info p[for='clientListSelect']").addClass('blue-text');
        $(".param-info p[for='clientListSelect']").removeClass('grey-text');
    } else {
        $(".param-info p[for='clientListSelect']").addClass('grey-text');
        $(".param-info p[for='clientListSelect']").removeClass('blue-text');
    }
}, 300);

var options = {
    twentyFour: true,
    upArrow: 'wickedpicker__controls__control-up',
    downArrow: 'wickedpicker__controls__control-down',
    close: 'wickedpicker__close',
    hoverState: 'hover-state',
    title: 'Timepicker',
    showSeconds: false,
    secondsInterval: 1,
    minutesInterval: 1,
    beforeShow: null,
    show: null, 
};

$(document).ready(function () {
    addLiveSearchToStatic();
});

/* active table */
$(document).ready(function () {
    $('.godshop_add_cont .active').click(function () {
        if (!$('#goodshopTable').hasClass('active_table')) {
            $('#goodshopTable').addClass('active_table');
            $('.godshop_add_cont').addClass('active_addblock');
            $('#add_good_submit').removeAttr('disabled');
            $('.godshop_add_cont #add_good_submit').removeAttr('disabled');
        } else {
            $('#goodshopTable').removeClass('active_table');
            $('.godshop_add_cont').removeClass('active_addblock');
            $('.godshop_add_cont #add_good_submit').attr('disabled', 'disabled');
        }
    });
    $('#checkbox0').change(function () {
        if (this.checked) {
            $('#goodshopTable').find(":checkbox").attr("checked", "checked");
        } else {
            $('#goodshopTable').find(":checkbox").removeAttr("checked", "checked");
        }
    });


    $('.shops_add_fbut').click(function () {
        if (!$('#shopsTable').hasClass('active_table')) {
            $('#shopsTable').addClass('active_table');
            $('.shops_add_block').addClass('active_addblock')
            $('.shops_add_block .add_shop_submit').removeAttr('disabled');
        } else {
            $('#shopsTable').removeClass('active_table');
            $('.shops_add_block').removeClass('active_addblock');
            $('.shops_add_block .add_shop_submit').attr('disabled', 'disabled');
        }
    });
    $('.tab_shops').click(function () {
        $('.shopvievbuttom').css('display', 'flex');
    });
    $('.tab_mylist_shop').click(function () {
        $('.shopvievbuttom').css('display', 'none');
    });
    $('#checkbox0').change(function () {
        if (this.checked) {
            $('#shopsTable').find(":checkbox").attr("checked", "checked");
        } else {
            $('#shopsTable').find(":checkbox").removeAttr("checked", "checked");
        }
    });
});




function addLiveSearchToStatic() {
    if ($('*').is('#lsearch1')) {
        $.ajax({
            url: '/bas/lists.txt',
            type: "GET",
            success: function (response) {
                var liveSearchData = JSON.parse(response);
                addAutoCompliteToListData('#lsearch1', liveSearchData["dataShops"]);
                addAutoCompliteToListData('#lsearch2', liveSearchData["dataCatprod"]);
                addAutoCompliteToListData('#lsearch3', liveSearchData["dataActions"]);
                addAutoCompliteToListData('#lsearch4', liveSearchData["dataActsms"]);
            }
        });
    }
}

$('select[name="addactionList"]').change(function () {
    var el = $(this).val();
    if (el == 1) {
        alert = "123";
    }
});

function addAutoCompliteToListData(elem, data) {
    $(elem).autocomplete({
        minLength: 0,
        source: data,
        focus: function (event, ui) {
            $(elem).val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $(elem).val(ui.item.label);
            $("#project-id").val(ui.item.value);
            return false;
        },
        search: function (event, ui) {
            $(this).addClass('serching');

        },
        close: function (event, ui) {
            $(this).removeClass('serching');
        },
    }).autocomplete("instance")._renderItem = function (ul, item) {
        return $("<li>").append("<div>" + item.label + "</div>").appendTo(ul);
    };
}

// Функция вызывающая диалог подтверждения, через Promise объект
function ConfirmDialog(message) {
    return new Promise(function (resolve, reject) {
        $("#BtnYesConfirmDialog").click(function () {
            $('#ConfirmDialog').hide();
            resolve(true);
        });
        $("#BtnNoConfirmDialog").click(function () {
            $('#ConfirmDialog').hide();
            resolve(false);
        });
        $('#confirm-dialog-message').html(message);
        $('#ConfirmDialog').show();
    });
}

//Вызывает диалог "Ошибка"
// param header - заголовок диалогового окна
// param message - текст ошибки
function DialogError(header, message) {
    $("#modal_ochibka").html(header);
    $("#dialog_error_message").html(message);
    $("#DialogError").show();
}

//Вызывает диалог "Успех"
// param message - текст сообщения для пользователя
function DialogSuccess(message) {
    $("#dialog_success_message").html(message);
    $("#DialogSuccess").show();
}