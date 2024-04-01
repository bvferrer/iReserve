<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MonitorDisplayConferenceRoom.aspx.cs"
    Inherits="MonitorDisplayConferenceRoom" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>iReserve</title>
    <link rel="SHORTCUT ICON" href="img/PEL.ico" />
    <link href="css/Style.css" media="screen" rel="Stylesheet" />
    <script src="js/jquery-1.12.0.js"></script>
    <%--<script src="js/jquery-1.9.1.js" type="text/javascript"></script>--%>
    <%--<script src="js/jquery-ui-1.10.0.custom.js"></script>--%>
    <meta http-equiv="refresh" content="600" />
    
    <style type="text/css">
        html, body
        {
            margin: 0;
            padding: 0;
            height: 100%;
            width: 100%;
            overflow: auto;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="calendarUpdatePanel" runat="server">
        <ContentTemplate>
            <div class="displayContainer">
                <div class="displayheader">
                    <asp:Label ID="displayTitleLabel" runat="server" Text="P & EL Conference Room Schedule"></asp:Label>
                    <asp:Label ID="displayDateLabel" runat="server" Text=""></asp:Label>
                </div>
                <div class="displaywrapper">
                    <asp:GridView ID="displayGridView" runat="server" OnRowDataBound="displayGridView_RowDataBound"
                        Font-Names="Arial" CssClass="displayGridViewCSS" AllowPaging="true" PageSize="8"
                        OnDataBound="displayGridView_DataBound">
                        <PagerSettings Visible="false" />
                    </asp:GridView>
                    <asp:Timer ID="pageTimer" runat="server" Interval="10000" OnTick="pageTimer_Tick">
                    </asp:Timer>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
    <script type="text/javascript">
        //catch timeout error
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() != undefined) {
                var errorMessage;
                var error = args.get_error;
                if (args.get_response().get_statusCode() == '12031') {    //ERROR_INTERNET_CONNECTION_RESET
                    errorMessage = "Session Timeout.";
                }
                else if (args.get_response().get_statusCode() == '200') {
                    errorMessage = args.get_error().message;
                }
                else {
                    errorMessage = 'An unspecified error occurred. ';
                }
                args.set_errorHandled(true);
//                alert("An error occurred: " + errorMessage + " Please refresh the page to restart your session.");
                // window.location.reload();  //refresh to re-login
            }
        }
    </script>
</body>
</html>
