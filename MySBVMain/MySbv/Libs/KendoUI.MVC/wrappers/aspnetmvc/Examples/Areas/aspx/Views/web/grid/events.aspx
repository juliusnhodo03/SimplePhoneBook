﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="demo-section">
    <%= Html.Kendo().Grid<Kendo.Mvc.Examples.Models.ProductViewModel>()
        .Name("grid")
        .Columns(columns =>
        {
            columns.Bound(p => p.ProductName).Title("Product Name");
            columns.Bound(p => p.UnitPrice).Title("Unit Price");
            columns.Bound(p => p.UnitsInStock).Title("Units In Stock");
        })
        .Pageable()
        .Sortable()
        .Selectable(selectable => selectable
            .Mode(GridSelectionMode.Multiple)
            .Type(GridSelectionType.Cell))
        .Events(events => events.Change("onChange").DataBound("onDataBound").DataBinding("onDataBinding"))
        .DataSource(dataSource => dataSource
            .Ajax()
            .Read(read => read.Action("Products_Read", "Grid"))
         )
    %>
</div>
 <div class="demo-section">
    <h3 class="title">Console log</h3>
    <div class="console"></div>
</div>
<style scoped="scoped">
    
    .demo-section {
        width: 600px;
    }    
        
</style>

<script type="text/javascript">

    function onChange(arg) {
        var selected = $.map(this.select(), function (item) {
            return $(item).text();
        });

        kendoConsole.log("Selected: " + selected.length + " item(s), [" + selected.join(", ") + "]");
    }

    function onDataBound(arg) {
        kendoConsole.log("Grid data bound");
    }

    function onDataBinding(arg) {
        kendoConsole.log("Grid data binding");
    }

</script>
</asp:Content>
