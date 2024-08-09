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
using System.Collections.Generic;

public partial class MaintenanceTrainingRoomTran : System.Web.UI.Page
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
            if (profileName != "Convention Center Administrator" && profileName != "SOA Approver")
            {
                Response.Write("<script language=javascript> alert('You are not allowed to access this page. Please click on the Ok Button to go back to the Home Page.'); window.location.href ='Home.aspx';</script>");
            }
        }

        if (!IsPostBack)
        {
            BindDropDownLists();
            SetInitialRow();
        }

        param = Request.QueryString["Param"];
        status = param.Substring(0, 1);

        if (status == "A")
        {
            statusLabel.Text = "Add Training Room";
            type = 1;
            roomID = 0;
        }
        else if (status == "E")
        {
            statusLabel.Text = "Edit Training Room";
            type = 2;
            roomID = Convert.ToInt32(param.Substring(1));
        }
        else if (status == "V")
        {
            statusLabel.Text = "View Training Room";
            roomID = Convert.ToInt32(param.Substring(1));
            locationDropDownList.Enabled = false;
            roomCodeTextBox.Enabled = false;
            roomNameTextBox.Enabled = false;
            roomDescriptionTextBox.Enabled = false;
            numberOfPartitionNumericBox.Enabled = false;
            upButton.Enabled = false;
            downButton.Enabled = false;
            saveButton.Enabled = false;
        }

        if (!IsPostBack)
        {
            if (status == "E")
            {
                retrieveTrainingRoomRecord(roomID);
            }
            else if (status == "V")
            {
                retrieveTrainingRoomRecord(roomID);
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

    private void SetInitialRow()
    {
        numberOfPartitionNumericBox.Text = "1";
        SetPartitionGridViewRows();
        SetRateGridViewRows();
    }

    protected void upButton_Click(object sender, EventArgs e)
    {
        int partition = Convert.ToInt32(numberOfPartitionNumericBox.Text);

        if (partition != 5)
        {
            partition += 1;
            numberOfPartitionNumericBox.Text = partition.ToString();
        }

        numberOfPartitionNumericBox.Text = partition.ToString();

        SetPartitionGridViewRows();
        SetRateGridViewRows();
    }

    protected void downButton_Click(object sender, EventArgs e)
    {
        int partition = Convert.ToInt32(numberOfPartitionNumericBox.Text);

        if (partition != 1)
        {
            partition -= 1;
            numberOfPartitionNumericBox.Text = partition.ToString();
        }

        numberOfPartitionNumericBox.Text = partition.ToString();

        SetPartitionGridViewRows();
        SetRateGridViewRows();
    }

    private void SetPartitionGridViewRows()
    {
        int row = Convert.ToInt32(numberOfPartitionNumericBox.Text);

        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(new DataColumn("PartitionID", typeof(string)));
        dt.Columns.Add(new DataColumn("PartitionCode", typeof(string)));
        dt.Columns.Add(new DataColumn("PartitionName", typeof(string)));
        dt.Columns.Add(new DataColumn("PartitionDesc", typeof(string)));
        dt.Columns.Add(new DataColumn("MaxPerson", typeof(string)));

        for (int i = 0; i < row; i++)
        {
            dr = dt.NewRow();
            dr["PartitionID"] = "0";
            dr["PartitionCode"] = string.Empty;
            dr["PartitionName"] = string.Empty;
            dr["PartitionDesc"] = string.Empty;
            dr["MaxPerson"] = string.Empty;
            dt.Rows.Add(dr);
        }

        Session["CurrentPartitionTable"] = dt;

        partitionGridView.DataSource = dt;
        partitionGridView.DataBind();
    }

    private void SetRateGridViewRows()
    {
        int row = Convert.ToInt32(numberOfPartitionNumericBox.Text);

        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(new DataColumn("RateID", typeof(string)));
        dt.Columns.Add(new DataColumn("NumberOfPartition", typeof(string)));
        dt.Columns.Add(new DataColumn("RatePerDay", typeof(string)));
        dt.Columns.Add(new DataColumn("RatePerHour", typeof(string)));

        for (int i = 0; i < row; i++)
        {
            dr = dt.NewRow();
            dr["RateID"] = "0";
            dr["NumberOfPartition"] = ((int)(i + 1)).ToString();
            dr["RatePerDay"] = string.Empty;
            dr["RatePerHour"] = string.Empty;
            dt.Rows.Add(dr);
        }

        Session["CurrentRateTable"] = dt;

        rateGridView.DataSource = dt;
        rateGridView.DataBind();
    }

    public void retrieveTrainingRoomRecord(int retRoomID)
    {
        RetrieveTrainingRoomRecordDetailsRequest retrieveTrainingRoomRecordDetailsRequest = new RetrieveTrainingRoomRecordDetailsRequest();
        retrieveTrainingRoomRecordDetailsRequest.RoomID = retRoomID;

        RetrieveTrainingRoomRecordDetailsResult retrieveTrainingRoomRecordDetailsResult = svc.RetrieveTrainingRoomRecordDetails(retrieveTrainingRoomRecordDetailsRequest);

        if (retrieveTrainingRoomRecordDetailsResult.ResultStatus == iReserveWS.ResultStatus.Successful)
        {
            locationDropDownList.SelectedValue = retrieveTrainingRoomRecordDetailsResult.TrainingRoom.LocationID.ToString();
            roomCodeTextBox.Text = retrieveTrainingRoomRecordDetailsResult.TrainingRoom.TRoomCode;
            roomNameTextBox.Text = retrieveTrainingRoomRecordDetailsResult.TrainingRoom.TRoomName;
            roomDescriptionTextBox.Text = retrieveTrainingRoomRecordDetailsResult.TrainingRoom.TRoomDesc;
            numberOfPartitionNumericBox.Text = retrieveTrainingRoomRecordDetailsResult.TrainingRoom.NumberOfPartition.ToString();

            //bind gridviews
            partitionGridView.DataSource = retrieveTrainingRoomRecordDetailsResult.TRPartitionList;
            partitionGridView.DataBind();

            rateGridView.DataSource = retrieveTrainingRoomRecordDetailsResult.TRRateList;
            rateGridView.DataBind();
        }
        else
        {
            Utilities.MyMessageBoxWithHomeRedirect(retrieveTrainingRoomRecordDetailsResult.Message);
        }
    }

    protected void partitionGridView_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (status == "V")
            {
                TextBox partitionCodeTextBox = (TextBox)e.Row.Cells[2].FindControl("partitionCodeTextBox");
                TextBox partitionNameTextBox = (TextBox)e.Row.Cells[3].FindControl("partitionNameTextBox");
                TextBox partitionDescriptionTextBox = (TextBox)e.Row.Cells[4].FindControl("partitionDescriptionTextBox");
                TextBox maxPersonTextBox = (TextBox)e.Row.Cells[5].FindControl("maxPersonTextBox");

                partitionCodeTextBox.Enabled = false;
                partitionNameTextBox.Enabled = false;
                partitionDescriptionTextBox.Enabled = false;
                maxPersonTextBox.Enabled = false;
            }
        }
    }

    protected void rateGridView_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (status == "V")
            {
                TextBox ratePerDayTextBox = (TextBox)e.Row.Cells[2].FindControl("ratePerDayTextBox");
                TextBox ratePerHourTextBox = (TextBox)e.Row.Cells[3].FindControl("ratePerHourTextBox");

                ratePerDayTextBox.Enabled = false;
                ratePerHourTextBox.Enabled = false;
            }
        }
    }

    protected void saveButton_Click(object sender, EventArgs e)
    {
        string strMessage = "";

        int validationStatus;

        TrainingRoom valTrainingRoom = new TrainingRoom();
        valTrainingRoom.TRoomID = roomID;
        valTrainingRoom.TRoomCode = roomCodeTextBox.Text;
        valTrainingRoom.TRoomName = roomNameTextBox.Text;

        ValidateTrainingRoomRecordRequest validateTrainingRoomRecordRequest = new ValidateTrainingRoomRecordRequest();
        validateTrainingRoomRecordRequest.Type = type;
        validateTrainingRoomRecordRequest.TrainingRoom = valTrainingRoom;

        ValidateTrainingRoomRecordResult validateTrainingRoomRecordResult = svc.ValidateTrainingRoomRecord(validateTrainingRoomRecordRequest);

        if (validateTrainingRoomRecordResult.ResultStatus != iReserveWS.ResultStatus.Successful)
        {
            Utilities.MyMessageBoxWithHomeRedirect(validateTrainingRoomRecordResult.Message);
        }
        else
        {
            validationStatus = validateTrainingRoomRecordResult.ValidationStatus;

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
                TrainingRoom tranTrainingRoom = new TrainingRoom();
                tranTrainingRoom.TRoomID = roomID;
                tranTrainingRoom.TRoomCode = roomCodeTextBox.Text.ToUpper();
                tranTrainingRoom.TRoomName = roomNameTextBox.Text.Trim();
                tranTrainingRoom.TRoomDesc = roomDescriptionTextBox.Text;
                tranTrainingRoom.LocationID = Convert.ToInt32(locationDropDownList.SelectedValue);
                tranTrainingRoom.NumberOfPartition = Convert.ToInt32(numberOfPartitionNumericBox.Text);
                tranTrainingRoom.IsDeleted = false;

                AuditTrail tranAuditTrail = new AuditTrail();
                tranAuditTrail.UserID = userID;
                tranAuditTrail.MacAdress = macAddress;
                tranAuditTrail.Browser = browser;
                tranAuditTrail.BrowserVersion = browserVersion;

                List<TRPartition> trPartitionList = new List<TRPartition>();
                List<TRRate> trRateList = new List<TRRate>();

                int rowsCount = Convert.ToInt32(numberOfPartitionNumericBox.Text);

                for (int i = 0; i < rowsCount; i++)
                {
                    int partitionID = Convert.ToInt32(partitionGridView.Rows[i].Cells[1].Text);
                    TextBox partitionCodeTextBox = (TextBox)partitionGridView.Rows[i].Cells[2].FindControl("partitionCodeTextBox");
                    TextBox partitionNameTextBox = (TextBox)partitionGridView.Rows[i].Cells[3].FindControl("partitionNameTextBox");
                    TextBox partitionDescriptionTextBox = (TextBox)partitionGridView.Rows[i].Cells[4].FindControl("partitionDescriptionTextBox");
                    TextBox maxPersonTextBox = (TextBox)partitionGridView.Rows[i].Cells[5].FindControl("maxPersonTextBox");

                    TRPartition trPartition = new TRPartition();
                    trPartition.PartitionID = partitionID;
                    trPartition.PartitionCode = partitionCodeTextBox.Text;
                    trPartition.PartitionName = partitionNameTextBox.Text;
                    trPartition.PartitionDesc = partitionDescriptionTextBox.Text;
                    trPartition.MaxPerson = Convert.ToInt32(maxPersonTextBox.Text);
                    trPartitionList.Add(trPartition);

                    int rateID = Convert.ToInt32(rateGridView.Rows[i].Cells[0].Text);
                    int numberOfPartition = Convert.ToInt32(rateGridView.Rows[i].Cells[1].Text);
                    TextBox ratePerDayTextBox = (TextBox)rateGridView.Rows[i].Cells[2].FindControl("ratePerDayTextBox");
                    TextBox ratePerHourTextBox = (TextBox)rateGridView.Rows[i].Cells[3].FindControl("ratePerHourTextBox");

                    TRRate trRate = new TRRate();
                    trRate.RateID = rateID;
                    trRate.NumberOfPartition = numberOfPartition;
                    trRate.RatePerDay = Convert.ToDecimal(ratePerDayTextBox.Text).ToString("#,###.00");
                    trRate.RatePerHour = Convert.ToDecimal(ratePerHourTextBox.Text).ToString("#,###.00");
                    trRateList.Add(trRate);
                }

                TrainingRoomTransactionRequest trainingRoomTransactionRequest = new TrainingRoomTransactionRequest();
                trainingRoomTransactionRequest.Type = type;
                trainingRoomTransactionRequest.TrainingRoom = tranTrainingRoom;
                trainingRoomTransactionRequest.TRPartitionList = trPartitionList.ToArray();
                trainingRoomTransactionRequest.TRRateList = trRateList.ToArray();
                trainingRoomTransactionRequest.AuditTrail = tranAuditTrail;

                TrainingRoomTransactionResult trainingRoomTransactionResult = svc.TrainingRoomTransaction(trainingRoomTransactionRequest);

                if (trainingRoomTransactionResult.ResultStatus != iReserveWS.ResultStatus.Successful)
                {
                    Utilities.MyMessageBoxWithHomeRedirect(trainingRoomTransactionResult.Message);
                }
                else
                {
                    if (type == 1)
                    {
                        #region Audit Trail

                        bool isSuccess = false;

                        AuditTrail auditTrail = new AuditTrail();
                        auditTrail.ActionDate = DateTime.Now;
                        auditTrail.ActionTaken = "Add Training Room";
                        auditTrail.ActionDetails = "Added Training Room || Room Name: " + roomNameTextBox.Text.Trim();
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
                        auditTrail.ActionTaken = "Edit Training Room";
                        auditTrail.ActionDetails = "Edited Training Room || Room ID: " + roomID;
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