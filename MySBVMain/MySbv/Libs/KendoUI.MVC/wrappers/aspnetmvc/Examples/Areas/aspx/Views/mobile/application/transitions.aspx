﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Mobile.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<% Html.Kendo().MobileView()                
        .Name("view-transitions")
        .Title("Camera App")
        .Content(() =>
        {
            %>
            <img src="<%= Url.Content("~/content/mobile/shared/color-lens.png")%>" class="camera-image" /><br />
            <% Html.Kendo().MobileButton()
                   .Name("signUp")
                   .Text("Login/Sign-up")
                   .HtmlAttributes(new { @class = "transitions-button" })
                   .Url("#view-transitions-login")
                   .Render();
            %>
            <%
        })
        .Render();
%>

<% Html.Kendo().MobileView()
        .Layout("examples")
        .Name("view-transitions-login")
        .Transition("overlay:up")        
        .Title("Login/Sign-up")
        .Content(() =>
        {
            %>                      
            <% Html.Kendo().MobileListView()
                   .Style("inset")
                   .Items(items =>
                   {
                       items.Add().Content(() =>
                       {
                           %>
                           <label for="username">Username:</label> <input type="text" id="text" />
                           <%
                       });
                       
                       items.Add().Content(() =>
                       {
                           %>
                           <label for="password">Password:</label> <input type="password" id="password" />
                           <%
                       });
                   })
                   .Render();                
            %>         
            <% Html.Kendo().MobileButton()
                   .Name("login")
                   .Text("Login")
                   .Transition("overlay:down reverse")
                   .HtmlAttributes(new { @class = "transitions-button" })
                   .Url("#view-transitions-welcome")
                   .Render();
            %>
            <br />
            <% Html.Kendo().MobileButton()
                   .Name("cancel")
                   .Text("Cancel")
                   .Transition("overlay:up reverse")
                   .HtmlAttributes(new { @class = "transitions-cancel" })
                   .Url("#view-transitions")
                   .Render();
            %>            
            <%
        })
        .Render();
%>

<% Html.Kendo().MobileView()
        .Layout("examples")
        .Name("view-transitions-welcome")
        .Title("Welcome")
        .Content(() =>
        {
            %>
            <img src="<%= Url.Content("~/content/mobile/modalview/lens.png") %>" class="camera-image" /><br />            
            <% Html.Kendo().MobileButton()
                   .Name("SignOut")
                   .Text("Sign out")
                   .Transition("slide:right")
                   .HtmlAttributes(new { @class = "transitions-button" })
                   .Url("#view-transitions")
                   .Render();
            %>
            <%
        })
        .Render();
%>
<style scoped>
    .transitions-button,
    .transitions-cancel {
        display: block;
        text-align: center;
        margin: .6em .8em 0;
        font-size: 1.2em;
    }

    #view-transitions,
    #view-transitions-welcome p {
    	color: #fff;
        text-align: center;
    }

    #view-transitions img,
    #view-transitions-welcome img {
        display: block;
        margin: 30px auto;
    }
    
    #view-transitions .km-content,
    #view-transitions-login .km-content,
    #view-transitions-welcome .km-content {
        background: url(../../content/shared/images/patterns/pattern1.png) repeat 0 0;
    }

    .km-ios #view-transitions-welcome .km-button,
    .km-flat #view-transitions-welcome .km-content .km-button {
        background-color: DarkRed;
	    color: #fff;
    }
    
    .km-ios #view-transitions-login .km-button,
    .km-flat #view-transitions-login .km-content .km-button {
        background-color: Green;
	    color: #fff;
    }
    
    .km-ios #view-transitions .km-button,
    .km-ios #view-transitions-login .transitions-cancel {
        background-color: #000;
    }
    
    .km-flat #view-transitions .km-button,
    .km-flat #view-transitions-login .km-content .transitions-cancel {
        background-color: #000;
	    color: #fff;
    }
    
    .km-ios #view-transitions .km-navbar,
    .km-ios #view-transitions-login .km-navbar,
    .km-ios #view-transitions-welcome .km-navbar,
    .km-flat #view-transitions .km-navbar,
    .km-flat #view-transitions-login .km-navbar,
    .km-flat #view-transitions-welcome .km-navbar {
        background-color: #000;
	    color: #fff;
    }
</style>

</asp:Content>
