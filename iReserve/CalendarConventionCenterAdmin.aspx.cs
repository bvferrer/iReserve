using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using iReserveWS;
using System.Web.Services.Protocols;

public partial class CalendarConventionCenterAdmin : System.Web.UI.Page
{
    iReserveWS.Service svc = new iReserveWS.Service();

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

        if (!IsPostBack)
        {
            LoadControls();
        }
    }

    private void LoadControls()
    {
        datepicker.Text = DateTime.Now.ToString("MM/dd/yyyy");
        BindCalendarHeader(datepicker.Text);
        BindCalendarGridView();
    }

    private void BindCalendarHeader(string date)
    {
        string strDates = CalendarUtilities.GetDateRange(date);
        string[] arrDates = strDates.Split(',');

        Session["previous20"] = Convert.ToDateTime(arrDates[0]).AddDays(-20).ToString("MM/dd/yyyy");
        Session["next20"] = Convert.ToDateTime(arrDates[arrDates.Length - 1]).AddDays(1).ToString("MM/dd/yyyy");

        CalendarUtilities.LoadTRInventoryHeader(date, arrDates[arrDates.Length - 1].ToString(), ccCalendarHeaderTable);
        CalendarUtilities.LoadARInventoryHeader(date, arrDates[arrDates.Length - 1].ToString(), accCalendarHeaderTable);
    }

    private void BindCalendarGridView()
    {
        string dateFrom = datepicker.Text;

        dateFrom = string.IsNullOrEmpty(dateFrom) ? DateTime.Now.ToString("MM/dd/yyyy") : dateFrom;

        RetrieveTRCalendarScheduleRequest retrieveTRCalendarScheduleRequest = new RetrieveTRCalendarScheduleRequest();
        retrieveTRCalendarScheduleRequest.DateFrom = dateFrom;

        RetrieveTRCalendarScheduleResult retrieveTRCalendarScheduleResult = new RetrieveTRCalendarScheduleResult();
        retrieveTRCalendarScheduleResult = svc.RetrieveTRCalendarSchedule(retrieveTRCalendarScheduleRequest);

        if (retrieveTRCalendarScheduleResult.ResultStatus != iReserveWS.ResultStatus.Successful)
        {
            Utilities.MyMessageBox(retrieveTRCalendarScheduleResult.Message);
        }
        else
        {
            trainingRoomScheduleGridview.DataSource = retrieveTRCalendarScheduleResult.TRScheduleDataTable;
            trainingRoomScheduleGridview.DataBind();
        }

        RetrieveAccRoomCalendarScheduleRequest retrieveAccRoomCalendarScheduleRequest = new RetrieveAccRoomCalendarScheduleRequest();
        retrieveAccRoomCalendarScheduleRequest.DateFrom = dateFrom;

        RetrieveAccRoomCalendarScheduleResult retrieveAccRoomCalendarScheduleResult = new RetrieveAccRoomCalendarScheduleResult();
        retrieveAccRoomCalendarScheduleResult = svc.RetrieveAccRoomCalendarSchedule(retrieveAccRoomCalendarScheduleRequest);

        if (retrieveAccRoomCalendarScheduleResult.ResultStatus != iReserveWS.ResultStatus.Successful)
        {
            Utilities.MyMessageBox(retrieveAccRoomCalendarScheduleResult.Message);
        }
        else
        {
            accomodationRoomScheduleGridView.DataSource = retrieveAccRoomCalendarScheduleResult.AccRoomScheduleDataTable;
            accomodationRoomScheduleGridView.DataBind();
        }
    }

    protected void previousLinkButton_Click(object sender, EventArgs e)
    {
        datepicker.Text = Convert.ToDateTime(Session["previous20"]).ToString("MM/dd/yyyy");
        BindCalendarHeader(datepicker.Text);
        BindCalendarGridView();
    }

    protected void nextLinkButton_Click(object sender, EventArgs e)
    {
        datepicker.Text = Convert.ToDateTime(Session["next20"].ToString()).ToString("MM/dd/yyyy");
        BindCalendarHeader(datepicker.Text);
        BindCalendarGridView();
    }

    protected void datepicker_TextChanged(object sender, EventArgs e)
    {
        BindCalendarHeader(datepicker.Text);
        BindCalendarGridView();
    }

    protected void trainingRoomScheduleGridview_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string strDates = CalendarUtilities.GetDateRange(datepicker.Text);
            string[] arrDates = strDates.Split(',');

            string onmouseoverStyle = "this.style.backgroundColor='#FFFED1'";
            string onmouseoutStyle = "this.style.backgroundColor='white'";

            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[2].CssClass = "trainingRoomCell";
            e.Row.Cells[3].CssClass = "partitionRoomCell";

            for (int columnIndex = 4; columnIndex < e.Row.Cells.Count; columnIndex++)
            {
                e.Row.Cells[columnIndex].CssClass = "scheduleCell";

                if (e.Row.Cells[columnIndex].Text == "&nbsp;" || e.Row.Cells[columnIndex].Text == "")
                {
                    e.Row.Cells[columnIndex].CssClass = "noSchedule";
                    e.Row.Cells[columnIndex].Text = "";

                    if ((Convert.ToDateTime(arrDates[columnIndex - 4]) > Convert.ToDateTime(DateTime.Now.ToShortDateString())) && (Convert.ToDateTime(arrDates[columnIndex - 4]) < Convert.ToDateTime(DateTime.Now.AddMonths(6).ToShortDateString())))
                    {
                        e.Row.Cells[columnIndex].Attributes["style"] += "cursor:pointer;cursor:hand;";
                        e.Row.Cells[columnIndex].Attributes.Add("onclick", String.Format("trTranCellClicked({0},{1},{2},'{3}','{4}');", TranCode.Insert, e.Row.RowIndex, columnIndex, e.Row.Cells[columnIndex].Text, e.Row.Cells[2].Text + " " + e.Row.Cells[3].Text));
                        e.Row.Cells[columnIndex].Attributes.Add("onmouseover", onmouseoverStyle);
                        e.Row.Cells[columnIndex].Attributes.Add("onmouseout", onmouseoutStyle);
                    }
                    else
                    {
                        e.Row.Cells[columnIndex].CssClass = "pastdate";
                    }
                }
                else if (e.Row.Cells[columnIndex].Text.Substring(0,1) == "x")
                {
                    e.Row.Cells[columnIndex].Attributes["style"] += "cursor:pointer;cursor:hand;";
                    e.Row.Cells[columnIndex].Attributes.Add("onclick", String.Format("trTranCellClicked({0},{1},{2},'{3}','{4}');", TranCode.Delete, e.Row.RowIndex, columnIndex, e.Row.Cells[columnIndex].Text, e.Row.Cells[2].Text + " " + e.Row.Cells[3].Text));

                    e.Row.Cells[columnIndex].CssClass = "blocked";
                    e.Row.Cells[columnIndex].Text = "";
                }
                else
                {
                    e.Row.Cells[columnIndex].Attributes["style"] += "cursor:pointer;cursor:hand;";
                    e.Row.Cells[columnIndex].Attributes.Add("onclick", String.Format("cellClicked('{0}');", e.Row.Cells[columnIndex].Text));

                    e.Row.Cells[columnIndex].CssClass = "withSchedule";
                    e.Row.Cells[columnIndex].Text = "";
                }
            }
        }
    }

    protected void trainingRoomScheduleGridview_DataBound(object sender, EventArgs e)
    {
        for (int i = trainingRoomScheduleGridview.Rows.Count - 1; i > 0; i--)
        {
            GridViewRow row = trainingRoomScheduleGridview.Rows[i];
            GridViewRow previousRow = trainingRoomScheduleGridview.Rows[i - 1];
            int j = 2;

            if (row.Cells[j].Text != "&nbsp;" || row.Cells[j].Text == "")
            {
                if (row.Cells[j].Text == previousRow.Cells[j].Text)
                {
                    if (previousRow.Cells[j].RowSpan == 0)
                    {
                        if (row.Cells[j].RowSpan == 0)
                        {
                            previousRow.Cells[j].RowSpan += 2;
                        }
                        else
                        {
                            previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                        }
                        row.Cells[j].Visible = false;
                    }
                }
            }
        }
    }

    protected void accomodationRoomScheduleGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string strDates = CalendarUtilities.GetDateRange(datepicker.Text);
            string[] arrDates = strDates.Split(',');

            string onmouseoverStyle = "this.style.backgroundColor='#FFFED1'";
            string onmouseoutStyle = "this.style.backgroundColor='white'";

            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].CssClass = "accomodationRoomCell";

            for (int columnIndex = 2; columnIndex < e.Row.Cells.Count; columnIndex++)
            {
                e.Row.Cells[columnIndex].CssClass = "scheduleCell";

                if (e.Row.Cells[columnIndex].Text == "&nbsp;" || e.Row.Cells[columnIndex].Text == "")
                {
                    e.Row.Cells[columnIndex].CssClass = "noSchedule";
                    e.Row.Cells[columnIndex].Text = "";

                    if ((Convert.ToDateTime(arrDates[columnIndex - 2]) > Convert.ToDateTime(DateTime.Now.ToShortDateString())) && (Convert.ToDateTime(arrDates[columnIndex - 2]) < Convert.ToDateTime(DateTime.Now.AddMonths(6).ToShortDateString())))
                    {
                        e.Row.Cells[columnIndex].Attributes["style"] += "cursor:pointer;cursor:hand;";
                        e.Row.Cells[columnIndex].Attributes.Add("onclick", String.Format("arTranCellClicked({0},{1},{2},'{3}','{4}');", TranCode.Insert, e.Row.RowIndex, columnIndex, e.Row.Cells[columnIndex].Text, e.Row.Cells[1].Text));
                        e.Row.Cells[columnIndex].Attributes.Add("onmouseover", onmouseoverStyle);
                        e.Row.Cells[columnIndex].Attributes.Add("onmouseout", onmouseoutStyle);
                    }
                    else
                    {
                        e.Row.Cells[columnIndex].CssClass = "pastdate";
                    }
                }
                else if (e.Row.Cells[columnIndex].Text.Substring(0, 1) == "x")
                {
                    e.Row.Cells[columnIndex].Attributes["style"] += "cursor:pointer;cursor:hand;";
                    e.Row.Cells[columnIndex].Attributes.Add("onclick", String.Format("arTranCellClicked({0},{1},{2},'{3}','{4}');", TranCode.Delete, e.Row.RowIndex, columnIndex, e.Row.Cells[columnIndex].Text, e.Row.Cells[1].Text));

                    e.Row.Cells[columnIndex].CssClass = "blocked";
                    e.Row.Cells[columnIndex].Text = "";
                }
                else
                {
                    e.Row.Cells[columnIndex].Attributes["style"] += "cursor:pointer;cursor:hand;";
                    e.Row.Cells[columnIndex].Attributes.Add("onclick", String.Format("cellClicked('{0}');", e.Row.Cells[columnIndex].Text));

                    e.Row.Cells[columnIndex].CssClass = "withSchedule";
                    e.Row.Cells[columnIndex].Text = "";
                }
            }
        }
    }

    protected void hiddenButton_Click(object sender, EventArgs e)
    {
        string selectedRefNumber = inhRefNumber.Value;

        refNumberLabel.Text = selectedRefNumber;

        if (inhAction.Value == "CELLCLICKED")
        {
            RetrieveCCRequestDetailsRequest retrieveCCRequestDetailsRequest = new RetrieveCCRequestDetailsRequest();
            retrieveCCRequestDetailsRequest.CCRequestReferenceNo = selectedRefNumber;

            RetrieveCCRequestDetailsResult retrieveCCRequestDetailsResult = new RetrieveCCRequestDetailsResult();
            retrieveCCRequestDetailsResult = svc.RetrieveCCRequestDetails(retrieveCCRequestDetailsRequest);

            if (retrieveCCRequestDetailsResult.ResultStatus != iReserveWS.ResultStatus.Successful)
            {
                Utilities.MyMessageBox(retrieveCCRequestDetailsResult.Message);
            }
            else
            {
                eventNameLabel.Text = retrieveCCRequestDetailsResult.CCRequest.EventName;
                startDateLabel.Text = retrieveCCRequestDetailsResult.CCRequest.StartDate.ToString("MM/dd/yyyy");
                endDateLabel.Text = retrieveCCRequestDetailsResult.CCRequest.EndDate.ToString("MM/dd/yyyy");
                createdByLabel.Text = retrieveCCRequestDetailsResult.CCRequest.CreatedBy.ToString();
                costCenterLabel.Text = retrieveCCRequestDetailsResult.CCRequest.CostCenterName;
                dateCreatedLabel.Text = retrieveCCRequestDetailsResult.CCRequest.DateCreated.ToString();

                trainingRoomDetailsDiv.Style.Add("display", "block");
                trainingRoomDetails.Show();
            }
        }
    }

    protected void cancelButton_Click(object sender, EventArgs e)
    {
        trainingRoomDetails.Hide();
    }

    private void ShowBlockControls()
    {
        blockPanel.Visible = true;
        blockButton.Visible = true;

        unblockPanel.Visible = false;
        unblockButton.Visible = false;
    }

    private void ShowUnblockControls()
    {
        unblockPanel.Visible = true;
        unblockButton.Visible = true;

        blockPanel.Visible = false;
        blockButton.Visible = false;
    }

    protected void trHiddenButton_Click(object sender, EventArgs e)
    {
        int type = Convert.ToInt32(inhType.Value);
        int selectedRowIndex = Convert.ToInt32(inhRow.Value);
        int selectedColumnIndex = Convert.ToInt32(inhColumn.Value);
        string roomName = inhRoom.Value; ;
        string remarks = inhRefNumber.Value;

        string date = trainingRoomScheduleGridview.HeaderRow.Cells[selectedColumnIndex].Text;
        string troomID = trainingRoomScheduleGridview.Rows[selectedRowIndex].Cells[0].Text;
        string partitionID = trainingRoomScheduleGridview.Rows[selectedRowIndex].Cells[1].Text;

        roomTypeLabel.Text = "TR";
        roomNameLabel.Text = roomName;
        roomIDLabel.Text = partitionID;

        blockStartDateTextBox.Text = Convert.ToDateTime(date).ToString("MM/dd/yyyy");
        blockEndDateTextBox.Text = Convert.ToDateTime(date).ToString("MM/dd/yyyy");

        if (inhAction.Value == "CELLCLICKED")
        {
            if (type == TranCode.Insert)
            {
                tranLabel.Text = "BLOCK SCHEDULES";

                ShowBlockControls();

                blockDateTodayTextBox.Text = DateTime.Today.ToString("MM/dd/yyyy"); ;
            }
            else
            {
                tranLabel.Text = "UNBLOCK SCHEDULES";
                remarksLabel.Text = remarks.Substring(1);

                ShowUnblockControls();
            }

            tranDetailsDiv.Style.Add("display", "block");
            tranDetails.Show();
        }
    }

    protected void arHiddenButton_Click(object sender, EventArgs e)
    {
        int type = Convert.ToInt32(inhType.Value);
        int selectedRowIndex = Convert.ToInt32(inhRow.Value);
        int selectedColumnIndex = Convert.ToInt32(inhColumn.Value);
        string roomName = inhRoom.Value; ;
        string remarks = inhRefNumber.Value;

        string date = accomodationRoomScheduleGridView.HeaderRow.Cells[selectedColumnIndex].Text;
        string accRoomID = accomodationRoomScheduleGridView.Rows[selectedRowIndex].Cells[0].Text;

        roomTypeLabel.Text = "AR";
        roomNameLabel.Text = roomName;
        roomIDLabel.Text = accRoomID;

        blockStartDateTextBox.Text = Convert.ToDateTime(date).ToString("MM/dd/yyyy");
        blockEndDateTextBox.Text = Convert.ToDateTime(date).ToString("MM/dd/yyyy");

        if (inhAction.Value == "CELLCLICKED")
        {
            if (type == TranCode.Insert)
            {
                tranLabel.Text = "BLOCK SCHEDULES";

                ShowBlockControls();

                blockDateTodayTextBox.Text = DateTime.Today.ToString("MM/dd/yyyy"); ;
            }
            else
            {
                tranLabel.Text = "UNBLOCK SCHEDULES";
                remarksLabel.Text = remarks.Substring(1);

                ShowUnblockControls();
            }

            tranDetailsDiv.Style.Add("display", "block");
            tranDetails.Show();
        }
    }

    private bool IsTRScheduleAvailable()
    {
        ValidateTrainingRoomScheduleAvailabilityRequest validateTrainingRoomScheduleAvailabilityRequest = new ValidateTrainingRoomScheduleAvailabilityRequest();
        validateTrainingRoomScheduleAvailabilityRequest.RoomID = Convert.ToInt32(roomIDLabel.Text);
        validateTrainingRoomScheduleAvailabilityRequest.StartDate = Convert.ToDateTime(blockStartDateTextBox.Text);
        validateTrainingRoomScheduleAvailabilityRequest.EndDate = Convert.ToDateTime(blockEndDateTextBox.Text);

        ValidateTrainingRoomScheduleAvailabilityResult validateTrainingRoomScheduleAvailabilityResult = new ValidateTrainingRoomScheduleAvailabilityResult();
        validateTrainingRoomScheduleAvailabilityResult = svc.ValidateTrainingRoomScheduleAvailability(validateTrainingRoomScheduleAvailabilityRequest);

        if (validateTrainingRoomScheduleAvailabilityResult.ResultStatus != iReserveWS.ResultStatus.Successful)
        {
            Utilities.MyMessageBox(validateTrainingRoomScheduleAvailabilityResult.Message);

            return false;
        }
        else
        {
            return validateTrainingRoomScheduleAvailabilityResult.ValidationStatus;
        }
    }

    private bool IsARScheduleAvailable()
    {
        ValidateAccomodationRoomScheduleAvailabilityRequest validateAccomodationRoomScheduleAvailabilityRequest = new ValidateAccomodationRoomScheduleAvailabilityRequest();
        validateAccomodationRoomScheduleAvailabilityRequest.RoomID = Convert.ToInt32(roomIDLabel.Text);
        validateAccomodationRoomScheduleAvailabilityRequest.StartDate = Convert.ToDateTime(blockStartDateTextBox.Text);
        validateAccomodationRoomScheduleAvailabilityRequest.EndDate = Convert.ToDateTime(blockEndDateTextBox.Text);

        ValidateAccomodationRoomScheduleAvailabilityResult validateAccomodationRoomScheduleAvailabilityResult = new ValidateAccomodationRoomScheduleAvailabilityResult();
        validateAccomodationRoomScheduleAvailabilityResult = svc.ValidateAccomodationRoomScheduleAvailability(validateAccomodationRoomScheduleAvailabilityRequest);

        if (validateAccomodationRoomScheduleAvailabilityResult.ResultStatus != iReserveWS.ResultStatus.Successful)
        {
            Utilities.MyMessageBox(validateAccomodationRoomScheduleAvailabilityResult.Message);

            return false;
        }
        else
        {
            return validateAccomodationRoomScheduleAvailabilityResult.ValidationStatus;
        }
    }

    private void TRTran(int tranCode, string action)
    {
        TrainingRoomScheduleMappingTransactionRequest trainingRoomScheduleMappingTransactionRequest = new TrainingRoomScheduleMappingTransactionRequest();
        trainingRoomScheduleMappingTransactionRequest.Type = tranCode;
        trainingRoomScheduleMappingTransactionRequest.PartitionID = Convert.ToInt32(roomIDLabel.Text);
        trainingRoomScheduleMappingTransactionRequest.StartDate = Convert.ToDateTime(blockStartDateTextBox.Text);
        trainingRoomScheduleMappingTransactionRequest.EndDate = Convert.ToDateTime(blockEndDateTextBox.Text);
        trainingRoomScheduleMappingTransactionRequest.Remarks = blockRemarksTextBox.Text.Trim().Insert(0, "x");

        TrainingRoomScheduleMappingTransactionResult trainingRoomScheduleMappingTransactionResult = new TrainingRoomScheduleMappingTransactionResult();
        trainingRoomScheduleMappingTransactionResult = svc.TrainingRoomScheduleMappingTransaction(trainingRoomScheduleMappingTransactionRequest);

        if (trainingRoomScheduleMappingTransactionResult.ResultStatus != iReserveWS.ResultStatus.Successful)
        {
            Utilities.MyMessageBox(trainingRoomScheduleMappingTransactionResult.Message);
        }
        else
        {
            #region Audit Trail

            bool isSuccess = false;

            AuditTrail auditTrail = new AuditTrail();
            auditTrail.ActionDate = DateTime.Now;
            auditTrail.ActionTaken = action + " training room";
            auditTrail.ActionDetails = "Room: " + roomNameLabel.Text + " || Start Date: " + blockStartDateTextBox.Text + " || End Date: " + blockEndDateTextBox.Text;
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

            BindCalendarGridView();
        }
    }

    private void ARTran(int tranCode, string action)
    {
        AccomodationRoomScheduleMappingTransactionRequest accomodationRoomScheduleMappingTransactionRequest = new AccomodationRoomScheduleMappingTransactionRequest();
        accomodationRoomScheduleMappingTransactionRequest.Type = tranCode;
        accomodationRoomScheduleMappingTransactionRequest.AccRoomID = Convert.ToInt32(roomIDLabel.Text);
        accomodationRoomScheduleMappingTransactionRequest.StartDate = Convert.ToDateTime(blockStartDateTextBox.Text);
        accomodationRoomScheduleMappingTransactionRequest.EndDate = Convert.ToDateTime(blockEndDateTextBox.Text);
        accomodationRoomScheduleMappingTransactionRequest.Remarks = blockRemarksTextBox.Text.Trim().Insert(0, "x");

        AccomodationRoomScheduleMappingTransactionResult accomodationRoomScheduleMappingTransactionResult = new AccomodationRoomScheduleMappingTransactionResult();
        accomodationRoomScheduleMappingTransactionResult = svc.AccomodationRoomScheduleMappingTransaction(accomodationRoomScheduleMappingTransactionRequest);

        if (accomodationRoomScheduleMappingTransactionResult.ResultStatus != iReserveWS.ResultStatus.Successful)
        {
            Utilities.MyMessageBox(accomodationRoomScheduleMappingTransactionResult.Message);
        }
        else
        {
            #region Audit Trail

            bool isSuccess = false;

            AuditTrail auditTrail = new AuditTrail();
            auditTrail.ActionDate = DateTime.Now;
            auditTrail.ActionTaken = action + " accomodation room";
            auditTrail.ActionDetails = "Room: " + roomNameLabel.Text + " || Start Date: " + blockStartDateTextBox.Text + " || End Date: " + blockEndDateTextBox.Text;
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

            BindCalendarGridView();
        }
    }

    protected void blockButton_Click(object sender, EventArgs e)
    {
        string strMessage = "";

        if (roomTypeLabel.Text == "TR")
        {
            if (!IsTRScheduleAvailable())
            {
                strMessage = "The selected schedule is no longer available to block.";
                blockScheduleValidationLabel.Text = strMessage;
                tranDetails.Show();
            }
            else
            {
                TRTran(TranCode.Insert, "Blocked");

                blockScheduleValidationLabel.Text = "";
                blockRemarksTextBox.Text = "";
            }
        }
        else
        {
            if (!IsARScheduleAvailable())
            {
                strMessage = "The selected schedule is no longer available to block.";
                blockScheduleValidationLabel.Text = strMessage;
                tranDetails.Show();
            }
            else
            {
                ARTran(TranCode.Insert, "Blocked");

                blockScheduleValidationLabel.Text = "";
                blockRemarksTextBox.Text = "";
            }
        }
    }

    protected void unblockButton_Click(object sender, EventArgs e)
    {
        if (roomTypeLabel.Text == "TR")
        {
            TRTran(TranCode.Delete, "Unblocked");
        }
        else
        {
            ARTran(TranCode.Delete, "Unblocked");
        }
    }

    protected void tranCancelButton_Click(object sender, EventArgs e)
    {
        tranDetails.Hide();
        blockRemarksTextBox.Text = "";
        blockScheduleValidationLabel.Text = "";
    }
}