var mySbvApp = angular.module("MySbvApp", ["kendo.directives"]);

mySbvApp.directive('showErrors', function() {
    return {
        restrict: 'A',
        require: '^form',
        link: function (scope, el, attrs, formCtrl) {
            var isInvalid = false;
            // find the text box element, which has the 'name' attribute
            var inputEl = el[0].querySelector("[name]");

            // convert the native text box element to an angular element
            var inputNgEl = angular.element(inputEl);
            // get the name on the text box so we know the property to check
            // on the form controller
            var inputName = inputNgEl.attr('name');

            // only apply the has-error class after the user leaves the text box
            inputNgEl.bind('blur', function () {
                // fisrt clear red lines
                $(this).removeClass("validationError");

                if (formCtrl[inputName].$invalid || $(this).val() == 0) {
                    // fisrt clear red lines
                    $(this).addClass("validationError");
                    isInvalid = true;
                }

                el.toggleClass('has-error', formCtrl[inputName].$invalid || isInvalid);
            });

            // remove the has-error class on focus
            inputNgEl.bind('focus', function () {
                $(this).removeClass("validationError");
            });

            // remove the has-error class on focus
            inputNgEl.bind('change', function () {
                $(this).removeClass("validationError");
            });

            // inside the directive's link function from the previous example
            scope.$on('show-errors-check-validity', function () {
                el.toggleClass('has-error', formCtrl[inputName].$invalid);
            });
            return isValid || formCtrl[inputName].$invalid;
        }
    };
});

