﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/DataViz.Master"
         Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<style>
    .chart-wrapper .bubble 
    {
        padding-top: 50px;
    }
    .chart-wrapper .bubble 
    {
        width: 530px;
        height: 380px;
    }
</style>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div class="chart-wrapper">
    <%= Html.Kendo().Chart<Kendo.Mvc.Examples.Models.CrimeData>()
        .Name("chart")
        .Legend(false)
        .DataSource(ds => ds.Read(read => read
            .Action("_CrimeStats", "Bubble_Charts")
        ))
        .Series(series =>
        {
            series.Bubble(
                model => model.Murder,
                model => model.Burglary,
                model => model.Population,
                model => model.State
            );
        })
        .XAxis(axis => axis
            .Numeric()
            .Labels(labels => labels
                .Format("{0:N0}")
            )
            .Title("Murders per 100,000 population")
        )
        .YAxis(axis => axis
            .Numeric()
            .Labels(labels => labels
                .Format("{0:N0}")
            )
            .Title("Murders per 100,000 population")
        )
        .Tooltip(tooltip => tooltip
            .Visible(true)
            .Format("{3}: Population {2:N0}")
        )
    %>
</div>
</asp:Content>
