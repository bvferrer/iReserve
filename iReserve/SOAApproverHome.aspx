<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeFile="SOAApproverHome.aspx.cs" Inherits="SOAApproverHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager" EnablePageMethods="true" runat="server" />
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
                                <span>Search by Reference Number :</span>
                                <asp:TextBox ID="adminSearchTextBox" runat="server" MaxLength="16"></asp:TextBox>
                                <asp:Button ID="adminSearchButton" runat="server" CssClass="searchButton" Text="Go"
                                    OnClick="adminSearchButton_Click" /><br />
                                <asp:Label ID="searchValidationLabel" runat="server" Text="" ForeColor="Red" Font-Size="10px"></asp:Label>
                            </div>
                            <div class="gridview_wrapper">
                                <asp:GridView ID="adminSearchGridView" runat="server" DataKeyNames="CCRequestReferenceNo"
                                    EnableViewState="False" AllowSorting="True" AutoGenerateColumns="False" OnRowCreated="adminSearchGridView_RowCreated"
                                    OnSelectedIndexChanged="adminSearchGridView_SelectedIndexChanged">
                                    <RowStyle BackColor="#EFF3FB" Wrap="true"></RowStyle>
                                    <Columns>
                                        <asp:CommandField ShowSelectButton="True" ButtonType="button" SelectText="View">
                                            <ItemStyle CssClass="gridviewselect" />
                                        </asp:CommandField>
                                        <asp:BoundField DataField="CCRequestReferenceNo" HeaderText="Reference Number"></asp:BoundField>
                                        <asp:BoundField DataField="EventName" HeaderText="Event Name"></asp:BoundField>
                                        <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:MMMM d, yyyy}"></asp:BoundField>
                                        <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:MMMM d, yyyy}"></asp:BoundField>
                                        <asp:BoundField DataField="CreatedBy" HeaderText="Requestor"></asp:BoundField>
                                        <asp:BoundField DataField="CostCenterName" HeaderText="Cost Center"></asp:BoundField>
                                        <asp:BoundField DataField="DateCreated" HeaderText="Date Requested" DataFormatString="{0:MMMM d, yyyy hh:mm tt}"></asp:BoundField>
                                        <asp:BoundField DataField="SOAStatusName" HeaderText="Status"></asp:BoundField>
                                    </Columns>
                                    <AlternatingRowStyle BackColor="#FFFFFF"></AlternatingRowStyle>
                                </asp:GridView>
                            </div>
                            <span class="bold">SOA For Approval</span>
                            <div class="gridview_wrapper">
                                <asp:GridView ID="soaForApprovalGridView" runat="server" OnPageIndexChanging="soaForApprovalGridView_PageIndexChanging"
                                    PageSize="5" DataKeyNames="CCRequestReferenceNo" OnDataBound="soaForApprovalGridView_DataBound"
                                    AllowPaging="True" EnableViewState="False" AllowSorting="True" AutoGenerateColumns="False"
                                    OnRowCreated="soaForApprovalGridView_RowCreated" OnSelectedIndexChanged="soaForApprovalGridView_SelectedIndexChanged">
                                    <RowStyle BackColor="#EFF3FB" Wrap="true"></RowStyle>
                                    <Columns>
                                        <asp:CommandField ShowSelectButton="True" ButtonType="button" SelectText="View">
                                            <ItemStyle CssClass="gridviewselect" />
                                        </asp:CommandField>
                                        <asp:BoundField DataField="CCRequestReferenceNo" HeaderText="Reference Number"></asp:BoundField>
                                        <asp:BoundField DataField="EventName" HeaderText="Event Name"></asp:BoundField>
                                        <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:MMMM d, yyyy}"></asp:BoundField>
                                        <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:MMMM d, yyyy}"></asp:BoundField>
                                        <asp:BoundField DataField="CreatedBy" HeaderText="Requestor"></asp:BoundField>
                                        <asp:BoundField DataField="CostCenterName" HeaderText="Cost Center"></asp:BoundField>
                                        <asp:BoundField DataField="DateCreated" HeaderText="Date Requested" DataFormatString="{0:MMMM d, yyyy hh:mm tt}"></asp:BoundField>
                                    </Columns>
                                    <PagerTemplate>
                                        <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="~/img/first.gif" AlternateText="Go to first page"
                                            CommandArgument="First" CommandName="Page" Style="vertical-align: middle" />
                                        <asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="~/img/prev.gif" AlternateText="Go to previous page"
                                            CommandArgument="Prev" CommandName="Page" Style="vertical-align: middle" />
                                        Page &nbsp
                                        <asp:DropDownList ID="ddlPages5" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPages5_SelectedIndexChanged"
                                            Font-Names="Tahoma" Font-Size="11px">
                                        </asp:DropDownList>
                                        &nbsp of &nbsp
                                        <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                                        <asp:ImageButton ID="ImageButton9" runat="server" ImageUrl="~/img/next.gif" AlternateText="Go to next page"
                                            CommandArgument="Next" CommandName="Page" Style="vertical-align: middle" />
                                        <asp:ImageButton ID="ImageButton10" runat="server" ImageUrl="~/img/last.gif" AlternateText="Go to last page"
                                            CommandArgument="Last" CommandName="Page" Style="vertical-align: middle" />
                                    </PagerTemplate>
                                    <PagerStyle CssClass="tablefooter"></PagerStyle>
                                    <AlternatingRowStyle BackColor="#FFFFFF"></AlternatingRowStyle>
                                </asp:GridView>
                            </div>
                            <span class="bold">Recently Approved SOA</span>
                            <div class="gridview_wrapper">
                                <asp:GridView ID="approvedSOAGridView" runat="server" OnPageIndexChanging="approvedSOAGridView_PageIndexChanging"
                                    PageSize="5" DataKeyNames="CCRequestReferenceNo" OnDataBound="approvedSOAGridView_DataBound"
                                    AllowPaging="True" EnableViewState="False" AllowSorting="True" AutoGenerateColumns="False"
                                    OnRowCreated="approvedSOAGridView_RowCreated" OnSelectedIndexChanged="approvedSOAGridView_SelectedIndexChanged">
                                    <RowStyle BackColor="#EFF3FB" Wrap="true"></RowStyle>
                                    <Columns>
                                        <asp:CommandField ShowSelectButton="True" ButtonType="button" SelectText="View">
                                            <ItemStyle CssClass="gridviewselect" />
                                        </asp:CommandField>
                                        <asp:BoundField DataField="CCRequestReferenceNo" HeaderText="Reference Number"></asp:BoundField>
                                        <asp:BoundField DataField="EventName" HeaderText="Event Name"></asp:BoundField>
                                        <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:MMMM d, yyyy}"></asp:BoundField>
                                        <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:MMMM d, yyyy}"></asp:BoundField>
                                        <asp:BoundField DataField="CreatedBy" HeaderText="Requestor"></asp:BoundField>
                                        <asp:BoundField DataField="CostCenterName" HeaderText="Cost Center"></asp:BoundField>
                                        <asp:BoundField DataField="DateCreated" HeaderText="Date Requested" DataFormatString="{0:MMMM d, yyyy hh:mm tt}"></asp:BoundField>
                                    </Columns>
                                    <PagerTemplate>
                                        <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="~/img/first.gif" AlternateText="Go to first page"
                                            CommandArgument="First" CommandName="Page" Style="vertical-align: middle" />
                                        <asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="~/img/prev.gif" AlternateText="Go to previous page"
                                            CommandArgument="Prev" CommandName="Page" Style="vertical-align: middle" />
                                        Page &nbsp
                                        <asp:DropDownList ID="ddlPages6" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPages6_SelectedIndexChanged"
                                            Font-Names="Tahoma" Font-Size="11px">
                                        </asp:DropDownList>
                                        &nbsp of &nbsp
                                        <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                                        <asp:ImageButton ID="ImageButton9" runat="server" ImageUrl="~/img/next.gif" AlternateText="Go to next page"
                                            CommandArgument="Next" CommandName="Page" Style="vertical-align: middle" />
                                        <asp:ImageButton ID="ImageButton10" runat="server" ImageUrl="~/img/last.gif" AlternateText="Go to last page"
                                            CommandArgument="Last" CommandName="Page" Style="vertical-align: middle" />
                                    </PagerTemplate>
                                    <PagerStyle CssClass="tablefooter"></PagerStyle>
                                    <AlternatingRowStyle BackColor="#FFFFFF"></AlternatingRowStyle>
                                </asp:GridView>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
