﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

<% var delay = (int)ViewBag.delay;%>
<% using (Html.BeginForm("Animation", "Menu", FormMethod.Post, new { @class = "configuration k-widget k-header" }))
  {%>
    <span class="configHead">Animation Settings</span>
     <ul class="options">
        <li>
            <%= Html.RadioButton("animation", "toggle") %>
            <%= Html.Label("toggle", "toggle animation") %>            
        </li>
        <li>
            <%= Html.RadioButton("animation", "slide") %>
            <%= Html.Label("slide", "slide animation") %>
        </li>
        <li>
            <%= Html.RadioButton("animation", "expand") %>
            <%= Html.Label("expand", "expand animation") %>            
        </li>
        <li>
            <%= Html.CheckBox("opacity") %>
            <%= Html.Label("opacity", "animate opacity") %>               
        </li>
        <li>
            <%= Html.TextBox("delay", delay, new { @class = "k-textbox" }) %>
            <%= Html.Label("delay", "open/close delay")  %>               
        </li>
    </ul>

    <button class="k-button">Apply</button>
  <% }
%>

<%: Html.Kendo().Menu()
    .Name("menu")
    .Animation(animation =>
    {
        animation.Open(open =>
        {
            if (ViewBag.animation == "expand")
            {
                open.Expand(ExpandDirection.Vertical);
            }

            if (ViewBag.animation == "slide")
            {
                open.SlideIn(SlideDirection.Down);
            }

            if (ViewBag.opacity)
            {
                open.Fade(FadeDirection.In);
            }
        });
    })
    .HoverDelay(delay)
    .Items(items =>
    {
        items.Add().Text("Furniture")
                        .Items(children =>
                        {
                            children.Add().Text("Tables & Chairs");
                            children.Add().Text("Sofas");
                            children.Add().Text("Occasional Furniture");
                            children.Add().Text("Childerns Furniture");
                            children.Add().Text("Beds");
                        });

        items.Add().Text("Decor")
                .Items(children =>
                {
                    children.Add().Text("Bed Linen");
                    children.Add().Text("Throws");
                    children.Add().Text("Curtains & Blinds");
                    children.Add().Text("Rugs");
                    children.Add().Text("Carpets");
                });

        items.Add().Text("Storage")
                .Items(children =>
                {
                    children.Add().Text("Wall Shelving");
                    children.Add().Text("Kids Storage");
                    children.Add().Text("Baskets");
                    children.Add().Text("Multimedia Storage");
                    children.Add().Text("Floor Shelving");
                    children.Add().Text("Toilet Roll Holders");
                    children.Add().Text("Storage Jars");
                    children.Add().Text("Drawers");
                    children.Add().Text("Boxes");
                });

        items.Add().Text("Lights")
                .Items(children =>
                {
                    children.Add().Text("Ceiling");
                    children.Add().Text("Table");
                    children.Add().Text("Floor");
                    children.Add().Text("Shades");
                    children.Add().Text("Wall Lights");
                    children.Add().Text("Spotlights");
                    children.Add().Text("Push Light");
                    children.Add().Text("String Lights");
                });
    })
%>
  
<style scoped>
    .configuration .k-textbox
    {
        margin-top: -3px;
    }

    .k-menu
    {
        margin-right: 220px;
    }
</style>

</asp:Content>
