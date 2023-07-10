var mySbvApp = angular.module("MySbvApp", [ "kendo.directives" ]);

mySbvApp.controller("FailedRequestController", function ($scope, $http) {
	// model
	$scope.model = model;

	$scope.dateOptions = {
		format: "yyyy-MM-dd HH:mm:ss"
	};

	$scope.getType = function (x) {
		return typeof x;
	};

	$scope.isDate = function (x) {
		return x instanceof Date;
	};
	
	$scope.getDisabled = function (ispending) {
		var cssClass = "";
		if (ispending) {
			cssClass = "TransactionDate";
		}
		else {
			cssClass = "TransactionDate white";
		}
		return cssClass;
	};

	$scope.showApprover = function() {
		if ($scope.model.IsApprover && $scope.model.IsPending) {
			return true;
		}
		return false;
	};

	$scope.updateRequest = function (model) {
		if ($scope.failed_transactions_form.$valid) {
			if ($scope.model.IsPending) {
				var title = "Failed to update";
				popUpMessage("MessageWindow", "Vault Transaction", "Cannot update a Request pending approval!", false, title, $scope.model.IndexUrl);
			}
			else {
				var text = "Are you sure you want to update Request?";
				$scope.isloading = true;
				$scope.isupdating = true;
				confirmSave(model, text, true, false, false);
				$scope.isloading = false;
				$scope.isupdating = false;
			}
		}
	};

	$scope.isBeneficiaryCodeRequired = function () {
		return $scope.model.IsGptRequest && $scope.model.IsCitRequest;
	};

	$scope.approveRequest = function (model) {
		if ($scope.failed_transactions_form.$valid) {
			if ($scope.model.SameAsCapturer == true) {
				var title = "Failed to update";
				popUpMessage("MessageWindow", "Vault Transaction", "You cannot approve your own changes!", false, title, $scope.model.IndexUrl);
			}
			else {
				var text = "Are you sure you want to approve Request?";
				$scope.isloading = true;
				$scope.isupdating = true;
				confirmSave(model, text, false, true, false);
				$scope.isloading = false;
				$scope.isupdating = false;
			}
		}
	};
	

	$scope.declineRequest = function (model) { 
		if ($scope.failed_transactions_form.$valid) {
			if ($scope.model.SameAsCapturer == true) {
				var title = "Failed to update";
				popUpMessage("MessageWindow", "Vault Transaction", "You cannot decline your own changes!", false, title, $scope.model.IndexUrl);
			}
			else {
				var text = "Are you sure you want to decline a Request?";
				$scope.isloading = true;
				$scope.isupdating = true;
				confirmSave(model, text, false, false, true);
				$scope.isloading = false;
				$scope.isupdating = false;
			}
		}
	};


	$scope.denominationResolver = function (value) {
		var amount = parseFloat(value);
		if (amount / 100 >= 1)
			return "R" + (amount / 100).toFixed(0);
		return value + "c";
	};


	$scope.Exit = function (indexUrl) {
		popUpMessage("ConfirmWindow", "Vault Transaction",
		"Are you sure you want to exit? you may lose all unsaved data.",
		true,
		null, indexUrl);
	};
	

	function update(model) {
		$http.post($scope.model.PostUrl, model).success(function (data) {
			$scope.messageResults = data.Message;
			var message = "";
			for (var index = 0; index < data.Message.length; index++) {
				message += "<li>" + data.Message[index].Error + "</li>";
			}
			var titlehead = data.ResponseCode == false ? "Failed to update Request:" : null;
			popUpMessage("MessageWindow", "Vault Transaction", message, data.ResponseCode, titlehead, $scope.model.IndexUrl);
			$scope.isloading = false;
			$scope.isupdating = false;
		}).error(function (data) {
			popUpMessage("MessageWindow", "Vault Transaction", data, false, "Failed to update Request:", null);
			$scope.isloading = false;
			$scope.isupdating = false;
		});
	}

	function approve(model) {
		$http.post($scope.model.ApproveUrl, model).success(function (data) {
			$scope.messageResults = data.Message;
			var message = "";
			for (var index = 0; index < data.Message.length; index++) {
				message += "<li>" + data.Message[index].Error + "</li>";
			}
			var titlehead = data.ResponseCode == false ? "Failed to approve Request:" : null;
			popUpMessage("MessageWindow", "Vault Transaction", message, data.ResponseCode, titlehead, $scope.model.IndexUrl);
			$scope.isloading = false;
			$scope.isupdating = false;
		}).error(function (data) {
			popUpMessage("MessageWindow", "Vault Transaction", data, false, null, null);
			$scope.isloading = false;
			$scope.isupdating = false;
		});
	}

	function decline(model) {
		$http.post($scope.model.DeclineUrl, model).success(function (data) {
			$scope.messageResults = data.Message;
			var message = "";
			for (var index = 0; index < data.Message.length; index++) {
				message += "<li>" + data.Message[index].Error + "</li>";
			}
			var titlehead = data.ResponseCode == false ? "Failed to decline Request:" : null;
			popUpMessage("MessageWindow", "Vault Transaction", message, data.ResponseCode, titlehead, $scope.model.IndexUrl);
			$scope.isloading = false;
			$scope.isupdating = false;
		}).error(function (data) {
			popUpMessage("MessageWindow", "Vault Transaction", data, false, null, null);
			$scope.isloading = false;
			$scope.isupdating = false;
		});
	}

	function confirmSave(model, message, isUpdate, isApprove, isDecline) {	
		var kendoWindow = $("<div />").kendoWindow({
			title: "Vault Transaction",
			resizable: false,
			modal: true,
			width: 430,
			height: 160
		});
		kendoWindow.data("kendoWindow")
			.content($("#ConfirmWindow").html())
			.center().open();

		$(".confirmation-message").html(message);
		$(".headerTitle").html("");

		kendoWindow
			.find(".no-cancel,.confirm-cancel")
			.click(function () {
				if ($(this).hasClass("confirm-cancel")) {
					if (isUpdate) {
						update(model);
					}
					else if (isApprove) {
						approve(model);
					}
					else if (isDecline) {
						decline(model);
					}
				};
				kendoWindow.data("kendoWindow").close();
			})
			.end();
	}

});


// pop up message box
function popUpMessage(windowId, header, message, isExit, title, indexUrl) {
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

	$(".confirmation-message").html(message.replace('"', '').replace('"', ''));
	$(".headerTitle").html(title);

	kendoWindow
		.find(".no-cancel,.confirm-cancel")
		.click(function () {
			if ($(this).hasClass("confirm-cancel")) {
				if (isExit == true) {
					window.location.href = indexUrl;
				}
			};
			kendoWindow.data("kendoWindow").close();
		})
		.end();
}



