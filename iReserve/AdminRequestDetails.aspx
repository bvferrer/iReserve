<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="AdminRequestDetails.aspx.cs" Inherits="AdminRequestDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager" EnablePageMethods="true" runat="server" />
    <script language="javascript" type="text/javascript">
        function DefaultBack() {
            location.replace("Default.aspx");
        }
    </script>
    <div class="mainDiv">
        <h2 class="header">
            <span>Request Details </span>
        </h2>
        <asp:UpdatePanel ID="UpdatePanel" runat="server" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="contentDiv">
                    <div class="details_wrapper">
                        <table>
                            <tr>
                                <td>
                                    <strong>Reference Number</strong>
                                </td>
                                <td>
                                    <asp:Label ID="referenceNumberLabel" runat="server" Text=""></asp:Label>
                                    <asp:LinkButton ID="viewHistoryLinkButton" runat="server" Text="View Request History"
                                        OnClick="viewHistoryLinkButton_Click"></asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <strong>Date</strong>
                                </td>
                                <td>
                                    <asp:Label ID="dateLabel" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <strong>Start Time</strong>
                                </td>
                                <td>
                                    <asp:Label ID="startTimeLabel" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <strong>End Time</strong>
                                </td>
                                <td>
                                    <asp:Label ID="endTimeLabel" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <strong>Room</strong>
                                </td>
                                <td>
                                    <asp:Label ID="roomLabel" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <strong>Requestor</strong>
                                </td>
                                <td>
                                    <asp:Label ID="requestorLabel" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <strong>Cost Center</strong>
                                </td>
                                <td>
                                    <asp:Label ID="costCenterLabel" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <strong>Date Requested</strong>
                                </td>
                                <td>
                                    <asp:Label ID="dateRequestedLabel" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <strong>Agenda</strong>
                                </td>
                                <td>
                                    <asp:Label ID="agendaLabel" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <strong>Attendees</strong>
                                </td>
                                <td>
                                    <asp:GridView ID="attendeeGridView" runat="server" AutoGenerateColumns="false" CssClass="detailsattendee">
                                        <HeaderStyle BackColor="#507CD1" ForeColor="#FFFFFF" Font-Bold="true" />
                                        <RowStyle BackColor="#EFF3FB"></RowStyle>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="FullName" HeaderText="Name" />
                                            <asp:BoundField DataField="Company" HeaderText="Company" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <strong>Equipment To Use</strong>
                                </td>
                                <td>
                                    <asp:Label ID="noneLabel" runat="server" Text="-" Visible="false"></asp:Label>
                                    <asp:Label ID="dataPortLabel" runat="server" Text="- Data Port/Wifi" Visible="false"></asp:Label><br />
                                    <asp:Label ID="monitorLabel" runat="server" Text="- TV/Monitor" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <strong>Attachments</strong>
                                </td>
                                <td>
                                    <asp:GridView ID="attachmentGridView" runat="server" AutoGenerateColumns="false"
                                        CssClass="detailsattachment">
                                        <HeaderStyle BackColor="#507CD1" ForeColor="#FFFFFF" Font-Bold="true" />
                                        <RowStyle BackColor="#EFF3FB"></RowStyle>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="downloadLinkButton" runat="server" OnClick="downloadLinkButton_Click"
                                                        CausesValidation="false">Download</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="StatusName" HeaderText="Approval" />
                                            <asp:BoundField DataField="FileName" HeaderText="File Name" />
                                            <asp:BoundField DataField="StatusCode" HeaderText="Status" HeaderStyle-CssClass="hiddencol"
                                                ItemStyle-CssClass="hiddencol" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <strong>Status</strong>
                                </td>
                                <td>
                                    <asp:Label ID="statusLabel" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField ID="referenceNumberHiddenField" runat="server" />
                    </div>
                    <div class="button_wrapper">
                        <table>
                            <tr>
                                <td>
                                    <input id="backButton" type="button" value="Back" runat="server" class="ButtonStyle"
                                        onclick="javascript:DefaultBack()" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:Button ID="btnPopUp2" runat="server" Style="display: none" />
                    <asp:ModalPopupExtender ID="historyDetails" runat="server" TargetControlID="btnPopUp2"
                        PopupControlID="pnlpopup2" BackgroundCssClass="modalBackground">
                    </asp:ModalPopupExtender>
                    <div id="historyDetailsDiv" runat="server" style="display: none;">
                        <asp:Panel ID="pnlpopup2" runat="server" BackColor="White">
                            <div class="historyformwrapper">
                                <div class="formheaderdiv">
                                    <div class="bold">
                                        <asp:Label ID="headerHistoryLabel" runat="server" Text="Reservation Request History"></asp:Label>
                                    </div>
                                </div>
                                <div class="formfieldsdiv">
                                    <div>
                                        <asp:GridView ID="historyGridView" runat="server" AutoGenerateColumns="false" CssClass="historygrid">
                                            <HeaderStyle BackColor="#507CD1" ForeColor="#FFFFFF" Font-Bold="true" />
                                            <RowStyle BackColor="#EFF3FB"></RowStyle>
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                                <asp:BoundField DataField="DateProcessed" HeaderText="Date Processed" />
                                                <asp:BoundField DataField="ProcessedBy" HeaderText="Processed By" />
                                                <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div>
                                        <asp:Button ID="closeButton" runat="server" Text="Close" Width="80px" OnClick="closeButton_Click"
                                            CausesValidation="false" />
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="attachmentGridView" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
