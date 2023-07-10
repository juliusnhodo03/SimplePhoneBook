<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/DataViz.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        #gauge-container {
            background: transparent url(<%= Url.Content("~/Content/dataviz/gauge/gauge-container.png") %>) no-repeat 50% 0;
            width: 404px;
            height: 404px;
            text-align: center;
            margin: auto;
            padding-top: 27px;
        }

        #gauge {
            width: 330px;
            height: 330px;
            margin: 0 auto 0;
            border-color: transparent;
        }

        #gauge svg {
            left: 0 !important;
        }
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div class="configuration k-widget k-header" style="width:190px;">
    <span class="configHead">Gauge</span>
    <ul class="options">
        <li>
            <input id="labels" checked="checked" type="checkbox" autocomplete="off" />
            <label for="labels">Show labels</label>
        </li>

        <li>
            <input id="labels-inside" type="radio" value="inside" name="labels-position" checked="checked" />
            <label for="labels-inside">- inside the gauge</label>
        </li>

        <li>
            <input id="labels-outside" type="radio" value="outside" name="labels-position">
            <label for="labels-outside">- outside of the gauge</label>
        </li>

        <li>
            <input id="ranges" checked="checked" type="checkbox" autocomplete="off" />
            <label for="ranges">Show ranges</label>
        </li>
    </ul>
</div>
<div id="gauge-container">
    <%= Html.Kendo().RadialGauge()
            .Name("gauge")
            .Pointer(pointer => pointer.Value(65))
            .Scale(scale => scale
                .MinorUnit(5)
                .StartAngle(-60)
                .EndAngle(240)
                .Max(180)
                .Labels(labels => labels
                    .Position(GaugeRadialScaleLabelsPosition.Inside)
                )
                .Ranges(ranges => {
                    ranges.Add().From(80).To(120).Color("#ffc700");
                    ranges.Add().From(120).To(150).Color("#ff7a00");
                    ranges.Add().From(150).To(180).Color("#c20000");
                })
            )
    %>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $(".configuration").bind("change", refresh);

        window.configuredRanges = $("#gauge").data("kendoRadialGauge").options.scale.ranges;
    });

    function refresh() {
        var gauge = $("#gauge").data("kendoRadialGauge"),
            showLabels = $("#labels").prop("checked"),
            showRanges = $("#ranges").prop("checked"),
            positionInputs = $("input[name='labels-position']"),
            labelsPosition = positionInputs.filter(":checked").val(),
            options = gauge.options;

        options.transitions = false;
        options.scale.labels = options.scale.labels || {};
        options.scale.labels.visible = showLabels;
        options.scale.labels.position = labelsPosition;
        options.scale.ranges = showRanges ? window.configuredRanges : [];

        gauge.redraw();
    }
</script>
</asp:Content>