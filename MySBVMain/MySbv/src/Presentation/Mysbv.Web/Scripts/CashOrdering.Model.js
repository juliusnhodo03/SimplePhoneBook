if (sessionStorage.initialFiles === undefined) {
    sessionStorage.initialFiles = "[]";
}
var initialFiles = JSON.parse(sessionStorage.initialFiles);


function saveCashOrder(navigationUrl, listUrl, statusName) {
    //
    // Container Information
    //
    
    var container = $("#CashOrderContainer").find(".CombinedContainerDrops");
    
    var containerObject =
    {
    	CashOrderContainerId: $('#hdCashOrderContainerId').val(),
    	Amount: $('#CashOrderAmount').val(),
    	SerialNumber: $('#ContainerNumberWithCashForExchange').val(),
    	CashOrderContainerDrops: []
    };
	    
    //
    // Container Drops
    //
    var containerDrops = container.find(".CashForwarded, .CashRequired");

    containerDrops.each(function () {

        var notesTotal = $(this).find("input[name='textNotesSubtotal']").val() ;
        var coinsTotal = $(this).find("input[name='textCoinsSubtotal']").val();

        var hdContainerDropId = $(this).find("input[name='hdContainerDropId']").val();
		
        var containerDropObject =
        {
        	CashOrderContainerDropId: hdContainerDropId,
        	CashOrderContainerId: containerObject.CashOrderContainerId,
            Amount: parseFloat(coinsTotal) + parseFloat(notesTotal),
            IsCashRequiredInExchange: $(this).hasClass("CashRequired"),
            IsCashForwardedForExchange: $(this).hasClass("CashForwarded"),
            CashOrderContainerDropItems: []
        };


        var containerDropItems = $(this).find("select");
        var dropAmount = 0;


        containerDropItems.each( function () {
            var row = $(this).parents(".clsline, .clsCoinsline");
            var noteValue = row.find("input[name='textNoteValue']");
            var coinValue = row.find("input[name='textCoinValue']");
            var noteCount = row.find("input[name='textNoteCount']");
            var coinCount = row.find("input[name='textCoinCount']");
            var hdContainerDropItemId = row.find("input[name='hdContainerDropItemId']").val();
            
            var value = noteValue.length == 0 ? coinValue : noteValue;
            var count = noteCount.length == 0 ? coinCount : noteCount;

            var containerDropItemObject =
            {
                CashOrderContainerDropItemId: hdContainerDropItemId,
                CashOrderContainerDropId: containerDropObject.CashOrderContainerDropId,
                ValueInCents: $(this).val() == 1000500 ? 0 : $(this).val(),
                Count: isNumber(count.val()) == false ? 0 : count.val(),
                Value: isNumber(value.val()) == false ? 0 : value.val()
            };

            dropAmount = dropAmount + containerDropItemObject.Value;

            //
            // Stack container Drop items.
            //
            if (containerDropItemObject.Count > 0 && containerDropItemObject.Value > 0) {
            	containerDropObject.CashOrderContainerDropItems.push(containerDropItemObject);
            }
        });

        //
        // Stack container Drops.
        //
        containerObject.CashOrderContainerDrops.push(containerDropObject);
        
    });
    
    var cashOrderObject =
    {
    	CashOrderId: $.trim($('#hdCashOrderId').val()),
    	CashOrderTypeId: $('#ddlOrderType').val(),
    	CashOrderContainerId: $('#hdCashOrderContainerId').val(),
    	SiteId: $("#ddlSites").length == 0 ? $("#hdSiteId").val() : $("#ddlSites").val(),
    	MerchantId: $('#ddlMerchants').val(),
    	ReferenceNumber: $("#hdTransactionReferenceNumber").val(),
    	DeliveryDateDto: $('#DeliveryDate').val(),
    	ContainerNumberWithCashForExchange: $.trim($('#ContainerNumberWithCashForExchange').val()),
    	EmptyContainerOrBagNumber: $.trim($('#EmptyContainerOrBagNumber').val()),
    	CashOrderAmount: $('#CashOrderAmount').val(),
    	Comments: $("#PreviousComments").val(),
    	CreatedById: $('#hdCreatedById').val(),
        CreateDate: $('#hdCreatedDate').val(),
        CapturedDateTime: $('#CapturedDateTime').val(),
        StatusName: statusName,
        CitCode: $("#CitCode").val(),
        CashOrderContainer: containerObject
        
    };

    // validate EFT orders
    if (statusName == "SUBMITTED") {
        var orderType = $('#ddlOrderType option:selected').text();
        if (orderType == "EFT") {
            hasValidEftAttachments();
            if (uploads.length > 0) {
                uploads = [];
                return false;
            }
        }
    }

    // validate CashOrder
    if (validateCashOrder(cashOrderObject) == false) {
        return false;
    }

    var saveText = (statusName == "SUBMITTED") ? "Submitting" : "Saving";
    

	loadingWindow("Please wait...<br>"+ saveText + " Cash Order");
    
	var obj = { cashOrderDto: cashOrderObject };

    //
    // Posting to server

    $.ajax(
    {
        type: "POST",
        url: navigationUrl,
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        enctype: 'multipart/form-data',
        success: function (result) {
            // Call successfull
        	if (result.ResponseCode <= 0)
        	{
                // Close Loading Indicator
                $("#btnConfirmCancel").click();
                OnErrorMessage("MessageWindow", "Error Message", "", result.Message, listUrl);
                $("#btnConfirmCancel").click();
            }
            else
        	{
                if (result.CashOrder.IsSubmitted == true) {
                	window.location.href = navigationUrl + "/?id=" + result.CashOrder.CashOrderId;
                }
                else {
                    // Close Loading Indicator
                    $("#btnConfirmCancel").click();
                    CashOrderMessage("MessageWindow", "Cash Order", "", result.Message, listUrl);
                }
            }
        },
        error: function (result) {
           // CashOrderMessage("MessageWindow", "Error Message", "", "Error Saving CashOrder! Please see Administrator", listUrl);
        	$("#btnConfirmCancel").click();
        	window.location.href = listUrl;
        }
    });
    return false;
}


