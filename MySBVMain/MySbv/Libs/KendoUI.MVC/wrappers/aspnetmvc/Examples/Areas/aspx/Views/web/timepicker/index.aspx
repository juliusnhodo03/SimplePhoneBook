<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

<label for="timepicker">Select alarm time:</label>

<%= Html.Kendo().TimePicker()
        .Name("timepicker")
        .Value("10:00 AM")
%>

</asp:Content>