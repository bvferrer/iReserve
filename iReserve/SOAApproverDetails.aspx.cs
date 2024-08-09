using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iReserveWS;
using System.Web.Services.Protocols;

public partial class SOAApproverDetails : System.Web.UI.Page
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
      if (profileName != "SOA Approver")
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

    LoadSOADetails();
  }

  private void LoadSOADetails()
  {
    decimal trainingRoomSum = 0, accRoomSum = 0, otherChargesSum = 0;

    RetrieveSOADetailsRequest retrieveSOADetailsRequest = new RetrieveSOADetailsRequest();
    retrieveSOADetailsRequest.CCRequestReferenceNo = Convert.ToString(Session["CCReferenceNumber"]);

    RetrieveSOADetailsResult retrieveSOADetailsResult = svc.RetrieveSOADetails(retrieveSOADetailsRequest);

    if (retrieveSOADetailsResult.ResultStatus != iReserveWS.ResultStatus.Successful)
    {
      Utilities.MyMessageBoxWithHomeRedirect(retrieveSOADetailsResult.Message);
    }
    else
    {

      string costCenter = retrieveSOADetailsResult.CCRequest.CostCenterName;
      string chargedCompanyCostCenter = retrieveSOADetailsResult.CCRequest.ChargedCompanyCostCenter.CostCenterName;
      ccReferenceNumberLabel.Text = retrieveSOADetailsResult.CCRequest.CCRequestReferenceNo;
      referenceNumberHiddenField.Value = retrieveSOADetailsResult.CCRequest.CCRequestReferenceNo;
      eventNameLabel.Text = retrieveSOADetailsResult.CCRequest.EventName;
      startDateLabel.Text = retrieveSOADetailsResult.CCRequest.StartDate.ToString("MMM d, yyyy");
      endDateLabel.Text = retrieveSOADetailsResult.CCRequest.EndDate.ToString("MMM d, yyyy");
      chargedCompanyLabel.Text = costCenter;
      costCenterLabel.Text = chargedCompanyCostCenter != null ? chargedCompanyCostCenter : string.Empty;
      requestorLabel.Text = retrieveSOADetailsResult.CCRequest.CreatedBy;
      dateRequestedLabel.Text = retrieveSOADetailsResult.CCRequest.DateCreated.ToString("MMM d, yyyy hh:mm tt");

      if (retrieveSOADetailsResult.TrainingRoomRequestChargeList != null)
      {
        if (retrieveSOADetailsResult.TrainingRoomRequestChargeList.Length == 0)
        {
          tRoomGridViewLabel.Visible = true;
        }
        else
        {
          trainingRoomGridView.DataSource = retrieveSOADetailsResult.TrainingRoomRequestChargeList;
          trainingRoomGridView.DataBind();

          foreach (TrainingRoomRequestCharge trCharge in retrieveSOADetailsResult.TrainingRoomRequestChargeList)
          {
            trainingRoomSum += Convert.ToDecimal(trCharge.AmountCharge);
          }
        }
      }

      if (retrieveSOADetailsResult.AccomodationRoomRequestList != null)
      {
        if (retrieveSOADetailsResult.AccomodationRoomRequestList.Length == 0)
        {
          accRoomGridviewLabel.Visible = true;
        }
        else
        {
          accomodationRoomGridView.DataSource = retrieveSOADetailsResult.AccomodationRoomRequestList;
          accomodationRoomGridView.DataBind();

          foreach (AccomodationRoomRequest arRequest in retrieveSOADetailsResult.AccomodationRoomRequestList)
          {
            accRoomSum += Convert.ToDecimal(arRequest.AmountCharge);
          }
        }
      }

      if (retrieveSOADetailsResult.OtherChargeList != null)
      {
        if (retrieveSOADetailsResult.OtherChargeList.Length != 0)
        {
          otherChargesGridView.DataSource = retrieveSOADetailsResult.OtherChargeList;
          otherChargesGridView.DataBind();

          foreach (OtherCharge otherCharge in retrieveSOADetailsResult.OtherChargeList)
          {
            otherChargesSum += Convert.ToDecimal(otherCharge.AmountCharge);
          }
        }
      }

      double totalAmountDouble = Convert.ToDouble(trainingRoomSum + accRoomSum + otherChargesSum);
      double discountedPrice = totalAmountDouble * (retrieveSOADetailsResult.CCRequest.PercentDiscount / 100);
      double totalAmountPayable = totalAmountDouble - discountedPrice;
      txtPercentDiscount.Text = retrieveSOADetailsResult.CCRequest.PercentDiscount.ToString();
      totalLabel.Text = "PHP " + totalAmountPayable.ToString("#,###.00");

      if (retrieveSOADetailsResult.CCRequest.SOAStatusCode == SOAStatusCode.ForApproval)
      {
        remarksLabel.Visible = true;
        remarksPanel.Visible = true;
        approveButton.Visible = true;
        disapproveButton.Visible = true;
      }
    }
  }

  protected void approveButton_Click(object sender, EventArgs e)
  {
    remarksValidationLabel.Text = "";
    Process(SOAStatusCode.Approved);
  }

  protected void disapproveButton_Click(object sender, EventArgs e)
  {
    if (remarksTextBox.Text.Trim() == "")
    {
      remarksValidationLabel.Text = "Remarks is required to disapprove.";
    }
    else
    {
      Process(SOAStatusCode.Disapproved);
      remarksValidationLabel.Text = "";
    }
  }

  private void Process(int soaStatusCode)
  {
    string smessage = "";

    UpdateSOAStatusRequest updateSOAStatusRequest = new UpdateSOAStatusRequest();
    updateSOAStatusRequest.CCRequestReferenceNo = referenceNumberHiddenField.Value;
    updateSOAStatusRequest.SOAStatusCode = soaStatusCode;

    SOAHistory soaHistory = new SOAHistory();
    soaHistory.CCRequestReferenceNumber = referenceNumberHiddenField.Value;
    soaHistory.SOAStatusCode = soaStatusCode;
    soaHistory.ProcessedByID = Session["UserID"].ToString();
    soaHistory.ProcessedBy = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();
    soaHistory.Remarks = remarksTextBox.Text.Trim();
    updateSOAStatusRequest.SOAHistory = soaHistory;

    UpdateSOAStatusResult updateSOAStatusResult = svc.UpdateSOAStatus(updateSOAStatusRequest);

    if (updateSOAStatusResult.ResultStatus != iReserveWS.ResultStatus.Successful)
    {
      smessage = updateSOAStatusResult.Message;
    }
    else
    {
      if (soaStatusCode == SOAStatusCode.Approved)
      {
        smessage = "The statement of account for reservation request with Reference Number: " + referenceNumberHiddenField.Value + " has been successfully approved.";

        #region Audit Trail

        bool isSuccess = false;

        AuditTrail auditTrail = new AuditTrail();
        auditTrail.ActionDate = DateTime.Now;
        auditTrail.ActionTaken = "Approve SOA";
        auditTrail.ActionDetails = "Reference Number: " + referenceNumberHiddenField.Value + " || SOA Status: Approved";
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
      else if (soaStatusCode == SOAStatusCode.Disapproved)
      {
        smessage = "The statement of account for reservation request with Reference Number: " + referenceNumberHiddenField.Value + " has been successfully disapproved.";

        #region Audit Trail

        bool isSuccess = false;

        AuditTrail auditTrail = new AuditTrail();
        auditTrail.ActionDate = DateTime.Now;
        auditTrail.ActionTaken = "Disapprove SOA";
        auditTrail.ActionDetails = "Reference Number: " + referenceNumberHiddenField.Value + " || SOA Status: Disapproved";
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
    }

    Utilities.MyMessageBoxWithHomeRedirect(smessage);
  }
}