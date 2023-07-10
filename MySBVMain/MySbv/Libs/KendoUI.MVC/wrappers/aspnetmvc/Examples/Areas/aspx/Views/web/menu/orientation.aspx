﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<MenuOrientation>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<% Html.BeginForm(); %>
    <text>
    <div class="configuration k-widget k-header">
        <span class="configHead">Orientation Settings</span>
        <ul class="options">
            <li>
                <label for="orientation">orientation</label>

                <%= Html.Kendo().DropDownList()
                      .Name("orientation")
                      .Items(items =>
                            {
                                items.Add().Text("Horizontal").Value("horizontal");
                                items.Add().Text("Vertical").Value("vertical");
                            })
                      .SelectedIndex(Model == MenuOrientation.Horizontal ? 0 : 1)
                %>
            </li>
        </ul>
        <button class="k-button">Apply</button>
    </div>
    </text>
<% Html.EndForm(); %>

<%= Html.Kendo().Menu()
      .Name("menu")
      .HtmlAttributes(new { style = "margin-right: 220px; width: 300px;" })
      .Items(items =>
      {
          items.Add()
              .Text("Men's")
              .Items(children =>
               {
                   children.Add().Text("Footwear")
                           .Items(innerChildren =>
                           {
                               innerChildren.Add().Text("Leisure Trainers");
                               innerChildren.Add().Text("Running Shoes");
                               innerChildren.Add().Text("Outdoor Footwear");
                               innerChildren.Add().Text("Sandals/Flip Flops");
                               innerChildren.Add().Text("Footwear Accessories");
                           });

                   children.Add().Text("Leisure Clothing")
                           .Items(innerChildren =>
                           {
                               innerChildren.Add().Text("T-Shirts");
                               innerChildren.Add().Text("Hoodies & Sweatshirts");
                               innerChildren.Add().Text("Jackets");
                               innerChildren.Add().Text("Pants");
                               innerChildren.Add().Text("Shorts");
                           });

                   children.Add().Text("Sports Clothing")
                           .Items(innerChildren =>
                           {
                               innerChildren.Add().Text("Football");
                               innerChildren.Add().Text("Basketball");
                               innerChildren.Add().Text("Golf");
                               innerChildren.Add().Text("Tennis");
                               innerChildren.Add().Text("Swimwear");
                           });

                   children.Add().Text("Accessories");
               });

          items.Add().Text("Women's")
               .Items(children =>
               {
                   children.Add().Text("Footwear")
                           .Items(innerChildren =>
                           {
                               innerChildren.Add().Text("Leisure Trainers");
                               innerChildren.Add().Text("Running Shoes");
                               innerChildren.Add().Text("Outdoor Footwear");
                               innerChildren.Add().Text("Sandals/Flip Flops");
                               innerChildren.Add().Text("Footwear Accessories");
                           });

                   children.Add().Text("Leisure Clothing")
                           .Items(innerChildren =>
                           {
                               innerChildren.Add().Text("T-Shirts");
                               innerChildren.Add().Text("Jackets");
                           });

                   children.Add().Text("Sports Clothing")
                           .Items(innerChildren =>
                           {
                               innerChildren.Add().Text("Basketball");
                               innerChildren.Add().Text("Golf");
                               innerChildren.Add().Text("Tennis");
                               innerChildren.Add().Text("Swimwear");
                           });

                   children.Add().Text("Accessories");
               });

          items.Add()
              .Text("Boy's")
              .Items(children =>
              {
                  children.Add().Text("Footwear")
                          .Items(innerChildren =>
                          {
                              innerChildren.Add().Text("Leisure Trainers");
                              innerChildren.Add().Text("Running Shoes");
                              innerChildren.Add().Text("Outdoor Footwear");
                              innerChildren.Add().Text("Sandals/Flip Flops");
                              innerChildren.Add().Text("Footwear Accessories");
                          });

                  children.Add().Text("Leisure Clothing")
                          .Items(innerChildren =>
                          {
                              innerChildren.Add().Text("T-Shirts");
                              innerChildren.Add().Text("Hoodies & Sweatshirts");
                              innerChildren.Add().Text("Jackets");
                              innerChildren.Add().Text("Pants");
                              innerChildren.Add().Text("Shorts");
                          });

                  children.Add().Text("Sports Clothing")
                          .Items(innerChildren =>
                          {
                              innerChildren.Add().Text("Football");
                              innerChildren.Add().Text("Basketball");
                              innerChildren.Add().Text("Golf");
                              innerChildren.Add().Text("Tennis");
                              innerChildren.Add().Text("Swimwear");
                          });

                  children.Add().Text("Accessories");
              });

          items.Add().Text("Girl's")
               .Items(children =>
               {
                   children.Add().Text("Footwear")
                           .Items(innerChildren =>
                           {
                               innerChildren.Add().Text("Leisure Trainers");
                               innerChildren.Add().Text("Running Shoes");
                               innerChildren.Add().Text("Outdoor Footwear");
                               innerChildren.Add().Text("Sandals/Flip Flops");
                               innerChildren.Add().Text("Footwear Accessories");
                           });

                   children.Add().Text("Leisure Clothing")
                           .Items(innerChildren =>
                           {
                               innerChildren.Add().Text("T-Shirts");
                               innerChildren.Add().Text("Jackets");
                           });

                   children.Add().Text("Sports Clothing")
                           .Items(innerChildren =>
                           {
                               innerChildren.Add().Text("Basketball");
                               innerChildren.Add().Text("Golf");
                               innerChildren.Add().Text("Tennis");
                               innerChildren.Add().Text("Swimwear");
                           });

                   children.Add().Text("Accessories");
               });
      })
      .Orientation(Model)
%>
</asp:Content>
