
var container_clone;
var row_clone_notes;
var row_clone_coins;
var denomination_groups_clone;
var container_counter = 0;
var add_denomination_group_clone;
var deposit_reference_clone;
var container_types_dropdown_clone;
var ddlGlobalContainerType;


function AddContainer() {   
    // clone container
    container_counter++;
    var containerId = "container_clone_template" + container_counter;
    container_clone.clone(true).attr("id", containerId).appendTo("#ContainersHolder" );
    var invokingFunction = "AddContainer";
    $("#ContainersHolder .bagOrCanister .gap:not(:last)").removeClass('show').addClass('hide' );

    OnNewDenominationGroupOrAddContainer(invokingFunction);
    
    // New
    var dropOrDepositDescription = $('#ddlDepositType option:selected').text() == "Multi Drop" ? "Drop" : "Deposit";    

    var container = $("#ContainersHolder .bagOrCanister").last();
    var lblDropReferences = container.find(".lblDropReference");
    var labelDropAmount = container.find(".lblDropAmount");
    var dropTitleAndNumberLabels = container.find(".lblDropTitleAndNumber");

    lblDropReferences.each(function () {
        $(this).html((dropOrDepositDescription != 'Drop') ? "Custom Deposit Reference" : dropOrDepositDescription + " Reference");
    });

    labelDropAmount.each(function () {
        $(this).html(dropOrDepositDescription + " Amount");
    });

    var dropCount = 1;
    dropTitleAndNumberLabels.each(function () {
        $(this).html(dropOrDepositDescription + " " + dropCount);
        dropCount++;
    });

    if ($('#ddlDepositType option:selected').text() == "Multi Deposit") {
        var lblSerialNumber = container.find(".lblSerialNumber");
        var txtDropSerialNumber = container.find("input[name='txtDropSerialNumber']");

        lblSerialNumber.remove();
        txtDropSerialNumber.remove();
    }
    getContainerTypes();
}




function getContainerTypes() {

    var primaryBag = $("#ContainersHolder .bagOrCanister").first();
    var primaryContainerType = primaryBag.find("#ddlContainerType");

    var container = $("#ContainersHolder .bagOrCanister").last();
    var ddlContainerType = container.find("#ddlContainerType");
    ddlContainerType.find("option").remove();

    var options = primaryContainerType.find('option');
    if(options.length == 0)
    {
        primaryContainerType.append($('<option value="0">Please Select</option>'));
    }

    options.each(function () {
            ddlContainerType.append($('<option value="' + $(this).val() + '">' + $(this).text() + '</option>'));
    });
    
}


function OnNewDenominationGroupOrAddContainer(invokingFunction) {

    if (invokingFunction != "AddContainer") {
        $("#ContainersHolder .bagOrCanister").remove();
        container_clone.clone(true).attr("id", "container_clone_template").appendTo("#ContainersHolder");
    }

    $("#ddlDepositeReference").removeAttr("disabled");
    $("#ContainersHolder .bagOrCanister #hideDeleteContainer :first").hide();

    // remove the "Add New Container Drop". it will be created by Cloning on Multi Deposit & Multi Drop. 
    //Single deposit does not need it thus the removal.
    $("button[name='ButtonNewContainerDrop']").remove();
    
    $("div #containerbutton").removeClass('hide').addClass('show');

    var depositeTypeSelectedOption = $("#ddlDepositType");
    var selectedText = depositeTypeSelectedOption.find("option:selected").text();
    var dropOrDepositDescription = $('#ddlDepositType option:selected').text() == "Multi Drop" ? "Drop" : "Deposit";
    
    if (selectedText == 'Multi Deposit') {
        // deposit-reference-clone
        $(".deposit-reference-clone").remove();
    } else if ($(".deposit-reference-clone").length == 0) {
        deposit_reference_clone.insertAfter(".add-after-me");
    }

    switch (selectedText)
    {
        case 'Single Deposit':
            {   // Single Deposit
                $("#ContainersHolder .bagOrCanister .denomination_groups_parent:not(:first)").remove();
                $("div[name='IsNotForSingleDeposit']").removeClass('show').addClass('hide');
                $(".well").find("button[name='Submitted']").show();
                break;
            }
        case 'Multi Deposit':
            {
                // Multi Deposit
                add_denomination_group_clone.clone(true).appendTo(".sFieldsetContainer");               
                $("div[name='IsNotForSingleDeposit']").show();
                $(".well").find("button[name='Submitted']").hide();            
                $('#ddlDepositeReference option:first-child').attr("selected", "selected");
	            $("#ddlDepositeReference").change();
                $("#ddlDepositeReference").attr("disabled", "disabled");
                break;
            }
        case 'Multi Drop':
            {   // Multi Drop
                add_denomination_group_clone.clone(true).appendTo(".sFieldsetContainer");
                $("div[name='IsNotForSingleDeposit']").show();
                $(".well").find("button[name='Submitted']").hide();
                break;
            }
    }

    var buttonCopyDrop = $("#ContainersHolder").find("button[name ='ButtonCopyDrop']");
    buttonCopyDrop.each(function () {
        $(this).html("Copy " + dropOrDepositDescription);
    });

    var buttonDrop = $("#ContainersHolder").find("button[name ='ButtonNewContainerDrop']");
    buttonDrop.each(function () {
        $(this).html("Add New " + dropOrDepositDescription);
    });
    
    enumerateContainers();

    if ($("#CashCenterUser").length > 0) {
        getContainerTypes();
    }

    addContainerDropElements();
}




