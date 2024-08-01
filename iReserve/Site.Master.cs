using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using iReserveWS;
using System.Web.Services.Protocols;
using AppCryptor;
using AESCryptor;

public partial class Site : System.Web.UI.MasterPage
{
    static Service svc = new Service();

    string userID, macAddress2, browser, browserVersion;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1d);
        Response.Expires = -1500;
        Response.CacheControl = "no-cache";

        if (Convert.ToString(Session["UserID"]) == "")
        {
            Response.BufferOutput = true;
            Response.Redirect("Login.aspx");
        }
        else
        {
            userID = HttpContext.Current.Session["UserID"].ToString();
            browser = Request.Browser.Browser;
            browserVersion = Request.Browser.Type;

            fullNameLabel.Text = Convert.ToString(Session["FullName"]);
            fullNameLabel.ToolTip = "Profile: " + Convert.ToString(Session["ProfileName"]);

            if (Convert.ToInt32(Session["AccountStatus"]) == 7)
            {
                mnuMain.Enabled = false;
                lbHome.Enabled = false;
            }
            else if (Convert.ToString(Session["FirstLogOnChecker"]) == "False")
            {
                mnuMain.Enabled = false;
                lbHome.Enabled = false;
            }
            else if (Convert.ToString(Session["FirstLogOnChecker"]) == "True")
            {
                mnuMain.Enabled = true;
                lbHome.Enabled = true;
            }
            else
            {
                mnuMain.Enabled = true;
                lbHome.Enabled = true;
            }
        }
    }
    protected void lbLogout_Click(object sender, EventArgs e)
    {
        #region Audit Trail

        bool isSuccess = false;

        AuditTrail auditTrail = new AuditTrail();
        auditTrail.ActionDate = DateTime.Now;
        auditTrail.ActionTaken = "Logout";
        auditTrail.ActionDetails = "Logged out";
        auditTrail.Browser = HttpContext.Current.Request.Browser.Browser;
        auditTrail.BrowserVersion = HttpContext.Current.Request.Browser.Version;
        auditTrail.IpAddress = HttpContext.Current.Request.ServerVariables[32];
        System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();

        auditTrail.MacAdress = Session["MacAddress"].ToString();
        auditTrail.UserID = userID;

        try
        {
            isSuccess = svc.InsertAuditTrailEntry(auditTrail);
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericAuditTrailMessage);
        }

        #endregion

        Session.Clear();
        Session["ReadLegalBanner"] = "Read";
        Response.BufferOutput = true;
        Response.Redirect("Login.aspx");
    }

    protected void mnuMain_MenuItemDataBound(object sender, MenuEventArgs e)
    {
        string profile = Session["ProfileName"].ToString();

        Menu menu = (Menu)sender;
        SiteMapNode mapNode = (SiteMapNode)e.Item.DataItem;

        if (mapNode.ResourceKey != null)
        {
            if (!mapNode.ResourceKey.Contains(profile))
            {
                menu.Items.Remove(e.Item);
            }
        }
    }
}
