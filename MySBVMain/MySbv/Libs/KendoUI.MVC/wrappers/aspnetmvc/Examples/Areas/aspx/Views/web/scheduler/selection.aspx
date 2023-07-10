﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1"  ContentPlaceHolderID="MainContent" runat="server">

<%=Html.Kendo().Scheduler<Kendo.Mvc.Examples.Models.Scheduler.MeetingViewModel>()
    .Name("scheduler")
    .Date(new DateTime(2013,6 ,13))
    .StartTime(new DateTime(2013, 6, 13, 7, 00, 00))
    .Height(600)
    .Views(views =>
    {
        views.DayView();
        views.WeekView(weekView => weekView.Selected(true));
        views.MonthView();
        views.AgendaView();
    })
    .Selectable(true)
    .Timezone("Etc/UTC")
    .Resources(resource =>
         {
            resource.Add(m => m.RoomID)
                .Title("Room")
                .DataTextField("Text")
                .DataValueField("Value")
                .DataColorField("Color")
                .BindTo(new[] { 
                    new { Text = "Meeting Room 101", Value = 1, Color = "#6eb3fa" },
                    new { Text = "Meeting Room 201", Value = 2, Color = "#f58a8a" } 
                });
            resource.Add(m => m.Atendees)
                .Title("Atendees")
                .Multiple(true)
                .DataTextField("Text")
                .DataValueField("Value")
                .DataColorField("Color")
                .BindTo(new[] { 
                    new { Text = "Alex", Value = 1, Color = "#f8a398" } ,
                    new { Text = "Bob", Value = 2, Color = "#51a0ed" } ,
                    new { Text = "Charlie", Value = 3, Color = "#56ca85" } 
                });
         })
    .DataSource(d => d
            .Model(m => { 
                m.Id(f => f.MeetingID);                                                 
            })
        .Read("Meetings_Read", "Scheduler")
        .Create(create => create.Action("Meetings_Create", "Scheduler").Data("serialize"))
        .Destroy("Meetings_Destroy", "Scheduler")
        .Update(update => update.Action("Meetings_Update", "Scheduler").Data("serialize"))
    )
%>

<script type="text/javascript">
    $(document.body).keydown(function (e) {
        if (e.altKey && e.keyCode == 87) {
            $("#scheduler").data("kendoScheduler").wrapper.focus();
        }
    });
</script>

<ul class="keyboard-legend" style="padding-top: 25px">
    <li>
        <span class="button-preview">
            <span class="key-button leftAlign">Alt</span>
            +
            <span class="key-button">w</span>
        </span>
        <span class="button-descr">
            focuses the widget
        </span>
    </li>
</ul>
 
<h4>Actions applied on Scheduler table</h4>
<ul class="keyboard-legend">
    <li>
        <span class="button-preview">
            <span class="key-button wider">Arrow Keys</span>
        </span>
        <span class="button-descr">
            to navigate over the cells.
        </span>
    </li>
    <li>
        <span class="button-preview">
            <span class="key-button">Enter</span>
        </span>
        <span class="button-descr">
            creates new event from empty cell or selection. On selected event opens the edit popup window.
        </span>
    </li>
    <li>
        <span class="button-preview">
            <span class="key-button">Tab</span>
        </span>
        <span class="button-descr">
            moves between available events.
        </span>
    </li>
    <li>
        <span class="button-preview">
            <span class="key-button leftAlign">Shift</span> 
            +
            <span class="key-button">Tab</span>
        </span>
        <span class="button-descr">
            moves between the available events backwards.
        </span>
    </li>
    <li>
        <span class="button-preview">
            <span class="key-button">1</span> 
            -
            <span class="key-button">4</span>
        </span>
        <span class="button-descr">
            moves between the available views.
        </span>
    </li>
    <li>
        <span class="button-preview">
                <span class="key-button">Esc</span>
        </span>
        <span class="button-descr">
            closes the edit popup window.
        </span>
    </li>
    <li>
        <span class="button-preview">
            <span class="key-button leftAlign">Shift</span>
        </span>
        <span class="button-descr">
            to create multiple selection.
        </span>
    </li>
</ul>

<script type="text/javascript">
    function serialize(data) {
        for (var property in data) {
            if ($.isArray(data[property])) {
                serializeArray(property, data[property], data);
            }
        }
    }

    function serializeArray(prefix, array, result) {
        for (var i = 0; i < array.length; i++) {
            if ($.isPlainObject(array[i])) {
                for (var property in array[i]) {
                    result[prefix + "[" + i + "]." + property] = array[i][property];
                }
            }
            else {
                result[prefix + "[" + i + "]"] = array[i];
            }
        }
    }
</script>
</asp:Content>
