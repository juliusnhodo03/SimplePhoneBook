

function cancelBankAccount(indexUrl) {
    var windowId = "ConfirmWindow";
    var header = "Confirm";
    var errorWarning = "";
    var message = "Bank Account is not yet saved&nbsp;Do you want to continue?";
    showMessageBox(windowId, header, errorWarning, message, true, indexUrl);
}



function showMessageBox(windowId, header, errorWarning, message, isCancelConfirmed, indexUrl) {
    var kendoWindow = $("<div />").kendoWindow({
        title: header,
        resizable: false,
        modal: true,
        width: 430,
        height: 160
    });

    kendoWindow.data("kendoWindow")
        .content($("#" + windowId).html())
        .center().open();

    $(".confirmation-message").html(message);
    $(".headerTitle").html(errorWarning);

    kendoWindow
        .find(".no-cancel,.confirm-cancel")
        .click(function () {
            if ($(this).hasClass("confirm-cancel")) {
                if (isCancelConfirmed) {
                    window.location.href = indexUrl;
                }
            }

            kendoWindow.data("kendoWindow").close();
        })
        .end();
}


var kendoWindowLoader = $("<div />").kendoWindow({
    resizable: false,
    modal: true,
    width: 380,
    height: 160
});


function showLoadingBox(windowId, message) {
    kendoWindowLoader.data("kendoWindow")
        .content($("#" + windowId).html())
        .center().open();
    $(".confirmation-message").html(message);

    kendoWindowLoader
        .find(".no-cancel,.confirm-cancel")
        .click(function () {
            kendoWindowLoader.data("kendoWindow").close();
        })
        .end();
}


function loadingWindow(message) {
    var windowId = "loadingWindow";
    showLoadingBox(windowId, message);
    $("#loadingWindow").find(".k-window-action").css("visibility", "hidden");
}


function validationMessageBox(windowId, header, errorWarning, message, width, height, processName) {
    var kendoWindow = $("<div />").kendoWindow({
        title: header,
        resizable: false,
        modal: true,
        width: width,
        height: height
    });

    kendoWindow.data("kendoWindow")
        .content($("#" + windowId).html())
        .center().open();

    $(".confirmation-message").html(message);
    $(".headerTitle").html(errorWarning);

    kendoWindow
        .find(".no-cancel,.confirm-cancel")
        .click(function () {
            if ($(this).hasClass("confirm-cancel")) {
                $("#btnExitToIndex").click();
            }

            kendoWindow.data("kendoWindow").close();
        })
        .end();
}


