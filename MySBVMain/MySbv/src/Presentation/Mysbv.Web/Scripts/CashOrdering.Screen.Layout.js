
var cash_order_row_clone_notes;
var cash_order_row_clone_coins;
var cash_order_denomination_groups_clone;
var cash_order_eft_clone;


//document ready
//


$(document).ready(function () {
    
    cash_order_row_clone_notes = $(".outer .clsline").clone(true);
    cash_order_row_clone_coins = $(".outer .clsCoinsline").clone(true);
    cash_order_denomination_groups_clone = $(".EFTDeleteCloneAtLoadinging").clone(true);
    cash_order_eft_clone = $(".EFTDeleteCloneAtLoadinging").clone(true);
    
    
    $("#ddlOrderType").change(function () {
        var siteId = $("#ddlSites").val();

    	var orderType = $('#ddlOrderType option:selected').text();
    	$(".EFTDeleteCloneAtLoadinging").remove();
    	$(".columnInsertEft").children().remove();

    	if (orderType == "EFT") {
    	    $('#ContainerNumberWithCashForExchange').val("");
    	    $('#EmptyContainerOrBagNumber').val("");

            $(".header-forwarded").parents(".ui-widget-header").hide();
            $(".CashForwarded").css("background-color", "#f5f5f5");

            $("#ContainerNumberWithCashForExchange").attr("disabled", "disabled");
            $("#EmptyContainerOrBagNumber").attr("disabled", "disabled");

            if (siteId > 0) {
                $("#EftUploads").show();
            } else {                
                $("#EftUploads").hide();                
            }
        } else {
        	$(".CashForwarded, .ui-widget-header").css("visibility", "visible");
            cash_order_eft_clone.clone(true).appendTo(".columnInsertEft");
            $(".header-forwarded").parents(".ui-widget-header").show();
            $(".CashForwarded").css("background-color", "#fff");

            $("#ContainerNumberWithCashForExchange").attr("disabled", false);
            $("#EmptyContainerOrBagNumber").attr("disabled", false);
            $("#EftUploads").hide();
        }
    });



    // Delivery Date Calculation
    var date = new Date();
    var currentMonth = date.getMonth();
    var currentDate = date.getDate() + cashOrderNumberOfDays;

    var currentYear = date.getFullYear();

    var dates = $("#DeliveryDate").datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        numberOfMonths: numberOfMonthsToShowOnCalendar,
        minDate: dateToday,
        maxDate: new Date(currentYear, currentMonth, currentDate),
        onSelect: function (selectedDate) {
            var option = this.id == "from" ? "minDate" : "maxDate",
                instance = $(this).data("datepicker"),
                date = $.datepicker.parseDate(instance.settings.dateFormat || $.datepicker._defaults.dateFormat, selectedDate, instance.settings);
            dates.not(this).datepicker("option", option, date);
        }
    });





    $("#ddlSites").change(function () {
        var siteId = $("#ddlSites").val();
        var orderType = $('#ddlOrderType option:selected').text();

        if (siteId > 0 && orderType == "EFT") {
            $("#EftUploads").show();
        } else {
            $("#EftUploads").hide();
        }

        var itemSelected = $('#ddlSites option:selected');  
        var citCode = itemSelected.attr('name');
        var citdigit = itemSelected.attr('citdigit');
        
        citCode = (citCode == 'null') ? "" : citCode;
        citdigit = (citdigit == 'null') ? "" : citdigit;

        $("#CitCode").val(citCode);
        $("#hdInitialCitSerialNumber").val(citdigit);
    });

    


    //----------------------------------------------------------------------------------------------------------------------------------------
    // Notes
    //----------------------------------------------------------------------------------------------------------------------------------------
    
    $("#CashOrderContainer select[id='dropDownListNote']").on('change', function () {
        var parents = $(this).parents(".clsline");
        var note = $(this);
        var count = parents.find("input[name='textNoteCount']");
        var value = parents.find("input[name='textNoteValue']");

        calculate(note, count, value);
        var subtotalparents = $(this).parents(".clsNotessubtotal");

        var notesCollection = subtotalparents.find("input[name='textNoteValue']");
        var textSubtotal = subtotalparents.find("input[name='textNotesSubtotal']");

        getsubTotal(notesCollection, textSubtotal);

        // remove selected denomination from the other dropdowns in container 
        var denominationCollection = $(this).parents(".clsNotessubtotal");
        var listRows = denominationCollection.find("select[id='dropDownListNote']");
        removeSelectedDenomination(note, listRows);

        subtotalparents = $(this).parents(".CashForwardedContainerDrop, .CashRequiredContainerDrop");
        var textCoinsSubtotal = subtotalparents.find("input[name='textCoinsSubtotal']");
        var overallTotal = subtotalparents.find(".TotalInDrop");

        getTotalInDrop(overallTotal, textSubtotal, textCoinsSubtotal);
    });




    $("#CashOrderContainer input[name='textNoteValue']").on('keyup', function () {
        var parents = $(this).parents(".clsline");
        var note = parents.find("select[id='dropDownListNote']");
        var value = $(this);
        var count = parents.find("input[name='textNoteCount']");

        if (note.attr('value') == 1000500 || value.val() <= 0 || isNaN(value.val())) {
            value.attr('value', "");
            count.attr('value', "");
        }

        var subtotalparents = $(this).parents(".clsNotessubtotal");
        var notesCollection = subtotalparents.find("input[name='textNoteValue']");
        var textSubtotal = subtotalparents.find("input[name='textNotesSubtotal']");

        getsubTotal(notesCollection, textSubtotal);

        subtotalparents = $(this).parents(".CashForwardedContainerDrop, .CashRequiredContainerDrop");
        var textCoinsSubtotal = subtotalparents.find("input[name='textCoinsSubtotal']");
        var overallTotal = subtotalparents.find(".TotalInDrop");

        getTotalInDrop(overallTotal, textSubtotal, textCoinsSubtotal);
        count.attr("value", "");
    });




    function calculateCashOrderAmount() {
        var cashOrderSubtotals = $("#CashOrderContainer .CashRequired").find("input[name='textNotesSubtotal'], input[name='textCoinsSubtotal']");
        var cashOrderAmount = 0;

        cashOrderSubtotals.each(function () {
            cashOrderAmount += parseFloat($(this).val());
        });

        $("#CashOrderAmount").val(cashOrderAmount.toFixed(2));
    }


    $("#CashOrderContainer input[name='textNoteValue']").on('blur', function () {
        var parents = $(this).parents(".clsline");
        var note = parents.find("select[id='dropDownListNote']");
        var value = $(this);
        var count = parents.find("input[name='textNoteCount']");

        var denomination = (parseFloat(note.val())) / 100;
        var amount = parseFloat(value.val());
        var result = amount / denomination;

        if (note.val() == 1000500) {
        	value.val("");
        }

        if (value.val() <= 0 || isNaN(value.val())) {
        	value.val("");
        	count.val("");
        } else {
        	var amountOverDenomination = (amount * 10) / (denomination * 10);
        	count.val(amountOverDenomination);
        }

        var isNotANumberPassed = true;

        if (isNaN(result)) {
        	isNotANumberPassed = false;
        	if (note.val() == 1000500) {
        		isNotANumberPassed = true;
        	}
        }

        if (isNotANumberPassed == false || parseFloat((amount * 10) % (denomination * 10)) > 0) {
        	var selectedText = note.find("option:selected").text();
        	var deno = (parseInt(note.val())) / 100;
        	var randSymbol = deno < 1 ? "" : "R";

        	var text = $.trim(value.val());
        	if (text.length > 0) {
        		warningBox("Please enter multiples of <b>" + randSymbol + selectedText + "</b> as value !<br>Make sure you entered a number without spaces or special characters!");
        	}

        	value.val("");
        	count.val("");
        }

        var subtotalparents = $(this).parents(".clsNotessubtotal");
        var notesCollection = subtotalparents.find("input[name='textNoteValue']");
        var textNotesSubtotal = subtotalparents.find("input[name='textNotesSubtotal']");

        getsubTotal(notesCollection, textNotesSubtotal);

        subtotalparents = $(this).parents(".CashForwardedContainerDrop, .CashRequiredContainerDrop");
        var textCoinsSubtotal = subtotalparents.find("input[name='textCoinsSubtotal']");
        var overallTotal = subtotalparents.find(".TotalInDrop");

        getTotalInDrop(overallTotal, textCoinsSubtotal, textNotesSubtotal);
    });




    $("#CashOrderContainer input[name='textNoteCount']").on('keyup', function () {
        var parents = $(this).parents(".clsline");
        var note = parents.find("select[id='dropDownListNote']");
        var count = $(this);
        var value = parents.find("input[name='textNoteValue']");

        var countVal = 0;

        if (count.val() <= 0 || isNaN(count.val())) {
            count.attr("value", "");
            value.attr("value", "");
        } else {
            countVal = parseFloat(count.val()) * 10;
            if (countVal % 10 > 0) {
                warningBox("Please enter whole numbers as <b>Count </b>!");
                value.attr('value', "");
                count.attr("value", "");
                return false;
            }
        }
        // calculate note value       
        calculate(note, count, value);
        var subtotalparents = $(this).parents(".clsNotessubtotal");

        var notesCollection = subtotalparents.find("input[name='textNoteValue']");
        var textSubtotal = subtotalparents.find("input[name='textNotesSubtotal']");

        getsubTotal(notesCollection, textSubtotal);

        subtotalparents = $(this).parents(".CashForwardedContainerDrop, .CashRequiredContainerDrop");
        var textCoinsSubtotal = subtotalparents.find("input[name='textCoinsSubtotal']");
        var overallTotal = subtotalparents.find(".TotalInDrop");

        getTotalInDrop(overallTotal, textSubtotal, textCoinsSubtotal);
    });




    // attach AddNotes() event handler
    $("#CashOrderContainer a[name='buttonAddNotes']").on('click', function () {
        var parents = $(this).parents(".clsnotes");

            // only add 6 denomination groups
        if (parents.find(".clsline").length < 5) {
            // perform copy denomination group

            cash_order_row_clone_notes.clone(true).appendTo(parents);

            var denominationCollection = $(this).parents(".clsNotessubtotal");
            var listRows = denominationCollection.find("select[id='dropDownListNote']");
            var dropdown = parents.find(".last select:last");

            parents.find(".denominationparent .remove").removeClass('hide').addClass('show');

            removePreviouslySelectedDenominations(dropdown, listRows);
        }
    });




    // attach RemoveNotes() event handler
    $("#CashOrderContainer a[name='buttonRemoveNotes']").on('click', function() {

        var clsNotesParents = $(this).parents(".clsNotessubtotal");
        var parents = $(this).closest("tr").remove();
        var removeButton = parents.find(".remove:first");
        removeButton.hide();

        var notesCollection = clsNotesParents.find("input[name='textNoteValue']");
        var textNotesSubtotal = clsNotesParents.find("input[name='textNotesSubtotal']");
        getsubTotal(notesCollection, textNotesSubtotal);

        var subtotalparents = clsNotesParents.parents(".CashForwardedContainerDrop, .CashRequiredContainerDrop");
        var textCoinsSubtotal = subtotalparents.find("input[name='textCoinsSubtotal']");
        var overallTotal = subtotalparents.find(".TotalInDrop");

        getTotalInDrop(overallTotal, textNotesSubtotal, textCoinsSubtotal);

    });

    //----------------------------------------------------------------------------------------------------------------------------------------
    // Coins
    //----------------------------------------------------------------------------------------------------------------------------------------

    // add coin denominations
    $("#CashOrderContainer a[name='buttonAddCoins']").on('click', function () {

        var parents = $(this).parents(".clscoins");

        // only add 6 denomination groups
        if (parents.find(".clsCoinsline").length < 7) {
            // perform copy denomination group
            cash_order_row_clone_coins.clone(true).appendTo(parents);
            var denominationCollection = $(this).parents(".clsCoinssubtotal");
            var listRows = denominationCollection.find("select[id='dropDownListCoin']");
            var dropdown = parents.find(".last select:last");

            parents.find(".denominationparent .remove").removeClass('hide').addClass('show');

            removePreviouslySelectedDenominations(dropdown, listRows);
        }
    });


    // attach RemoveCoins() event handler
    $("#CashOrderContainer a[name='buttonRemoveCoins']").on('click', function() {

        var clsCoinssubtotal = $(this).parents(".clsCoinssubtotal");
        var parents = $(this).closest("tr").remove();
        var removeButton = parents.find(".remove:first");
        removeButton.hide();

        var coinsCollection = clsCoinssubtotal.find("input[name='textCoinValue']");
        var textCoinsSubtotal = clsCoinssubtotal.find("input[name='textCoinsSubtotal']");
        getsubTotal(coinsCollection, textCoinsSubtotal);

        var subtotalparents = clsCoinssubtotal.parents(".CashForwardedContainerDrop, .CashRequiredContainerDrop");
        var textNotesSubtotal = subtotalparents.find("input[name='textNotesSubtotal']");
        var overallTotal = subtotalparents.find(".TotalInDrop");

        getTotalInDrop(overallTotal, textNotesSubtotal, textCoinsSubtotal);

    });



    $("#CashOrderContainer input[name='textCoinCount']").on('keyup', function () {
        var parents = $(this).parents(".clsCoinsline");
        var coin = parents.find("select[id='dropDownListCoin']");
        var value = parents.find("input[name='textCoinValue']");
        var count = $(this);

        var countVal = 0;

        if (count.val() <= 0 || isNaN(count.val())) {
            count.attr("value", "");
            value.attr("value", "");
        } else {
            countVal = parseFloat(count.val()) * 10;
            if (countVal % 10 > 0) {
                warningBox("Please enter whole numbers as <b>Count </b>!");
                value.attr('value', "");
                count.attr("value", "");
                return false;
            }
        }

        // calculate coin value       
        calculate(coin, count, value);
        var subtotalparents = $(this).parents(".clsCoinssubtotal");

        var coinsCollection = subtotalparents.find("input[name='textCoinValue']");
        var textCoinsSubtotal = subtotalparents.find("input[name='textCoinsSubtotal']");

        getsubTotal(coinsCollection, textCoinsSubtotal);

        subtotalparents = $(this).parents(".CashForwardedContainerDrop, .CashRequiredContainerDrop");
        var textNotesSubtotal = subtotalparents.find("input[name='textNotesSubtotal']");
        var overallTotal = subtotalparents.find(".TotalInDrop");

        getTotalInDrop(overallTotal, textNotesSubtotal, textCoinsSubtotal);
    });





    $("#CashOrderContainer input[name='textCoinValue']").on('keyup', function () {
        var parents = $(this).parents(".clsCoinsline");
        var coin = parents.find("select[id='dropDownListCoin']");
        var value = $(this);
        var count = parents.find("input[name='textCoinCount']");

        if (coin.val() == 1000500) {
            value.val("");
        }

        if (value.val() < 0 || isNaN(value.val())) {
            value.val("");
            count.val("");
        }


        var subtotalparents = $(this).parents(".clsCoinssubtotal");
        var coinsCollection = subtotalparents.find("input[name='textCoinValue']");
        var textCoinsSubtotal = subtotalparents.find("input[name='textCoinsSubtotal']");

        getsubTotal(coinsCollection, textCoinsSubtotal);

        subtotalparents = $(this).parents(".CashForwardedContainerDrop, .CashRequiredContainerDrop");
        var textNotesSubtotal = subtotalparents.find("input[name='textNotesSubtotal']");
        var overallTotal = subtotalparents.find(".TotalInDrop");

        getTotalInDrop(overallTotal, textNotesSubtotal, textCoinsSubtotal);
        count.val("");
    });




    $("#CashOrderContainer input[name='textCoinValue']").on('blur', function () {
        var parents = $(this).parents(".clsCoinsline");
        var coin = parents.find("select[id='dropDownListCoin']");
        var value = $(this);
        var count = parents.find("input[name='textCoinCount']");


        var denomination = (parseFloat(coin.val())) / 100;
        var amount = parseFloat(value.val());
        var result = amount / denomination;

        if (coin.val() == 1000500) {
        	value.val("");
        }

        if (value.val() <= 0 || isNaN(value.val())) {
            value.val("");
            count.val("");
        } else {
            var amountOverDenomination = (amount * 10) / (denomination * 10);
            count.val(amountOverDenomination);
        }

        var isNotANumberPassed = true;

        if (isNaN(result)) {
            isNotANumberPassed = false;
            if (coin.val() == 1000500) {
                isNotANumberPassed = true;
            }
        }

        if (isNotANumberPassed == false || parseFloat((amount * 10) % (denomination * 10)) > 0) {
            var selectedText = coin.find("option:selected").text();
            var deno = (parseInt(coin.val())) / 100;
            var randSymbol = deno < 1 ? "" : "R";

            var text = $.trim(value.val());
            if (text.length > 0) {
            	warningBox("Please enter multiples of <b>" + randSymbol + selectedText + "</b> as value !<br>Make sure you entered a number without spaces or special characters!");
            }

            value.val("");
            count.val("");
        }

        var subtotalparents = $(this).parents(".clsCoinssubtotal");
        var coinsCollection = subtotalparents.find("input[name='textCoinValue']");
        var textCoinsSubtotal = subtotalparents.find("input[name='textCoinsSubtotal']");

        getsubTotal(coinsCollection, textCoinsSubtotal);

        subtotalparents = $(this).parents(".CashForwardedContainerDrop, .CashRequiredContainerDrop");
        var textNotesSubtotal = subtotalparents.find("input[name='textNotesSubtotal']");
        var overallTotal = subtotalparents.find(".TotalInDrop");

        getTotalInDrop(overallTotal, textNotesSubtotal, textCoinsSubtotal);
    });




    $("#CashOrderContainer select[id='dropDownListCoin']").on('change', function () {
        var parents = $(this).parents(".clsCoinsline");
        var coin = $(this);
        var count = parents.find("input[name='textCoinCount']");
        var value = parents.find("input[name='textCoinValue']");
        // calculate coin value       
        calculate(coin, count, value);
        var subtotalparents = $(this).parents(".clsCoinssubtotal");

        var coinsCollection = subtotalparents.find("input[name='textCoinValue']");
        var textCoinsSubtotal = subtotalparents.find("input[name='textCoinsSubtotal']");

        getsubTotal(coinsCollection, textCoinsSubtotal);

        // remove selected denomination from the other dropdowns in container 
        var denominationCollection = $(this).parents(".clsCoinssubtotal");
        var listRows = denominationCollection.find("select[id='dropDownListCoin']");
        removeSelectedDenomination(coin, listRows);

        subtotalparents = $(this).parents(".CashForwardedContainerDrop, .CashRequiredContainerDrop");
        var textNotesSubtotal = subtotalparents.find("input[name='textNotesSubtotal']");
        var overallTotal = subtotalparents.find(".TotalInDrop");

        getTotalInDrop(overallTotal, textNotesSubtotal, textCoinsSubtotal);
    });


    function getTotalInDrop(overallTotal, notesTotal, coisTotal) {
        var notes = notesTotal.val();
        var totalNotes = parseFloat(notes);

        var coins = coisTotal.val();
        var totalCoins = parseFloat(coins);

        var allTotal = "Total : " + (totalNotes + totalCoins).toFixed(2);
        overallTotal.html(allTotal);
        //calculateCashOrderAmount();
    }



    // calculate sub total
    function getsubTotal(denominationGroupCollection, textSubTotal) {
        var linetotal = 0;
        denominationGroupCollection.each(function () {
            var textnote = $(this).attr('value');
            linetotal += isNaN(textnote) || textnote.trim() == "" ? 0 : parseFloat(textnote);
        });
        var subtotal = textSubTotal;
        subtotal.attr('value', linetotal.toFixed(2));
        linetotal = 0;
    }



    function calculate(denominationId, countId, valueId) {
        var denomination = denominationId.attr("value");
        var count = countId.attr('value');

        if (isNaN(count) || count.trim() == "") {
            valueId.attr('value', '');
        }
        else {
            var linetotal = (parseFloat(denomination) * parseFloat(count) / 100).toFixed(2);
            valueId.attr('value', (denomination == 1000500) ? '0.00' : linetotal);
        }
    }



    // Remove Previously Selected Denominations
    function removePreviouslySelectedDenominations(newdropdown, dropdownsCollection) {
        for (var i = 0; i < dropdownsCollection.length; i++) {
            if (dropdownsCollection[i].value != 1000500) {
                newdropdown.find("option[value='" + dropdownsCollection[i].value + "']").remove();
            }
        }
    }


	//
	// Remove selected denomination from the other dropdown elements of the same group.
	//
    function removeSelectedDenomination(selectedDropdown, denominationCollection) {
    	var selectedValue = selectedDropdown.val();
    	var selectedText = selectedDropdown.find("option:selected").text();
    	denominationCollection.each(function () {
    		var optionsCollection = $(this);

    		var itemsCollection = optionsCollection.children();
    		//
    		itemsCollection.each(function () {
    			var itemValue = $(this).val();
    			if (selectedValue != 1000500) {
    				if (itemValue == selectedValue) {
    					$(this).remove();
    				}
    			}
    		});
    	});

    	if (selectedValue != 1000500) {
    		selectedDropdown.append("<option value='" + selectedValue + "' selected='selected'>" + selectedText + "</option");
    		sortOrder(selectedDropdown);
    	}
    }


    function sortOrder(select) {
    	// Loop for each select element on the page.
    	select.each(function () {

    		// Keep track of the selected option.
    		var selectedValue = $(this).val();

    		// Sort all the options by Value.
    		$(this).html($("option", $(this)).sort(function (a, b) {
    			return parseFloat(a.value) == parseFloat(b.value) ? 0 : parseFloat(a.value) > parseFloat(b.value) ? -1 : 1;
    		}));

    		// Select one option.
    		$(this).val(selectedValue);
    	});
    }


    function warningBox(message) {
        var windowId = "MessageWindow";
        var header = "Alert Response";
        var errorWarning = "";
        showMessageBox(windowId, header, errorWarning, message, 430, 160);
    }



    function showMessageBox(windowId, header, errorWarning, message, width, height) {
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
                    //window.location.href = '@Url.Action("Index")';
                }

                kendoWindow.data("kendoWindow").close();
            })
            .end();
    }

});