mySbvApp.controller("VaultPaymentController", function ($scope, $http) {

    $scope.Url = {
        siteUrl: siteUrl,
        deviceUrl: deviceUrl,
        amountUrl: amountUrl,
        paymentUrl: paymentUrl,
        receiptUrl: receiptUrl,
        reLoadUrl: reLoadUrl
    };

    // regular expression to positive numbers.
    // this is passed to a pattern on html control.
    $scope.positiveNumberRegexValidator = function () {
        var dateRegex = /^[1-9]\d*\.?[0-9]*$/;
        var regex = new RegExp(dateRegex);
        return regex;
    };

	// model
    $scope.model = model;
    $scope.model.Payment.RemainingAmount = '0.00';
    $scope.model.Payment.AmountToBePaid = (0).toFixed(2);

    // this object is used to refresh the page;
    $scope.resetObject = angular.copy($scope.model);

    $scope.site = {
        IsDefaultReference: null,
        DepositReference : null
    };

    // clean up beneficiary details
    $scope.cleanupBeneficiaryDetails = function() {
        $scope.model.Payment.AccountId = null;
        $scope.model.Payment.BankName = null;
        $scope.model.Payment.BeneficiaryName = null;
        $scope.model.Payment.BeneficiaryCode = null;
    }


    $scope.calculateRemaingAmount = function () {
        var regex = $scope.positiveNumberRegexValidator();
        var isNumber = regex.test($scope.model.Payment.AmountToBePaid);
        if (isNumber == false) {
            $scope.model.Payment.AmountToBePaid = "0.00";
        }
        $scope.model.Payment.RemainingAmount = parseFloat($scope.model.Payment.AvailableFunds - $scope.model.Payment.AmountToBePaid).toFixed(2);
        if (parseFloat($scope.model.Payment.RemainingAmount) < 0) {
            $("#RemainingAmount").addClass("validationError");
        }
        else {
            $("#RemainingAmount").removeClass("validationError");
        }
    };


    // beneficiary
    $scope.getBeneficiaryDetails = function (e) {
        var accountId = e.sender.value();
        $scope.cleanupBeneficiaryDetails();
        for (var index = 0; index < $scope.model.Accounts.length; index++) {
            if ($scope.model.Accounts[index].AccountId == accountId) {
                var account = $scope.model.Accounts[index];
                $scope.model.Payment.AccountId = accountId;
                $scope.model.Payment.BankName = account.BankName;
                $scope.model.Payment.BeneficiaryName = account.AccountHolderName;
                $scope.model.Payment.BeneficiaryCode = account.BeneficiaryCode;
                break;
            }
        }
    };


    $scope.afterDefaultChanged = function (isDefaultReference) {
        if (!isDefaultReference)
        {
            $scope.model.Payment.IsDefaultReference = $scope.site.IsDefaultReference;
            $scope.model.Payment.PaymentReference = $scope.site.DepositReference;
        }
    };


    // get sites
    $scope.getSites = function (e, url) {
        var merchantId = e.sender.value();
        $scope.cleanupBeneficiaryDetails();
        $http.get(url, { params: { merchantId: merchantId } }).success(function (data) {
            $scope.model.Sites = data.Sites;
            $scope.model.Payment.CitCode = "";
            $scope.model.Devices = data.Devices;
        }).error(function () {});
    };


    // get devices on site
    $scope.getDevices = function (e, url) {
        var siteId = e.sender.value();
        $scope.model.Payment.AvailableFunds = '0.00';
        $scope.cleanupBeneficiaryDetails();
        $http.get(url, { params: { siteId: siteId } }).success(function (data) {
            $scope.model.Devices = data.Devices;
            $scope.model.Payment.CitCode = data.SiteData.CitCode;
            $scope.model.Payment.SiteName = data.SiteData.Name;
            $scope.model.Accounts = data.SiteData.Accounts;
            $scope.site = data.SiteData;
            $scope.model.Payment.IsDefaultReference = $scope.site.IsDefaultReference;
            $scope.model.Payment.PaymentReference = $scope.site.DepositReference;
            $scope.model.Payment.AvailableFunds = parseFloat(0).toFixed(2);
        }).error(function (data) { });
    };


    // get amount in device
    $scope.getAmountInDevice = function (e, url) {
        $scope.model.Payment.AvailableFunds = '0.00';
        var deviceId = e.sender.value();
        $http.get(url, { params: { deviceId: deviceId } }).success(function (data) {
            $scope.model.Payment.AvailableFunds = parseFloat(data.Amount).toFixed(2);
            $scope.model.Payment.BagSerialNumber = data.BagSerialNumber;
        }).error(function () {});
    };
    

    // validates form controls on submit button click.
    $scope.hasError = function () {
        $scope.errors = [];

        // get all elements with a name atribute
        var elementList = document.querySelectorAll("[name]");
        var isInvalid = false;
        for (var index = 0; index < elementList.length; index++) {
            // find the text box element, which has the 'name' attribute
            var inputEl = elementList[index];

            // convert the native text box element to an angular element
            var inputNgEl = angular.element(inputEl);

            // set the focus to enable 'show-errors' directive
            inputNgEl.focus();
            inputNgEl.blur();

            var widget = inputNgEl.parents("span.k-timepicker, span.k-datepicker");

            // is invalid if it has 'validationError' class
            if (inputNgEl.hasClass("validationError") || widget.hasClass("validationError")) {
                isInvalid = true;
                var error = inputNgEl.attr('name');

                switch (error) {
                    case 'AmountToBePaid':
                        $scope.errors.push('Amount To Be Paid');
                        break;
                    case 'AccountNumber':
                        $scope.errors.push('Account Number');
                        break;
                    case 'Device':
                        $scope.errors.push('Device');
                        break;
                    case 'Site':
                        $scope.errors.push('Site');
                        break;
                    case 'Merchant':
                        $scope.errors.push('Merchant');
                        break;
                    case 'PaymentReference':
                        $scope.errors.push('Payment Reference');
                        break;
                    default: break;
                }
            }
        }
        var beneficiaryEmail = $("#BeneficiaryEmail");
        var valid = $scope.emailValidator().test(beneficiaryEmail.val()) || $.trim(beneficiaryEmail.val())  == "";
        
        if (valid == false) {
            isInvalid = true;
            $scope.errors.push("Beneficiary Email (wrong Email Format)");
        }

        return isInvalid;
    }


    $scope.confirmPayment = function () {
        $scope.accountNumberText = $scope.accountNumberCollection($scope.model.Payment.AccountId);
        $scope.model.Payment.AmountToBePaid = parseFloat($scope.model.Payment.AmountToBePaid).toFixed(2);
        var isValid = $scope.hasError();

        if (!isValid) {            
            var data = {
                IsConfirmation: true,
                IsValidation: false,
                HasAmountExceeded:false,
                TitleBar: 'Payment Request Confirmation',
                IsSuccess: false,
                ErrorEncountered: false,
                IsError: false,
                IsWarning: false,
                Message: '<b>You are about to make a payment to the following:</b>',
            };

            if (parseFloat($scope.model.Payment.RemainingAmount) < 0) {
                data.TitleBar = "Field Validation";
                data.Message = 'The amount to be paid cannot exceed the Available funds!';
                data.IsValidation = false;
                data.HasAmountExceeded = true;
                data.IsConfirmation = false;
                $scope.openWindow(data);
            }
            $scope.openWindow(data);
        }
        else {
            var info = {
                IsConfirmation: false,
                IsValidation: true,
                HasAmountExceeded: false,
                TitleBar: 'Field Validation',
                IsSuccess: false,
                ErrorEncountered: false,
                IsError: false,
                IsWarning: false,
                Message: "Please fill in required fields:",
            };

            $scope.openWindow(info);
        }
    };


    $scope.emailValidator = function () {
        var emailRegex = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        var regex = new RegExp(emailRegex);
        return regex;
    };


    // get account number   
    $scope.accountNumberCollection = function (accountId) {
        for (var index = 0; index < $scope.model.Accounts.length; index++) {
            if ($scope.model.Accounts[index].AccountId == accountId) {
                return $scope.model.Accounts[index].AccountNumber;
            }
        }
    };
       
    

    $scope.requestPayment = function (model, url) {
        var data = {
            IsConfirmation: false,
            IsValidation: false,
            HasAmountExceeded: false,
            TitleBar: 'Payment Confirmation',
            ErrorEncountered: false,
            IsSuccess: true,
            IsError: false,
            IsWarning: false,
            Message: ''
        };
        $scope.confirmWindow.center().close();
        $scope.loadingWindow.center().open();

        $http.post(url, model).success(function (result) {
            $scope.PaymentResponse = result;
            $scope.loadingWindow.center().close();
            data.Message = result.ResponseMessage;

            data.ErrorEncountered = result.ErrorEncountered;

            if (result.ErrorEncountered == false) {
                data.IsSuccess = true;
            } else {
                data.TitleBar = "Payment Validation";
            }

            $scope.model.Payment.AvailableFunds = parseFloat(result.AvailableFunds).toFixed(2);
            $scope.openWindow(data);
        }).error(function (result) {
            $scope.loadingWindow.center().close();
        });
    };


    $scope.printReport = function (url) {
        var paymentSlipUrl = url + $scope.PaymentResponse.VaultPartialPaymentId;
        window.open(paymentSlipUrl, '_blank');
        window.location.href = reLoadUrl;
    };



    $scope.closePopup = function () {
        $scope.responseWindow.center().close();
        window.location.href = reLoadUrl;
    };


    $scope.openWindow = function (result) {
        if (result.HasAmountExceeded || result.ErrorEncountered) {
            $scope.validationWindow.center().open();
        }
        else if (result.IsValidation) {
                $scope.errorsWindow.center().open();
        }
        else if (result.IsConfirmation) {
            $scope.confirmWindow.center().open();
        }
        else {
            var paragraph = $(".k-window").find(".row p");

            if (result.IsSuccess) {
                paragraph.removeClass("warning");
                paragraph.removeClass("error");
                paragraph.addClass("success");
            }
            else if (result.IsError) {
                paragraph.removeClass("warning");
                paragraph.removeClass("success");
                paragraph.addClass("error");
            }
            else if (result.IsWarning) {
                paragraph.removeClass("error");
                paragraph.removeClass("success");
                paragraph.addClass("warning");
            }
            $scope.responseWindow.center().open();
        }
        $(".k-window-title").html(result.TitleBar);
        $(".confirmation-message").html(result.Message);
    };


    $scope.clearForm = function (form) {
        
        // get all elements with a name atribute
        var elementList = document.querySelectorAll("[name]");
        for (var index = 0; index < elementList.length; index++) {
            // find the text box element, which has the 'name' attribute
            var inputEl = elementList[index];

            // convert the native text box element to an angular element
            var inputNgEl = angular.element(inputEl);
            inputNgEl.removeClass("validationError");
        }

        $("#RemainingAmount").removeClass("validationError");
        $scope.model = angular.copy($scope.resetObject);

        form.$setPristine();
        form.$setUntouched();
    };

});