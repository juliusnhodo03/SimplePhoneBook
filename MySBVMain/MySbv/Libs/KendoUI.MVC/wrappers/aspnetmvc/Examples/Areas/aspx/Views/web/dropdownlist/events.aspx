<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div class="demo-section">
    <h3 class="title">DropDownList</h3>
    <%= Html.Kendo().DropDownList()
            .Name("dropdownlist")
            .DataTextField("Text")
            .DataValueField("Value")
            .BindTo(new List<SelectListItem>()
            {
                new SelectListItem() {
                    Text = "Item1", Value = "1"  
                },
                new SelectListItem() {
                    Text = "Item2", Value = "2"  
                },
                new SelectListItem() {
                    Text = "Item3", Value = "3"  
                }
            })
            .Events(e =>
            {
                e.Change("change").Select("select").Open("open").Close("close").DataBound("dataBound");
            })
    %>
</div>
<div class="demo-section">
    <h3 class="title">Console log</h3>
    <div class="console"></div>
</div>
<script>
    function open() {
        kendoConsole.log("event: open");
    };

    function close() {
        kendoConsole.log("event: close");
    };

    function change() {
        kendoConsole.log("event: change");
    };

    function dataBound() {
        kendoConsole.log("event: dataBound");
    };

    function select(e) {
        if ("kendoConsole" in window) {
            var dataItem = this.dataItem(e.item.index());
            kendoConsole.log("event :: select (" + dataItem.Text + " : " + dataItem.Value + ")" );
        }
    };
</script>
<style scoped>
    .demo-section {
        width: 500px;
        text-align: center;
    }
    .demo-section .k-dropdown {
        text-align: left;
    }
    .console {
        margin: 0;
    }
</style>
</asp:Content>