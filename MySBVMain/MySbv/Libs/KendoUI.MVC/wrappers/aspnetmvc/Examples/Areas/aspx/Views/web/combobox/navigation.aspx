<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="demo-section">
    <h3>T-shirt Size</h3>
    <%= Html.Kendo().ComboBox()
          .Name("size")
          .HtmlAttributes(new { accesskey="w" })
          .Placeholder("Select size...")
          .BindTo(new List<string>() {
              "X-Small",
              "Small",
              "Medium",
              "Large",
              "X-Large",
              "2X-Large"
          })
          .SelectedIndex(3)
          .Suggest(true)
    %>
</div>

<ul class="keyboard-legend">
    <li>
        <span class="button-preview">
            <span class="key-button leftAlign wider"><a target="_blank" href="http://en.wikipedia.org/wiki/Access_key">Access key</a></span>
            +
            <span class="key-button">w</span>
        </span>
        <span class="button-descr">
            focuses the widget
        </span>
    </li>
</ul>

<ul class="keyboard-legend">
    <li>
        <span class="button-preview">
            <span class="key-button wide leftAlign">up arrow</span>
        </span>
        <span class="button-descr">
            highlights previous item
        </span>
    </li>
    <li>
        <span class="button-preview">
            <span class="key-button wider leftAlign">down arrow</span>
        </span>
        <span class="button-descr">
            highlights next item
        </span>
    </li>
    <li>
        <span class="button-preview">
            <span class="key-button wider rightAlign">enter</span>
        </span>
        <span class="button-descr">
            selects highlighted item
        </span>
    </li>
    <li>
        <span class="button-preview">
            <span class="key-button">esc</span>
        </span>
        <span class="button-descr">
            closes the popup
        </span>
    </li>
    <li>
        <span class="button-preview">
            <span class="key-button">alt</span>
            <span class="key-button wider leftAlign">down arrow</span>
        </span>
        <span class="button-descr">
            opens the popup
        </span>
    </li>
    <li>
        <span class="button-preview">
            <span class="key-button">alt</span>
            <span class="key-button wide leftAlign">up arrow</span>
        </span>
        <span class="button-descr">
            closes the popup
        </span>
    </li>
</ul>

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
</asp:Content>