$("#sendRegisterCode").click(function (e) {
    e.preventDefault();
    var phone = $("#Phone").val();
    phone = phone.replace("+7", "").replace(/\D/g, "");
    if (phone.length === 10) {
        $.ajax({
            type: "POST",
            url: "../Home/AjaxRegister",
            data: '{phone:\'' + phone + '\'}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function () {
                console.log("send code");
                document.getElementById("Phone").style.borderColor = "#C5CBD1";
                smsCodeMessage('Отправлен смс-код', "#11B9A3");
                //var regSubmit = document.getElementById('TerminalRegisterSubmit');
                //regSubmit.disabled = 0;
            }
        });
    }
    else {
        document.getElementById("Phone").style.borderColor = "#D4614A";
    }
});

function smsCodeMessage(innerTxt, colorCode) {
    var codeMessage = document.getElementById('TerminalModalWrongSmsCode');
    codeMessage.innerHTML = innerTxt;
    codeMessage.style.color = colorCode;
    codeMessage.style.display = "block";
}

function activateBuyButton() {
    var inputClientIdPurchaseTerminal = document.getElementById('clientIdPurchaseTerminal');
    var inputTerminalBuySum = document.getElementById('TerminalBuySum');
    var inputTerminalBuySubmit = document.getElementById("TerminalBuySubmit");
    var phone = document.getElementById("profilePhone");
    if (inputClientIdPurchaseTerminal.value.length > 0 && inputTerminalBuySum.value.length > 0 && phone.value.length > 0) {
        inputTerminalBuySubmit.disabled = 0;
    }
    else {
        inputTerminalBuySubmit.disabled = 1;
    }
}

function activateRefundButton() {
    var inputclientIdRefundTerminal = document.getElementById('clientIdRefundTerminal');
    var inputTerminalRefundChequeSum = document.getElementById('TerminalRefundChequeSum');
    var inputTerminalRefundChequeNum = document.getElementById('TerminalRefundChequeNum');
    var inputTerminalRefundDate = document.getElementById('TerminalRefundDate');
    var inputTerminalRefundSubmit = document.getElementById("TerminalRefundSubmit");
    var phone = document.getElementById("profilePhone");
    if (inputclientIdRefundTerminal.value.length > 0 &&
        inputTerminalRefundChequeSum.value.length > 0 &&
        inputTerminalRefundChequeNum.value.length > 0 &&
        inputTerminalRefundDate.value.length > 0 &&
        phone.value.length > 0) {
        inputTerminalRefundSubmit.disabled = 0;
    }
    else {
        inputTerminalRefundSubmit.disabled = 1;
    }
}

var inputTerminalRegisterSmsCode = document.getElementById('TerminalRegisterSmsCode');
inputTerminalRegisterSmsCode.oninput = function () {
    //console.log(inputTerminalRegisterSmsCode.value);
    //alert(inputTerminalRegisterSmsCode.value);
    if (inputTerminalRegisterSmsCode.value.length === 4) {
        var code = inputTerminalRegisterSmsCode.value;
        var phone = $("#Phone").val();
        $.ajax({
            type: "POST",
            url: "../Home/TerminalConfirmCode",
            data: '{phone: \'' + phone + '\', code: \'' + code + '\'}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (confirmCodeResult) {
                if (confirmCodeResult.ErrorCode === 0) {
                    var regSubmit = document.getElementById('TerminalRegisterSubmit');
                    regSubmit.disabled = 0;
                    smsCodeMessage('Верный смс-код', "#11B9A3");
                }
                else {
                    smsCodeMessage("Неверный смс-код", "#e31e24");
                }
            }
        })
    }
}

$("#searchClientPurchaseTerminal").click(function (e) {
    e.preventDefault();
    var clientinfo = $("#clientIdPurchaseTerminal").val();
    searchClient(clientinfo);
});

