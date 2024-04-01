using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iReserveWS;
using System.Data;
using System.IO;
using System.Web.Services.Protocols;

public partial class RequestDetails : System.Web.UI.Page
{
    Service svc = new Service();

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
            if (profileName != "Requestor")
            {
                Response.Write("<script language=javascript> alert('You are not allowed to access this page. Please click on the Ok Button to go back to the Home Page.'); window.location.href ='Default.aspx';</script>");
            }
        }

        if (Convert.ToString(Session["ReferenceNumber"]) == "")
        {
            Response.Redirect("Default.aspx");
        }

        if (!IsPostBack)
        {

        }

        LoadRequestDetails();
    }

    public void LoadRequestDetails()
    {
        CRRequest request = new CRRequest();
        request.RequestReferenceNo = Convert.ToString(Session["ReferenceNumber"]);

        CRRequest result = new CRRequest();

        try
        {
            result = svc.RetrieveCRRequestDetails(request);
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericWebServiceMessage);
        }

        referenceNumberLabel.Text = result.RequestReferenceNo;
        referenceNumberHiddenField.Value = result.RequestReferenceNo;
        dateLabel.Text = result.Date.ToShortDateString();
        startTimeLabel.Text = result.StartTime.StartTime12;
        endTimeLabel.Text = result.EndTime.EndTime12;
        roomLabel.Text = result.ConferenceRoom.RoomName;
        dateRequestedLabel.Text = result.DateRequested.ToString();
        agendaLabel.Text = result.Agenda;
        dataPortLabel.Visible = Convert.ToBoolean(result.IsUseDataPort);
        monitorLabel.Visible = Convert.ToBoolean(result.IsUseMonitor);
        statusLabel.Text = result.Status.StatusName;
        statusCodeHiddenField.Value = result.Status.StatusCode.ToString();

        if (dataPortLabel.Visible == false && monitorLabel.Visible == false)
        {
            noneLabel.Visible = true;
        }

        attendeeGridView.DataSource = result.AttendeeList;
        attendeeGridView.DataBind();

        request.Attachment = new CRRequestAttachment();
        request.Attachment.RequestReferenceNo = referenceNumberHiddenField.Value;

        CRRequestAttachment[] attachmentList;

        try
        {
            attachmentList = svc.RetrieveCRRequestAttachment(request);
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericWebServiceMessage);
        }

        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("StatusCode", typeof(int)));
        dt.Columns.Add(new DataColumn("StatusName", typeof(string)));
        dt.Columns.Add(new DataColumn("FileName", typeof(string)));

        foreach (CRRequestAttachment attachment in attachmentList)
        {
            DataRow dr = dt.NewRow();
            dr["StatusCode"] = attachment.Status.StatusCode;
            dr["StatusName"] = attachment.Status.StatusName;
            dr["FileName"] = attachment.FileName;
            dt.Rows.Add(dr);
        }

        attachmentGridView.DataSource = dt;
        attachmentGridView.DataBind();

        DateTime requestDateTime = result.Date.Add(TimeSpan.Parse(result.StartTime.StartTime));

        if (result.Status.StatusCode == StatusCode.ForConfirmation || result.Status.StatusCode == StatusCode.Confirmed)
        {
            requestorCancelRequestButton.Visible = true;

            if (requestDateTime <= DateTime.Now)
            {
                requestorCancelRequestButton.Enabled = false;
            }
        }

        //if (result.Status.StatusCode == StatusCode.Confirmed)
        //{
        //    cancelRequestButton.Visible = true;

        //    if (requestDateTime <= DateTime.Now)
        //    {
        //        cancelRequestButton.Enabled = false;
        //    }
        //}
    }

    protected void downloadLinkButton_Click(object sender, EventArgs e)
    {
        LinkButton link = (LinkButton)sender;
        GridViewRow gv = (GridViewRow)(link.Parent.Parent);

        CRRequest request = new CRRequest();
        request.RequestReferenceNo = referenceNumberHiddenField.Value;
        request.Status = new Status();
        request.Status.StatusCode = Convert.ToInt32(gv.Cells[3].Text);

        CRRequestAttachment attachment = new CRRequestAttachment();

        try
        {
            attachment = svc.RetrieveCRRequestAttachmentByStatus(request);
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericWebServiceMessage);
        }

        DownloadAttachment(attachment.FileName, attachment.FileType, attachment.File);
    }

    private void DownloadAttachment(string fileName, string fileType, byte[] file)
    {
        HttpContext.Current.Response.Buffer = false;
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.ContentType = fileType;
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);

        //Code for streaming the object while writing
        const int ChunkSize = 1024;
        byte[] buffer = new byte[ChunkSize];
        //byte[] binary = byteDocument as byte[];
        MemoryStream ms = new MemoryStream(file);

        int SizeToWrite = ChunkSize;

        for (int i = 0; i < file.GetUpperBound(0) - 1; i = i + ChunkSize)
        {
            if (!HttpContext.Current.Response.IsClientConnected) return;
            if (i + ChunkSize >= file.Length)
                SizeToWrite = file.Length - i;
            byte[] chunk = new byte[SizeToWrite];
            ms.Read(chunk, 0, SizeToWrite);
            HttpContext.Current.Response.BinaryWrite(chunk);
            HttpContext.Current.Response.Flush();
        }

        HttpContext.Current.Response.Close();
    }

    #region Cancel Confirmed Request

    //protected void cancelRequestButton_Click(object sender, EventArgs e)
    //{
    //    submitDetailsDiv.Style.Add("display", "block");
    //    submitDetails.Show();
    //}

    //protected void cancelButton_Click(object sender, EventArgs e)
    //{
    //    submitDetails.Hide();
    //    ClearControls();
    //}

    //private void ClearControls()
    //{
    //    remarksTextBox.Text = "";
    //    approvalLabel.Text = "";
    //}

    //protected void submitButton_Click(object sender, EventArgs e)
    //{
    //    string strMessage = "";
    //    int fileNameLength = approvalFileUpload.FileName.Length;
    //    int fileSizeLimit = 3000000;
    //    int fileSizeActual = approvalFileUpload.PostedFile.ContentLength;

    //    if (fileNameLength > 50)
    //    {
    //        strMessage = "File name is too long. File name should NOT be more than 50 characters ";
    //        strMessage += "(including spaces and file extension).";
    //        approvalLabel.Text = strMessage;
    //        submitDetails.Show();
    //    }
    //    else
    //    {
    //        if (fileSizeActual > fileSizeLimit)
    //        {
    //            strMessage = "Allowed attachment file size is up to 3MB only.";
    //            approvalLabel.Text = strMessage;
    //            submitDetails.Show();
    //        }
    //        else
    //        {
    //            approvalLabel.Text = "";

    //            Submit();
    //            ClearControls();
    //        }
    //    }
    //}

    //public void Submit()
    //{
    //    string smessage = "";

    //    CRRequest request = new CRRequest();
    //    request.RequestReferenceNo = referenceNumberHiddenField.Value;
    //    request.Status = new Status();
    //    request.Status.StatusCode = StatusCode.ForCancellation;

    //    CRRequestHistory history = new CRRequestHistory();
    //    history.RequestReferenceNumber = referenceNumberHiddenField.Value;
    //    history.Status = new Status();
    //    history.Status.StatusCode = StatusCode.ForCancellation;
    //    history.ProcessedByID = Session["UserID"].ToString();
    //    history.ProcessedBy = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();
    //    history.Remarks = remarksTextBox.Text.Trim();
    //    request.RequestHistory = history;

    //    CRRequestAttachment attachment = new CRRequestAttachment();
    //    attachment.RequestReferenceNo = referenceNumberHiddenField.Value;
    //    attachment.Status = new Status();
    //    attachment.Status.StatusCode = StatusCode.ForCancellation;
    //    attachment.FileName = approvalFileUpload.FileName.Replace(" ", "_");
    //    attachment.FileType = approvalFileUpload.PostedFile.ContentType;
    //    attachment.FileSize = approvalFileUpload.PostedFile.ContentLength;
    //    attachment.File = approvalFileUpload.FileBytes;
    //    request.Attachment = attachment;

    //    Result result = svc.UpdateReservationRequest(request);

    //    if (result.ResultStatus == iReserveWS.ResultStatus.Successful)
    //    {
    //        smessage = "Your request to cancel the reservation with Reference Number: " + referenceNumberHiddenField.Value + " has been successfully submitted.";
    //    }
    //    else if (result.ResultStatus == iReserveWS.ResultStatus.Error)
    //    {
    //        smessage = result.Message;
    //    }

    //    #region Audit Trail

    //    bool isSuccess = false;

    //    AuditTrail auditTrail = new AuditTrail();
    //    auditTrail.ActionDate = DateTime.Now;
    //    auditTrail.ActionTaken = "Request for cancellation";
    //    auditTrail.ActionDetails = "Reference Number: " + referenceNumberHiddenField.Value + " || Status: For Cancellation";
    //    auditTrail.Browser = HttpContext.Current.Request.Browser.Browser;
    //    auditTrail.BrowserVersion = HttpContext.Current.Request.Browser.Version;
    //    auditTrail.IpAddress = HttpContext.Current.Request.ServerVariables[32];
    //    System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();

    //    auditTrail.MacAdress = Session["MacAddress"].ToString();
    //    auditTrail.UserID = Session["UserID"].ToString();

    //    try
    //    {
    //        isSuccess = svc.InsertAuditTrailEntry(auditTrail);
    //    }

    //    catch (SoapException ex)
    //    {
    //        throw new Exception(Settings.GenericAuditTrailMessage);
    //    }

    //    #endregion

    //    Utilities.MyMessageBoxWithHomeRedirect(smessage);
    //}

    #endregion

    #region Requestor Cancel Request

    protected void requestorCancelRequestButton_Click(object sender, EventArgs e)
    {
        submitDetailsDiv2.Style.Add("display", "block");
        submitDetails2.Show();
    }

    protected void cancelButton2_Click(object sender, EventArgs e)
    {
        submitDetails2.Hide();
        ClearControls2();
    }

    private void ClearControls2()
    {
        remarksTextBox2.Text = "";
    }

    protected void submitButton2_Click(object sender, EventArgs e)
    {
        Submit2();
        ClearControls2();
    }

    public void Submit2()
    {
        string smessage = "";

        CRRequest request = new CRRequest();
        request.RequestReferenceNo = referenceNumberHiddenField.Value;
        request.Status = new Status();
        request.Status.StatusCode = StatusCode.Cancelled;

        CRRequestHistory history = new CRRequestHistory();
        history.RequestReferenceNumber = referenceNumberHiddenField.Value;
        history.Status = new Status();
        history.Status.StatusCode = StatusCode.Cancelled;
        history.ProcessedByID = Session["UserID"].ToString();
        history.ProcessedBy = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();
        history.Remarks = remarksTextBox2.Text.Trim();
        request.RequestHistory = history;

        //Result result = svc.CancelReservationRequest(request);
        Result result = svc.UpdateReservationRequest(request);

        if (result.ResultStatus == iReserveWS.ResultStatus.Successful)
        {
            smessage = "Your reservation request with Reference Number: " + referenceNumberHiddenField.Value + " has been successfully cancelled.";
        }
        else if (result.ResultStatus == iReserveWS.ResultStatus.Error)
        {
            smessage = result.Message;
        }

        #region Audit Trail

        bool isSuccess = false;

        AuditTrail auditTrail = new AuditTrail();
        auditTrail.ActionDate = DateTime.Now;
        auditTrail.ActionTaken = "Cancelled the request";
        auditTrail.ActionDetails = "Reference Number: " + referenceNumberHiddenField.Value + " || Status: Cancelled";
        auditTrail.Browser = HttpContext.Current.Request.Browser.Browser;
        auditTrail.BrowserVersion = HttpContext.Current.Request.Browser.Version;
        auditTrail.IpAddress = HttpContext.Current.Request.ServerVariables[32];
        System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();

        auditTrail.MacAdress = Session["MacAddress"].ToString();
        auditTrail.UserID = Session["UserID"].ToString();

        try
        {
            isSuccess = svc.InsertAuditTrailEntry(auditTrail);
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericAuditTrailMessage);
        }

        #endregion

        Utilities.MyMessageBoxWithHomeRedirect(smessage);
    }

    #endregion

    #region Request History

    protected void closeButton_Click(object sender, EventArgs e)
    {
        historyDetails.Hide();
    }

    protected void viewHistoryLinkButton_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("Status", typeof(string)));
        dt.Columns.Add(new DataColumn("DateProcessed", typeof(DateTime)));
        dt.Columns.Add(new DataColumn("ProcessedBy", typeof(string)));
        dt.Columns.Add(new DataColumn("Remarks", typeof(string)));

        CRRequest request = new CRRequest();
        request.RequestHistory = new CRRequestHistory();
        request.RequestHistory.RequestReferenceNumber = referenceNumberHiddenField.Value;

        CRRequestHistory[] historyList;

        try
        {
            historyList = svc.RetrieveCRRequestHistoryRecords(request);
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericWebServiceMessage);
        }

        foreach (CRRequestHistory history in historyList)
        {
            DataRow dr = dt.NewRow();
            dr["Status"] = history.Status.StatusName;
            dr["DateProcessed"] = history.DateProcessed;
            dr["ProcessedBy"] = history.ProcessedBy;
            dr["Remarks"] = history.Remarks;
            dt.Rows.Add(dr);
        }

        historyGridView.DataSource = dt;
        historyGridView.DataBind();

        historyDetailsDiv.Style.Add("display", "block");
        historyDetails.Show();
    }

    #endregion
}