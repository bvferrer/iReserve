<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CalendarConferenceRoomView.aspx.cs"
  Inherits="CalendarConferenceRoomView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>iReserve</title>
  <link rel="SHORTCUT ICON" href="img/PEL.ico" />
  <link href="css/Style.css" media="screen" rel="Stylesheet" />
  <script src="js/jquery-1.12.0.js"></script>
  <%--<script src="js/jquery-1.9.1.js" type="text/javascript"></script>--%>
  <%--<script src="js/jquery-ui-1.10.0.custom.js"></script>--%>
</head>
<body>
  <script type="text/javascript">
    function cellClicked(refNo, e) {
      document.getElementById('inhAction').value = "CELLCLICKED";
      document.getElementById('inhRefNumber').value = refNo;
      document.getElementById('hiddenButton').click();
    }
  </script>
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" />
    <asp:UpdatePanel ID="calendarUpdatePanel" runat="server">
      <ContentTemplate>
        <div class="calContainer">
          <asp:Button ID="hiddenButton" runat="server" Text="" OnClick="hiddenButton_Click"
            Style="display: none;" />
          <input runat="server" type="hidden" id="inhAction" />
          <input runat="server" type="hidden" id="inhRefNumber" />
          <div class="innerNav">
            <asp:LinkButton ID="homeLinkButton" runat="server" PostBackUrl="~/Default.aspx" Text="Home"></asp:LinkButton>
            <span>| Conference Room Reservation Calendar</span>
          </div>
          <div class="room">
            <span>Conference Room: </span>
            <asp:DropDownList ID="conferenceRoomDropDownList" runat="server" CssClass="DropDownStyle"
              AutoPostBack="true" OnSelectedIndexChanged="conferenceRoomDropDownList_SelectedIndexChanged">
            </asp:DropDownList>
          </div>
          <div class="calendarDatePicker">
            <span>Date: </span>
            <asp:TextBox ID="datepicker" runat="server" MaxLength="10" onkeypress="return false;"
              AutoCompleteType="Disabled" OnTextChanged="datepicker_TextChanged" AutoPostBack="True"></asp:TextBox>
            <asp:CalendarExtender ID="CalendarExtender" Format="MM/dd/yyyy" TargetControlID="datepicker"
              runat="server" PopupButtonID="datepicker" />
          </div>
          <div class="calendarNavigation">
            <span>
              <asp:LinkButton ID="previousLinkButton" class="prev" runat="server" OnClick="previousLinkButton_Click">Previous</asp:LinkButton>
            </span><span>
              <asp:LinkButton ID="nextLinkButton" class="next" runat="server" OnClick="nextLinkButton_Click">Next</asp:LinkButton>
            </span>
          </div>
          <div id="calendarHeaderTable" runat="server" class="calendarHeader">
          </div>
          <div class="scheduleGridviewWrapper">
            <asp:GridView ID="scheduleGridview" runat="server" OnRowDataBound="scheduleGridview_RowDataBound"
              Font-Names="Arial" Font-Size="9pt" CssClass="scheduleGridviewCSS" ShowHeader="false">
            </asp:GridView>
          </div>
          <asp:Button ID="btnPopUp" runat="server" Style="display: none" />
          <asp:ModalPopupExtender ID="roomDetails" runat="server" TargetControlID="btnPopUp"
            PopupControlID="pnlpopup" BackgroundCssClass="modalBackground">
          </asp:ModalPopupExtender>
          <div id="roomDetailsDiv" runat="server" style="display: none;">
            <asp:Panel ID="pnlpopup" runat="server" BackColor="White">
              <div class="formwrapper">
                <div class="formheaderdiv">
                  <div class="bold">
                    <asp:Label ID="headerHistoryLabel" runat="server" Text="Reservation Details"></asp:Label>
                  </div>
                </div>
                <div class="formfieldsdiv">
                  <div>
                    <span class="bold">Reference Number :</span>
                    <asp:Label ID="refNumberLabel" runat="server" Text=""></asp:Label>
                  </div>
                  <div>
                    <span class="bold">Room :</span>
                    <asp:Label ID="roomLabel" runat="server" Text=""></asp:Label>
                  </div>
                  <div>
                    <span class="bold">Date :</span>
                    <asp:Label ID="dateLabel" runat="server" Text=""></asp:Label>
                  </div>
                  <div>
                    <span class="bold">Time :</span>
                    <asp:Label ID="fromLabel" runat="server" Text=""></asp:Label>
                    <span>- </span>
                    <asp:Label ID="toLabel" runat="server" Text=""></asp:Label>
                  </div>
                  <div>
                    <span class="bold">Agenda :</span>
                    <asp:Label ID="agendaLabel" runat="server" Text=""></asp:Label>
                  </div>
                  <div>
                    <span class="bold">Company :</span>
                    <asp:Label ID="chargedCompanyLabel" runat="server" Text=""></asp:Label>
                  </div>
                  <div>
                    <span class="bold">Cost Center :</span>
                    <asp:Label ID="costCenterLabel" runat="server" Text=""></asp:Label>
                  </div>
                  <div>
                    <span class="bold">Requested By :</span>
                    <asp:Label ID="requestedByLabel" runat="server" Text=""></asp:Label>
                  </div>
                  <div>
                    <asp:Button ID="cancelButton" runat="server" Text="CLOSE" Width="80px" OnClick="cancelButton_Click"
                      CausesValidation="false" />
                  </div>
                </div>
              </div>
            </asp:Panel>
          </div>
        </div>
      </ContentTemplate>
    </asp:UpdatePanel>
  </form>
</body>
</html>
