﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Mobile.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<% Html.Kendo().MobileView()
        .Name("hierarchical-view")       
        .Transition("slide")
        .Events(events => events.Show("rebindListView"))
        .Header(() => 
            Html.Kendo().MobileNavBar()
                .Name("employee-navbar")
                .Content(navbar =>
                {
                    %>
                    <%: Html.Kendo().MobileBackButton()
                            .Name("employee-back")
                            .Text("Back")
                            .Align(MobileButtonAlign.Left)
                    %>
                    <%: navbar.ViewTitle("") %>
                    <%
                })
                .Render()
        )
        .Content(() =>
        {
            %>
            <%: Html.Kendo().MobileListView()
                    .Name("hierarchical-listview")
                    .TemplateId("hierarchicalMobileListViewTemplate")                    
            %>            
            <%
        })
        .Render();
%>

<style>
    #hierarchical-listview .employee-image {
        width: 4em;
        height: 4.4em;
        margin: 0;
        -webkit-box-shadow: 0 1px 3px #333;
        box-shadow: 0 1px 3px #333;
        -webkit-border-radius: 8px;
        border-radius: 8px;
        display: inline-block;
        -webkit-background-size: auto 100%;
        background-size: auto 100%;
        background-repeat: no-repeat;
        background-position: center center;
        -webkit-transform: translatez(0);
        vertical-align: middle;
    }

    #hierarchical-listview h2 {
        display: inline-block;
        margin: .5em;
        font-size: 1.1em;
    }
</style>

<script id="hierarchicalMobileListViewTemplate" type="text/x-kendo-template">
    # if (data.HasEmployees) { #
        <a href="\#hierarchical-view?parent=#: data.id #">
            <span class="employee-image" style="background-image: url('<%:Url.Content("~/content/web/Employees/")%>#: id #.jpg')"></span>
            <h2> #: FullName # </h2>
        </a>
    # } else { #
        <span class="employee-image" style="background-image: url('<%:Url.Content("~/content/web/Employees/")%>#: id #.jpg')"></span>
        <h2> #: FullName # </h2>
    # } #
</script>

<script>    
    
    var employees = new kendo.data.HierarchicalDataSource({        
        transport: {
            read: {
                url: '<%:Url.Action("Employees", "ListView") %>'                
            }
        },

        schema: {
            model: {
                id: "EmployeeId",
                hasChildren: "HasEmployees"
            }
        }
    });

    function rebindListView(e) {
        
        var parentID = e.view.params.parent,
            element = e.view.element,
            backButton = element.find('#employee-back'),
            listView = element.find("#hierarchical-listview").data('kendoMobileListView'),            
            navBar = element.find('#employee-navbar').data('kendoMobileNavBar');
        
        if (parentID) {
            employees.fetch(function () {
                var item = employees.get(parentID);
                if (item) {
                    backButton.show();
                    navBar.title(item.FullName);
                    listView.setDataSource(item.children);
                } else {
                    // redirect to root
                    setTimeout(function () {
                        kendo.mobile.application.navigate('#hierarchical-view');
                    }, 0);
                }
            });
        } else {
            backButton.hide();
            navBar.title('Employees');
            listView.setDataSource(employees);
        }
    }

</script>

</asp:Content>
