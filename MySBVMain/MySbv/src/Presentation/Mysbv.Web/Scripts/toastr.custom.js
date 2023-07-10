function ShowAlert(type, message, title) {

    toastr.options = {
        positionClass: 'toast-top-right',
        onclick: null,
        fadeIn: 300,
        fadeOut: 1000,
        timeOut: 5000,
        extendedTimeOut: 1000
    };
    toastr[type](message, title);
}