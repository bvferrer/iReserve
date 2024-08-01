<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CalendarConventionCenterAdmin.aspx.cs"
    Inherits="CalendarConventionCenterAdmin" %>

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

        function BlockButtonLoading() {
            if (!Page_ClientValidate('block')) {
                return false;
            }

            submit = document.getElementById('blockButton');
            submit.disabled = true;
            submit.value = 'Please wait...';

            document.getElementById('tranCancelButton').disabled = true;
        }

        function UnblockButtonLoading() {
            submit = document.getElementById('unblockButton');
            submit.disabled = true;
            submit.value = 'Please wait...';

            document.getElementById('tranCancelButton').disabled = true;
        }
    </script>
</head>
<body>
    <script type="text/javascript">
        function cellClicked(refNo, e) {
            document.getElementById('inhAction').value = "CELLCLICKED";
            document.getElementById('inhRefNumber').value = refNo;
            document.getElementById('hiddenButton').click();
        }
        function trTranCellClicked(type, row, column, refNo, room, e) {
            document.getElementById('inhAction').value = "CELLCLICKED";
            document.getElementById('inhRefNumber').value = refNo;
            document.getElementById('inhRoom').value = room;
            document.getElementById('inhRow').value = row;
            document.getElementById('inhColumn').value = column;
            document.getElementById('inhType').value = type;
            document.getElementById('trHiddenButton').click();
        }
        function arTranCellClicked(type, row, column, refNo, room, e) {
            document.getElementById('inhAction').value = "CELLCLICKED";
            document.getElementById('inhRefNumber').value = refNo;
            document.getElementById('inhRoom').value = room;
            document.getElementById('inhRow').value = row;
            document.getElementById('inhColumn').value = column;
            document.getElementById('inhType').value = type;
            document.getElementById('arHiddenButton').click();
        }
    </script>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" />
    <asp:UpdatePanel ID="calendarUpdatePanel" runat="server">
        <ContentTemplate>
            <div class="calContainer">
                <asp:Button ID="hiddenButton" runat="server" Text="" OnClick="hiddenButton_Click"
                    Style="display: none;" />
                <asp:Button ID="trHiddenButton" runat="server" Text="" OnClick="trHiddenButton_Click"
                    Style="display: none;" />
                <asp:Button ID="arHiddenButton" runat="server" Text="" OnClick="arHiddenButton_Click"
                    Style="display: none;" />
                <input runat="server" type="hidden" id="inhAction" />
                <input runat="server" type="hidden" id="inhRefNumber" />
                <input runat="server" type="hidden" id="inhRoom" />
                <input runat="server" type="hidden" id="inhRow" />
                <input runat="server" type="hidden" id="inhColumn" />
                <input runat="server" type="hidden" id="inhType" />
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
                <asp:Button ID="btnPopUp2" runat="server" Style="display: none" />
                <asp:ModalPopupExtender ID="trainingRoomDetails" runat="server" TargetControlID="btnPopUp"
                    PopupControlID="pnlpopup" BackgroundCssClass="modalBackground">
                </asp:ModalPopupExtender>
                <asp:ModalPopupExtender ID="tranDetails" runat="server" TargetControlID="btnPopUp2"
                    PopupControlID="pnlpopup2" BackgroundCssClass="modalBackground">
                </asp:ModalPopupExtender>
                <div id="trainingRoomDetailsDiv" runat="server" style="display: none;">
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
                                    <span class="bold">Event Name :</span>
                                    <asp:Label ID="eventNameLabel" runat="server" Text=""></asp:Label>
                                </div>
                                <div>
                                    <span class="bold">Start Date :</span>
                                    <asp:Label ID="startDateLabel" runat="server" Text=""></asp:Label>
                                </div>
                                <div>
                                    <span class="bold">End Date :</span>
                                    <asp:Label ID="endDateLabel" runat="server" Text=""></asp:Label>
                                </div>
                                <div>
                                    <span class="bold">Requestor :</span>
                                    <asp:Label ID="createdByLabel" runat="server" Text=""></asp:Label>
                                </div>
                                <div>
                                    <span class="bold">Cost Center :</span>
                                    <asp:Label ID="costCenterLabel" runat="server" Text=""></asp:Label>
                                </div>
                                <div>
                                    <span class="bold">Date Requested :</span>
                                    <asp:Label ID="dateCreatedLabel" runat="server" Text=""></asp:Label>
                                </div>
                                <div>
                                    <asp:Button ID="cancelButton" runat="server" Text="CLOSE" Width="80px" OnClick="cancelButton_Click"
                                        CausesValidation="false" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <div id="tranDetailsDiv" runat="server" style="display: none;">
                    <asp:Panel ID="pnlpopup2" runat="server" BackColor="White">
                        <div class="formwrapper">
                            <div class="formheaderdiv">
                                <div class="bold">
                                    <asp:Label ID="tranLabel" runat="server" Text=""></asp:Label>
                                </div>
                                <div>
                                    <asp:Label ID="roomTypeLabel" runat="server" Text="" Visible="false"></asp:Label>
                                    <asp:Label ID="roomIDLabel" runat="server" Text="" Visible="false"></asp:Label>
                                    <asp:Label ID="roomNameLabel" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <div class="formfieldsdiv">
                                <asp:Panel ID="blockPanel" runat="server">
                                    <div>
                                        <div class="formCalendarDatePicker">
                                            <span class="bold">From</span>
                                            <asp:TextBox ID="blockStartDateTextBox" runat="server" MaxLength="10" onkeypress="return false;"
                                                AutoCompleteType="Disabled" ValidationGroup="block" Width="100px"></asp:TextBox>
                                            <asp:CalendarExtender ID="blockStartDateCalendarExtender" Format="MM/dd/yyyy" TargetControlID="blockStartDateTextBox"
                                                runat="server" PopupButtonID="blockStartDate" />
                                            <span class="bold">To</span>
                                            <asp:TextBox ID="blockEndDateTextBox" runat="server" MaxLength="10" onkeypress="return false;"
                                                AutoCompleteType="Disabled" ValidationGroup="block" Width="100px"></asp:TextBox>
                                            <asp:CalendarExtender ID="blockEndDateCalendarExtender" Format="MM/dd/yyyy" TargetControlID="blockEndDateTextBox"
                                                runat="server" PopupButtonID="blockEndDate" />
                                        </div>
                                        <asp:TextBox ID="blockDateTodayTextBox" runat="server" ValidationGroup="block" CssClass="hiddencol"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="blockStartDateRequiredFieldValidator" runat="server"
                                            SetFocusOnError="true" Display="Dynamic" ControlToValidate="blockStartDateTextBox"
                                            ErrorMessage="Start date is required." ValidationGroup="block" ForeColor="Red"
                                            Font-Size="10px"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="blockEndDateRequiredFieldValidator" runat="server"
                                            SetFocusOnError="true" Display="Dynamic" ControlToValidate="blockEndDateTextBox"
                                            ErrorMessage="End date is required." ValidationGroup="block" ForeColor="Red"
                                            Font-Size="10px"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="blockStartDateCompareValidator" Type="Date" runat="server"
                                            SetFocusOnError="true" Display="Dynamic" ControlToValidate="blockStartDateTextBox"
                                            ControlToCompare="blockDateTodayTextBox" Operator="GreaterThan" ValidationGroup="block"
                                            ErrorMessage="Past/Present start date is not allowed." ForeColor="Red" Font-Size="10px"></asp:CompareValidator>
                                        <asp:CompareValidator ID="blockEndDateCompareValidator" Type="Date" runat="server"
                                            SetFocusOnError="true" Display="Dynamic" ControlToValidate="blockEndDateTextBox"
                                            ControlToCompare="blockDateTodayTextBox" Operator="GreaterThan" ValidationGroup="block"
                                            ErrorMessage="Past/Present end date is not allowed." ForeColor="Red" Font-Size="10px"></asp:CompareValidator>
                                        <asp:CompareValidator ID="blockStartEndDateCompareValidator" Type="Date" runat="server"
                                            SetFocusOnError="true" Display="Dynamic" ControlToValidate="blockStartDateTextBox"
                                            ControlToCompare="blockEndDateTextBox" Operator="LessThanEqual" ValidationGroup="block"
                                            ErrorMessage="End date should be later than start date." ForeColor="Red" Font-Size="10px"></asp:CompareValidator>
                                    </div>
                                    <div>
                                        <span class="bold">Remarks</span><br />
                                        <asp:TextBox ID="blockRemarksTextBox" runat="server" Height="40px" CssClass="TextBoxStyle"
                                            Width="300px" MaxLength="100" ValidationGroup="block" TextMode="MultiLine"></asp:TextBox><br />
                                        <asp:RequiredFieldValidator ID="blockRemarksRequiredFieldValidator" runat="server"
                                            SetFocusOnError="true" Display="Dynamic" ControlToValidate="blockRemarksTextBox"
                                            ErrorMessage="Remarks is required." ValidationGroup="block" ForeColor="Red" Font-Size="10px"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="blockRemarksLengthValidator" runat="server" SetFocusOnError="true"
                                            Display="Dynamic" ControlToValidate="blockRemarksTextBox" ValidationExpression="^.{1,100}$"
                                            ValidationGroup="block" ErrorMessage="Remarks is up to 100 characters only."
                                            ForeColor="Red" Font-Size="10px"></asp:RegularExpressionValidator>
                                        <asp:RegularExpressionValidator ID="blockRemarksRegularExpressionValidator" runat="server"
                                            SetFocusOnError="true" Display="Dynamic" ControlToValidate="blockRemarksTextBox"
                                            ValidationExpression="^[0-9A-Za-z\.,\-'\s?&)(/]+$" ValidationGroup="block" ErrorMessage="The only accepted special characters are .,-')(?&/"
                                            ForeColor="Red" Font-Size="10px"></asp:RegularExpressionValidator>
                                        <asp:Label ID="blockScheduleValidationLabel" runat="server" Text="" ForeColor="Red"
                                            Font-Size="10px"></asp:Label>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="unblockPanel" runat="server">
                                    <div>
                                        <span class="bold">Remarks :</span>
                                        <asp:Label ID="remarksLabel" runat="server" Text=""></asp:Label>
                                    </div>
                                </asp:Panel>
                                <div>
                                    <asp:Button ID="blockButton" runat="server" Text="BLOCK" Width="80px" CausesValidation="true"
                                        ValidationGroup="block" OnClick="blockButton_Click" OnClientClick="BlockButtonLoading()"
                                        UseSubmitBehavior="false" />
                                    <asp:Button ID="unblockButton" runat="server" Text="UNBLOCK" Width="80px" CausesValidation="true"
                                        ValidationGroup="unblock" OnClick="unblockButton_Click" OnClientClick="UnblockButtonLoading()"
                                        UseSubmitBehavior="false" />
                                    <asp:Button ID="tranCancelButton" runat="server" Text="CLOSE" Width="80px" OnClick="tranCancelButton_Click"
                                        CausesValidation="false" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="blockButton" />
            <asp:PostBackTrigger ControlID="unblockButton" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
