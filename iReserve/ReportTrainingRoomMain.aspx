<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="ReportTrainingRoomMain.aspx.cs" Inherits="ReportTrainingRoomMain" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="eWorld.UI, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
    Namespace="eWorld.UI" TagPrefix="ew" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager" EnablePageMethods="true" runat="server" />
    <script language="javascript" type="text/javascript">
        function GetStringDate(dateid) {
            var date = CalendarPopup_FindCalendar(dateid).GetDate();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            return month + "/" + day + "/" + year;
        }

        function GenerateReport() {
            var selectedStatus = document.getElementById("<%=statusDropDownList.ClientID%>");

            //            if (selectedStatus.value == "-") {
            //                alert('Select Status.');
            //                return false;
            //            }
            //            else {
            var selectedDateFrom = CalendarPopup_FindCalendar("<%=calendarFrom.ClientID%>").GetDate();
            var selectedDateTo = CalendarPopup_FindCalendar("<%=calendarTo.ClientID%>").GetDate();

            selectedDateFrom.setHours(0, 0, 0, 0)
            selectedDateTo.setHours(0, 0, 0, 0)
            var currentDate = new Date();
            currentDate.setHours(0, 0, 0, 0)

            if (selectedDateFrom > selectedDateTo) {
                alert('Date From should not be later than Date To.');
                return false;
            }
            else {
                selectedDateFrom = GetStringDate('<%=calendarFrom.ClientID%>');
                selectedDateTo = GetStringDate('<%=calendarTo.ClientID%>');
                window.open('ReportTrainingRoom.aspx?param1=' + selectedDateFrom + '&param2=' + selectedDateTo + '&param3=' + selectedStatus.value, '', 'top=' + ((screen.height) * 0.05) + ',left=' + ((screen.width) * 0.1) + ',resizable=1,scrollbars=yes,width=' + ((screen.width) * 0.8) + ',height=' + ((screen.height) * 0.8) + '');
                location.reload();
            }
            //            }
        }    
    </script>
    <div class="mainDiv">
        <h2 class="header">
            <span>Training Room Reservation Requests Report </span>
        </h2>
        <asp:UpdatePanel ID="UpdatePanel" runat="server">
            <ContentTemplate>
                <div class="contentDiv">
                    <div class="reportmain_wrapper">
                        <div>
                            <span class="bold">Status :</span>
                            <asp:DropDownList ID="statusDropDownList" runat="server" CssClass="reportdropdown">
                                <asp:ListItem Value="A" Selected="True">All</asp:ListItem>
                                <asp:ListItem Value="0">Confirmed</asp:ListItem>
                                <asp:ListItem Value="1">Cancelled</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div>
                            <span class="bold">Date From :</span>
                            <ew:CalendarPopup ID="calendarFrom" runat="server" DisableTextBoxEntry="true">
                            </ew:CalendarPopup>
                        </div>
                        <div>
                            <span class="bold">Date To :</span>
                            <ew:CalendarPopup ID="calendarTo" runat="server" DisableTextBoxEntry="true">
                            </ew:CalendarPopup>
                        </div>
                        <div class="reportbutton_wrapper">
                            <input id="generateReportButton" type="button" value="Generate Report" class="reportbutton"
                                runat="server" onclick="GenerateReport()" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
