<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="climateCtrl">
    <%= Html.Kendo().Slider()
            .Name("slider")
            .Min(0)
            .Max(30)
            .SmallStep(1)
            .LargeStep(10)
            .Value(18)
            .Events(events => events
                .Slide("sliderSlide")
                .Change("sliderChange"))
            .HtmlAttributes(new { @class = "temperature" })
      %>

     <%= Html.Kendo().RangeSlider()
             .Name("rangeslider")
             .Min(0)
             .Max(10)
             .SmallStep(1)
             .LargeStep(10)
             .Events(events => events
                 .Slide("rangeSliderSlide")
                 .Change("rangeSliderChange"))
             .HtmlAttributes(new { @class = "humidity" })
     %>
</div>

<script>
    function sliderSlide(e) {
        kendoConsole.log("Slide :: new slide value is: " + e.value);
    }

    function sliderChange(e) {
        kendoConsole.log("Change :: new value is: " + e.value);
    }

    function rangeSliderSlide(e) {
        kendoConsole.log("Slide :: new slide values are: " + e.values.toString().replace(",", " - "));
    }

    function rangeSliderChange(e) {
        kendoConsole.log("Change :: new values are: " + e.values.toString().replace(",", " - "));
    }
</script>

<style>
    #climateCtrl {
        width: 245px;
        height: 167px;
        margin: 30px auto;
        padding: 102px 0 0 156px;
        background: url(<%= Url.Content("~/Content/web/slider/climateController.png") %>) transparent no-repeat 0 0;
    }
    .humidity {
        margin: 67px 0 0 15px;
        width: 170px;
    }
</style>

<div class="console"></div>
</asp:Content>