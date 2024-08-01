using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using iReserveWS;
using System.Web.Services.Protocols;

public partial class MaintenanceLocationTran : System.Web.UI.Page
{
    Service svc = new Service();
    string param, userID, macAddress, browser, browserVersion;
    int type, locationID;
    public string status;

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

        string profileName = Convert.ToString(Session["ProfileName"]);

        if (profileName != "")
        {
            if (profileName != "Conference Room Administrator")
            {
                Response.Write("<script language=javascript> alert('You are not allowed to access this page. Please click on the Ok Button to go back to the Home Page.'); window.location.href ='Default.aspx';</script>");
            }
        }

        param = Request.QueryString["Param"];
        status = param.Substring(0, 1);

        if (status == "A")
        {
            statusLabel.Text = "Add Location";
            type = 1;
            locationID = 0;
        }
        else if (status == "E")
        {
            statusLabel.Text = "Edit Location";
            type = 2;
            locationID = Convert.ToInt32(param.Substring(1));
            //locationCodeTextBox.Enabled = false;
        }
        else if (status == "V")
        {
            statusLabel.Text = "View Location";
            locationID = Convert.ToInt32(param.Substring(1));
            locationCodeTextBox.Enabled = false;
            locationNameTextBox.Enabled = false;
            locationDescriptionTextBox.Enabled = false;
            saveButton.Enabled = false;
        }

        if (!IsPostBack)
        {
            if (status == "E")
            {
                RetrieveLocationRecordDetails(locationID);
            }
            else if (status == "V")
            {
                RetrieveLocationRecordDetails(locationID);
            }
        }

        userID = HttpContext.Current.Session["UserID"].ToString();
        macAddress = HttpContext.Current.Session["MacAddress"].ToString();
        browser = Request.Browser.Browser;
        browserVersion = Request.Browser.Type;
    }

    public void RetrieveLocationRecordDetails(int locationNo)
    {
        param = Request.QueryString["Param"];
        status = param.Substring(0, 1);

        MaintenanceLocationList locationRecord = new MaintenanceLocationList();

        try
        {
            locationRecord = svc.RetrieveLocationRecordDetails(locationNo);
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericWebServiceMessage);
        }

        locationCodeTextBox.Text = locationRecord.LocationCode;
        locationNameTextBox.Text = locationRecord.LocationName;
        locationDescriptionTextBox.Text = locationRecord.LocationDesc;
    }

    protected void saveButton_Click(object sender, EventArgs e)
    {
        string strMessage = "";

        int validationStatus;

        try
        {
            validationStatus = svc.ValidateLocationRecord(type, locationID, locationCodeTextBox.Text, locationNameTextBox.Text);
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericWebServiceMessage);
        }

        if (validationStatus == 1)
        {
            strMessage = "Location code and name already exists.";
            ClientScript.RegisterStartupScript(Type.GetType("System.String"), "messagebox", "<script type=\"text/javascript\">alert('" + strMessage + "');</script>");
        }
        else if (validationStatus == 2)
        {
            strMessage = "Location code already exists.";
            ClientScript.RegisterStartupScript(Type.GetType("System.String"), "messagebox", "<script type=\"text/javascript\">alert('" + strMessage + "');</script>");
        }
        else if (validationStatus == 3)
        {
            strMessage = "Location name already exists.";
            ClientScript.RegisterStartupScript(Type.GetType("System.String"), "messagebox", "<script type=\"text/javascript\">alert('" + strMessage + "');</script>");
        }
        else if (validationStatus == 0)
        {
            if (type == 1)
            {
                try
                {
                    svc.LocationRecordTransaction(type, userID, locationID, locationCodeTextBox.Text.ToUpper(), locationNameTextBox.Text.Trim(),
                                                            locationDescriptionTextBox.Text, false, macAddress, browser, browserVersion);
                }

                catch (SoapException ex)
                {
                    throw new Exception(Settings.GenericWebServiceMessage);
                }

                #region Audit Trail

                bool isSuccess = false;

                AuditTrail auditTrail = new AuditTrail();
                auditTrail.ActionDate = DateTime.Now;
                auditTrail.ActionTaken = "Add Location";
                auditTrail.ActionDetails = "Added Location || Location Name: " + locationNameTextBox.Text.Trim();
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

                ClientScript.RegisterStartupScript(Type.GetType("System.String"), "messagebox", "<script type=\"text/javascript\">OnSucceeded(1);</script>");

            }
            else if (type == 2)
            {
                try
                {
                    svc.LocationRecordTransaction(type, userID, locationID, locationCodeTextBox.Text.ToUpper(), locationNameTextBox.Text,
                                                            locationDescriptionTextBox.Text, false, macAddress, browser, browserVersion);
                }

                catch (SoapException ex)
                {
                    throw new Exception(Settings.GenericWebServiceMessage);
                }

                #region Audit Trail

                bool isSuccess = false;

                AuditTrail auditTrail = new AuditTrail();
                auditTrail.ActionDate = DateTime.Now;
                auditTrail.ActionTaken = "Edit Location";
                auditTrail.ActionDetails = "Edited Location || Location ID: " + locationID;
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

                ClientScript.RegisterStartupScript(Type.GetType("System.String"), "messagebox", "<script type=\"text/javascript\">OnSucceeded(2);</script>");
            }
        }
    }
}