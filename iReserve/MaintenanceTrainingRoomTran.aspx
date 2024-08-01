<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="MaintenanceTrainingRoomTran.aspx.cs" Inherits="MaintenanceTrainingRoomTran"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="eWorld.UI, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
    Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
            var noOfPartition = $get("<%=numberOfPartitionNumericBox.ClientID%>");

            var savingButton = $get("<%=saveButton.ClientID%>");

            if (status != "V") {
                if (roomLocation.value != 0 || roomCode.value != "" || roomName.value != "" || roomDesc.value != "" || noOfPartition.value != "") {
                    if (savingButton.disabled == false) {
                        if (confirm("The changes you have made will not be saved. Are you sure you want to leave this page?")) {
                            location.replace("MaintenanceTrainingRoom.aspx");
                        }
                    }
                    else {
                        location.replace("MaintenanceTrainingRoom.aspx");
                    }
                }
                else {
                    location.replace("MaintenanceTrainingRoom.aspx");
                }
            }
            else {
                location.replace("MaintenanceTrainingRoom.aspx");
            }
        }
        // =============================================================================================
        function CheckInputs() {
            var retvOk;
            var roomLocation = $get("<%=locationDropDownList.ClientID%>");
            var roomCode = $get("<%=roomCodeTextBox.ClientID%>");
            var roomName = $get("<%=roomNameTextBox.ClientID%>");
            var roomDesc = $get("<%=roomDescriptionTextBox.ClientID%>");
            var noOfPartition = $get("<%=numberOfPartitionNumericBox.ClientID%>");

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

            if (noOfPartition.value == "") {
                alert("No. of Partition is required.");
                maxPerson.focus();
                return false;
            }

            if (noOfPartition.value == "0" || noOfPartition.value == "00" || noOfPartition.value == "000") {
                alert("No. of Partition should not be zero.");
                maxPerson.focus();
                return false;
            }

            if (!Page_ClientValidate('trainroom')) {
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
            var noOfPartition = $get("<%=numberOfPartitionNumericBox.ClientID%>");
            var upButton = $get("<%=upButton.ClientID%>");
            var downButton = $get("<%=downButton.ClientID%>");
            var partitiongrid = $get("<%=partitionGridView.ClientID%>");
            var rategrid = $get("<%=rateGridView.ClientID%>");
            var savingButton = $get("<%=saveButton.ClientID%>");

            if (typeID == 1) {
                if (confirm("Room " + roomCode.value + "-" + roomName.value + " is successfully saved. Do you want to add another record?")) {
                    location.replace("MaintenanceTrainingRoomTran.aspx?Param=A");
                }
                else {
                    location.replace("MaintenanceTrainingRoom.aspx");
                }
            }
            else {
                roomLocation.disabled = true;
                roomCode.disabled = true;
                roomName.disabled = true;
                roomDesc.disabled = true;
                noOfPartition.disabled = true;
                upButton.disabled = true;
                downButton.disabled = true;
                partitiongrid.disabled = true;
                rategrid.disabled = true;
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

        function isWholeNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
        // =============================================================================================
        //        function NumericDown() {
        //            var obj = $get("<%=numberOfPartitionNumericBox.ClientID%>");
        //            var num = parseFloat(obj.value)
        //            if (isNaN(num)) {
        //                return;
        //            }

        //            if (num == "1") {
        //                return;
        //            }
        //            else {
        //                num -= 1;
        //                obj.value = num;
        //            }
        //        }

        //        function NumericUp() {
        //            var obj = $get("<%=numberOfPartitionNumericBox.ClientID%>");
        //            var num = parseFloat(obj.value);
        //            if (isNaN(num)) {
        //                return;
        //            }

        //            if (num == "5") {
        //                return;
        //            }
        //            else {
        //                num += 1;
        //                obj.value = num;
        //            }
        //        }
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
                            <asp:TextBox ID="roomCodeTextBox" runat="server" MaxLength="4" CssClass="TextBoxStyleCode">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Name :</strong>
                        </td>
                        <td>
                            <asp:TextBox ID="roomNameTextBox" runat="server" Width="450px" MaxLength="50" CssClass="TextBoxStyle">
                            </asp:TextBox>
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
                                runat="server" CssClass="TextBoxStyle" Width="450px">
                            </asp:TextBox>
                            <%--<br />
                            <asp:TextBox ID="txtCounter" runat="server" Style="border: none" CssClass="TextBoxStyle"
                                MaxLength="3" Width="18px" Font-Names="Tahoma" Font-Size="8pt" ForeColor="Gray"></asp:TextBox>
                            <span style="font-size: 8pt; color: gray">characters remaining</span>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Number of Sub Rooms :</strong>
                        </td>
                        <td>
                            <ew:NumericBox ID="numberOfPartitionNumericBox" runat="server" MaxLength="2" PositiveNumber="true"
                                DecimalPlaces="0" CssClass="TextBoxStyle" Width="100px" ReadOnly="true" Enabled="false"></ew:NumericBox>
                            <asp:Button ID="upButton" runat="server" Text="+" CssClass="tranupdownbutton" OnClick="upButton_Click" />
                            <asp:Button ID="downButton" runat="server" Text="-" CssClass="tranupdownbutton" OnClick="downButton_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <table>
                    <tr>
                        <td>
                            <strong>Sub Rooms</strong>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="partitionGridView" runat="server" AutoGenerateColumns="False" 
                                CssClass="subgridview" onrowcreated="partitionGridView_RowCreated">
                                <HeaderStyle BackColor="#507CD1" ForeColor="#FFFFFF" Font-Bold="true" />
                                <RowStyle BackColor="#EFF3FB"></RowStyle>
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="PartitionID" HeaderText="Partition ID">
                                        <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                                        <ItemStyle CssClass="hiddencol"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Sub Room Code">
                                        <ItemTemplate>
                                            <asp:TextBox ID="partitionCodeTextBox" runat="server" MaxLength="4" ValidationGroup="trainroom" Text='<%# Bind("PartitionCode") %>'></asp:TextBox><br />
                                            <asp:RequiredFieldValidator ID="partitionCodeRequiredFieldValidator" runat="server"
                                                SetFocusOnError="true" Display="Dynamic" ControlToValidate="partitionCodeTextBox"
                                                ErrorMessage="Sub Room Code is required." ValidationGroup="trainroom" ForeColor="Red"
                                                Font-Size="10px"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="partitionCodeRegularExpressionValidator" runat="server"
                                                SetFocusOnError="true" Display="Dynamic" ControlToValidate="partitionCodeTextBox"
                                                ValidationExpression="^[0-9A-Za-z]+$" ValidationGroup="trainroom" ErrorMessage="Sub Room Code should not contain special char and spaces."
                                                ForeColor="Red" Font-Size="10px"></asp:RegularExpressionValidator>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sub Room Name">
                                        <ItemTemplate>
                                            <asp:TextBox ID="partitionNameTextBox" runat="server" MaxLength="50" ValidationGroup="trainroom" Text='<%# Bind("PartitionName") %>'></asp:TextBox><br />
                                            <asp:RequiredFieldValidator ID="partitionNameRequiredFieldValidator" runat="server"
                                                SetFocusOnError="true" Display="Dynamic" ControlToValidate="partitionNameTextBox"
                                                ErrorMessage="Sub Room Name is required." ValidationGroup="trainroom" ForeColor="Red"
                                                Font-Size="10px"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="partitionNameRegularExpressionValidator" runat="server"
                                                SetFocusOnError="true" Display="Dynamic" ControlToValidate="partitionNameTextBox"
                                                ValidationExpression="^[0-9A-Za-z\.,\-'\s&)(/]+$" ValidationGroup="trainroom"
                                                ErrorMessage="Sub Room name should not contain special char." ForeColor="Red"
                                                Font-Size="10px"></asp:RegularExpressionValidator>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:TextBox ID="partitionDescriptionTextBox" runat="server" MaxLength="500" ValidationGroup="trainroom" Text='<%# Bind("PartitionDesc") %>'></asp:TextBox><br />
                                            <asp:RegularExpressionValidator ID="partitionDescriptionRegularExpressionValidator"
                                                runat="server" SetFocusOnError="true" Display="Dynamic" ControlToValidate="partitionDescriptionTextBox"
                                                ValidationExpression="^[0-9A-Za-z\.,\-'\s?&)(/]+$" ValidationGroup="trainroom"
                                                ErrorMessage="The only accepted special characters are .,-')(?&/" ForeColor="Red"
                                                Font-Size="10px"></asp:RegularExpressionValidator>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Max Person">
                                        <ItemTemplate>
                                            <ew:NumericBox ID="maxPersonTextBox" runat="server" MaxLength="2" PositiveNumber="true"
                                                DecimalPlaces="0" onkeypress="return isWholeNumber();" ValidationGroup="trainroom" Text='<%# Bind("MaxPerson") %>'></ew:NumericBox><br />
                                            <asp:RequiredFieldValidator ID="maxPersonRequiredFieldValidator" runat="server" SetFocusOnError="true"
                                                Display="Dynamic" ControlToValidate="maxPersonTextBox" ErrorMessage="Max Person is required."
                                                ValidationGroup="trainroom" ForeColor="Red" Font-Size="10px"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="trMaxPersonCompareValidator" runat="server" Type="Integer" SetFocusOnError="true"
                                                Display="Dynamic" ControlToValidate="maxPersonTextBox" ErrorMessage="Max Person should not be zero."
                                                ValueToCompare="0" Operator="GreaterThan" ValidationGroup="trainroom" ForeColor="Red" Font-Size="10px"></asp:CompareValidator>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <br />
                <table>
                    <tr>
                        <td>
                            <strong>Rates</strong>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="rateGridView" runat="server" AutoGenerateColumns="False" 
                                CssClass="rategridview" onrowcreated="rateGridView_RowCreated">
                                <HeaderStyle BackColor="#507CD1" ForeColor="#FFFFFF" Font-Bold="true" />
                                <RowStyle BackColor="#EFF3FB"></RowStyle>
                                <Columns>
                                    <asp:BoundField DataField="RateID" HeaderText="Rate ID">
                                        <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                                        <ItemStyle CssClass="hiddencol"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NumberOfPartition" HeaderText="Number of Sub Rooms"></asp:BoundField>
                                    <asp:TemplateField HeaderText="Rate per Day">
                                        <ItemTemplate>
                                            <ew:NumericBox ID="ratePerDayTextBox" runat="server" MaxLength="8" PositiveNumber="true"
                                                DecimalPlaces="2" ValidationGroup="trainroom" Text='<%# Eval("RatePerDay") %>'></ew:NumericBox><br />
                                            <asp:RequiredFieldValidator ID="ratePerDayRequiredFieldValidator" runat="server"
                                                SetFocusOnError="true" Display="Dynamic" ControlToValidate="ratePerDayTextBox"
                                                ErrorMessage="Rate per Day is required." ValidationGroup="trainroom" ForeColor="Red"
                                                Font-Size="10px"></asp:RequiredFieldValidator>
                                            <asp:RangeValidator ID="ratePerDayRangeValidator" runat="server" Type="Double" SetFocusOnError="true"
                                                Display="Dynamic" ControlToValidate="ratePerDayTextBox" ErrorMessage="Rate per Day should not be zero."
                                                MinimumValue="0.01" MaximumValue="99999999" ValidationGroup="trainroom" ForeColor="Red" Font-Size="10px"></asp:RangeValidator>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Extension Rate per Hour">
                                        <ItemTemplate>
                                            <ew:NumericBox ID="ratePerHourTextBox" runat="server" MaxLength="8" PositiveNumber="true"
                                                DecimalPlaces="2" ValidationGroup="trainroom" Text='<%# Eval("RatePerHour") %>'></ew:NumericBox><br />
                                            <asp:RequiredFieldValidator ID="ratePerHourRequiredFieldValidator" runat="server"
                                                SetFocusOnError="true" Display="Dynamic" ControlToValidate="ratePerHourTextBox"
                                                ErrorMessage="Extension Rate is required." ValidationGroup="trainroom" ForeColor="Red"
                                                Font-Size="10px"></asp:RequiredFieldValidator>
                                            <asp:RangeValidator ID="ratePerHourRangeValidator" runat="server" Type="Double" SetFocusOnError="true"
                                                Display="Dynamic" ControlToValidate="ratePerHourTextBox" ErrorMessage="Extension Rate should not be zero."
                                                MinimumValue="0.01" MaximumValue="99999999" ValidationGroup="trainroom" ForeColor="Red" Font-Size="10px"></asp:RangeValidator>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="button_wrapper">
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="saveButton" runat="server" CssClass="ButtonStyle" Text="Save" OnClick="saveButton_Click"
                                OnClientClick="javascript:return CheckInputs()" ValidationGroup="trainroom" CausesValidation="true" />
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
