<%@ Page Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<% 
    var culture = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();
%>

<div class="configuration k-widget k-header" style="width: 190px">
    <ul class="options">
        <li>Choose culture: 
            <%: Html.Kendo().DropDownList()
                .Name("CulturesSelector")
                .BindTo(new[] { "en-US", "de-DE", "bg-BG", "fr-FR", "pl-PL", "ru-RU", "uk-UA" })
                .Value(culture)
                .Events(events => events.Change("cultureChange"))
            %>
        </li>
    </ul>
</div>

<% Html.Kendo().Editor()
      .Name("editor")
      .HtmlAttributes(new { style = "width: 700px;height:440px" })
      .Value(() => 
      {
      %>
            <p>
               <img src="http://www.kendoui.com/Image/kendo-logo.png" alt="Editor for ASP.NET MVC logo" style="display:block;margin-left:auto;margin-right:auto;" />
            </p>
            <p>
                Kendo UI Editor allows your users to edit HTML in a familiar, user-friendly way.<br />
                In this version, the Editor provides the core HTML editing engine, which includes basic text formatting, hyperlinks, lists,
                and image handling. The widget <strong>outputs identical HTML</strong> across all major browsers, follows
                accessibility standards and provides API for content manipulation.
            </p>
            <p>Features include:</p>
            <ul>
                <li>Text formatting &amp; alignment</li>
                <li>Bulleted and numbered lists</li>
                <li>Hyperlink and image dialogs</li>
                <li>Cross-browser support</li>
                <li>Identical HTML output across browsers</li>
                <li>Gracefully degrades to a <code>textarea</code> when JavaScript is turned off</li>
            </ul>
            <p>
                Read <a href="http://www.kendoui.com/documentation/introduction.aspx">more details</a> or send us your
                <a href="http://www.kendoui.com/forums.aspx">feedback</a>!
            </p>
            <%
            })
            .Render();
%>

<script type="text/javascript">
    var href = window.location.href;
    if (href.indexOf('culture') > -1) {
        $('#culture').val(href.replace(/(.*)culture=([^&]*)/, '$2'));
    }

    function cultureChange() {
        var value = this.value();
        if (href.indexOf('culture') > -1) {
            href = href.replace(/culture=([^&]*)/, 'culture=' + value);
        } else {
            href += href.indexOf('?') > -1 ? '&culture=' + value : '?culture=' + value;
        }
        window.location.href = href;
    }

</script>
</asp:Content>