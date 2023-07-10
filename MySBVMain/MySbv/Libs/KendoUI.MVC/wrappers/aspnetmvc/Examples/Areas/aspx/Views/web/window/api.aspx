<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div class="configuration k-widget k-header" style="z-index: 10000">
    <span class="configHead">API Functions</span>
    <ul class="options">
        <li>
            <button id="open" class="k-button">Open</button> / <button id="close" class="k-button">Close</button>
        </li>
        <li>
            <button id="refresh" class="k-button">Refresh</button>
        </li>
        <li>
            <button id="center" class="k-button">Center</button>
        </li>
        <li>
            <button id="pin" class="k-button">Pin</button>
        </li>
        <li>
            <button id="unpin" class="k-button">Unpin</button>
        </li>
    </ul>
</div>
    
<%= Html.Kendo().Window()
        .Name("window")
        .Width(630)
        .Height(315)
        .Title("Rams's Ten Principles of Good Design")
        .Actions(actions => actions.Pin().Refresh().Maximize().Close())
        .LoadContentFrom("ajaxcontent1", "window")
%>

<script>
    $(document).ready(function() {
        var window = $("#window");

        $("#open").click( function (e) {
            window.data("kendoWindow").open();
        });

        $("#close").click( function (e) {
            window.data("kendoWindow").close();
        });

        $("#refresh").click( function (e) {
            window.data("kendoWindow").refresh();
        });

        $("#center").click( function (e) {
            window.data("kendoWindow").center();
        });

        $("#pin").click( function (e) {
            window.data("kendoWindow").pin();
        });

        $("#unpin").click( function (e) {
            window.data("kendoWindow").unpin();
        });
    });
</script>

<style scoped>
    #example 
    {
        min-height: 360px;
    }
</style>
</asp:Content>