﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

<div class="demo-section">
    <h3>Grid with single column sorting enabled</h3>
    <%=Html.Kendo().Grid<Kendo.Mvc.Examples.Models.OrderViewModel>()
        .Name("singleSort")
        .Columns(columns => {
            columns.Bound(o => o.ShipCountry).Width(200);
            columns.Bound(p => p.Freight).Width(200);
            columns.Bound(p => p.OrderDate).Format("{0:dd/MM/yyyy}");
        })
        .Pageable(pageable=> pageable.ButtonCount(5))
        .Sortable(sortable => sortable.AllowUnsort(false))
        .DataSource(dataSource => dataSource
            .Ajax()
            .PageSize(5)
            .Read(read => read.Action("Orders_Read", "Grid"))
         )
    %>
</div>

<div class="demo-section">
    <h3>Grid with multiple column sorting enabled</h3>
    <%= Html.Kendo().Grid<Kendo.Mvc.Examples.Models.OrderViewModel>()
        .Name("multipleSort")
        .Columns(columns => {
            columns.Bound(o => o.ShipCountry).Width(200);
            columns.Bound(p => p.Freight).Width(200);
            columns.Bound(p => p.OrderDate).Format("{0:dd/MM/yyyy}");
        })
        .Pageable(pageable => pageable.ButtonCount(5))
        .Sortable(sortable => sortable
            .AllowUnsort(true)
            .SortMode(GridSortMode.MultipleColumn))
        .DataSource(dataSource => dataSource
            .Ajax()
            .PageSize(5)
            .Read(read => read.Action("Orders_Read", "Grid"))
         )
    %>
</div>

<style scoped="scoped">
    .demo-section {
        width: 600px;
    }
    .demo-section h3 {
        margin: 5px 0 15px 0;
        text-align: center;
    }
</style>

</asp:Content>
