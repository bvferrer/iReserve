using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iReserveWS;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.Services.Protocols;

public partial class CalendarConferenceRoomView : System.Web.UI.Page
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
      if (profileName != "Conference Room Administrator")
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

      e.Row.Cells[0].Visible = false;
      e.Row.Cells[1].CssClass = "scheduleCell";

      for (int columnIndex = 2; columnIndex < e.Row.Cells.Count; columnIndex++)
      {
        e.Row.Cells[columnIndex].CssClass = "scheduleCell";

        if (e.Row.Cells[columnIndex].Text == "&nbsp;" || e.Row.Cells[columnIndex].Text == "")
        {
          e.Row.Cells[columnIndex].CssClass = "noSchedule";
          e.Row.Cells[columnIndex].Text = "";

          if (Convert.ToDateTime(arrDates[columnIndex - 2]) < Convert.ToDateTime(DateTime.Now.ToShortDateString()))
          {
            e.Row.Cells[columnIndex].CssClass = "pastdate";
          }
        }
        else
        {
          e.Row.Cells[columnIndex].Attributes["style"] += "cursor:pointer;cursor:hand;";
          e.Row.Cells[columnIndex].Attributes.Add("onclick", String.Format("cellClicked('{0}');", e.Row.Cells[columnIndex].Text));

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

  protected void hiddenButton_Click(object sender, EventArgs e)
  {
    string selectedRefNumber = inhRefNumber.Value;

    refNumberLabel.Text = selectedRefNumber;

    if (inhAction.Value == "CELLCLICKED")
    {
      CRRequest request = new CRRequest();
      request.RequestReferenceNo = selectedRefNumber;

      CRRequest result = new CRRequest();

      try
      {
        result = svc.RetrieveCRRequestDetails(request);
      }

      catch (SoapException ex)
      {
        throw new Exception(Settings.GenericWebServiceMessage);
      }

      roomLabel.Text = result.ConferenceRoom.RoomName;
      dateLabel.Text = result.Date.ToString("MMMM d, yyyy");
      fromLabel.Text = result.StartTime.StartTime12;
      toLabel.Text = result.EndTime.EndTime12;
      agendaLabel.Text = result.Agenda;

      //if (result.CostCenter.CostCenterName != null && result.ChargedCompanyCostCenter.CostCenterName == null)
      //{
      //  divChargedCompany.Visible = false;
      //  spanCompany.InnerText = "Cost Center";
      //  costCenterLabel.Text = result.CostCenter.CostCenterName;
      //}
      //else if (result.CostCenter.CostCenterName != null && result.ChargedCompanyCostCenter.CostCenterName != null)
      //{
      //  divChargedCompany.Visible = true;
      //  divChargedCompanyCostCenter.Visible = true;
      //  chargedCompanyLabel.Text = result.CostCenter.CostCenterName;
      //  costCenterLabel.Text = result.ChargedCompanyCostCenter.CostCenterName;
      //}

      string chargedCompanyCostCenter = result.ChargedCompanyCostCenter.CostCenterName != null ? result.ChargedCompanyCostCenter.CostCenterName : string.Empty;
      chargedCompanyLabel.Text = result.CostCenter.CostCenterName;
      costCenterLabel.Text = chargedCompanyCostCenter;

      requestedByLabel.Text = result.RequestedBy;

      roomDetailsDiv.Style.Add("display", "block");
      roomDetails.Show();
    }
  }

  protected void cancelButton_Click(object sender, EventArgs e)
  {
    roomDetails.Hide();
  }
}