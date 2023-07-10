


function AjaxCall(url, successCallback, errorCallback, data, type) {

    $.ajax({
        type:type,
        url: url,
        data: data,
        success: successCallback,

        error:function(jqXhr, exception) {
            if (jqXhr.status === 0) {
                ShowAlert('error', 'Not connect.\n Verify Network.', 'AjaxCall Error');
            } else if (jqXhr.status == 404) {
                ShowAlert('error', 'Requested page not found. [404]', 'AjaxCall Error');
            } else if (jqXhr.status == 500) {
                ShowAlert('error', 'Internal Server Error [500].', 'AjaxCall Error');
            } else if (exception === 'parsererror') {
                ShowAlert('error','Requested JSON parse failed.','AjaxCall Error');
            } else if (exception === 'timeout') {
                ShowAlert('error', 'Time out error.', 'AjaxCall Error');
            } else if (exception === 'abort') {
                ShowAlert('error', 'Ajax request aborted.', 'AjaxCall Error');
            } else {
                ShowAlert('error','Uncaught Error.\n' + jqXhr.responseText,'AjaxCall Error');
            }
            errorCallback();
        }
        
    });



}