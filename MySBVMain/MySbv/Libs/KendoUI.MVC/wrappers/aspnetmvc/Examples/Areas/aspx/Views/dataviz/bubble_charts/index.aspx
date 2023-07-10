﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/DataViz.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
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
    <%= Html.Kendo().Chart()
        .Name("chart")
        .Title("Job Growth for 2011")
        .Legend(false)
        .Series(series =>
        {
            series.Bubble(new dynamic[] {
                new {
                    x = -2500,
                    y = 50000,
                    size = 500000,
                    category = "Microsoft"
                }, new {
                    x = 500,
                    y = 110000,
                    size = 7600000,
                    category = "Starbucks"
                }, new {
                    x = 7000,
                    y = 19000,
                    size = 700000,
                    category = "Google"
                }, new {
                    x = 1400,
                    y = 150000,
                    size = 700000,
                    category = "Publix Super Markets"
                }, new {
                    x = 2400,
                    y = 30000,
                    size = 300000,
                    category = "PricewaterhouseCoopers"
                }, new {
                    x = 2450,
                    y = 34000,
                    size = 90000,
                    category = "Cisco"
                }, new {
                    x = 2700,
                    y = 34000,
                    size = 400000,
                    category = "Accenture"
                }, new {
                    x = 2900,
                    y = 40000,
                    size = 450000,
                    category = "Deloitte"
                }, new {
                    x = 3000,
                    y = 55000,
                    size = 900000,
                    category = "Whole Foods Market"
                }
            });
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
