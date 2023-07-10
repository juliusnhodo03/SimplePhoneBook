﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<% using (Html.BeginForm("Animation", "Window", FormMethod.Post, new { @class = "configuration k-widget k-header" }))
 {%>     
    <span class="configHead">Animation Settings</span>
     <ul class="options">
        <li>
            <%=Html.RadioButton("animation", "zoom")%>
            <%=Html.Label("zoom", "default/zoom animation")%>            
        </li>
        <li>
            <%=Html.RadioButton("animation", "toggle")%>
            <%=Html.Label("toggle", "toggle animation")%>            
        </li>
        <li>
             <%=Html.RadioButton("animation", "expand")%>
             <%=Html.Label("expand", "expand animation")%>            
        </li>
        <li>
             <%=Html.CheckBox("opacity")%>
             <%=Html.Label("opacity", "animate opacity")%>               
        </li>
    </ul>

    <button class="k-button">Apply</button>
  <% }
%>

<%
    Html.Kendo().Window()
        .Name("window")
        .Animation(animation =>
        {
            animation.Open(open =>
            {
                if (ViewBag.animation == "expand")
                {
                    open.Expand(ExpandDirection.Vertical);
                }

                if (ViewBag.animation == "zoom")
                {
                    open.Zoom(ZoomDirection.In);
                }

                if (ViewBag.opacity)
                {
                    open.Fade(FadeDirection.In);
                }
            });
            
            animation.Close(close =>
            {
                close.Reverse(true);
                if (ViewBag.animation == "expand")
                {
                    close.Expand(ExpandDirection.Vertical);                
                }

                if (ViewBag.animation == "zoom")
                {
                    close.Zoom(ZoomDirection.Out);
                    close.Reverse(false);                  
                }

                if (ViewBag.opacity)
                {
                    close.Fade(FadeDirection.In);
                }                            
            });
        })
        .Content(() => 
        {
            %>
            <div style="text-align: center;">
                    <img src="<%=Url.Content("~/Content/web/window/egg-chair.png")%>" alt="ARNE JACOBSEN EGG CHAIR" />
                    <p>ARNE JACOBSEN EGG CHAIR<br /> Image by: <a href="http://www.conranshop.co.uk/" title="http://www.conranshop.co.uk/">http://www.conranshop.co.uk/</a></p>
                </div>        
            <%
        })
        .Width(500)
        .Draggable()
        .Resizable()
        .Title("EGG CHAIR")
        .Events(events=> events.Close("close"))
        .Render();
%>

 <span id="undo" style="display:none" class="k-group">Click here to open the window.</span>

<script type="text/javascript">
    function close() {
        $("#undo").fadeIn(300);
    }

    $("#undo")
        .bind("click", function () {
            $("#window").data("kendoWindow").open();
            $("#undo").fadeOut(300);
        });

</script>

<style scoped>
    #example {
        min-height:400px;
    }

    #undo {
        text-align: center;
        position: absolute;
        white-space: nowrap;
        border-width: 1px;
        border-style: solid;
        padding: 2em;
        cursor: pointer;
    }
</style>

</asp:Content>
