using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iReserveWS;
using System.Data;
using System.IO;
using System.Web.Services.Protocols;

public partial class ConfirmRequestDetails : System.Web.UI.Page
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
      if (profileName != "Conference Room Administrator")
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
    requestorLabel.Text = result.RequestedBy;

    //if (result.CostCenter.CostCenterName != null && result.ChargedCompanyCostCenter.CostCenterName == null)
    //{
    //  costCenterLabel.Text = result.CostCenter.CostCenterName;
    //  rowChargedCompanyCostCenter.Visible = false;
    //  lblChargedCompany.Text = "Cost Center";
    //}
    //if(result.CostCenter.CostCenterName != null && result.ChargedCompanyCostCenter.CostCenterName != null)
    //{
    //  costCenterLabel.Text = result.CostCenter.CostCenterName;
    //  chargedCompanyCostCenterLbl.Text = result.ChargedCompanyCostCenter.CostCenterName;
    //  rowChargedCompanyCostCenter.Visible = true;
    //  lblChargedCompany.Text = "Company";
    //}

    string chargedCompanyCostCenter = result.ChargedCompanyCostCenter.CostCenterName != null ? result.ChargedCompanyCostCenter.CostCenterName : string.Empty;
    chargedCompanyLbl.Text = result.CostCenter.CostCenterName;
    chargedCompanyCostCenterLbl.Text = chargedCompanyCostCenter;

    dateRequestedLabel.Text = result.DateRequested.ToString();
    agendaLabel.Text = result.Agenda;
    dataPortLabel.Visible = Convert.ToBoolean(result.IsUseDataPort);
    monitorLabel.Visible = Convert.ToBoolean(result.IsUseMonitor);
    statusLabel.Text = result.Status.StatusName;

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

  protected void declineButton_Click(object sender, EventArgs e)
  {
    typeLabel.Text = "Decline";
    statusCodeHiddenField.Value = StatusCode.Declined.ToString();

    submitDetailsDiv.Style.Add("display", "block");
    submitDetails.Show();
  }

  protected void confirmButton_Click(object sender, EventArgs e)
  {
    typeLabel.Text = "Confirm";
    statusCodeHiddenField.Value = StatusCode.Confirmed.ToString();

    submitDetailsDiv.Style.Add("display", "block");
    submitDetails.Show();
  }

  protected void cancelButton_Click(object sender, EventArgs e)
  {
    submitDetails.Hide();
    ClearControls();
  }

  private void ClearControls()
  {
    remarksTextBox.Text = "";
    statusCodeHiddenField.Value = "";
    scheduleValidationLabel.Text = "";
  }

  protected void submitButton_Click(object sender, EventArgs e)
  {
    CRRequest request = new CRRequest();
    request.RequestReferenceNo = referenceNumberHiddenField.Value;
    request.Status = new Status();
    request.Status.StatusCode = Convert.ToInt32(statusCodeHiddenField.Value);

    CRRequestHistory history = new CRRequestHistory();
    history.RequestReferenceNumber = referenceNumberHiddenField.Value;
    history.Status = new Status();
    history.Status.StatusCode = Convert.ToInt32(statusCodeHiddenField.Value);
    history.ProcessedByID = Session["UserID"].ToString();
    history.ProcessedBy = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();
    history.Remarks = remarksTextBox.Text.Trim();
    request.RequestHistory = history;

    if (Convert.ToInt32(statusCodeHiddenField.Value) == StatusCode.Confirmed)
    {
      Confirm(request);
    }
    else if (Convert.ToInt32(statusCodeHiddenField.Value) == StatusCode.Declined)
    {
      Decline(request);
    }
  }

  private void Confirm(CRRequest request)
  {
    CRRequest validateAvailability = new CRRequest();

    try
    {
      validateAvailability = svc.RetrieveCRRequestDetails(request);
    }

    catch (SoapException ex)
    {
      throw new Exception(Settings.GenericWebServiceMessage);
    }

    if (!svc.ValidateScheduleAvailability(validateAvailability))
    {
      string smessage = "The selected schedule has a conflict with another reservation. Kindly check Reservation Calendar Page.";
      scheduleValidationLabel.Text = smessage;
      submitDetails.Show();
    }
    else
    {
      scheduleValidationLabel.Text = "";
      string smessage = "";

      Result result = svc.UpdateReservationRequest(request);

      if (result.ResultStatus == iReserveWS.ResultStatus.Successful)
      {
        smessage = "The reservation request with Reference Number: " + referenceNumberHiddenField.Value + " has been successfully confirmed.";

        #region Audit Trail

        bool isSuccess = false;

        AuditTrail auditTrail = new AuditTrail();
        auditTrail.ActionDate = DateTime.Now;
        auditTrail.ActionTaken = "Confirmed the request";
        auditTrail.ActionDetails = "Reference Number: " + referenceNumberHiddenField.Value + " || Status: Confirmed";
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
      }
      else if (result.ResultStatus == iReserveWS.ResultStatus.Error)
      {
        smessage = result.Message;
      }

      Utilities.MyMessageBoxWithHomeRedirect(smessage);

      ClearControls();
    }
  }

  private void Decline(CRRequest request)
  {
    string smessage = "";

    Result result = svc.UpdateReservationRequest(request);

    if (result.ResultStatus == iReserveWS.ResultStatus.Successful)
    {
      smessage = "The reservation request with Reference Number: " + referenceNumberHiddenField.Value + " has been successfully declined.";

      #region Audit Trail

      bool isSuccess = false;

      AuditTrail auditTrail = new AuditTrail();
      auditTrail.ActionDate = DateTime.Now;
      auditTrail.ActionTaken = "Declined the request";
      auditTrail.ActionDetails = "Reference Number: " + referenceNumberHiddenField.Value + " || Status: Declined";
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
    }
    else if (result.ResultStatus == iReserveWS.ResultStatus.Error)
    {
      smessage = result.Message;
    }

    Utilities.MyMessageBoxWithHomeRedirect(smessage);

    ClearControls();
  }

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
}