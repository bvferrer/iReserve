<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="AdminHome.aspx.cs" Inherits="AdminHome" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager" EnablePageMethods="true" runat="server" />
    <script type="text/javascript">
        function ViewRedirect() {
            window.open('CalendarConferenceRoomView.aspx', '', 'top=' + ((screen.height) * 0.05) + ',left=' + ((screen.width) * 0.1) + ',resizable=1,scrollbars=yes,width=' + ((screen.width) * 0.8) + ',height=' + ((screen.height) * 0.8) + '');
            window.location.href = window.location;
        }
    </script>
    <div class="mainDiv">
        <h2 class="header">
            <span>Home </span>
        </h2>
        <asp:UpdatePanel ID="UpdatePanel" runat="server">
            <ContentTemplate>
                <div class="contentDiv">
                    <asp:Panel ID="adminPanel" runat="server" DefaultButton="adminSearchButton">
                        <div class="section_wrapper">
                            <div class="default_search_wrapper">
                                <span>Search Request by Reference Number :</span>
                                <asp:TextBox ID="adminSearchTextBox" runat="server" MaxLength="16"></asp:TextBox>
                                <asp:Button ID="adminSearchButton" runat="server" CssClass="searchButton" Text="Go"
                                    OnClick="adminSearchButton_Click" /><br />
                                <asp:Label ID="searchValidationLabel" runat="server" Text="" ForeColor="Red" Font-Size="10px"></asp:Label>
                            </div>
                            <div class="gridview_wrapper">
                                <asp:GridView ID="adminSearchGridView" runat="server" DataKeyNames="RequestReferenceNo"
                                    EnableViewState="False" AllowSorting="True" AutoGenerateColumns="False" OnRowCreated="adminSearchGridView_RowCreated"
                                    OnSelectedIndexChanged="adminSearchGridView_SelectedIndexChanged">
                                    <RowStyle BackColor="#EFF3FB" Wrap="true"></RowStyle>
                                    <Columns>
                                        <asp:CommandField ShowSelectButton="True" ButtonType="button" SelectText="View">
                                            <ItemStyle CssClass="gridviewselect" />
                                        </asp:CommandField>
                                        <asp:BoundField DataField="RequestReferenceNo" HeaderText="Reference Number"></asp:BoundField>
                                        <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:MMMM d, yyyy}">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="StartTime12" HeaderText="Start Time"></asp:BoundField>
                                        <asp:BoundField DataField="EndTime12" HeaderText="End Time"></asp:BoundField>
                                        <asp:BoundField DataField="RoomName" HeaderText="Room"></asp:BoundField>
                                        <asp:BoundField DataField="DateRequested" HeaderText="Date Requested"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>
                                    </Columns>
                                    <AlternatingRowStyle BackColor="#FFFFFF"></AlternatingRowStyle>
                                </asp:GridView>
                            </div>
                            <span class="bold">Requests For Confirmation</span>
                            <div class="adminstats">
                                <asp:DataList ID="reserveCountDataList" runat="server" RepeatLayout="Table" RepeatColumns="3">
                                    <ItemTemplate>
                                        <div class="admindatalist">
                                            <asp:Label ID="roomLabel" runat="server" Text='<%# Eval("fld_RoomName") %>' CssClass="roomname"></asp:Label>
                                            <span> : </span>
                                            <asp:Label ID="countLabel" runat="server" Text='<%# Eval("fld_Count") %>' CssClass="count"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:DataList>
                            </div>
                            <div class="roomdropdownwrapper">
                                <span>Filter By Room:</span>
                                <asp:DropDownList ID="reserveRoomDropDownList" runat="server" CssClass="default_dropdown"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                            <div class="gridview_wrapper">
                                <asp:GridView ID="reserveGridView" runat="server" OnPageIndexChanging="reserveGridView_PageIndexChanging"
                                    PageSize="5" DataKeyNames="RequestReferenceNo" OnDataBound="reserveGridView_DataBound"
                                    AllowPaging="True" EnableViewState="False" AllowSorting="True" AutoGenerateColumns="False"
                                    OnRowCreated="reserveGridView_RowCreated" OnSelectedIndexChanged="reserveGridView_SelectedIndexChanged">
                                    <RowStyle BackColor="#EFF3FB" Wrap="true"></RowStyle>
                                    <Columns>
                                        <asp:CommandField ShowSelectButton="True" ButtonType="button" SelectText="View">
                                            <ItemStyle CssClass="gridviewselect" />
                                        </asp:CommandField>
                                        <asp:BoundField DataField="RequestReferenceNo" HeaderText="Reference Number"></asp:BoundField>
                                        <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:MMMM d, yyyy}">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="StartTime12" HeaderText="Start Time"></asp:BoundField>
                                        <asp:BoundField DataField="EndTime12" HeaderText="End Time"></asp:BoundField>
                                        <asp:BoundField DataField="RoomName" HeaderText="Room"></asp:BoundField>
                                        <asp:BoundField DataField="RequestedBy" HeaderText="Requestor"></asp:BoundField>
                                    </Columns>
                                    <PagerTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/img/first.gif" AlternateText="Go to first page"
                                            CommandArgument="First" CommandName="Page" Style="vertical-align: middle" />
                                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/img/prev.gif" AlternateText="Go to previous page"
                                            CommandArgument="Prev" CommandName="Page" Style="vertical-align: middle" />
                                        Page &nbsp
                                        <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged"
                                            Font-Names="Tahoma" Font-Size="11px">
                                        </asp:DropDownList>
                                        &nbsp of &nbsp
                                        <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                                        <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/img/next.gif" AlternateText="Go to next page"
                                            CommandArgument="Next" CommandName="Page" Style="vertical-align: middle" />
                                        <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/img/last.gif" AlternateText="Go to last page"
                                            CommandArgument="Last" CommandName="Page" Style="vertical-align: middle" />
                                    </PagerTemplate>
                                    <PagerStyle CssClass="tablefooter"></PagerStyle>
                                    <AlternatingRowStyle BackColor="#FFFFFF"></AlternatingRowStyle>
                                </asp:GridView>
                            </div>
                            <%--<span class="bold">Requests For Cancellation</span>
                            <div class="gridview_wrapper">
                                <asp:GridView ID="cancelGridView" runat="server" OnPageIndexChanging="cancelGridView_PageIndexChanging"
                                    PageSize="5" DataKeyNames="RequestReferenceNo" OnDataBound="cancelGridView_DataBound"
                                    AllowPaging="True" EnableViewState="False" AllowSorting="True" AutoGenerateColumns="False"
                                    OnRowCreated="cancelGridView_RowCreated" OnSelectedIndexChanged="cancelGridView_SelectedIndexChanged">
                                    <RowStyle BackColor="#EFF3FB" Wrap="true"></RowStyle>
                                    <Columns>
                                        <asp:CommandField ShowSelectButton="True" ButtonType="button" SelectText="View">
                                            <ItemStyle CssClass="gridviewselect" />
                                        </asp:CommandField>
                                        <asp:BoundField DataField="RequestReferenceNo" HeaderText="Reference Number"></asp:BoundField>
                                        <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:MMMM d, yyyy}">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="StartTime12" HeaderText="Start Time"></asp:BoundField>
                                        <asp:BoundField DataField="EndTime12" HeaderText="End Time"></asp:BoundField>
                                        <asp:BoundField DataField="RoomName" HeaderText="Room"></asp:BoundField>
                                        <asp:BoundField DataField="RequestedBy" HeaderText="Requestor"></asp:BoundField>
                                    </Columns>
                                    <PagerTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/img/first.gif" AlternateText="Go to first page"
                                            CommandArgument="First" CommandName="Page" Style="vertical-align: middle" />
                                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/img/prev.gif" AlternateText="Go to previous page"
                                            CommandArgument="Prev" CommandName="Page" Style="vertical-align: middle" />
                                        Page &nbsp
                                        <asp:DropDownList ID="ddlPages3" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPages3_SelectedIndexChanged"
                                            Font-Names="Tahoma" Font-Size="11px">
                                        </asp:DropDownList>
                                        &nbsp of &nbsp
                                        <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                                        <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/img/next.gif" AlternateText="Go to next page"
                                            CommandArgument="Next" CommandName="Page" Style="vertical-align: middle" />
                                        <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/img/last.gif" AlternateText="Go to last page"
                                            CommandArgument="Last" CommandName="Page" Style="vertical-align: middle" />
                                    </PagerTemplate>
                                    <PagerStyle CssClass="tablefooter"></PagerStyle>
                                    <AlternatingRowStyle BackColor="#FFFFFF"></AlternatingRowStyle>
                                </asp:GridView>
                            </div>--%>
                            <div class="default_button_wrapper">
                                <input id="viewCalendarButton" type="button" value="View Reservation Calendar" class="newRequestButton"
                                    runat="server" onclick="ViewRedirect()" />
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