function addContainerDropElements() {

    var dropOrDepositDescription = $('#ddlDepositType option:selected').text() == "Multi Drop" ? "Drop" : "Deposit";

    var rowDropTitle = $(".ContainerDropElements").last().find("tr");

    var totalInDrop = rowDropTitle.find(".TotalInDrop").html();
    var dropAmount = rowDropTitle.find("input[name='txtDropAmount']");

    var dropAmountVal = (dropAmount.length == 0) ? "" : dropAmount.val();

    $(".ContainerDropElements").last().find("tr").remove();

    if ($('#ddlDepositType option:selected').text() == "Single Deposit") {
        var dropTitleAndNumber = "<tr>" +
                                     "<td colspan='2' style='width:370px;'>" +
                                         "<span class='lblDropTitleAndNumber' style='font-weight: bold'> " + dropOrDepositDescription + " 1 </span><span class='TotalInDrop'> "+ totalInDrop + "</span>" +
                                     "</td>" +
                                     "<td></td>" +
                                     "<td style='width:105px; padding-right:10px' class='labelDropAmount'>" + dropOrDepositDescription + " Amount</td>" +
                                     "<td><input style='width:215px;' value='" + dropAmountVal + "' type='text' name='txtDropAmount'/></td>" +
                                 "</tr>";

        $(".ContainerDropElements").last().append(dropTitleAndNumber);
    }
    else if ($('#ddlDepositType option:selected').text() == "Multi Deposit") {

        var dropSerialNumberControl = "<tr>" +
            "<td></td>" +
            "<td></td>" +
            "<td style='width:100px; padding-right:10px' class='labelDropAmount'>" + dropOrDepositDescription + " Amount</td>" +
            "<td><input style='width:215px;' type='text' name='txtDropAmount'/></td>" +
            "</tr>";

        dropSerialNumberControl += "<tr>" +
            "<td>Custom " + dropOrDepositDescription + " Reference</td>" +
            "<td><input type='text' name='txtDropReference'/> </td>" +
            "<td style='padding-right:10px'>" +
            "<span class='lblDropTitleAndNumber'>" + dropOrDepositDescription + " 1</span> </td>" +
            "<td><span class='TotalInDrop'>Total : R 0.00</span></td>" +
            "</tr>";

        $(".ContainerDropElements").last().append(dropSerialNumberControl);
    }
    else {
        var dropSerialNumberControl = "<tr>" +
            "<td>" + dropOrDepositDescription + " Serial Number</td>" +
            "<td><input type='text' name='txtDropSerialNumber' maxlength=14/></td>" +
            "<td style='width:80px; padding-right:10px'>Drop Amount</td>" +
            "<td><input style='width:215px;' type='text' name='txtDropAmount'/></td>" +
            "</tr>";
        
        dropSerialNumberControl += "<tr>" +
            "<td>" + dropOrDepositDescription + " Reference</td>" +
            "<td><input type='text' name='txtDropReference'/> </td>" +
            "<td style='width:80px; padding-right:10px'>" +
            "<span class='lblDropTitleAndNumber'>" + dropOrDepositDescription + " 1</span> </td>" +
            "<td><span class='TotalInDrop'>Total : R 0.00</span></td>" +
            "</tr>";

        $(".ContainerDropElements").last().append(dropSerialNumberControl);
    }
}

