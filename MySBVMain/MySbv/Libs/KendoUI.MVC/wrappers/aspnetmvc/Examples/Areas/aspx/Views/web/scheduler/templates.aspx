﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Kendo.Mvc.Examples.Models.Scheduler.Projection>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


<%=Html.Kendo().Scheduler<Kendo.Mvc.Examples.Models.Scheduler.Projection>()
    .Name("scheduler")
    .Date(new DateTime(2013, 6, 13))
    .StartTime(new DateTime(2013, 6, 13, 10, 00, 00))
    .EndTime(new DateTime(2013, 6, 13, 23, 00, 00))
    .Editable(false)
    .Height(600)
    .EventTemplate(
            "<div class='movie-template'>" +
                "<img src='" + Url.Content("~/Content/web/scheduler/") + "#= Image #' />" +
                "<p>" + 
                    "#= kendo.toString(start, 'hh:mm') # - #= kendo.toString(end, 'hh:mm') #" + 
                "</p>" + 
                "<h3>#= title #</h3>" +
                "<a href='#= Imdb #'>Movie in IMDB</a>" +
            "</div>")
    .Views(views =>
    {
        views.DayView();
        views.AgendaView();
    })
    .BindTo(Model)
 %>

<style scoped>
    .movie-template img {
        float: left;
        margin: 0 8px;
    }
    .movie-template p {
        margin: 5px 0 0;
    }
    .movie-template h3 {
        padding: 0 8px 5px;
        font-size: 12px;
    }
    .movie-template a {
        color: #ffffff;
        font-weight: bold;
        text-decoration: none;
    }
    .k-state-hover .movie-template a,
    .movie-template a:hover {
        color: #000000;
    }
    
    body, h1, h2, h3
    {
        margin: 0px;        
    }
</style>

</asp:Content>
