


$(document).ready(function () {
    
    // On Submit Cash deposit
    $("button[name='ButtonSubmitDrop']").click(function () {
        var dropDescription = $('#ddlDepositType option:selected').text() == "Multi Drop" ? "Drop" : "Deposit";
        var depositType = $('#ddlDepositType option:selected').text();

        loadingWindow("Please wait...<br>Submitting a " + dropDescription);
        
        // Bag Properties
        var currentContainer = $(this).parents(".bagOrCanister");
        
        var containerTypeId = currentContainer.find("#ddlContainerType");
        var containerTotal = currentContainer.find(".container_header_total").html();
        var attribute1 = currentContainer.find("#Attribute1");
        var attribute2 = currentContainer.find("#Attribute2");
        var allContainers = $("#ContainersHolder .bagOrCanister");

        
        // ContainerDrop Properties
        var submitButton = $(this);
        var formDrop = $(this).parents(".drop_level");
        var containerDrop = $(this).parents(".denomination_groups_clone");
        
        var dropBagSerialNumberInput = containerDrop.find("input[name='txtDropSerialNumber']");
        var txtDropAmount = $.trim(containerDrop.find("input[name='txtDropAmount']").val()); // check if its equal to drop total amount below  

        var containerId = $(this).parents(".bagOrCanister").find("input[name='hdContainerId']").val();
        
        var containerDropModel =
        {
            ContainerDropId: containerDrop.find("input[name='hdContainerDropId']").val(),
            ContainerId: containerId,
            DropNumber: containerDrop.find("input[name='hdDropNumber']").val(),
            DropTotal: 0,
            StatusName : "Submitted",
            ClientReference: containerDrop.find("input[name='txtDropReference']").val(),
            DropBagSerialNumber: $.trim(dropBagSerialNumberInput.length == 0 ? "" : dropBagSerialNumberInput.val()),
            ContainerDropItems: []
        };

        var containerDropItems = formDrop.find("select");
        var dropTotalAmount = 0;


        containerDropItems.each(function () {
            var row = $(this).parents(".clsline, .clsCoinsline");
            var noteValue = row.find("input[name='textNoteValue']");
            var coinValue = row.find("input[name='textCoinValue']");
            var noteCount = row.find("input[name='textNoteCount']");
            var coinCount = row.find("input[name='textCoinCount']");

            var value = noteValue.length == 0 ? coinValue : noteValue;
            var count = noteCount.length == 0 ? coinCount : noteCount;

            var containerDropItemModel =
            {
                ContainerDropId: containerDropModel.ContainerDropId,
                ValueInCents: $(this).attr("value") == 1000500 ? 0 : $(this).attr("value"),
                Count: isNumber(count.attr('value')) == false ? 0 : count.attr('value'),
                Value: isNumber(value.attr('value')) == false ? 0 : value.attr('value')
            };

            dropTotalAmount += parseFloat(containerDropItemModel.Value);

            // add denominations
            if ($(this).attr("value") != 1000500 && containerDropItemModel.Count > 0) {
                containerDropModel.ContainerDropItems.push(containerDropItemModel);
            }
        });

        containerDropModel.DropTotal = dropTotalAmount;

        var containerModel =
        {
            ContainerId: currentContainer.find("input[name='hdContainerId']").val(),
            CashDepositId: $("#hdCashDepositId").val(),
            ContainerTypeId: (containerTypeId.val() == -1) ? null : containerTypeId.val(),
            Attribute1: attribute1.length == 0 ? "" : attribute1.val(),
            Attribute2: attribute2.length == 0 ? "" : attribute2.val(),
            IsPrimaryContainer: allContainers.length == 1,
            TotalContainerAmount: containerTotal,
            ContainerDrops: []
        };

        containerModel.ContainerDrops.push(containerDropModel);
        

        // Cash deposit Properties        var referenceNumber = "";
        var siteId = ($("#ddlSiteSbvUser").length == 0) ? $("#hdSiteId").val() : $("#ddlSiteSbvUser").val();
        
        var referenceNumber = "";

        if ($("#ddlDepositeReference").length > 0) {
            referenceNumber = $("#ddlDepositeReference").val() != 0 ? $("#ddlDepositeReference option:selected").text() : $.trim($("#txtCustomDepositeRef").val());
        }
        else {
            referenceNumber = $("#txtCustomDepositeRef").val();
        }

        var cashDepositModel =
        {
            CashDepositId: $.trim($('#hdCashDepositId').val()),
            DepositTypeId: $.trim($('#ddlDepositType').val()),
            DepositTypeName: $('#ddlDepositType option:selected').text(),
            SiteId: siteId,
            SiteSettlementAccountId: $.trim($('#ddlSiteSettlementAccount').val()),
            ReferenceNumber: referenceNumber,
            DepositStartDate: $.trim($('#hdDepositStartDate').val()),
            TotalDeposited: $('#txtTotalDepositAmount').val(),
            CapturingSourceName: "SBV Portal",
            TransactionStatusName: 'Active',
            Containers: []
        };
        cashDepositModel.Containers.push(containerModel);
        
        if (containerTypeId.val() == -1 || containerTypeId.val() == "" || containerTypeId.val() == null) {
            $("#btnConfirmCancel").click();
            showMessageBox("MessageWindow", "Validation Error", "", "Please select Container Type!");
            $("#btnConfirmCancel").click();
            return true;
        }

        if (attribute1.length != 0 && $.trim(attribute1.val()).length < 14) {
            $("#btnConfirmCancel").click();
            showMessageBox("MessageWindow", "Validation Error", "", "Countainer Serial Number must be 14 characters.");
            $("#btnConfirmCancel").click();
            return true;
        }

        if (attribute1.length > 0 && isInitialCitStartCode(attribute1.val()) == false) {
            $("#btnConfirmCancel").click();
            showMessageBox("MessageWindow", "Validation Error", "", " 'Serial Number' must be a number starting with " + $.trim($("#hdInitialCitSerialNumber").val()));
            $("#btnConfirmCancel").click();
            return true;
        }

        if (attribute2.length != 0 && $.trim(attribute2.val()).length < 14) {
            $("#btnConfirmCancel").click();
            showMessageBox("MessageWindow", "Validation Error", "", "Countainer Seal Number must be 14 characters.");
            $("#btnConfirmCancel").click();
            return true;
        }

        if (attribute2.length > 0 && isSealNumberANumber(attribute2.val()) == false) {
            $("#btnConfirmCancel").click();
            showMessageBox("MessageWindow", "Validation Error", "", "Countainer Seal Number must be a number.");
            $("#btnConfirmCancel").click();
            return true;
        }

        if (attribute2.length > 0 && isSealNumberANumber(attribute2.val()) == false) {
            $("#btnConfirmCancel").click();
            showMessageBox("MessageWindow", "Validation Error", "", "Countainer Seal Number must be a number.");
            $("#btnConfirmCancel").click();
            return true;
        }

        if (depositType == "Multi Drop") {
            if (containerDropModel.DropBagSerialNumber != "") {
                arrayOfDropSerialNumbers.push(containerDropModel.DropBagSerialNumber);
            }

            if (containerDropModel.DropBagSerialNumber.length < 3) {
                $("#btnConfirmCancel").click();
                showMessageBox("MessageWindow", "Validation Error", "", "Please enter correct " + dropDescription + " Serial Number.");
                $("#btnConfirmCancel").click();
                return true;
            }

            if (isDropSealNumbersDuplicated()) {
                $("#btnConfirmCancel").click();
                showMessageBox("MessageWindow", "Validation Error", "", "Please remove duplicate drop Serail Number.");
                $("#btnConfirmCancel").click();
                return true;
            }
        }

        var txtDropReference = containerDrop.find("input[name='txtDropReference']");

        if (txtDropReference.length == 0 || $.trim(txtDropReference.val()) == "") {
            $("#btnConfirmCancel").click();
            showMessageBox("MessageWindow", "Validation Error", "", "Please enter " + dropDescription + " Reference.");
            $("#btnConfirmCancel").click();
            return true;
        }

        if (containerDropModel.ContainerDropItems.length == 0) {
            $("#btnConfirmCancel").click();
            showMessageBox("MessageWindow", "Validation Error", "", "There are no denominations in " + dropDescription);
            $("#btnConfirmCancel").click();
            return true;
        }


        var cumDropAmount = parseFloat(txtDropAmount).toFixed(2);
        var containerAmount = parseFloat(containerDropModel.DropTotal).toFixed(2);

        if (cumDropAmount != containerAmount) {
            $("#btnConfirmCancel").click();
            showMessageBox("MessageWindow", "Submit Failure", "", dropDescription + " Amount not matching " + dropDescription + " Total");
            return false;
        }


        // Check if NOTES are exceeding "R1,000,000"
        var notesSubTotal = 0;
        var allNotesSubtotals = $("input[name='textNotesSubtotal']");
        allNotesSubtotals.each(function () {
            notesSubTotal += parseFloat($(this).val());
        });

        if (parseFloat(notesSubTotal) > 1000000) {
            $("#btnConfirmCancel").click();
            showMessageBox("MessageWindow", "Validation Error", "", "Total Notes Value must not exceed R1,000,000");
            return false;
        }

        // Check if COINS are exceeding "R1,000,000"
        var coinsSubTotal = 0;
        var allCoinsSubtotals = $("input[name='textCoinsSubtotal']");
        allCoinsSubtotals.each(function () {
            coinsSubTotal += parseFloat($(this).val());
        });
       
        if (parseFloat(coinsSubTotal) > 100000) {
            $("#btnConfirmCancel").click();
            showMessageBox("MessageWindow", "Validation Error", "", "Total Coins Value must not exceed R100,000");
            return false;
        }

        
        // Post to server
        if (cashDepositModel.CashDepositId == 0 && containerModel.ContainerId == 0) { // CashDeposit 
            submitContainerDropInNewContainerInNewCashDeposit(cashDepositModel, submitButton, formDrop, dropDescription);
        }
        else if (containerModel.ContainerId > 0) { // Container
            submitDropOrDeposit(containerDropModel, submitButton, formDrop, dropDescription, containerTotal);
        }
        else { // Container Drop
            var totalDepositAmount = $('#txtTotalDepositAmount').val();
            submitNewDropInNewContainer(containerModel, submitButton, formDrop, dropDescription, totalDepositAmount);
        }
    });
    
    
    $("button[name='ButtonCopyDrop']").click(function () {

        var dropDescription = $('#ddlDepositType option:selected').text() == "Multi Drop" ? "Drop" : "Deposit";


        var containerDropItems =
        {
            ContainerDropId: 0,
            ValueInCents: 0,
            Count: 0,
            Value: 0
        };

        var containerId = $(this).parents(".bagOrCanister").find("input[name='hdContainerId']").val();
        var containerDropObject =
        {
            ContainerDropId: 0,
            ContainerId: containerId,
            ContainerDropItems: []
        };
        
        var formDrop = $(this).parents(".drop_level");
        var containerDropControlItems = formDrop.find("select");
        var hasPassed = false;

        containerDropControlItems.each(function () {
            var row = $(this).parents(".clsline, .clsCoinsline");
            var noteValue = row.find("input[name='textNoteValue']");
            var coinValue = row.find("input[name='textCoinValue']");
            var noteCount = row.find("input[name='textNoteCount']");
            var coinCount = row.find("input[name='textCoinCount']");

            var value = noteValue.length == 0 ? coinValue : noteValue;
            var count = noteCount.length == 0 ? coinCount : noteCount;

            containerDropItems =
            {
                ContainerDropId: 0,
                DenominationType: noteValue.length > 0 ? "Notes" : "Coins",
                ValueInCents: $(this).val(),
                Count: isNumber(count.val()) == false ? 0 : count.val(),
                Value: isNumber(value.val()) == false ? 0 : value.val()
            };


            containerDropObject.ContainerDropItems.push(containerDropItems);

            if (containerDropItems.ValueInCents > 0 && (containerDropItems.Value > 0) || containerDropItems.Count > 0) {
                hasPassed = true;
            }
        });

        if (hasPassed == false) {
            showMessageBox("MessageWindow", "Alert Response", "", "Cant Copy " + dropDescription + " without denominations!");
            return false;
        }


        // ContainerDrop Properties
        var currentContainer = $(this).parents(".bagOrCanister");
        var containerDrops = currentContainer.find(".denomination_groups_parent");
        denomination_groups_clone.clone(true).appendTo(containerDrops);


        var lastDropInContainer = $(this).parents(".bagOrCanister").find(".drop_level").last();

        // Drop serial number
        var oldDropSerialNumber = formDrop.find("input[name='txtDropSerialNumber']").val();
        lastDropInContainer.find("input[name='txtDropSerialNumber']").val(oldDropSerialNumber);

        // Drop Amount
        var oldDropAmount = formDrop.find("input[name='txtDropAmount']").val();
        lastDropInContainer.find("input[name='txtDropAmount']").val(oldDropAmount);

        // Drop Reference
        var oldDropReference = formDrop.find("input[name='txtDropReference']").val();
        lastDropInContainer.find("input[name='txtDropReference']").val(oldDropReference);

        // always get the last appended container drop
        var lastDenominationDrop = currentContainer.find(".denominationgroup").last();

        lastDropInContainer.find("input[name='hdContainerDropId']").val(0);
        lastDropInContainer.find("input[name='hdDropStatusName']").val("Active");

        lastDenominationDrop.find(".clsnotes .clsline").remove();
        lastDenominationDrop.find(".clscoins .clsCoinsline").remove();

        // denominations
        var notesParent = lastDenominationDrop.find("table[CurrencyType='Notes']");
        var coinsParent = lastDenominationDrop.find("table[CurrencyType='Coins']");

        $.each(containerDropObject.ContainerDropItems, function (c, denomination) {
            var denominationCount = denomination.Count;
            var denominationType = denomination.DenominationType;
            var valueInCents = denomination.ValueInCents;
       
            if (denominationType == "Notes") {
                row_clone_notes.clone(true).appendTo(notesParent);
                // note values
                var notes = lastDenominationDrop.find(".clsnotes .clsline:last-child");
                var noteCount = notes.find('input[name="textNoteCount"]');
                var noteDeno = notes.find('select[name="dropDownListNote"]');
                var noteValue = notes.find('input[name="textNoteValue"]');
                setCashDepositProcessingElementsValues(noteCount, noteValue, noteDeno, valueInCents, denominationCount, false);
            }

            if (denominationType == "Coins") {
                row_clone_coins.clone(true).appendTo(coinsParent);
                // coin values
                var coins = lastDenominationDrop.find(".clscoins .clsCoinsline:last-child");
                var coinCount = coins.find('input[name="textCoinCount"]');
                var coinDeno = coins.find('select[name="dropDownListCoin"]');
                var coinValue = coins.find('input[name="textCoinValue"]');
                setCashDepositProcessingElementsValues(coinCount, coinValue, coinDeno, valueInCents, denominationCount, false);
            }
        });

        var hasNotesInDrop = notesParent.find("input[name='textNoteCount']");
        if (hasNotesInDrop.length == 0) {
            row_clone_notes.clone(true).appendTo(notesParent);
        }

        var hasCoinsInDrop = coinsParent.find("input[name='textCoinCount']");
        if (hasCoinsInDrop.length == 0) {
            row_clone_coins.clone(true).appendTo(coinsParent);
        }

        notesParent.find("a.remove").first().hide();
        coinsParent.find("a.remove").first().hide();

        // notes subtotals
        var denominationGroupCollectionNotes = lastDenominationDrop.find("table[CurrencyType='Notes'] input[name='textNoteValue']");
        var textSubTotal = lastDenominationDrop.find(".clsNotessubtotal input[name='textNotesSubtotal']");
        getsubTotal(denominationGroupCollectionNotes, textSubTotal);

        // coins subtotals
        var denominationGroupCollectionCoins = lastDenominationDrop.find("table[CurrencyType='Coins'] input[name='textCoinValue']");
        var textCoinSubTotal = lastDenominationDrop.find(".clsCoinssubtotal input[name='textCoinsSubtotal']");
        getsubTotal(denominationGroupCollectionCoins, textCoinSubTotal);



        var lblSerialNumber = lastDropInContainer.find(".lblSerialNumber");
        var txtDropSerialNumber = lastDropInContainer.find("input[name='txtDropSerialNumber']");

        // TotalInDrop
        var oldTotalInDrop = formDrop.find(".TotalInDrop").html();
        lastDropInContainer.find(".TotalInDrop").html(oldTotalInDrop);

        if ($('#ddlDepositType option:selected').text() == "Multi Deposit") {
            lblSerialNumber.remove();
            txtDropSerialNumber.remove();
        }
         
        var dropsOrDeposits = $(this).parents(".bagOrCanister").find(".denomination_groups_clone");
        var counter = 0;

        dropsOrDeposits.each(function () {
            counter++;
            $(this).find("input[name='hdDropNumber']").val(counter);
            $(this).find(".lblDropTitleAndNumber").html(dropDescription + " " + counter);
        });

        enumerateContainers();

        var buttonCopyDrop = $("button[name ='ButtonCopyDrop']");
        buttonCopyDrop.each(function () {
            $(this).html("Copy " + dropDescription);
        });
    });
    


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

    
    function isBetweenZeroAndNine(value) {
        return value >= "0" && value <= "9";
    }


    function isEmptyString(value) {
        return value == null || value.length == 0;
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


    function isDropSealNumbersDuplicated() {
        var serialNumbers = [];
        var dropBagSerialNumbers = $("#ContainersHolder").find("input[name='txtDropSerialNumber']");
        
        dropBagSerialNumbers.each(function () {
            if ($.trim($(this).val()) != "") {
                serialNumbers.push($.trim($(this).val()));
            }
        });

        var count = 0;
        for (var i = 0; i < serialNumbers.length; i++) {

            for (var j = i + 1; j < serialNumbers.length; j++) {
                if (serialNumbers[i] == serialNumbers[j]) {
                    count++;
                }
            }
        }
        return count > 0;
    }

    
    function isNumber(n) {
        return !isNaN(parseFloat(n)) && isFinite(n);
    }

});