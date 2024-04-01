<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CalendarConferenceRoom.aspx.cs"
    Inherits="CalendarConferenceRoom" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>iReserve</title>
    <link rel="SHORTCUT ICON" href="img/PEL.ico"/>
    <link href="css/Style.css" media="screen" rel="Stylesheet" />
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

        function ButtonLoading(obj) {
            if (!Page_ClientValidate('form')) {
                return false;
            }
            obj.disabled = true;
            obj.value = 'Please wait...';
        }
    </script>
</head>
<body>
    <script type="text/javascript">
        function cellClicked(row, column, e) {
            document.getElementById('inhAction').value = "CELLCLICKED";
            document.getElementById('inhRow').value = row;
            document.getElementById('inhColumn').value = column;
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
                <input runat="server" type="hidden" id="inhRow" />
                <input runat="server" type="hidden" id="inhColumn" />
                <div class="innerNav">
                    <asp:LinkButton ID="homeLinkButton" runat="server" PostBackUrl="~/Default.aspx" Text="Home"></asp:LinkButton>
                    <span>| Conference Room Reservation Calendar</span>
                </div>
                <div class="room">
                    <span>Conference Room: </span>
                    <asp:DropDownList ID="conferenceRoomDropDownList" runat="server" 
                        CssClass="DropDownStyle" AutoPostBack="true" 
                        onselectedindexchanged="conferenceRoomDropDownList_SelectedIndexChanged">
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
                <%--<asp:Button ID="btnLoading" runat="server" Style="display: none" />
                <asp:ModalPopupExtender ID="loadingModalPopupExtender" runat="server" TargetControlID="btnLoading"
                    PopupControlID="pnlProgress" BackgroundCssClass="modalBackground">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlProgress" runat="server" Style="background-color: #ffffff; display: none;"
                    Width="290px">
                    <div style="padding: 8px">
                        <table border="0" cellpadding="2" cellspacing="0" style="width: 100%">
                            <tbody>
                                <tr>
                                    <td style="width: 50%">
                                    </td>
                                    <td style="text-align: right">
                                        <img alt="" src="img/indicator-big.gif" />
                                    </td>
                                    <td style="text-align: left; white-space: nowrap">
                                        <span style="font-size: larger; font-family: Verdana;">Please wait...</span>
                                    </td>
                                    <td style="width: 50%">
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </asp:Panel>--%>
                <asp:Button ID="btnPopUp" runat="server" Style="display: none" />
                <asp:ModalPopupExtender ID="roomDetails" runat="server" TargetControlID="btnPopUp"
                    PopupControlID="pnlpopup" BackgroundCssClass="modalBackground">
                </asp:ModalPopupExtender>
                <div id="roomDetailsDiv" runat="server" style="display: none;">
                    <asp:Panel ID="pnlpopup" runat="server" BackColor="White">
                        <div class="formwrapper">
                            <div class="formheaderdiv">
                                <div class="bold">
                                    <asp:Label ID="roomNameLabel" runat="server" Text="ROOM NAME"></asp:Label>
                                    <span>- </span>
                                    <asp:Label ID="dateLabel" runat="server" Text="Date"></asp:Label>
                                </div>
                                <div>
                                    <span class="bold">Location: </span>
                                    <asp:Label ID="locationLabel" runat="server" Text="Location"></asp:Label>
                                    <span class="bold">Max Person: </span>
                                    <asp:Label ID="maxPersonLabel" runat="server" Text="3"></asp:Label>
                                </div>
                                <div>
                                    <span class="bold">Details: </span>
                                    <asp:Label ID="detailsLabel" runat="server" Text="Details"></asp:Label>
                                </div>
                            </div>
                            <div class="formfieldsdiv">
                                <div>
                                    <span class="bold">From: </span>
                                    <asp:DropDownList ID="fromDropDownList" runat="server" CssClass="DropDownStyle" Width="100px">
                                    </asp:DropDownList>
                                    <span class="bold">To: </span>
                                    <asp:DropDownList ID="toDropDownList" runat="server" CssClass="DropDownStyle" Width="100px">
                                    </asp:DropDownList>
                                    <br />
                                    <asp:CompareValidator ID="timeCompareValidator" Type="Integer" runat="server" SetFocusOnError="true"
                                        Display="Dynamic" ControlToValidate="toDropDownList" ControlToCompare="fromDropDownList"
                                        ValueToCompare="Value" Operator="GreaterThanEqual" ValidationGroup="form" ErrorMessage="End Time should be later than Start Time."
                                        ForeColor="Red" Font-Size="10px"></asp:CompareValidator>
                                </div>
                                <div>
                                    <span class="bold">Agenda</span><br />
                                    <asp:TextBox ID="agendaTextBox" runat="server" Height="40px" CssClass="TextBoxStyle"
                                        Width="300px" MaxLength="100" ValidationGroup="form" TextMode="MultiLine"></asp:TextBox><br />
                                    <asp:RequiredFieldValidator ID="agendaRequiredFieldValidator" runat="server" SetFocusOnError="true"
                                        Display="Dynamic" ControlToValidate="agendaTextBox" ErrorMessage="Agenda is required."
                                        ValidationGroup="form" ForeColor="Red" Font-Size="10px"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="agendaRegularExpressionValidator" runat="server"
                                        SetFocusOnError="true" Display="Dynamic" ControlToValidate="agendaTextBox" ValidationExpression="^[0-9A-Za-z\.,\-'\s&)(/]+$"
                                        ValidationGroup="form" ErrorMessage="Agenda should not contain special char."
                                        ForeColor="Red" Font-Size="10px"></asp:RegularExpressionValidator>
                                </div>
                                <div>
                                    <span class="bold">Attendees</span><br />
                                    <span>Name: </span>
                                    <asp:TextBox ID="nameTextBox" runat="server" Width="100px" MaxLength="100" ValidationGroup="attendees"></asp:TextBox>
                                    <span>Company: </span>
                                    <asp:TextBox ID="companyTextBox" runat="server" Width="100px" MaxLength="100" ValidationGroup="attendees"></asp:TextBox>
                                    <asp:Button ID="addButton" runat="server" Text="ADD" Width="50px" ValidationGroup="attendees"
                                        CausesValidation="true" OnClick="addButton_Click" /><br />
                                    <asp:RequiredFieldValidator ID="nameRequiredFieldValidator" runat="server" SetFocusOnError="true"
                                        Display="Dynamic" ControlToValidate="nameTextBox" ErrorMessage="Name is required."
                                        ValidationGroup="attendees" ForeColor="Red" Font-Size="10px"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="nameRegularExpressionValidator" runat="server"
                                        SetFocusOnError="true" Display="Dynamic" ControlToValidate="nameTextBox" ValidationExpression="^[A-Za-z\.,\-'\s]+$"
                                        ValidationGroup="attendees" ErrorMessage="Name should not contain num/special char."
                                        ForeColor="Red" Font-Size="10px"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="companyRequiredFieldValidator" runat="server" SetFocusOnError="true"
                                        Display="Dynamic" ControlToValidate="companyTextBox" ErrorMessage="Company is required."
                                        ValidationGroup="attendees" ForeColor="Red" Font-Size="10px"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="companyRegularExpressionValidator" runat="server"
                                        SetFocusOnError="true" Display="Dynamic" ControlToValidate="companyTextBox" ValidationExpression="^[0-9A-Za-z\.,\-'\s&)(/]+$"
                                        ValidationGroup="attendees" ErrorMessage="Company should not contain special char."
                                        ForeColor="Red" Font-Size="10px"></asp:RegularExpressionValidator>
                                    <asp:GridView ID="attendeeGridView" runat="server" OnRowDeleting="attendeeGridView_RowDeleting"
                                        AutoGenerateColumns="false" CssClass="attendeegrid">
                                        <HeaderStyle BackColor="#507CD1" ForeColor="#FFFFFF" Font-Bold="true" />
                                        <RowStyle BackColor="#EFF3FB"></RowStyle>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Name" HeaderText="Name" />
                                            <asp:BoundField DataField="Company" HeaderText="Company" />
                                            <asp:CommandField ShowDeleteButton="True" ButtonType="button">
                                                <ItemStyle CssClass="gridviewselect" />
                                            </asp:CommandField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:TextBox ID="maxPersonTextBox" runat="server" ValidationGroup="form" CssClass="hiddencol"></asp:TextBox>
                                    <asp:TextBox ID="countTextBox" runat="server" ValidationGroup="form" CssClass="hiddencol"></asp:TextBox>
                                    <asp:CompareValidator ID="zeroCountCompareValidator" Type="Integer" runat="server"
                                        SetFocusOnError="true" Display="Dynamic" ControlToValidate="countTextBox" ValueToCompare="0"
                                        Operator="GreaterThan" ValidationGroup="form" ErrorMessage="Please add Attendees."
                                        ForeColor="Red" Font-Size="10px"></asp:CompareValidator>
                                    <asp:CompareValidator ID="countCompareValidator" Type="Integer" runat="server" SetFocusOnError="true"
                                        Display="Dynamic" ControlToValidate="countTextBox" ControlToCompare="maxPersonTextBox"
                                        Operator="LessThanEqual" ValidationGroup="form" ErrorMessage="Attendees should not exceed Max Person."
                                        ForeColor="Red" Font-Size="10px"></asp:CompareValidator>
                                </div>
                                <div>
                                    <span class="bold">Equipment To Use</span><br />
                                    <asp:CheckBox ID="dataPortCheckBox" runat="server" Text="Data Port/Wifi" Checked="false" />
                                    <asp:CheckBox ID="monitorCheckBox" runat="server" Text="Monitor Display" Checked="false" />
                                </div>
                                <div>
                                    <span class="bold">Cost Center</span><br />
                                    <asp:DropDownList ID="costCenterDropDownList" runat="server" CssClass="DropDownStyle"
                                        Width="300px" ValidationGroup="form">
                                    </asp:DropDownList>
                                    <br />
                                    <asp:RequiredFieldValidator ID="costCenterRequiredFieldValidator" runat="server"
                                        SetFocusOnError="true" Display="Dynamic" ControlToValidate="costCenterDropDownList"
                                        ErrorMessage="Please select Cost Center." ValidationGroup="form" ForeColor="Red"
                                        Font-Size="10px" InitialValue="0"></asp:RequiredFieldValidator>
                                </div>
                                <div>
                                    <span class="bold">Immediate Head Approval</span><br />
                                    <asp:FileUpload ID="approvalFileUpload" runat="server" Width="200px" ValidationGroup="form" /><br />
                                    <asp:RequiredFieldValidator ID="approvalRequiredFieldValidator" runat="server" SetFocusOnError="true"
                                        Display="Dynamic" ControlToValidate="approvalFileUpload" ErrorMessage="Approval is required."
                                        ValidationGroup="form" ForeColor="Red" Font-Size="10px"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="approvalRegularExpressionValidator" runat="server"
                                        SetFocusOnError="true" Display="Dynamic" ControlToValidate="approvalFileUpload"
                                        ValidationExpression="([a-zA-Z0-9\s_\\.\-\[\]\{\},;!@#$%^&)('+=:])+(.msg|.MSG|.doc|.DOC|.docx|.DOCX|.jpg|.JPG|.png|.PNG|.gif|.GIF)$"
                                        ValidationGroup="form" ErrorMessage="Valid file types are: .msg|.doc|.docx|.jpg|.png|.gif"
                                        ForeColor="Red" Font-Size="10px"></asp:RegularExpressionValidator>
                                    <asp:Label ID="approvalLabel" runat="server" Text="" ForeColor="Red" Font-Size="10px"></asp:Label>
                                    <asp:CustomValidator ID="approvalCustomValidator" runat="server" ForeColor="Red" Font-Size="10px"
                                        SetFocusOnError="true" Display="Dynamic" ErrorMessage="Allowed attachment file size is up to 3MB only."></asp:CustomValidator>
                                </div>
                                <div>
                                    <span class="bold">Remarks</span><br />
                                    <asp:TextBox ID="remarksTextBox" runat="server" Height="40px" CssClass="TextBoxStyle"
                                        Width="300px" MaxLength="100" ValidationGroup="form" TextMode="MultiLine"></asp:TextBox><br />
                                    <asp:RequiredFieldValidator ID="remarksRequiredFieldValidator" runat="server" SetFocusOnError="true"
                                            Display="Dynamic" ControlToValidate="remarksTextBox" ErrorMessage="Remarks is required."
                                            ValidationGroup="form" ForeColor="Red" Font-Size="10px"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="remarksLengthValidator" runat="server"
                                        SetFocusOnError="true" Display="Dynamic" ControlToValidate="remarksTextBox" ValidationExpression="^.{1,100}$"
                                        ValidationGroup="form" ErrorMessage="Remarks is up to 100 characters only."
                                        ForeColor="Red" Font-Size="10px"></asp:RegularExpressionValidator>
                                    <asp:RegularExpressionValidator ID="remarksRegularExpressionValidator" runat="server"
                                        SetFocusOnError="true" Display="Dynamic" ControlToValidate="remarksTextBox" ValidationExpression="^[0-9A-Za-z\.,\-'\s?&)(/]+$"
                                        ValidationGroup="form" ErrorMessage="The only accepted special characters are .,-')(?&/"
                                        ForeColor="Red" Font-Size="10px"></asp:RegularExpressionValidator>
                                    <asp:Label ID="scheduleValidationLabel" runat="server" Text="" ForeColor="Red" Font-Size="10px"></asp:Label>
                                </div>
                                <div>
                                    <asp:Button ID="submitButton" runat="server" Text="SUBMIT" Width="80px" CausesValidation="true"
                                        ValidationGroup="form" OnClick="submitButton_Click" OnClientClick="ButtonLoading(this)" UseSubmitBehavior="false" />
                                    <asp:Button ID="cancelButton" runat="server" Text="CANCEL" Width="80px" OnClick="cancelButton_Click"
                                        CausesValidation="false" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="submitButton" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
