<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="MonitorDisplayConferenceRoomMain.aspx.cs" Inherits="MonitorDisplayConferenceRoomMain" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager" EnablePageMethods="true" runat="server" />
    <script language="javascript" type="text/javascript">
        function ViewMonitorDisplay() {
            var selectedDisplay = document.getElementById("<%=displayDropDownList.ClientID%>");

            if (selectedDisplay.value == "0") {
                alert('Select Monitor Display.');
                return false;
            }
            else {
                window.open('MonitorDisplayConferenceRoom.aspx?disp=' + selectedDisplay.value, '', 'top=' + ((screen.height) * 0.05) + ',left=' + ((screen.width) * 0.1) + ',resizable=1,scrollbars=auto,width=' + ((screen.width) * 0.8) + ',height=' + ((screen.height) * 0.8) + '');
                location.reload();
            }
        }    
    </script>
    <div class="mainDiv">
        <h2 class="header">
            <span>View Conference Room Monitor Display </span>
        </h2>
        <asp:UpdatePanel ID="UpdatePanel" runat="server">
            <ContentTemplate>
                <div class="contentDiv">
                    <div class="reportmain_wrapper">
                        <div>
                            <span class="bold">Monitor Display :</span>
                            <asp:DropDownList ID="displayDropDownList" runat="server" CssClass="displaydropdown">
                            </asp:DropDownList>
                            <input id="viewDisplayButton" type="button" value="View" class="reportbutton"
                                runat="server" onclick="ViewMonitorDisplay()" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
