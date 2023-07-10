<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="demo-section">
    <div class="product-display">
        <img class="product-image" src="<%= Url.Content("~/content/web/colorpicker/sofa-ffcc33.jpg") %>" alt="" width="285" height="149">

        <div id="color-chooser"></div>
        <%= Html.Kendo().ColorPalette()
              .Name("color-chooser")
              .Palette(new string[] { "#ddd1c3", "#d2d2d2", "#746153", "#3a4c8b", "#ffcc33", "#fb455f", "#ac120f" })
              .Value("#ffcc33")
              .TileSize(30)
              .Events(events => events.Change("change"))
        %>
    </div>

    <div class="product-info">
        <h3>Natural Linen Loveseat</h3>
        <p>Add comfortable seating to your living room or den. The soft foam in the cushions make this loveseat a great place to relax.</p>
        <dl>
            <dt>Dimensions</dt>
            <dd>34" D x 52" W x 31" H</dd>

            <dt>Materials</dt>
            <dd>Wood, Fabric, Foam</dd>

            <dt>Assembly</dt>
            <dd>No Assembly Required</dd>
        </dl>

        <button class="k-button">Add to cart</button>
    </div>
</div>

<script>
    function change() {
        var colorId = this.value().substring(1);
        $(".product-image").attr("src", "<%= Url.Content("~/content/web/colorpicker/") %>sofa-" + colorId + ".jpg");
    }
</script>

<style scoped>

    .demo-section {
        overflow: hidden;
    }

    .product-display, .product-info {
        float: left;
    }

    .product-display {
        background-color: #fff;
        background-image: url();
        height: 380px;
        margin: -10px 8.33% -10px 6.225%;
        text-align: center;
    }

    .product-image {
        margin: 30% 0 18.4%;
    }

    .product-info {
        width: 34.722%;
    }

    .product-info h3 {
        font-size: 2.166em;
        padding: 1.615em 0 .6em;
    }

    .product-info dl {
        margin: 2em 0;
        overflow: hidden;
    }

    .product-info dt,
    .product-info dd {
        float: left;
        margin: 0;
        padding: 0 0 .7em 0;
    }

    .product-info dt {
        font-weight: bold;
        width: 34%;
        text-align: right;
        padding-right: 2.4%;
    }

    .product-info dt:after {
        content: ":";
    }

    .product-info dd {
        width: 63.6%;
    }

    .product-info button {
        width: 60%;
        text-align: center;
    }
</style>
</asp:Content>