<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="MaintenanceConferenceRoom.aspx.cs" Inherits="Room" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager" EnablePageMethods="true" runat="server" />
    <script language="javascript" type="text/javascript">
        var TotalRows;
        var roomRowIndex;
        // ================================================================================================================================================
        function GetRoomRowIndex(rowindex) {
            roomRowIndex = rowindex;
        }
        // ================================================================================================================================================
        function ConfirmDelete() {
            if ($get("<%=roomGridView.ClientID%>") != null) {
                if (roomRowIndex != null) {
                    var grid = $get('<%=roomGridView.ClientID%>');
                    var selectedRoomID = grid.rows[roomRowIndex].cells[1].innerText;
                    var selectedLocationID = grid.rows[roomRowIndex].cells[2].innerText;
                    var selectedLocationName = grid.rows[roomRowIndex].cells[3].innerText;
                    var selectedRoomCode = grid.rows[roomRowIndex].cells[4].innerText;
                    var selectedRoomName = grid.rows[roomRowIndex].cells[5].innerText;
                    var selectedRoomDesc = grid.rows[roomRowIndex].cells[6].innerText;
                    var selectedRecordDetails = selectedRoomCode + '-' + selectedRoomName;
                    var macAddress = $get("<%=macAddressHiddenField.ClientID%>");

                    if (confirm("Delete room: " + selectedRecordDetails + "?")) {
                        PageMethods.deleteRoom(3, parseInt(selectedRoomID), parseInt(selectedLocationID), selectedRoomCode, selectedRoomName,
                                                selectedRoomDesc, 0, true, macAddress.value, OnDeleted);
                    }
                    else {
                        location.replace("MaintenanceConferenceRoom.aspx");
                    }
                }
                else {
                    alert("Please select a record to delete.");
                }
            }
            else {
                alert("There is no record to delete.");
            }
        }
        // ================================================================================================================================================
        function OnDeleted(result, context, methodname) {
            if (result) {
                if (methodname == "deleteRoom") {
                    alert("Room is successfully deleted.");
                    location.replace("MaintenanceConferenceRoom.aspx");
                }
            }
            else {
                alert('Delete not possible. Existing reference found.');
            }
        }
        // ================================================================================================================================================
        function RoomTransaction(transactionID) {
            var transaction;

            if (transactionID == 1) {
                location.replace("MaintenanceConferenceRoomTran.aspx?Param=A");
            }
            else {
                if ($get("<%=roomGridView.ClientID%>") != null) {
                    if (roomRowIndex != null) {
                        var grid = $get('<%=roomGridView.ClientID%>');
                        var selectedRecordNo = grid.rows[roomRowIndex].cells[1].innerText;

                        if (transactionID == 2) {
                            transaction = "E";
                        }
                        else {
                            transaction = "V";
                        }

                        location.replace("MaintenanceConferenceRoomTran.aspx?Param=" + transaction + selectedRecordNo + "");
                    }
                    else {
                        if (transactionID == 2) {
                            alert("Please select a record to edit.");
                        }
                        else {
                            alert("Please select a record to view.");
                        }
                    }
                }
                else {
                    if (transactionID == 2) {
                        alert("There is no record to edit.");
                    }
                    else {
                        alert("There is no record to view.");
                    }
                }
            }
        }
        // ================================================================================================================================================    
    </script>
    <div id="room" class="mainDiv">
        <h2 class="header">
            <span>Conference Room </span>
        </h2>
        <asp:UpdatePanel ID="UpdatePanel" runat="server">
            <ContentTemplate>
                <div class="contentDiv">
                    <div class="search_wrapper">
                        <table>
                            <tr>
                                <td style="width: 80px">
                                    <strong>Location :</strong>
                                </td>
                                <td style="width: 175px">
                                    <asp:TextBox ID="paramLocationTextBox" runat="server" Width="165px" CssClass="TextBoxStyle"
                                        MaxLength="50"></asp:TextBox>
                                </td>
                                <td style="width: 10px">
                                </td>
                                <td style="width: 80px">
                                    <strong>Code :</strong>
                                </td>
                                <td style="width: 175px">
                                    <asp:TextBox ID="paramCodeTextBox" runat="server" Width="165px" CssClass="TextBoxStyle"
                                        MaxLength="50"></asp:TextBox>
                                </td>
                                <td style="width: 10px">
                                </td>
                                <td style="width: 110px; text-align: right">
                                    <asp:Button ID="searchButton" runat="server" Text="Search" CssClass="ButtonStyle"
                                        OnClick="searchButton_Click"></asp:Button>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <strong>Name :</strong>
                                </td>
                                <td>
                                    <asp:TextBox ID="paramNameTextBox" runat="server" Width="165px" CssClass="TextBoxStyle"
                                        MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="gridview_wrapper">
                        <asp:GridView ID="roomGridView" runat="server" OnPageIndexChanging="roomGridView_PageIndexChanging"
                            PageSize="5" DataKeyNames="RoomID" OnRowDataBound="roomGridView_RowDataBound"
                            OnDataBound="roomGridView_DataBound" AllowPaging="True" EnableViewState="False"
                            AllowSorting="True" AutoGenerateColumns="False" OnRowCreated="roomGridView_RowCreated">
                            <RowStyle BackColor="#EFF3FB" Wrap="true"></RowStyle>
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" ButtonType="button">
                                    <ItemStyle CssClass="gridviewselect" />
                                </asp:CommandField>
                                <asp:BoundField DataField="RoomID" HeaderText="Room ID">
                                    <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                                    <ItemStyle CssClass="hiddencol"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="LocationID" HeaderText="Location ID">
                                    <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                                    <ItemStyle CssClass="hiddencol"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="LocationName" HeaderText="Location"></asp:BoundField>
                                <asp:BoundField DataField="RoomCode" HeaderText="Room Code"></asp:BoundField>
                                <asp:BoundField DataField="RoomName" HeaderText="Room Name"></asp:BoundField>
                                <asp:BoundField DataField="RoomDesc" HeaderText="Room Details"></asp:BoundField>
                            </Columns>
                            <PagerTemplate>
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/img/first.gif" AlternateText="Go to first page"
                                    CommandArgument="First" CommandName="Page" Style="vertical-align: middle" />
                                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/img/prev.gif" AlternateText="Go to previous page"
                                    CommandArgument="Prev" CommandName="Page" Style="vertical-align: middle" />
                                Page &nbsp
                                <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged"
                                    Font-Names="Tahoma" Font-Size="11px">
                                </asp:DropDownList>
                                &nbsp of &nbsp
                                <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/img/next.gif" AlternateText="Go to next page"
                                    CommandArgument="Next" CommandName="Page" Style="vertical-align: middle" />
                                <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/img/last.gif" AlternateText="Go to last page"
                                    CommandArgument="Last" CommandName="Page" Style="vertical-align: middle" />
                            </PagerTemplate>
                            <PagerStyle CssClass="tablefooter"></PagerStyle>
                            <AlternatingRowStyle BackColor="#FFFFFF"></AlternatingRowStyle>
                            <SelectedRowStyle BackColor="DarkOrange" />
                        </asp:GridView>
                    </div>
                    <div class="button_wrapper">
                        <table>
                            <tbody>
                                <tr>
                                    <td>
                                        <input id="addButton" class="ButtonStyle" type="button" value="Add" runat="server"
                                            onclick="javascript:RoomTransaction(1)" />
                                        <input id="editButton" class="ButtonStyle" type="button" value="Edit" runat="server"
                                            onclick="javascript:RoomTransaction(2)" />
                                        <input id="deleteButton" class="ButtonStyle" type="button" value="Delete" runat="server"
                                            onclick="javascript:ConfirmDelete()" />
                                        <input id="viewButton" class="ButtonStyle" type="button" value="View" runat="server"
                                            onclick="javascript:RoomTransaction(3)" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <asp:HiddenField ID="macAddressHiddenField" runat="server"></asp:HiddenField>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Button ID="postBackButton" runat="server" Text="Delete" CssClass="ButtonStyle"
            Visible="false" Style="display: none" />
    </div>
</asp:Content>
