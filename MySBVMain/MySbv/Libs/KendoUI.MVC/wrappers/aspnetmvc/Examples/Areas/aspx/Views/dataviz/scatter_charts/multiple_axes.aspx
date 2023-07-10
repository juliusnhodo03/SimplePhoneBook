﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/DataViz.Master"
Inherits="System.Web.Mvc.ViewPage<IEnumerable<Kendo.Mvc.Examples.Models.EngineDataPoint>>" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div class="chart-wrapper">
    <%= Html.Kendo().Chart(Model)
        .Name("chart")
        .Title("Dyno run results")
        .Legend(legend => legend
            .Visible(false)
        )
        .SeriesDefaults(seriesDefaults => seriesDefaults
            .ScatterLine().Width(2)
        )
        .Series(series =>
        {
            series.ScatterLine(model => model.RPM, model => model.Power)
                .Name("Power")
                .Tooltip(tooltip => tooltip.Format("{1} bhp @ {0:N0} rpm"));

            series.ScatterLine(model => model.RPM, model => model.Torque)
                .Name("Torque")
                .YAxis("torque")
                .Tooltip(tooltip => tooltip.Format("{1} lb-ft @ {0:N0} rpm"));
        })
        .XAxis(x => x
            .Numeric()
            .Title(title => title.Text("Engine rpm"))

            // Align torque axis to the right by specifying
            // a crossing value greater than or equal to the axis maximum.
            .AxisCrossingValue(0, 10000)
            .Labels(labels => labels.Format("{0:N0}"))
        )
        .YAxis(y => y
            .Numeric()
            .Title(title => title.Text("Power (bhp)"))
        )
        .YAxis(y => y
            .Numeric("torque")
            .Title(title => title.Text("Torque (lb-ft)"))
        )
        .Tooltip(tooltip => tooltip
            .Visible(true)
        )
    %>
</div>
</asp:Content>
