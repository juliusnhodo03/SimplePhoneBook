$(document).ready(function() {

    $('.discrepancy').change(function() {
        var loginScreen = "<hr/><br>" +
            "<table style='width:100%; padding:30px;' cellpadding='3'>" +
            "<tr>" +
            "<td style='width:20px;'></td>" +
            "<td>Username</td>" +
            "<td><input id='txtSupervisorUsername' class='dropdown' type='text' /></td>" +
            "</tr>" +
            "<tr>" +
            "<td></td>" +
            "<td>Username</td>" +
            "<td><input id='txtSupervisorPassword' class='dropdown' type='password' /></td>" +
            "</tr>" +
            "</table>";

        var discrepancyCheckbox = $(this);

        var elementsParent = $(this).closest(".denomination_groups_clone");
        var textInputs = elementsParent.find('input[type="text"]:not(.SubTotals)');
        var selectDropdowns = elementsParent.find('select');

        if ($(this).is(":checked")) {

            elementsParent.addClass('selectedBg');

            $.confirm({
                'title': 'supervisor login',
                'message': loginScreen,
                'buttons': {
                    'Login': {
                        'class': 'blue',
                        'action': function() {

                            var user = { username: $("#txtSupervisorUsername").val(), password: $("#txtSupervisorPassword").val() };

                            //call server method
                            $.ajax(
                                {
                                    type: "POST",
                                    url: "/AjaxServerRequests/AuthenticateSupervisor.aspx/LoginSupervisor",
                                    data: JSON.stringify(user),
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function(result) { //call successfull
                                        if (result.d == "") {
                                            $('#hdDiscrepeancyIndicator').attr('value', true);
                                            $('#hdSupervisorId').attr('value', user.username);                                                                                        
                                            
                                            checkUncheck(textInputs, selectDropdowns, false);
                                            $.confirm.hide();
                                        } else alert(result.d);
                                    },
                                    error: function(result) {
                                        alert(result.d);
                                    }
                                }); // End $.ajax method

                        }
                    },
                    'Cancel': {
                        'class': 'gray',
                        'action': function() {                            
                             checkUncheck(textInputs, selectDropdowns, true);
                            discrepancyCheckbox.attr('checked', false);
                            $.confirm.hide();
                            elementsParent.removeClass('selectedBg');
                        } // Nothing to do in this case. You can as well omit the action property.
                    }
                }
            });
        } else {
            checkUncheck(textInputs, selectDropdowns, true);     
            elementsParent.removeClass('selectedBg');
        }
    }); //end discrepancy click event


    function checkUncheck(textboxes, dropdownlists, state) {
        dropdownlists.each(function() {
            $(this).attr('disabled', state);
        });

        textboxes.each(function() {
            $(this).attr('disabled', state);
        });
    }


    //
    $("#ContainersHolder input[name='checkBoxApproved']").on('click', function () {
        var discrepancyCheckbox = $(this).parents(".clsApprovalParent").find("input[name='checkBoxDiscrepancy']");
        if ($(this).is(':checked')) {
            discrepancyCheckbox.attr('checked', false);

            var elementsParent = $(this).closest(".denomination_groups_clone");
            var textInputs = elementsParent.find('input[type="text"]:not(.SubTotals)');
            var selectDropdowns = elementsParent.find('select');
            checkUncheck(textInputs, selectDropdowns, true);
            elementsParent.removeClass('selectedBg');
        }
    });


    $("#ContainersHolder input[name='checkBoxDiscrepancy']").on('click', function () {
        var approvedCheckbox = $(this).parents(".clsApprovalParent").find("input[name='checkBoxApproved']");
        if ($(this).is(':checked')) {
            approvedCheckbox.attr('checked', false);
        }
    });
    
});