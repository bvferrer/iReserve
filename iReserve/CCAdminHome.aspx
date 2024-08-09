<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
  CodeFile="CCAdminHome.aspx.cs" Inherits="CCAdminHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="Server">
  <asp:ScriptManager ID="ScriptManager" EnablePageMethods="true" runat="server" />
  <script type="text/javascript">
    function ViewRedirect() {
      window.open('CalendarConventionCenterAdmin.aspx', '', 'top=' + ((screen.height) * 0.05) + ',left=' + ((screen.width) * 0.1) + ',resizable=1,scrollbars=yes,width=' + ((screen.width) * 0.8) + ',height=' + ((screen.height) * 0.8) + '');
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
                    <asp:BoundField DataField="ChargedCompany" HeaderText="Company"></asp:BoundField>
                    <asp:BoundField DataField="CostCenterName" HeaderText="Cost Center"></asp:BoundField>
                    <asp:BoundField DataField="DateCreated" HeaderText="Date Requested" DataFormatString="{0:MMMM d, yyyy hh:mm tt}"></asp:BoundField>
                    <asp:BoundField DataField="StatusName" HeaderText="Status"></asp:BoundField>
                  </Columns>
                  <AlternatingRowStyle BackColor="#FFFFFF"></AlternatingRowStyle>
                </asp:GridView>
              </div>
              <span class="bold">Confirmed Requests</span>
              <div class="gridview_wrapper">
                <asp:GridView ID="ccAdminConfirmedGridView" runat="server" OnPageIndexChanging="ccAdminConfirmedGridView_PageIndexChanging"
                  PageSize="5" DataKeyNames="CCRequestReferenceNo" OnDataBound="ccAdminConfirmedGridView_DataBound"
                  AllowPaging="True" EnableViewState="False" AllowSorting="True" AutoGenerateColumns="False"
                  OnRowCreated="ccAdminConfirmedGridView_RowCreated" OnSelectedIndexChanged="ccAdminConfirmedGridView_SelectedIndexChanged">
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
                    <asp:BoundField DataField="ChargedCompany" HeaderText="Company"></asp:BoundField>
                    <asp:BoundField DataField="CostCenterName" HeaderText="Cost Center"></asp:BoundField>
                    <asp:BoundField DataField="DateCreated" HeaderText="Date Requested" DataFormatString="{0:MMMM d, yyyy hh:mm tt}"></asp:BoundField>
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
              <br />
              <span class="bold">SOA Processing</span>
              <div class="roomdropdownwrapper">
                <span>Status:</span>
                <asp:DropDownList ID="soaStatusDropDownList" runat="server" CssClass="reportdropdown"
                  AutoPostBack="True">
                  <asp:ListItem Value="0" Selected="True">For Processing</asp:ListItem>
                  <asp:ListItem Value="1">For Approval</asp:ListItem>
                  <asp:ListItem Value="2">Approved</asp:ListItem>
                </asp:DropDownList>
              </div>
              <div class="gridview_wrapper">
                <asp:GridView ID="forSOAProcessingGridView" runat="server" OnPageIndexChanging="forSOAProcessingGridView_PageIndexChanging"
                  PageSize="5" DataKeyNames="CCRequestReferenceNo" OnDataBound="forSOAProcessingGridView_DataBound"
                  AllowPaging="True" EnableViewState="False" AllowSorting="True" AutoGenerateColumns="False"
                  OnRowCreated="forSOAProcessingGridView_RowCreated" OnSelectedIndexChanged="forSOAProcessingGridView_SelectedIndexChanged">
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
                    <asp:BoundField DataField="ChargedCompany" HeaderText="Company"></asp:BoundField>
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
