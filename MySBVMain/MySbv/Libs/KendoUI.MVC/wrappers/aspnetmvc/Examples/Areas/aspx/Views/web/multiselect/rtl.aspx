<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div class="demo-section k-rtl">
    <h2>Select Continents</h2>
    <%= Html.Kendo().MultiSelect()
            .Name("multiselect")
            .DataTextField("Text")
            .DataValueField("Value")
            .BindTo(new List<SelectListItem>()
            {
                new SelectListItem() {
                    Text = "Africa", Value = "1"
                },
                new SelectListItem() {
                    Text = "Europe", Value = "2"
                },
                new SelectListItem() {
                    Text = "Asia", Value = "3"
                },
                new SelectListItem() {
                    Text = "North America", Value = "4"
                },
                new SelectListItem() {
                    Text = "South America", Value = "5"
                },
                new SelectListItem() {
                    Text = "Antarctica", Value = "6"
                },
                new SelectListItem() {
                    Text = "Australia", Value = "7"
                }
            })
    %>

    <style scoped>
        .demo-section {
            width: 250px;
            margin: 35px auto 50px;
            padding: 30px;
        }
        .demo-section h2 {
            text-transform: uppercase;
            font-size: 1.2em;
            margin-bottom: 10px;
        }
    </style>
</div>
</asp:Content>
