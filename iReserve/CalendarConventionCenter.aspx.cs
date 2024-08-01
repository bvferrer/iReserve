using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using iReserveWS;
using System.Web.Services.Protocols;

public partial class CalendarConventionCenter : System.Web.UI.Page
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
            if (profileName != "Requestor")
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

        DataTable trDataTable = new DataTable();
        trDataTable = (DataTable)Session["TrainingRoom"];

        DataTable trDetailDataTable = new DataTable();
        trDetailDataTable = (DataTable)Session["TrainingRoomDetail"];

        DataTable arDataTable = new DataTable();
        arDataTable = (DataTable)Session["AccomodationRoom"];

        if (trDataTable == null)
        {
            trDataTable = new DataTable();
            trDataTable.Columns.Add(new DataColumn("TRoomID", typeof(int)));
            trDataTable.Columns.Add(new DataColumn("PartitionID", typeof(int)));
            trDataTable.Columns.Add(new DataColumn("TRoomName", typeof(string)));
            trDataTable.Columns.Add(new DataColumn("PartitionName", typeof(string)));
            trDataTable.Columns.Add(new DataColumn("StartDate", typeof(DateTime)));
            trDataTable.Columns.Add(new DataColumn("EndDate", typeof(DateTime)));
            trDataTable.Columns.Add(new DataColumn("NumberOfDays", typeof(int)));
            trDataTable.Columns.Add(new DataColumn("HeadCount", typeof(int)));
            trDataTable.Columns.Add(new DataColumn("EquipmentToUse", typeof(string)));
            trDataTable.Columns.Add(new DataColumn("Remarks", typeof(string)));
            Session["TrainingRoom"] = trDataTable;
        }

        if (trDetailDataTable == null)
        {
            trDetailDataTable = new DataTable();
            trDetailDataTable.Columns.Add(new DataColumn("PartitionID", typeof(int)));
            trDetailDataTable.Columns.Add(new DataColumn("Date", typeof(DateTime)));
            trDetailDataTable.Columns.Add(new DataColumn("StartDateTime", typeof(DateTime)));
            trDetailDataTable.Columns.Add(new DataColumn("EndDateTime", typeof(DateTime)));
            Session["TrainingRoomDetail"] = trDetailDataTable;
        }

        if (arDataTable == null)
        {
            arDataTable = new DataTable();
            arDataTable.Columns.Add(new DataColumn("AccRoomID", typeof(int)));
            arDataTable.Columns.Add(new DataColumn("AccRoomName", typeof(string)));
            arDataTable.Columns.Add(new DataColumn("StartDate", typeof(DateTime)));
            arDataTable.Columns.Add(new DataColumn("EndDate", typeof(DateTime)));
            arDataTable.Columns.Add(new DataColumn("NumberOfNights", typeof(int)));
            arDataTable.Columns.Add(new DataColumn("HeadCount", typeof(int)));
            arDataTable.Columns.Add(new DataColumn("Remarks", typeof(string)));
            Session["AccomodationRoom"] = arDataTable;
        }
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

    #region Training Room

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
                        e.Row.Cells[columnIndex].Attributes.Add("onclick", String.Format("trCellClicked({0},{1});", e.Row.RowIndex, columnIndex));
                        e.Row.Cells[columnIndex].Attributes.Add("onmouseover", onmouseoverStyle);
                        e.Row.Cells[columnIndex].Attributes.Add("onmouseout", onmouseoutStyle);
                    }
                    else
                    {
                        e.Row.Cells[columnIndex].CssClass = "pastdate";
                    }
                }
                else
                {
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

    protected void trHiddenButton_Click(object sender, EventArgs e)
    {
        int selectedRowIndex = Convert.ToInt32(inhRow.Value);
        int selectedColumnIndex = Convert.ToInt32(inhColumn.Value);

        string date = trainingRoomScheduleGridview.HeaderRow.Cells[selectedColumnIndex].Text;
        string troomID = trainingRoomScheduleGridview.Rows[selectedRowIndex].Cells[0].Text;
        string partitionID = trainingRoomScheduleGridview.Rows[selectedRowIndex].Cells[1].Text;

        if (inhAction.Value == "CELLCLICKED")
        {
            RetrieveTRPartitionRecordDetailsRequest retrieveTRPartitionRecordDetailsRequest = new RetrieveTRPartitionRecordDetailsRequest();
            retrieveTRPartitionRecordDetailsRequest.PartitionID = Convert.ToInt32(partitionID);

            RetrieveTRPartitionRecordDetailsResult retrieveTRPartitionRecordDetailsResult = new RetrieveTRPartitionRecordDetailsResult();
            retrieveTRPartitionRecordDetailsResult = svc.RetrieveTRPartitionRecordDetails(retrieveTRPartitionRecordDetailsRequest);

            if (retrieveTRPartitionRecordDetailsResult.ResultStatus != iReserveWS.ResultStatus.Successful)
            {
                Utilities.MyMessageBox(retrieveTRPartitionRecordDetailsResult.Message);
            }
            else
            {
                tRoomIDLabel.Text = retrieveTRPartitionRecordDetailsResult.TrainingRoom.TRoomID.ToString();
                tRoomNameLabel.Text = retrieveTRPartitionRecordDetailsResult.TrainingRoom.TRoomName;
                partitionIDLabel.Text = retrieveTRPartitionRecordDetailsResult.TRPartition.PartitionID.ToString();
                partitionNameLabel.Text = retrieveTRPartitionRecordDetailsResult.TRPartition.PartitionName;
                tRoomLocationLabel.Text = retrieveTRPartitionRecordDetailsResult.TrainingRoom.LocationName;
                partitionMaxPersonLabel.Text = retrieveTRPartitionRecordDetailsResult.TRPartition.MaxPerson.ToString();
                partitionDetailsLabel.Text = retrieveTRPartitionRecordDetailsResult.TRPartition.PartitionDesc;

                trStartDateLabel.Text = Convert.ToDateTime(date).ToString("MMM d, yyyy");
                trStartDateTextBox.Text = Convert.ToDateTime(date).ToString("MM/dd/yyyy");
                //trEndDateTextBox.Text = Convert.ToDateTime(date).ToString("MM/dd/yyyy");

                SetInitialRow();

                trainingRoomDetailsDiv.Style.Add("display", "block");
                trainingRoomDetails.Show();

                partitionMaxPersonTextBox.Text = retrieveTRPartitionRecordDetailsResult.TRPartition.MaxPerson.ToString();
                //trDateTodayTextBox.Text = DateTime.Today.ToString("MM/dd/yyyy"); ;
            }
        }
    }

    protected void trAddButton_Click(object sender, EventArgs e)
    {
        string strMessage = "";
        trScheduleValidationLabel.Text = "";

        if (IsTRAdded())
        {
            strMessage = "The selected room is already added. Please check Request Summary.";
            trScheduleValidationLabel.Text = strMessage;
            trainingRoomDetails.Show();
        }
        else
        {
            if (!IsValidTRStartEndDate())
            {
                strMessage = "Training Room Subrooms should have the same start/end dates. Please check Request Summary.";
                trScheduleValidationLabel.Text = strMessage;
                trainingRoomDetails.Show();
            }
            else
            {
                if (!IsTRScheduleAvailable())
                {
                    strMessage = "The selected schedule is not available.";
                    trScheduleValidationLabel.Text = strMessage;
                    trainingRoomDetails.Show();
                }
                else
                {
                    AddTrainingRoomToSummary();

                    trScheduleValidationLabel.Text = "";
                    TrainingRoomClearControls();

                    //Utilities.MyMessageBox(partitionNameLabel.Text + " is added to summary.");
                }
            }
        }
    }

    protected void trCancelButton_Click(object sender, EventArgs e)
    {
        trainingRoomDetails.Hide();
        TrainingRoomClearControls();
    }

    private void SetInitialRow()
    {
        numberOfDaysLabel.Text = "1";
        numberOfDaysNumericBox.Text = "1";
        SetDetailGridViewRows();
    }

    protected void upButton_Click(object sender, EventArgs e)
    {
        int days = Convert.ToInt32(numberOfDaysNumericBox.Text);

        if (days != 10)
        {
            days += 1;
            numberOfDaysLabel.Text = days.ToString();
            numberOfDaysNumericBox.Text = days.ToString();
        }

        numberOfDaysLabel.Text = days.ToString();
        numberOfDaysNumericBox.Text = days.ToString();

        SetDetailGridViewRows();

        trainingRoomDetailsDiv.Style.Add("display", "block");
        trainingRoomDetails.Show();
    }

    protected void downButton_Click(object sender, EventArgs e)
    {
        int days = Convert.ToInt32(numberOfDaysNumericBox.Text);

        if (days != 1)
        {
            days -= 1;
            numberOfDaysLabel.Text = days.ToString();
            numberOfDaysNumericBox.Text = days.ToString();
        }

        numberOfDaysLabel.Text = days.ToString();
        numberOfDaysNumericBox.Text = days.ToString();

        SetDetailGridViewRows();

        trainingRoomDetailsDiv.Style.Add("display", "block");
        trainingRoomDetails.Show();
    }

    private void SetDetailGridViewRows()
    {
        int row = Convert.ToInt32(numberOfDaysNumericBox.Text);
        DateTime date = Convert.ToDateTime(trStartDateTextBox.Text);

        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(new DataColumn("PartitionID", typeof(string)));
        dt.Columns.Add(new DataColumn("Date", typeof(DateTime)));

        for (int i = 0; i < row; i++)
        {
            dr = dt.NewRow();
            dr["PartitionID"] = partitionIDLabel.Text;
            dr["Date"] = date;
            dt.Rows.Add(dr);

            date = date.AddDays(1);
        }

        trainingRoomDetailGridView.DataSource = dt;
        trainingRoomDetailGridView.DataBind();
    }

    //protected void trainingRoomDetailGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        DropDownList startTimeDropDownList = (DropDownList)e.Row.FindControl("startTimeDropDownList");
    //        startTimeDropDownList.Items.Insert(0, new ListItem("7:00 AM", "7:00"));
    //        startTimeDropDownList.Items.Insert(1, new ListItem("8:00 AM", "8:00"));
    //        startTimeDropDownList.Items.Insert(2, new ListItem("9:00 AM", "9:00"));
    //        startTimeDropDownList.Items.Insert(3, new ListItem("10:00 AM", "10:00"));
    //        startTimeDropDownList.Items.Insert(4, new ListItem("11:00 AM", "11:00"));
    //        startTimeDropDownList.Items.Insert(5, new ListItem("12:00 PM", "12:00"));
    //        startTimeDropDownList.Items.Insert(6, new ListItem("1:00 PM", "13:00"));
    //        startTimeDropDownList.Items.FindByValue("8:00").Selected = true;

    //        DropDownList endTimeDropDownList = (DropDownList)e.Row.FindControl("endTimeDropDownList");
    //        endTimeDropDownList.Items.Insert(0, new ListItem("4:00 PM", "16:00"));
    //        endTimeDropDownList.Items.Insert(1, new ListItem("5:00 PM", "17:00"));
    //        endTimeDropDownList.Items.Insert(2, new ListItem("6:00 PM", "18:00"));
    //        endTimeDropDownList.Items.Insert(3, new ListItem("7:00 PM", "19:00"));
    //        endTimeDropDownList.Items.Insert(4, new ListItem("8:00 PM", "20:00"));
    //        endTimeDropDownList.Items.Insert(5, new ListItem("9:00 PM", "21:00"));
    //        endTimeDropDownList.Items.Insert(6, new ListItem("10:00 PM", "22:00"));
    //        endTimeDropDownList.Items.Insert(7, new ListItem("11:00 PM", "23:00"));
    //        endTimeDropDownList.Items.FindByValue("17:00").Selected = true;
    //    }
    //}

    private void TrainingRoomClearControls()
    {
        trHeadCountNumericBox.Text = "";

        trEquipmentTextBox.Text = "";
        trRemarksTextBox.Text = "";

        trScheduleValidationLabel.Text = "";
    }

    private bool IsTRAdded()
    {
        DataTable trainingRoomDataTable = new DataTable();
        trainingRoomDataTable = (DataTable)Session["TrainingRoom"];

        if (trainingRoomDataTable.Select("PartitionID = '" + partitionIDLabel.Text + "'").Length > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsValidTRStartEndTime()
    {
        int rowsCount = Convert.ToInt32(numberOfDaysNumericBox.Text);

        for (int i = 0; i < rowsCount; i++)
        {
            DropDownList startTimeDropDownList = (DropDownList)trainingRoomDetailGridView.Rows[i].Cells[2].FindControl("startTimeDropDownList");
            DropDownList endTimeDropDownList = (DropDownList)trainingRoomDetailGridView.Rows[i].Cells[3].FindControl("endTimeDropDownList");

            if (Array.IndexOf(Settings.ValidStartTime, startTimeDropDownList.SelectedValue) == null)
            {
                return false;
            }

            if (Array.IndexOf(Settings.ValidEndTime, endTimeDropDownList.SelectedValue) == null)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsValidTRStartEndDate()
    {
        int tRoomID = Convert.ToInt16(tRoomIDLabel.Text);
        DateTime startDate = Convert.ToDateTime(trStartDateTextBox.Text.Trim());
        DateTime endDate = Convert.ToDateTime(trStartDateTextBox.Text.Trim()).AddDays(Convert.ToInt16(numberOfDaysNumericBox.Text) - 1);

        DataTable trainingRoomDataTable = (DataTable)Session["TrainingRoom"];
        DataRow[] dr = trainingRoomDataTable.Select("TRoomID = '" + tRoomID + "'");

        if (dr.Length == 0)
        {
            return true;
        }
        else
        {
            if (startDate != Convert.ToDateTime(dr[0]["StartDate"]))
            {
                return false;
            }
            else
            {
                if (endDate != Convert.ToDateTime(dr[0]["EndDate"]))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }

    private bool IsTRScheduleAvailable()
    {
        ValidateTrainingRoomScheduleAvailabilityRequest validateTrainingRoomScheduleAvailabilityRequest = new ValidateTrainingRoomScheduleAvailabilityRequest();
        validateTrainingRoomScheduleAvailabilityRequest.RoomID = Convert.ToInt32(partitionIDLabel.Text);
        validateTrainingRoomScheduleAvailabilityRequest.StartDate = Convert.ToDateTime(trStartDateTextBox.Text);
        validateTrainingRoomScheduleAvailabilityRequest.EndDate = Convert.ToDateTime(trStartDateTextBox.Text.Trim()).AddDays(Convert.ToInt16(numberOfDaysNumericBox.Text) - 1);

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

    private void AddTrainingRoomToSummary()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["TrainingRoom"];

        DataRow dr = dt.NewRow();
        dr["TRoomID"] = tRoomIDLabel.Text;
        dr["PartitionID"] = partitionIDLabel.Text;
        dr["TRoomName"] = tRoomNameLabel.Text;
        dr["PartitionName"] = partitionNameLabel.Text;
        dr["StartDate"] = Convert.ToDateTime(trStartDateTextBox.Text.Trim());
        dr["EndDate"] = Convert.ToDateTime(trStartDateTextBox.Text.Trim()).AddDays(Convert.ToInt16(numberOfDaysNumericBox.Text) - 1);
        dr["NumberOfDays"] = Convert.ToInt32(numberOfDaysNumericBox.Text);
        dr["HeadCount"] = trHeadCountNumericBox.Text.Trim();
        dr["EquipmentToUse"] = trEquipmentTextBox.Text.Trim();
        dr["Remarks"] = trRemarksTextBox.Text.Trim();
        dt.Rows.Add(dr);

        Session["TrainingRoom"] = dt;

        DataTable dt2 = new DataTable();
        dt2 = (DataTable)Session["TrainingRoomDetail"];

        int rowsCount = Convert.ToInt32(numberOfDaysNumericBox.Text);

        for (int i = 0; i < rowsCount; i++)
        {
            string date = trainingRoomDetailGridView.Rows[i].Cells[1].Text;
            DropDownList startTimeDropDownList = (DropDownList)trainingRoomDetailGridView.Rows[i].Cells[2].FindControl("startTimeDropDownList");
            DropDownList endTimeDropDownList = (DropDownList)trainingRoomDetailGridView.Rows[i].Cells[3].FindControl("endTimeDropDownList");

            DataRow dr2 = dt2.NewRow();
            dr2["PartitionID"] = partitionIDLabel.Text;
            dr2["Date"] = Convert.ToDateTime(date);
            dr2["StartDateTime"] = Convert.ToDateTime(date + " " + TimeSpan.Parse(startTimeDropDownList.SelectedValue));
            dr2["EndDateTime"] = Convert.ToDateTime(date + " " + TimeSpan.Parse(endTimeDropDownList.SelectedValue));
            dt2.Rows.Add(dr2);
        }

        Session["TrainingRoomDetail"] = dt2;

        Utilities.MyMessageBox(partitionNameLabel.Text + " has been added to Request Summary. Click View Request Summary button to complete your reservation.");
    }

    #endregion

    #region Accomodation Room

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
                        e.Row.Cells[columnIndex].Attributes.Add("onclick", String.Format("arCellClicked({0},{1});", e.Row.RowIndex, columnIndex));
                        e.Row.Cells[columnIndex].Attributes.Add("onmouseover", onmouseoverStyle);
                        e.Row.Cells[columnIndex].Attributes.Add("onmouseout", onmouseoutStyle);
                    }
                    else
                    {
                        e.Row.Cells[columnIndex].CssClass = "pastdate";
                    }
                }
                else
                {
                    e.Row.Cells[columnIndex].CssClass = "withSchedule";
                    e.Row.Cells[columnIndex].Text = "";
                }
            }
        }
    }

    protected void arHiddenButton_Click(object sender, EventArgs e)
    {
        int selectedRowIndex = Convert.ToInt32(inhRow.Value);
        int selectedColumnIndex = Convert.ToInt32(inhColumn.Value);

        string date = accomodationRoomScheduleGridView.HeaderRow.Cells[selectedColumnIndex].Text;
        string accRoomID = accomodationRoomScheduleGridView.Rows[selectedRowIndex].Cells[0].Text;

        if (inhAction.Value == "CELLCLICKED")
        {
            RetrieveAccomodationRoomRecordDetailsRequest retrieveAccomodationRoomRecordDetailsRequest = new RetrieveAccomodationRoomRecordDetailsRequest();
            retrieveAccomodationRoomRecordDetailsRequest.RoomID = Convert.ToInt32(accRoomID);

            RetrieveAccomodationRoomRecordDetailsResult retrieveAccomodationRoomRecordDetailsResult = new RetrieveAccomodationRoomRecordDetailsResult();
            retrieveAccomodationRoomRecordDetailsResult = svc.RetrieveAccomodationRoomRecordDetails(retrieveAccomodationRoomRecordDetailsRequest);

            if (retrieveAccomodationRoomRecordDetailsResult.ResultStatus != iReserveWS.ResultStatus.Successful)
            {
                Utilities.MyMessageBox(retrieveAccomodationRoomRecordDetailsResult.Message);
            }
            else
            {
                accRoomIDLabel.Text = retrieveAccomodationRoomRecordDetailsResult.AccomodationRoom.AccRoomID.ToString();
                accRoomNameLabel.Text = retrieveAccomodationRoomRecordDetailsResult.AccomodationRoom.RoomName;
                accRoomLocationLabel.Text = retrieveAccomodationRoomRecordDetailsResult.AccomodationRoom.LocationName;
                accRoomMaxPersonLabel.Text = retrieveAccomodationRoomRecordDetailsResult.AccomodationRoom.MaxPerson.ToString();
                accRateLabel.Text = Convert.ToDecimal(retrieveAccomodationRoomRecordDetailsResult.AccomodationRoom.RatePerNight).ToString("#,###.00");
                accRoomDetailsLabel.Text = retrieveAccomodationRoomRecordDetailsResult.AccomodationRoom.RoomDesc;

                arStartDateTextBox.Text = Convert.ToDateTime(date).ToString("MM/dd/yyyy");
                arEndDateTextBox.Text = Convert.ToDateTime(date).AddDays(1).ToString("MM/dd/yyyy");

                accomodationRoomDetailsDiv.Style.Add("display", "block");
                accomodationRoomDetails.Show();

                arMaxPersonTextBox.Text = retrieveAccomodationRoomRecordDetailsResult.AccomodationRoom.MaxPerson.ToString();
                arDateTodayTextBox.Text = DateTime.Today.ToString("MM/dd/yyyy"); ;
            }
        }
    }

    protected void arAddButton_Click(object sender, EventArgs e)
    {
        string strMessage = "";
        arScheduleValidationLabel.Text = "";

        if (IsARAdded())
        {
            strMessage = "The selected room is already added. Please check Request Summary.";
            arScheduleValidationLabel.Text = strMessage;
            accomodationRoomDetails.Show();
        }
        else
        {
            if (!IsARScheduleAvailable())
            {
                strMessage = "The selected schedule is not available";
                arScheduleValidationLabel.Text = strMessage;
                accomodationRoomDetails.Show();
            }
            else
            {
                AddAccomodationRoomToSummary();

                arScheduleValidationLabel.Text = "";
                AccomodationRoomClearControls();
            }
        }
    }

    protected void arCancelButton_Click(object sender, EventArgs e)
    {
        accomodationRoomDetails.Hide();
        AccomodationRoomClearControls();
    }

    private void AccomodationRoomClearControls()
    {
        arHeadCountNumericBox.Text = "";
        arRemarksTextBox.Text = "";
        arScheduleValidationLabel.Text = "";
    }

    private bool IsARAdded()
    {
        DataTable accomodationRoomDataTable = new DataTable();
        accomodationRoomDataTable = (DataTable)Session["AccomodationRoom"];

        if (accomodationRoomDataTable.Select("AccRoomID = '" + accRoomIDLabel.Text + "'").Length > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsARScheduleAvailable()
    {
        ValidateAccomodationRoomScheduleAvailabilityRequest validateAccomodationRoomScheduleAvailabilityRequest = new ValidateAccomodationRoomScheduleAvailabilityRequest();
        validateAccomodationRoomScheduleAvailabilityRequest.RoomID = Convert.ToInt32(accRoomIDLabel.Text);
        validateAccomodationRoomScheduleAvailabilityRequest.StartDate = Convert.ToDateTime(arStartDateTextBox.Text);
        validateAccomodationRoomScheduleAvailabilityRequest.EndDate = Convert.ToDateTime(arEndDateTextBox.Text);

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

    private void AddAccomodationRoomToSummary()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["AccomodationRoom"];

        DataRow dr = dt.NewRow();
        dr["AccRoomID"] = accRoomIDLabel.Text;
        dr["AccRoomName"] = accRoomNameLabel.Text;
        dr["StartDate"] = Convert.ToDateTime(arStartDateTextBox.Text.Trim());
        dr["EndDate"] = Convert.ToDateTime(arEndDateTextBox.Text.Trim());
        dr["NumberOfNights"] = Convert.ToInt32((Convert.ToDateTime(arEndDateTextBox.Text.Trim()) - Convert.ToDateTime(arStartDateTextBox.Text.Trim())).TotalDays);
        dr["HeadCount"] = arHeadCountNumericBox.Text.Trim();
        dr["Remarks"] = arRemarksTextBox.Text.Trim();
        dt.Rows.Add(dr);

        Session["AccomodationRoom"] = dt;

        Utilities.MyMessageBox(accRoomNameLabel.Text + " has been added to Request Summary. Click View Request Summary button to complete your reservation.");
    }

    #endregion

    #region Summary

    private void ClearSummaryControls()
    {
        eventNameTextBox.Text = "";
        remarksTextBox.Text = "";

        DataTable trainingRoomDataTable = new DataTable();
        trainingRoomDataTable = (DataTable)Session["TrainingRoom"];
        trainingRoomDataTable.Rows.Clear();
        Session["TrainingRoom"] = trainingRoomDataTable;

        DataTable trainingRoomDetailDataTable = new DataTable();
        trainingRoomDetailDataTable = (DataTable)Session["TrainingRoomDetail"];
        trainingRoomDetailDataTable.Rows.Clear();
        Session["TrainingRoomDetail"] = trainingRoomDetailDataTable;

        DataTable accomodationRoomDataTable = new DataTable();
        accomodationRoomDataTable = (DataTable)Session["AccomodationRoom"];
        accomodationRoomDataTable.Rows.Clear();
        Session["AccomodationRoom"] = accomodationRoomDataTable;

        approvalLabel.Text = "";
        summaryValidationLabel.Text = "";
    }

    private void BindCostCenter()
    {
        try
        {
            costCenterDropDownList.DataSource = svc.RetrieveCostCenter();
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericWebServiceMessage);
        }

        costCenterDropDownList.DataValueField = "CostCenterID";
        costCenterDropDownList.DataTextField = "CostCenterName";
        costCenterDropDownList.DataBind();
        costCenterDropDownList.Items.Insert(0, new ListItem("Select", "0"));
    }

    private void BindSummaryGridView()
    {
        DataTable trainingRoomDataTable = new DataTable();
        trainingRoomDataTable = (DataTable)Session["TrainingRoom"];
        summaryTrainingRoomGridView.DataSource = trainingRoomDataTable;
        summaryTrainingRoomGridView.DataBind();

        if (summaryTrainingRoomGridView.Rows.Count == 0)
        {
            DataTable trdt = trainingRoomDataTable.Clone();
            trdt.Rows.Add(trdt.NewRow());

            summaryTrainingRoomGridView.DataSource = trdt;
            summaryTrainingRoomGridView.DataBind();

            int columncount = summaryTrainingRoomGridView.Rows[0].Cells.Count;
            summaryTrainingRoomGridView.Rows[0].Cells.Clear();
            summaryTrainingRoomGridView.Rows[0].Cells.Add(new TableCell());
            summaryTrainingRoomGridView.Rows[0].Cells[0].ColumnSpan = columncount;
            summaryTrainingRoomGridView.Rows[0].Cells[0].Text = "-";
        }

        DataTable accomodationRoomDataTable = new DataTable();
        accomodationRoomDataTable = (DataTable)Session["AccomodationRoom"];
        summaryAccomodationRoomGridView.DataSource = accomodationRoomDataTable;
        summaryAccomodationRoomGridView.DataBind();

        if (summaryAccomodationRoomGridView.Rows.Count == 0)
        {
            DataTable ardt = accomodationRoomDataTable.Clone();
            ardt.Rows.Add(ardt.NewRow());

            summaryAccomodationRoomGridView.DataSource = ardt;
            summaryAccomodationRoomGridView.DataBind();

            int columncount = summaryAccomodationRoomGridView.Rows[0].Cells.Count;
            summaryAccomodationRoomGridView.Rows[0].Cells.Clear();
            summaryAccomodationRoomGridView.Rows[0].Cells.Add(new TableCell());
            summaryAccomodationRoomGridView.Rows[0].Cells[0].ColumnSpan = columncount;
            summaryAccomodationRoomGridView.Rows[0].Cells[0].Text = "-";
        }
    }

    protected void viewSummaryButton_Click(object sender, EventArgs e)
    {
        BindCostCenter();
        BindSummaryGridView();

        summaryDetailsDiv.Style.Add("display", "block");
        summaryDetails.Show();
    }

    protected void summaryTrainingRoomGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string partitionID = summaryTrainingRoomGridView.Rows[e.RowIndex].Cells[1].Text;

        DataTable dt = new DataTable();
        dt = (DataTable)Session["TrainingRoom"];
        dt.Rows[e.RowIndex].Delete();

        Session["TrainingRoom"] = dt;

        DataTable dt2 = new DataTable();
        dt2 = (DataTable)Session["TrainingRoomDetail"];

        foreach (DataRow dr in dt2.Select("PartitionID = " + partitionID))
        {
            dr.Delete();
        }

        Session["TrainingRoomDetail"] = dt2;

        BindSummaryGridView();

        summaryDetails.Show();
    }

    protected void summaryAccomodationRoomGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["AccomodationRoom"];
        dt.Rows[e.RowIndex].Delete();

        Session["AccomodationRoom"] = dt;

        BindSummaryGridView();

        summaryDetails.Show();
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        string strMessage = "";
        int fileNameLength = approvalFileUpload.FileName.Length;
        int fileSizeLimit = 3000000;
        int fileSizeActual = approvalFileUpload.PostedFile.ContentLength;

        if (fileNameLength > 50)
        {
            strMessage = "File name is too long. File name should NOT be more than 50 characters ";
            strMessage += "(including spaces and file extension).";
            approvalLabel.Text = strMessage;
            summaryDetails.Show();
            BindSummaryGridView();
        }
        else
        {
            if (fileSizeActual > fileSizeLimit)
            {
                strMessage = "Allowed attachment file size is up to 3MB only.";
                approvalLabel.Text = strMessage;
                summaryDetails.Show();
                BindSummaryGridView();
            }
            else
            {
                approvalLabel.Text = "";

                if (IsSummaryRoomEmpty())
                {
                    strMessage = "Please add training/accomodation room.";
                    summaryValidationLabel.Text = strMessage;
                    summaryDetails.Show();
                    BindSummaryGridView();
                }
                else
                {
                    if (!IsSummaryScheduleAvailable())
                    {
                        strMessage = "The selected schedule is no longer available. Kindly check Reservation Calendar Page.";
                        summaryValidationLabel.Text = strMessage;
                        summaryDetails.Show();
                        BindSummaryGridView();
                    }
                    else
                    {
                        Submit();

                        summaryValidationLabel.Text = "";
                        ClearSummaryControls();
                    }
                }
            }
        }
    }

    protected void clearButton_Click(object sender, EventArgs e)
    {
        ClearSummaryControls();
    }

    protected void backButton_Click(object sender, EventArgs e)
    {
        summaryDetails.Hide();
    }

    private bool IsSummaryRoomEmpty()
    {
        DataTable trainingRoomDataTable = new DataTable();
        trainingRoomDataTable = (DataTable)Session["TrainingRoom"];

        DataTable accomodationRoomDataTable = new DataTable();
        accomodationRoomDataTable = (DataTable)Session["AccomodationRoom"];

        if ((trainingRoomDataTable.Rows.Count + accomodationRoomDataTable.Rows.Count) > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool IsSummaryScheduleAvailable()
    {
        //Training Room
        DataTable trainingRoomDataTable = (DataTable)Session["TrainingRoom"];
        List<TrainingRoomRequest> trainingRoomRequestList = new List<TrainingRoomRequest>();

        foreach (DataRow dr in trainingRoomDataTable.Rows)
        {
            TrainingRoomRequest trainingRoomRequest = new TrainingRoomRequest();
            trainingRoomRequest.PartitionID = Convert.ToInt32(dr["PartitionID"]);
            trainingRoomRequest.StartDate = Convert.ToDateTime(dr["StartDate"]);
            trainingRoomRequest.EndDate = Convert.ToDateTime(dr["EndDate"]);
            trainingRoomRequestList.Add(trainingRoomRequest);
        }

        //Accomodation Room
        DataTable accomodationRoomDataTable = (DataTable)Session["AccomodationRoom"];
        List<AccomodationRoomRequest> accomodationRoomRequestList = new List<AccomodationRoomRequest>();

        foreach (DataRow dr in accomodationRoomDataTable.Rows)
        {
            AccomodationRoomRequest accomodationRoomRequest = new AccomodationRoomRequest();
            accomodationRoomRequest.AccID = Convert.ToInt32(dr["AccRoomID"]);
            accomodationRoomRequest.StartDate = Convert.ToDateTime(dr["StartDate"]);
            accomodationRoomRequest.EndDate = Convert.ToDateTime(dr["EndDate"]);
            accomodationRoomRequestList.Add(accomodationRoomRequest);
        }

        ValidateSummaryScheduleAvailabilityRequest validateSummaryScheduleAvailabilityRequest = new ValidateSummaryScheduleAvailabilityRequest();
        validateSummaryScheduleAvailabilityRequest.TrainingRoomRequestList = trainingRoomRequestList.ToArray();
        validateSummaryScheduleAvailabilityRequest.AccomodationRoomRequestList = accomodationRoomRequestList.ToArray();

        ValidateSummaryScheduleAvailabilityResult validateSummaryScheduleAvailabilityResult = svc.ValidateSummaryScheduleAvailability(validateSummaryScheduleAvailabilityRequest);

        if (validateSummaryScheduleAvailabilityResult.ResultStatus != iReserveWS.ResultStatus.Successful)
        {
            Utilities.MyMessageBox(validateSummaryScheduleAvailabilityResult.Message);
            return false;
        }
        else
        {
            return validateSummaryScheduleAvailabilityResult.ValidationStatus;
        }
    }

    private DateTime GetMinStartDate()
    {
        DataTable trainingRoomDataTable = (DataTable)Session["TrainingRoom"];
        DateTime trMinDate = DateTime.MaxValue;

        foreach (DataRow dr in trainingRoomDataTable.Select("StartDate = MIN(StartDate)"))
        {
            trMinDate = Convert.ToDateTime(dr["StartDate"]);
        }

        DataTable accomodationRoomDataTable = (DataTable)Session["AccomodationRoom"];
        DateTime arMinDate = DateTime.MaxValue;

        foreach (DataRow dr in accomodationRoomDataTable.Select("StartDate = MIN(StartDate)"))
        {
            arMinDate = Convert.ToDateTime(dr["StartDate"]);
        }

        return (trMinDate > arMinDate ? arMinDate : trMinDate);
    }

    private DateTime GetMaxEndDate()
    {
        DataTable trainingRoomDataTable = (DataTable)Session["TrainingRoom"];
        DateTime trMaxDate = DateTime.MinValue;

        foreach (DataRow dr in trainingRoomDataTable.Select("EndDate = MAX(EndDate)"))
        {
            trMaxDate = Convert.ToDateTime(dr["EndDate"]);
        }

        DataTable accomodationRoomDataTable = (DataTable)Session["AccomodationRoom"];
        DateTime arMaxDate = DateTime.MinValue;

        foreach (DataRow dr in accomodationRoomDataTable.Select("EndDate = MAX(EndDate)"))
        {
            arMaxDate = Convert.ToDateTime(dr["EndDate"]);
        }

        return (trMaxDate > arMaxDate ? trMaxDate : arMaxDate);
    }

    private void Submit()
    {
        //Generate reference number
        string lastGeneratedRefNo = "";

        RetrieveLastGeneratedCCRefNoRequest retrieveLastGeneratedCCRefNoRequest = new RetrieveLastGeneratedCCRefNoRequest();
        retrieveLastGeneratedCCRefNoRequest.DateGenerated = DateTime.Today;

        RetrieveLastGeneratedCCRefNoResult retrieveLastGeneratedCCRefNoResult = svc.RetrieveLastGeneratedCCRefNo(retrieveLastGeneratedCCRefNoRequest);

        if (retrieveLastGeneratedCCRefNoResult.ResultStatus != iReserveWS.ResultStatus.Successful)
        {
            Utilities.MyMessageBox(retrieveLastGeneratedCCRefNoResult.Message);
        }
        else
        {
            lastGeneratedRefNo = retrieveLastGeneratedCCRefNoResult.CCLastGeneratedReferenceNumber.LastReferenceNumber;
        }

        if (Application["LastGeneratedCCRefNo"] == null)
        {
            Application["LastGeneratedCCRefNo"] = lastGeneratedRefNo;
        }

        //Insert new request
        //Request Details
        CCRequest request = new CCRequest();
        request.CCRequestReferenceNo = lastGeneratedRefNo;
        request.EventName = eventNameTextBox.Text.Trim();
        request.StartDate = GetMinStartDate();
        request.EndDate = GetMaxEndDate();
        request.CostCenterID = Convert.ToInt32(costCenterDropDownList.SelectedValue);
        request.StatusCode = StatusCode.Confirmed;
        request.SOAStatusCode = SOAStatusCode.ForProcessing;
        request.CreatedByID = Session["UserID"].ToString();
        request.CreatedBy = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();

        //Attachment
        CCRequestAttachment attachment = new CCRequestAttachment();
        attachment.CCRequestReferenceNo = lastGeneratedRefNo;
        attachment.StatusCode = StatusCode.Confirmed;
        attachment.FileName = approvalFileUpload.FileName.Replace(" ", "_");
        attachment.FileType = approvalFileUpload.PostedFile.ContentType;
        attachment.FileSize = approvalFileUpload.PostedFile.ContentLength;
        attachment.File = approvalFileUpload.FileBytes;

        //History
        CCRequestHistory history = new CCRequestHistory();
        history.CCRequestReferenceNumber = lastGeneratedRefNo;
        history.StatusCode = StatusCode.Confirmed;
        history.ProcessedByID = Session["UserID"].ToString();
        history.ProcessedBy = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();
        history.Remarks = remarksTextBox.Text.Trim();

        //Training Room
        DataTable trainingRoomDataTable = (DataTable)Session["TrainingRoom"];
        List<TrainingRoomRequest> trainingRoomRequestList = new List<TrainingRoomRequest>();

        foreach (DataRow dr in trainingRoomDataTable.Rows)
        {
            TrainingRoomRequest trainingRoomRequest = new TrainingRoomRequest();
            trainingRoomRequest.CCRequestReferenceNo = lastGeneratedRefNo;
            trainingRoomRequest.PartitionID = Convert.ToInt32(dr["PartitionID"]);
            trainingRoomRequest.StartDate = Convert.ToDateTime(dr["StartDate"]);
            trainingRoomRequest.EndDate = Convert.ToDateTime(dr["EndDate"]);
            trainingRoomRequest.HeadCount = Convert.ToInt32(dr["HeadCount"]);
            trainingRoomRequest.EquipmentToUse = dr["EquipmentToUse"].ToString();
            trainingRoomRequest.Remarks = dr["Remarks"].ToString();
            trainingRoomRequestList.Add(trainingRoomRequest);
        }

        DataTable trainingRoomDetailDataTable = (DataTable)Session["TrainingRoomDetail"];
        List<TrainingRoomRequestDetail> trainingRoomRequestDetailList = new List<TrainingRoomRequestDetail>();

        foreach (DataRow dr in trainingRoomDetailDataTable.Rows)
        {
            TrainingRoomRequestDetail trainingRoomRequestDetail = new TrainingRoomRequestDetail();
            trainingRoomRequestDetail.CCRequestReferenceNo = lastGeneratedRefNo;
            trainingRoomRequestDetail.PartitionID = Convert.ToInt32(dr["PartitionID"]);
            trainingRoomRequestDetail.Date = Convert.ToDateTime(dr["Date"]);
            trainingRoomRequestDetail.StartDateTime = Convert.ToDateTime(dr["StartDateTime"]);
            trainingRoomRequestDetail.EndDateTime = Convert.ToDateTime(dr["EndDateTime"]);
            trainingRoomRequestDetailList.Add(trainingRoomRequestDetail);
        }

        //Accomodation Room
        DataTable accomodationRoomDataTable = (DataTable)Session["AccomodationRoom"];
        List<AccomodationRoomRequest> accomodationRoomRequestList = new List<AccomodationRoomRequest>();

        foreach (DataRow dr in accomodationRoomDataTable.Rows)
        {
            AccomodationRoomRequest accomodationRoomRequest = new AccomodationRoomRequest();
            accomodationRoomRequest.CCRequestReferenceNo = lastGeneratedRefNo;
            accomodationRoomRequest.AccID = Convert.ToInt32(dr["AccRoomID"]);
            accomodationRoomRequest.StartDate = Convert.ToDateTime(dr["StartDate"]);
            accomodationRoomRequest.EndDate = Convert.ToDateTime(dr["EndDate"]);
            accomodationRoomRequest.HeadCount = Convert.ToInt32(dr["HeadCount"]);
            accomodationRoomRequest.Remarks = dr["Remarks"].ToString();
            accomodationRoomRequestList.Add(accomodationRoomRequest);
        }

        InsertNewCCRequestRequest insertNewCCRequestRequest = new InsertNewCCRequestRequest();
        insertNewCCRequestRequest.CCRequest = request;
        insertNewCCRequestRequest.CCRequestAttachment = attachment;
        insertNewCCRequestRequest.CCRequestHistory = history;
        insertNewCCRequestRequest.TrainingRoomRequestList = trainingRoomRequestList.ToArray();
        insertNewCCRequestRequest.TrainingRoomRequestDetailList = trainingRoomRequestDetailList.ToArray();
        insertNewCCRequestRequest.AccomodationRoomRequestList = accomodationRoomRequestList.ToArray();

        //Submit
        string smessage = "";

        InsertNewCCRequestResult insertNewCCRequestResult = svc.InsertNewCCRequest(insertNewCCRequestRequest);

        if (insertNewCCRequestResult.ResultStatus != iReserveWS.ResultStatus.Successful)
        {
           smessage = insertNewCCRequestResult.Message;
        }
        else
        {
            smessage = "Your reservation request has been successfully submitted.\\nReference Number: " + lastGeneratedRefNo;

            #region Audit Trail

            bool isSuccess = false;

            AuditTrail auditTrail = new AuditTrail();
            auditTrail.ActionDate = DateTime.Now;
            auditTrail.ActionTaken = "Submitted new convention center request";
            auditTrail.ActionDetails = "Reference Number: " + lastGeneratedRefNo + " || Status: Confirmed";
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

        Utilities.MyMessageBox(smessage);
    }

    #endregion
}