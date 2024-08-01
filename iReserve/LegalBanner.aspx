<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LegalBanner.aspx.cs" Inherits="LegalBanner" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Legal Banner | iReserve</title>
    <link rel="SHORTCUT ICON" href="img/PEL.ico"/>
    <link type="text/css" href="css/Style.css" media="screen" rel="Stylesheet" />
</head>
<body>
    <form id="legalBannerForm" runat="server">
        <div title="Legal Banner | iReserve">
            <asp:Panel ID="LegalBannerPanel" runat="server">
                <p>For Authorized Users Only</p>
                <p>This computer system is for authorized users only. Activities shall be logged and
                    reviewed on a regular basis. Individuals using this system without authority or
                    in excess of their authority shall be dealt with as per applicable provisions of
                    the Company Code of Conduct and/or Laws of the Republic of the Philippines</p>
                <p class="highlight">Log off now if you have not been expressly authorized to use this system.</p>
                <asp:Button ID="btnProceed" runat="server" Font-Names="Verdana" Text="Proceed" OnClick="btnProceed_Click" />
            </asp:Panel>
        </div>
    </form>
</body>
</html>
