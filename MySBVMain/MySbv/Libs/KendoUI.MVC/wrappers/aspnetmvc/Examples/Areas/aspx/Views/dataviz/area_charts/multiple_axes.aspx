﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/DataViz.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div class="chart-wrapper">
    <%= Html.Kendo().Chart()
        .Name("chart")
        .Title("Hybrid car mileage report")
        .Legend(legend => legend
            .Position(ChartLegendPosition.Top)
        )
        .Series(series =>
        {
            series
                .Column(new int[] { 20, 40, 45, 30, 50 })
                .Stack(true)
                .Color("#003c72")
                .Name("on battery");
            series
                .Column(new int[] { 20, 30, 35, 35, 40 })
                .Stack(true)
                .Color("#0399d4")
                .Name("on gas");
            series
                .Area(new double[] { 30, 38, 40, 32, 42 })
                .Name("mpg")
                .Color("#642381")
                .Axis("mpg");
            series
                .Area(new double[] { 7.8, 6.2, 5.9, 7.4, 5.6 })
                .Name("l/100 km")
                .Color("#e5388a")
                .Axis("l100km");
        })
        .CategoryAxis(axis => axis
            .Categories("Mon", "Tue", "Wed", "Thu", "Fri")
            // Align the first two value axes to the left
            // and the last two to the right.
            //
            // Right alignment is done by specifying a
            // crossing value greater than or equal to
            // the number of categories.
            .AxisCrossingValue(0, 0, 10, 10)
        )
        .ValueAxis(axis => axis
            .Numeric()
                .Title("miles")
                .Min(0).Max(100)
        )
        .ValueAxis(axis => axis
            .Numeric("km")
                .Title("km")
                .Min(0).Max(161).MajorUnit(32)
        )
        .ValueAxis(axis => axis
            .Numeric("mpg")
                .Title("miles per gallon")
                .Color("#642381")
        )
        .ValueAxis(axis => axis
            .Numeric("l100km")
                .Title("liters per 100km")
                .Color("#e5388a")
        )
    %>
</div>
</asp:Content>
