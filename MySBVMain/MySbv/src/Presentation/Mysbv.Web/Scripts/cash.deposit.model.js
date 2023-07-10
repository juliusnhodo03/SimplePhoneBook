

function cancelTransaction(indexUrl) {
    var windowId = "ConfirmWindow";
    var header = "Confirm";
    var errorWarning = "";
    var message = "Transaction is not yet saved&nbsp;Do you want to continue?";
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

// Creating empty array of drop-Serial-numbers
var arrayOfDropSerialNumbers = [];


function onSubmitSingleDeposit(actionUrl, processType, returnMessage, transactionStatusName, underlyingActionName, indexUrl, hdStatusNames, submitActionUrl) {
    var windowId = "ConfirmWindow";
    var header = "Submit Deposit";
    var message = "Are you sure you want to submit Cash Deposit?";
    submitSingleDepositYesOrNoBox(windowId, header, message, processType, returnMessage, transactionStatusName, underlyingActionName, actionUrl, indexUrl, hdStatusNames, submitActionUrl);
}



function submitSingleDepositYesOrNoBox(windowId, header, message, processType, returnMessage, transactionStatusName, underlyingActionName, actionUrl, indexUrl, hdStatusNames, submitActionUrl) {
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

    kendoWindow
        .find(".no-cancel,.confirm-cancel")
        .click(function () {
            if ($(this).hasClass("confirm-cancel")) {
                $("#btnExitToIndex").click();
                loadingWindow("Please wait...<br/>Submitting Cash deposit.");
                returnMessage = "Cash Deposit Submitted successfully.";
                saveCashDeposit(actionUrl, processType, returnMessage, transactionStatusName, underlyingActionName, indexUrl, hdStatusNames, submitActionUrl);
            }

            kendoWindow.data("kendoWindow").close();
        })
        .end();
}



function saveCashDeposit(actionUrl, processType, returnMessage, transactionStatusName, underlyingActionName, indexUrl, hdStatusNames, submitActionUrl) {

	arrayOfDropSerialNumbers = [];

	var dropOrDepositCaption = ($('#ddlDepositType option:selected').text() == "Multi Drop") ? "Drop" : "Deposit";

    var narrative = "";

    if ($("#ddlDepositeReference").length > 0) {
    	narrative = $("#ddlDepositeReference").val() > 0 ? $("#ddlDepositeReference option:selected").text() : $.trim($("#txtCustomDepositeRef").val());
    }
    else {
    	narrative = $("#txtCustomDepositeRef").val();
    }


    var siteId = ($("#ddlSiteSbvUser").length == 0) ? $("#hdSiteId").val() : $("#ddlSiteSbvUser").val();
    var totalDepositedAmount = $.trim($('#txtTotalDepositAmount').val());

    var cashDepositObject =
    {
    	CashDepositId: $.trim($('#hdCashDepositId').val()),
    	DepositTypeId: $.trim($('#ddlDepositType').val()),
        DepositTypeName: $('#ddlDepositType option:selected').text(),
        SiteId: siteId,
        AccountId: $.trim($('#ddlSiteSettlementAccount').val()),
        Narrative: narrative,
        DepositedAmount: parseFloat(totalDepositedAmount),
        TransactionStatusName: transactionStatusName,
        TransactionReference: $("#hdTransactionReferenceNumber").val(),
        ReturnMessage: returnMessage,
        IsSubmitted: transactionStatusName == "SUBMITTED" ,
        ActionName: $("#hdProcessName").val(),
        CreatedById : $("#hdCreatedById").val(),
        CreateDate: $("#hdCreateDate").val(),
        Containers: []
    } ;

    validateCashDeposit();

    var allContainers = $("#ContainersHolder  .bagOrCanister");

    var countCountainers = 0;
    var totalInAllContainers = 0;

    allContainers.each(function () {
        // Enumerate each Container
        countCountainers++;

        var canisterOrbag = $(this);

        // Container Attributes
        var containerTypeId = $(this).find("#ddlContainerType");
        var attribute1 = $(this).find("#Attribute1");
        var attribute2 = $(this).find("#Attribute2");

        if (containerTypeId.val() <= 0 || containerTypeId.val() == "" || containerTypeId.val() == null) {
            message += "<li>Container (" + countCountainers + ")  Container Type</li>\n";
            hasValidationFailed = true;
        }

        if (attribute1.length != 0 && $.trim(attribute1.val()).length < 14) {
            message += "<li>Countainer: " + countCountainers + " " + attribute1.attr('attributeName') + " must be " + attribute1.attr('maxlength') + " Characters.</li>\n";
            hasValidationFailed = true;
        }

        if (attribute2.length != 0 && $.trim(attribute2.val()).length < 14) {
            message += "<li>Countainer: " + countCountainers + " " + attribute2.attr('attributeName') + " must be " + attribute2.attr('maxlength') + " Characters.</li>\n";
            hasValidationFailed = true;
        }

        
    	// containerId
        var hdContainerId = canisterOrbag.find("input[name='hdContainerId']").val();
        var hdContainerReferenceNumber = canisterOrbag.find("input[name='hdContainerReferenceNumber']").val();
        var totalInContainer = $.trim(canisterOrbag.find(".container_header_total").html());

        var containerObject =
        {
        	ContainerId: (parseInt(hdContainerId) > 0) ? hdContainerId : 0,
        	CashDepositId: cashDepositObject.CashDepositId,
        	ContainerTypeId: (containerTypeId.val() <= 0) ? null : containerTypeId.val(),
        	SerialNumber: attribute1.length == 0 ? "" : attribute1.val(),
        	SealNumber: attribute2.length == 0 ? "" : attribute2.val(),
        	IsPrimaryContainer: countCountainers == 1,
        	Amount: 0.00,
        	ReferenceNumber: hdContainerReferenceNumber,
            ContainerDrops: []
        };

        containerObject.ContainerId = (processType == 'Submitted' && underlyingActionName == "copied") ? 0 : hdContainerId;

        if (containerObject.SerialNumber.length > 0) {
        	if (isNaN(containerObject.SerialNumber)) {
        		message += "<li>Container: " + countCountainers + " 'Serial Number' must be a number.</li>\n";
        		hasValidationFailed = true;
        	}
        	else {
        		if (isInitialCitStartCode(containerObject.SerialNumber) == false) {
        			message += "<li>Container: " + countCountainers + " 'Serial Number' must be a number starting with " + $.trim($("#hdInitialCitSerialNumber").val()) + "</li>\n";
        			hasValidationFailed = true;
        		}
	        }
        }

        if (containerObject.SealNumber.length > 0 && isSealNumberANumber(containerObject.SealNumber) == false) {
            message += "<li>Container: " + countCountainers + " SealNumber  must be a number.</li>\n";
            hasValidationFailed = true;
        }

        totalInAllContainers += parseFloat(totalInContainer);

        // get all ContainerDrops in Container
        var dropsInContainer = $(this).find(".denominationgroup");
	    var dropNumber = 0;
        dropsInContainer.each(function () {
	        dropNumber++;
            var lblDropTitleAndNumber = $(this).parents(".drop_level").find(".lblDropTitleAndNumber");

            // DropReference
            var dropReferenceInput = $(this).parents(".denomination_groups_clone").find("input[name='txtDropReference']");
            var dropReference = (dropReferenceInput.length == 0) ? "" : dropReferenceInput.val();

            // DropBagSerialNumber
            var dropBagSerialNumberInput = $(this).parents(".denomination_groups_clone").find("input[name='txtDropSerialNumber']");
            var dropBagSerialNumber = (dropBagSerialNumberInput.length == 0) ? "" : dropBagSerialNumberInput.val();

            // dropAmount
            var dropAmountInput = $(this).parents(".denomination_groups_clone").find("input[name='txtDropAmount']");
            var dropAmount = (dropAmountInput.length == 0) ? parseFloat(0).toFixed(2) : parseFloat(dropAmountInput.val()).toFixed(2);

            // containerId
            var hdContainerDropId = $(this).parents(".denomination_groups_clone").find("input[name='hdContainerDropId']").val();
			
        	// Drop Status Id
            var hdStatusId = $(this).parents(".denomination_groups_clone").find("input[name='hdStatusId']").val();
			
        	// Drop Status Name
            var hdDropStatusName = $(this).parents(".denomination_groups_clone").find("input[name='hdDropStatusName']").val();
        	// Drop ReferenceNumber
            var hdDropReferenceNumber = $(this).parents(".denomination_groups_clone").find("input[name='hdDropReferenceNumber']").val();

            hdDropStatusName = (cashDepositObject.DepositTypeName == "Single Deposit" && cashDepositObject.TransactionStatusName == "SUBMITTED") ? "SUBMITTED" : hdDropStatusName;

            var containerDropObject =
            {
            	ContainerDropId: (parseInt(hdContainerDropId) > 0) ? hdContainerDropId : 0,
            	ContainerId: containerObject.ContainerId,
            	StatusId: hdStatusId,
            	Name: hdDropStatusName,
                Number: dropNumber,
                Amount: dropAmount,
                Narrative: dropReference,
                BagSerialNumber: $.trim(dropBagSerialNumber),
                ReferenceNumber: hdDropReferenceNumber,
                ContainerDropItems: []
            };

            containerDropObject.ContainerDropId = (processType == 'Submitted' && underlyingActionName == "copied") ? 0 : hdContainerDropId;
            containerObject.Amount += parseFloat(containerDropObject.Amount);

            //Drop Serial Number
            var depositType = $('#ddlDepositType option:selected').text();

            if ($.trim(containerDropObject.Narrative) == "" && depositType != "Single Deposit") {
                message += "<li>Container: " + countCountainers + " Enter " + dropOrDepositCaption + " Reference</li>\n";
                hasValidationFailed = true;
            }

            if (depositType == "Multi Drop") {
            	if (containerDropObject.BagSerialNumber != "") {
            		arrayOfDropSerialNumbers.push(containerDropObject.BagSerialNumber);
	            }

            	if (containerDropObject.BagSerialNumber.length < 3 || containerDropObject.BagSerialNumber.length > 14) {
                    message += "<li>Container: " + countCountainers + " Enter correct " + dropOrDepositCaption + " Serial Number</li>\n";
                    hasValidationFailed = true;
                }
            }

            var denominationsInDrop = $(this).find("select");
            var containerDropsCount = 0;
            var dropTotalAmount = 0;

            // denominations in each drop
            denominationsInDrop.each(function () {
                var row = $(this).parents(".clsline, .clsCoinsline");
                var noteValue = row.find("input[name='textNoteValue']");
                var coinValue = row.find("input[name='textCoinValue']");
                var noteCount = row.find("input[name='textNoteCount']");
                var coinCount = row.find("input[name='textCoinCount']");
                var dropItmId = row.find("input[name='hdContainerDropItemId']").val();

                var value = noteValue.length == 0 ? coinValue : noteValue;
                var count = noteCount.length == 0 ? coinCount : noteCount;

                var containerDropItemObject =
                {
                	ContainerDropItemId: dropItmId,
                	ContainerDropId: containerDropObject.ContainerDropId,
                	ValueInCents: $(this).val() == 1000500 ? 0 : $(this).val(),
                    Count: isNumber(count.val()) == false ? 0 : count.val(),
                    Value: isNumber(value.val()) == false ? 0 : value.val()
                };

                dropTotalAmount += parseFloat(containerDropItemObject.Value);

                //==> push object to arraylist
                if (parseInt($(this).val()) != 1000500 && parseInt(containerDropItemObject.Count) > 0) {
                    containerDropObject.ContainerDropItems.push(containerDropItemObject);
                }
                //Check if 1 Drop has denominations
                containerDropsCount += parseFloat(containerDropItemObject.Count);
            });


            if (lblDropTitleAndNumber.html() != 'undefined') {
                if (containerDropsCount == 0 && denominationsInDrop.length != 0) {
                    message += "<li>No denomination Counts in Countainer (" + countCountainers + "), " + lblDropTitleAndNumber.html() + " </li>\n";
                    hasValidationFailed = true;
                }


                if (containerDropsCount > 0 && denominationsInDrop.length > 0) {
                	var verifiedAmount = parseFloat(dropTotalAmount).toFixed(2);
					
                	if (verifiedAmount != dropAmount) {
                        message += "<li><b>" + dropOrDepositCaption + " Amount</b>: Container (" + countCountainers + ") " + lblDropTitleAndNumber.html() + " not matching " + dropOrDepositCaption + "  Amount</li>\n";
                        hasValidationFailed = true;
                    }
                }
            }


            //==> push object to arraylist
            containerObject.ContainerDrops.push(containerDropObject);
        });

        //Add containers
        cashDepositObject.Containers.push(containerObject);
    });  //end containers loop


    //Check Drop Serial Number Duplication
    var depositTypeName = $('#ddlDepositType option:selected').text();

    if (depositTypeName != "Single Deposit") {
        if (isDropSealNumberDuplicated(arrayOfDropSerialNumbers) > 0) {
            message += "<li>Please remove duplicate Container " + dropOrDepositCaption + " Serial Numbers</li>\n";
            hasValidationFailed = true;
        }
    }

    arrayOfDropSerialNumbers = [];

    // check containers duplication
    var countRepeatedSerialNumbers = checkForDuplicateSerialNumbers(cashDepositObject.Containers);
    if (countRepeatedSerialNumbers > 0) {
        message += "<li>Please remove duplicate Serial Numbers</li>\n";
        hasValidationFailed = true;
    }

    var countRepeatedSealNumbers = checkForDuplicateSealNumbers(cashDepositObject.Containers);
    if (countRepeatedSealNumbers > 0) {
        message += "<li>Please remove duplicate Seal Numbers</li>\n";
        hasValidationFailed = true;
    }

    countRepeatedSealNumbers = 0;
    countRepeatedSerialNumbers = 0;

    // Check if NOTES are exceeding "R1,000,000"
    var notesSubTotal = 0;
    var allNotesSubtotals = $("input[name='textNotesSubtotal']");
    allNotesSubtotals.each(function () {
        notesSubTotal += parseFloat($(this).val());
    });

    if (parseFloat(notesSubTotal) > 1000000) {
        message += "<li>Total Notes Value must not exceed R1,000,000</li>\n";
        hasValidationFailed = true;
    }

    // Check if COINS are exceeding "R1,000,000"
    var coinsSubTotal = 0;
    var allCoinsSubtotals = $("input[name='textCoinsSubtotal']");
    allCoinsSubtotals.each(function () {
        coinsSubTotal += parseFloat($(this).val());
    });

    if (parseFloat(coinsSubTotal) > 100000) {
        message += "<li>Total Coins Value must not exceed R100,000</li>\n";
        hasValidationFailed = true;
    }

    if (parseFloat(totalInAllContainers) != parseFloat(cashDepositObject.DepositedAmount)) {
        message += "<li>Please re-check total amount</li>\n";
        hasValidationFailed = true;
    }

    if (hasValidationFailed == true) {
        message += "</ul>";
        hasValidationFailed = false;

        var windowId = "ValidationWindow";
        var header = "Field Validation";
        var errorWarning = "Please fill in required fields:";

        validationMessageBox(windowId, header, errorWarning, message, 470, 300);

        if (processType == "Submitted") {
            $("#btnConfirmCancel").click();
        }

        message = "<ul style='list-style-image:url(" + resolveUrl("../Content/images/bullet.jpg", processType) + ") !important;'>";
        return true;
    }


    if (processType == 'Submitted') {
        if (hasValidationFailed == false) {
            // After validation passes
            // Mark drops/ deposits as "Submitted"
            hdStatusNames.each(function () {
            	$(this).val("SUBMITTED");
            });
        }
    }

    var loadingMessage = cashDepositObject.TransactionStatusName == "SUBMITTED" ? "Submitting" : "Saving";
    loadingWindow("Please wait...<br>" + loadingMessage + " Cash Deposit");

    cashDepositObject.ActionState = processType;


    var baseUrl = indexUrl.replace("Index", "");

    var obj = { cashDepositDto: cashDepositObject };
    //call server method
    $.ajax(
    {
        type: "POST",
        url: actionUrl,
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            // Call successfull
            if (result.ResponseCode == null) {
                // Close Loading Indicator
                $("#btnConfirmCancel").click();

                showMessageBox("MessageWindow", "Error Message", "", result.Message, false, processType);
            }
            else {
            	if (result.CashDeposit.IsSubmitted == true) {
            			window.location.href = submitActionUrl + result.CashDeposit.CashDepositId;		            
                }
                else {
                	$('#hdCashDepositId').val(result.CashDeposit.CashDepositId);

                	// Close Loading Indicator
                	$("#btnConfirmCancel").click();
                	afterSavingMessage("MessageWindow", "Cash Deposit", "", result.Message, indexUrl);

                    //$("#btnConfirmCancel").click();
                    //showMessageBox("MessageWindow", "Cash Deposit", "", result.Message, false, processType);
					
                    var bagOrCanister = $(".bagOrCanister");
                    allBagsTraversal(result.CashDeposit, bagOrCanister);
            	}
            }
        },
        error: function (result) {
            showMessageBox("MessageWindow", "Error Message", "", result.Message, false, processType);

        }
    }); // End $.ajax method
    return false;
}



