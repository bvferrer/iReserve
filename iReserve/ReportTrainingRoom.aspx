<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportTrainingRoom.aspx.cs"
    Inherits="ReportTrainingRoom" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>iReserve</title>
    <link rel="SHORTCUT ICON" href="img/PEL.ico"/>
    <link href="css/Style.css" media="screen" rel="Stylesheet" />
    <script src="js/jquery-1.12.0.js"></script>
    <%--<script src="js/jquery-1.9.1.js" type="text/javascript"></script>--%>
    <%--<script src="js/jquery-ui-1.10.0.custom.js"></script>--%>
</head>
<body class="reportbody">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="calendarUpdatePanel" runat="server">
        <ContentTemplate>
            <div class="reportContainer">
                <div class="innerNav">
                    <span>Training Room Reservation Report</span>
                </div>
                <div>
                    <span>Status :</span>
                    <asp:Label ID="statusLabel" runat="server" Text=""></asp:Label><br />
                    <span>Date From :</span>
                    <asp:Label ID="dateFromLabel" runat="server" Text=""></asp:Label><br />
                    <span>Date To :</span>
                    <asp:Label ID="dateToLabel" runat="server" Text=""></asp:Label>
                </div>
                <div class="reportgridview_wrapper">
                    <asp:GridView ID="reportGridView" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        Font-Names="Tahoma" Font-Size="11px" ForeColor="#333333" EnableViewState="False" EnableModelValidation="True">
                        <RowStyle BackColor="#EFF3FB" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CCRequestReferenceNo" HeaderText="Reference Number" />
                            <asp:BoundField DataField="EventName" HeaderText="Event Name" />
                            <asp:BoundField DataField="TRoomName" HeaderText="Room Name" />
                            <asp:BoundField DataField="NumberOfPartition" HeaderText="Number Of Subrooms" />
                            <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:MMMM d, yyyy}" />
                            <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:MMMM d, yyyy}" />
                            <asp:BoundField DataField="NumberOfDays" HeaderText="Number Of Days" />
                            <asp:BoundField DataField="StatusName" HeaderText="Status" />
                            <asp:BoundField DataField="CostCenterName" HeaderText="Cost Center" />
                            <asp:BoundField DataField="DateCreated" HeaderText="Date Requested" DataFormatString="{0:MMMM d, yyyy hh:mm tt}" />
                            <asp:BoundField DataField="DateCancelled" HeaderText="Date Cancelled" DataFormatString="{0:MMMM d, yyyy hh:mm tt}" />
                        </Columns>
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="50px" />
                        <EditRowStyle BackColor="#2461BF" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                </div>
                <div class="reportbutton_wrapper">
                    <asp:Button ID="exportButton" runat="server" Text="Export" OnClick="exportButton_Click"
                        CssClass="ButtonStyle" />
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="exportButton" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