//APPROVE CASH ORDER FUNCTION
function approveCashOrder(navigationUrl, listUrl, cashOrderId) {

    loadingWindow("Please wait...<br> Approving Cash Order");

    // Posting to server
    
    $.ajax(
    {
        type: "POST",
        url: navigationUrl + "/?id=" + cashOrderId,
        //data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        enctype: 'multipart/form-data',
        success: function (result) {

            // Call successfull
            if (result.ResponseCode <= 0) {
                // Close Loading Indicator
                $("#btnConfirmCancel").click();
                OnErrorMessage("MessageWindow", "Error Message", "", result.Message, listUrl);
                $("#btnConfirmCancel").click();
            }
            else {
                if (result.CashOrder.IsSubmitted == true) {
                    //window.location.href = navigationUrl + "/?id=" + result.CashOrder.CashOrderId;
                    CashOrderMessage("MessageWindow", "Cash Order", "", result.Message, listUrl);
                }
                else {
                    // Close Loading Indicator
                    $("#btnConfirmCancel").click();
                    CashOrderMessage("MessageWindow", "Cash Order", "", result.Message, listUrl);
                }
            }
        },
        error: function (result) {
            // CashOrderMessage("MessageWindow", "Error Message", "", "Error Saving CashOrder! Please see Administrator", listUrl);
            $("#btnConfirmCancel").click();
            window.location.href = listUrl;
        }
    });
    return false;
}


