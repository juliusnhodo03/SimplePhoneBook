﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Mobile.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<% Html.Kendo().MobileView()
        .Name("listview-templates")
        .Title("ListView")
        .Content(() =>
        {
                %>
                <div class="head">&nbsp;</div>
                <%: Html.Kendo().MobileListView()
                        .Name("custom-listview")
                        .TemplateId("customListViewTemplate")
                        .HeaderTemplateId("customListViewHeaderTemplate")
                        .DataSource(dataSource =>
                            dataSource
                                .Read("FixedHeaders_Read", "ListView")
                                .Group(group => group.Add("Letter", typeof(string)))
                        )
                %>
                <%
        })
        .Render();        
%>

<script type="text/x-kendo-template" id="customListViewTemplate">
    <img class="item-photo" src="#=Url#" />
    <h3 class="item-title">#=Name#</h3>
    <p class="item-info">#=Description#</p>    
    <%: Html.Kendo().MobileButton()
            .Text("Order")
            .HtmlAttributes(new { @class = "details-link" })
    %>
</script>

<script type="text/x-kendo-template" id="customListViewHeaderTemplate">
    <h2>Letter #=value#</h2>
</script>

<style scoped>
#listview-templates .km-content {
    background-color: #f0f0f0;
}

#listview-templates .head {
    display: block;
    margin: 0 auto;
    width: 100%;
    height: 100px;
    background: url(../../content/mobile/listview/food.jpg) no-repeat center top;
    -webkit-background-size: 100% auto;
    background-size: 100% auto;
}

#listview-templates .km-navbar .km-button {
    background-color: #974d2e;
}

#listview-templates .km-navbar,
.km-wp-light #listview-templates .km-navbar,
.km-wp-dark #listview-templates .km-navbar,
#listview-templates .km-content .km-button {
    background: url(../../content/shared/images/patterns/pattern9.png);
    color: #ffffff;
}

#listview-templates .item-photo {
    width: 4em;
    height: 4em;
    float: left;
    margin: .5em 0;
    -webkit-box-shadow: 0 1px 3px #333;
    box-shadow: 0 1px 3px #333;
    -webkit-border-radius: 8px;
    border-radius: 8px;
}

#custom-listview .item-title {
    float: left;
    font-size: 1em;
    line-height: 1.4em;
    margin: .3em 1em 0 .6em;
    width: 50%;
}
#custom-listview .item-info {
    float: left;
    font-size: .7em;
    line-height: 1em;
    margin: 0 0 0 .95em;
    width: 45%;
}

#custom-listview .item-title,
#custom-listview .item-info {
    color: #4c2a1b;
}

#custom-listview .item-info {
    color: #974d2e;
}

#custom-listview {
    margin: 0;
}

#listview-templates .details-link {
    margin-top: -1em;
    position: absolute;
    right: 0.6em;
    top: 50%;
}

#listview-templates .km-listview .km-list {
    margin: 0;
}

#listview-templates .km-listview .km-group-title {
    padding: 0;
    border-top: 0;
    box-shadow: none;
}

#listview-templates .km-group-title h2 {
    margin: 0;
    padding-top: .2em;
    text-shadow: none;
}

#listview-templates .km-group-title h2 {
    color: #974d2e;
    font-weight: normal;
    font-size: 1.4em;
    background-image: -moz-linear-gradient(center top , rgba(255, 255, 255, 0.5), rgba(255, 255, 255, 0.45) 6%, rgba(255, 255, 255, 0.2) 50%, rgba(255, 255, 255, 0.15) 50%, rgba(100, 100, 100, 0)), url(../../content/shared/images/patterns/pattern4.png);
    background-image: -webkit-gradient(linear, 50% 0, 50% 100%, color-stop(0, rgba(255, 255, 255, 0.5)), color-stop(0.06, rgba(255, 255, 255, 0.45)), color-stop(0.5, rgba(255, 255, 255, 0.2)), color-stop(0.5, rgba(255, 255, 255, 0.15)), color-stop(1, rgba(100, 100, 100, 0))), url(../../content/shared/images/patterns/pattern4.png);
}

#listview-templates .km-listview .km-list {
    border-top-color: #dcbe87;
    background-image: url(../../content/shared/images/patterns/pattern8.png);
}
.km-tablet .km-ios #listview-templates .km-view-title
{
    color: #fff;
    text-shadow: 0 -1px rgba(0,0,0,.5);
}
.km-wp .km-group-title .km-text {
    padding: 0;
}
.km-wp .km-group-title .km-text h2 {
    padding: 0 0 .1em .7em;
}
</style>

</asp:Content>
