using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iReserveWS;
using System.Web.Services.Protocols;

public partial class CCSOADetails : System.Web.UI.Page
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

        LoadSOADetails();
    }

    public void LoadSOADetails()
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
            ccReferenceNumberLabel.Text = retrieveSOADetailsResult.CCRequest.CCRequestReferenceNo;
            referenceNumberHiddenField.Value = retrieveSOADetailsResult.CCRequest.CCRequestReferenceNo;
            eventNameLabel.Text = retrieveSOADetailsResult.CCRequest.EventName;
            startDateLabel.Text = retrieveSOADetailsResult.CCRequest.StartDate.ToString("MMM d, yyyy");
            endDateLabel.Text = retrieveSOADetailsResult.CCRequest.EndDate.ToString("MMM d, yyyy");
            costCenterLabel.Text = retrieveSOADetailsResult.CCRequest.CostCenterName;
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

            totalLabel.Text = "PHP " + (trainingRoomSum + accRoomSum + otherChargesSum).ToString("#,###.00");

            if (retrieveSOADetailsResult.CCRequest.SOAStatusCode == SOAStatusCode.ForProcessing || retrieveSOADetailsResult.CCRequest.SOAStatusCode == SOAStatusCode.Disapproved)
            {
                submitSOAButton.Visible = true;
            }
            else
            {
                otherChargesAddButton.Visible = false;
                otherChargesGridView.Columns[5].Visible = false;
            }

            if (retrieveSOADetailsResult.CCRequest.SOAStatusCode == SOAStatusCode.Approved)
            {
                sendSOAButton.Visible = true;
            }
        }
    }

    protected void otherChargesAddButton_Click(object sender, EventArgs e)
    {
        addDetailsDiv.Style.Add("display", "block");
        addDetails.Show();
    }

    protected void cancelButton_Click(object sender, EventArgs e)
    {
        addDetails.Hide();
        ClearControls();
    }

    private void ClearControls()
    {
        particularsTextBox.Text = "";
        amountChargeNumericBox.Text = "";
        remarksTextBox.Text = "";
    }

    protected void otherChargesGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        OtherCharge otherCharge = new OtherCharge();
        otherCharge.CCRequestReferenceNo = referenceNumberHiddenField.Value;
        otherCharge.OtherChargeID = Convert.ToInt32(otherChargesGridView.DataKeys[e.RowIndex].Value);

        OtherChargeTransactionRequest otherChargeTransactionRequest = new OtherChargeTransactionRequest();
        otherChargeTransactionRequest.Type = TranCode.Delete;
        otherChargeTransactionRequest.OtherCharge = otherCharge;

        OtherChargeTransactionResult otherChargeTransactionResult = new OtherChargeTransactionResult();
        otherChargeTransactionResult = svc.OtherChargeTransaction(otherChargeTransactionRequest);

        if (otherChargeTransactionResult.ResultStatus != iReserveWS.ResultStatus.Successful)
        {
            Utilities.MyMessageBoxWithHomeRedirect(otherChargeTransactionResult.Message);
        }
        else
        {
            #region Audit Trail

            //bool isSuccess = false;

            //AuditTrail auditTrail = new AuditTrail();
            //auditTrail.ActionDate = DateTime.Now;
            //auditTrail.ActionTaken = "Delete other charges";
            //auditTrail.ActionDetails = "Reference Number: " + referenceNumberHiddenField.Value + " || Other Charge ID: " + otherCharge.OtherChargeID;
            //auditTrail.Browser = HttpContext.Current.Request.Browser.Browser;
            //auditTrail.BrowserVersion = HttpContext.Current.Request.Browser.Version;
            //auditTrail.IpAddress = HttpContext.Current.Request.ServerVariables[32];
            //System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();

            //auditTrail.MacAdress = Session["MacAddress"].ToString();
            //auditTrail.UserID = Session["UserID"].ToString();

            //try
            //{
            //    isSuccess = svc.InsertAuditTrailEntry(auditTrail);
            //}

            //catch (SoapException ex)
            //{
            //    throw new Exception(Settings.GenericAuditTrailMessage);
            //}

            #endregion
        }

        LoadSOADetails();
    }

    protected void addButton_Click(object sender, EventArgs e)
    {
        AddOtherCharge();
        LoadSOADetails();
        ClearControls();
    }

    private void AddOtherCharge()
    {
        OtherCharge otherCharge = new OtherCharge();
        otherCharge.CCRequestReferenceNo = referenceNumberHiddenField.Value;
        otherCharge.Particulars = particularsTextBox.Text;
        otherCharge.AmountCharge = amountChargeNumericBox.Text;
        otherCharge.Remarks = remarksTextBox.Text;

        OtherChargeTransactionRequest otherChargeTransactionRequest = new OtherChargeTransactionRequest();
        otherChargeTransactionRequest.Type = TranCode.Insert;
        otherChargeTransactionRequest.OtherCharge = otherCharge;

        OtherChargeTransactionResult otherChargeTransactionResult = new OtherChargeTransactionResult();
        otherChargeTransactionResult = svc.OtherChargeTransaction(otherChargeTransactionRequest);

        if (otherChargeTransactionResult.ResultStatus != iReserveWS.ResultStatus.Successful)
        {
            Utilities.MyMessageBoxWithHomeRedirect(otherChargeTransactionResult.Message);
        }
        else
        {
            #region Audit Trail

            //bool isSuccess = false;

            //AuditTrail auditTrail = new AuditTrail();
            //auditTrail.ActionDate = DateTime.Now;
            //auditTrail.ActionTaken = "Add other charges";
            //auditTrail.ActionDetails = "Reference Number: " + referenceNumberHiddenField.Value + " || Other Charge Particulars: " + particularsTextBox.Text;
            //auditTrail.Browser = HttpContext.Current.Request.Browser.Browser;
            //auditTrail.BrowserVersion = HttpContext.Current.Request.Browser.Version;
            //auditTrail.IpAddress = HttpContext.Current.Request.ServerVariables[32];
            //System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();

            //auditTrail.MacAdress = Session["MacAddress"].ToString();
            //auditTrail.UserID = Session["UserID"].ToString();

            //try
            //{
            //    isSuccess = svc.InsertAuditTrailEntry(auditTrail);
            //}

            //catch (SoapException ex)
            //{
            //    throw new Exception(Settings.GenericAuditTrailMessage);
            //}

            #endregion
        }
    }

    protected void submitSOAButton_Click(object sender, EventArgs e)
    {
        submitDetailsDiv.Style.Add("display", "block");
        submitDetails.Show();
    }

    protected void cancelSubmitButton_Click(object sender, EventArgs e)
    {
        submitDetails.Hide();
        ClearSubmitControls();
    }

    private void ClearSubmitControls()
    {
        submitRemarksTextBox.Text = "";
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        Submit();
        ClearSubmitControls();
    }

    private void Submit()
    {
        string smessage = "";

        UpdateSOAStatusRequest updateSOAStatusRequest = new UpdateSOAStatusRequest();
        updateSOAStatusRequest.CCRequestReferenceNo = referenceNumberHiddenField.Value;
        updateSOAStatusRequest.SOAStatusCode = SOAStatusCode.ForApproval;

        SOAHistory soaHistory = new SOAHistory();
        soaHistory.CCRequestReferenceNumber = referenceNumberHiddenField.Value;
        soaHistory.SOAStatusCode = SOAStatusCode.ForApproval;
        soaHistory.ProcessedByID = Session["UserID"].ToString();
        soaHistory.ProcessedBy = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();
        soaHistory.Remarks = submitRemarksTextBox.Text.Trim();
        updateSOAStatusRequest.SOAHistory = soaHistory;

        UpdateSOAStatusResult updateSOAStatusResult = svc.UpdateSOAStatus(updateSOAStatusRequest);

        if (updateSOAStatusResult.ResultStatus != iReserveWS.ResultStatus.Successful)
        {
            smessage = updateSOAStatusResult.Message;
        }
        else
        {
            smessage = "The statement of account for reservation request with Reference Number: " + referenceNumberHiddenField.Value + " has been successfully submitted.";
        }

        #region Audit Trail

        //bool isSuccess = false;

        //AuditTrail auditTrail = new AuditTrail();
        //auditTrail.ActionDate = DateTime.Now;
        //auditTrail.ActionTaken = "Submit request SOA";
        //auditTrail.ActionDetails = "Reference Number: " + referenceNumberHiddenField.Value + " || SOA Status: For Approval";
        //auditTrail.Browser = HttpContext.Current.Request.Browser.Browser;
        //auditTrail.BrowserVersion = HttpContext.Current.Request.Browser.Version;
        //auditTrail.IpAddress = HttpContext.Current.Request.ServerVariables[32];
        //System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();

        //auditTrail.MacAdress = Session["MacAddress"].ToString();
        //auditTrail.UserID = Session["UserID"].ToString();

        //try
        //{
        //    isSuccess = svc.InsertAuditTrailEntry(auditTrail);
        //}

        //catch (SoapException ex)
        //{
        //    throw new Exception(Settings.GenericAuditTrailMessage);
        //}

        #endregion

        Utilities.MyMessageBoxWithHomeRedirect(smessage);
    }

    protected void sendSOAButton_Click(object sender, EventArgs e)
    {
        string smessage = "";

        UpdateSOAStatusRequest updateSOAStatusRequest = new UpdateSOAStatusRequest();
        updateSOAStatusRequest.CCRequestReferenceNo = referenceNumberHiddenField.Value;
        updateSOAStatusRequest.SOAStatusCode = SOAStatusCode.Completed;

        SOAHistory soaHistory = new SOAHistory();
        soaHistory.CCRequestReferenceNumber = referenceNumberHiddenField.Value;
        soaHistory.SOAStatusCode = SOAStatusCode.Completed;
        soaHistory.ProcessedByID = Session["UserID"].ToString();
        soaHistory.ProcessedBy = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();
        soaHistory.Remarks = "SOA sent to requestor";
        updateSOAStatusRequest.SOAHistory = soaHistory;

        UpdateSOAStatusResult updateSOAStatusResult = svc.UpdateSOAStatus(updateSOAStatusRequest);

        if (updateSOAStatusResult.ResultStatus != iReserveWS.ResultStatus.Successful)
        {
            smessage = updateSOAStatusResult.Message;
        }
        else
        {
            smessage = "The statement of account for reservation request with Reference Number: " + referenceNumberHiddenField.Value + " has been successfully sent to requestor.";
        }

        #region Audit Trail

        //bool isSuccess = false;

        //AuditTrail auditTrail = new AuditTrail();
        //auditTrail.ActionDate = DateTime.Now;
        //auditTrail.ActionTaken = "Send SOA";
        //auditTrail.ActionDetails = "Reference Number: " + referenceNumberHiddenField.Value + " || SOA Status: Completed";
        //auditTrail.Browser = HttpContext.Current.Request.Browser.Browser;
        //auditTrail.BrowserVersion = HttpContext.Current.Request.Browser.Version;
        //auditTrail.IpAddress = HttpContext.Current.Request.ServerVariables[32];
        //System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();

        //auditTrail.MacAdress = Session["MacAddress"].ToString();
        //auditTrail.UserID = Session["UserID"].ToString();

        //try
        //{
        //    isSuccess = svc.InsertAuditTrailEntry(auditTrail);
        //}

        //catch (SoapException ex)
        //{
        //    throw new Exception(Settings.GenericAuditTrailMessage);
        //}

        #endregion

        Utilities.MyMessageBoxWithHomeRedirect(smessage);
    }
}