//REJECT CASH ORDER FUNCTION
function rejectCashOrder(navigationUrl, listUrl, cashOrderID, previousComments, currentComments) {
   
    if ($.trim(currentComments) == "") {
        var title = "Cash Order Validation";
        var message = "Please enter Comments to Reject Approval.";
        OnErrorMessage("MessageWindow", title, "", message, listUrl);
        return false;
    }
    
    var obj = { cashOrderId: cashOrderID, prevComments: previousComments, newComments: currentComments };

    //Show Please wait window
    loadingWindow("Please wait...<br> Declining Cash Order");

    //
    // Posting to server

    $.ajax(
    {
        type: "POST",
        url: navigationUrl,
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        enctype: 'multipart/form-data',
        success: function (result) {
            // Call successfull
            if (result.ResponseCode <= 0) {
                // Close Loading Indicator
                $("#btnConfirmCancel").click();
                OnErrorMessage("MessageWindow", "Error Message", "", result.Message, listUrl);
                $("#btnConfirmCancel").click();
            }
            else {
                // Close Loading Indicator
                $("#btnConfirmCancel").click();
                CashOrderMessage("MessageWindow", "Cash Order", "", result.Message, listUrl);
            }
        },
        error: function (result) {
            // CashOrderMessage("MessageWindow", "Error Message", "", "Error Saving CashOrder! Please see Administrator", listUrl);
            $("#btnConfirmCancel").click();
            window.location.href = listUrl;
        }
    });
    return false;
}


function onSelect(e) {
    var currentInitialFiles = JSON.parse(sessionStorage.initialFiles);
    for (var i = 0; i < e.files.length; i++) {
        var current = {
            name: e.files[i].name,
            extension: e.files[i].extension,
            size: e.files[i].size
        }
        currentInitialFiles.push(current);

        //if (e.operation == "upload") {
        //    alert("upload operation");
        //    currentInitialFiles.push(current);
        //} else {
        //    var indexOfFile = currentInitialFiles.indexOf(current);
        //    currentInitialFiles.splice(indexOfFile, 1);

        //    alert("remove operation");
        //}
    }
    sessionStorage.initialFiles = JSON.stringify(currentInitialFiles);

    //alert("Select :: " + getFileInfo(e));
}


function onUploadFile(e) {
    alert(JSON.stringify(e.data));
    e.data = { cashOrderId: $("#hdCashOrderId").val() };
}


function getFileInfo(e) {
    return $.map(e.files, function (file) {
        var info = file.name;

        // File size is not available in all browsers
        if (file.size > 0) {
            info += " (" + Math.ceil(file.size / 1024) + " KB)";
        }
        return info;
    }).join(", ");
}


