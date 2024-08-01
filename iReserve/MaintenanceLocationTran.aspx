<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="MaintenanceLocationTran.aspx.cs" Inherits="MaintenanceLocationTran" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager" EnablePageMethods="true" runat="server" />
    <script language="javascript" type="text/javascript">
        // =============================================================================================        
        function ConfirmNavigationMsgBox(statusID) {
            var status = '<%=status%>';
            var locationCode = $get("<%=locationCodeTextBox.ClientID%>");
            var locationName = $get("<%=locationNameTextBox.ClientID%>");
            var locationDesc = $get("<%=locationDescriptionTextBox.ClientID%>");
            var savingButton = $get("<%=saveButton.ClientID%>");

            if (status != "V") {
                if (locationCode.value != "" || locationName.value != "" || locationDesc.value != "") {
                    if (savingButton.disabled == false) {
                        if (confirm("The changes you have made will not be saved. Are you sure you want to leave this page?")) {
                            location.replace("MaintenanceLocation.aspx");
                        }
                    }
                    else {
                        location.replace("MaintenanceLocation.aspx");
                    }
                }
                else {
                    location.replace("MaintenanceLocation.aspx");
                }
            }
            else {
                location.replace("MaintenanceLocation.aspx");
            }
        }
        // =============================================================================================   
        function CheckInputs() {
            var retvOk;
            var locationCode = $get("<%=locationCodeTextBox.ClientID%>");
            var locationName = $get("<%=locationNameTextBox.ClientID%>");
            var locationDesc = $get("<%=locationDescriptionTextBox.ClientID%>");
            var status = '<%=status%>';

            if (locationCode.value == "") {
                alert("Code is required.");
                locationCode.focus();
                return false;
            }
            if (locationCode.value != "") {
                if (locationCode.value.charAt(0) == " ") {
                    alert("First character on the Code field should not contain white spaces.");
                    locationCode.focus();
                    return false;
                }
                if (locationCode.value.charAt(locationCode.value.length - 1) == " ") {
                    alert("Last character on the Code field should not contain white spaces.");
                    locationCode.focus();
                    return false;
                }
                if (locationCode.value.length >= 1) {
                    retvOk = ValidateInputs(locationCode);
                    if (retvOk[0] == true) {
                        alert(retvOk[1]);
                        locationCode.focus();
                        return false;
                    }
                }
                if (locationCode.value.length < 4) {
                    alert("Entry on the Code field should be exactly four(4) characters long.");
                    locationCode.focus();
                    return false;
                }
            }

            if (locationName.value == "") {
                alert("Name is required.");
                locationName.focus();
                return false;
            }
            if (locationName.value != "") {
                if (locationName.value.charAt(0) == " ") {
                    alert("First character on the Name field should not contain white spaces.");
                    locationName.focus();
                    return false;
                }
                if (locationName.value.charAt(locationName.value.length - 1) == " ") {
                    alert("Last character on the Name field should not contain white spaces.");
                    locationName.focus();
                    return false;
                }
                if (locationName.value.length >= 1) {
                    retvOk = ValidateInputs(locationName);
                    if (retvOk[0] == true) {
                        alert(retvOk[1]);
                        locationName.focus();
                        return false;
                    }
                }
            }

            if (locationDesc.value != "") {
                if (locationDesc.value.charAt(0) == " ") {
                    alert("First character on the Details field should not contain white spaces.");
                    locationDesc.focus();
                    return false;
                }
                if (locationDesc.value.charAt(locationDesc.value.length - 1) == " ") {
                    alert("Last character on the Details field should not contain white spaces.");
                    locationDesc.focus();
                    return false;
                }
                if (locationDesc.value.length > 500) {
                    alert("Details field is up to 500 characters only.");
                    locationDesc.focus();
                    return false;
                }
                if (locationDesc.value.length >= 1) {
                    retvOk = ValidateInputs(locationDesc);
                    if (retvOk[0] == true) {
                        alert(retvOk[1]);
                        locationDesc.focus();
                        return false;
                    }
                }
            }
        }
        // =============================================================================================   
        function ValidateInputs(obj) {
            var validentry;
            var vOk;
            var msg;
            var retval;

            if (obj == $get("<%=locationCodeTextBox.ClientID%>")) {
                validentry = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            }
            else if (obj == $get('<%=locationNameTextBox.ClientID%>')) {
                validentry = " ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@&()-:\',.?/";
            }
            else if (obj == $get('<%=locationDescriptionTextBox.ClientID%>')) {
                validentry = " ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$&()-_+=[]{}:;\'<>,.?/\r\n";
            }

            for (var i = 0; i <= obj.value.length - 1; i++) {
                vOk = false
                for (var n = 0; n <= validentry.length - 1; n++) {
                    if (obj.value.charAt(i) == validentry.charAt(n)) {
                        vOk = true;
                    }
                }
                if (vOk == false) {
                    if (obj == $get("<%=locationCodeTextBox.ClientID%>")) {
                        msg = "Code field only accepts alphanumeric characters.";
                    }
                    else if (obj == $get('<%=locationNameTextBox.ClientID%>')) {
                        msg = "Name field does not accept special characters except for the following: !@&()-:\',.?/";
                    }
                    else if (obj == $get('<%=locationDescriptionTextBox.ClientID%>')) {
                        msg = "Details field does not accept special characters except for the following: !@#$&()-_+=[]{}:;\'<>,.?/";
                    }
                    retval = true;
                    return [retval, msg];
                }
            }
            return [false, ""];
        }
        // =============================================================================================
        function OnSucceeded(typeID) {
            var locationCode = $get("<%=locationCodeTextBox.ClientID%>");
            var locationName = $get("<%=locationNameTextBox.ClientID%>");
            var locationDesc = $get("<%=locationDescriptionTextBox.ClientID%>");
            var savingButton = $get("<%=saveButton.ClientID%>");

            if (typeID == 1) {
                if (confirm("Location " + locationCode.value + "-" + locationName.value + " is successfully saved. Do you want to add another record?")) {
                    location.replace("MaintenanceLocationTran.aspx?Param=A");
                }
                else {
                    location.replace("MaintenanceLocation.aspx");
                }
            }
            else {
                locationCode.disabled = true;
                locationName.disabled = true;
                locationDesc.disabled = true;
                savingButton.disabled = true;
                alert("Location " + locationCode.value + "-" + locationName.value + " is successfully updated.");
            }
        }
        // =============================================================================================
        function checkTextAreaMaxLength(textBox, e, length) {

        }
        // =============================================================================================
        function checkSpecialKeys(e) {
            if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
                return false;
            else
                return true;
        }
        
        // =============================================================================================
        function noCopyMouse(e) {
            var isRight = (e.button) ? (e.button == 2) : (e.which == 3);

            if (isRight) {
                return false;
            }
            return true;
        }
        // =============================================================================================
        function noCopyKey(textBox, e, length) {
            var forbiddenKeys = new Array('c', 'x', 'v');
            var keyCode = (e.keyCode) ? e.keyCode : e.which;
            var isCtrl;

            if (window.event)
                isCtrl = e.ctrlKey
            else
                isCtrl = (window.Event) ? ((e.modifiers & Event.CTRL_MASK) == Event.CTRL_MASK) : false;

            if (isCtrl) {
                for (i = 0; i < forbiddenKeys.length; i++) {
                    if (forbiddenKeys[i] == String.fromCharCode(keyCode).toLowerCase()) {
                        return false;
                    }
                }
            }

            var mLen = textBox["MaxLength"];

            if (null == mLen)
                mLen = length;

            var maxLength = parseInt(mLen);

            if (!checkSpecialKeys(e)) {
                if (textBox.value.length > maxLength - 1) {
                    if (window.event) //Browser: INTERNET EXPLORER
                        e.returnValue = false;
                    else //Browser: FIREFOX
                        e.preventDefault();
                }
            }

            return true;
        }
        // ============================================================================================= 
    </script>
    <div id="location" class="mainDiv">
        <h2 class="header">
            <span>
                <asp:Label ID="statusLabel" runat="server"></asp:Label>
            </span>
        </h2>
        <div class="contentDiv">
            <div class="fields_wrapper">
                <table>
                    <tr>
                        <td>
                            <strong>Code :</strong>
                        </td>
                        <td>
                            <asp:TextBox ID="locationCodeTextBox" runat="server" MaxLength="4" CssClass="TextBoxStyleCode"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Name :</strong>
                        </td>
                        <td>
                            <asp:TextBox ID="locationNameTextBox" runat="server" Width="450px" MaxLength="50"
                                CssClass="TextBoxStyle"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Details :</strong>
                        </td>
                        <td>
                            <asp:TextBox Rows="5" Columns="80" ID="locationDescriptionTextBox" MaxLength="500"
                                TextMode="MultiLine" runat="server" CssClass="TextBoxStyle" Width="450px"></asp:TextBox>
                            <%--<br />
                            <asp:TextBox ID="txtCounter" runat="server" Style="border: none" CssClass="TextBoxStyle"
                                MaxLength="3" Width="18px" Font-Names="Tahoma" Font-Size="8pt" ForeColor="Gray"></asp:TextBox>
                            <span style="font-size: 8pt; color: gray">characters remaining</span>--%>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="button_wrapper">
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="saveButton" runat="server" CssClass="ButtonStyle" Text="Save" OnClick="saveButton_Click"
                                OnClientClick="javascript:return CheckInputs()" />
                            <input id="backButton" type="button" value="Back" runat="server" class="ButtonStyle"
                                onclick="javascript:ConfirmNavigationMsgBox()" />
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="macAddressHiddenField" runat="server" />
        </div>
    </div>
</asp:Content>
