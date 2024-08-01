<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Error.aspx.cs" Inherits="ErrorPages_Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentBody" runat="Server">
    <div class="errorwrapper">
        <div class="errorimage">
            <asp:Image ID="Sad" runat="server" ImageUrl="sad.png" Height="150" Width="150" />
        </div>
        <div class="errorcontent">
            <h2>
                Sorry, something went wrong.</h2>
            <br />
            Please contact P & EL at <a href="mailto:reserve@pjlhuillier.com">
                reserve@pjlhuillier.com</a>.
        </div>
    </div>
</asp:Content>
