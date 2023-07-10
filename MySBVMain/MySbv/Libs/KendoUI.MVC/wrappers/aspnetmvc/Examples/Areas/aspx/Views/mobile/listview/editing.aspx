﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Mobile.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Kendo.Mvc.Examples.Models.ProductViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<%: Html.Kendo().MobileView()
        .Name("edit-listview")
        .Title("Products List")
        .Events(events => events.Init("listViewInit"))
        .Content(obj =>
            Html.Kendo().MobileListView<Kendo.Mvc.Examples.Models.ProductViewModel>()
                .Name("listview")
                .TemplateId("itemTemplate")
                .DataSource(dataSource => dataSource
                    .Read("Editing_Read", "ListView")
                    .Destroy("Editing_Destroy", "ListView")                   
                    .Model(model => model.Id(product => product.ProductID))
                )                
        )
%>

<script id="itemTemplate" type="text/x-kendo-template">
    <a> #=ProductName# </a>
    <%: Html.Kendo().MobileButton()
            .Text("Delete")
            .HtmlAttributes(new { @class = "delete" })
    %>    
</script>


<style scoped>
    .km-ios #edit-listview .km-navbar,
    .km-ios #edit-detailview .km-navbar {
        background: -webkit-gradient(linear, 50% 0, 50% 100%, color-stop(0, rgba(255, 255, 255, 0.5)), color-stop(0.06, rgba(255, 255, 255, 0.45)), color-stop(0.5, rgba(255, 255, 255, 0.2)), color-stop(0.5, rgba(255, 255, 255, 0.15)), color-stop(1, rgba(100, 100, 100, 0))), url(<%=Url.Content("~/content/shared/images/patterns/pattern1.png")%>);
        background: -moz-linear-gradient(center top , rgba(255, 255, 255, 0.5), rgba(255, 255, 255, 0.45) 6%, rgba(255, 255, 255, 0.2) 50%, rgba(255, 255, 255, 0.15) 50%, rgba(100, 100, 100, 0)), url(<%=Url.Content("~/content/shared/images/patterns/pattern1.png")%>);
    }
    .km-ios #edit-listview .km-navbar .km-button,
    .km-ios #edit-detailview .km-navbar .km-button {
        background-color: #000;
    }
    .km-ios #edit-detailview #done {
        background-color: Green;
    }
    .km-tablet .km-ios #edit-listview .km-view-title,
    .km-tablet .km-ios #edit-detailview .km-view-title {
        color: #fff;
        text-shadow: 0 -1px rgba(0,0,0,.5);
    }
    .km-ios #edit-listview .km-content,
    .km-ios #edit-detailview .km-content,
    .km-ios #edit-detailview .km-insetcontent,
    .km-ios #edit-listview li,
    .km-ios #edit-detailview li {
        background: #373737;
    }
    .km-ios #edit-listview li > a,
    .km-ios #edit-detailview li,
    .km-ios #edit-detailview input,
    .km-ios #edit-detailview li > a {
        text-decoration: none;
        color: #fff;
    }

    .km-ios #edit-detailview .km-listinset > li,
    .km-ios #edit-detailview .km-listgroupinset .km-list > li,
    .km-ios #edit-detailview .km-listinset > li:first-child,
    .km-ios #edit-detailview .km-listgroupinset .km-list >  li:first-child,
    .km-ios #edit-detailview .km-listinset > li:last-child,
    .km-ios #edit-detailview .km-listgroupinset .km-list >  li:last-child {
        box-shadow: none;
        -webkit-box-shadow: none;
        border-color: #565656;
    }
    .km-ios #edit-detailview .km-listinset > li:first-child,
    .km-ios #edit-detailview .km-listgroupinset .km-list > li:first-child {
        border-width: 1px;
    }
    .km-ios #edit-detailview .km-listinset > li,
    .km-ios #edit-detailview .km-listgroupinset .km-list > li {
        border-width: 0 1px 1px;
    }
    #edit-listview .delete {
        display:none;
        position: absolute;
        top: .15em;
        right: .5em;
        width: 60px;
        background-color: #bd2729;
        color: #fff;
    }
</style>

<script>

    var dataSource;

    function listViewInit(e) {
        dataSource = e.view.element.find("#listview").data("kendoMobileListView").dataSource;

        e.view.element.find("#listview")
            .kendoTouch({
                filter: ">li",
                enableSwipe: true,
                touchstart: touchstart,
                tap: navigate,
                swipe: swipe
            });
    }

    function navigate(e) {
        var itemUID = $(e.touch.currentTarget).data("uid");
        kendo.mobile.application.navigate("editdetails?productID=" + dataSource.getByUid(itemUID).ProductID);
    }

    function swipe(e) {
        var button = kendo.fx($(e.touch.currentTarget).find("[data-role=button]"));
        button.expand().duration(200).play();
    }

    function touchstart(e) {
        var target = $(e.touch.initialTouch),
            listview = $("#listview").data("kendoMobileListView"),
            model,
            button = $(e.touch.target).find("[data-role=button]:visible");

        if (target.closest("[data-role=button]")[0]) {
            model = dataSource.getByUid($(e.touch.target).attr("data-uid"));
            dataSource.remove(model);
            dataSource.sync();

            //prevent `swipe`
            this.events.cancel();
            e.event.stopPropagation();
        } else if (button[0]) {
            button.hide();

            //prevent `swipe`
            this.events.cancel();
        } else {
            listview.items().find("[data-role=button]:visible").hide();
        }
    }    
</script>

</asp:Content>
