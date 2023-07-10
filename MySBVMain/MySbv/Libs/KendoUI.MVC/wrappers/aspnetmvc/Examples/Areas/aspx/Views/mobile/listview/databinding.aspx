﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Mobile.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<%: Html.Kendo().MobileView()
        .Name("flat")       
        .Title("ListView")
        .Layout("databinding")
        .Content(obj =>        
            Html.Kendo().MobileListView()
                .Name("flat-listview")                
                .DataSource(dataSource => 
                    dataSource
                            .Read("FlatData", "ListView")                        
                )
        )        
%>

<%: Html.Kendo().MobileView()
        .Name("grouped")       
        .Title("ListView")
        .Layout("databinding")
        .Content(obj =>        
            Html.Kendo().MobileListView()
                .Name("grouped-listview")
                .TemplateId("template")                
                .FixedHeaders(true)
                .DataSource(dataSource => 
                    dataSource
                        .Read("GroupedData", "ListView")
                        .Group(group => group.Add("Letter", typeof(string)))
                )
        )        
%>

<% Html.Kendo().MobileLayout()
       .Name("databinding")
       .Header(() =>
        {            
            Html.Kendo().MobileNavBar()
                .Name("navbar")                        
                .Content((navbar) => 
                    {                                
                        %>       
                        <%: Html.Kendo().MobileBackButton()
                                .Align(MobileButtonAlign.Left) 
                                .HtmlAttributes(new { @class = "nav-button" })
                                .Url(Url.RouteUrl(new { controller = "suite" }))
                                .Text("Back")
                        %>                             
                        <%: navbar.ViewTitle("Index") %>                                                            
                        <%
                    })
                .Render();            
        })
        .Footer(() =>
        {
            %>
            <% Html.Kendo().MobileTabStrip()
                   .Items(items =>
                    {
                        items.Add().Text("Flat").Icon("stop").Url("#flat");
                        items.Add().Text("Grouped").Icon("organize").Url("#grouped");
                    })
                   .Render();
            %>
            <%
        })
        .Render();
%>
<script type="text/x-kendo-template" id="template">    
    #=Name#    
</script>

</asp:Content>
