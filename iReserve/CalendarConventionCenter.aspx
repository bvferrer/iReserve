<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CalendarConventionCenter.aspx.cs"
  Inherits="CalendarConventionCenter" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="eWorld.UI, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
  Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>iReserve Convention Center</title>
  <link rel="SHORTCUT ICON" href="img/PEL.ico" />
  <link href="css/Style.css" media="screen" rel="Stylesheet" />
  <link href="css/CCStyles.css" media="screen" rel="Stylesheet" />
  <script src="js/jquery-1.12.0.js"></script>
  <%--<script src="js/jquery-1.9.1.js" type="text/javascript"></script>--%>
  <%--<script src="js/jquery-ui-1.10.0.custom.js"></script>--%>
  <script type="text/javascript">
    //        function pageLoad() {
    //            ShowPopup();
    //            setTimeout(HidePopup, 300);
    //        }

    //        function ShowPopup() {
    //            $find('loadingModalPopupExtender').show();
    //            //$get('Button1').click();
    //        }

    //        function HidePopup() {
    //            $find('loadingModalPopupExtender').hide();
    //            //$get('btnCancel').click();
    //        }

    function isNumberKey(evt) {
      var charCode = (evt.which) ? evt.which : event.keyCode;
      if (charCode == 46 && evt.srcElement.value.split('.').length > 1) {
        return false;
      }
      if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
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

    function isLetter(event) {
      var key = event.keyCode;
      return ((key >= 65 && key <= 90) || key == 8);
    };

    //        function sticky_relocate() {
    //            var window_top = $(window).scrollTop();
    //            var div_top = $('#previousNext').offset().top;
    //            if (window_top > div_top) {
    //                $('#calendarHeaderTable').addClass('calendarHeaderStick');
    //            } else {
    //                $('#calendarHeaderTable').removeClass('calendarHeaderStick');
    //            }
    //        }

    //        $(function () {
    //            $(window).scroll(sticky_relocate);
    //            sticky_relocate();
    //        });

    function ButtonLoading() {
      if (!Page_ClientValidate('summary')) {
        return false;
      }

      submit = document.getElementById('submitButton');
      submit.disabled = true;
      submit.value = 'Please wait...';

      document.getElementById('summaryTrainingRoomGridView').disabled = true;
      document.getElementById('summaryAccomodationRoomGridView').disabled = true;
      document.getElementById('clearButton').disabled = true;
      document.getElementById('backButton').disabled = true;
    }
  </script>
</head>
<body>
  <script type="text/javascript">
    function trCellClicked(row, column, e) {
      document.getElementById('inhAction').value = "CELLCLICKED";
      document.getElementById('inhRow').value = row;
      document.getElementById('inhColumn').value = column;
      document.getElementById('trHiddenButton').click();
    }
    function arCellClicked(row, column, e) {
      document.getElementById('inhAction').value = "CELLCLICKED";
      document.getElementById('inhRow').value = row;
      document.getElementById('inhColumn').value = column;
      document.getElementById('arHiddenButton').click();
    }
  </script>
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" />
    <asp:UpdatePanel ID="calendarUpdatePanel" runat="server">
      <ContentTemplate>
        <div class="calContainer">
          <asp:Button ID="trHiddenButton" runat="server" Text="" OnClick="trHiddenButton_Click"
            Style="display: none;" />
          <asp:Button ID="arHiddenButton" runat="server" Text="" OnClick="arHiddenButton_Click"
            Style="display: none;" />
          <input runat="server" type="hidden" id="inhAction" />
          <input runat="server" type="hidden" id="inhRow" />
          <input runat="server" type="hidden" id="inhColumn" />
          <div class="innerNav">
            <asp:LinkButton ID="homeLinkButton" runat="server" PostBackUrl="~/Default.aspx" Text="Home"></asp:LinkButton>
            <span>| Convention Center Reservation Calendar</span>
          </div>
          <div class="ccCalendarDatePicker">
            <span>Date: </span>
            <asp:TextBox ID="datepicker" runat="server" MaxLength="10" onkeypress="return false;"
              AutoCompleteType="Disabled" OnTextChanged="datepicker_TextChanged" AutoPostBack="True"></asp:TextBox>
            <asp:CalendarExtender ID="dateCalendarExtender" Format="MM/dd/yyyy" TargetControlID="datepicker"
              runat="server" PopupButtonID="datepicker" />
            <span>
              <asp:LinkButton ID="previousLinkButton" class="prev" runat="server" OnClick="previousLinkButton_Click">Previous</asp:LinkButton>
            </span><span>
              <asp:LinkButton ID="nextLinkButton" class="next" runat="server" OnClick="nextLinkButton_Click">Next</asp:LinkButton>
            </span>
          </div>
          <div class="ccCalendarNavigation">
          </div>
          <div class="ccRoom">
            <span>Select schedule: </span>
            <div class="buttonwrapper">
              <asp:Button ID="viewSummaryButton" runat="server" Text="View Request Summary" class="newRequestButton"
                OnClick="viewSummaryButton_Click" />
            </div>
          </div>
          <div id="ccCalendarHeaderTable" runat="server" class="ccCalendarHeader">
          </div>
          <div class="ccScheduleGridviewWrapper">
            <asp:GridView ID="trainingRoomScheduleGridview" runat="server" OnRowDataBound="trainingRoomScheduleGridview_RowDataBound"
              Font-Names="Arial" Font-Size="9pt" CssClass="ccScheduleGridviewCSS" ShowHeader="false"
              OnDataBound="trainingRoomScheduleGridview_DataBound">
            </asp:GridView>
          </div>
          <div id="accCalendarHeaderTable" runat="server" class="ccCalendarHeader">
          </div>
          <div class="ccScheduleGridviewWrapper">
            <asp:GridView ID="accomodationRoomScheduleGridView" runat="server" OnRowDataBound="accomodationRoomScheduleGridView_RowDataBound"
              Font-Names="Arial" Font-Size="9pt" CssClass="ccScheduleGridviewCSS" ShowHeader="false">
            </asp:GridView>
          </div>
          <asp:Button ID="btnPopUp" runat="server" Style="display: none" />
          <asp:ModalPopupExtender ID="trainingRoomDetails" runat="server" TargetControlID="btnPopUp"
            PopupControlID="pnlpopup" BackgroundCssClass="modalBackground">
          </asp:ModalPopupExtender>
          <div id="trainingRoomDetailsDiv" runat="server" style="display: none;">
            <asp:Panel ID="pnlpopup" runat="server" BackColor="White">
              <div class="formwrapper">
                <div class="formheaderdiv">
                  <div class="bold">
                    <asp:Label ID="tRoomIDLabel" runat="server" Text="" Visible="false"></asp:Label>
                    <asp:Label ID="tRoomNameLabel" runat="server" Text="" Visible="false"></asp:Label>
                    <asp:Label ID="partitionIDLabel" runat="server" Text="" Visible="false"></asp:Label>
                    <asp:Label ID="partitionNameLabel" runat="server" Text=""></asp:Label>
                  </div>
                  <div>
                    <span class="bold">Location: </span>
                    <asp:Label ID="tRoomLocationLabel" runat="server" Text=""></asp:Label>
                    <span class="bold">Max Person: </span>
                    <asp:Label ID="partitionMaxPersonLabel" runat="server" Text=""></asp:Label>
                  </div>
                  <div>
                    <span class="bold">Details: </span>
                    <asp:Label ID="partitionDetailsLabel" runat="server" Text="Details"></asp:Label>
                  </div>
                </div>
                <div class="formfieldsdiv">
                  <%--<div>
                                    <span class="bold">Sub Rooms</span><br />
                                    <asp:CheckBoxList ID="subRoomCheckBoxList" runat="server" CssClass=""></asp:CheckBoxList>
                                </div>--%>
                  <div>
                    <div class="formCalendarDatePicker">
                      <span class="bold">Start Date</span><br />
                      <asp:Label ID="trStartDateLabel" runat="server" Text=""></asp:Label>
                      <%--<asp:CalendarExtender ID="trStartDateCalendarExtender" Format="MM/dd/yyyy" TargetControlID="trStartDateTextBox"
                                            runat="server" PopupButtonID="trStartDate" />--%>
                      <%--<span class="bold">To</span>--%>
                      <%--<asp:TextBox ID="trEndDateTextBox" runat="server" MaxLength="10" onkeypress="return false;"
                                            AutoCompleteType="Disabled" ValidationGroup="trainroom" Width="100px" CssClass="hiddencol"></asp:TextBox>--%>
                      <%--<asp:CalendarExtender ID="trEndDateCalendarExtender" Format="MM/dd/yyyy" TargetControlID="trEndDateTextBox"
                                            runat="server" PopupButtonID="trEndDate" />--%>
                      <%--<asp:TextBox ID="trDateTodayTextBox" runat="server" ValidationGroup="trainroom" CssClass="hiddencol"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="trStartDateRequiredFieldValidator" runat="server"
                                            SetFocusOnError="true" Display="Dynamic" ControlToValidate="trStartDateTextBox"
                                            ErrorMessage="Start date is required." ValidationGroup="trainroom" ForeColor="Red"
                                            Font-Size="10px"></asp:RequiredFieldValidator>--%>
                      <%--<asp:RequiredFieldValidator ID="trEndDateRequiredFieldValidator" runat="server" SetFocusOnError="true"
                                        Display="Dynamic" ControlToValidate="trEndDateTextBox" ErrorMessage="End date is required."
                                        ValidationGroup="trainroom" ForeColor="Red" Font-Size="10px"></asp:RequiredFieldValidator>--%>
                      <%--<asp:CompareValidator ID="trStartDateCompareValidator" Type="Date" runat="server"
                                            SetFocusOnError="true" Display="Dynamic" ControlToValidate="trStartDateTextBox"
                                            ControlToCompare="trDateTodayTextBox" Operator="GreaterThan" ValidationGroup="trainroom"
                                            ErrorMessage="Past/Present start date is not allowed." ForeColor="Red" Font-Size="10px"></asp:CompareValidator>--%>
                      <%--<asp:CompareValidator ID="trEndDateCompareValidator" Type="Date" runat="server" SetFocusOnError="true"
                                        Display="Dynamic" ControlToValidate="trEndDateTextBox" ControlToCompare="trDateTodayTextBox"
                                        Operator="GreaterThan" ValidationGroup="trainroom" ErrorMessage="Past/Present end date is not allowed."
                                        ForeColor="Red" Font-Size="10px"></asp:CompareValidator>--%>
                      <%--<asp:CompareValidator ID="trStartEndDateCompareValidator" Type="Date" runat="server"
                                        SetFocusOnError="true" Display="Dynamic" ControlToValidate="trStartDateTextBox"
                                        ControlToCompare="trEndDateTextBox" Operator="LessThanEqual" ValidationGroup="trainroom"
                                        ErrorMessage="End date should not be earlier than start date." ForeColor="Red"
                                        Font-Size="10px"></asp:CompareValidator>--%>
                      <asp:TextBox ID="trStartDateTextBox" runat="server" MaxLength="10" onkeypress="return false;"
                        AutoCompleteType="Disabled" ValidationGroup="trainroom" Width="100px" Enabled="false"
                        CssClass="hiddencol"></asp:TextBox>
                    </div>
                  </div>
                  <div>
                    <span class="bold">Number of Days</span><br />
                    <asp:Label ID="numberOfDaysLabel" runat="server" Text="" CssClass="daysLabel"></asp:Label>
                    <asp:Button ID="upButton" runat="server" Text="+" CssClass="upDownButton" OnClick="upButton_Click" />
                    <asp:Button ID="downButton" runat="server" Text="-" CssClass="upDownButton" OnClick="downButton_Click" />
                    <ew:NumericBox ID="numberOfDaysNumericBox" runat="server" MaxLength="2" PositiveNumber="true"
                      DecimalPlaces="0" Width="50px" ReadOnly="true" Enabled="false" CssClass="hiddencol"></ew:NumericBox>
                  </div>
                  <div>
                    <asp:GridView ID="trainingRoomDetailGridView" runat="server" AutoGenerateColumns="False"
                      CssClass="trdetailsgrid">
                      <Columns>
                        <asp:BoundField DataField="PartitionID" HeaderText="Partition ID">
                          <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                          <ItemStyle CssClass="hiddencol"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:MMM d, yyyy}"></asp:BoundField>
                        <asp:TemplateField HeaderText="Start Time">
                          <ItemTemplate>
                            <asp:DropDownList ID="startTimeDropDownList" runat="server">
                              <asp:ListItem Value="7:00" Text="7:00 AM"></asp:ListItem>
                              <asp:ListItem Selected="True" Value="8:00" Text="8:00 AM"></asp:ListItem>
                              <asp:ListItem Value="9:00" Text="9:00 AM"></asp:ListItem>
                              <asp:ListItem Value="10:00" Text="10:00 AM"></asp:ListItem>
                              <asp:ListItem Value="11:00" Text="11:00 AM"></asp:ListItem>
                              <asp:ListItem Value="12:00" Text="12:00 PM"></asp:ListItem>
                              <asp:ListItem Value="13:00" Text="1:00 PM"></asp:ListItem>
                            </asp:DropDownList>
                          </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="End Time">
                          <ItemTemplate>
                            <asp:DropDownList ID="endTimeDropDownList" runat="server">
                              <asp:ListItem Value="16:00" Text="4:00 PM"></asp:ListItem>
                              <asp:ListItem Selected="True" Value="17:00" Text="5:00 PM"></asp:ListItem>
                              <asp:ListItem Value="18:00" Text="6:00 PM"></asp:ListItem>
                              <asp:ListItem Value="19:00" Text="7:00 PM"></asp:ListItem>
                              <asp:ListItem Value="20:00" Text="8:00 PM"></asp:ListItem>
                              <asp:ListItem Value="21:00" Text="9:00 PM"></asp:ListItem>
                              <asp:ListItem Value="22:00" Text="10:00 PM"></asp:ListItem>
                              <asp:ListItem Value="23:00" Text="11:00 PM"></asp:ListItem>
                            </asp:DropDownList>
                          </ItemTemplate>
                        </asp:TemplateField>
                      </Columns>
                    </asp:GridView>
                  </div>
                  <div>
                    <span class="bold">Head Count</span><br />
                    <ew:NumericBox ID="trHeadCountNumericBox" runat="server" CssClass="TextBoxStyle"
                      Width="100px" MaxLength="2" PositiveNumber="true" DecimalPlaces="0" onkeypress="return isWholeNumber();"
                      ValidationGroup="trainroom"></ew:NumericBox>
                    <asp:TextBox ID="partitionMaxPersonTextBox" runat="server" ValidationGroup="trainroom"
                      CssClass="hiddencol"></asp:TextBox><br />
                    <asp:RequiredFieldValidator ID="trHeadCountRequiredFieldValidator" runat="server"
                      SetFocusOnError="true" Display="Dynamic" ControlToValidate="trHeadCountNumericBox"
                      ErrorMessage="Head count is required." ValidationGroup="trainroom" ForeColor="Red"
                      Font-Size="10px"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="trHeadCountCompareValidator" Type="Integer" runat="server"
                      SetFocusOnError="true" Display="Dynamic" ControlToValidate="trHeadCountNumericBox"
                      ControlToCompare="partitionMaxPersonTextBox" Operator="LessThanEqual" ValidationGroup="trainroom"
                      ErrorMessage="Head count should not exceed Max Person." ForeColor="Red" Font-Size="10px"></asp:CompareValidator>
                    <asp:CompareValidator ID="trHeadCountZeroCompareValidator" Type="Integer" runat="server"
                      SetFocusOnError="true" Display="Dynamic" ControlToValidate="trHeadCountNumericBox"
                      ValueToCompare="0" Operator="GreaterThan" ValidationGroup="trainroom"
                      ErrorMessage="Head count should not be zero." ForeColor="Red" Font-Size="10px"></asp:CompareValidator>
                  </div>
                  <div>
                    <span class="bold">Equipment To Use</span><br />
                    <asp:TextBox ID="trEquipmentTextBox" runat="server" Height="40px" CssClass="TextBoxStyle"
                      Width="300px" MaxLength="100" ValidationGroup="trainroom" TextMode="MultiLine"></asp:TextBox><br />
                    <asp:RequiredFieldValidator ID="trEquipmentRequiredFieldValidator" runat="server"
                      SetFocusOnError="true" Display="Dynamic" ControlToValidate="trEquipmentTextBox"
                      ErrorMessage="Equipment is required. (Type n/a if none)" ValidationGroup="trainroom"
                      ForeColor="Red" Font-Size="10px"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="trEquipmentLengthValidator" runat="server" SetFocusOnError="true"
                      Display="Dynamic" ControlToValidate="trEquipmentTextBox" ValidationExpression="^.{1,100}$"
                      ValidationGroup="trainroom" ErrorMessage="Remarks is up to 100 characters only."
                      ForeColor="Red" Font-Size="10px"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="trEquipmentRegularExpressionValidator" runat="server"
                      SetFocusOnError="true" Display="Dynamic" ControlToValidate="trEquipmentTextBox"
                      ValidationExpression="^[0-9A-Za-z\.,\-'\s?&)(/]+$" ValidationGroup="trainroom"
                      ErrorMessage="The only accepted special characters are .,-')(?&/" ForeColor="Red"
                      Font-Size="10px"></asp:RegularExpressionValidator>
                  </div>
                  <div>
                    <span class="bold">Remarks</span><br />
                    <asp:TextBox ID="trRemarksTextBox" runat="server" Height="40px" CssClass="TextBoxStyle"
                      Width="300px" MaxLength="100" ValidationGroup="trainroom" TextMode="MultiLine"></asp:TextBox><br />
                    <asp:RegularExpressionValidator ID="trRemarksLengthValidator" runat="server" SetFocusOnError="true"
                      Display="Dynamic" ControlToValidate="trRemarksTextBox" ValidationExpression="^.{1,100}$"
                      ValidationGroup="trainroom" ErrorMessage="Remarks is up to 100 characters only."
                      ForeColor="Red" Font-Size="10px"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="trRemarksRegularExpressionValidator" runat="server"
                      SetFocusOnError="true" Display="Dynamic" ControlToValidate="trRemarksTextBox"
                      ValidationExpression="^[0-9A-Za-z\.,\-'\s?&)(/]+$" ValidationGroup="trainroom"
                      ErrorMessage="The only accepted special characters are .,-')(?&/" ForeColor="Red"
                      Font-Size="10px"></asp:RegularExpressionValidator>
                    <asp:Label ID="trScheduleValidationLabel" runat="server" Text="" ForeColor="Red"
                      Font-Size="10px"></asp:Label>
                  </div>
                  <div>
                    <asp:Button ID="trAddButton" runat="server" Text="ADD" Width="80px" CausesValidation="true"
                      ValidationGroup="trainroom" OnClick="trAddButton_Click" UseSubmitBehavior="false" />
                    <asp:Button ID="trCancelButton" runat="server" Text="CANCEL" Width="80px" OnClick="trCancelButton_Click"
                      CausesValidation="false" />
                  </div>
                </div>
              </div>
            </asp:Panel>
          </div>
          <asp:Button ID="btnPopUp2" runat="server" Style="display: none" />
          <asp:ModalPopupExtender ID="accomodationRoomDetails" runat="server" TargetControlID="btnPopUp2"
            PopupControlID="pnlpopup2" BackgroundCssClass="modalBackground">
          </asp:ModalPopupExtender>
          <div id="accomodationRoomDetailsDiv" runat="server" style="display: none;">
            <asp:Panel ID="pnlpopup2" runat="server" BackColor="White">
              <div class="formwrapper">
                <div class="formheaderdiv">
                  <div class="bold">
                    <asp:Label ID="accRoomIDLabel" runat="server" Text="" Visible="false"></asp:Label>
                    <asp:Label ID="accRoomNameLabel" runat="server" Text=""></asp:Label>
                  </div>
                  <div>
                    <span class="bold">Location: </span>
                    <asp:Label ID="accRoomLocationLabel" runat="server" Text=""></asp:Label>
                    <span class="bold">Max Person: </span>
                    <asp:Label ID="accRoomMaxPersonLabel" runat="server" Text=""></asp:Label>
                  </div>
                  <div>
                    <span class="bold">Rate per Night: </span>
                    <asp:Label ID="accRateLabel" runat="server" Text=""></asp:Label>
                  </div>
                  <div>
                    <span class="bold">Details: </span>
                    <asp:Label ID="accRoomDetailsLabel" runat="server" Text="Details"></asp:Label>
                  </div>
                </div>
                <div class="formfieldsdiv">
                  <div>
                    <div class="formCalendarDatePicker">
                      <span class="bold">From</span>
                      <asp:TextBox ID="arStartDateTextBox" runat="server" MaxLength="10" onkeypress="return false;"
                        AutoCompleteType="Disabled" ValidationGroup="accroom" Width="100px"></asp:TextBox>
                      <asp:CalendarExtender ID="arStartDateCalendarExtender" Format="MM/dd/yyyy" TargetControlID="arStartDateTextBox"
                        runat="server" PopupButtonID="arStartDate" />
                      <span class="bold">To</span>
                      <asp:TextBox ID="arEndDateTextBox" runat="server" MaxLength="10" onkeypress="return false;"
                        AutoCompleteType="Disabled" ValidationGroup="accroom" Width="100px"></asp:TextBox>
                      <asp:CalendarExtender ID="arEndDateCalendarExtender" Format="MM/dd/yyyy" TargetControlID="arEndDateTextBox"
                        runat="server" PopupButtonID="arEndDate" />
                    </div>
                    <asp:TextBox ID="arDateTodayTextBox" runat="server" ValidationGroup="accroom" CssClass="hiddencol"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="arStartDateRequiredFieldValidator" runat="server"
                      SetFocusOnError="true" Display="Dynamic" ControlToValidate="arStartDateTextBox"
                      ErrorMessage="Start date is required." ValidationGroup="accroom" ForeColor="Red"
                      Font-Size="10px"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="arEndDateRequiredFieldValidator" runat="server" SetFocusOnError="true"
                      Display="Dynamic" ControlToValidate="arEndDateTextBox" ErrorMessage="End date is required."
                      ValidationGroup="accroom" ForeColor="Red" Font-Size="10px"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="arStartDateCompareValidator" Type="Date" runat="server"
                      SetFocusOnError="true" Display="Dynamic" ControlToValidate="arStartDateTextBox"
                      ControlToCompare="arDateTodayTextBox" Operator="GreaterThan" ValidationGroup="accroom"
                      ErrorMessage="Past/Present start date is not allowed." ForeColor="Red" Font-Size="10px"></asp:CompareValidator>
                    <asp:CompareValidator ID="arEndDateCompareValidator" Type="Date" runat="server" SetFocusOnError="true"
                      Display="Dynamic" ControlToValidate="arEndDateTextBox" ControlToCompare="arDateTodayTextBox"
                      Operator="GreaterThan" ValidationGroup="accroom" ErrorMessage="Past/Present end date is not allowed."
                      ForeColor="Red" Font-Size="10px"></asp:CompareValidator>
                    <asp:CompareValidator ID="arStartEndDateCompareValidator" Type="Date" runat="server"
                      SetFocusOnError="true" Display="Dynamic" ControlToValidate="arStartDateTextBox"
                      ControlToCompare="arEndDateTextBox" Operator="LessThan" ValidationGroup="accroom"
                      ErrorMessage="End date should be later than start date." ForeColor="Red" Font-Size="10px"></asp:CompareValidator>
                  </div>
                  <div>
                    <span class="bold">Head Count</span><br />
                    <ew:NumericBox ID="arHeadCountNumericBox" runat="server" CssClass="TextBoxStyle"
                      Width="100px" MaxLength="2" PositiveNumber="true" DecimalPlaces="0" onkeypress="return isWholeNumber();"
                      ValidationGroup="accroom"></ew:NumericBox>
                    <asp:TextBox ID="arMaxPersonTextBox" runat="server" ValidationGroup="trroom" CssClass="hiddencol"></asp:TextBox><br />
                    <asp:RequiredFieldValidator ID="arHeadCountRequiredFieldValidator" runat="server"
                      SetFocusOnError="true" Display="Dynamic" ControlToValidate="arHeadCountNumericBox"
                      ErrorMessage="Head count is required." ValidationGroup="accroom" ForeColor="Red"
                      Font-Size="10px"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="arHeadCountCompareValidator" Type="Integer" runat="server"
                      SetFocusOnError="true" Display="Dynamic" ControlToValidate="arHeadCountNumericBox"
                      ControlToCompare="arMaxPersonTextBox" Operator="LessThanEqual" ValidationGroup="accroom"
                      ErrorMessage="Head count should not exceed Max Person." ForeColor="Red" Font-Size="10px"></asp:CompareValidator>
                    <asp:CompareValidator ID="arHeadCountZeroCompareValidator" Type="Integer" runat="server"
                      SetFocusOnError="true" Display="Dynamic" ControlToValidate="arHeadCountNumericBox"
                      ValueToCompare="0" Operator="GreaterThan" ValidationGroup="accroom"
                      ErrorMessage="Head count should not be zero." ForeColor="Red" Font-Size="10px"></asp:CompareValidator>
                  </div>
                  <div>
                    <span class="bold">Remarks</span><br />
                    <asp:TextBox ID="arRemarksTextBox" runat="server" Height="40px" CssClass="TextBoxStyle"
                      Width="300px" MaxLength="100" ValidationGroup="accroom" TextMode="MultiLine"></asp:TextBox><br />
                    <asp:RegularExpressionValidator ID="arRemarksLengthValidator" runat="server" SetFocusOnError="true"
                      Display="Dynamic" ControlToValidate="arRemarksTextBox" ValidationExpression="^.{1,100}$"
                      ValidationGroup="accroom" ErrorMessage="Remarks is up to 100 characters only."
                      ForeColor="Red" Font-Size="10px"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="arRemarksRegularExpressionValidator" runat="server"
                      SetFocusOnError="true" Display="Dynamic" ControlToValidate="arRemarksTextBox"
                      ValidationExpression="^[0-9A-Za-z\.,\-'\s?&)(/]+$" ValidationGroup="accroom"
                      ErrorMessage="The only accepted special characters are .,-')(?&/" ForeColor="Red"
                      Font-Size="10px"></asp:RegularExpressionValidator>
                    <asp:Label ID="arScheduleValidationLabel" runat="server" Text="" ForeColor="Red"
                      Font-Size="10px"></asp:Label>
                  </div>
                  <div>
                    <asp:Button ID="arAddButton" runat="server" Text="ADD" Width="80px" CausesValidation="true"
                      ValidationGroup="accroom" OnClick="arAddButton_Click" UseSubmitBehavior="false" />
                    <asp:Button ID="arCancelButton" runat="server" Text="CANCEL" Width="80px" OnClick="arCancelButton_Click"
                      CausesValidation="false" />
                  </div>
                </div>
              </div>
            </asp:Panel>
          </div>
          <asp:Button ID="btnPopUp3" runat="server" Style="display: none" />
          <asp:ModalPopupExtender ID="summaryDetails" runat="server" TargetControlID="btnPopUp3"
            PopupControlID="pnlpopup3" BackgroundCssClass="modalBackground">
          </asp:ModalPopupExtender>
          <div id="summaryDetailsDiv" runat="server" style="display: none;">
            <asp:Panel ID="pnlpopup3" runat="server" BackColor="White">
              <div class="summaryformwrapper">
                <div class="formheaderdiv">
                  <div class="bold">
                    <asp:Label ID="titleLabel" runat="server" Text="RESERVATION REQUEST SUMMARY"></asp:Label>
                  </div>
                </div>
                <div class="formfieldsdiv">
                  <div>
                    <span class="bold">Event Name</span><br />
                    <asp:TextBox ID="eventNameTextBox" runat="server" Height="40px" CssClass="TextBoxStyle"
                      Width="300px" MaxLength="100" ValidationGroup="summary" TextMode="MultiLine"></asp:TextBox><br />
                    <asp:RequiredFieldValidator ID="eventNameRequiredFieldValidator" runat="server" SetFocusOnError="true"
                      Display="Dynamic" ControlToValidate="eventNameTextBox" ErrorMessage="Event name is required."
                      ValidationGroup="summary" ForeColor="Red" Font-Size="10px"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="eventNameRegularExpressionValidator" runat="server"
                      SetFocusOnError="true" Display="Dynamic" ControlToValidate="eventNameTextBox"
                      ValidationExpression="^[0-9A-Za-z\.,\-'\s&)(/]+$" ValidationGroup="summary" ErrorMessage="Event name should not contain special char."
                      ForeColor="Red" Font-Size="10px"></asp:RegularExpressionValidator>
                  </div>
                  <div>
                    <span class="bold">Company</span><br />
                    <asp:DropDownList ID="costCenterDropDownList" runat="server" CssClass="DropDownStyle"
                      Width="300px" ValidationGroup="summary" AutoPostBack="true" OnSelectedIndexChanged="costCenterDropDownList_SelectedIndexChanged">
                    </asp:DropDownList>
                    <br />
                    <asp:RequiredFieldValidator ID="costCenterRequiredFieldValidator" runat="server"
                      SetFocusOnError="true" Display="Dynamic" ControlToValidate="costCenterDropDownList"
                      ErrorMessage="Please select a Company." ValidationGroup="summary" ForeColor="Red"
                      Font-Size="10px" InitialValue="0"></asp:RequiredFieldValidator>
                  </div>
                  <div>
                    <span class="bold">Cost Center</span><br />
                    <asp:DropDownList ID="chargedCompanyCostCenterDropdownList" runat="server" CssClass="DropDownStyle"
                      Width="300px" ValidationGroup="summary">
                    </asp:DropDownList>
                    <br />
                    <asp:RequiredFieldValidator ID="chargedCompanyCostCenterRequiredFieldValidator" runat="server"
                      SetFocusOnError="true" Display="Dynamic" ControlToValidate="chargedCompanyCostCenterDropdownList"
                      ErrorMessage="Please select a Cost Center." ValidationGroup="summary" ForeColor="Red"
                      Font-Size="10px" InitialValue="0"></asp:RequiredFieldValidator>
                  </div>
                  <div>
                    <span class="bold">Immediate Head Approval</span><br />
                    <asp:FileUpload ID="approvalFileUpload" runat="server" Width="200px" ValidationGroup="summary" /><br />
                    <asp:RequiredFieldValidator ID="approvalRequiredFieldValidator" runat="server" SetFocusOnError="true"
                      Display="Dynamic" ControlToValidate="approvalFileUpload" ErrorMessage="Approval is required."
                      ValidationGroup="summary" ForeColor="Red" Font-Size="10px"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="approvalRegularExpressionValidator" runat="server"
                      SetFocusOnError="true" Display="Dynamic" ControlToValidate="approvalFileUpload"
                      ValidationExpression="([a-zA-Z0-9\s_\\.\-\[\]\{\},;!@#$%^&)('+=:])+(.msg|.MSG|.doc|.DOC|.docx|.DOCX|.jpg|.JPG|.png|.PNG|.gif|.GIF)$"
                      ValidationGroup="summary" ErrorMessage="Valid file types are: .msg|.doc|.docx|.jpg|.png|.gif"
                      ForeColor="Red" Font-Size="10px"></asp:RegularExpressionValidator>
                    <asp:Label ID="approvalLabel" runat="server" Text="" ForeColor="Red" Font-Size="10px"></asp:Label>
                  </div>
                  <div>
                    <span class="bold">Remarks</span><br />
                    <asp:TextBox ID="remarksTextBox" runat="server" Height="40px" CssClass="TextBoxStyle"
                      Width="300px" MaxLength="100" ValidationGroup="summary" TextMode="MultiLine"></asp:TextBox><br />
                    <asp:RegularExpressionValidator ID="remarksRegularExpressionValidator" runat="server"
                      SetFocusOnError="true" Display="Dynamic" ControlToValidate="remarksTextBox" ValidationExpression="^.{1,100}$"
                      ValidationGroup="summary" ErrorMessage="Remarks is up to 100 characters only."
                      ForeColor="Red" Font-Size="10px"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" SetFocusOnError="true"
                      Display="Dynamic" ControlToValidate="remarksTextBox" ValidationExpression="^[0-9A-Za-z\.,\-'\s?&)(/]+$"
                      ValidationGroup="summary" ErrorMessage="The only accepted special characters are .,-')(?&/"
                      ForeColor="Red" Font-Size="10px"></asp:RegularExpressionValidator>
                  </div>
                  <div>
                    <span class="bold">Training Rooms</span><br />
                    <asp:GridView ID="summaryTrainingRoomGridView" runat="server" OnRowDeleting="summaryTrainingRoomGridView_RowDeleting"
                      AutoGenerateColumns="false" CssClass="summarygrid">
                      <HeaderStyle BackColor="#507CD1" ForeColor="#FFFFFF" Font-Bold="true" />
                      <RowStyle BackColor="#EFF3FB"></RowStyle>
                      <Columns>
                        <asp:TemplateField>
                          <ItemTemplate>
                            <%#Container.DataItemIndex+1 %>
                          </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="PartitionID" HeaderText="Room Name" HeaderStyle-CssClass="hiddencol"
                          ItemStyle-CssClass="hiddencol" />
                        <asp:BoundField DataField="PartitionName" HeaderText="Room Name" HeaderStyle-CssClass="summarytrname" />
                        <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-CssClass="summarydate" />
                        <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-CssClass="summarydate" />
                        <asp:BoundField DataField="NumberOfDays" HeaderText="Days" HeaderStyle-CssClass="summarydays" />
                        <asp:BoundField DataField="HeadCount" HeaderText="Head Count" HeaderStyle-CssClass="summarycount" />
                        <asp:BoundField DataField="EquipmentToUse" HeaderText="Equipment To Use" HeaderStyle-CssClass="summarytreq" />
                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" HeaderStyle-CssClass="summarytrrem" />
                        <asp:TemplateField HeaderText="">
                          <ItemTemplate>
                            <asp:Button ID="deleteButton" runat="server" CommandName="Delete" Text="Delete" CssClass="gridviewselect"
                              OnClientClick="return confirm('Are you sure you want to delete this room?');" />
                          </ItemTemplate>
                        </asp:TemplateField>
                      </Columns>
                    </asp:GridView>
                  </div>
                  <div>
                    <span class="bold">Accomodation Rooms</span><br />
                    <asp:GridView ID="summaryAccomodationRoomGridView" runat="server" OnRowDeleting="summaryAccomodationRoomGridView_RowDeleting"
                      AutoGenerateColumns="false" CssClass="summarygrid">
                      <HeaderStyle BackColor="#507CD1" ForeColor="#FFFFFF" Font-Bold="true" />
                      <RowStyle BackColor="#EFF3FB"></RowStyle>
                      <Columns>
                        <asp:TemplateField>
                          <ItemTemplate>
                            <%#Container.DataItemIndex+1 %>
                          </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="AccRoomID" HeaderText="Room Name" HeaderStyle-CssClass="hiddencol"
                          ItemStyle-CssClass="hiddencol" />
                        <asp:BoundField DataField="AccRoomName" HeaderText="Room Name" HeaderStyle-CssClass="summaryarname" />
                        <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-CssClass="summarydate" />
                        <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-CssClass="summarydate" />
                        <asp:BoundField DataField="NumberOfNights" HeaderText="Nights" HeaderStyle-CssClass="summarynights" />
                        <asp:BoundField DataField="HeadCount" HeaderText="Head Count" HeaderStyle-CssClass="summarycount" />
                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" HeaderStyle-CssClass="summaryarrem" />
                        <asp:TemplateField HeaderText="">
                          <ItemTemplate>
                            <asp:Button ID="deleteButton" runat="server" CommandName="Delete" Text="Delete" CssClass="gridviewselect"
                              OnClientClick="return confirm('Are you sure you want to delete this room?');" />
                          </ItemTemplate>
                        </asp:TemplateField>
                      </Columns>
                    </asp:GridView>
                    <asp:Label ID="summaryValidationLabel" runat="server" Text="" ForeColor="Red" Font-Size="10px"></asp:Label>
                  </div>
                  <div>
                    <asp:Button ID="submitButton" runat="server" Text="SUBMIT" Width="80px" CausesValidation="true"
                      ValidationGroup="summary" OnClick="submitButton_Click" OnClientClick="ButtonLoading()"
                      UseSubmitBehavior="false" />
                    <asp:Button ID="clearButton" runat="server" Text="CLEAR" Width="80px" OnClick="clearButton_Click"
                      CausesValidation="false" OnClientClick="return confirm('All entries in the request summary will be deleted. Are you sure you want to proceed?')" />
                    <asp:Button ID="backButton" runat="server" Text="BACK" Width="80px" OnClick="backButton_Click"
                      CausesValidation="false" />
                  </div>
                </div>
              </div>
            </asp:Panel>
          </div>
        </div>
      </ContentTemplate>
      <Triggers>
        <asp:PostBackTrigger ControlID="trAddButton" />
        <asp:PostBackTrigger ControlID="arAddButton" />
        <asp:PostBackTrigger ControlID="submitButton" />
      </Triggers>
    </asp:UpdatePanel>
  </form>
</body>
</html>
