<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Redirect.aspx.cs" Inherits="Redirect" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolKit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Loading...</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="LoginScriptManager" EnablePageMethods="true" runat="server" />
    <script language="javascript" type="text/javascript">
        function pageLoad() {
            GetMacAddress();
        }

        function GetMacAddress() {
            var macAddress = "";
            var ipAddress = "";
            var computerName = "";
            var wmi = GetObject("winmgmts:{impersonationLevel=impersonate}");
            e = new Enumerator(wmi.ExecQuery("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = True"));
            for (; !e.atEnd(); e.moveNext()) {
                var s = e.item();
                macAddress = s.MACAddress;
            }

            $get("<%=macAddressHiddenField.ClientID%>").value = unescape(macAddress);
        }
    </script>
    <div>
        <span>Please wait...</span>
        <asp:HiddenField ID="macAddressHiddenField" runat="server" />
    </div>
    </form>
</body>
</html>
