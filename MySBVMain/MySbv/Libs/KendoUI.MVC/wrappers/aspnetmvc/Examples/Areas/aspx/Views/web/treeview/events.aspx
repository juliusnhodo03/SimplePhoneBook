<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%= Html.Kendo().TreeView()
    .Name("treeview")
    .HtmlAttributes(new { @class = "demo-section", style = "width: 200px" })
    .DragAndDrop(true)
    .Events(events => events
        .Select("onSelect")
        .Change("onChange")
        .Collapse("onCollapse")
        .Expand("onExpand")
        .DragStart("onDragStart")
        .Drag("onDrag")
        .Drop("onDrop")
        .DragEnd("onDragEnd")
    )
    .Items(treeview =>
    {
        treeview.Add().Text("Furniture")
            .Expanded(true)
            .Items(furniture =>
            {
                furniture.Add().Text("Tables & Chairs");
                furniture.Add().Text("Sofas");
                furniture.Add().Text("Occasional Furniture");
            });

        treeview.Add().Text("Decor")
            .Expanded(false)
            .Items(furniture =>
            {
                furniture.Add().Text("Bed Linen");
                furniture.Add().Text("Curtains & Blinds");
                furniture.Add().Text("Carpets");
            });

        treeview.Add().Text("Storage");
    })
%>

<div class="demo-section">
    <h3 class="title">Console log
    </h3>
    <div class="console"></div>
</div>

<script>
    var treeview;

    function onSelect(e) {
        kendoConsole.log("Selecting: " + this.text(e.node));
    }

    function onChange(e) {
        kendoConsole.log("Selection changed");
    }

    function onCollapse(e) {
        kendoConsole.log("Collapsing " + treeview.text(e.node));
    }

    function onExpand(e) {
        kendoConsole.log("Expanding " + treeview.text(e.node));
    }

    function onDragStart(e) {
        kendoConsole.log("Started dragging " + treeview.text(e.sourceNode));
    }

    function onDragCancelled(e) {
        kendoConsole.log("Cancelled dragging of " + treeview.text(e.sourceNode));
    }

    function onDrag(e) {
        kendoConsole.log("Dragging " + this.text(e.sourceNode));
    }

    function onDrop(e) {
        kendoConsole.log(
        "Dropped " + treeview.text(e.sourceNode) +
        " (" + (e.valid ? "valid" : "invalid") + ")"
        );
    }

    function onDragEnd(e) {
        kendoConsole.log("Finished dragging " + treeview.text(e.sourceNode));
    }
        
    $(document).ready(function() {
        treeview = $("#treeview").data("kendoTreeView");
    });
</script>

</asp:Content>