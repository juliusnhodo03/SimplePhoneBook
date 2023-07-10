﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/cultures/kendo.culture." + System.Threading.Thread.CurrentThread.CurrentCulture + ".min.js") %>"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        //set culture of the Kendo UI
        kendo.culture("<%: System.Threading.Thread.CurrentThread.CurrentCulture  %>");
    </script>

    <div class="configuration k-widget k-header" style="width: 190px">
        <ul class="options">
            <li>Choose culture: 
                <%: Html.Kendo().DropDownList()
                    .Name("CulturesSelector")
                    .BindTo(new[] { "en-US", "de-DE", "bg-BG" })
                    .Value(System.Threading.Thread.CurrentThread.CurrentCulture.ToString())
                    .Events(events => events.Change("cultureChange"))
                %>
            </li>
        </ul>
    </div>

    <%=
    Html.Kendo().Scheduler<Kendo.Mvc.Examples.Models.Scheduler.TaskViewModel>()
        .Name("scheduler")
        .Date(new DateTime(2013, 6, 13))
        .StartTime(new DateTime(2013, 6, 13, 7, 00, 00))
        .Height(400)
        .Timezone("Etc/UTC")
        .Views(views =>
        {
            views.DayView();
            views.WeekView(weekView => weekView.Selected(true));
            views.MonthView();
            views.AgendaView();
        })
        .DataSource(d => d
            .Model(m => m.Id(f => f.TaskID))
            .Read("Read", "Scheduler")
            .Create("Create", "Scheduler")
            .Destroy("Destroy", "Scheduler")
            .Update("Update", "Scheduler")
        )
    %>

    <script type="text/javascript">
        var href = window.location.href;
        if (href.indexOf('culture') > -1) {
            $('#culture').val(href.replace(/(.*)culture=([^&]*)/, '$2'));
        }

        function sendCulture() {
            return {
                culture: "<%= System.Threading.Thread.CurrentThread.CurrentCulture%>"
            }
        }


        function cultureChange() {
            var value = this.value();
            if (href.indexOf('culture') > -1) {
                href = href.replace(/culture=([^&]*)/, 'culture=' + value);
            } else {
                href += href.indexOf('?') > -1 ? '&culture=' + value : '?culture=' + value;
            }
            window.location.href = href;
        }

        function error(e) {
            if (e.errors) {
                var message = "Errors:\n";
                $.each(e.errors, function (key, value) {
                    if ('errors' in value) {
                        $.each(value.errors, function () {
                            message += this + "\n";
                        });
                    }
                });
                alert(message);
            }
        }
</script>

<style type="text/css" scoped>
    .configuration 
    {
        padding-left: 5px;
        margin-bottom: 5px;    
    }
</style>
</asp:Content>