﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/DataViz.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div class="chart-wrapper">
    <%= Html.Kendo().Chart()
        .Name("chart")
        .Title("Nutrient balance: Apples, raw")
        .Legend(legend => legend
            .Visible(false)
        )
        .Series(series =>
        {
            series.RadarColumn(new double[] {
                    5, 1, 1, 5, 0, 1,
                    1, 2, 1, 2, 1, 0,
                    0, 2, 1, 0, 3, 1,
                    1, 1, 0, 0, 0
                })
                .Name("Nutrients");
        })
        .CategoryAxis(axis => axis
            .Categories("Df", "Pr", "A", "C", "D", "E",
                        "Th", "Ri", "Ni", "B", "F", "B",
                        "Se", "Mn", "Cu", "Zn", "K", "P",
                        "Fe", "Ca", "Na", "Ch", "Sf")
        )
        .ValueAxis(axis => axis
            .Numeric()
            .Visible(false)
        )
    %>
</div>
</asp:Content>
