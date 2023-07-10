<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

        <div id="agglomerations">
        <a href="#" title="Canton - 26,300,000" id="canton"></a>
        <a href="#" title="Jakarta - 25,800,000" id="jakarta"></a>
        <a href="#" title="Mexico City - 23,500,000" id="mexico"></a>
        <a href="#" title="Delhi - 23,500,000" id="delhi"></a>
        <a href="#" title="Karachi - 22,100,000" id="karachi"></a>
        <a href="#" title="New York - 21,500,000" id="newyork"></a>
        <a href="#" title="Sao Paulo - 21,300,000" id="saopaolo"></a>
        <a href="#" title="Mumbay/Bombay - 21,100,000" id="bombay"></a>
        <a href="#" title="Los Angeles - 17,100,000" id="losangeles"></a>
        <a href="#" title="Osaka - 16,800,000" id="osaka"></a>
        <a href="#" title="Moscow - 16,200,000" id="moscow"></a>
    </div>

<%:Html.Kendo().Tooltip()
    .For("#agglomerations")
    .Filter("a")
    .Position(TooltipPosition.Top)
    .Width(120)    
%>
    <script type="text/javascript">
        $(document).ready(function() {
            $("#agglomerations").data("kendoTooltip").show($("#canton"));
        });
    </script>
 <style scoped="scoped">

    .demo-section {
        width: 692px;
    }
                
    #agglomerations {
        position: relative;
        width: 692px;
        height: 480px;
        margin: 0 auto;
        background: url('<%: Url.Content("~/content/web/tooltip/world-map.jpg") %>') no-repeat 0 0;
    }
                
    #agglomerations a {
        position: absolute;
        display: block;
        width: 12px;
        height: 12px;
        background-color: #fff600;
        -moz-border-radius: 30px;
        -webkit-border-radius: 30px;
        border-radius: 30px;
        border: 0;
        -moz-box-shadow: 0 0 0 1px rgba(0,0,0,0.5);
        -webkit-box-shadow: 0 0 0 1px rgba(0,0,0,0.5);
        box-shadow: 0 0 0 1px rgba(0,0,0,0.5);
        -moz-transition:  -moz-box-shadow .3s;
        -webkit-transition:  -webkit-box-shadow .3s;
        transition:  box-shadow .3s;
    }
                
    #agglomerations a:hover {
        -moz-box-shadow: 0 0 0 15px rgba(0,0,0,0.5);
        -webkit-box-shadow: 0 0 0 15px rgba(0,0,0,0.5);
        box-shadow: 0 0 0 15px rgba(0,0,0,0.5);
        -moz-transition:  -moz-box-shadow .3s;
        -webkit-transition:  -webkit-box-shadow .3s;
        transition:  box-shadow .3s;
    }
                
    #canton { top: 226px; left: 501px; }
    #jakarta { top: 266px; left: 494px; }
    #mexico { top: 227px; left: 182px; }
    #delhi { top: 214px; left: 448px; }
    #karachi { top: 222px; left: 431px; }
    #newyork { top: 188px; left: 214px; }
    #saopaolo { top: 304px; left: 248px; }
    #bombay { top: 233px; left: 438px; }
    #losangeles { top: 202px; left: 148px; }
    #osaka { top: 201px; left: 535px; }
    #moscow { top: 153px; left: 402px; }
                
    #canton:hover,
    #jakarta:hover,
    #mexico:hover,
    #delhi:hover,
    #karachi:hover,
    #newyork:hover,
    #saopaolo:hover,
    #bombay:hover,
    #losangeles:hover,
    #osaka:hover,
    #moscow:hover { z-index: 10; }
                
</style>

</asp:Content>