$("#TerminalAddBuy").submit(function (e) {
    e.preventDefault();
    var url = "../Home/TerminalChequeAdd";
    //$("#profileCard").val
    var card = document.getElementById("profileCard").value;
    var amount = document.getElementById("TerminalBuySum").value;
    var paidbybonus = document.getElementById("TerminalRedeem").value;
    var maxRedeem = document.getElementById("TerminalMaxSumRedeem").value;
    $.ajax({
        type: "POST",
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: '{Card: \'' + card + '\', Amount: \'' + amount + '\', PaidByBonus: \'' + paidbybonus + '\', MaxRedeem: \'' + maxRedeem + '\'}',
        success: function (data) {
            console.log(data);
            if (data != "") {
                //var chequeAdd = JSON.parse(data);
                if (data.ErrorCode == 0) {
                    document.getElementById("TerminalModalChequeAddAmount").innerText = data.Amount;
                    document.getElementById("TerminalModalChequeAddBonusAdd").innerText = data.BonusAdd;
                    document.getElementById("TerminalModalChequeAddBonusRedeemed").innerText = data.BonusRedeem;
                    document.getElementById("TerminalModalChequeAddCash").innerText = data.Cash;
                    var modal = document.getElementById("TerminalModalChequeAdd");
                    var btn = document.getElementById("TerminalModalChequeAddClose");
                    modal.style.display = "block";
                    btn.onclick = function () {
                        modal.style.display = "none";
                    }
                    // When the user clicks anywhere outside of the modal, close it
                    window.onclick = function (event) {
                        if (event.target == modal) {
                            modal.style.display = "none";
                        }
                    }

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
                }
                else {
                    var modal = document.getElementById("TerminalModalOperationError");
                    var btn = document.getElementById("TerminalModalOperationErrorClose");
                    modal.style.display = "block";
                    btn.onclick = function () {
                        modal.style.display = "none";
                    }
                    // When the user clicks anywhere outside of the modal, close it
                    window.onclick = function (event) {
                        if (event.target == modal) {
                            modal.style.display = "none";
                        }
                    }
                }
            }
        }
    })
});

$("#TerminalRefund").submit(function (e) {
    e.preventDefault();
    var url = "../Home/TerminalRefund";
    var card = document.getElementById("profileCard").value;
    var chequesum = document.getElementById("TerminalRefundChequeSum").value;
    var chequedate = document.getElementById("TerminalRefundDate").value;
    var chequenum = document.getElementById("TerminalRefundChequeNum").value;
    $.ajax({
        type: "POST",
        url: url,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        data: '{Card: \'' + card + '\', ChequeDate: \'' + chequedate + '\', ChequeNum: \'' + chequenum + '\', ChequeSum: \'' + chequesum + '\'}',
        success: function (data) {
            console.log(data);
            if (data != "") {
                var refund = JSON.parse(data);
                if (refund.ErrorCode == 0) {
                    document.getElementById("TerminalModalRefundAmount").innerText = (-1) * refund.Amount;
                    document.getElementById("TerminalModalRefundBonusAdd").innerText = refund.Added;
                    document.getElementById("TerminalModalRefundBonusRedeem").innerText = refund.Redeemed;
                    var modal = document.getElementById("TerminalModalRefund");
                    var btn = document.getElementById("TerminalModalRefundClose");
                    modal.style.display = "block";
                    btn.onclick = function () {
                        modal.style.display = "none";
                    }
                    // When the user clicks anywhere outside of the modal, close it
                    window.onclick = function (event) {
                        if (event.target == modal) {
                            modal.style.display = "none";
                        }
                    }
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
                }
                else {
                    var modal = document.getElementById("TerminalModalOperationError");
                    var btn = document.getElementById("TerminalModalOperationErrorClose");
                    modal.style.display = "block";
                    btn.onclick = function () {
                        modal.style.display = "none";
                    }
                    // When the user clicks anywhere outside of the modal, close it
                    window.onclick = function (event) {
                        if (event.target == modal) {
                            modal.style.display = "none";
                        }
                    }
                }
            }
            else {
                var modal = document.getElementById("TerminalModalOperationError");
                var btn = document.getElementById("TerminalModalOperationErrorClose");
                modal.style.display = "block";
                btn.onclick = function () {
                    modal.style.display = "none";
                }
                // When the user clicks anywhere outside of the modal, close it
                window.onclick = function (event) {
                    if (event.target == modal) {
                        modal.style.display = "none";
                    }
                }
            }
        }
    })
});

$("#TerminalRegisterNewUser").submit(function (e) {
    e.preventDefault();
    var url = "../Home/Terminal";
    $.ajax({
        type: "POST",
        url: url,
        dataType: "json",
        data: $("#TerminalRegisterNewUser").serialize(),
        success: function (data) {
            console.log(data.ErrorCode);
            if (data.ErrorCode == 4) {
                var modalErrorCode = document.getElementById('TerminalModalWrongSmsCode');
                modalErrorCode.style.display = "block";
            }
            else if (data.ErrorCode > 0) {
                console.log("error");
                var modal = document.getElementById('TerminalModalErrorRegister');

                // Get the button that opens the modal
                var btn = document.getElementById("TerminalModalErrorRegisterClose");
                modal.style.display = "block";
                btn.onclick = function () {
                    modal.style.display = "none";
                }
                // When the user clicks anywhere outside of the modal, close it
                window.onclick = function (event) {
                    if (event.target == modal) {
                        modal.style.display = "none";
                    }
                }
            }
            else {
                console.log("success");
                document.getElementById("TerminalRegisterNewUserName").value = "";
                document.getElementById("TerminalRegisterNewUserSurname").value = "";
                document.getElementById("TerminalRegisterNewUserPatronymic").value = "";
                document.getElementById("Phone").value = "";
                document.getElementById("TerminalRegisterNewUserEmail").value = "";
                document.getElementById("TeminalBirthDate").value = "";
                document.getElementById("TerminalRegisterSmsCode").value = "";
                document.getElementById("TerminalModalWrongSmsCode").style.display = "none";
                $("#TerminalRegisterNewUserGender").val("null").niceSelect('update');

                var modal = document.getElementById("TerminalModalSuccessRegister");
                modal.style.display = "block";
                var btn = document.getElementById("TerminalModalSuccessRegisterClose");
                btn.onclick = function () {
                    modal.style.display = "none";
                }
                window.onclick = function (event) {
                    if (event.target == modal) {
                        modal.style.display = "none";
                    }
                }
            }
        }
    })
});