function validateCashOrder(cashOrder) {
    var message = "<ul style='list-style-image:url(../Content/images/bullet.jpg) !important;'>";
    var validationPassed = true;

    if (cashOrder.MerchantId < 1) {
        message += "<li>Merchant</li>";
        validationPassed = false;
    }

    if (cashOrder.SiteId < 1) {
        message += "<li>Site</li>";
        validationPassed = false;
    }

    if (cashOrder.CitCode == "") {
        message += "<li>Cit Code</li>";
        validationPassed = false;
    }

    if ($.trim(cashOrder.DeliveryDateDto) == "") {
    	message += "<li>Delivery Date</li>";
    	validationPassed = false;
    }

    if ($.trim(cashOrder.CashOrderAmount) == "") {
    	message += "<li>Cash Order Amount</li>";
    	validationPassed = false;
    }
	
    if (isNaN($.trim(cashOrder.CashOrderAmount))) {
    	message += "<li>Cash Order Amount must be a number</li>";
    	validationPassed = false;
    }
    else {
        if ($.trim(cashOrder.CashOrderAmount) == "") {
            cashOrder.CashOrderAmount = 0;
        }

    	if (parseFloat(cashOrder.CashOrderAmount) != parseFloat(cashOrder.CashOrderContainer.CashOrderContainerDrops[1].Amount)) {
    		message += "<li>Cash Order Amount is not matching Amount captured.</li>";
    		validationPassed = false;
    	}
    }
	
	//


    var orderType = $('#ddlOrderType option:selected').text();

    if (orderType != "EFT") {

        if ($.trim(cashOrder.ContainerNumberWithCashForExchange).length < 14) {
            message += "<li>Container Number with Cash for Exchange must be 14 characters</li>";
            validationPassed = false;
        }

        if ($.trim(cashOrder.ContainerNumberWithCashForExchange).length == 14) {

            if ($.trim(cashOrder.CitCode) != "") {
                if (isInitialCitStartCode(cashOrder.ContainerNumberWithCashForExchange) == false) {
                    message += "<li>Cash for Exchange Bag Number initial digit must be " + $("#hdInitialCitSerialNumber").val() + "</li>";
                    validationPassed = false;
                }
            }


            if (isSealNumberANumber(cashOrder.ContainerNumberWithCashForExchange) == false) {
                message += "<li>Cash for Exchange Bag Number must be a number.</li>\n";
                validationPassed = false;
            }

        }


        if ($.trim(cashOrder.EmptyContainerOrBagNumber).length < 14) {
            message += "<li>Empty Container / Bag Number must be 14 characters</li>";
            validationPassed = false;
        }

        if ($.trim(cashOrder.EmptyContainerOrBagNumber).length == 14) {

            if ($.trim(cashOrder.CitCode) != "") {
                if (isInitialCitStartCode(cashOrder.EmptyContainerOrBagNumber) == false) {
                    message += "<li>Empty Container Number initial digit must be " + $("#hdInitialCitSerialNumber").val() + "</li>";
                    validationPassed = false;
                }
            }

            if (isSealNumberANumber(cashOrder.EmptyContainerOrBagNumber) == false) {
                message += "<li>Empty Container Number must be a number.</li>\n";
                validationPassed = false;
            }

        }

        // check containers duplication
        if (cashOrder.EmptyContainerOrBagNumber == cashOrder.ContainerNumberWithCashForExchange) {
            message += "<li>Dispatch Bag & Empty Bag Numbers cannot be the same</li>\n";
            validationPassed = false;
        }
    }
    else {
        cashOrder.EmptyContainerOrBagNumber = '';
        cashOrder.ContainerNumberWithCashForExchange = '';
    }
    


    if (cashOrder.CashOrderTypeId < 1) {
        message += "<li>Order Type</li>";
        validationPassed = false;
    }

    for (var i = 0; i < cashOrder.CashOrderContainer.CashOrderContainerDrops.length; i++) {

    	if (cashOrder.CashOrderContainer.CashOrderContainerDrops[i].Amount <= 0) {
    		if (cashOrder.CashOrderContainer.CashOrderContainerDrops[i].IsCashForwardedForExchange == true) {
                // if Order type is EFT then
                message += "<li>Cash Forwarded for Exchange cannot be R0.00</li>";
            }
            else {
                message += "<li>Cash Required in Exchange cannot be R0.00</li>";
            }
            validationPassed = false;
        }
    }

    var orderTypeName = $('#ddlOrderType option:selected').text();

    if (orderTypeName != "EFT") {
    	if (cashOrder.CashOrderContainer.CashOrderContainerDrops[0].Amount != cashOrder.CashOrderContainer.CashOrderContainerDrops[1].Amount) {
            message += "<li>Cash-Required-in-Exchange must equal Cash-Forwarded-for-Exchange</li>";
            validationPassed = false;
        }
    }


    if (getRepeatedDenominations() == true) {
    	message += "<li> The Forwarded-Denomination can not be the same as Required-Denomination </li>";
    	validationPassed = false;
    }

    message += "</ul>";    

    if (validationPassed == false) {
        validationMessageBox(message);
    }
    

    return validationPassed;
}


function getRepeatedDenominations() {
    var container = $("#CashOrderContainer").find(".CombinedContainerDrops");
    var cashForwardedDropItems = container.find(".CashForwarded select");
    var cashRequiredDropItems = container.find(".CashRequired select");

    var hasDuplicates = false;

    // Forwarded
    cashForwardedDropItems.each(function () {
    	var cashForwardedDenomination = $(this).val();

        // Required
        cashRequiredDropItems.each(function () {
        	var cashRequiredDenomination = $(this).val();

            if (cashForwardedDenomination == cashRequiredDenomination && cashForwardedDenomination != 1000500) {
                hasDuplicates = true;
            }

        });

    });
    return hasDuplicates;
}


