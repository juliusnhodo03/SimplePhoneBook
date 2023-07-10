﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/DataViz.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .k-chart {
            height: 280px;
            padding: 37px;
            margin: 0 0 50px 0;
            width: 390px;
        }
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div class="configuration k-widget k-header" style="width:170px;">
    <span class="configHead">Configuration</span>
    <span class="configTitle">Pie Chart</span>
    <ul class="options">
        <li>
            <input id="labels" checked="checked" type="checkbox" autocomplete="off" />
            <label for="labels">Show labels</label>
        </li>
        <li>
            <input id="alignCircle" name="alignType" type="radio"
                    value="circle" checked="checked" autocomplete="off" />
            <label for="alignCircle">- aligned in circle</label>
        </li>
        <li>
            <input id="alignColumn" name="alignType" type="radio"
                    value="column" autocomplete="off" />
            <label for="alignColumn">- aligned in columns</label>
        </li>
    </ul>
</div>
<%= Html.Kendo().Chart()
    .Name("chart")
    .Title("What is you favourite sport?")
    .Legend(legend => legend
        .Position(ChartLegendPosition.Top)
    )
    .Series(series =>
    {
        series.Pie(new dynamic[] {
            new {category = "Football",value = 35},
            new {category = "Basketball",value = 25},
            new {category = "Volleyball",value = 20},
            new {category = "Rugby",value = 10},
            new {category = "Tennis",value = 10}            
        })
        .Labels(labels => labels
            .Visible(true)
            .Template("#= category # - #= kendo.format('{0:P}', percentage)#")
        );
    })
    .Tooltip(tooltip => tooltip
        .Visible(true)
        .Template("#= category # - #= kendo.format('{0:P}', percentage)#")
    )
%>
<script>
    $(document).ready(function() {
        $(".configuration").bind("change", refresh);
    });

    function refresh() {
        var chart = $("#chart").data("kendoChart"),
            pieSeries = chart.options.series[0],
            labels = $("#labels").prop("checked"),
            alignInputs = $("input[name='alignType']"),
            alignLabels = alignInputs.filter(":checked").val();

        chart.options.transitions = false;
        pieSeries.labels.visible = labels;
        pieSeries.labels.align = alignLabels;

        alignInputs.attr("disabled", !labels);

        chart.refresh();
    }
</script>
</asp:Content>
