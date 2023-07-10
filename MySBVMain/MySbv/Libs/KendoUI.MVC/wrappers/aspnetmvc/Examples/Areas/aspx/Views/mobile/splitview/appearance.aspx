﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Mobile.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<% Html.Kendo().MobileSplitView()
       .Name("appearance")
       .Style(MobileSplitViewStyle.Vertical)
       .Panes(panes => 
       {
            panes.Add().Id("main-panel").Layout("side-default").Content(() =>
            {
                %>
                <% Html.Kendo().MobileView()
                       .Title("Photo Gallery")
                       .Zoom(true)
                       .Content(() =>
                        {
                            %><div class="zoom-image"></div><%
                        })
                       .Render();
                %>

                <% Html.Kendo().MobileLayout()
                       .Name("side-default")
                       .Header(() =>
                        {
                            %>

                            <% Html.Kendo().MobileNavBar()                                   
                                    .Content(navbar =>
                                    {
                                        %>                                            
                                        <%:navbar.ViewTitle("") %>                                            
                                        <%: Html.Kendo().MobileBackButton()
                                                .Align(MobileButtonAlign.Left) 
                                                .Target("_top")
                                                .Url(Url.RouteUrl(new { controller = "suite" }))
                                                .Text("Back")
                                        %>
                                        <%
                                    })
                                
                                    .Render();
                            %>
                            <%
                        })
                        .Render();
                %>
                <%
            });

            panes.Add().Id("bottom-panel").Content(() =>
            {
                %>

                <% Html.Kendo().MobileView()
                       .Name("side-root")
                       .Content(() =>
                        {
                            %>
                            <div id="scrollview-container">

                            <% Html.Kendo().MobileScrollView()
                                   .Page(0)
                                   .Items(items => 
                                   {
                                       items.Add().HtmlAttributes(new { @class = "photo photo1" });
                                       items.Add().HtmlAttributes(new { @class = "photo photo2" });
                                       items.Add().HtmlAttributes(new { @class = "photo photo3" });
                                       items.Add().HtmlAttributes(new { @class = "photo photo4" });
                                       items.Add().HtmlAttributes(new { @class = "photo photo5" });
                                       items.Add().HtmlAttributes(new { @class = "photo photo6" });
                                       items.Add().HtmlAttributes(new { @class = "photo photo7" });
                                       items.Add().HtmlAttributes(new { @class = "photo photo8" });
                                       items.Add().HtmlAttributes(new { @class = "photo photo9" });
                                       items.Add().HtmlAttributes(new { @class = "photo photo10" });
                                       items.Add().HtmlAttributes(new { @class = "photo photo11" });
                                       items.Add().HtmlAttributes(new { @class = "photo photo12" });
                                       items.Add().HtmlAttributes(new { @class = "photo photo13" });
                                       items.Add().HtmlAttributes(new { @class = "photo photo14" });
                                   })
                                   .Render();
                            %>

                            </div>
                            <%
                        })
                        .Render();
                %>

                <%
            });
       })
       .Render();
%>

<script type="text/javascript">
    $(document).on("click", ".photo", function () {
        $("#main-panel").find(".zoom-image").css("background-image", $(this).css("background-image").replace("220/", ""));
    });
</script>

<style scoped>
    #bottom-panel {
        height: 140px;
    }

    .zoom-image {
        width: 2048px;
        height: 1024px;
    }

    #scrollview-container .photo {
        margin: 1px;
        width: 99px;
        height: 99px;
        display: inline-block;
        -webkit-background-size: auto 100%;
        background-size: auto 100%;
        background-repeat: no-repeat;
        background-position: center center;
    }

    .photo1 {background-image: url("<%=Url.Content("~/content/shared/images/photos/220/1.jpg")%>");}
    .photo2 {background-image: url("<%=Url.Content("~/content/shared/images/photos/220/2.jpg")%>");}
    .photo3 {background-image: url("<%=Url.Content("~/content/shared/images/photos/220/3.jpg")%>");}
    .photo4 {background-image: url("<%=Url.Content("~/content/shared/images/photos/220/4.jpg")%>");}
    .photo5 {background-image: url("<%=Url.Content("~/content/shared/images/photos/220/5.jpg")%>");}
    .photo6 {background-image: url("<%=Url.Content("~/content/shared/images/photos/220/6.jpg")%>");}
    .photo7 {background-image: url("<%=Url.Content("~/content/shared/images/photos/220/7.jpg")%>");}
    .photo8 {background-image: url("<%=Url.Content("~/content/shared/images/photos/220/8.jpg")%>");}
    .photo9 {background-image: url("<%=Url.Content("~/content/shared/images/photos/220/9.jpg")%>");}
    .photo10 {background-image: url("<%=Url.Content("~/content/shared/images/photos/220/10.jpg")%>");}
    .photo11 {background-image: url("<%=Url.Content("~/content/shared/images/photos/220/11.jpg")%>");}
    .photo12 {background-image: url("<%=Url.Content("~/content/shared/images/photos/220/12.jpg")%>");}
    .photo13 {background-image: url("<%=Url.Content("~/content/shared/images/photos/220/13.jpg")%>");}
    .photo14 {background-image: url("<%=Url.Content("~/content/shared/images/photos/220/14.jpg")%>");}

    #main-panel .zoom-image
    {
        background-image: url("<%=Url.Content("~/content/shared/images/photos/1.jpg")%>");
        background-position: 50% 50%;
        -webkit-background-size: 100% auto;
        background-size: 100% auto;
        background-repeat: no-repeat;
    }

    #main-panel {
        -webkit-box-flex: 1000;
        -moz-box-flex: 1000;
        -moz-flex: 1000;
        -webkit-flex: 1000;
        -ms-flex: 1000;
        flex: 1000;
    }

    #scrollview-container {
        -webkit-transform: translatez(0);
        width: 95%;
        height: 100%;
        margin: 10px auto 0 auto;
    }
    
</style>

</asp:Content>