//document ready
//

$(document).ready(function () {

    container_clone = $("#container_clone_template").clone(true);
    row_clone_notes = $(".outer .clsline").clone(true);
    row_clone_coins = $(".outer .clsCoinsline").clone(true);
    denomination_groups_clone = $(".outer_groups_clone").clone(true);
    add_denomination_group_clone = $("button[name='ButtonNewContainerDrop']").clone(true);
    deposit_reference_clone = $(".deposit-reference-clone").clone(true);
    

    $("#ContainersHolder .bagOrCanister #containerbutton").removeClass('show').addClass('hide');



    $("#txtTotalDepositAmount").on("keyup", function () {
        var amount = $(this).val();

        if (isNaN(amount) || amount < 1) {
            $("#txtTotalDepositAmount").css({ 'border-color': 'red' });
        }
        else {
            $("#txtTotalDepositAmount").css({ 'border-color': '#d1c7ac' });
        }
    });



    $("#txtTotalDepositAmount").on("blur", function () {

        var amount = $(this).val();
        if (isNaN(amount) || amount < 1) {
            $("#txtTotalDepositAmount").css({ 'border-color': 'red' });
            $(this).focus();
        }
        else {
            $(this).val(parseFloat(amount).toFixed(2));
            $("#txtTotalDepositAmount").css({ 'border-color': '#d1c7ac' });
        }
    });

    //
    $("#ContainersHolder select[name='dropDownListNote']").on('change', function () {
        // initialize / get values from child controls as per selector
        var parents = $(this).parents(".bagOrCanister .clsline");
        var note = $(this);
        var count = parents.find("input[name='textNoteCount']");
        var value = parents.find("input[name='textNoteValue']");

        Calculate(note, count, value);
        var subtotalparents = $(this).parents(".bagOrCanister .clsNotessubtotal");

        var notesCollection = subtotalparents.find("input[name='textNoteValue']");
        var textSubtotal = subtotalparents.find("input[name='textNotesSubtotal']");

        getsubTotal(notesCollection, textSubtotal);
		
        // remove selected denomination from the other dropdowns in container 
        var denominationCollection = $(this).parents(".bagOrCanister .clsNotessubtotal");
        var listRows = denominationCollection.find("select[name='dropDownListNote']");
       
        removeSelectedDenomination(note, listRows);
      
        subtotalparents = $(this).parents(".bagOrCanister .denomination_groups_clone");
        var textCoinsSubtotal = subtotalparents.find("input[name='textCoinsSubtotal']");
        var overallTotal = subtotalparents.find(".TotalInDrop");
		
        getTotalInDrop(overallTotal, textSubtotal, textCoinsSubtotal);
    });


    $("#txtTotalDepositAmount, input[name='txtDropAmount'], input[name='textNoteValue'], input[name='textCoinValue']").on('blur', function () {

        var amount = $(this).val();

        if (isNumber(amount)) {
            $(this).val(parseFloat(amount).toFixed(2));
        }
});



function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}


    $("#ContainersHolder input[name='textNoteValue']").on('keyup', function () {
        // initialize / get values from child controls as per selector
        var parents = $(this).parents(".bagOrCanister .clsline");
        var note = parents.find("select[name='dropDownListNote']");
        var value = $(this);
        var count = parents.find("input[name='textNoteCount']");

        if (note.val() == 1000500 || value.val() <= 0 || isNaN(value.val())) {
            value.attr('value', "");
            count.attr('value', "");
        }

        var subtotalparents = $(this).parents(".bagOrCanister .clsNotessubtotal");
        var notesCollection = subtotalparents.find("input[name='textNoteValue']");
        var textSubtotal = subtotalparents.find("input[name='textNotesSubtotal']");

        getsubTotal(notesCollection, textSubtotal);

        subtotalparents = $(this).parents(".bagOrCanister .denomination_groups_clone");
        var textCoinsSubtotal = subtotalparents.find("input[name='textCoinsSubtotal']");
        var overallTotal = subtotalparents.find(".TotalInDrop");

        getTotalInDrop(overallTotal, textSubtotal, textCoinsSubtotal);
        count.attr("value", "");
    });



    $("#ContainersHolder input[name='textNoteValue']").on('blur', function () {
        // initialize / get values from child controls as per selector
        var parents = $(this).parents(".bagOrCanister .clsline");
        var note = parents.find("select[name='dropDownListNote']");
        var value = $(this);
        var count = parents.find("input[name='textNoteCount']");

        if (note.val() == 1000500 || value.val() <= 0 || isNaN(value.val())) {
            value.attr('value', "");
            count.attr('value', "");
        }

        var denomination = parseFloat(note.val() / 100);
        var amount = parseFloat(value.val());
        var result = parseInt(amount) / parseInt(denomination);

        var isNotANumberPassed = true;

        if (isNaN(result)) {
            isNotANumberPassed = note.val() == 1000500;
        }

        if (isNotANumberPassed == false || amount % denomination > 0) {
            var selectedText = note.find("option:selected").text();
            var deno = (parseInt(note.val())) / 100;
            var randSymbol = deno < 1 ? "" : "R";
	        var text = $.trim(value.val());
            if (text.length > 0) {
            	warningBox("Please enter multiples of <b>" + randSymbol + selectedText + "</b> as value !<br>Make sure you entered a number without spaces or special characters!");
            }
            value.attr('value', "");
            count.attr("value", "");
        }
        else count.val(parseInt(result));

        var subtotalparents = $(this).parents(".bagOrCanister .clsNotessubtotal");
        var notesCollection = subtotalparents.find("input[name='textNoteValue']");
        var textSubtotal = subtotalparents.find("input[name='textNotesSubtotal']");

        getsubTotal(notesCollection, textSubtotal);

        subtotalparents = $(this).parents(".bagOrCanister .denomination_groups_clone");
        var textCoinsSubtotal = subtotalparents.find("input[name='textCoinsSubtotal']");
        var overallTotal = subtotalparents.find(".TotalInDrop");

        getTotalInDrop(overallTotal, textSubtotal, textCoinsSubtotal);
    });


    $("#ContainersHolder input[name='textNoteCount']").on('keyup', function () {
        // initialize / get values from child controls as per selector
        var parents = $(this).parents(".bagOrCanister .clsline");
        var note = parents.find("select[name='dropDownListNote']");
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
        Calculate(note, count, value);
        var subtotalparents = $(this).parents(".bagOrCanister .clsNotessubtotal");

        var notesCollection = subtotalparents.find("input[name='textNoteValue']");
        var textSubtotal = subtotalparents.find("input[name='textNotesSubtotal']");

        getsubTotal(notesCollection, textSubtotal);

        subtotalparents = $(this).parents(".bagOrCanister .denomination_groups_clone");
        var textCoinsSubtotal = subtotalparents.find("input[name='textCoinsSubtotal']");
        var overallTotal = subtotalparents.find(".TotalInDrop");

        getTotalInDrop(overallTotal, textSubtotal, textCoinsSubtotal);
    });


    // attach AddNotes() event handler
    $("#ContainersHolder a[name='buttonAddNotes']").on('click', function () {
        var parents = $(this).parents(".denominationparent");
        
        var containerDrop = $(this).parents(".denomination_groups_clone");

        var hasSubmittedDrops = false;
        var hdDropStatuses = containerDrop.find("input[name='hdDropStatusName']");

        hdDropStatuses.each(function () {
        	if ($(this).val() == "SUBMITTED") {
                hasSubmittedDrops = true;
            }
        });

        if (hasSubmittedDrops == true) {
            var dropOrDepositDescription = $('#ddlDepositType option:selected').text() == "Multi Drop" ? "Drop" : "Deposit";
            warningBox("Cannot add denominations to a submitted " + dropOrDepositDescription);
            return true;
        }
        else {

            // only add 6 denomination groups
            if (parents.find(".clsline").length < 5) {
                // perform copy denomination group

                row_clone_notes.clone(true).appendTo(parents);

                var denominationCollection = $(this).parents(".bagOrCanister .clsNotessubtotal");
                var listRows = denominationCollection.find("select[name='dropDownListNote']");
                var dropdown = parents.find(".last select:last");

                parents.find(".denominationparent .remove").removeClass('hide').addClass('show');

                OnAddNewRemovePreviouslySelectedDenominations(dropdown, listRows);
            }
            return true;
        }
        
    });
    


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



    // attach RemoveNotes() event handler
    $("#ContainersHolder a[name='buttonRemoveNotes']").on('click', function () {

        var containerDrop = $(this).parents(".denomination_groups_clone");

        var hasSubmittedDrops = false;
        var hdDropStatuses = containerDrop.find("input[name='hdDropStatusName']");

        hdDropStatuses.each(function () {
        	if ($(this).val() == "SUBMITTED") {
                hasSubmittedDrops = true;
            }
        });

        if (hasSubmittedDrops == true) {
            var dropOrDepositDescription = $('#ddlDepositType option:selected').text() == "Multi Drop" ? "Drop" : "Deposit";
            warningBox("Cannot delete denominations from a submitted " + dropOrDepositDescription);
            return true;
        }
        else {

            var clsNotesParents = $(this).parents(".clsNotessubtotal");
            var parents = $(this).closest("tr").remove();
            var removeButton = parents.find(".remove:first");
            removeButton.hide();

            var notesCollection = clsNotesParents.find("input[name='textNoteValue']");
            var textNotesSubtotal = clsNotesParents.find("input[name='textNotesSubtotal']");
            getsubTotal(notesCollection, textNotesSubtotal);

            var subtotalparents = clsNotesParents.parents(".bagOrCanister .denomination_groups_clone");
            var textCoinsSubtotal = subtotalparents.find("input[name='textCoinsSubtotal']");
            var overallTotal = subtotalparents.find(".TotalInDrop");

            getTotalInDrop(overallTotal, textNotesSubtotal, textCoinsSubtotal);
            return true;
        }
        
    });


    // attach Remove Denomination group event handler
    $("#ContainersHolder a[name='DeleteContainerDrop'], .DeleteContainerDrop").on('click', function () {
	
        var dropOrDepositDescription = $('#ddlDepositType option:selected').text() == "Multi Drop" ? "Drop" : "Deposit";
  
        // Drop Status Name
        var hdDropStatusName = $(this).parents(".drop_level").find("input[name='hdDropStatusName']").val();
       
        if (hdDropStatusName == "SUBMITTED") {
            warningBox("Cannot remove a submitted " + dropOrDepositDescription);
            return true;
        }

        var currentContainer = $(this).parents(".bagOrCanister");
        var parents = $(this).closest(".outer_groups_clone").remove();


        var currentDrop = $(this).closest(".outer_groups_clone");
        
        var removeButton = parents.find(".remove:first");
        removeButton.hide();

        var smallBags = currentContainer.find(".denomination_groups_clone");
        
        var counter = 0;

        smallBags.each(function () {
            //if ($(this) != currentDrop) {
                counter++;
                $(this).find("input[name='hdDropNumber']").val(counter);
                $(this).find(".lblDropTitleAndNumber").html(dropOrDepositDescription + " " + counter);
            //}
        });

        enumerateContainers();
    });




    // perform remove container
    $("#ContainersHolder button[name='btnRemoveContainer']").on('click', function () {
        var bag = $(this).parents(".bagOrCanister");

        var hasSubmittedDrops = false;
        var hdDropStatuses = bag.find("input[name='hdDropStatusName']");
        
        hdDropStatuses.each(function() {
        	if ($(this).val() == "SUBMITTED") {
                hasSubmittedDrops = true;
            }
        });

        if (hasSubmittedDrops == true) {
            var dropOrDepositDescription = $('#ddlDepositType option:selected').text() == "Multi Drop" ? "Drop" : "Deposit";
            warningBox("Cannot remove this container\nSome " + dropOrDepositDescription + "(s) are already submitted!");
            return true;
        }
        else {
            bag.remove();
            enumerateContainers();
        }
    });


    
    // On change deposite type
    $("#ddlDepositType").on('change', function () {
        OnNewDenominationGroupOrAddContainer("OnDropdownDepositeType");

        var primaryContainerType = $("#ddlGlobalContainerType");

        var container = $("#ContainersHolder .bagOrCanister").last();
        var ddlContainerType = container.find("#ddlContainerType");
        ddlContainerType.find("option").remove();

        var options = primaryContainerType.find('option');
        if (options.length == 0) {
            primaryContainerType.append($('<option value="0">Please Select</option>'));
        }

        options.each(function () {
            ddlContainerType.append($('<option value="' + $(this).val() + '">' + $(this).text() + '</option>'));
        });
    });



    // On Container Drop Cloning
    $("button[name='ButtonNewContainerDrop']").on('click', function () {
        
        var dropOrDepositDescription = $('#ddlDepositType option:selected').text() == "Multi Drop" ? "Drop" : "Deposit";

        var currentContainer = $(this).parents(".bagOrCanister");
        var containerDrops = currentContainer.find(".denomination_groups_parent");
        denomination_groups_clone.clone(true).appendTo(containerDrops);

        containerDrops = $(this).parents(".bagOrCanister #fieldsetContainer");
        
        //var denominationChildren = containerDrops.find(".lblDropTitleAndNumber");
        var labelDropAmount = containerDrops.find(".lblDropAmount");
        var lblDropReferences = containerDrops.find(".lblDropReference");

        var lblSerialNumber = containerDrops.find(".lblSerialNumber");
        lblSerialNumber.html(dropOrDepositDescription + " Serial Number");

        labelDropAmount.each(function () {
            $(this).html(dropOrDepositDescription + " Amount");
        });

        var smallBags = currentContainer.find(".denomination_groups_clone");
        var counter = 0;

        smallBags.each(function () {
            counter++;
            $(this).find("input[name='hdDropNumber']").val(counter);
            $(this).find(".lblDropTitleAndNumber").html(dropOrDepositDescription + " " + counter);
        });
        
        // Deposit Reference
        lblDropReferences.each(function () {
            $(this).html((dropOrDepositDescription != 'Drop') ? "Custom Deposit Reference" : dropOrDepositDescription + " Reference");
        });
            
        $("#ContainersHolder a[name='DeleteContainerDrop'] :not(:first)").removeClass('hide').addClass('show');

        if ($('#ddlDepositType option:selected').text() == "Multi Deposit") {
            var txtDropSerialNumber = containerDrops.find("input[name='txtDropSerialNumber']");
            
            lblSerialNumber.remove();
            txtDropSerialNumber.remove();
        }


        var buttonCopyDrop = $("button[name ='ButtonCopyDrop']");
        buttonCopyDrop.each(function () {
            $(this).html("Copy " + dropOrDepositDescription);
        });

    });




    //=========================== COINS =========================================

    $("#ContainersHolder select[name='dropDownListCoin']").on('change', function () {
        // initialize / get values from child controls as per selector
        var parents = $(this).parents(".bagOrCanister .clsCoinsline");
        var coin = $(this);
        var count = parents.find("input[name='textCoinCount']");
        var value = parents.find("input[name='textCoinValue']");
        // calculate coin value       
        Calculate(coin, count, value);
        var subtotalparents = $(this).parents(".bagOrCanister .clsCoinssubtotal");

        var coinsCollection = subtotalparents.find("input[name='textCoinValue']");
        var textCoinsSubtotal = subtotalparents.find("input[name='textCoinsSubtotal']");

        getsubTotal(coinsCollection, textCoinsSubtotal);

        // remove selected denomination from the other dropdowns in container 
        var denominationCollection = $(this).parents(".bagOrCanister .clsCoinssubtotal");
        var listRows = denominationCollection.find("select[name='dropDownListCoin']");
        removeSelectedDenomination(coin, listRows);

        subtotalparents = $(this).parents(".bagOrCanister .denomination_groups_clone");
        var textNotesSubtotal = subtotalparents.find("input[name='textNotesSubtotal']");
        var overallTotal = subtotalparents.find(".TotalInDrop");

        getTotalInDrop(overallTotal, textNotesSubtotal, textCoinsSubtotal);
    });



    $("#ContainersHolder input[name='textCoinCount']").on('keyup', function () {
        // initialize / get values from child controls as per selector
        var parents = $(this).parents(".bagOrCanister .clsCoinsline");
        var coin = parents.find("select[name='dropDownListCoin']");
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
        Calculate(coin, count, value);
        var subtotalparents = $(this).parents(".bagOrCanister .clsCoinssubtotal");

        var coinsCollection = subtotalparents.find("input[name='textCoinValue']");
        var textCoinsSubtotal = subtotalparents.find("input[name='textCoinsSubtotal']");

        getsubTotal(coinsCollection, textCoinsSubtotal);

        subtotalparents = $(this).parents(".bagOrCanister .denomination_groups_clone");
        var textNotesSubtotal = subtotalparents.find("input[name='textNotesSubtotal']");
        var overallTotal = subtotalparents.find(".TotalInDrop");

        getTotalInDrop(overallTotal, textNotesSubtotal, textCoinsSubtotal);
    });



    $("#ContainersHolder input[name='textCoinValue']").on('keyup', function () {
        // initialize / get values from child controls as per selector
        var parents = $(this).parents(".bagOrCanister .clsCoinsline");
        var coin = parents.find("select[name='dropDownListCoin']");
        var value = $(this);
        var count = parents.find("input[name='textCoinCount']");

        if (coin.val() == 1000500) {
            value.attr('value', '');
        }

        if (value.val() < 0 || isNaN(value.val())) {
            value.attr("value", "");
            count.attr("value", "");
        } 


        var subtotalparents = $(this).parents(".bagOrCanister .clsCoinssubtotal");
        var coinsCollection = subtotalparents.find("input[name='textCoinValue']");
        var textCoinsSubtotal = subtotalparents.find("input[name='textCoinsSubtotal']");

        getsubTotal(coinsCollection, textCoinsSubtotal);

        subtotalparents = $(this).parents(".bagOrCanister .denomination_groups_clone");
        var textNotesSubtotal = subtotalparents.find("input[name='textNotesSubtotal']");
        var overallTotal = subtotalparents.find(".TotalInDrop");

        getTotalInDrop(overallTotal, textNotesSubtotal, textCoinsSubtotal);
        count.attr("value", "");
    });



    $("#ContainersHolder input[name='textCoinValue']").on('blur', function () {
        // initialize / get values from child controls as per selector
        var parents = $(this).parents(".bagOrCanister .clsCoinsline");
        var coin = parents.find("select[name='dropDownListCoin']");
        var value = $(this);
        var count = parents.find("input[name='textCoinCount']");


        var denomination = (parseFloat(coin.val())) / 100;
        var amount = parseFloat(value.val());
        var result = amount / denomination;

        if (coin.val() == 1000500) {
            value.attr('value', 0.00);
        }

        if (value.val() <= 0 || isNaN(value.val())) {
            value.attr("value", "");
            count.attr("value", "");
        } else {
            var val = (amount * 10) / (denomination * 10);
            count.attr('value', val);
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

            value.attr('value', "");
            count.attr("value", "");
        }

        var subtotalparents = $(this).parents(".bagOrCanister .clsCoinssubtotal");
        var coinsCollection = subtotalparents.find("input[name='textCoinValue']");
        var textCoinsSubtotal = subtotalparents.find("input[name='textCoinsSubtotal']");

        getsubTotal(coinsCollection, textCoinsSubtotal);

        subtotalparents = $(this).parents(".bagOrCanister .denomination_groups_clone");
        var textNotesSubtotal = subtotalparents.find("input[name='textNotesSubtotal']");
        var overallTotal = subtotalparents.find(".TotalInDrop");

        getTotalInDrop(overallTotal, textNotesSubtotal, textCoinsSubtotal);
    });



    // attach AddCoins() event handler
    $("#ContainersHolder a[name='buttonAddCoins']").on('click', function () {
        var parents = $(this).parents(".denominationparent");

        var containerDrop = $(this).parents(".denomination_groups_clone");

        var hasSubmittedDrops = false;
        var hdDropStatuses = containerDrop.find("input[name='hdDropStatusName']");

        hdDropStatuses.each(function () {
        	if ($(this).val() == "SUBMITTED") {
                hasSubmittedDrops = true;
            }
        });

        if (hasSubmittedDrops == true) {
            var dropOrDepositDescription = $('#ddlDepositType option:selected').text() == "Multi Drop" ? "Drop" : "Deposit";
            warningBox("Cannot add denominations to a submitted " + dropOrDepositDescription);
            return true;
        }
        else {

            // only add 6 denomination groups
            if (parents.find(".clsCoinsline").length < 7) {
                // perform copy denomination group
                row_clone_coins.clone(true).appendTo(parents);
                var denominationCollection = $(this).parents(".bagOrCanister .clsCoinssubtotal");
                var listRows = denominationCollection.find("select[name='dropDownListCoin']");
                var dropdown = parents.find(".last select:last");

                parents.find(".denominationparent .remove").removeClass('hide').addClass('show');

                OnAddNewRemovePreviouslySelectedDenominations(dropdown, listRows);
            }
            return true;
        }
    });


    // attach RemoveCoins() event handler
    $("#ContainersHolder a[name='buttonRemoveCoins']").on('click', function () {

        var containerDrop = $(this).parents(".denomination_groups_clone");

        var hasSubmittedDrops = false;
        var hdDropStatuses = containerDrop.find("input[name='hdDropStatusName']");

        hdDropStatuses.each(function () {
        	if ($(this).val() == "SUBMITTED") {
                hasSubmittedDrops = true;
            }
        });

        if (hasSubmittedDrops == true) {
            var dropOrDepositDescription = $('#ddlDepositType option:selected').text() == "Multi Drop" ? "Drop" : "Deposit";
            warningBox("Cannot delete denominations from a submitted " + dropOrDepositDescription);
            return true;
        }
        else {
            var clsCoinssubtotal = $(this).parents(".clsCoinssubtotal");
            var parents = $(this).closest("tr").remove();
            var removeButton = parents.find(".remove:first");
            removeButton.hide();

            var coinsCollection = clsCoinssubtotal.find("input[name='textCoinValue']");
            var textCoinsSubtotal = clsCoinssubtotal.find("input[name='textCoinsSubtotal']");
            getsubTotal(coinsCollection, textCoinsSubtotal);

            var subtotalparents = clsCoinssubtotal.parents(".bagOrCanister .denomination_groups_clone");
            var textNotesSubtotal = subtotalparents.find("input[name='textNotesSubtotal']");
            var overallTotal = subtotalparents.find(".TotalInDrop");

            getTotalInDrop(overallTotal, textNotesSubtotal, textCoinsSubtotal);
            
            return true;
        }
    });

    OnNewDenominationGroupOrAddContainer("AddContainer");
});                         // end doc ready


