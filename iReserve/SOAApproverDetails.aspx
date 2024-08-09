<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
  CodeFile="SOAApproverDetails.aspx.cs" Inherits="SOAApproverDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="Server">
  <asp:ScriptManager ID="ScriptManager" EnablePageMethods="true" runat="server" />
  <script language="javascript" type="text/javascript">
    function DefaultBack() {
      location.replace("SOAApproverHome.aspx");
    }

    function ButtonLoading() {
      var approve = $get("<%=approveButton.ClientID%>");
      var dispprove = $get("<%=disapproveButton.ClientID%>");
      var back = $get("<%=backButton.ClientID%>");

      if (!Page_ClientValidate('form')) {
        return false;
      }

      approve.disabled = true;
      dispprove.disabled = true;
      back.disabled = true;
    }
  </script>
  <div class="mainDiv">
    <h2 class="header">
      <span>Request Details </span>
    </h2>
    <asp:UpdatePanel ID="UpdatePanel" runat="server" ChildrenAsTriggers="true">
      <ContentTemplate>
        <div class="contentDiv">
          <div class="soadetails_wrapper">
            <strong>STATEMENT OF ACCOUNT</strong>
            <br />
            <br />
            <hr />
            <strong>Reservation Details</strong>
            <hr />
            <table>
              <tr>
                <td>
                  <strong>Reference Number</strong>
                </td>
                <td>
                  <asp:Label ID="ccReferenceNumberLabel" runat="server" Text=""></asp:Label>
                </td>
                <td>
                  <strong>Event Name</strong>
                </td>
                <td>
                  <asp:Label ID="eventNameLabel" runat="server" Text=""></asp:Label>
                </td>
              </tr>
              <tr>
                <td>
                  <strong>Company</strong>
                </td>
                <td>
                  <asp:Label ID="chargedCompanyLabel" runat="server" Text=""></asp:Label>
                </td>
                <td>
                  <strong>Cost Center</strong>
                </td>
                <td>
                  <asp:Label ID="costCenterLabel" runat="server" Text=""></asp:Label>
                </td>

              </tr>
              <tr>
                <td>
                  <strong>Start Date</strong>
                </td>
                <td>
                  <asp:Label ID="startDateLabel" runat="server" Text=""></asp:Label>
                </td>
                <td>
                  <strong>End Date</strong>
                </td>
                <td>
                  <asp:Label ID="endDateLabel" runat="server" Text=""></asp:Label>
                </td>
              </tr>
              <tr>
                <td>
                  <strong>Requestor</strong>
                </td>
                <td>
                  <asp:Label ID="requestorLabel" runat="server" Text=""></asp:Label>
                </td>
                <td>
                  <strong>Date Requested</strong>
                </td>
                <td>
                  <asp:Label ID="dateRequestedLabel" runat="server" Text=""></asp:Label>
                </td>
              </tr>
            </table>
            <hr />
            <strong>Breakdown of Charges</strong>
            <hr />
            <table>
              <tr>
                <td>
                  <strong>Training Room</strong>
                </td>
                <td>
                  <asp:GridView ID="trainingRoomGridView" runat="server" AutoGenerateColumns="false"
                    CssClass="detailsattendee">
                    <HeaderStyle BackColor="#507CD1" ForeColor="#FFFFFF" Font-Bold="true" />
                    <RowStyle BackColor="#EFF3FB"></RowStyle>
                    <Columns>
                      <asp:TemplateField>
                        <ItemTemplate>
                          <%#Container.DataItemIndex+1 %>
                        </ItemTemplate>
                      </asp:TemplateField>
                      <asp:BoundField DataField="TRChargeID" HeaderText="Charge ID" HeaderStyle-CssClass="hiddencol"
                        ItemStyle-CssClass="hiddencol" />
                      <asp:BoundField DataField="TRoomName" HeaderText="Room Name" />
                      <asp:BoundField DataField="NumberOfPartition" HeaderText="Number of Sub Room" />
                      <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:MM/dd/yyyy}" />
                      <asp:BoundField DataField="StartDateTime" HeaderText="Start Time" DataFormatString="{0:hh:mm tt}" />
                      <asp:BoundField DataField="EndDateTime" HeaderText="End Time" DataFormatString="{0:hh:mm tt}" />
                      <asp:BoundField DataField="NumberOfHours" HeaderText="Hours" />
                      <asp:BoundField DataField="RatePerDay" HeaderText="Rate Per Day" />
                      <asp:BoundField DataField="ExtensionRatePerHour" HeaderText="Extension Rate Per Hour" />
                      <asp:BoundField DataField="AmountCharge" HeaderText="Amount Charge" />
                    </Columns>
                  </asp:GridView>
                  <asp:Label ID="tRoomGridViewLabel" runat="server" Text="-" Visible="false"></asp:Label>
                </td>
              </tr>
              <tr>
                <td>
                  <strong>Accomodation Room</strong>
                </td>
                <td>
                  <asp:GridView ID="accomodationRoomGridView" runat="server" AutoGenerateColumns="false"
                    CssClass="detailsattendee">
                    <HeaderStyle BackColor="#507CD1" ForeColor="#FFFFFF" Font-Bold="true" />
                    <RowStyle BackColor="#EFF3FB"></RowStyle>
                    <Columns>
                      <asp:TemplateField>
                        <ItemTemplate>
                          <%#Container.DataItemIndex+1 %>
                        </ItemTemplate>
                      </asp:TemplateField>
                      <asp:BoundField DataField="AccID" HeaderText="Room Name" HeaderStyle-CssClass="hiddencol"
                        ItemStyle-CssClass="hiddencol" />
                      <asp:BoundField DataField="RoomName" HeaderText="Room Name" />
                      <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:MM/dd/yyyy}" />
                      <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:MM/dd/yyyy}" />
                      <asp:BoundField DataField="NumberOfNights" HeaderText="Nights" />
                      <asp:BoundField DataField="RatePerNight" HeaderText="Rate Per Night" />
                      <asp:BoundField DataField="AmountCharge" HeaderText="Amount Charge" />
                    </Columns>
                  </asp:GridView>
                  <asp:Label ID="accRoomGridviewLabel" runat="server" Text="-" Visible="false"></asp:Label>
                </td>
              </tr>
              <tr>
                <td>
                  <strong>Other Charges</strong>
                </td>
                <td>
                  <asp:GridView ID="otherChargesGridView" runat="server" AutoGenerateColumns="false"
                    DataKeyNames="OtherChargeID" CssClass="detailsattendee">
                    <HeaderStyle BackColor="#507CD1" ForeColor="#FFFFFF" Font-Bold="true" />
                    <RowStyle BackColor="#EFF3FB"></RowStyle>
                    <Columns>
                      <asp:TemplateField>
                        <ItemTemplate>
                          <%#Container.DataItemIndex+1 %>
                        </ItemTemplate>
                      </asp:TemplateField>
                      <asp:BoundField DataField="OtherChargeID" HeaderText="Charge ID" HeaderStyle-CssClass="hiddencol"
                        ItemStyle-CssClass="hiddencol" />
                      <asp:BoundField DataField="Particulars" HeaderText="Particulars" />
                      <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                      <asp:BoundField DataField="AmountCharge" HeaderText="Amount Charge" />
                    </Columns>
                  </asp:GridView>
                </td>
              </tr>
              <tr>
                <td><strong>Percent Discount</strong></td>
                <td>
                  <asp:Label ID="txtPercentDiscount" runat="server"></asp:Label>%</td>
              </tr>
              <tr>
                <td>
                  <strong>Total Payable</strong>
                </td>
                <td>
                  <asp:Label ID="totalLabel" runat="server" Text=""></asp:Label>
                </td>
              </tr>

              <tr>
                <td>
                  <strong>
                    <asp:Label ID="remarksLabel" runat="server" Text="Remarks" Visible="false"></asp:Label></strong>
                </td>
                <td>
                  <asp:Panel ID="remarksPanel" runat="server" Visible="false">
                    <asp:TextBox ID="remarksTextBox" runat="server" Height="50px" CssClass="TextBoxStyle"
                      Width="300px" MaxLength="100" ValidationGroup="form" TextMode="MultiLine"></asp:TextBox><br />
                    <asp:Label ID="remarksValidationLabel" runat="server" Text="" ForeColor="Red" Font-Size="10px"></asp:Label>
                    <asp:RegularExpressionValidator ID="remarksLengthValidator" runat="server" SetFocusOnError="true"
                      Display="Dynamic" ControlToValidate="remarksTextBox" ValidationExpression="^.{1,100}$"
                      ValidationGroup="form" ErrorMessage="Remarks is up to 100 characters only." ForeColor="Red"
                      Font-Size="10px"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="remarksRegularExpressionValidator" runat="server"
                      SetFocusOnError="true" Display="Dynamic" ControlToValidate="remarksTextBox" ValidationExpression="^[0-9A-Za-z\.,\-'\s?&)(/]+$"
                      ValidationGroup="form" ErrorMessage="The only accepted special characters are .,-')(?&/"
                      ForeColor="Red" Font-Size="10px"></asp:RegularExpressionValidator>
                  </asp:Panel>
                </td>
              </tr>
            </table>
          </div>
          <div class="button_wrapper">
            <table>
              <tr>
                <td>
                  <asp:Button ID="approveButton" runat="server" CssClass="ButtonStyle" Text="Approve"
                    Visible="false" OnClick="approveButton_Click" OnClientClick="ButtonLoading()"
                    CausesValidation="true" ValidationGroup="form" UseSubmitBehavior="false" />
                  <asp:Button ID="disapproveButton" runat="server" CssClass="ButtonStyle" Text="Disapprove"
                    Visible="false" OnClick="disapproveButton_Click" OnClientClick="ButtonLoading()"
                    CausesValidation="true" ValidationGroup="form" UseSubmitBehavior="false" />
                  <input id="backButton" type="button" value="Back" runat="server" class="ButtonStyle"
                    onclick="javascript:DefaultBack()" />
                </td>
              </tr>
            </table>
          </div>
          <asp:HiddenField ID="referenceNumberHiddenField" runat="server" />
      </ContentTemplate>
      <Triggers>
        <asp:PostBackTrigger ControlID="approveButton" />
        <asp:PostBackTrigger ControlID="disapproveButton" />
      </Triggers>
    </asp:UpdatePanel>
  </div>
</asp:Content>
