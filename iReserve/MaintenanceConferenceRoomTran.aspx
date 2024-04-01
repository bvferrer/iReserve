<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="MaintenanceConferenceRoomTran.aspx.cs" Inherits="MaintenanceRoomTran" %>

<%@ Register Assembly="eWorld.UI, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
    Namespace="eWorld.UI" TagPrefix="ew" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager" EnablePageMethods="true" runat="server" />
    <script language="javascript" type="text/javascript">
        // =============================================================================================
        function ConfirmNavigationMsgBox(statusID) {
            var status = '<%=status%>';
            var roomLocation = $get('<%=locationDropDownList.ClientID%>');
            var roomCode = $get("<%=roomCodeTextBox.ClientID%>");
            var roomName = $get("<%=roomNameTextBox.ClientID%>");
            var roomDesc = $get("<%=roomDescriptionTextBox.ClientID%>");
            var maxPerson = $get("<%=maxPersonNumericBox.ClientID%>");
            var dataPort = $get("<%=dataPortCheckBox.ClientID%>");
            var monitor = $get("<%=monitorCheckBox.ClientID%>");
            var rate = $get("<%=rateNumericBox.ClientID%>");
            var tablet = $get("<%=tabletIDTextBox.ClientID%>");
            var monitorDisplay = $get("<%=monitorDisplayDropDownList.ClientID%>");

            var savingButton = $get("<%=saveButton.ClientID%>");

            if (status != "V") {
                if (roomLocation.value != 0 || roomCode.value != "" || roomName.value != "" || roomDesc.value != "" || maxPerson.value != "" || rate.value != "" || tablet.value != "") {
                    if (savingButton.disabled == false) {
                        if (confirm("The changes you have made will not be saved. Are you sure you want to leave this page?")) {
                            location.replace("MaintenanceConferenceRoom.aspx");
                        }
                    }
                    else {
                        location.replace("MaintenanceConferenceRoom.aspx");
                    }
                }
                else {
                    location.replace("MaintenanceConferenceRoom.aspx");
                }
            }
            else {
                location.replace("MaintenanceConferenceRoom.aspx");
            }
        }
        // =============================================================================================
        function CheckInputs() {
            var retvOk;
            var roomLocation = $get("<%=locationDropDownList.ClientID%>");
            var roomCode = $get("<%=roomCodeTextBox.ClientID%>");
            var roomName = $get("<%=roomNameTextBox.ClientID%>");
            var roomDesc = $get("<%=roomDescriptionTextBox.ClientID%>");
            var maxPerson = $get("<%=maxPersonNumericBox.ClientID%>");

            if (roomCode.value == "") {
                alert("Code is required.");
                roomCode.focus();
                return false;
            }
            if (roomCode.value != "") {
                if (roomCode.value.charAt(0) == " ") {
                    alert("First character on the Code field should not contain white spaces.");
                    roomCode.focus();
                    return false;
                }
                if (roomCode.value.charAt(roomCode.value.length - 1) == " ") {
                    alert("Last character on the Code field should not contain white spaces.");
                    roomCode.focus();
                    return false;
                }
                if (roomCode.value.length >= 1) {
                    retvOk = ValidateInputs(roomCode);
                    if (retvOk[0] == true) {
                        alert(retvOk[1]);
                        roomCode.focus();
                        return false;
                    }
                }
                if (roomCode.value.length < 4) {
                    alert("Entry on the Code field should be exactly four(4) characters long.");
                    roomCode.focus();
                    return false;
                }
            }

            if (roomName.value == "") {
                alert("Name is required.");
                roomName.focus();
                return false;
            }
            if (roomName.value != "") {
                if (roomName.value.charAt(0) == " ") {
                    alert("First character on the Name field should not contain white spaces.");
                    roomName.focus();
                    return false;
                }
                if (roomName.value.charAt(roomName.value.length - 1) == " ") {
                    alert("Last character on the Name field should not contain white spaces.");
                    roomName.focus();
                    return false;
                }
                if (roomName.value.length >= 1) {
                    retvOk = ValidateInputs(roomName);
                    if (retvOk[0] == true) {
                        alert(retvOk[1]);
                        roomName.focus();
                        return false;
                    }
                }
            }

            if (roomDesc.value != "") {
                if (roomDesc.value.charAt(0) == " ") {
                    alert("First character on the Details field should not contain white spaces.");
                    roomDesc.focus();
                    return false;
                }
                if (roomDesc.value.charAt(roomDesc.value.length - 1) == " ") {
                    alert("Last character on the Details field should not contain white spaces.");
                    roomDesc.focus();
                    return false;
                }
                if (roomDesc.value.length > 500) {
                    alert("Details field is up to 500 characters only.");
                    roomDesc.focus();
                    return false;
                }
                if (roomDesc.value.length >= 1) {
                    retvOk = ValidateInputs(roomDesc);
                    if (retvOk[0] == true) {
                        alert(retvOk[1]);
                        roomDesc.focus();
                        return false;
                    }
                }
            }

            if (roomLocation.value == 0) {
                alert("Location is required.");
                roomLocation.focus();
                return false;
            }

            if (maxPerson.value == "") {
                alert("Max Person is required.");
                maxPerson.focus();
                return false;
            }

            if (maxPerson.value == "0" || maxPerson.value == "00" || maxPerson.value == "000") {
                alert("Max Person should not be zero.");
                maxPerson.focus();
                return false;
            }
        }
        // =============================================================================================
        function ValidateInputs(obj) {
            var validentry;
            var vOk;
            var msg;
            var retval;

            if (obj == $get("<%=roomCodeTextBox.ClientID%>")) {
                validentry = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            }
            else if (obj == $get('<%=roomNameTextBox.ClientID%>')) {
                validentry = " ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@&()-:\',.?/";
            }
            else if (obj == $get('<%=roomDescriptionTextBox.ClientID%>')) {
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
                    if (obj == $get("<%=roomCodeTextBox.ClientID%>")) {
                        msg = "Code field only accepts alphanumeric characters.";
                    }
                    else if (obj == $get('<%=roomNameTextBox.ClientID%>')) {
                        msg = "Name field does not accept special characters except for the following: !@&()-:\',.?/";
                    }
                    else if (obj == $get('<%=roomDescriptionTextBox.ClientID%>')) {
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
            var roomLocation = $get('<%=locationDropDownList.ClientID%>');
            var roomCode = $get("<%=roomCodeTextBox.ClientID%>");
            var roomName = $get("<%=roomNameTextBox.ClientID%>");
            var roomDesc = $get("<%=roomDescriptionTextBox.ClientID%>");
            var maxPerson = $get("<%=maxPersonNumericBox.ClientID%>");
            var dataPort = $get("<%=dataPortCheckBox.ClientID%>");
            var monitor = $get("<%=monitorCheckBox.ClientID%>");
            var ratePerHour = $get("<%=rateNumericBox.ClientID%>");
            var tablet = $get("<%=tabletIDTextBox.ClientID%>");
            var monitorDisplay = $get("<%=monitorDisplayDropDownList.ClientID%>");
            var savingButton = $get("<%=saveButton.ClientID%>");

            if (typeID == 1) {
                if (confirm("Room " + roomCode.value + "-" + roomName.value + " is successfully saved. Do you want to add another record?")) {
                    location.replace("MaintenanceConferenceRoomTran.aspx?Param=A");
                }
                else {
                    location.replace("MaintenanceConferenceRoom.aspx");
                }
            }
            else {
                roomLocation.disabled = true;
                roomCode.disabled = true;
                roomName.disabled = true;
                roomDesc.disabled = true;
                maxPerson.disabled = true;
                dataPort.disabled = true;
                monitor.disabled = true;
                ratePerHour.disabled = true;
                tablet.disabled = true;
                monitorDisplay.disabled = true;
                savingButton.disabled = true;
                alert("Room " + roomCode.value + "-" + roomName.value + " is successfully updated.");
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
    <div id="room" class="mainDiv">
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
                            <asp:TextBox ID="roomCodeTextBox" runat="server" MaxLength="4" CssClass="TextBoxStyleCode"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Name :</strong>
                        </td>
                        <td>
                            <asp:TextBox ID="roomNameTextBox" runat="server" Width="450px" MaxLength="30" CssClass="TextBoxStyle"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Location :</strong>
                        </td>
                        <td>
                            <asp:DropDownList ID="locationDropDownList" runat="server" CssClass="DropDownStyle"
                                Width="456px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Details :</strong>
                        </td>
                        <td>
                            <asp:TextBox Rows="5" Columns="80" ID="roomDescriptionTextBox" MaxLength="500" TextMode="MultiLine"
                                runat="server" CssClass="TextBoxStyle" Width="450px"></asp:TextBox>
                            <%--<br />
                            <asp:TextBox ID="txtCounter" runat="server" Style="border: none" CssClass="TextBoxStyle"
                                MaxLength="3" Width="18px" Font-Names="Tahoma" Font-Size="8pt" ForeColor="Gray"></asp:TextBox>
                            <span style="font-size: 8pt; color: gray">characters remaining</span>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Max Person :</strong>
                        </td>
                        <td>
                            <ew:NumericBox ID="maxPersonNumericBox" runat="server" MaxLength="3" PositiveNumber="true"
                                DecimalPlaces="0" CssClass="TextBoxStyle"></ew:NumericBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Equipment :</strong>
                        </td>
                        <td>
                            <asp:CheckBox ID="dataPortCheckBox" runat="server" Text="Data Port/Wifi" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:CheckBox ID="monitorCheckBox" runat="server" Text="TV/Monitor" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Rate per Hour :</strong>
                        </td>
                        <td>
                            <ew:NumericBox ID="rateNumericBox" runat="server" Width="150px" MaxLength="10" PositiveNumber="true"
                                DecimalPlaces="2" CssClass="TextBoxStyle"></ew:NumericBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Assigned Tablet ID :</strong>
                        </td>
                        <td>
                            <asp:TextBox ID="tabletIDTextBox" runat="server" Width="450px" MaxLength="100" CssClass="TextBoxStyle"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Monitor Display :</strong>
                        </td>
                        <td>
                            <asp:DropDownList ID="monitorDisplayDropDownList" runat="server" CssClass="DropDownStyle"
                                Width="456px">
                            </asp:DropDownList>
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
                <asp:HiddenField ID="macAddressHiddenField" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>