function saveBankAccount(actionUrl, processType, indexUrl, productCreateUrl) {

    var hasValidationFailed = false;
    var message = "";

    var allContainers = $(".child");
    var countCountainers = 0;

    var siteId = ($("#SiteId").length == 0) ? $("#hdSiteId").attr("value") : $("#SiteId").val();


    //Get All Account Numbers for the current site
    var bankAccounts = $.getSiteAccountNumbers("../BankAccount/GetSiteBankAccouns/?siteId=" + siteId);
    
    var bankAccountHolderDto = {
        SiteId: siteId,
        CitCode: $("#CitCode").val(),
        ProcessType: processType,
        Accounts: []
    };

    var loadingMessage = "Saving";
    loadingWindow("Please wait...<br>" + loadingMessage + " Site Account");

    message += "<ul style='list-style-image:url(" + resolveUrl("../Content/images/bullet.jpg", "captured") + ") !important;'>\n";

    allContainers.each(function () {

        // Enumerate each Container
        countCountainers++;
        var bankId = $(this).find("#ddlBank").val();
        var accountTypeId = $.trim($(this).find('#ddlAccountType').val());
        var accountHolderName = $.trim($(this).find('#txtAccountHolderName').val());
        var accountNumber = $.trim($(this).find('#txtAccountNumber').val());


        if (siteId == '' || siteId == -1 || siteId == 0) {
            message += "<li>Bank Account " + countCountainers + " - Site</li>\n";
            hasValidationFailed = true;
        }


        if (bankId == '' || bankId == -1 || bankId == 0 || bankId == 1000500) {
            message += "<li>Bank Account " + countCountainers + " - Bank</li>\n";
            hasValidationFailed = true;
        }


        if (accountTypeId == '' || accountTypeId == -1 || accountTypeId == 0) {
            message += "<li>Bank Account " + countCountainers + " - Account Type</li>\n";
            hasValidationFailed = true;
        }

        if (accountHolderName == '' || accountHolderName == -1) {
            message += "<li>Bank Account " + countCountainers + " - Account Holder Name</li>\n";
            hasValidationFailed = true;
        }

        if (accountNumber == '' || accountNumber == -1) {
            message += "<li>Bank Account " + countCountainers + " - Account Number</li>\n";
            hasValidationFailed = true;
        }

        if (/^[0-9]*$/.test(accountNumber) == false) {
            message += "<li>Bank Account " + countCountainers + " - Account Number Must be numeric</li>\n";
            hasValidationFailed = true;
        }

        //Create BankAccount Object for each Record
        var bankAccountObj =
        {
            SiteId: siteId,
            AccountTypeId: $.trim($(this).find('#ddlAccountType').val()),
            AccountHolderName: $.trim($(this).find('#txtAccountHolderName').val()),
            BeneficiaryCode: $.trim($(this).find('#txtBeneficiaryCode').val()),
            AccountNumber: $.trim($(this).find('#txtAccountNumber').val()),
            BankId: $.trim($(this).find('#ddlBank').val()),
            TransactionTypeId: 1,
            DefaultAccount: $(this).find('#chkDefaultAccount').is(":checked")
        };

        bankAccountHolderDto.Accounts.push(bankAccountObj);

        return true;
    });


    //ADD SCREEN ACCOUNTS TO DB ACCOUNTS LIST
    for (var i = 0; i < bankAccountHolderDto.Accounts.length; i++) {
        bankAccounts.push(bankAccountHolderDto.Accounts[i].AccountNumber);
    };


    //Check Duplicate BankAccounts from muliple captured Bank Accounts.
    var hasDuplicateAccounts = isDuplicateBank(bankAccounts);
    if (hasDuplicateAccounts != "") {
        message += "<li>Bank is duplicated: Account Number: " + hasDuplicateAccounts + "</li>\n";
        hasValidationFailed = true;
    }


    //Fire the Validation Window
    if (hasValidationFailed == true) {
        message += "</ul>";

        var windowId = "ValidationWindow";
        var header = "Field Validation";
        var errorWarning = "Please fill in required fields:";

        validationMessageBox(windowId, header, errorWarning, message, 450, 300);

        if (processType == "captured" || processType == "Continue") {
            $("#btnConfirmCancel").click();
        }

        return true;
    }


    if (hasValidationFailed == true) {
        return true;
    }

    var obj = { bankAccounts: bankAccountHolderDto };

    //call server method
    $.ajax(
    {
        type: "POST",
        url: actionUrl,
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        complete: function (result) {

            if (processType == "captured") {
                // Close Loading Indicator
                $("#btnConfirmCancel").click();
                window.location.href = indexUrl + "?load=true";
            } else {
                // Close Loading Indicator
                $("#btnConfirmCancel").click();
                window.location.href = productCreateUrl + "?siteId=" + siteId;
            }

        },
    }); // End $.ajax method

    return false;
}



function resolveUrl(actualUrl, processName) {
    var url = "";

    if (processName == "ceptured") {
        url = "../" + actualUrl;
    }
    else {
        url = actualUrl;
    }
    return url;
}


function isDuplicateBank(accounts) {

    for (var i = 0; i < accounts.length; i++) {
        var temp = accounts[i];

        for (var k = i + 1; k < accounts.length; k++) {
            if (temp == accounts[k]) {
                return temp;
            }
        }
    }
    return "";
}


//
jQuery.extend({
    getSiteAccountNumbers: function (url) {
        var result = null;
        $.ajax({
            url: url,
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (data) {
                result = data.Accounts;
            }
        });
        return result;
    }
});