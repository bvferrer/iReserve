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
using System.Web.Configuration;
using System.IO;
using iReserveWS;
using System.Data.SqlClient;
using wsAACF;
using System.Web.Services.Protocols;

public partial class ChangePassword : System.Web.UI.Page
{
    public static Service svc = new Service();
    static wsAACF.AACFws serviceAACF = new wsAACF.AACFws();
    public string strPassword = "", decryptPassword = "", userHostAddress = "", appName = "", email = "";
    public string userID, macAddress, browser, browserVersion;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1d);
        Response.Expires = -1500;
        Response.CacheControl = "no-cache";
        macAddress = macAddressHiddenField.Value.ToString();

        if (Convert.ToString(Session["UserID"]) == "")
        {
            Response.BufferOutput = true;
            Response.Redirect("Login.aspx");
        }

        if (Convert.ToInt32(Session["AccountStatus"]) == 7)
        {
            lblmessage.Text = "Sorry, your password has already expired.<br>Please change your password or contact Information Security Department (infosec@pjlhuillier.com) for assistance.";
        }

        try
        {
            PasswordRecoveryInfo changePassword = new PasswordRecoveryInfo();

            try
            {
                changePassword = svc.ChangePassword(Convert.ToString(Session["UserID"]));
            }

            catch (SoapException ex)
            {
                throw new Exception(Settings.GenericWebServiceMessage);
            }

            strPassword = changePassword.Password;
            decryptPassword = changePassword.Password;
            lblEmail.Text = changePassword.Email;
            email = changePassword.Email;


        }
        catch (Exception error)
        {
            throw error;
        }


        Page.Form.DefaultFocus = txtOldPassword.ClientID;

        appName = "IRSV";
        userHostAddress = Request.UserHostAddress.ToString();
    }

    [WebMethod(EnableSession = true)]
    public static bool PasswordChanged(string strUserID, string strCPass, string strNPass,
                                        string strUserHostAddress, string strAppName, string strEmail, string strMacAddress)
    {
        string strChangePassword = serviceAACF.ChangePassword(strUserID, strCPass, strNPass, strUserHostAddress, strAppName);

        bool blnChangePasswordValidation = false;

        if (strChangePassword == "Successful")
        {
            blnChangePasswordValidation = true;

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
            auditTrail.ActionTaken = "Change password";
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
            blnChangePasswordValidation = false;
        }

        return blnChangePasswordValidation;
    }

    //public void Page_Error(object sender, EventArgs e)
    //{
    //    Exception objErr = Server.GetLastError().GetBaseException();

    //    string err = "<h2 style='color:red'>This page has encountered an unexpected problem.<hr></h2><br>" +
    //                 "<b style='color:blue'>What Happened:</b><br><br>" +
    //                 "There was an unexpected error on this page. This may be due to a programming bug.<br><br>" +
    //                 "<b style='color:blue'>How this will affect you:</b><br><br>" +
    //                 "The current page will not load.<br><br>" +
    //                 "<b style='color:blue'>What you can do about it:</b><br><br>" +
    //                 "Navigate back to this page, and try repeating your last action." +
    //                 "Try alternative methods of performing the action. If problem persist, contact Networld " +
    //                 "Capital Ventures Incorporated for assistance.<br><br>" +
    //                 "<b style='color:blue'>More Information:</b><br><br>" +
    //                 "<b style='color:blue'>Error in: </b>" + Request.Url.ToString() +
    //                 "<br><b style='color:blue'>Error Message: </b>" + objErr.Message.ToString() +
    //                 "<br><b style='color:blue'>Stack Trace:</b><br>" + objErr.StackTrace.ToString();
    //    Response.Write(err.ToString());

    //    // This prevents the error from continuing to the Application_Error event handler.
    //    Server.ClearError();
    //}
}