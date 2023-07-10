﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Mobile.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<% Html.Kendo().MobileView()
        .Name("button-home")        
        .Title("Sports Academy")
        .Content(() =>
        {
            %>
            <div class="home head">&nbsp;</div>            
            <% Html.Kendo().MobileButton()                    
                    .HtmlAttributes(new { @class = "button", style = "color: #fff; background-color: #f60" })
                    .Text("Home")
                    .Url("#button-home")
                    .Render();
            %>
            <% Html.Kendo().MobileButton()                    
                    .HtmlAttributes(new { @class = "button" })
                    .Text("Facility")
                    .Url("#facility")
                    .Render();
            %>
            <% Html.Kendo().MobileButton()                    
                    .HtmlAttributes(new { @class = "button" })
                    .Text("Sports")
                    .Url("#sports")
                    .Render();
            %>          
            <% Html.Kendo().MobileListView()
                   .Style("inset")
                   .Items(items => items.Add()
                       .Text("<p>Our Sports Academy provides a venue for outdoor and indoor sports and activities for children and adults of all ages.</p>"))
                   .Render();                
            %>
            <%
        })
        .Render();
%>

<% Html.Kendo().MobileView()
        .Name("facility")
        .Layout("examples")
        .Title("Facility")
        .Content(() =>
        {
            %>
            <div class="facility head">&nbsp;</div>            
            <% Html.Kendo().MobileButton()                    
                    .HtmlAttributes(new { @class = "button" })
                    .Text("Home")
                    .Url("#button-home")
                    .Render();
            %>
            <% Html.Kendo().MobileButton()
                    .HtmlAttributes(new { @class = "button", style = "color: #fff; background-color: #f60" })
                    .Text("Facility")
                    .Url("#facility")
                    .Render();
            %>
            <% Html.Kendo().MobileButton()                    
                    .HtmlAttributes(new { @class = "button" })
                    .Text("Sports")
                    .Url("#sports")
                    .Render();
            %>          
            <% Html.Kendo().MobileListView()
                   .Style("inset")
                   .Items(items => items.Add()
                       .Text("<p>The facility has two indoor basketball fields, olympic size swimming pool, outdoor soccer field, baseball field, golf club and more.</p>"))
                   .Render();                
            %>
            <%
        })
        .Render();
%>

<% Html.Kendo().MobileView()
        .Name("sports")
        .Layout("examples")
        .Title("Sports")
        .Content(() =>
        {
            %>
            <div class="sports head">&nbsp;</div>            
            <% Html.Kendo().MobileButton()                    
                    .HtmlAttributes(new { @class = "button" })
                    .Text("Home")
                    .Url("#button-home")
                    .Render();
            %>
            <% Html.Kendo().MobileButton()
                    .HtmlAttributes(new { @class = "button" })
                    .Text("Facility")
                    .Url("#facility")
                    .Render();
            %>
            <% Html.Kendo().MobileButton()
                    .HtmlAttributes(new { @class = "button", style = "color: #fff; background-color: #f60" })
                    .Text("Sports")
                    .Url("#sports")
                    .Render();
            %>           
            <% Html.Kendo().MobileListView()
                   .Style("inset")
                   .Items(items => 
                       {
                           items.Add().Text("American Football");
                           items.Add().Text("Baseball");
                           items.Add().Text("Basketball");
                           items.Add().Text("Football");
                           items.Add().Text("Golf");
                           items.Add().Text("Swimming");
                       })
                   .Render();                
            %>
            <%
        })
        .Render();
%>
<style scoped>
    .button {
        margin: 0 0 0 .5em;
        text-align: center;
    }
    .button:first-of-type {
        margin: 0 0 0 1em;
    }
    .home {
        background: url(<%= Url.Content("~/content/mobile/shared/sports-home.jpg")%>) no-repeat center center;	
    }
    .facility {
        background: url(<%= Url.Content("~/content/mobile/shared/sports-facility.jpg")%>) no-repeat center center;	
    }
    .sports {
        background: url(<%= Url.Content("~/content/mobile/shared/sports.jpg")%>) no-repeat center center;	
    }
    #button-home .head,
    #facility .head,
    #sports .head {
	    display: block;
	    margin: 1em;
	    height: 120px;
        -webkit-background-size: 100% auto;
        background-size: 100% auto;
    }
    .km-ios .head,
    .km-blackberry .head {
        -webkit-border-radius: 10px;
        border-radius: 10px;
    }
</style>

</asp:Content>
