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

public partial class MaintenanceRoomTran : System.Web.UI.Page
{
    Service svc = new Service();
    string param, userID, macAddress, browser, browserVersion;
    int type, roomID;
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
                Response.Write("<script language=javascript> alert('You are not allowed to access this page. Please click on the Ok Button to go back to the Home Page.'); window.location.href ='Home.aspx';</script>");
            }
        }

        if (!IsPostBack)
        {
            BindDropDownLists();
        }

        param = Request.QueryString["Param"];
        status = param.Substring(0, 1);

        if (status == "A")
        {
            statusLabel.Text = "Add Conference Room";
            type = 1;
            roomID = 0;
        }
        else if (status == "E")
        {
            statusLabel.Text = "Edit Conference Room";
            type = 2;
            roomID = Convert.ToInt32(param.Substring(1));
        }
        else if (status == "V")
        {
            statusLabel.Text = "View Conference Room";
            roomID = Convert.ToInt32(param.Substring(1));
            locationDropDownList.Enabled = false;
            roomCodeTextBox.Enabled = false;
            roomNameTextBox.Enabled = false;
            roomDescriptionTextBox.Enabled = false;
            maxPersonNumericBox.Enabled = false;
            dataPortCheckBox.Enabled = false;
            monitorCheckBox.Enabled = false;
            rateNumericBox.Enabled = false;
            tabletIDTextBox.Enabled = false;
            monitorDisplayDropDownList.Enabled = false;
            saveButton.Enabled = false;
        }

        if (!IsPostBack)
        {
            if (status == "E")
            {
                retrieveConferenceRoomRecord(roomID);
            }
            else if (status == "V")
            {
                retrieveConferenceRoomRecord(roomID);
            }
        }

        userID = HttpContext.Current.Session["UserID"].ToString();
        macAddress = HttpContext.Current.Session["MacAddress"].ToString();
        browser = Request.Browser.Browser;
        browserVersion = Request.Browser.Type;
    }

    public void BindDropDownLists()
    {
        try
        {
            locationDropDownList.DataSource = svc.RetrieveLocationRecords(string.Empty, string.Empty);
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericWebServiceMessage);
        }

        locationDropDownList.DataValueField = "LocationID";
        locationDropDownList.DataTextField = "LocationName";
        locationDropDownList.DataBind();
        locationDropDownList.Items.Insert(0, new ListItem("Select", "0"));

        try
        {
            monitorDisplayDropDownList.DataSource = svc.RetrieveMonitorDisplayRecords();
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericWebServiceMessage);
        }

        monitorDisplayDropDownList.DataValueField = "MonitorDisplayCode";
        monitorDisplayDropDownList.DataTextField = "MonitorDisplayName";
        monitorDisplayDropDownList.DataBind();
    }

    public void retrieveConferenceRoomRecord(int retroomID)
    {
        MaintenanceConferenceRoomList roomRecord = new MaintenanceConferenceRoomList();

        try
        {
            roomRecord = svc.RetrieveConferenceRoomRecordDetails(retroomID);
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericWebServiceMessage);
        }

        locationDropDownList.SelectedValue = roomRecord.LocationID.ToString();
        roomCodeTextBox.Text = roomRecord.RoomCode;
        roomNameTextBox.Text = roomRecord.RoomName;
        roomDescriptionTextBox.Text = roomRecord.RoomDesc;
        maxPersonNumericBox.Text = roomRecord.MaxPerson;
        dataPortCheckBox.Checked = Convert.ToBoolean(roomRecord.IsDataPortAvailable);
        monitorCheckBox.Checked = Convert.ToBoolean(roomRecord.IsMonitorAvailable);
        rateNumericBox.Text = Convert.ToDecimal(roomRecord.RatePerHour).ToString("#,##0.00");
        tabletIDTextBox.Text = roomRecord.TabletID;
        monitorDisplayDropDownList.SelectedValue = roomRecord.MonitorDisplayCode.ToString();
    }
    protected void saveButton_Click(object sender, EventArgs e)
    {
        string strMessage = "";

        int validationStatus;

        try
        {
            validationStatus = svc.ValidateConferenceRoomRecord(type, roomID, roomCodeTextBox.Text, roomNameTextBox.Text, Convert.ToInt32(monitorDisplayDropDownList.SelectedValue));
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericWebServiceMessage);
        }

        if (validationStatus == 1)
        {
            strMessage = "Room code and name already exists.";
            ClientScript.RegisterStartupScript(Type.GetType("System.String"), "messagebox", "<script type=\"text/javascript\">alert('" + strMessage + "');</script>");
        }
        else if (validationStatus == 2)
        {
            strMessage = "Room code already exists.";
            ClientScript.RegisterStartupScript(Type.GetType("System.String"), "messagebox", "<script type=\"text/javascript\">alert('" + strMessage + "');</script>");
        }
        else if (validationStatus == 3)
        {
            strMessage = "Room name already exists.";
            ClientScript.RegisterStartupScript(Type.GetType("System.String"), "messagebox", "<script type=\"text/javascript\">alert('" + strMessage + "');</script>");
        }
        else if (validationStatus == 4)
        {
            strMessage = "Number of rooms in a monitor display is up to 8 only.";
            ClientScript.RegisterStartupScript(Type.GetType("System.String"), "messagebox", "<script type=\"text/javascript\">alert('" + strMessage + "');</script>");
        }
        else if (validationStatus == 0)
        {
            try
            {
                svc.ConferenceRoomRecordTransaction(type, userID, roomID, roomCodeTextBox.Text.ToUpper(), roomNameTextBox.Text.Trim(),
                                                    roomDescriptionTextBox.Text, Convert.ToInt32(locationDropDownList.SelectedValue),
                                                    Convert.ToInt32(maxPersonNumericBox.Text), dataPortCheckBox.Checked, monitorCheckBox.Checked,
                                                    rateNumericBox.Text, tabletIDTextBox.Text, Convert.ToInt32(monitorDisplayDropDownList.SelectedValue), false, macAddress, browser, browserVersion);
            }

            catch (SoapException ex)
            {
                throw new Exception(Settings.GenericWebServiceMessage);
            }

            if (type == 1)
            {
                #region Audit Trail

                bool isSuccess = false;

                AuditTrail auditTrail = new AuditTrail();
                auditTrail.ActionDate = DateTime.Now;
                auditTrail.ActionTaken = "Add Conference Room";
                auditTrail.ActionDetails = "Added Conference Room || Room Name: " + roomNameTextBox.Text.Trim();
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
                #region Audit Trail

                bool isSuccess = false;

                AuditTrail auditTrail = new AuditTrail();
                auditTrail.ActionDate = DateTime.Now;
                auditTrail.ActionTaken = "Edit Conference Room";
                auditTrail.ActionDetails = "Edited Conference Room || Room ID: " + roomID;
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