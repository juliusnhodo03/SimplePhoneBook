﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div class="configuration k-widget k-header" style="width: 300px">
    <span class="infoHead">Information</span>
    <p>
        This example shows how to handle events triggered by kendoUpload.
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
            .Events(events => events
                .Cancel("onCancel")
                .Complete("onComplete")
                .Error("onError")
                .Progress("onProgress")
                .Remove("onRemove")
                .Select("onSelect")
                .Success("onSuccess")
                .Upload("onUpload")
            )
        %>
    </div>
</div>

<div class="demo-section" style="margin-top: 50px;">
    <h3 class="title">Console log
    </h3>
    <div class="console"></div>
</div>
<script>
    function onSelect(e) {
        kendoConsole.log("Select :: " + getFileInfo(e));
    }

    function onUpload(e) {
        kendoConsole.log("Upload :: " + getFileInfo(e));
    }

    function onSuccess(e) {
        kendoConsole.log("Success (" + e.operation + ") :: " + getFileInfo(e));
    }

    function onError(e) {
        kendoConsole.log("Error (" + e.operation + ") :: " + getFileInfo(e));
    }

    function onComplete(e) {
        kendoConsole.log("Complete");
    }

    function onCancel(e) {
        kendoConsole.log("Cancel :: " + getFileInfo(e));
    }

    function onRemove(e) {
        kendoConsole.log("Remove :: " + getFileInfo(e));
    }

    function onProgress(e) {
        kendoConsole.log("Upload progress :: " + e.percentComplete + "% :: " + getFileInfo(e));
    }

    function getFileInfo(e) {
        return $.map(e.files, function(file) {
            var info = file.name;

            // File size is not available in all browsers
            if (file.size > 0) {
                info  += " (" + Math.ceil(file.size / 1024) + " KB)";
            }
            return info;
        }).join(", ");
    }
</script>

</asp:Content>
