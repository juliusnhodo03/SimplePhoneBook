﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div class="configuration k-widget k-header" style="width: 300px">
    <span class="infoHead">Information</span>
    <p>
        The Upload is able to upload files out-of-band using the
        HTML5 File API with fallback for legacy browsers.
    </p>
    <p>
        You need to configure save action that will receive
        the uploaded files.
        An optional remove action is also available.
    </p>
</div>

<div style="width:45%">
    <div class="demo-section">
        <%= Html.Kendo().Upload()
            .Name("files")
            .Async(a => a
                .Save("Save", "Upload")
                .Remove("Remove", "Upload")
                .AutoUpload(true)
            )
        %>
    </div>
</div>
</asp:Content>
