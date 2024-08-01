using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iReserveWS;
using System.Data;
using System.Web.Services.Protocols;

public partial class CalendarConferenceRoom : System.Web.UI.Page
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
        BindConferenceRoomDropdown();
        BindCalendarHeader(datepicker.Text);
        BindCalendarGridView();

        countTextBox.Text = "0";
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("Name", typeof(string)));
        dt.Columns.Add(new DataColumn("Company", typeof(string)));
        Session["Attendees"] = dt;
    }

    private void ClearControls()
    {
        agendaTextBox.Text = "";
        nameTextBox.Text = "";
        companyTextBox.Text = "";
        dataPortCheckBox.Checked = false;
        monitorCheckBox.Checked = false;
        remarksTextBox.Text = "";

        DataTable dt = new DataTable();
        dt = (DataTable)Session["Attendees"];
        dt.Rows.Clear();

        attendeeGridView.DataSource = dt;
        attendeeGridView.DataBind();

        Session["Attendees"] = dt;

        countTextBox.Text = (attendeeGridView.Rows.Count).ToString();

        approvalLabel.Text = "";
        scheduleValidationLabel.Text = "";
    }

    private void BindConferenceRoomDropdown()
    {
        try
        {
            conferenceRoomDropDownList.DataSource = svc.RetrieveConferenceRoomRecords(string.Empty, string.Empty, string.Empty);
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericWebServiceMessage);
        }

        conferenceRoomDropDownList.DataValueField = "RoomID";
        conferenceRoomDropDownList.DataTextField = "RoomName";
        conferenceRoomDropDownList.DataBind();
        conferenceRoomDropDownList.SelectedIndex = 0;
    }

    private void BindCalendarHeader(string date)
    {
        string strDates = CalendarUtilities.GetDateRange(date);
        string[] arrDates = strDates.Split(',');

        Session["previous20"] = Convert.ToDateTime(arrDates[0]).AddDays(-20).ToString("MM/dd/yyyy");
        Session["next20"] = Convert.ToDateTime(arrDates[arrDates.Length - 1]).AddDays(1).ToString("MM/dd/yyyy");

        CalendarUtilities.LoadInventoryHeader(date, arrDates[arrDates.Length - 1].ToString(), calendarHeaderTable);
    }

    private void BindCalendarGridView()
    {
        try
        {
            string dateFrom = datepicker.Text;

            try
            {
                if (string.IsNullOrEmpty(dateFrom))
                {
                    scheduleGridview.DataSource = svc.RetrieveConferenceRoomSchedules(DateTime.Now.ToString("MM/dd/yyyy"), Convert.ToInt32(conferenceRoomDropDownList.SelectedValue));
                }

                else
                {
                    scheduleGridview.DataSource = svc.RetrieveConferenceRoomSchedules(dateFrom, Convert.ToInt32(conferenceRoomDropDownList.SelectedValue));
                }
            }

            catch (SoapException ex)
            {
                throw new Exception(Settings.GenericWebServiceMessage);
            }

            scheduleGridview.DataBind();
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericWebServiceMessage);
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

    protected void conferenceRoomDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCalendarHeader(datepicker.Text);
        BindCalendarGridView();
    }

    protected void scheduleGridview_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string strDates = CalendarUtilities.GetDateRange(datepicker.Text);
            string[] arrDates = strDates.Split(',');

            string onmouseoverStyle = "this.style.backgroundColor='#FFFED1'";
            string onmouseoutStyle = "this.style.backgroundColor='white'";

            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].CssClass = "scheduleCell";

            for (int columnIndex = 2; columnIndex < e.Row.Cells.Count; columnIndex++)
            {
                e.Row.Cells[columnIndex].CssClass = "scheduleCell";

                if (e.Row.Cells[columnIndex].Text == "&nbsp;" || e.Row.Cells[columnIndex].Text == "")
                {
                    e.Row.Cells[columnIndex].CssClass = "noSchedule";
                    e.Row.Cells[columnIndex].Text = "";

                    if (Convert.ToDateTime(arrDates[columnIndex - 2]) == Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                    {
                        CRTimeSlot request = new CRTimeSlot();
                        request.TimeSlotID = Convert.ToInt32(e.Row.Cells[0].Text);

                        CRTimeSlot result = new CRTimeSlot();

                        try
                        {
                            result = svc.RetrieveCRTimeSlotDetails(request);
                        }

                        catch (SoapException ex)
                        {
                            throw new Exception(Settings.GenericWebServiceMessage);
                        }

                        if (TimeSpan.Parse(result.StartTime) > DateTime.Now.TimeOfDay)
                        {
                            e.Row.Cells[columnIndex].Attributes["style"] += "cursor:pointer;cursor:hand;";
                            e.Row.Cells[columnIndex].Attributes.Add("onclick", String.Format("cellClicked({0},{1});", e.Row.RowIndex, columnIndex));
                            e.Row.Cells[columnIndex].Attributes.Add("onmouseover", onmouseoverStyle);
                            e.Row.Cells[columnIndex].Attributes.Add("onmouseout", onmouseoutStyle);
                        }
                        else
                        {
                            e.Row.Cells[columnIndex].CssClass = "pastdate";
                        }
                    }
                    else if ((Convert.ToDateTime(arrDates[columnIndex - 2]) > Convert.ToDateTime(DateTime.Now.ToShortDateString())) && (Convert.ToDateTime(arrDates[columnIndex - 2]) < Convert.ToDateTime(DateTime.Now.AddYears(1).ToShortDateString())))
                    {
                        e.Row.Cells[columnIndex].Attributes["style"] += "cursor:pointer;cursor:hand;";
                        e.Row.Cells[columnIndex].Attributes.Add("onclick", String.Format("cellClicked({0},{1});", e.Row.RowIndex, columnIndex));
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

                    CRRequest request = new CRRequest();
                    request.RequestReferenceNo = e.Row.Cells[columnIndex].Text;

                    CRRequest result = new CRRequest();

                    try
                    {
                        result = svc.RetrieveCRRequestDetails(request);
                    }

                    catch (SoapException ex)
                    {
                        throw new Exception(Settings.GenericWebServiceMessage);
                    }

                    if (Convert.ToBoolean(result.HasLoggedIn))
                    {
                        e.Row.Cells[columnIndex].CssClass = "timedIn";
                    }

                    e.Row.Cells[columnIndex].Text = "";
                }
            }
        }
    }

    private void BindStartEndTime(string selectedTimeSlotID)
    {
        CRTimeSlot[] timeSlotList;

        try
        {
            timeSlotList = svc.RetrieveTimeSlots();
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericWebServiceMessage);
        }

        fromDropDownList.DataSource = timeSlotList;
        fromDropDownList.DataValueField = "TimeSlotID";
        fromDropDownList.DataTextField = "StartTime12";
        fromDropDownList.DataBind();
        fromDropDownList.SelectedValue = selectedTimeSlotID;

        toDropDownList.DataSource = timeSlotList;
        toDropDownList.DataValueField = "TimeSlotID";
        toDropDownList.DataTextField = "EndTime12";
        toDropDownList.DataBind();
        toDropDownList.SelectedValue = selectedTimeSlotID;

        if (Convert.ToDateTime(dateLabel.Text) == Convert.ToDateTime(DateTime.Now.ToShortDateString()))
        {
            foreach (CRTimeSlot timeSlot in timeSlotList)
            {
                if (TimeSpan.Parse(timeSlot.StartTime) < DateTime.Now.TimeOfDay)
                {
                    fromDropDownList.Items.Remove(fromDropDownList.Items.FindByText(timeSlot.StartTime12));
                    toDropDownList.Items.Remove(toDropDownList.Items.FindByText(timeSlot.EndTime12));
                }
            }
        }
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

    protected void hiddenButton_Click(object sender, EventArgs e)
    {
        int selectedRowIndex = Convert.ToInt32(inhRow.Value);
        int selectedColumnIndex = Convert.ToInt32(inhColumn.Value);

        string date = scheduleGridview.HeaderRow.Cells[selectedColumnIndex].Text;
        int roomID = Convert.ToInt32(conferenceRoomDropDownList.SelectedValue);
        string timeSlotID = scheduleGridview.Rows[selectedRowIndex].Cells[0].Text;

        if (inhAction.Value == "CELLCLICKED")
        {
            MaintenanceConferenceRoomList room = new MaintenanceConferenceRoomList();
            
            try
            {
                room = svc.RetrieveConferenceRoomRecordDetails(Convert.ToInt32(conferenceRoomDropDownList.SelectedValue));
            }

            catch (SoapException ex)
            {
                throw new Exception(Settings.GenericWebServiceMessage);
            }

            roomNameLabel.Text = room.RoomName.ToUpper();
            dateLabel.Text = Convert.ToDateTime(date).ToString("MMM d, yyyy");
            locationLabel.Text = room.LocationName;
            maxPersonLabel.Text = room.MaxPerson;
            detailsLabel.Text = room.RoomDesc;
            dataPortCheckBox.Enabled = Convert.ToBoolean(room.IsDataPortAvailable);
            monitorCheckBox.Enabled = Convert.ToBoolean(room.IsMonitorAvailable);

            BindStartEndTime(timeSlotID);
            BindCostCenter();

            roomDetailsDiv.Style.Add("display", "block");
            roomDetails.Show();

            maxPersonTextBox.Text = room.MaxPerson;
        }
    }

    protected void fromDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        toDropDownList.SelectedValue = fromDropDownList.SelectedValue;
    }

    protected void addButton_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["Attendees"];

        DataRow dr = dt.NewRow();
        dr["Name"] = nameTextBox.Text.Trim();
        dr["Company"] = companyTextBox.Text.Trim();
        dt.Rows.Add(dr);

        attendeeGridView.DataSource = dt;
        attendeeGridView.DataBind();

        Session["Attendees"] = dt;

        countTextBox.Text = (attendeeGridView.Rows.Count).ToString();
        nameTextBox.Text = "";
        companyTextBox.Text = "";
        roomDetails.Show();
    }

    protected void attendeeGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["Attendees"];
        dt.Rows[e.RowIndex].Delete();

        attendeeGridView.DataSource = dt;
        attendeeGridView.DataBind();

        Session["Attendees"] = dt;

        countTextBox.Text = (attendeeGridView.Rows.Count).ToString();
        roomDetails.Show();
    }

    protected void cancelButton_Click(object sender, EventArgs e)
    {
        roomDetails.Hide();
        ClearControls();
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
            roomDetails.Show();
        }
        else
        {
            if (fileSizeActual > fileSizeLimit)
            {
                strMessage = "Allowed attachment file size is up to 3MB only.";
                approvalLabel.Text = strMessage;
                roomDetails.Show();
            }
            else
            {
                approvalLabel.Text = "";

                if (!IsScheduleAvailable())
                {
                    strMessage = "The selected schedule has a conflict with another reservation. Kindly check Reservation Calendar Page.";
                    scheduleValidationLabel.Text = strMessage;
                    roomDetails.Show();
                }
                else
                {
                    Submit();

                    scheduleValidationLabel.Text = "";
                    ClearControls();
                }
            }
        }
    }

    private bool IsScheduleAvailable()
    {
        CRRequest request = new CRRequest();
        request.ConferenceRoom = new ConferenceRoom();
        request.ConferenceRoom.RoomID = Convert.ToInt32(conferenceRoomDropDownList.SelectedValue);
        request.Date = Convert.ToDateTime(dateLabel.Text.Trim());
        request.StartTime = new CRTimeSlot();
        request.StartTime.TimeSlotID = Convert.ToInt32(fromDropDownList.SelectedValue);
        request.EndTime = new CRTimeSlot();
        request.EndTime.TimeSlotID = Convert.ToInt32(toDropDownList.SelectedValue);

        try
        {
            return svc.ValidateScheduleAvailability(request);
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericWebServiceMessage);
        }
    }

    private void Submit()
    {
        //Generate reference number
        CRLastGeneratedReferenceNumber lastRefNo = new CRLastGeneratedReferenceNumber();

        try
        {
            lastRefNo = svc.GetLastGeneratedReferenceNumber(DateTime.Today);
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericWebServiceMessage);
        }

        string lastGeneratedRefNo = lastRefNo.LastReferenceNumber;

        if (Application["LastGeneratedCRRF"] == null)
        {
            Application["LastGeneratedCRRF"] = lastGeneratedRefNo;
        }

        //Insert new request
        //Request Details
        CRRequest request = new CRRequest();
        request.RequestReferenceNo = lastGeneratedRefNo;
        request.ConferenceRoom = new ConferenceRoom();
        request.ConferenceRoom.RoomID = Convert.ToInt32(conferenceRoomDropDownList.SelectedValue);
        request.Date = Convert.ToDateTime(dateLabel.Text.Trim());
        request.StartTime = new CRTimeSlot();
        request.StartTime.TimeSlotID = Convert.ToInt32(fromDropDownList.SelectedValue);
        request.EndTime = new CRTimeSlot();
        request.EndTime.TimeSlotID = Convert.ToInt32(toDropDownList.SelectedValue);
        request.Agenda = agendaTextBox.Text.Trim();
        request.HeadCount = Convert.ToInt32(countTextBox.Text);
        request.IsUseDataPort = dataPortCheckBox.Checked.ToString();
        request.IsUseMonitor = monitorCheckBox.Checked.ToString();
        request.CostCenter = new CostCenter();
        request.CostCenter.CostCenterID = Convert.ToInt32(costCenterDropDownList.SelectedValue);
        request.Status = new Status();
        request.Status.StatusCode = StatusCode.ForConfirmation;
        request.RequestedByID = Session["UserID"].ToString();
        request.RequestedBy = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();

        //Attachment
        CRRequestAttachment attachment = new CRRequestAttachment();
        attachment.RequestReferenceNo = lastGeneratedRefNo;
        attachment.Status = new Status();
        attachment.Status.StatusCode = StatusCode.ForConfirmation;
        attachment.FileName = approvalFileUpload.FileName.Replace(" ", "_");
        attachment.FileType = approvalFileUpload.PostedFile.ContentType;
        attachment.FileSize = approvalFileUpload.PostedFile.ContentLength;
        attachment.File = approvalFileUpload.FileBytes;
        request.Attachment = attachment;

        //Attendees
        DataTable attendeesDataTable = (DataTable)Session["Attendees"];
        List<CRRequestAttendee> attendeeList = new List<CRRequestAttendee>();
        
        foreach (DataRow dr in attendeesDataTable.Rows)
        {
            CRRequestAttendee attendee = new CRRequestAttendee();
            attendee.RequestReferenceNumber = lastGeneratedRefNo;
            attendee.FullName = dr["Name"].ToString();
            attendee.Company = dr["Company"].ToString();

            attendeeList.Add(attendee);
        }

        request.AttendeeList = attendeeList.ToArray();

        //History
        CRRequestHistory history = new CRRequestHistory();
        history.RequestReferenceNumber = lastGeneratedRefNo;
        history.Status = new Status();
        history.Status.StatusCode = StatusCode.ForConfirmation;
        history.ProcessedByID = Session["UserID"].ToString();
        history.ProcessedBy = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();
        history.Remarks = remarksTextBox.Text.Trim();
        request.RequestHistory = history;

        //Submit
        string smessage = "";

        Result result = svc.InsertNewRequest(request);

        if (result.ResultStatus == iReserveWS.ResultStatus.Successful)
        {
            smessage = "Your reservation request has been successfully submitted.\\nReference Number: " + lastGeneratedRefNo;

            #region Audit Trail

            bool isSuccess = false;

            AuditTrail auditTrail = new AuditTrail();
            auditTrail.ActionDate = DateTime.Now;
            auditTrail.ActionTaken = "Submitted new request";
            auditTrail.ActionDetails = "Reference Number: " + lastGeneratedRefNo + " || Status: For Confirmation";
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

        Utilities.MyMessageBox(smessage);
    }
}