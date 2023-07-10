<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/DataViz.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        #main {
            border-left: 0;
            width: 940px;
        }
        #example {
            background-color: #fff;
        }
        #gauge-container {
            margin: 0 auto;
            overflow: hidden;
            width: 614px;
            height: 324px;

            background: transparent url("<%= Url.Content("~/Content/dataviz/dashboards/car-dashboard.png") %>") no-repeat 50% 50%;
        }

        .k-gauge {
            float: left;
            border-color: transparent;
        }

        #rpm {
            width: 142px;
            height: 147px;
            margin: 85px 0 0 38px;
        }

        #kmh {
            width: 216px;
            height: 216px;
            margin: 57px 0 0 20px;
        }

        #fuel {
            width: 77px;
            height: 84px;

            margin: 90px 0 0 68px;
        }

        #water-temprature {
            width: 84px;
            height: 80px;

            margin: -7px 0 0 62px;
        }
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div id="gauge-container">
    <%= Html.Kendo().RadialGauge()
          .Name("rpm")
          .Theme("black")
          .Pointer(pointer => pointer.Value(0).Color("#ea7001"))
          .Scale(scale => scale
              .StartAngle(-45)
              .EndAngle(120)
              .Min(0)
              .Max(6)
              .MajorUnit(1)
              .MajorTicks(ticks => ticks.Width(1).Size(7))
              .MinorUnit(0.2)
              .MinorTicks(ticks => ticks.Size(5))
              .Ranges(ranges =>
              {
                  ranges.Add().From(4).To(5).Color("#ff7a00");
                  ranges.Add().From(5).To(6).Color("#c20000");
              })
              .Labels(labels => labels.Font("11px Arial,Helvetica,sans-serif"))
          )
    %>

    <%= Html.Kendo().RadialGauge()
          .Name("kmh")
          .Theme("black")
          .Pointer(pointer => pointer.Value(0).Color("#ea7001"))
          .Scale(scale => scale
              .StartAngle(-60)
              .EndAngle(240)
              .Min(0)
              .Max(220)
              .MajorTicks(ticks => ticks.Width(1).Size(14))
              .MinorUnit(2)
              .MinorTicks(ticks => ticks.Size(10))
          )
    %>

    <%= Html.Kendo().RadialGauge()
          .Name("fuel")
          .Theme("black")
          .Pointer(pointer => pointer.Value(0.5).Color("#ea7001"))
          .Scale(scale => scale
              .StartAngle(90)
              .EndAngle(180)
              .Min(0)
              .Max(1)
              .MajorUnit(0.5)
              .MajorTicks(ticks => ticks.Width(2).Size(6))
              .MinorUnit(0.25)
              .MinorTicks(ticks => ticks.Size(3))
              .Ranges(ranges =>
              {
                  ranges.Add().From(0).To(0.1).Color("#c20000");
              })
              .Labels(labels => labels.Font("9px Arial,Helvetica,sans-serif"))
          )
    %>

    <%= Html.Kendo().RadialGauge()
          .Name("water-temprature")
          .Theme("black")
          .Pointer(pointer => pointer.Value(90).Color("#ea7001"))
          .Scale(scale => scale
              .StartAngle(180)
              .EndAngle(270)
              .Min(60)
              .Max(120)
              .MajorUnit(30)
              .MajorTicks(ticks => ticks.Width(2).Size(6))
              .MinorUnit(10)
              .MinorTicks(ticks => ticks.Size(3))
              .Ranges(ranges =>
              {
                  ranges.Add().From(110).To(120).Color("#c20000");
              })
              .Labels(labels => labels.Font("9px Arial,Helvetica,sans-serif"))
          )
    %>
</div>
<script>
    var animateInterval;
    function animateDashboard() {
        if (animateInterval) {
            return;
        }

        var GEARS = [0.14, 0.06, 0.035, 0.027, 0.019],
            IDLE_RPM = 0.9,
            CHANGE_RPM = 4,
            CHANGE_DELAY = 400,
            DECAY_RATE = 0.0017,
            TOP_SPEED = 210,
            ACCELERATION = 0.6,
            INTERVAL = 50;

        var speed = 0,
            skip = 0,
            ratio,
            gear = 0;

        function update() {
            $("#rpm").data("kendoRadialGauge").value(GEARS[gear] * speed + IDLE_RPM);
            $("#kmh").data("kendoRadialGauge").value(speed);
        }

        animateInterval = setInterval(function() {
            if(speed < TOP_SPEED) {
                if (GEARS[gear] * speed > CHANGE_RPM && gear < GEARS.length) {
                    gear++;
                    skip = CHANGE_DELAY / INTERVAL;
                    update();
                }

                if (skip-- < 0) {
                    speed += ACCELERATION - (DECAY_RATE * speed);
                    update();
                }
            } else {
                skip = 100;
                speed = 0;
                gear = 0;
            }
        }, INTERVAL);
    }

    $(document).ready(function() {
        animateDashboard();
    });
</script>
</asp:Content>