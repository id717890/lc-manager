$(document).ready(function () {
    $(".openstSales").on('click', 'div', function () {
        $(this).toggleClass('opens');
        $(this).next().toggleClass('open');
    });
});

$(document).ready(function () {
    $(".openst").on('click', '.client_table_item', function () {
        if ($(this).next("div").is("openinnfo")) {
            $(this).next("div").slideDown("slow");
            $(this).removeClass('opens');
        } else if ($(this).is(".opens")) {
            $(this).next("div").slideToggle("slow");
            $(this).removeClass('opens');
        } else {
            $(".openst .client_table_info").slideUp("slow");
            $(this).next("div").slideToggle("slow");
            $(".openst .client_table_item").removeClass('opens');
            $(this).addClass('opens');
        }
    });
});

$(".dial").knob();
$({ animatedVal: 0 }).animate({ animatedVal: 70 }, {
    duration: 2000,
    easing: "swing",
    step: function () {
        $(".dial").val(Math.ceil(this.animatedVal)).trigger("change");
    }
});

$(".actions-block").on('click', '.openblocklist', function () {
    var cilscrt = $(this).parent(1).prev('.actions-block-content')[0].scrollHeight;
    $(this).parent(1).parent(1).toggleClass("opens");
    $(this).parent(1).prev('.actions-block-content')[0].style.height = cilscrt + 'px';
    if (!$(this).parent(1).parent(1).hasClass("opens")) {
        $(this).parent(1).prev('.actions-block-content')[0].style.height = '180px';
    }
});

$(".addclientlist-block").on('click', '.openblocklist', function () {
    var cilscrt = $(this).parent(1).prev('.addclientlist-block-content')[0].scrollHeight;
    $(this).parent(1).parent(1).toggleClass("opens");
    $(this).parent(1).prev('.addclientlist-block-content')[0].style.height = cilscrt + 'px';
    if (!$(this).parent(1).parent(1).hasClass("opens")) {
        $(this).parent(1).prev('.addclientlist-block-content')[0].style.height = '0';
    }
});

//$(".datepicker").datepicker();
$(".clear-date").click(function () {
    $('#date2').val('');
    $('#ClientsClientCreateBirthdate').val('');
});

$(function () {
    $(".client_nav a").click(function(e) {
        e.preventDefault();
        var tab = $(".form-terminal__tabs-content > div");
        var thisTab = $(this).attr('href');

        tab.removeClass('active');
        $(thisTab).addClass('active');

        $(".client_nav div a").removeClass('this-p');
        $(this).addClass('this-p');
    });
});

$("#Phone").mask("+7 (999) 999-99-99");

$("#FriendPhone").mask("+7 (999) 999-99-99");

$("#profilePhone").mask("+7 (999) 999-99-99");

$("#ClientsClientCreatePhone, .modal_client_change_form .phone").mask("+7 (999) 999-99-99");
$("#activate_modal_phone").mask("+7 (999) 999-99-99");

$("#activateCard").click(function (ev) {
    ev.preventDefault();
    var modal = document.getElementById("ModalActivateCard");
    modal.style.display = "block";
    // When the user clicks anywhere outside of the modal, close it
    window.onclick = function (event) {
        if (event.target === modal) {
            modal.style.display = "none";
        }
    }
});



$("#TerminalBuyRefresh, #TerminalRefundRefresh").click(function (e) {
    e.preventDefault();
    $("#profileName").val("");
    $("#profileSurname").val("");
    document.getElementById("profilePatronymic").value = "";
    document.getElementById("profileBirthDate").value = "";
    $("#profilePhone").val("");
    $("#profileEmail").val("");
    $("#profileCard").val("");
    $("#profileBalance").val("");
    $("#profileLevel").val("");
    $("#profileGender").val("null").niceSelect('update');
    $("#profileLastPurchaseAmount").val("");
    document.getElementById("profileLastPurchaseDate").value = "";
    var table = $('table#TerminalCheques').DataTable();
    table.clear().draw();
    document.getElementById("clientIdRefundTerminal").value = "";
    document.getElementById("clientIdPurchaseTerminal").value = "";
    document.getElementById("TerminalBuySum").value = "";
    document.getElementById("TerminalMaxSumRedeem").value = "";
	document.getElementById("TerminalRedeem").value = "";
	document.getElementById("TerminalRefundDate").value = "";
	document.getElementById("TerminalRefundChequeNum").value = "";
	document.getElementById("TerminalRefundChequeSum").value = "";
	document.getElementById("TerminalBuySubmit").disabled = 1;
	document.getElementById("TerminalRefundSubmit").disabled = 1;
});

$('.timeinput').each(function () {
    $(this).timepicker();
});