﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

<div class="configuration k-widget k-header">
    <span class="configHead">API Functions</span>
    <ul class="options">
        <li>
            <button class="k-button" id="disableNode">Disable node</button>
        </li>
        <li>
            <button class="k-button" id="enableAllNodes">Enable all nodes</button>
        </li>
        <li>
            <button class="k-button" id="removeNode">Remove node</button>
        </li>
        <li>
            <button class="k-button" id="expandAllNodes">Expand all nodes</button>
        </li>
        <li>
            <button class="k-button" id="collapseAllNodes">Collapse all nodes</button>
        </li>
        <li>
            <input id="appendNodeText" value="Node" class="k-textbox"/>
            <button class="k-button" id="appendNodeToSelected">Append node</button>
        </li>
    </ul>
    <span class="configHead" style="margin-top: 1.5em">DataSource interaction</span>
    <ul class="options">
        <li>
            <button class="k-button" id="sortDataSource">Sort</button>
        </li>
        <li>
            <input id="filterText" value="Item 1" class="k-textbox"/>
            <button class="k-button" id="filterDataSource">Filter by text</button>
        </li>
    </ul>
</div>

<div class="demo-section">
    <%=
        Html.Kendo().TreeView()
            .Name("treeview")
            .Items(items =>
            {
                items.Add().Text("Item 1").Expanded(true)
                    .Items(subItems =>
                    {
                        subItems.Add().Text("Item 1.1");
                        subItems.Add().Text("Item 1.2");
                        subItems.Add().Text("Item 1.3");
                    });
                items.Add().Text("Item 2")
                    .Items(subItems =>
                    {
                        subItems.Add().Text("Item 2.1");
                        subItems.Add().Text("Item 2.2");
                        subItems.Add().Text("Item 2.3");
                    });
                items.Add().Text("Item 3");
            })
    %>
</div>
<script>
    $(document).ready(function () {
        var treeview = $("#treeview").data("kendoTreeView"),
            handleTextBox = function (callback) {
                return function (e) {
                    if (e.type != "keypress" || kendo.keys.ENTER == e.keyCode) {
                        callback(e);
                    }
                };
            };


        $("#disableNode").click(function () {
            var selectedNode = treeview.select();

            treeview.enable(selectedNode, false);
        });

        $("#enableAllNodes").click(function () {
            var selectedNode = treeview.select();

            treeview.enable(".k-item");
        });

        $("#removeNode").click(function () {
            var selectedNode = treeview.select();

            treeview.remove(selectedNode);
        });

        $("#expandAllNodes").click(function () {
            treeview.expand(".k-item");
        });

        $("#collapseAllNodes").click(function () {
            treeview.collapse(".k-item");
        });

        var append = handleTextBox(function (e) {
            var selectedNode = treeview.select();

            // passing a falsy value as the second append() parameter
            // will append the new node to the root group
            if (selectedNode.length == 0) {
                selectedNode = null;
            }

            treeview.append({
                text: $("#appendNodeText").val()
            }, selectedNode);
        });

        $("#appendNodeToSelected").click(append);
        $("#appendNodeText").keypress(append);

        // datasource actions

        var ascending = false;

        $("#sortDataSource")
            .text(ascending ? "Sort ascending" : "Sort descending")
            .click(function () {
                treeview.dataSource.sort({
                    field: "text",
                    dir: ascending ? "asc" : "desc"
                });

                ascending = !ascending;

                $(this).text(ascending ? "Sort ascending" : "Sort descending")
            });

        var filter = handleTextBox(function (e) {
            var filterText = $("#filterText").val();

            if (filterText !== "") {
                treeview.dataSource.filter({
                    field: "text",
                    operator: "contains",
                    value: filterText
                });
            } else {
                treeview.dataSource.filter({});
            }
        });

        $("#filterDataSource").click(filter);
        $("#filterText").keypress(filter);
    });
</script>
<style scoped>
    .configuration .k-textbox {
        width: 50px;
    }

    .demo-section{
        width: 200px;
        margin: 0 auto;
    }
</style>

</asp:Content>


