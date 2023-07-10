﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Kendo.Mvc.Examples.Models" %>
<%@ Import Namespace="Kendo.Mvc.UI.Fluent" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">

<div class="demo-section">
    <strong>Inline data (default settings)</strong>
    <%=
        Html.Kendo().TreeView()
            .Name("treeview-left")
            .BindTo((IEnumerable<TreeViewItemModel>)ViewBag.inlineDefault)
    %>
</div>

<div class="demo-section">
    <strong>Inline data</strong>
    <%=
        Html.Kendo().TreeView()
            .Name("treeview-right")
            .BindTo((IEnumerable<CategoryItem>)ViewBag.inline, (NavigationBindingFactory<TreeViewItem> mappings) =>
            {
                mappings.For<CategoryItem>(binding => binding.ItemDataBound((item, category) =>
                    {
                        item.Text = category.CategoryName;
                    })
                    .Children(category => category.SubCategories));
                
                mappings.For<SubCategoryItem>(binding => binding.ItemDataBound((item, subCategory) =>
                {
                    item.Text = subCategory.SubCategoryName;
                }));
            })
    %>
</div>

<style scoped>
    #example {
        text-align: center;
    }

    .demo-section {
        display: inline-block;
        vertical-align: top;
        width: 220px;
        text-align: left;
        margin: 0 2em;
    }
</style>

</asp:Content>