function isInitialCitStartCode(s) {

    var citInitialDigit = $("#hdInitialCitSerialNumber").val();

    if (isEmptyString(s)) {
        if (isInitialCitStartCode.arguments.length == 1) return 0;
        else return (isInitialCitStartCode.arguments[1] == true);
    }
    for (var i = 0; i < s.length; i++) {
        var c = s[i];
        if (!isBetweenZeroAndNine(c)) return false;
    }
    if (s.charAt(0) != citInitialDigit) { return false; }
    return true;
}


function isSealNumberANumber(s) {
    if (isEmptyString(s)) {
        if (isSealNumberANumber.arguments.length == 1) return 0;
        else return (isSealNumberANumber.arguments[1] == true);
    }
    for (var i = 0; i < s.length; i++) {
        var c = s[i];
        if (!isBetweenZeroAndNine(c)) return false;
    }
    return true;
}


function isBetweenZeroAndNine(value) {
    return value >= "0" && value <= "9";
}


function isEmptyString(value) {
    return value == null || value.length == 0;
}


function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}


function cancelTransaction(listUrl) {
    var windowId = "ConfirmWindow";
    var header = "Confirm";
    var errorWarning = "";
    var message = "Transaction is not yet saved&nbsp;Do you want to continue?";
    CashOrderMessage(windowId, header, errorWarning, message, listUrl);
}


function CashOrderMessage(windowId, header, errorWarning, message, listUrl) {
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
                window.location.href = listUrl;
            }

            kendoWindow.data("kendoWindow").close();
        })
        .end();
}


function OnErrorMessage(windowId, header, errorWarning, message, listUrl) {
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
        		//window.location.href = listUrl;
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


function validationMessageBox(message) {
    var kendoWindow = $("<div />").kendoWindow({
        title: "Cash Order Validation",
        resizable: false,
        modal: true,
        width: 450,
        height: 360
    });

    kendoWindow.data("kendoWindow")
        .content($("#ValidationWindow").html())
        .center().open();

    $(".confirmation-message").html(message);
    $(".headerTitle").html("Please fill in required fields:");

    kendoWindow
        .find(".no-cancel,.confirm-cancel")
        .click(function () {
            if ($(this).hasClass("confirm-cancel")) {
                $("#btnConfirmCancel").click();
            }

            kendoWindow.data("kendoWindow").close();
        })
        .end();
}

function fileUploadMessageBox(message) {
    var kendoWindow = $("<div />").kendoWindow({
        title: "EFT Validation",
        resizable: false,
        modal: true,
        width: 400,
        height: 120
    });

    kendoWindow.data("kendoWindow")
        .content($("#UploadsWindow").html())
        .center().open();

    $(".confirmation-message").html(message);

    kendoWindow
        .find(".no-cancel,.confirm-cancel")
        .click(function () {
            if ($(this).hasClass("confirm-cancel")) {
                $("#btnConfirmCancel").click();
            }

            kendoWindow.data("kendoWindow").close();
        })
        .end();
}

var uploads = [];

function hasValidEftAttachments() {
    var fileWrapper = $("#EftUploads").find(".k-upload-files");

    if (fileWrapper.length == 0) {
        fileUploadMessageBox("No Attachment has been added, Please attach Proof of Payment");
        uploads.push(false);
        return false;
    }

    var files = fileWrapper.find(".k-file");

    files.each(function() {
        var self = $(this);
        var success = self.hasClass("k-file-success");
        var error = self.hasClass("k-file-error");
        var progress = self.hasClass("k-file-progress");

        if (progress == true) {
            fileUploadMessageBox("Uploads in progress, Please wait for all selected Attachments to finish uploading.");
            uploads.push(false);
            return false;
        }

        if (success == false && error == false) {
            fileUploadMessageBox("Not all Attachments are uploaded, Please click the 'Upload files' button to attach Proof of Payment");
            uploads.push(false);
            return false;
        }

        if (error == true) {
            fileUploadMessageBox("There was an error uploading an Attachment, Please refresh or remove the document");
            uploads.push(false);
            return false;
        }
    });
    return false;
}
