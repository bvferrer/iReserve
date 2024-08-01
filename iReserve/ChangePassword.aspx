<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="ChangePassword.aspx.cs" Inherits="ChangePassword" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolKit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager" EnablePageMethods="true" runat="server" />
    <script language="javascript" type="text/javascript">

        window.onload = function () {
            OnChange();
        }
        // =============================================================================================
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

        // =============================================================================================

        function ContainsWhitespace(s) {
            var whitespace = " \t\n\r";

            var i;
            for (i = 0; i < s.length; i++) {
                var c = s.charAt(i);

                if (whitespace.indexOf(c) > -1) return true;
            }

            return false;
        }

        function ContainsNumbers(s) {
            var regExp = /^[0-9]$/;

            for (var i = 0; i < s.length; i++) {
                if (s.charAt(i).match(regExp)) return true;
            }

            return false;
        }

        function ContainsAlphabet(s) {
            var regExp = /^[A-Za-z]$/;

            for (var i = 0; i < s.length; i++) {
                if (s.charAt(i).match(regExp)) return true;
            }

            return false;
        }

        function ContainsSpecialChar(s) {
            var regExp = /^[A-Za-z0-9]$/;

            for (var i = 0; i < s.length; i++) {
                if (!s.charAt(i).match(regExp)) return true;
            }

            return false;
        }

        function HasComplexity(s) {
            return (ContainsNumbers(s) && ContainsAlphabet(s) && ContainsSpecialChar(s));
        }

        function Has3RepeatChar(s) {
            var ctr = 1;
            var chr = s.charAt(0);
            for (var i = 1; i < s.length; i++) {
                if (s.charAt(i) == chr)
                    ctr++;
                else {
                    ctr = 1;
                    chr = s.charAt(i);
                }

                if (ctr >= 3) return true;
            }

            return false;

        }

        function Validate() {
            var oldpass = $get("<%=txtOldPassword.ClientID%>");
            var newpass = $get("<%=txtNewPassword.ClientID%>");
            var verpass = $get("<%=txtRetypePassword.ClientID%>");
            var userid = '<%= Session["UserID"]%>';
            var userHostAddress = '<%=userHostAddress%>';
            var appName = '<%=appName%>';
            var npass = newpass.value;
            var pass = "<%=decryptPassword%>";


            if (oldpass.value == "") {
                alert("Old Password is required!");
                oldpass.focus();
                return false;
            }
            if (pass != oldpass.value) {
                alert("Incorrect old password.");
                oldpass.focus();
                return false;
            }
            if (newpass.value == "") {
                alert("New Password is required!");
                newpass.focus();
                return false;
            }
            if (newpass.value.length < 8) {
                alert("Password must be at least eight (8) characters in length.");
                newpass.focus();
                return false;
            }
            if (ContainsWhitespace(newpass.value)) {
                alert("New password must not contain white spaces.");
                newpass.focus();
                return false;
            }
            if (!HasComplexity(newpass.value)) {
                alert("New password must be a combination of alphabet, number and at least one special character.");
                newpass.focus();
                return false;
            }
            if (Has3RepeatChar(newpass.value)) {
                alert("New password must not contain 3 repeating character in consecutive order.");
                newpass.focus();
                return false;
            }
            if (npass.indexOf(userid) > -1) {
                alert("New password must not contain or be equal to user name.");
                newpass.focus();
                return false;
            }
            if (verpass.value == "") {
                alert("Retype Password is required!");
                verpass.focus();
                return false;
            }
            if (newpass.value != verpass.value) {
                alert("Please verify your new password.");
                verpass.focus();
                return false;
            }

            if ((oldpass.value == newpass.value) && (newpass.value == verpass.value)) {
                alert("New password must not be same as old.");
                newpass.value = "";
                verpass.value = "";
                newpass.focus();
                return false;
            }

            var email = $get('<%=lblEmail.ClientID%>').innerHTML;
            var macAddress = $get("<%=macAddressHiddenField.ClientID%>");
            $find('<%=mdlPopup.ClientID%>').show();
            PageMethods.PasswordChanged(userid, oldpass.value, newpass.value, userHostAddress, appName, email, macAddress.value, OnSucceeded, ErrorHandler, TimeOutHandler);
        }

        function OnSucceeded(result) {
            $find('<%=mdlPopup.ClientID%>').hide();
            var newpass = $get("<%=txtNewPassword.ClientID%>");
            var verpass = $get("<%=txtRetypePassword.ClientID%>");

            if (result) {
                alert("New password is successfully set.");
                window.location.href = "Login.aspx";
            }
            else {
                alert("Password had already been used. Please choose a different password.");

                newpass.value = "";
                verpass.value = "";
                newpass.focus();
            }
        }

        function ErrorHandler(result) {
            $find('<%=mdlPopup.ClientID%>').show();
            var msg = result.get_exceptionType() + "\r\n";
            msg += result.get_message() + "\r\n";
            msg += result.get_stackTrace();
            alert(msg);
        }

        function TimeOutHandler(result) {
            $find('<%=mdlPopup.ClientID%>').show();
            alert("Timeout :" + result);
        }
        function OnChange() {
            var pass = "<%=decryptPassword%>";
            var lock = ($get("<%=txtOldPassword.ClientID%>").value != pass)
            $get("<%=txtNewPassword.ClientID%>").disabled = lock;
            $get("<%=txtRetypePassword.ClientID%>").disabled = lock;
            $get("<%=txtNewPassword.ClientID%>").style.backgroundColor = lock ? 'silver' : 'white'
            $get("<%=txtRetypePassword.ClientID%>").style.backgroundColor = lock ? 'silver' : 'white'
            $get("<%=lblNewPassword.ClientID%>").disabled = lock;
            $get("<%=lblRetypePassword.ClientID%>").disabled = lock;
            if (!lock)
                $get("<%=txtNewPassword.ClientID%>").focus();


        }

        function ShowRules() {
            window.showModalDialog("PasswordRules.aspx", "", "center:yes;resizable:no;dialogHeight:300px;dialogWidth:428px;status:no;scroll:no");
            return false;
        }       
        
    </script>
    <div class="mainDiv">
        <h2 class="header">
            <span>Change Password </span>
        </h2>
        <div class="contentDiv">
            <center>
                <asp:Label ID="lblmessage" runat="server" Font-Size="12px" ForeColor="Red" Font-Names="Tahoma"></asp:Label>
            </center>
            <center>
                &nbsp;</center>
            <span style="font-family: Tahoma; font-size: 12px;">To change your
                password, supply the following information, then click Change.</span>
            <table width="80%" style="margin-top: 5px">
                <tr style="height: 28px">
                    <td style="width: 30%" align="right">
                        <asp:Label ID="Label1" runat="server" Text="E-mail address:" Font-Bold="True" Font-Size="12px"
                            Font-Names="Tahoma"></asp:Label>
                    </td>
                    <td style="width: 70%" align="left">
                        <asp:Label ID="lblEmail" runat="server" Text="test@pjlhuillier.com" Font-Bold="True"
                            Font-Italic="True" ForeColor="#787878" Font-Size="11px"></asp:Label>
                    </td>
                </tr>
                <tr style="height: 40px">
                    <td style="width: 30%" align="right">
                        <asp:Label ID="lblOldPassword" runat="server" Text="Old password:" Font-Bold="True"
                            Font-Names="Tahoma" Font-Size="12px"></asp:Label><br />
                        <br />
                    </td>
                    <td style="width: 70%" align="left">
                        <asp:TextBox ID="txtOldPassword" runat="server" Font-Size="11px" MaxLength="20" Width="197px"
                            onchange="OnChange()" TextMode="Password" BorderStyle="Solid" BorderWidth="1px"
                            CssClass="TextBoxStyle"></asp:TextBox><br />
                        <asp:Label ID="Label3" runat="server" Text="Enter your old password to enable encoding of the new password."
                            ForeColor="DarkGray" Font-Names="Tahoma" Font-Size="12px"></asp:Label>
                    </td>
                </tr>
                <tr style="height: 60px">
                    <td style="width: 30%" align="right">
                        <asp:Label ID="lblNewPassword" runat="server" Text="New password:" Font-Bold="True"
                            Font-Names="Tahoma" Font-Size="12px"></asp:Label><br />
                        <br />
                        <br />
                    </td>
                    <td style="width: 70%" align="left">
                        <asp:TextBox ID="txtNewPassword" runat="server" Font-Size="11px" MaxLength="20" Width="197px"
                            TabIndex="1" TextMode="Password" onFocus="this.select()" BorderStyle="Solid"
                            BorderWidth="1px" CssClass="TextBoxStyle"></asp:TextBox><br />
                        <asp:Label ID="Label2" runat="server" Text="Minimum of eight characters in length with no spaces."
                            ForeColor="DarkGray" Font-Names="Tahoma" Font-Size="12px"></asp:Label><br />
                        <asp:LinkButton ID="lbRules" runat="server" Text="Password Complexity Requirements"
                            ForeColor="#0078D4" TabIndex="4" OnClientClick="return ShowRules()" Font-Names="Tahoma"
                            Font-Size="12px"></asp:LinkButton>
                    </td>
                </tr>
                <tr style="height: 28px">
                    <td style="width: 30%" align="right">
                        <asp:Label ID="lblRetypePassword" runat="server" Text="Retype new password:" Font-Bold="True"
                            Font-Names="Tahoma" Font-Size="12px"></asp:Label>
                    </td>
                    <td style="width: 70%" align="left">
                        <asp:TextBox ID="txtRetypePassword" runat="server" Font-Size="11px" MaxLength="20"
                            Width="197px" TabIndex="2" TextMode="Password" onFocus="this.select()" BorderStyle="Solid"
                            BorderWidth="1px" CssClass="TextBoxStyle"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding: 2px 5px 5px 10px;">
            <table width="80%" style="margin-top: 5px">
                <tr style="height: 28px">
                    <td style="width: 40%" align="right">
                    </td>
                    <td style="width: 60%" align="left">
                        <input type="button" id="btnSave" value="Change" onclick="Validate()" class="ButtonStyle" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 40%" align="right">
                    </td>
                    <td style="width: 60%" align="left">
                        <asp:HiddenField ID="macAddressHiddenField" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <%--Progress bar Modal Popup--%>
        <ajaxToolKit:ModalPopupExtender ID="mdlPopup" runat="server" TargetControlID="pnlProgress"
            PopupControlID="pnlProgress" BackgroundCssClass="modalBackground" DropShadow="true" />
        <asp:Panel ID="pnlProgress" runat="server" Style="background-color: #ffffff; display: none;"
            Width="290px">
            <div style="padding: 8px">
                <table border="0" cellpadding="2" cellspacing="0" style="width: 100%">
                    <tbody>
                        <tr>
                            <td style="width: 50%">
                            </td>
                            <td style="text-align: right">
                                <img alt="" src="img/indicator-big.gif" />
                            </td>
                            <td style="text-align: left; white-space: nowrap">
                                <span style="font-size: larger; font-family: Verdana;">Changing password, Please wait
                                    ...</span>
                            </td>
                            <td style="width: 50%">
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
