var classNames = {
    categories: { active: 'minus', inActive: 'plus' }
};


$(document).ready(function () {
    $(".collapse_expand_content").hide();
    $(".container_heading").removeClass(classNames.categories.active).addClass(classNames.categories.inActive);

    //toggle the componenet with class msg_body
    $(".container_heading").on("click", function ()
    {
        var indicator = $(this)[0];
        if (indicator.className.indexOf('minus') >= 0)
        {
            $(this).removeClass(classNames.categories.active).addClass(classNames.categories.inActive);
        }
        else 
        {
            $(this).removeClass(classNames.categories.inActive).addClass(classNames.categories.active);
        }
        $(this).next(".collapse_expand_content").slideToggle();
        return false;
    });
});