function afterSavingMessage(windowId, header, errorWarning, message, listUrl) {
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



function allBagsTraversal(cashDeposit, bagOrCanister) {
    // form containers
    bagOrCanister.each(function () {
        var bagSerialNumber = $(this).find("#Attribute1").val();

        // server containers
        $.each(cashDeposit.Containers, function (a, container) {

            if ($.trim(bagSerialNumber) == $.trim(container.Attribute1)) {

                bagOrCanister.find("input[name ='hdContainerId']").val(container.ContainerId);

                // form drops
                var containerDrops = bagOrCanister.find(".drop_level");

                containerDrops.each(function () {
                    var dropNumber = $(this).find("input[name='hdDropNumber']").val();
                    var formDrop = $(this);

                    // server drops
                    $.each(container.ContainerDrops, function (b, drop) {

                        if (dropNumber == drop.DropNumber) {
                            formDrop.find("input[name='hdContainerDropId']").val(drop.ContainerDropId);

                        }
                    });

                });
                // end form drops
            }

        });
        //End container
    });
}



function checkForDuplicateSerialNumbers(containers) {
    var count = 0;
    for (var i = 0; i < containers.length; i++) {
        var bag = containers[i];
        for (var j = i + 1; j < containers.length; j++) {
        	if (bag.SerialNumber == containers[j].SerialNumber && bag.SerialNumber != "" && containers[j].SerialNumber != "") {
                count++;
            }
        }
    }
    return count;
}


function checkForDuplicateSealNumbers(containers) {
    var count = 0;
    for (var i = 0; i < containers.length; i++) {
        var bag = containers[i];
        for (var j = i + 1; j < containers.length; j++) {
        	if (bag.SealNumber == containers[j].SealNumber && bag.SealNumber != "" && containers[j].SealNumber != "") {
                count++;
            }
        }
    }
    return count;
}


function isInitialCitStartCode(s) {

    var citInitialDigit = $.trim($("#hdInitialCitSerialNumber").val());

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


function isDropSealNumberDuplicated(arrayOfDropSerialNumbers) {
    var count = 0;
    for (var i = 0; i < arrayOfDropSerialNumbers.length; i++)
    {
    	var element = $.trim(arrayOfDropSerialNumbers[i]);

    	for (var k = i + 1; k < arrayOfDropSerialNumbers.length; k++)
    	{
    		if (element == $.trim(arrayOfDropSerialNumbers[k])) {
    			count++;
		    }
	    }
    }
    return count;
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


var hasValidationFailed = false;

var message = "<ul style='list-style-image:url(" + resolveUrl("../Content/images/bullet.jpg", "captured") + ") !important;'>";

function validateCashDeposit() {
    var depositTypeId = $.trim($('#ddlDepositType').val());

    var depositReference = $("#ddlDepositeReference").val();

    var referenceNumber = (depositReference == 0 || depositReference == "" || depositReference == null) ?
                            $.trim($("#txtCustomDepositeRef").val()) :
                            $.trim($('#ddlDepositeReference').val());

    if (referenceNumber == "") {
        referenceNumber = $("#txtDepositReference").val();
    }

    var citCode = $.trim($('#txtCitCode').val());

    var siteId = ($("#ddlSiteSbvUser").length == 0) ? $("#hdSiteId").val() : $("#ddlSiteSbvUser").val();

    var siteSettlementAccountId = $.trim($('#ddlSiteSettlementAccount').val());

    if (depositTypeId <= 0) {
        message += "<li>Deposit Type</li>\n";
        hasValidationFailed = true;
    }
    if (siteId == '' || siteId <= 0) {
        message += "<li>Site</li>\n";
        hasValidationFailed = true;
    }

    if ($("#ddlDepositType").find("option:selected").text() != 'Multi Deposit') {
        if (referenceNumber == "" || referenceNumber == null) {
            message += "<li>Deposit Reference</li>\n";
            hasValidationFailed = true;
        }
    }

    if (/^[a-zA-Z0-9- ]*$/.test(referenceNumber) == false) {
        message += "<li>The deposit reference contains illegal characters</li>\n";
        hasValidationFailed = true;
    }


    if (citCode == '') {
        message += "<li>CIT Code</li>\n";
        hasValidationFailed = true;
    }
    if (siteSettlementAccountId == '' || siteSettlementAccountId <= 0) {
        message += "<li>Site Settlement Account</li>\n";
        hasValidationFailed = true;
    }
    return message;
}


function setCashDepositProcessingElementsValues(textCount, textValue, textDeno, valueInCents, denominationCount, isDisabled) {
    textCount.attr('value', denominationCount);
    textDeno.attr('value', valueInCents);
    textValue.attr('value', (denominationCount * valueInCents / 100).toFixed(2));

    // disable controls
    textDeno.attr('disabled', isDisabled);
    textCount.attr('disabled', isDisabled);
    textValue.attr('disabled', isDisabled);
}


function enumerateContainers() {
    var container = $("#ContainersHolder .bagOrCanister");
    var containerCount = 1;
    var actualDepositAmount = 0;

    container.each(function () {
        var containerHeaderText = $(this).find(".container_header_text");
        containerHeaderText.html("Container " + containerCount );
        containerCount++;

        // container total
        var containerTotal = $(this).find(".SubTotals");
        var total = 0;
        containerTotal.each(function () {
            var value = isNaN($(this).val()) ? 0 : parseFloat($(this).val());
            total += value;
        });
        var totalInContainer = $(this).find(".container_header_total");
        totalInContainer.html(total.toFixed(2));
        actualDepositAmount += total;
    });

    var actual = $("#txtTotalDepositAmount");
    if (actual.length != 0) {
        actual.val(actualDepositAmount.toFixed(2));
    }
}


function resolveUrl(actualUrl, processName) {
    var url = "";
    if (processName == "exitcopy" || processName == "exitedit") {
        url = "../../" + actualUrl + "CashDeposit";
    }
    else if (processName == "exitcapture") {
        url = ".." + actualUrl + "/CashDeposit";
    }
    else if (processName == "isSubmitted") {
        url = "" + actualUrl + "/CashDeposit";
    }
    else if (processName == "isNormalExit") {
        url = "" + actualUrl + "/CashDeposit";
    }
    else if (processName == "copied" || processName == "edited" || processName == "submitted") {
        url = "../" + actualUrl;
    }
    else {
        url = actualUrl;
    }
    return url;
}


//document ready
$(document).ready(function () {


    /** On TextChanged Container Type **/
    $("#ddlContainerType").on('change', function () {
        var parameters = { containerTypeId: $(this).val() };
        var parent = $(this).parents(".sFieldsetContainer");
        getContainerTypeAttributes(parent, parameters);
    }); /** end Container Type changeEvent **/


    $("#ddlDepositeReference").change(function () {
        // Clear Narrative
        $("#txtCustomDepositeRef").val('');

        if ($("#ddlDepositeReference").val() == 0) {
            $("#txtCustomDepositeRef").attr("disabled", false);
        } else {
            $("#txtCustomDepositeRef").attr("disabled", true);
        }
    });

});
