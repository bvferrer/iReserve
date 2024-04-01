<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Login.aspx.cs" Inherits="Login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolKit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="Server">
    <asp:ScriptManager ID="LoginScriptManager" EnablePageMethods="true" runat="server" />
    <script language="javascript" type="text/javascript">
        window.onload = function () {
            $get("<%=userIDTextBox.ClientID%>").focus();
        }

//        function ForgotPassword() {
//            window.showModalDialog("ForgotPasswordUserVerification.aspx", "", "center:yes;resizable:no;dialogHeight:350px;dialogWidth:460px;status:no;scroll:no");
//            $get("<%=userIDTextBox.ClientID%>").focus();
//            return false;
//        }

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
    <div id="loginwrapper">
        <h2>
            <span style="">Welcome to P & EL iReserve System</span></h2>
        <div id="loginkeyboard">
        </div>
        <div id="logincontent">
            <asp:Label ID="Label1" runat="server" Text="Enter your user name and password." Font-Bold="True"
                Font-Size="12px" Width="264px"></asp:Label>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="userIDLabel" runat="server" Text="User ID:" Width="80px" Font-Size="12px"
                            ForeColor="Black"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="userIDTextBox" runat="server" MaxLength="6" Width="175px" onFocus="this.select()"
                            AutoCompleteType="Disabled"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="passwordLabel" runat="server" Text="Password:" Width="80px" Font-Size="12px"
                            ForeColor="Black"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="passwordTextBox" runat="server" MaxLength="20" Width="175px" TextMode="Password"
                            onFocus="this.select()"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <%--<asp:LinkButton ID="forgotPasswordLinkButton" runat="server" Text="Forgot Your Password?"
                            Style="font: bold 13px/20px Trebuchet MS, Arial, Helvetica, sans-serif; float: left;
                            color: #9D0A0A; background-color: inherit;" OnClientClick="return ForgotPassword()"></asp:LinkButton>--%>
                        <asp:Button ID="loginButton" runat="server" Text="Login" OnClick="btnLogin_Click"
                            Style="height: 23px; width: 70px; margin-left: 99px" />
                    </td>
                </tr>
            </table>
        </div>
        <%--Progress bar Popup--%>
        <ajaxToolKit:ModalPopupExtender ID="mdlPopup" runat="server" TargetControlID="pnlProgress"
            PopupControlID="pnlProgress" BackgroundCssClass="modalBackground" DropShadow="true" />
        <asp:Panel ID="pnlProgress" runat="server" Style="background-color: #ffffff; display: none;
            width: 300px">
            <div>
                <table border="0" cellpadding="2" cellspacing="0" style="width: 100%">
                    <tbody>
                        <tr>
                            <td style="width: 50%">
                            </td>
                            <td style="text-align: right">
                                <img alt="" src="img/indicator-big.gif" />
                            </td>
                            <td style="text-align: left; white-space: nowrap">
                                <span style="font-size: larger">
                                    <asp:Label ID="messageLabel" runat="server" Text="Validating, Please wait ..." Font-Names="Tahoma"
                                        Font-Size="12px"></asp:Label>
                                </span>
                            </td>
                            <td style="width: 50%">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:HiddenField ID="macAddressHiddenField" runat="server" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
