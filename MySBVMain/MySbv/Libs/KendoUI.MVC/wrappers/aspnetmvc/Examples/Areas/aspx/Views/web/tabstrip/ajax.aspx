<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="example" class="k-content">
    <div class="wrapper">
        <%= Html.Kendo().TabStrip()
              .Name("tabstrip")
              .Items(tabstrip =>
              {
                  tabstrip.Add().Text("Dimensions & Weights")
                      .Selected(true)
                      .LoadContentFrom(Url.Content("~/Content/web/tabstrip/ajax/ajaxContent1.html"));

                  tabstrip.Add().Text("Engine")
                      .LoadContentFrom(Url.Content("~/Content/web/tabstrip/ajax/ajaxContent2.html"));

                  tabstrip.Add().Text("Chassis")
                      .LoadContentFrom(Url.Content("~/Content/web/tabstrip/ajax/ajaxContent3.html"));
              })
        %>
    </div>
</div>

<style scoped="scoped">
    .wrapper {
        width: 270px;
        height: 455px;
        margin: 20px auto;
        padding: 20px 0 0 390px;
        background: url('<%=Url.Content("~/Content/web/tabstrip/bmw.png") %>') no-repeat 40px 60px transparent;
    }
    #tabstrip {
        width: 320px;
        float: right;
        margin-bottom: 20px;
    }
    .specification {
        max-width: 670px;
        margin: 10px 0;
        padding: 0;
        height:360px;
        overflow:auto;
    }
    .specification dt, dd {
        width: 140px;
        float: left;
        margin: 0;
        padding: 5px 0 7px 0;
        border-top: 1px solid rgba(0,0,0,0.3);
    }
    .specification dt {
        clear: left;
        width: 120px;
        margin-right: 7px;
        padding-right: 0;
        text-align: right;
        opacity: 0.7;
    }
    .specification:after, .wrapper:after {
        content: "";
        display: block;
        clear: both;
        height: 0;
        visibility: hidden;
    }
</style>
</asp:Content>