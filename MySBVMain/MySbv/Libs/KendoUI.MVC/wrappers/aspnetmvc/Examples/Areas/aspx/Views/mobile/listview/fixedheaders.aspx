﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Mobile.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<%: Html.Kendo().MobileView()
        .Name("listview-headers")       
        .Title("Fixed Headers")
        .Content(obj =>        
            Html.Kendo().MobileListView()
                .Name("fixed-listview")
                .TemplateId("listviewHeadersTemplate")                
                .FixedHeaders(true)
                .DataSource(dataSource => 
                    dataSource
                        .Read("FixedHeaders_Read", "ListView")
                        .Group(group => group.Add("Letter", typeof(string)))
                )
        )        
%>

<script type="text/x-kendo-template" id="listviewHeadersTemplate">
    <img class="item-photo" src="#=Url#" />
    <h3 class="item-title">#=Name#</h3>
    <p class="item-info">#=Description#</p>    
    <%: Html.Kendo().MobileButton()
            .Text("Order")
            .HtmlAttributes(new { @class = "details-link" })
    %>
</script>

<style scoped>
    .item-photo {
        width: 4em;
        height: 4em;
        float: left;
        margin: .5em 0;
        -webkit-box-shadow: 0 1px 3px #333;
        box-shadow: 0 1px 3px #333;
        -webkit-border-radius: 8px;
        border-radius: 8px;
    }

    #fixed-listview .item-title {
	    float: left;
	    font-size: 1em;
        line-height: 1.4em;
        margin: .3em 1em 0 .6em;
        width: 50%;
    }
    #fixed-listview .item-info {
	    float: left;
	    font-size: .7em;
	    line-height: 1em;
        margin: 0 0 0 .95em;
        width: 45%;
    }

    .details-link {
        margin-top: -1.2em;
        position: absolute;
        right: 0.6em;
        top: 50%;
    }
    .km-listview .km-list {
        margin: 0;
    }
</style>
</asp:Content>
