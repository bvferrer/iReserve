<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PasswordRules.aspx.cs" Inherits="PasswordRules" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self" />
    <title>Password Complexity</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="passrules" style="width: 393px; border: #aaaaaa solid 1px; color: #101010;
        padding: 5px 5px 30px 5px; font-family: Verdana; font-size: 11px; margin-left: 10px;
        margin-top: 10px; height: 207px;">
        <h2 style="display: block; height: 22px; background: url(img/Site_lefthead.gif) 0 0 repeat-x;
            padding: 4px 0 0 14px; margin: 0 0 1px 0;">
            <span style="padding: 0 0 0 16px; font-size: 12px; font-weight: bold; color: #101010;
                background-position: 0px 3px; background-attachment: scroll; background-image: url(img/arrow.gif);
                background-repeat: no-repeat;">
                <asp:Label ID="lblStatus" runat="server" Text="Password Complexity Requirements"
                    Font-Names="Arial" Font-Size="12px"></asp:Label>
            </span>
        </h2>
        <div style="padding: 5px 5px 10px 6px;">
            <span style="font-family: Tahoma; font-size: 12px; font-weight: bold; padding-left: 10px;
                padding-bottom: 0px; text-align: center">Here are the following rules in creating
                a strong password. </span>
            <hr />
            <ul>
                <li>Password must be at least eight (8) characters in length.</li>
                <li>You may use a combination of alphabet, number and at least one special character.</li>
                <li>Password must not be the same as the user name.</li>
                <li>User name must not be a subset within the password.</li>
                <li>3 Repeating characters in consecutive order is not allowed (e.g. aaa123, bbb@3wews).</li>
                <li>Password has not been used in the previous 4 passwords.</li>
                <li>Password must not be equal to dictionary list which will contain common password
                    values used in the company.</li>
            </ul>
            <br />
        </div>
    </div>
    <div style="padding: 13px 0px 0px 165px;">
        <input type="submit" id="btnClose" value="Close" style="font-family: Tahoma; font-size: 11px;
            height: 26px; width: 82px;" onclick="window.close()" />
    </div>
    </form>
</body>
</html>