function getTotalInDrop(overallTotal, notesTotal, coisTotal) 
{
    var notes = notesTotal.val();
    var totalNotes = parseFloat(notes);

    var coins = coisTotal.val();
    var totalCoins = parseFloat(coins);

    var allTotal = "Total : " + (totalNotes + totalCoins).toFixed(2);
    overallTotal.html(allTotal);
    enumerateContainers();
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
        Sort(selectedDropdown);
    }
}


function Sort(select) {
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


// calculate sub total
function getsubTotal(denominationGroupCollection, textSubTotal) 
{
    var linetotal = 0;
    denominationGroupCollection.each(function () {
        var textnote = $(this).val();
        linetotal += isNaN(textnote) || textnote.trim() == "" ? 0 : parseFloat(textnote);
    });
    var subtotal = textSubTotal;
    subtotal.attr('value', linetotal.toFixed(2));
    linetotal = 0;
}


function Calculate(denominationId, countId, valueId) {
    var denomination = denominationId.attr("value"); 
    var count = countId.val();

    if (isNaN(count) || count.trim() == "") {
        valueId.attr('value', '');
    }
    else {
        var linetotal = (parseFloat(denomination) * parseFloat(count) / 100 ).toFixed(2);
        valueId.attr('value', (denomination == 1000500) ? '0.00' : linetotal);
    }
}


// Remove Previously Selected Denominations
function OnAddNewRemovePreviouslySelectedDenominations(newdropdown, dropdownsCollection) {
    for (var i = 0; i < dropdownsCollection.length; i++) {
        if (dropdownsCollection[i].value != 1000500) {
            newdropdown.find("option[value='" + dropdownsCollection[i].value + "']").remove();
        }
    }
}


