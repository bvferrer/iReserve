<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
  CodeFile="CCSOADetails.aspx.cs" Inherits="CCSOADetails" %>

<%@ Register Assembly="eWorld.UI, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
  Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="Server">
  <asp:ScriptManager ID="ScriptManager" EnablePageMethods="true" runat="server" />
  <script language="javascript" type="text/javascript">
    function DefaultBack() {
      location.replace("CCAdminRequestDetails.aspx");
    }

    function ButtonLoadingAdd(obj) {
      if (!Page_ClientValidate('add')) {
        return false;
      }
      obj.disabled = true;
      obj.value = 'Please wait...';
    }

    function ButtonLoadingSubmit(obj) {
      if (!Page_ClientValidate('submit')) {
        return false;
      }
      obj.disabled = true;
      obj.value = 'Please wait...';
    }

    function ButtonLoadingSend(obj) {
      obj.disabled = true;
      obj.value = 'Please wait...';
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
                      <asp:BoundField DataField="RatePerNight" HeaderText="Rate per Night" />
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
                  <asp:GridView ID="otherChargesGridView" runat="server" AutoGenerateColumns="false" DataKeyNames="OtherChargeID"
                    CssClass="detailsattendee" OnRowDeleting="otherChargesGridView_RowDeleting">
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
                      <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                          <asp:Button ID="deleteButton" runat="server" CommandName="Delete" Text="Delete" CssClass="gridviewselect"
                            OnClientClick="return confirm('Are you sure you want to delete this record?');" />
                        </ItemTemplate>
                      </asp:TemplateField>
                    </Columns>
                  </asp:GridView>
                  <asp:Button ID="otherChargesAddButton" runat="server" CssClass="addButton" Text="Add" Width="60px" OnClick="otherChargesAddButton_Click" />
                </td>
              </tr>
              <tr>
                <td>
                  <strong>Percent Discount
                  </strong>
                </td>
                <td>
                  <asp:TextBox ID="txtPercentDiscount" AutoPostBack="true" OnTextChanged="txtPercentDiscount_TextChanged" runat="server" CssClass="input-discount" onkeypress="return isNumberKey(event)" onblur="validateRange(this)"></asp:TextBox>
                  %
                  <asp:RegularExpressionValidator
                    ID="regexPercentDiscount"
                    runat="server"
                    ControlToValidate="txtPercentDiscount"
                    ErrorMessage="Please enter a valid number"
                    ValidationExpression="^(\d{1,2}(\.\d{1,2})?|100(\.0{1,2})?)$"
                    Display="Dynamic"
                    ForeColor="Red">
</asp:RegularExpressionValidator>
                  <script type="text/javascript">
                    function isNumberKey(evt) {
                      var charCode = (evt.which) ? evt.which : evt.keyCode;
                      // Allow numbers (48-57) and decimal point (46)
                      if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                        return false;
                      }
                      // Allow only one decimal point
                      if (charCode == 46 && evt.target.value.indexOf('.') != -1) {
                        return false;
                      }
                      return true;
                    }

                    function validateRange(input) {
                      var value = parseFloat(input.value);
                      if (isNaN(value) || value < 0 || value > 100) {
                        alert("Please enter a number between 0 and 100");
                        input.value = '';
                      }
                    }
                  </script>

                </td>
              </tr>
              <tr>
                <td>
                  <strong>Total Payable</strong>
                </td>
                <td>
                  <asp:Label ID="totalLabel" runat="server" Text=""></asp:Label>
                </td>
              </tr>
            </table>
            <asp:HiddenField ID="referenceNumberHiddenField" runat="server" />
          </div>
          <div class="button_wrapper">
            <table>
              <tr>
                <td>
                  <asp:Button ID="submitSOAButton" runat="server" CssClass="ButtonStyle" Text="Submit" Visible="false"
                    OnClick="submitSOAButton_Click" />
                  <asp:Button ID="sendSOAButton" runat="server" CssClass="ButtonStyle" Text="Send" Visible="false"
                    OnClick="sendSOAButton_Click" OnClientClick="ButtonLoadingSend(this)"
                    UseSubmitBehavior="false" />
                  <input id="backButton" type="button" value="Back" runat="server" class="ButtonStyle"
                    onclick="javascript:DefaultBack()" />
                </td>
              </tr>
            </table>
          </div>
          <asp:Button ID="btnPopUp" runat="server" Style="display: none" />
          <asp:Button ID="btnPopUp2" runat="server" Style="display: none" />
          <asp:ModalPopupExtender ID="addDetails" runat="server" TargetControlID="btnPopUp"
            PopupControlID="pnlpopup" BackgroundCssClass="modalBackground">
          </asp:ModalPopupExtender>
          <asp:ModalPopupExtender ID="submitDetails" runat="server" TargetControlID="btnPopUp2"
            PopupControlID="pnlpopup2" BackgroundCssClass="modalBackground">
          </asp:ModalPopupExtender>
          <div id="addDetailsDiv" runat="server" style="display: none;">
            <asp:Panel ID="pnlpopup" runat="server" BackColor="White">
              <div class="formwrapper">
                <div class="formheaderdiv">
                  <div class="bold">
                    <asp:Label ID="headerLabel2" runat="server" Text="Add Other Charges"></asp:Label>
                  </div>
                </div>
                <div class="formfieldsdiv">
                  <div>
                    <span class="bold">Particulars</span><br />
                    <asp:TextBox ID="particularsTextBox" runat="server" Height="40px" CssClass="TextBoxStyle"
                      Width="300px" MaxLength="100" ValidationGroup="add" TextMode="MultiLine"></asp:TextBox><br />
                    <asp:RequiredFieldValidator ID="particularsRequiredFieldValidator" runat="server"
                      SetFocusOnError="true" Display="Dynamic" ControlToValidate="particularsTextBox"
                      ErrorMessage="Particulars field is required." ValidationGroup="add" ForeColor="Red"
                      Font-Size="10px"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="particularsRegularExpressionValidator" runat="server"
                      SetFocusOnError="true" Display="Dynamic" ControlToValidate="particularsTextBox"
                      ValidationExpression="^[0-9A-Za-z\.,\-'\s&)(/]+$" ValidationGroup="add" ErrorMessage="Particulars field should not contain special char."
                      ForeColor="Red" Font-Size="10px"></asp:RegularExpressionValidator>
                  </div>
                  <div>
                    <span class="bold">Amount Charge</span><br />
                    <ew:NumericBox ID="amountChargeNumericBox" runat="server" CssClass="TextBoxStyle"
                      Width="100px" MaxLength="6" PositiveNumber="true" DecimalPlaces="2" ValidationGroup="add"></ew:NumericBox><br />
                    <asp:RequiredFieldValidator ID="amountChargeRequiredFieldValidator" runat="server"
                      SetFocusOnError="true" Display="Dynamic" ControlToValidate="amountChargeNumericBox"
                      ErrorMessage="Amount charget is required." ValidationGroup="add" ForeColor="Red"
                      Font-Size="10px"></asp:RequiredFieldValidator>
                  </div>
                  <div>
                    <span class="bold">Remarks</span><br />
                    <asp:TextBox ID="remarksTextBox" runat="server" Height="40px" CssClass="TextBoxStyle"
                      Width="300px" MaxLength="100" ValidationGroup="add" TextMode="MultiLine"></asp:TextBox><br />
                    <asp:RegularExpressionValidator ID="remarksLengthValidator" runat="server" SetFocusOnError="true"
                      Display="Dynamic" ControlToValidate="remarksTextBox" ValidationExpression="^.{1,100}$"
                      ValidationGroup="add" ErrorMessage="Remarks is up to 100 characters only." ForeColor="Red"
                      Font-Size="10px"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="remarksRegularExpressionValidator" runat="server"
                      SetFocusOnError="true" Display="Dynamic" ControlToValidate="remarksTextBox" ValidationExpression="^[0-9A-Za-z\.,\-'\s?&)(/]+$"
                      ValidationGroup="add" ErrorMessage="The only accepted special characters are .,-')(?&/"
                      ForeColor="Red" Font-Size="10px"></asp:RegularExpressionValidator>
                  </div>
                  <div>
                    <asp:Button ID="addButton" runat="server" Text="Add" Width="80px" CausesValidation="true"
                      ValidationGroup="add" OnClick="addButton_Click" OnClientClick="ButtonLoadingAdd(this)"
                      UseSubmitBehavior="false" />
                    <asp:Button ID="cancelButton" runat="server" Text="Cancel" Width="80px" OnClick="cancelButton_Click"
                      CausesValidation="false" />
                  </div>
                </div>
              </div>
            </asp:Panel>
          </div>
          <div id="submitDetailsDiv" runat="server" style="display: none;">
            <asp:Panel ID="pnlpopup2" runat="server" BackColor="White">
              <div class="formwrapper">
                <div class="formheaderdiv">
                  <div class="bold">
                    <asp:Label ID="Label1" runat="server" Text="Submit Statement of Account"></asp:Label>
                  </div>
                </div>
                <div class="formfieldsdiv">
                  <div>
                    <span class="bold">Remarks</span><br />
                    <asp:TextBox ID="submitRemarksTextBox" runat="server" Height="50px" CssClass="TextBoxStyle"
                      Width="300px" MaxLength="100" ValidationGroup="submit" TextMode="MultiLine"></asp:TextBox><br />
                    <asp:RequiredFieldValidator ID="submitRemarksRequiredFieldValidator" runat="server" SetFocusOnError="true"
                      Display="Dynamic" ControlToValidate="submitRemarksTextBox" ErrorMessage="Remarks field is required."
                      ValidationGroup="submit" ForeColor="Red" Font-Size="10px"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="submitRemarksLengthRegularExpressionValidator" runat="server" SetFocusOnError="true"
                      Display="Dynamic" ControlToValidate="submitRemarksTextBox" ValidationExpression="^.{1,100}$"
                      ValidationGroup="submit" ErrorMessage="Remarks field is up to 100 characters only." ForeColor="Red"
                      Font-Size="10px"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="submitRemarksRegularExpressionValidator" runat="server"
                      SetFocusOnError="true" Display="Dynamic" ControlToValidate="submitRemarksTextBox" ValidationExpression="^[0-9A-Za-z\.,\-'\s?&)(/]+$"
                      ValidationGroup="submit" ErrorMessage="The only accepted special characters are .,-')(?&/"
                      ForeColor="Red" Font-Size="10px"></asp:RegularExpressionValidator>
                  </div>
                  <div>
                    <asp:Button ID="submitButton" runat="server" Text="Submit" Width="80px" CausesValidation="true"
                      ValidationGroup="submit" OnClick="submitButton_Click" OnClientClick="ButtonLoadingSubmit(this)"
                      UseSubmitBehavior="false" />
                    <asp:Button ID="cancelSubmitButton" runat="server" Text="Cancel" Width="80px" OnClick="cancelSubmitButton_Click"
                      CausesValidation="false" />
                  </div>
                </div>
              </div>
            </asp:Panel>
          </div>
        </div>
      </ContentTemplate>
      <Triggers>
        <asp:PostBackTrigger ControlID="addButton" />
        <asp:PostBackTrigger ControlID="submitButton" />
        <asp:PostBackTrigger ControlID="sendSOAButton" />
      </Triggers>
    </asp:UpdatePanel>
  </div>
</asp:Content>
