<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="demo-section">
    <dl>
        <dt><label for="palette-picker">ColorPicker (palette):</label></dt>
        <dd>
            <%= Html.Kendo().ColorPicker()
                  .Name("palette-picker")
                  .Value("#cc2222")
                  .Palette(ColorPickerPalette.Basic)
                  .Events(events => events
                      .Select("pickerSelect")
                      .Change("pickerChange")
                      .Open("pickerOpen")
                      .Close("pickerClose")
                  )
            %>
        </dd>

        <dt><label for="hsv-picker">ColorPicker (HSV):</label></dt>
        <dd>
            <%= Html.Kendo().ColorPicker()
                  .Name("hsv-picker")
                  .Value("#22cc22")
                  .Events(events => events
                      .Select("pickerSelect")
                      .Change("pickerChange")
                      .Open("pickerOpen")
                      .Close("pickerClose")
                  )
            %>
        </dd>

        <dt>ColorPalette:</dt>
        <dd>
            <%= Html.Kendo().ColorPalette()
                  .Name("palette")
                  .Events(events => events
                      .Change("paletteChange")
                  )
            %>
        </dd>

        <dt>FlatColorPicker:</dt>
        <dd>
            <%= Html.Kendo().FlatColorPicker()
                  .Name("flatcolorpicker")
                  .Events(events => events
                      .Change("flatChange")
                  )
            %>
        </dd>
    </dl>
</div>

<script>

    function pickerSelect(e) {
        kendoConsole.log("Select in picker #" + this.element.attr("id") + " :: " + e.value);
    }

    function pickerChange(e) {
        kendoConsole.log("Change in picker #" + this.element.attr("id") + " :: " + e.value);
    }

    function pickerOpen(e) {
        kendoConsole.log("Open in picker #" + this.element.attr("id"));
    }

    function pickerClose(e) {
        kendoConsole.log("Close in picker #" + this.element.attr("id"));
    }

    function paletteChange(e) {
        kendoConsole.log("Change in color palette :: " + e.value);
    }

    function flatChange(e) {
        kendoConsole.log("Change in flat color picker :: " + e.value);
    }
</script>

<div class="demo-section">
    <h3 class="title">Console log</h3>
    <div class="console"></div>
</div>

<style scoped>
    .demo-section {
        width: 500px;
    }

    .demo-section dl {
        display: inline-block;
    }

    .demo-section dl:after {
        content: " ";
        clear: both;
        font: 0/0;
    }

    .demo-section dt,
    .demo-section dd {
        float: left;
        margin: 0;
        padding: 0 0 1em;
    }

    .demo-section dt {
        width: 45%;
        padding-top: .3em;
        padding-right: 5%;
        clear: left;
        text-align: right;
    }

    .demo-section dd {
        width: 50%;
    }
</style>
</asp:Content>