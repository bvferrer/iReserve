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
using System.Web.Configuration;
using wsAACF;
using AppCryptor;
using AESCryptor;
using System.Text;
using System.Data.SqlClient;
using iReserveWS;
using System.Web.Services.Protocols;

public partial class Login : System.Web.UI.Page
{
    static AACFws serviceAACF = new AACFws();
    static Service svc = new Service();

    string param, userID, macAddress, browser, browserVersion;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1d);
        Response.Expires = -1500;
        Response.CacheControl = "no-cache";

        if (Session.Count == 0)
        {
            Response.BufferOutput = true;
            Response.Redirect("LegalBanner.aspx");
        }

        Page.ClientScript.RegisterStartupScript(this.GetType(), "invoke", "<script language='javascript'>GetMacAddress();</script>");

        userID = userIDTextBox.Text;
        macAddress = macAddressHiddenField.Value.ToString();
        browser = Request.Browser.Browser;
        browserVersion = Request.Browser.Type;

        Session["MacAddress"] = macAddress;
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string msg = "";

        if (userIDTextBox.Text == "")
        {
            msg = "User name is required.";
            userIDTextBox.Focus();
            ClientScript.RegisterStartupScript(Type.GetType("System.String"), "MessageBox", "<script language=javascript>alert('" + msg + "');</script>");
        }
        else if (passwordTextBox.Text == "")
        {
            msg = "Password is required.";
            passwordTextBox.Focus();
            ClientScript.RegisterStartupScript(Type.GetType("System.String"), "MessageBox", "<script language=javascript>alert('" + msg + "');</script>");
        }
        else
        {
            AACFUserProfile aacfUserProfile = new AACFUserProfile();

            object[] aacfUserProfileInfo = serviceAACF.UserProfile(userIDTextBox.Text,
                                                passwordTextBox.Text, "IRSV", Request.UserHostAddress);
            aacfUserProfile.UserName = userIDTextBox.Text;
            aacfUserProfile.UserAccess = Convert.ToString(aacfUserProfileInfo[0]);
            aacfUserProfile.FailedAttempts = Convert.ToString(aacfUserProfileInfo[1]);
            aacfUserProfile.HasLoggedIn = Convert.ToString(aacfUserProfileInfo[2]);
            aacfUserProfile.PasswordExpiryDate = Convert.ToString(aacfUserProfileInfo[3]);
            aacfUserProfile.Profiles = Convert.ToString(aacfUserProfileInfo[4]).Trim().Replace(" |", "");
            aacfUserProfile.FirstName = Convert.ToString(aacfUserProfileInfo[5]);
            aacfUserProfile.MiddleName = Convert.ToString(aacfUserProfileInfo[6]);
            aacfUserProfile.LastName = Convert.ToString(aacfUserProfileInfo[7]);
            aacfUserProfile.Unit = Convert.ToString(aacfUserProfileInfo[8]);
            aacfUserProfile.IsLocked = Convert.ToString(aacfUserProfileInfo[9]);

            if (aacfUserProfile.UserAccess == "Allowed")
            {
                Session["FullName"] = Convert.ToString(aacfUserProfileInfo[7]) + ", " + Convert.ToString(aacfUserProfileInfo[5]) + " " + Convert.ToString(aacfUserProfileInfo[6]).Substring(0, 1) + ".";

                if (aacfUserProfile.HasLoggedIn == "False")
                //First time to enter the system
                {
                    Session["FirstLogOnChecker"] = aacfUserProfile.HasLoggedIn;
                    Session["UserID"] = userIDTextBox.Text;
                    Session["Password"] = passwordTextBox.Text;
                    Session["FirstName"] = aacfUserProfile.FirstName;
                    Session["LastName"] = aacfUserProfile.LastName;
                    Session["ProfileName"] = aacfUserProfile.Profiles;
                    Response.BufferOutput = true;
                    Response.Redirect("FirstLogon.aspx", false);
                }
                else
                {
                    DateTime expiredPassword = Convert.ToDateTime(aacfUserProfile.PasswordExpiryDate);

                    if (expiredPassword < DateTime.Today)
                    //Password has expired
                    {
                        Session["ExpiredPassword"] = "Expired";
                        Session["UserID"] = userIDTextBox.Text;
                        Session["Password"] = passwordTextBox.Text;
                        Session["FirstName"] = aacfUserProfile.FirstName;
                        Session["LastName"] = aacfUserProfile.LastName;
                        Session["ProfileName"] = aacfUserProfile.Profiles;
                        Response.BufferOutput = true;
                        Response.Redirect("ChangePassword.aspx", false);
                    }
                    else
                    {
                        Session["UserID"] = userIDTextBox.Text;
                        Session["Password"] = passwordTextBox.Text;
                        Session["FirstName"] = aacfUserProfile.FirstName;
                        Session["LastName"] = aacfUserProfile.LastName;
                        Session["ProfileName"] = aacfUserProfile.Profiles;

                        EmployeeDetails empRequest = new EmployeeDetails();
                        empRequest.EmployeeID = userIDTextBox.Text;

                        EmployeeDetails empResult = new EmployeeDetails();
                        empResult = svc.RetrieveEmployeeDetails(empRequest);
                        
                        #region Audit Trail

                        bool isSuccess = false;

                        AuditTrail auditTrail = new AuditTrail();
                        auditTrail.ActionDate = DateTime.Now;
                        auditTrail.ActionTaken = "Login";
                        auditTrail.ActionDetails = "Logged in";
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

                        if (empResult.Group == "ICT Group")
                        {
                            string encUserID = AESCrypt.EncryptDecrypt(userIDTextBox.Text, "Encrypt");
                            string encPassword = AESCrypt.EncryptDecrypt(passwordTextBox.Text, "Encrypt");

                            string iReserveNCVIURL = String.Format(Settings.iReserveNCVIURL, encUserID, encPassword);

                            Response.BufferOutput = true;
                            Response.Redirect(iReserveNCVIURL);
                        }
                        else
                        {
                            Response.BufferOutput = true;
                            Response.Redirect("Default.aspx");
                        }
                    }
                }
            }

            else if (aacfUserProfile.UserAccess == "Your password has expired. Please update your password now.")
            {

                Session["FullName"] = Convert.ToString(aacfUserProfileInfo[7]) + ", " + Convert.ToString(aacfUserProfileInfo[5]) + " " + Convert.ToString(aacfUserProfileInfo[6]).Substring(0, 1) + ".";
                Session["AccountStatus"] = "7";

                if (aacfUserProfile.HasLoggedIn == "False")
                //First time to enter the system
                {
                    Session["ExpiredPassword"] = "Expired";
                    Session["FirstLogOnChecker"] = aacfUserProfile.HasLoggedIn;
                    Session["UserID"] = userIDTextBox.Text;
                    Session["Password"] = passwordTextBox.Text;
                    Session["FirstName"] = aacfUserProfile.FirstName;
                    Session["LastName"] = aacfUserProfile.LastName;
                    Session["ProfileName"] = aacfUserProfile.Profiles;
                    Response.BufferOutput = true;
                    Response.Redirect("FirstLogon.aspx", false);
                }
                else
                {
                    Session["ExpiredPassword"] = "Expired";
                    Session["UserID"] = userIDTextBox.Text;
                    Session["Password"] = passwordTextBox.Text;
                    Session["FirstName"] = aacfUserProfile.FirstName;
                    Session["LastName"] = aacfUserProfile.LastName;
                    Session["ProfileName"] = aacfUserProfile.Profiles;
                    Response.BufferOutput = true;
                    Response.Redirect("ChangePassword.aspx", false);
                }
            }

            else
            {
                string loginPrompt = Convert.ToString(aacfUserProfileInfo[0]);
                ClientScript.RegisterStartupScript(Type.GetType("System.String"), "MessageBox", "<script language=javascript>alert(' " + loginPrompt + " ');</script>");
            }
        }
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

    //    Server.ClearError();
    //}
}