$("#TerminalRedeem").bind('input', function () {
    var maxRedeem = parseInt(document.getElementById("TerminalMaxSumRedeem").value, 10);
    var redeem = parseInt($(this).val(), 10);
    if (maxRedeem >= 0 && redeem > maxRedeem) {
        $(this).val(maxRedeem);
    }
    console.log($(this).val());
});

$("#TerminalRedeemInBuy").click(function (e) {
    e.preventDefault();
    var summBuy = $("#TerminalBuySum").val();
    var card = $("#profileCard").val();
    //if (summBuy > 0) {
    $.ajax({
        type: "POST",
        url: "../Home/TerminalMaxSumRedeem",
        data: '{Card: \'' + card + '\', ' + 'Sum: \'' + summBuy + '\' }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (maxSumRedeemResult) {
            $("#TerminalMaxSumRedeem").val(maxSumRedeemResult.MaxSum);
            console.log(maxSumRedeemResult);
        },
        error: function (message) {
            console.log(message);
        }
    })
    //}
});

function searchClient(clientInfo) {
    $.ajax({
        type: "POST",
        url: "../Home/SearchClient",
        data: '{searchClient: \'' + clientInfo + '\'}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (clientInfoResult) {
            if (clientInfoResult.ErrorCode == 0) {
                $("#profileName").val(clientInfoResult.Name);
                $("#profileSurname").val(clientInfoResult.Surname);
                document.getElementById("profilePatronymic").value = clientInfoResult.Patronymic;
                var birthDate = new Date(clientInfoResult.Birthdate);
                if (birthDate > new Date('01-01-1900')) {
                    document.getElementById("profileBirthDate").value = $.format.date(clientInfoResult.Birthdate, "dd.MM.yyyy");
                }
                $("#profilePhone").val(clientInfoResult.Phone).mask("+7 (999) 999-99-99");
                $("#profileEmail").val(clientInfoResult.Email);
                $("#profileCard").val(clientInfoResult.Card);
                $("#profileBalance").val(clientInfoResult.FullBalance);
                $("#profileLevel").val(clientInfoResult.Condition);
                if (clientInfoResult.Gender == 1) {
                    $("#profileGender").val("1").niceSelect('update');
                }
                $("#profileLastPurchaseAmount").val(clientInfoResult.LastPurchaseAmount);
                var date = new Date(clientInfoResult.LastPurchaseDate);
                if (date > new Date('01-01-1900')) {
                    document.getElementById("profileLastPurchaseDate").value = $.format.date(clientInfoResult.LastPurchaseDate, "dd.MM.yyyy");
                }
                //document.getElementById("profileBirthDate").value = new Date(parseInt(clientInfoResult.Birthdate.substr(6)));
                var table = $('table#TerminalCheques').DataTable();
                table.clear().draw();
                $.ajax({
                    type: "POST",
                    url: "../Home/TerminalGetCheques",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: '{Card: \'' + clientInfoResult.Card + '\'}',
                    success: function (cheques) {
                        table.rows.add(cheques).draw();
                    }
                })
            }
            else if (clientInfoResult.ErrorCode == 6) {
                var modal = document.getElementById('TerminalModalClientCoincidence');

                // Get the button that opens the modal
                var btn = document.getElementById("TerminalModalClientCoincidenceClose");
                modal.style.display = "block";
                btn.onclick = function () {
                    modal.style.display = "none";
                }
                // When the user clicks anywhere outside of the modal, close it
                window.onclick = function (event) {
                    if (event.target == modal) {
                        modal.style.display = "none";
                    }
                }
            }
            else {
                var modal = document.getElementById('TerminalModalClientNotFound');

                // Get the button that opens the modal
                var btn = document.getElementById("TerminalModalClientNotFoundClose");
                modal.style.display = "block";
                btn.onclick = function () {
                    modal.style.display = "none";
                }
                // When the user clicks anywhere outside of the modal, close it
                window.onclick = function (event) {
                    if (event.target == modal) {
                        modal.style.display = "none";
                    }
                }
            }
        }
    })
}

$("#searchClientRefundTerminal").click(function (e) {
    e.preventDefault();
    var clientinfo = $("#clientIdRefundTerminal").val();
    searchClient(clientinfo);
});

