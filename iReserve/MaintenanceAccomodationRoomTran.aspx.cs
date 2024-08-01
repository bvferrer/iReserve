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

public partial class MaintenanceAccomodationRoomTran : System.Web.UI.Page
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
            if (profileName != "Convention Center Administrator")
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
            statusLabel.Text = "Add Accomodation Room";
            type = 1;
            roomID = 0;
        }
        else if (status == "E")
        {
            statusLabel.Text = "Edit Accomodation Room";
            type = 2;
            roomID = Convert.ToInt32(param.Substring(1));
        }
        else if (status == "V")
        {
            statusLabel.Text = "View Accomodation Room";
            roomID = Convert.ToInt32(param.Substring(1));
            locationDropDownList.Enabled = false;
            roomCodeTextBox.Enabled = false;
            roomNameTextBox.Enabled = false;
            roomDescriptionTextBox.Enabled = false;
            maxPersonNumericBox.Enabled = false;
            rateNumericBox.Enabled = false;
            saveButton.Enabled = false;
        }

        if (!IsPostBack)
        {
            if (status == "E")
            {
                retrieveAccomodationRoomRecord(roomID);
            }
            else if (status == "V")
            {
                retrieveAccomodationRoomRecord(roomID);
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
    }

    public void retrieveAccomodationRoomRecord(int retRoomID)
    {
        RetrieveAccomodationRoomRecordDetailsRequest retrieveAccomodationRoomRecordDetailsRequest = new RetrieveAccomodationRoomRecordDetailsRequest();
        retrieveAccomodationRoomRecordDetailsRequest.RoomID = retRoomID;

        RetrieveAccomodationRoomRecordDetailsResult retrieveAccomodationRoomRecordDetailsResult = svc.RetrieveAccomodationRoomRecordDetails(retrieveAccomodationRoomRecordDetailsRequest);

        if (retrieveAccomodationRoomRecordDetailsResult.ResultStatus == iReserveWS.ResultStatus.Successful)
        {
            locationDropDownList.SelectedValue = retrieveAccomodationRoomRecordDetailsResult.AccomodationRoom.LocationID.ToString();
            roomCodeTextBox.Text = retrieveAccomodationRoomRecordDetailsResult.AccomodationRoom.RoomCode;
            roomNameTextBox.Text = retrieveAccomodationRoomRecordDetailsResult.AccomodationRoom.RoomName;
            roomDescriptionTextBox.Text = retrieveAccomodationRoomRecordDetailsResult.AccomodationRoom.RoomDesc;
            maxPersonNumericBox.Text = retrieveAccomodationRoomRecordDetailsResult.AccomodationRoom.MaxPerson.ToString();
            rateNumericBox.Text = Convert.ToDecimal(retrieveAccomodationRoomRecordDetailsResult.AccomodationRoom.RatePerNight).ToString("#,##0.00");
        }
        else
        {
            Utilities.MyMessageBoxWithHomeRedirect(retrieveAccomodationRoomRecordDetailsResult.Message);
        }
    }
    protected void saveButton_Click(object sender, EventArgs e)
    {
        string strMessage = "";

        int validationStatus;

        AccomodationRoom valAccomodationRoom = new AccomodationRoom();
        valAccomodationRoom.AccRoomID = roomID;
        valAccomodationRoom.RoomCode = roomCodeTextBox.Text;
        valAccomodationRoom.RoomName = roomNameTextBox.Text;

        ValidateAccomodationRoomRecordRequest validateAccomodationRoomRecordRequest = new ValidateAccomodationRoomRecordRequest();
        validateAccomodationRoomRecordRequest.Type = type;
        validateAccomodationRoomRecordRequest.AccomodationRoom = valAccomodationRoom;

        ValidateAccomodationRoomRecordResult validateAccomodationRoomRecordResult = svc.ValidateAccomodationRoomRecord(validateAccomodationRoomRecordRequest);

        if (validateAccomodationRoomRecordResult.ResultStatus != iReserveWS.ResultStatus.Successful)
        {
            Utilities.MyMessageBoxWithHomeRedirect(validateAccomodationRoomRecordResult.Message);
        }
        else
        {
            validationStatus = validateAccomodationRoomRecordResult.ValidationStatus;

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
            else if (validationStatus == 0)
            {
                AccomodationRoom tranAccomodationRoom = new AccomodationRoom();
                tranAccomodationRoom.AccRoomID = roomID;
                tranAccomodationRoom.RoomCode = roomCodeTextBox.Text.ToUpper();
                tranAccomodationRoom.RoomName = roomNameTextBox.Text.Trim();
                tranAccomodationRoom.RoomDesc = roomDescriptionTextBox.Text;
                tranAccomodationRoom.LocationID = Convert.ToInt32(locationDropDownList.SelectedValue);
                tranAccomodationRoom.MaxPerson = Convert.ToInt32(maxPersonNumericBox.Text);
                tranAccomodationRoom.RatePerNight = rateNumericBox.Text;
                tranAccomodationRoom.IsDeleted = false;

                AuditTrail tranAuditTrail = new AuditTrail();
                tranAuditTrail.UserID = userID;
                tranAuditTrail.MacAdress = macAddress;
                tranAuditTrail.Browser = browser;
                tranAuditTrail.BrowserVersion = browserVersion;

                AccomodationRoomTransactionRequest accomodationRoomTransactionRequest = new AccomodationRoomTransactionRequest();
                accomodationRoomTransactionRequest.Type = type;
                accomodationRoomTransactionRequest.AccomodationRoom = tranAccomodationRoom;
                accomodationRoomTransactionRequest.AuditTrail = tranAuditTrail;

                AccomodationRoomTransactionResult accomodationRoomTransactionResult = svc.AccomodationRoomTransaction(accomodationRoomTransactionRequest);

                if (accomodationRoomTransactionResult.ResultStatus != iReserveWS.ResultStatus.Successful)
                {
                    Utilities.MyMessageBoxWithHomeRedirect(accomodationRoomTransactionResult.Message);
                }
                else
                {
                    if (type == 1)
                    {
                        #region Audit Trail

                        bool isSuccess = false;

                        AuditTrail auditTrail = new AuditTrail();
                        auditTrail.ActionDate = DateTime.Now;
                        auditTrail.ActionTaken = "Add Accomodation Room";
                        auditTrail.ActionDetails = "Added Accomodation Room || Room Name: " + roomNameTextBox.Text.Trim();
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
                        auditTrail.ActionTaken = "Edit Accomodation Room";
                        auditTrail.ActionDetails = "Edited Accomodation Room || Room ID: " + roomID;
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
    }
}