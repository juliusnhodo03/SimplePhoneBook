<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.Kendo().Grid<Kendo.Mvc.Examples.Models.ProductViewModel>()
        .Name("grid")
        .Columns(columns =>
        {
            columns.Bound(p => p.ProductName);
            columns.Bound(p => p.UnitPrice).Width(100);
            columns.Bound(p => p.UnitsInStock).Width(100);
            columns.Bound(p => p.Discontinued).Width(100);
            columns.Command(command => { command.Edit(); command.Destroy(); }).Width(172);
        })
        .ToolBar(toolbar => toolbar.Create())
        .Editable(editable => editable.Mode(GridEditMode.InLine))
        .Pageable()
        .Sortable()
        .Scrollable()
        .HtmlAttributes(new { style = "height:430px;" })
        .DataSource(dataSource => dataSource
            .Ajax()
            .PageSize(20)
            .Events(events => events.Error("error_handler"))
            .Model(model => model.Id(p => p.ProductID))
            .Create(update => update.Action("EditingInline_Create", "Grid"))
            .Read(read => read.Action("EditingInline_Read", "Grid"))
            .Update(update => update.Action("EditingInline_Update", "Grid"))
            .Destroy(update => update.Action("EditingInline_Destroy", "Grid"))
        )
    %>
    <script type="text/javascript">
        function error_handler(e) {
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
</asp:Content>
