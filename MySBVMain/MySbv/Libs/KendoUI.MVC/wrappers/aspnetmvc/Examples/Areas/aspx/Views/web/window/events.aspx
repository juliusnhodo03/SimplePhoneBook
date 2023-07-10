<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<%= Html.Kendo().Window()
        .Name("window")
        .Width(630)
        .Height(315)
        .Title("Rams's Ten Principles of Good Design")
        .Actions(actions => actions.Refresh().Close())
        .LoadContentFrom("ajaxcontent1", "window")
        .Draggable()
        .Events(events => events
            .Open("onOpen")
            .Activate("onActivate")
            .Close("onClose")
            .Refresh("onRefresh")
            .Resize("onResize")
            .DragStart("onDragStart")
            .DragEnd("onDragEnd")
            .Deactivate("onDeactivate")
        )
%>

<span id="undo" class="k-button">Click here to open the window.</span>

<script>
    function onOpen(e) {
        kendoConsole.log("event :: open");
    }

    function onClose(e) {
        kendoConsole.log("event :: close");
    }

    function onActivate(e) {
        kendoConsole.log("event :: activate");
    }

    function onDeactivate(e) {
        kendoConsole.log("event :: deactivate");
    }

    function onRefresh(e) {
        kendoConsole.log("event :: refresh");
    }

    function onResize(e) {
        kendoConsole.log("event :: resize");
    }

    function onDragStart(e) {
        kendoConsole.log("event :: dragstart");
    }

    function onDragEnd(e) {
        kendoConsole.log("event :: dragend");
    }

    $(document).ready(function() {
        $("#undo").bind("click", function() {
            $("#window").data("kendoWindow").open();
        })
    });
</script>

<br/>
<div class="console"></div>

<style scoped>
    #example 
    {
        min-height: 400px;
    }
</style>
</asp:Content>