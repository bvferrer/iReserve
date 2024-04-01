<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="RequestorHome.aspx.cs" Inherits="RequestorHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager" EnablePageMethods="true" runat="server" />
    <script type="text/javascript">
        function RequestRedirect() {
            window.open('CalendarConferenceRoom.aspx', '', 'top=' + ((screen.height) * 0.05) + ',left=' + ((screen.width) * 0.1) + ',resizable=1,scrollbars=yes,width=' + ((screen.width) * 0.8) + ',height=' + ((screen.height) * 0.8) + '');
            window.location.href = window.location;
        }
        function CCRequestRedirect() {
            window.open('CalendarConventionCenter.aspx', '', 'top=' + ((screen.height) * 0.05) + ',left=' + ((screen.width) * 0.1) + ',resizable=1,scrollbars=yes,width=' + ((screen.width) * 0.8) + ',height=' + ((screen.height) * 0.8) + '');
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
                    <asp:Panel ID="requestorPanel" runat="server" DefaultButton="requestorSearchButton">
                        <div class="section_wrapper">
                            <div class="default_search_wrapper">
                                <span>Search Request by Reference Number :</span>
                                <asp:TextBox ID="requestorSearchTextBox" runat="server" MaxLength="16"></asp:TextBox>
                                <asp:Button ID="requestorSearchButton" runat="server" CssClass="searchButton" Text="Go"
                                    OnClick="requestorSearchButton_Click" /><br />
                                <asp:Label ID="searchValidationLabel" runat="server" Text="" ForeColor="Red" Font-Size="10px"></asp:Label>
                            </div>
                            <div class="gridview_wrapper">
                                <asp:GridView ID="requestorSearchGridView" runat="server" DataKeyNames="RequestReferenceNo"
                                    EnableViewState="False" AllowSorting="True" AutoGenerateColumns="False" OnRowCreated="requestorSearchGridView_RowCreated"
                                    OnSelectedIndexChanged="requestorSearchGridView_SelectedIndexChanged">
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
                                <asp:GridView ID="ccRequestSearchGridView" runat="server" DataKeyNames="CCRequestReferenceNo"
                                    EnableViewState="False" AllowSorting="True" AutoGenerateColumns="False" OnRowCreated="ccRequestSearchGridView_RowCreated"
                                    OnSelectedIndexChanged="ccRequestSearchGridView_SelectedIndexChanged">
                                    <RowStyle BackColor="#EFF3FB" Wrap="true"></RowStyle>
                                    <Columns>
                                        <asp:CommandField ShowSelectButton="True" ButtonType="button" SelectText="View">
                                            <ItemStyle CssClass="gridviewselect" />
                                        </asp:CommandField>
                                        <asp:BoundField DataField="CCRequestReferenceNo" HeaderText="Reference Number"></asp:BoundField>
                                        <asp:BoundField DataField="EventName" HeaderText="Event Name"></asp:BoundField>
                                        <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:MMMM d, yyyy}"></asp:BoundField>
                                        <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:MMMM d, yyyy}"></asp:BoundField>
                                        <asp:BoundField DataField="DateCreated" HeaderText="Date Requested" DataFormatString="{0:MMMM d, yyyy hh:mm tt}">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="StatusName" HeaderText="Status"></asp:BoundField>
                                    </Columns>
                                    <AlternatingRowStyle BackColor="#FFFFFF"></AlternatingRowStyle>
                                </asp:GridView>
                            </div>
                            <div class="default_title_wrapper">
                                <span class="bold">Conference Room</span>
                            </div>
                            <span class="bold">Confirmed Requests</span>
                            <div class="gridview_wrapper">
                                <asp:GridView ID="requestorGridView" runat="server" OnPageIndexChanging="requestorGridView_PageIndexChanging"
                                    PageSize="5" DataKeyNames="RequestReferenceNo" OnDataBound="requestorGridView_DataBound"
                                    AllowPaging="True" EnableViewState="False" AllowSorting="True" AutoGenerateColumns="False"
                                    OnRowCreated="requestorGridView_RowCreated" OnSelectedIndexChanged="requestorGridView_SelectedIndexChanged">
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
                                        <asp:BoundField DataField="DateRequested" HeaderText="Date Requested" DataFormatString="{0:MMMM d, yyyy hh:mm tt}">
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/img/first.gif" AlternateText="Go to first page"
                                            CommandArgument="First" CommandName="Page" Style="vertical-align: middle" />
                                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/img/prev.gif" AlternateText="Go to previous page"
                                            CommandArgument="Prev" CommandName="Page" Style="vertical-align: middle" />
                                        Page &nbsp
                                        <asp:DropDownList ID="ddlPages2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPages2_SelectedIndexChanged"
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
                            <span class="bold">For Confirmation Requests</span>
                            <div class="gridview_wrapper">
                                <asp:GridView ID="pendingGridView" runat="server" OnPageIndexChanging="pendingGridView_PageIndexChanging"
                                    PageSize="5" DataKeyNames="RequestReferenceNo" OnDataBound="pendingGridView_DataBound"
                                    AllowPaging="True" EnableViewState="False" AllowSorting="True" AutoGenerateColumns="False"
                                    OnRowCreated="pendingGridView_RowCreated" OnSelectedIndexChanged="pendingGridView_SelectedIndexChanged">
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
                                        <asp:BoundField DataField="DateRequested" HeaderText="Date Requested" DataFormatString="{0:MMMM d, yyyy hh:mm tt}">
                                        </asp:BoundField>
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
                            </div>
                            <div class="default_button_wrapper">
                                <input id="newRequestButton" type="button" value="New Reservation Request" class="newRequestButton"
                                    runat="server" onclick="RequestRedirect()" />
                            </div>
                            <div class="default_title_wrapper">
                                <span class="bold">Convention Center</span>
                            </div>
                            <span class="bold">Confirmed Requests</span>
                            <div class="gridview_wrapper">
                                <asp:GridView ID="ccRequestGridView" runat="server" OnPageIndexChanging="ccRequestGridView_PageIndexChanging"
                                    PageSize="5" DataKeyNames="CCRequestReferenceNo" OnDataBound="ccRequestGridView_DataBound"
                                    AllowPaging="True" EnableViewState="False" AllowSorting="True" AutoGenerateColumns="False"
                                    OnRowCreated="ccRequestGridView_RowCreated" OnSelectedIndexChanged="ccRequestGridView_SelectedIndexChanged">
                                    <RowStyle BackColor="#EFF3FB" Wrap="true"></RowStyle>
                                    <Columns>
                                        <asp:CommandField ShowSelectButton="True" ButtonType="button" SelectText="View">
                                            <ItemStyle CssClass="gridviewselect" />
                                        </asp:CommandField>
                                        <asp:BoundField DataField="CCRequestReferenceNo" HeaderText="Reference Number"></asp:BoundField>
                                        <asp:BoundField DataField="EventName" HeaderText="Event Name"></asp:BoundField>
                                        <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:MMMM d, yyyy}"></asp:BoundField>
                                        <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:MMMM d, yyyy}"></asp:BoundField>
                                        <asp:BoundField DataField="DateCreated" HeaderText="Date Requested" DataFormatString="{0:MMMM d, yyyy hh:mm tt}">
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/img/first.gif" AlternateText="Go to first page"
                                            CommandArgument="First" CommandName="Page" Style="vertical-align: middle" />
                                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/img/prev.gif" AlternateText="Go to previous page"
                                            CommandArgument="Prev" CommandName="Page" Style="vertical-align: middle" />
                                        Page &nbsp
                                        <asp:DropDownList ID="ddlPages4" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPages4_SelectedIndexChanged"
                                            Font-Names="Tahoma" Font-Size="11px">
                                        </asp:DropDownList>
                                        &nbsp of &nbsp
                                        <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                                        <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/img/next.gif" AlternateText="Go to next page"
                                            CommandArgument="Next" CommandName="Page" Style="vertical-align: middle" />
                                        <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="~/img/last.gif" AlternateText="Go to last page"
                                            CommandArgument="Last" CommandName="Page" Style="vertical-align: middle" />
                                    </PagerTemplate>
                                    <PagerStyle CssClass="tablefooter"></PagerStyle>
                                    <AlternatingRowStyle BackColor="#FFFFFF"></AlternatingRowStyle>
                                </asp:GridView>
                            </div>
                            <div class="default_button_wrapper">
                                <input id="newCCRequestButton" type="button" value="New Reservation Request" class="newRequestButton"
                                    runat="server" onclick="CCRequestRedirect()" />
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
