<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% Html.Kendo().Splitter()
      .Name("vertical")
      .Orientation(SplitterOrientation.Vertical)
      .Panes(verticalPanes =>
      {
          verticalPanes.Add()
              .HtmlAttributes(new { id = "top-pane" })
              .Scrollable(false)
              .Collapsible(false)
              .Content(() => {
                %>
                <% Html.Kendo().Splitter()
                    .Name("horizontal")
                    .HtmlAttributes(new { style = "height: 100%;" })
                    .Panes(horizontalPanes =>
                    {
                        horizontalPanes.Add()
                            .HtmlAttributes(new { id = "left-pane" })
                            .Size("220px")
                            .Collapsible(true)
                            .Content(() => { %>
                                <div class="pane-content">
                                    <h3>Inner splitter / left pane</h3>
                                    <p>Resizable and collapsible.</p>
                                </div>
                            <% });
    
                        horizontalPanes.Add()
                            .HtmlAttributes(new { id = "center-pane" })
                            .Content(() => { %>
                                <div class="pane-content">
                                    <h3>Inner splitter / center pane</h3>
                                    <p>Resizable only.</p>
                                </div>
                            <% });
    
                        horizontalPanes.Add()
                            .HtmlAttributes(new { id = "right-pane" })
                            .Collapsible(true)
                            .Size("220px")
                            .Content(() => { %>
                                <div class="pane-content">
                                    <h3>Inner splitter / right pane</h3>
                                    <p>Resizable and collapsible.</p>
                                </div>
                            <% });
                    }).Render(); %>
              <% });

          verticalPanes.Add()
              .Size("100px")
              .HtmlAttributes(new { id = "middle-pane" })
              .Collapsible(false)
              .Content(() => { %>
                <div class="pane-content">
                    <h3>Outer splitter / middle pane</h3>
                    <p>Resizable only.</p>
                </div>
              <% });

          verticalPanes.Add()
              .Size("100px")
              .HtmlAttributes(new { id = "bottom-pane" })
              .Resizable(false)
              .Collapsible(false)
              .Content(() => { %>
                <div class="pane-content">
                    <h3>Outer splitter / bottom pane</h3>
                    <p>Non-resizable and non-collapsible.</p>
                </div>
              <% });
      })
      .Render();
%>

<script>
    /*$(document).ready(function() {
        $("#vertical").kendoSplitter({
            orientation: "vertical",
            panes: [
                { collapsible: false },
                { collapsible: false, size: "100px" },
                { collapsible: false, resizable: false, size: "100px" }
            ]
        });

        $("#horizontal").kendoSplitter({
            panes: [
                { collapsible: true, size: "220px" },
                { collapsible: false },
                { collapsible: true, size: "220px" }
            ]
        });
    });*/
</script>

<style scoped>
    #vertical {
        height: 380px;
        width: 700px;
        margin: 0 auto;
    }
    #middle-pane {
        background-color: rgba(60, 70, 80, 0.10);
    }
    #bottom-pane {
        background-color: rgba(60, 70, 80, 0.15);
    }
    #left-pane {
        background-color: rgba(60, 70, 80, 0.05);
    }
    #center-pane {
        background-color: rgba(60, 70, 80, 0.05);
    }
    #right-pane {
        background-color: rgba(60, 70, 80, 0.05);
    }
    .pane-content {
        padding: 0 10px;
    }
</style>
</asp:Content>