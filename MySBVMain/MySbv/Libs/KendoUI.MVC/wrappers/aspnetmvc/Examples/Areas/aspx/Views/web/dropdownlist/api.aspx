<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/aspx/Views/Shared/Web.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="configuration-horizontal">
        <div class="config-section">
        <span class="configHead">API Functions</span>
        <ul class="options">
            <li>
                <button id="enable" class="k-button">Enable</button> <button id="disable" class="k-button">Disable</button>
            </li>
            <li>
                <button id="readonly" class="k-button">Readonly</button>
            </li>
            <li>
                <button id="open" class="k-button">Open</button> <button id="close" class="k-button">Close</button>
            </li>
            <li>
                <button id="getValue" class="k-button">Get value</button> <button id="getText" class="k-button">Get text</button>
            </li>
        </ul>
        </div>
        <div class="config-section">
        <span class="configHead">Select</span>
        <ul class="options">
            <li>
                <input id="index" value="0" class="k-textbox" style="width: 40px; margin: 0;" /> <button id="select" class="k-button">Select by index</button>
            </li>
            <li>
                <input id="value" value="1" class="k-textbox" style="width: 40px; margin: 0;" /> <button id="setValue" class="k-button">Select by value</button>
            </li>
             <li>
                <input id="word" value="Pulp" class="k-textbox" style="width: 100px; margin: 0;" /> <button id="find" class="k-button">Select item starting with</button>
            </li>
        </ul>
        </div>
    </div>
    <div class="demo-section">        
        <%= Html.Kendo().DropDownList()
                .Name("movies")
                .DataTextField("Text")
                .DataValueField("Value")
                .HtmlAttributes(new { style = "width: 250px" })
                .BindTo(new List<SelectListItem>()
                {
                    new SelectListItem() {
                    Text = "The Shawshank Redemption", Value ="1"
                    },
                    new SelectListItem() {
                    Text = "The Godfather", Value ="2"
                    },
                    new SelectListItem() {
                    Text = "The Godfather: Part II", Value ="3"
                    },
                    new SelectListItem() {
                    Text = "Il buono, il brutto, il cattivo.", Value ="4"
                    },
                    new SelectListItem() {
                    Text = "Pulp Fiction", Value ="5"
                    },
                    new SelectListItem() {
                    Text = "12 Angry Men", Value ="6"
                    },
                    new SelectListItem() {
                    Text = "Schindler's List", Value ="7"
                    },
                    new SelectListItem() {
                    Text = "One Flew Over the Cuckoo's Nest", Value ="8"
                    },
                    new SelectListItem() {
                    Text = "Inception", Value ="9"
                    },
                    new SelectListItem() {
                    Text = "The Dark Knight", Value ="10"
                    }
                })
        %>
    </div>
    <script>
        $(document).ready(function() {
            $("#movies").closest(".k-widget")
                        .attr("id", "products_wrapper");

            var dropdownlist = $("#movies").data("kendoDropDownList"),
                setValue = function(e) {
                    if (e.type != "keypress" || kendo.keys.ENTER == e.keyCode)
                        dropdownlist.value($("#value").val());
                },
                setIndex = function(e) {
                    if (e.type != "keypress" || kendo.keys.ENTER == e.keyCode) {
                        var index = parseInt($("#index").val());
                        dropdownlist.select(index);
                    }
                },
                setSearch = function(e) {
                    if (e.type != "keypress" || kendo.keys.ENTER == e.keyCode)
                        dropdownlist.search($("#word").val());
                };

            $("#enable").click(function() {
                dropdownlist.enable();
            });

            $("#disable").click(function() {
                dropdownlist.enable(false);
            });

            $("#readonly").click(function () {
                dropdownlist.readonly();
            });

            $("#open").click(function() {
                dropdownlist.open();
            });

            $("#close").click(function() {
                dropdownlist.close();
            });

            $("#getValue").click(function() {
                alert(dropdownlist.value());
            });

            $("#getText").click(function() {
                alert(dropdownlist.text());
            });

            $("#setValue").click(setValue);
            $("#value").keypress(setValue);

            $("#select").click(setIndex);
            $("#index").keypress(setIndex);

            $("#find").click(setSearch);
            $("#word").keypress(setSearch);
        });
    </script>
    <style scoped>
        .configuration .k-textbox {
            width: 40px;
        }
        .demo-section {
            width: 660px;
            padding: 30px;
            text-align: center;
        }
        .k-button {
            min-width: 80px;
        }
        .configuration-horizontal .options li {
            padding: 3px 0;
        }
    </style>
</asp:Content>