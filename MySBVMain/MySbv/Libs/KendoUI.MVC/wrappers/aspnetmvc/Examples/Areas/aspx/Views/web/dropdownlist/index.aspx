<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

<div id="cap-view" class="k-header">
    <h2>Customize your Kendo Cap</h2>
    <div id="cap" class="black-cap"></div>
    <div id="options">
    <h3>Cap Color</h3>
    <%= Html.Kendo().DropDownList()
          .Name("color")
          .DataTextField("Text")
          .DataValueField("Value")
          .Events(e => e.Change("change"))
          .BindTo(new List<SelectListItem>() {
              new SelectListItem() {
                  Text = "Black",
                  Value = "1"
              },
              new SelectListItem() {
                  Text = "Orange",
                  Value = "2"
              },
              new SelectListItem() {
                  Text = "Grey",
                  Value = "3"
              }
          })
          .Value("1")
    %>

    <h3>Cap Size</h3>
    <%= Html.Kendo().DropDownList()
          .Name("size")
          .BindTo(new List<string>() {
              "S - 6 3/4\"",
              "M - 7 1/4\"",
              "L - 7 1/8\"",
              "XL - 7 5/8\""
          })
    %>
    
    <button class="k-button" id="get">Customize</button>
    </div>
</div>
<style scoped>
    #example h2 {
        font-weight: normal;
    }
    #cap-view {
        border-radius: 10px 10px 10px 10px;
        border-style: solid;
        border-width: 1px;
        overflow: hidden;
        width: 500px;
        margin: 30px auto;
        padding: 20px 20px 0 20px;
    }
    #cap {
        float: left;
        width: 242px;
        height: 225px;
        margin: 30px 40px 30px 20px;
        background-image: url('<%= Url.Content("~/Content/web/dropdownlist/cap.png") %>');
        background-repeat: no-repeat;
        background-color: transparent;
    }
    .black-cap {
        background-position: 0 0;
    }
    .grey-cap {
        background-position: 0 -225px;
    }
    .orange-cap {
        background-position: 0 -450px;
    }
    #options {
        padding: 30px;
    }
    #options h3 {
        font-size: 1em;
        font-weight: bold;
        margin: 25px 0 8px 0;
    }
    #get {
        margin-top: 25px;
    }
</style>

<script>
    function change() {
        var value = $("#color").val();
        $("#cap")
                .toggleClass("black-cap", value == 1)
                .toggleClass("orange-cap", value == 2)
                .toggleClass("grey-cap", value == 3);
    };

    $(document).ready(function () {
        $("#get").click(function () {
            var color = $("#color").data("kendoDropDownList"),
                size = $("#size").data("kendoDropDownList");

            alert('Thank you! Your Choice is:\n\nColor ID: ' + color.value() + ' and Size: ' + size.value());
        });
    });
</script>
</asp:Content>