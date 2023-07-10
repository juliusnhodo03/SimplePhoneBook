﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/DataViz.Master"
Inherits="System.Web.Mvc.ViewPage<IEnumerable<Kendo.Mvc.Examples.Models.JobGrowth>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<style>
    .chart-wrapper {
        position: relative;
    }

    .chart-wrapper ul {
        font-size: 11px;
        margin: 53px 20px 0 0;
        padding: 30px;
        position: absolute;
        right: 0;
        top: 0;
        text-transform: uppercase;
        width: 150px;
        height: 105px;
    }
</style>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div class="chart-wrapper">
    <%= Html.Kendo().Chart(Model)
        .Name("chart")
        .Title("Job Growth for 2011")
        .Legend(false)
        .Series(series =>
        {
            series.Bubble(
                model => model.Growth,
                model => model.Jobs,
                model => model.Applications,
                model => model.Company
            );
        })
        .XAxis(axis => axis
            .Numeric()
            .Labels(labels => labels
                .Format("{0:N0}")
                .Skip(1)
            )
            .AxisCrossingValue(-5000)
            .MajorUnit(2000)
            .PlotBands(plotBands => plotBands
                .Add(-5000, 0, "#00f").Opacity(0.05)
            )
        )
        .YAxis(axis => axis
            .Numeric()
            .Labels(labels => labels
                .Format("{0:N0}")
            )
            .Line(line => line
                .Width(0)
            )
        )
        .Tooltip(tooltip => tooltip
            .Visible(true)
            .Format("{3}: {2:N0} applications")
            .Opacity(1)
        )
    %>
    <ul class="k-content">
        <li>Circle size shows number of job applicants</li>
        <li>Vertical position shows number of employees</li>
        <li>Horizontal position shows job growth</li>
    </ul>
</div>
</asp:Content>
