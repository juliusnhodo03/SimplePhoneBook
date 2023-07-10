<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div class="demo-section" style="width:470px">
<label for="start">Start date:</label>
<%= Html.Kendo().DatePicker()
      .Name("start")
      .Value("10/10/2011")
      .Max("10/10/2012")
      .Events(e => e.Change("startChange"))
%>

<label for="end" style="margin-left:3em">End date:</label>
<%= Html.Kendo().DatePicker()
      .Name("end")
      .Value("10/10/2012")
      .Min("10/10/2011")
      .Events(e => e.Change("endChange"))
%>
</div>

<script>
    function startChange() {
        var endPicker = $("#end").data("kendoDatePicker"),
            startDate = this.value();

        if (startDate) {
            startDate = new Date(startDate);
            startDate.setDate(startDate.getDate() + 1);
            endPicker.min(startDate);
        }
    }

    function endChange() {
        var startPicker = $("#start").data("kendoDatePicker"),
            endDate = this.value();

        if (endDate) {
            endDate = new Date(endDate);
            endDate.setDate(endDate.getDate() - 1);
            startPicker.max(endDate);
        }
    }
</script>

<style scoped>
    #example .k-datepicker {
        vertical-align: middle;
    }

    #example h3 {
        clear: both;
    }

    #example .code-sample {
        width: 60%;
        float:left;
        margin-bottom: 20px;
    }

    #example .output {
        width: 24%;
        margin-left: 4%;
        float:left;
    }
</style>
</asp:Content>