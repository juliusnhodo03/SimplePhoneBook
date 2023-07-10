﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<%:Html.Kendo().Menu()
    .Name("verticalMenu")
    .HtmlAttributes(new { style = "width:140px;margin-bottom:30px" })
    .Orientation(MenuOrientation.Vertical)
    .Items(items =>
    {
        items.Add()
            .Text("First Item")
            .Items(children =>
            {
                children.Add().Text("Sub Item 1");
                children.Add().Text("Sub Item 2");
                children.Add().Text("Sub Item 3");
                children.Add().Text("Sub Item 4");
                children.Add().Text("Sub Item 5");
            });

        items.Add()
             .Text("Second Item")
             .Items(children =>
             {
                 children.Add().Text("Sub Item 1");
                 children.Add().Text("Sub Item 2");
                 children.Add().Text("Sub Item 3");
                 children.Add().Text("Sub Item 4");
                 children.Add().Text("Sub Item 5");
             });

        items.Add()
            .Text("Third Item")
            .Items(children =>
            {
                children.Add().Text("Sub Item 1");
                children.Add().Text("Sub Item 2");
                children.Add().Text("Sub Item 3");
                children.Add().Text("Sub Item 4");
                children.Add().Text("Sub Item 5");
            });

        items.Add()
             .Text("Fourth Item")
             .Items(children =>
             {
                 children.Add().Text("Sub Item 1");
                 children.Add().Text("Sub Item 2");
                 children.Add().Text("Sub Item 3");
                 children.Add().Text("Sub Item 4");
                 children.Add().Text("Sub Item 5");
             });

        items.Add()
            .Text("Fifth Item")
            .Items(children =>
            {
                children.Add().Text("Sub Item 1");
                children.Add().Text("Sub Item 2");
                children.Add().Text("Sub Item 3");
                children.Add().Text("Sub Item 4");
                children.Add().Text("Sub Item 5");
            });
    })
%>

<%:Html.Kendo().Menu()
    .Name("horizontalMenu")
    .Items(items =>
    {
        items.Add()
            .Text("First Item")
            .Items(children =>
            {
                children.Add().Text("Sub Item 1");
                children.Add().Text("Sub Item 2");
                children.Add().Text("Sub Item 3");
                children.Add().Text("Sub Item 4");
                children.Add().Text("Sub Item 5");
            });

        items.Add()
             .Text("Second Item")
             .Items(children =>
             {
                 children.Add().Text("Sub Item 1");
                 children.Add().Text("Sub Item 2");
                 children.Add().Text("Sub Item 3");
                 children.Add().Text("Sub Item 4");
                 children.Add().Text("Sub Item 5");
             });

        items.Add()
            .Text("Third Item")
            .Items(children =>
            {
                children.Add().Text("Sub Item 1");
                children.Add().Text("Sub Item 2");
                children.Add().Text("Sub Item 3");
                children.Add().Text("Sub Item 4");
                children.Add().Text("Sub Item 5");
            });

        items.Add()
             .Text("Fourth Item")
             .Items(children =>
             {
                 children.Add().Text("Sub Item 1");
                 children.Add().Text("Sub Item 2");
                 children.Add().Text("Sub Item 3");
                 children.Add().Text("Sub Item 4");
                 children.Add().Text("Sub Item 5");
             });

        items.Add()
            .Text("Fifth Item")
            .Items(children =>
            {
                children.Add().Text("Sub Item 1");
                children.Add().Text("Sub Item 2");
                children.Add().Text("Sub Item 3");
                children.Add().Text("Sub Item 4");
                children.Add().Text("Sub Item 5");
            });
    })
%>

<script>
    $(document.body).keydown(function (e) {
        if (e.altKey && e.keyCode == 87) {
            $("#verticalMenu").focus();
        } else if (e.altKey && e.keyCode == 81) {
            $("#horizontalMenu").focus();
        }
    });
</script>

<ul class="keyboard-legend" style="padding-top: 25px">
    <li>
        <span class="button-preview">
            <span class="key-button leftAlign">Alt</span>
            +
            <span class="key-button">W</span>
        </span>
        <span class="button-descr">
            focuses vertical menu (clicking on it or tabbing also work)
        </span>
    </li>
    <li>
        <span class="button-preview">
            <span class="key-button leftAlign">Alt</span>
            +
            <span class="key-button">Q</span>
        </span>
        <span class="button-descr">
            focuses the horizontal menu (clicking on it or tabbing also work)
        </span>
    </li>
</ul>

<h4>Supported keys and user actions</h4>
<ul class="keyboard-legend">
    <li>
        <span class="button-preview">
            <span class="key-button">Right</span>
        </span>
        <span class="button-descr">
            Goes to the next item or opens an item group
        </span>
    </li>
    <li>
        <span class="button-preview">
            <span class="key-button">Left</span>
        </span>
        <span class="button-descr">
            Goes to the previous item or closes an item group
        </span>
    </li>
    <li>
        <span class="button-preview">
            <span class="key-button">Down</span>
        </span>
        <span class="button-descr">
            Opens an item group or goes to the next item in a group
        </span>
    </li>
    <li>
        <span class="button-preview">
            <span class="key-button">Up</span>
        </span>
        <span class="button-descr">
            Goes to the previous item in a group
        </span>
    </li>
    <li>
        <span class="button-preview">
            <span class="key-button">Enter</span>
        </span>
        <span class="button-descr">
            Select or navigate item (same as click)
        </span>
    </li>
    <li>
        <span class="button-preview">
            <span class="key-button">Esc</span>
        </span>
        <span class="button-descr">
            closes the innermost open group
        </span>
    </li>
    <li>
        <span class="button-preview">
            <span class="key-button">Tab</span>
        </span>
        <span class="button-descr">
            tabs away from the Menu on the next focusable page element
        </span>
    </li>
    <li>
        <span class="button-preview">
            <span class="key-button leftAlign">Shift</span>
            +
            <span class="key-button">Tab</span>
        </span>
        <span class="button-descr">
            tabs away from the Menu on the previous focusable page element
        </span>
    </li>
</ul>

</asp:Content>