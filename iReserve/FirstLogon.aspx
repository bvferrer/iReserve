<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="FirstLogon.aspx.cs" Inherits="FirstLogon" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolKit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager" EnablePageMethods="true" runat="server" />
    <script language="javascript" type="text/javascript">

        window.onload = function () {
            OnChange();
        }

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
            var pass = "<%=decryptPassword%>";
            var userid = '<%= Session["UserID"]%>';
            var npass = newpass.value;
            var email = "<%=email%>";

            var question1 = $get("<%=ddlQuestion1.ClientID%>");
            var answer1 = $get("<%=txtAnswer1.ClientID%>");
            var question2 = $get("<%=ddlQuestion2.ClientID%>");
            var answer2 = $get("<%=txtAnswer2.ClientID%>");
            var question3 = $get("<%=ddlQuestion3.ClientID%>");
            var answer3 = $get("<%=txtAnswer3.ClientID%>");

            var userHostAddress = '<%=userHostAddress%>';
            var appName = '<%=appName%>';

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
                alert("New password must not contain or not equal to user name.");
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
                alert("New password and old password must not be equal.");
                newpass.value = "";
                verpass.value = "";
                newpass.focus();
                return false;
            }


            if (question1.selectedIndex < 0) {
                alert("Please specify 1st question.");
                question1.focus();
                return false;
            }
            if (answer1.value == "") {
                alert("Please specify the 1st answer.");
                answer1.focus();
                return false;
            }
            else if (answer1.value.length >= 1) {
                retvOk = checkText(answer1, 'checkAns');

                if (retvOk[0] == true) {
                    alert(retvOk[1]);
                    return;
                }
            }
            if (question2.selectedIndex < 0) {
                alert("Please specify 2nd question.");
                question2.focus();
                return false;
            }
            if (answer2.value == "") {
                alert("Please specify the 2nd answer.");
                answer2.focus();
                return false;
            }
            else if (answer2.value.length >= 1) {
                retvOk = checkText(answer2, 'checkAns');

                if (retvOk[0] == true) {
                    alert(retvOk[1]);
                    return;
                }
            }
            if (question2.selectedIndex == question1.selectedIndex) {
                alert("Repeating questions are not allowed.");
                question2.focus();
                return false;
            }
            if (question3.selectedIndex < 0) {
                alert("Please specify 3rd question.");
                question3.focus();
                return false;
            }
            if (answer3.value == "") {
                alert("Please specify the 3rd answer.");
                answer3.focus();
                return false;
            }
            else if (answer3.value.length >= 1) {
                retvOk = checkText(answer3, 'checkAns');

                if (retvOk[0] == true) {
                    alert(retvOk[1]);
                    return;
                }
            }
            if (question3.selectedIndex == question1.selectedIndex || question3.selectedIndex == question2.selectedIndex) {
                alert("Repeating questions are not allowed.");
                question3.focus();
                return false;
            }


            $find('<%=mdlPopup.ClientID%>').show();
            PageMethods.FirstLogonValidation(userid, oldpass.value, newpass.value, question1.options[question1.selectedIndex].text, question2.options[question2.selectedIndex].text,
                                                                question3.options[question3.selectedIndex].text, answer1.value, answer2.value, answer3.value, userHostAddress, appName, email,
                                                                OnSucceeded, ErrorHandler, TimeOutHandler);
        }

        function checkText(obj, type) {
            var schar = " !@#$%^&*()+=-[]\\\';,/{}|\":<>?~_";

            var validFullName = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ,.&/()\'-";
            var validDesc = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 !@#$%^&*()+=-[]\\\';.,/{}|\":<>?~_";
            var validEmail = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789._-@";
            var validAns = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 ,.&/()\'-";

            var validentry;
            var vOk;
            var msg;
            var retval;

            if (type == 'checkFullName') {
                validentry = validFullName
            }
            if (type == 'checkDesc') {
                validentry = validDesc
            }
            if (type == 'checkEmail') {
                validentry = validEmail
            }
            if (type == 'checkAns') {
                validentry = validAns
            }

            if (schar.indexOf(obj.value.charAt(0)) != -1) {
                msg = "First character is invalid. Special character(s) not allowed.";
                obj.focus();
                retval = true;
                return [retval, msg];
            }
            else if (schar.indexOf(obj.value.substring(obj.value.length - 1)) != -1) {
                msg = "Last character is invalid. Special character(s) not allowed.";
                obj.focus();
                retval = true;
                return [retval, msg];
            }
            else {
                for (var i = 0; i <= obj.value.length - 1; i++) {
                    vOk = false
                    for (var n = 0; n <= validentry.length - 1; n++) {
                        if (obj.value.charAt(i) == validentry.charAt(n)) {
                            vOk = true;
                        }
                    }
                    if (vOk == false) {
                        msg = "Invalid entry. Please try again.";
                        obj.focus();
                        retval = true;
                        return [retval, msg];
                    }
                }
            }

            return [false, ""];
        }

        function OnSucceeded(result) {
            $find('<%=mdlPopup.ClientID%>').hide();

            if (result) {
                alert("Details are successfully saved.");
                window.location.href = "SessionAbandon.aspx";
            }
            else {
                alert("Password had already been used. Please choose a different password.");
                location.replace("FirstLogon.aspx");
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

        function BackToLogin() {
            location.replace("Login.aspx");
        }
        
    </script>
    <div class="mainDiv">
        <h2 class="header">
            <span>First Log On </span>
        </h2>
        <asp:UpdatePanel ID="UpdatePanel" runat="server">
            <ContentTemplate>
                <div id="firstlogonDiv" class="contentDiv">
                    <span style="font-weight: bold; font-size: 12px; font-family: Tahoma">Fill Up Information</span>
                    <hr style="width: 540px; color: #a7a26a" />
                    <table style="margin-top: 5px" width="90%">
                        <tbody>
                            <tr style="height: 28px">
                                <td style="width: 180px" align="right">
                                    <asp:Label ID="Label1" runat="server" Text="User ID:" Font-Size="11px" Font-Bold="True"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblUserID" runat="server" ForeColor="#787878" Font-Size="11px" Font-Bold="True"
                                        Font-Italic="True"></asp:Label>
                                    <asp:Label ID="lblEmail" runat="server" ForeColor="White" Font-Size="11px" Font-Bold="True"
                                        Font-Italic="True" Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <tr style="height: 40px">
                                <td style="width: 180px" align="right">
                                    <asp:Label ID="lblOldPassword" runat="server" Text="Old password:" Font-Bold="true"></asp:Label>
                                    <br />
                                    <br />
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtOldPassword" runat="server" BorderColor="#106A04" Width="197px"
                                        Font-Size="11px" BorderWidth="1px" BorderStyle="Solid" TextMode="Password" onchange="OnChange()"
                                        MaxLength="20"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="Label3" runat="server" Text="Enter your old password to enable encoding of the new password."
                                        ForeColor="DarkGray"></asp:Label>
                                </td>
                            </tr>
                            <tr style="height: 60px">
                                <td style="width: 180px" align="right">
                                    <asp:Label ID="lblNewPassword" runat="server" Text="New password:" Font-Bold="true"></asp:Label>
                                    <br />
                                    <br />
                                    <br />
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtNewPassword" onfocus="this.select()" TabIndex="1" runat="server"
                                        BorderColor="#106A04" Width="197px" Font-Size="11px" BorderWidth="1px" BorderStyle="Solid"
                                        TextMode="Password" MaxLength="20"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="Label2" runat="server" Text="Minimum of eight characters in length with no spaces."
                                        ForeColor="DarkGray"></asp:Label>
                                    <br />
                                    <asp:LinkButton ID="lbRules" TabIndex="4" runat="server" Text="Password Complexity Requirements"
                                        ForeColor="#0078D4" OnClientClick="return ShowRules()"></asp:LinkButton>
                                </td>
                            </tr>
                            <tr style="height: 28px">
                                <td style="width: 180px" align="right">
                                    <asp:Label ID="lblRetypePassword" runat="server" Text="Retype new password:" Font-Bold="True"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtRetypePassword" onfocus="this.select()" TabIndex="2" runat="server"
                                        BorderColor="#106A04" Width="197px" Font-Size="11px" BorderWidth="1px" BorderStyle="Solid"
                                        TextMode="Password" MaxLength="20"></asp:TextBox>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                    <hr style="width: 540px; color: #a7a26a" />
                    <table style="margin-top: 15px; width: 90%">
                        <tbody>
                            <tr style="height: 30px">
                                <td style="font-weight: bold; font-size: 12px; text-align: center" align="center"
                                    colspan="2">
                                    Security Questions
                                </td>
                            </tr>
                            <tr style="height: 28px">
                                <td style="width: 180px" align="right">
                                    <asp:Label ID="Label4" runat="server" Text="1st Question:" Font-Bold="true"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlQuestion1" runat="server" Width="276px" Font-Size="11px"
                                        Font-Names="Verdana">
                                        <asp:ListItem Value="Where did you meet your spouse?">Where did you meet your spouse?</asp:ListItem>
                                        <asp:ListItem Value="What was the name of your first school?">What was the name of your first school?</asp:ListItem>
                                        <asp:ListItem Value="Who was your favorite childhood superhero?">Who was your favorite childhood superhero?</asp:ListItem>
                                        <asp:ListItem Value="What is your favorite pastime?">What is your favorite pastime?</asp:ListItem>
                                        <asp:ListItem Value="What is your favorite sports team?">What is your favorite sports team?</asp:ListItem>
                                        <asp:ListItem Value="What is your mother's maiden name?">What is your mother's maiden name?</asp:ListItem>
                                        <asp:ListItem Value="What is your pet's name?">What is your pet's name?</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="height: 28px">
                                <td style="width: 180px" align="right">
                                    <asp:Label ID="Label5" runat="server" Text="1st Answer:" Font-Bold="true"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtAnswer1" onfocus="this.select()" runat="server" BorderColor="#106A04"
                                        Width="270px" Font-Size="11px" BorderWidth="1px" BorderStyle="Solid" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="height: 28px">
                                <td style="width: 180px" align="right">
                                    <asp:Label ID="Label6" runat="server" Text="2nd Question:" Font-Bold="true"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlQuestion2" runat="server" Width="276px" Font-Size="11px"
                                        Font-Names="Verdana">
                                        <asp:ListItem Value="Where did you meet your spouse?">Where did you meet your spouse?</asp:ListItem>
                                        <asp:ListItem Value="What was the name of your first school?">What was the name of your first school?</asp:ListItem>
                                        <asp:ListItem Value="Who was your favorite childhood superhero?">Who was your favorite childhood superhero?</asp:ListItem>
                                        <asp:ListItem Value="What is your favorite pastime?">What is your favorite pastime?</asp:ListItem>
                                        <asp:ListItem Value="What is your favorite sports team?">What is your favorite sports team?</asp:ListItem>
                                        <asp:ListItem Value="What is your mother's maiden name?">What is your mother's maiden name?</asp:ListItem>
                                        <asp:ListItem Value="What is your pet's name?">What is your pet's name?</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="height: 28px">
                                <td style="width: 180px" align="right">
                                    <asp:Label ID="Label7" runat="server" Text="2nd Answer:" Font-Bold="true"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtAnswer2" onfocus="this.select()" runat="server" BorderColor="#106A04"
                                        Width="270px" Font-Size="11px" BorderWidth="1px" BorderStyle="Solid" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="height: 28px">
                                <td style="width: 180px" align="right">
                                    <asp:Label ID="Label8" runat="server" Text="3rd Question:" Font-Bold="true"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlQuestion3" runat="server" Width="276px" Font-Size="11px"
                                        Font-Names="Verdana">
                                        <asp:ListItem Value="Where did you meet your spouse?">Where did you meet your spouse?</asp:ListItem>
                                        <asp:ListItem Value="What was the name of your first school?">What was the name of your first school?</asp:ListItem>
                                        <asp:ListItem Value="Who was your favorite childhood superhero?">Who was your favorite childhood superhero?</asp:ListItem>
                                        <asp:ListItem Value="What is your favorite pastime?">What is your favorite pastime?</asp:ListItem>
                                        <asp:ListItem Value="What is your favorite sports team?">What is your favorite sports team?</asp:ListItem>
                                        <asp:ListItem Value="What is your mother's maiden name?">What is your mother's maiden name?</asp:ListItem>
                                        <asp:ListItem Value="What is your pet's name?">What is your pet's name?</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="height: 28px">
                                <td style="width: 180px" align="right">
                                    <asp:Label ID="Label9" runat="server" Text="3rd Answer:" Font-Bold="true"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtAnswer3" onfocus="this.select()" runat="server" BorderColor="#106A04"
                                        Width="270px" Font-Size="11px" BorderWidth="1px" BorderStyle="Solid" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                    <hr style="width: 540px; color: #a7a26a" />
                    &nbsp;<br />
                    <div style="padding-right: 5px; padding-left: 10px; padding-bottom: 5px; padding-top: 2px">
                        <input style="margin-top: 10px; margin-left: 175px" class="ButtonStyle" id="btnSave"
                            onclick="Validate()" type="button" value="Save" />
                        <input style="margin-top: 10px; margin-left: 10px" class="ButtonStyle" id="btnCancel"
                            onclick="BackToLogin()" type="button" value="Cancel" />
                    </div>
                    <%--Progress bar Modal Popup--%>
                    <ajaxToolKit:ModalPopupExtender ID="mdlPopup" runat="server" TargetControlID="pnlProgress"
                        PopupControlID="pnlProgress" BackgroundCssClass="modalBackground" DropShadow="true">
                    </ajaxToolKit:ModalPopupExtender>
                    <asp:Panel Style="display: none; background-color: #ffffff" ID="pnlProgress" runat="server"
                        Width="290px">
                        <div style="padding-right: 8px; padding-left: 8px; padding-bottom: 8px; padding-top: 8px">
                            <table style="width: 100%" cellspacing="0" cellpadding="2" border="0">
                                <tbody>
                                    <tr>
                                        <td style="width: 50%">
                                        </td>
                                        <td style="text-align: right">
                                            <img alt="" src="img/indicator-big.gif" />
                                        </td>
                                        <td style="white-space: nowrap; text-align: left">
                                            <span style="font-size: larger; font-family: Tahoma">Saving changes, please wait ...</span>
                                        </td>
                                        <td style="width: 50%">
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
