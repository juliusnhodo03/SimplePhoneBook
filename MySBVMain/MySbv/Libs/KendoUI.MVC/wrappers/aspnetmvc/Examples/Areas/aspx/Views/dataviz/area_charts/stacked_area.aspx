﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/DataViz.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div class="chart-wrapper">
    <%= Html.Kendo().Chart()
        .Name("chart")
        .Title("Browser Usage Trends")
        .Legend(legend => legend
            .Position(ChartLegendPosition.Bottom)
        )
        .SeriesDefaults(seriesDefaults =>
            seriesDefaults.Area().Stack(true)
        )
        .Series(series => {
            series.Area(new double[] { 0, 0, 0, 0, 3.6, 9.8, 22.4, 34.6 }).Name("Chrome");
            series.Area(new double[] { 0, 23.6, 29.9, 36.3, 44.4, 46.4, 43.5, 37.7 }).Name("Firefox");
            series.Area(new double[] { 76.2, 68.9, 60.6, 56.0, 46.0, 37.2, 27.5, 20.2 }).Name("Internet Explorer");
            series.Area(new double[] { 16.5, 2.8, 2.5, 1.2, 0, 0, 0, 0 }).Name("Mozilla");
            series.Area(new double[] { 1.6, 1.5, 1.5, 1.6, 2.4, 2.3, 2.2, 2.5 }).Name("Opera");
            series.Area(new double[] { 0, 0, 0, 1.8, 2.7, 3.6, 3.8, 4.2 }).Name("Safari");
        })
        .CategoryAxis(axis => axis
            .Categories("2004", "2005", "2006", "2007", "2008", "2009", "2010", "2011")
            .MajorGridLines(lines => lines.Visible(false))
        )
        .ValueAxis(axis => axis
            .Numeric().Labels(labels => labels.Format("{0}%"))
            .Max(100)
            .Line(line => line.Visible(false))
            .AxisCrossingValue(-10)
        )
        .Tooltip(tooltip => tooltip
            .Visible(true)
            .Format("{0}%")
            .Template("#= series.name #: #= value #%")
        )
    %>
</div>
</asp:Content>
