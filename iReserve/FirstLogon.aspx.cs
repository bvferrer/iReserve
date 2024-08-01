using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using iReserveWS;
using wsAACF;
using AppCryptor;
using AESCryptor;
using System.Web.Services.Protocols;

public partial class FirstLogon : System.Web.UI.Page
{
    static wsAACF.AACFws serviceAACF = new wsAACF.AACFws();
    static Service svc = new Service();
    public string strPassword = "", decryptPassword = "", userHostAddress = "", appName = "", email = "";

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

        try
        {
            PasswordRecoveryInfo changePassword = svc.ChangePassword(Convert.ToString(Session["UserID"]));

            strPassword = changePassword.Password;
            decryptPassword = changePassword.Password;
            lblUserID.Text = Convert.ToString(Session["UserID"]);
            email = changePassword.Email;
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericWebServiceMessage);
        }

        Page.Form.DefaultFocus = txtOldPassword.ClientID;

        appName = "IRSV";
        userHostAddress = Request.UserHostAddress.ToString();
    }

    [WebMethod(EnableSession = true)]
    public static bool FirstLogonValidation(string strUserID, string strCPass, string strNPass,
                                                string strSQuestion1, string strSQuestion2, string strSQuestion3,
                                                string strSAnswer1, string strSAnswer2, string strSAnswer3,
                                                string strUserHostAddress, string strAppName, string strEmail)
    {
        string strFirstLogon = serviceAACF.FirstLogon(strUserID, strCPass, strNPass, strSQuestion1, strSQuestion2, strSQuestion3, strSAnswer1, strSAnswer2, strSAnswer3, strUserHostAddress, strAppName);

        bool blnFirstLogonValidation = false;

        if (strFirstLogon == "Successful")
        {
            blnFirstLogonValidation = true;

            #region Send Email Notification

            bool isSent = false;

            try
            {
                isSent = svc.SendChangePasswordNotification(strEmail);
            }

            catch (SoapException ex)
            {
                throw new Exception(Settings.GenericWebServiceMessage);
            }

            #endregion

            #region Audit Trail

            bool isSuccess = false;

            AuditTrail auditTrail = new AuditTrail();
            auditTrail.ActionDate = DateTime.Now;
            auditTrail.ActionTaken = "First Logon";
            auditTrail.ActionDetails = "Password changed";
            auditTrail.Browser = HttpContext.Current.Request.Browser.Browser;
            auditTrail.BrowserVersion = HttpContext.Current.Request.Browser.Version;
            auditTrail.IpAddress = HttpContext.Current.Request.ServerVariables[32];
            System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();

            auditTrail.MacAdress = HttpContext.Current.Session["MacAddress"].ToString();
            auditTrail.UserID = HttpContext.Current.Session["UserID"].ToString();

            try
            {
                isSuccess = svc.InsertAuditTrailEntry(auditTrail);
            }

            catch (SoapException ex)
            {
                throw new Exception(Settings.GenericAuditTrailMessage);
            }

            #endregion
        }
        else
        {
            blnFirstLogonValidation = false;
        }

        return blnFirstLogonValidation;
    }
}