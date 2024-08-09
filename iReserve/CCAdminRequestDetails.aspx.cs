using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iReserveWS;
using System.Data;
using System.IO;
using System.Web.Services.Protocols;

public partial class CCAdminRequestDetails : System.Web.UI.Page
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
      if (profileName != "Convention Center Administrator")
      {
        Response.Write("<script language=javascript> alert('You are not allowed to access this page. Please click on the Ok Button to go back to the Home Page.'); window.location.href ='Default.aspx';</script>");
      }
    }

    if (Convert.ToString(Session["CCReferenceNumber"]) == "")
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
    RetrieveCCRequestDetailsRequest retrieveCCRequestDetailsRequest = new RetrieveCCRequestDetailsRequest();
    retrieveCCRequestDetailsRequest.CCRequestReferenceNo = Convert.ToString(Session["CCReferenceNumber"]);

    RetrieveCCRequestDetailsResult retrieveCCRequestDetailsResult = svc.RetrieveCCRequestDetails(retrieveCCRequestDetailsRequest);

    if (retrieveCCRequestDetailsResult.ResultStatus != iReserveWS.ResultStatus.Successful)
    {
      Utilities.MyMessageBoxWithHomeRedirect(retrieveCCRequestDetailsResult.Message);
    }
    else
    {
      ccReferenceNumberLabel.Text = retrieveCCRequestDetailsResult.CCRequest.CCRequestReferenceNo;
      referenceNumberHiddenField.Value = retrieveCCRequestDetailsResult.CCRequest.CCRequestReferenceNo;
      eventNameLabel.Text = retrieveCCRequestDetailsResult.CCRequest.EventName;
      startDateLabel.Text = retrieveCCRequestDetailsResult.CCRequest.StartDate.ToString("MM/dd/yyyy");
      endDateLabel.Text = retrieveCCRequestDetailsResult.CCRequest.EndDate.ToString("MM/dd/yyyy");
      startDateHiddenField.Value = retrieveCCRequestDetailsResult.CCRequest.StartDate.ToString("MM/dd/yyyy");

      chargedCompanyLabel.Text = retrieveCCRequestDetailsResult.CCRequest.CostCenterName;
      string costCenter = retrieveCCRequestDetailsResult.CCRequest.ChargedCompanyCostCenter.CostCenterName;
      chargedCompanyCostCenterLbl.Text = costCenter != null ? costCenter : string.Empty;

        
      //if (retrieveCCRequestDetailsResult.CCRequest.CostCenterName != null && retrieveCCRequestDetailsResult.CCRequest.ChargedCompanyCostCenter.CostCenterName == null)
      //{
      //  costCenterLabel.Text = retrieveCCRequestDetailsResult.CCRequest.CostCenterName;
      //  rowChargedCompanyCostCenter.Visible = false;
      //  lblChargedCompany.Text = "Cost Center";
      //}
      //if (retrieveCCRequestDetailsResult.CCRequest.CostCenterName != null && retrieveCCRequestDetailsResult.CCRequest.ChargedCompanyCostCenter.CostCenterName != null)
      //{
      //  costCenterLabel.Text = retrieveCCRequestDetailsResult.CCRequest.CostCenterName;
      //  chargedCompanyCostCenterLbl.Text = retrieveCCRequestDetailsResult.CCRequest.ChargedCompanyCostCenter.CostCenterName;
      //  rowChargedCompanyCostCenter.Visible = true;
      //  lblChargedCompany.Text = "Company";
      //}


      requestorLabel.Text = retrieveCCRequestDetailsResult.CCRequest.CreatedBy;
      dateRequestedLabel.Text = retrieveCCRequestDetailsResult.CCRequest.DateCreated.ToString();
      statusLabel.Text = retrieveCCRequestDetailsResult.CCRequest.StatusName;
      statusCodeHiddenField.Value = retrieveCCRequestDetailsResult.CCRequest.StatusCode.ToString();
      soaStatusLabel.Text = retrieveCCRequestDetailsResult.CCRequest.SOAStatusName;
      soaStatusCodeHiddenField.Value = retrieveCCRequestDetailsResult.CCRequest.SOAStatusCode.ToString();

      attachmentGridView.DataSource = retrieveCCRequestDetailsResult.CCRequestAttachmentList;
      attachmentGridView.DataBind();

      trainingRoomGridView.DataSource = retrieveCCRequestDetailsResult.TrainingRoomRequestList;
      trainingRoomGridView.DataBind();

      if (trainingRoomGridView.Rows.Count == 0)
      {
        tRoomGridViewLabel.Visible = true;
      }

      accomodationRoomGridView.DataSource = retrieveCCRequestDetailsResult.AccomodationRoomRequest;
      accomodationRoomGridView.DataBind();

      if (accomodationRoomGridView.Rows.Count == 0)
      {
        accRoomGridviewLabel.Visible = true;
      }

      if (retrieveCCRequestDetailsResult.CCRequest.StatusCode == StatusCode.Confirmed)
      {
        requestorCancelRequestButton.Visible = true;

        if (retrieveCCRequestDetailsResult.CCRequest.StartDate <= DateTime.Now)
        {
          requestorCancelRequestButton.Enabled = true;
        }

        if (retrieveCCRequestDetailsResult.CCRequest.SOAStatusCode == SOAStatusCode.ForProcessing)
        {
          if (retrieveCCRequestDetailsResult.CCRequest.EndDate < DateTime.Now)
          {
            createSOAButton.Visible = true;
          }
        }
        else
        {
          viewSOAButton.Visible = true;
        }
      }
    }
  }

  protected void downloadLinkButton_Click(object sender, EventArgs e)
  {
    LinkButton link = (LinkButton)sender;
    GridViewRow gv = (GridViewRow)(link.Parent.Parent);

    RetrieveCCRequestAttachmentByStatusRequest retrieveCCRequestAttachmentByStatusRequest = new RetrieveCCRequestAttachmentByStatusRequest();
    retrieveCCRequestAttachmentByStatusRequest.CCRequestReferenceNo = referenceNumberHiddenField.Value;
    retrieveCCRequestAttachmentByStatusRequest.StatusCode = Convert.ToInt32(gv.Cells[3].Text);

    RetrieveCCRequestAttachmentByStatusResult retrieveCCRequestAttachmentByStatusResult = svc.RetrieveCCRequestAttachmentByStatus(retrieveCCRequestAttachmentByStatusRequest);

    if (retrieveCCRequestAttachmentByStatusResult.ResultStatus != iReserveWS.ResultStatus.Successful)
    {
      Utilities.MyMessageBox(retrieveCCRequestAttachmentByStatusResult.Message);
    }
    else
    {
      DownloadAttachment(retrieveCCRequestAttachmentByStatusResult.CCRequestAttachment.FileName,
          retrieveCCRequestAttachmentByStatusResult.CCRequestAttachment.FileType, retrieveCCRequestAttachmentByStatusResult.CCRequestAttachment.File);
    }
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

  #region Requestor Cancel Request

  protected void requestorCancelRequestButton_Click(object sender, EventArgs e)
  {
    submitDetailsDiv2.Style.Add("display", "block");
    submitDetails2.Show();
  }

  protected void cancelButton_Click(object sender, EventArgs e)
  {
    submitDetails2.Hide();
    ClearControls2();
  }

  private void ClearControls2()
  {
    remarksTextBox.Text = "";
  }

  protected void submitButton_Click(object sender, EventArgs e)
  {
    Submit2();
    ClearControls2();
  }

  public void Submit2()
  {
    string smessage = "";

    CancelCCRequestRequest cancelCCRequestRequest = new CancelCCRequestRequest();
    cancelCCRequestRequest.CCRequestReferenceNo = referenceNumberHiddenField.Value;
    cancelCCRequestRequest.StatusCode = StatusCode.Cancelled;



    CCRequestHistory ccRequestHistory = new CCRequestHistory();
    ccRequestHistory.CCRequestReferenceNumber = referenceNumberHiddenField.Value;
    ccRequestHistory.StatusCode = StatusCode.Cancelled;
    ccRequestHistory.ProcessedByID = Session["UserID"].ToString();
    ccRequestHistory.ProcessedBy = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();
    ccRequestHistory.Remarks = remarksTextBox.Text.Trim();
    cancelCCRequestRequest.CCRequestHistory = ccRequestHistory;

    DateTime startDate = Convert.ToDateTime(startDateHiddenField.Value);
    int daysBeforeTheEvent = (startDate - DateTime.Now).Days;

    if (daysBeforeTheEvent <= 3)
    {
      CancellationFee fee = new CancellationFee();
      fee = svc.RetrieveCancellationFee();

      cancelCCRequestRequest.CancellationFee = new CancellationFee();
      cancelCCRequestRequest.CancellationFee.CancellationID = fee.CancellationID;
    }



    CancelCCRequestResult cancelCCRequestResult = svc.CancelCCRequest(cancelCCRequestRequest);


    if (cancelCCRequestResult.ResultStatus != iReserveWS.ResultStatus.Successful)
    {
      smessage = cancelCCRequestResult.Message;
    }
    else
    {
      smessage = "Reservation request with Reference Number: " + referenceNumberHiddenField.Value + " has been successfully cancelled.";

      #region Audit Trail

      bool isSuccess = false;

      AuditTrail auditTrail = new AuditTrail();
      auditTrail.ActionDate = DateTime.Now;
      auditTrail.ActionTaken = "Cancel convention center request";
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
    }

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
    soaHistoryGridView.DataSource = null;
    soaHistoryGridView.DataBind();

    RetrieveCCRequestHistoryRecordsRequest retrieveCCRequestHistoryRecordsRequest = new RetrieveCCRequestHistoryRecordsRequest();
    retrieveCCRequestHistoryRecordsRequest.CCRequestReferenceNo = referenceNumberHiddenField.Value;

    RetrieveCCRequestHistoryRecordsResult retrieveCCRequestHistoryRecordsResult = svc.RetrieveCCRequestHistoryRecords(retrieveCCRequestHistoryRecordsRequest);

    if (retrieveCCRequestHistoryRecordsResult.ResultStatus != iReserveWS.ResultStatus.Successful)
    {
      Utilities.MyMessageBox(retrieveCCRequestHistoryRecordsResult.Message);
    }
    else
    {
      historyGridView.DataSource = retrieveCCRequestHistoryRecordsResult.CCRequestHistoryList;
      historyGridView.DataBind();

      historyDetailsDiv.Style.Add("display", "block");
      historyDetails.Show();
    }
  }

  protected void viewSOAHistoryLinkButton_Click(object sender, EventArgs e)
  {
    historyGridView.DataSource = null;
    historyGridView.DataBind();

    RetrieveSOAHistoryRecordsRequest retrieveSOAHistoryRecordsRequest = new RetrieveSOAHistoryRecordsRequest();
    retrieveSOAHistoryRecordsRequest.CCRequestReferenceNo = referenceNumberHiddenField.Value;

    RetrieveSOAHistoryRecordsResult retrieveSOAHistoryRecordsResult = svc.RetrieveSOAHistoryRecords(retrieveSOAHistoryRecordsRequest);

    if (retrieveSOAHistoryRecordsResult.ResultStatus != iReserveWS.ResultStatus.Successful)
    {
      Utilities.MyMessageBox(retrieveSOAHistoryRecordsResult.Message);
    }
    else
    {
      soaHistoryGridView.DataSource = retrieveSOAHistoryRecordsResult.SOAHistoryList;
      soaHistoryGridView.DataBind();

      historyDetailsDiv.Style.Add("display", "block");
      historyDetails.Show();
    }
  }

  #endregion

  #region SOA Processing

  protected void createSOAButton_Click(object sender, EventArgs e)
  {
    Response.Redirect("CCSOADetails.aspx");
  }

  protected void viewSOAButton_Click(object sender, EventArgs e)
  {
    Response.Redirect("CCSOADetails.aspx");
  }

  #endregion
}