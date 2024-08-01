using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using wsAACF;
using iReserveWS;
using AppCryptor;
using AESCryptor;

public partial class Redirect : System.Web.UI.Page
{
    static AACFws serviceAACF = new AACFws();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1d);
        Response.Expires = -1500;
        Response.CacheControl = "no-cache";

        string userID, password, macAddress, browser, browserVersion;

        if (Request.QueryString["q1"] == null || Request.QueryString["q2"] == null)
        {
            userID = null;
            password = null;
        }
        else
        {
            string encUserID = Request.QueryString["q1"].ToString();
            string encPassword = Request.QueryString["q2"].ToString();

            try
            {
                encUserID = Utilities.FormatURLToBase64(encUserID);
                encPassword = Utilities.FormatURLToBase64(encPassword);

                userID = AESCrypt.EncryptDecrypt(encUserID, "Decrypt");
                password = AESCrypt.EncryptDecrypt(encPassword, "Decrypt");
            }
            catch
            {
                userID = null;
                password = null;
            }
        }

        if (userID == null || password == null)
        {
            ClientScript.RegisterStartupScript(Type.GetType("System.String"), "messagebox", "<script type='text/javascript'>location.replace('../LegalBanner.aspx')</script>");
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "invoke", "<script language='javascript'>GetMacAddress();</script>");

            macAddress = macAddressHiddenField.Value.ToString();
            browser = Request.Browser.Browser;
            browserVersion = Request.Browser.Type;

            Session["MacAddress"] = macAddress;

            AACFUserProfile aacfUserProfile = new AACFUserProfile();

            object[] aacfUserProfileInfo = serviceAACF.UserProfile(userID,
                                                password, "IRSV", Request.UserHostAddress);
            aacfUserProfile.UserName = userID;
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

                Session["UserID"] = userID;
                Session["Password"] = password;
                Session["FirstName"] = aacfUserProfile.FirstName;
                Session["LastName"] = aacfUserProfile.LastName;
                Session["ProfileName"] = aacfUserProfile.Profiles;

                Response.BufferOutput = true;
                Response.Redirect("Default.aspx");
            }
            else
            {
                ClientScript.RegisterStartupScript(Type.GetType("System.String"), "messagebox", "<script type='text/javascript'>alert('Invalid session.');location.replace('../LegalBanner.aspx')</script>");
            }
        }
    }
}