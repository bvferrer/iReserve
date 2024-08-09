﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using iReserveWS;
using System.IO;
using System.Drawing;
using System.Web.Services.Protocols;
using System.ServiceModel.Security.Tokens;

public partial class ReportAccomodationRoom : System.Web.UI.Page
{
  public string dateFrom;
  public string dateTo;
  public string statusCode;
  public string statusName;

  iReserveWS.Service svc = new iReserveWS.Service();

  protected void Page_Load(object sender, EventArgs e)
  {
    Response.ExpiresAbsolute = DateTime.Now.AddDays(-1d);
    Response.Expires = -1500;
    Response.CacheControl = "no-cache";

    if (Convert.ToString(Session["UserID"]) == "")
    {
      Response.Write("<script language=javascript>window.close();</script>");
    }

    string profileName = Convert.ToString(Session["ProfileName"]);

    if (profileName != "")
    {
      if (profileName != "Convention Center Administrator" && profileName != "SOA Approver")
      {
        Response.Write("<script language=javascript> alert('You are not allowed to access this page. Please click on the Ok Button to go back to the Home Page.'); window.location.href ='Default.aspx';</script>");
      }
    }

    dateFrom = Request.QueryString["param1"].ToString();
    dateTo = Request.QueryString["param2"].ToString();
    statusCode = Request.QueryString["param3"].ToString();

    if (statusCode == "A")
    {
      statusName = "All";
    }
    else
    {
      statusName = StatusCode.GetStatusName(Convert.ToInt32(statusCode));
    }

    statusLabel.Text = statusName;
    dateFromLabel.Text = Convert.ToDateTime(dateFrom).ToString("MMMM d, yyyy");
    dateToLabel.Text = Convert.ToDateTime(dateTo).ToString("MMMM d, yyyy");
    LoadReportGridView();
  }

  public void LoadReportGridView()
  {
    DataTable dt = new DataTable();
    dt.Columns.Add(new DataColumn("CCRequestReferenceNo", typeof(string)));
    dt.Columns.Add(new DataColumn("EventName", typeof(string)));
    dt.Columns.Add(new DataColumn("RoomName", typeof(string)));
    dt.Columns.Add(new DataColumn("StartDate", typeof(DateTime)));
    dt.Columns.Add(new DataColumn("EndDate", typeof(DateTime)));
    dt.Columns.Add(new DataColumn("NumberOfNights", typeof(int)));
    dt.Columns.Add(new DataColumn("StatusName", typeof(string)));
    dt.Columns.Add(new DataColumn("ChargedCompany", typeof(string)));
    dt.Columns.Add(new DataColumn("CostCenterName", typeof(string)));
    dt.Columns.Add(new DataColumn("DateCreated", typeof(DateTime)));
    dt.Columns.Add(new DataColumn("DateCancelled", typeof(DateTime)));

    RetrieveAccomodationRoomRequestReportRequest retrieveAccomodationRoomRequestReportRequest = new RetrieveAccomodationRoomRequestReportRequest();
    retrieveAccomodationRoomRequestReportRequest.SelectedStatus = statusCode;
    retrieveAccomodationRoomRequestReportRequest.StartDate = Convert.ToDateTime(dateFrom);
    retrieveAccomodationRoomRequestReportRequest.EndDate = Convert.ToDateTime(dateTo);

    RetrieveAccomodationRoomRequestReportResult retrieveAccomodationRoomRequestReportResult;
    
    try
    {
      retrieveAccomodationRoomRequestReportResult = svc.RetrieveAccomodationRoomRequestReport(retrieveAccomodationRoomRequestReportRequest);
    }
    catch(Exception ex)
    {
      throw new Exception(Settings.GenericWebServiceMessage);
    }

    if (retrieveAccomodationRoomRequestReportResult.ResultStatus != iReserveWS.ResultStatus.Successful)
    {
      Utilities.MyMessageBox(retrieveAccomodationRoomRequestReportResult.Message);
    }
    else
    {

      foreach (GridViewRow row in reportGridView.Rows)
      {
        if (row.Cells[10].Text == "January 1, 0001 12:00 AM")
        {
          row.Cells[10].Text = "";
        }
      }

      foreach (AccomodationRoomRequestReport record in retrieveAccomodationRoomRequestReportResult.AccomodationRoomRequestReportList)
      {
        string costCenter = record.ChargedCompanyCostCenter.CostCenterName;
        DataRow dr = dt.NewRow();
        dr["CCRequestReferenceNo"] = record.CCRequestReferenceNo;
        dr["EventName"] = record.EventName;
        dr["RoomName"] = record.RoomName;
        dr["StartDate"] = record.StartDate;
        dr["EndDate"] = record.EndDate;
        dr["NumberOfNights"] = record.NumberOfNights;
        dr["StatusName"] = record.StatusName;
        dr["ChargedCompany"] = record.CostCenterName;
        dr["CostCenterName"] = costCenter != null ? costCenter : string.Empty;
        dr["DateCreated"] = record.DateCreated;
        dr["DateCancelled"] = record.DateCancelled;

        if (record.DateCancelled == Convert.ToDateTime("1/1/0001"))
        {
          dr["DateCancelled"] = DBNull.Value;
        }

        dt.Rows.Add(dr);
      }

      try
      {
        reportGridView.DataSource = dt;
        reportGridView.DataBind();
      }
      catch(Exception ex)
      {

      }


      if (reportGridView.Rows.Count == 0)
      {
        DataTable dt2 = dt.Clone();
        dt2.Rows.Add(dt2.NewRow());

        reportGridView.DataSource = dt2;
        reportGridView.DataBind();

        int columncount = reportGridView.Rows[0].Cells.Count;
        reportGridView.Rows[0].Cells.Clear();
        reportGridView.Rows[0].Cells.Add(new TableCell());
        reportGridView.Rows[0].Cells[0].ColumnSpan = columncount;
        reportGridView.Rows[0].Cells[0].Text = "No record found";

        exportButton.Visible = false;
      }

      Session["dataSource"] = dt;
    }
  }

  protected void exportButton_Click(object sender, EventArgs e)
  {
    string dateFromQueryString = Request.QueryString["param1"].ToString();
    string dateToQueryString = Request.QueryString["param2"].ToString();
    string statusCodeQueryString = Request.QueryString["param3"].ToString();
    string statusNameQueryString;

    if (statusCodeQueryString == "A")
    {
      statusNameQueryString = "All";
    }
    else
    {
      statusNameQueryString = StatusCode.GetStatusName(Convert.ToInt32(statusCodeQueryString));
    }

    string formattedDateFromParameter = Convert.ToDateTime(dateFromQueryString).ToString("MMMM d, yyyy");
    string formattedDateToParameter = Convert.ToDateTime(dateToQueryString).ToString("MMMM d, yyyy");
    string formattedDate = DateTime.Now.ToString("MMMM d, yyyy");

    Response.ClearContent();
    Response.Buffer = true;
    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "AccomodationRoomReservationReport_" + statusNameQueryString + "_From_" + dateFromQueryString + "_To_" + dateToQueryString + ".xls"));
    Response.ContentType = "application/ms-excel";

    StringWriter stringWriter = new StringWriter();
    HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);

    reportGridView.AllowPaging = false;

    reportGridView.DataSource = Session["dataSource"];
    reportGridView.DataBind();

    //codeDepartmentGridView.DataBind();

    ////This will change the header background color
    //transcriptOfGuestFolioGridView.HeaderRow.Style.Add("background-color", "#507CD1");

    //This will apply style to gridview header cells
    for (int index = 0; index < reportGridView.HeaderRow.Cells.Count; index++)
    {
      reportGridView.HeaderRow.BackColor = Color.White;
      reportGridView.HeaderRow.Cells[index].Style.Add("background-color", "#507CD1");
      //transcriptOfGuestFolioGridView.HeaderRow.Style.Add("background-color", "#507CD1");
    }

    int index2 = 1;
    //This will apply style to alternate rows
    foreach (GridViewRow gridViewRow in reportGridView.Rows)
    {
      gridViewRow.BackColor = Color.White;

      if (index2 <= reportGridView.Rows.Count)
      {
        if (index2 % 2 != 0)
        {
          for (int index3 = 0; index3 < gridViewRow.Cells.Count; index3++)
          {
            gridViewRow.Cells[index3].Style.Add("background-color", "#eff3fb");
          }
        }
        else
        {
          for (int index3 = 0; index3 < gridViewRow.Cells.Count; index3++)
          {
            gridViewRow.Cells[index3].Style.Add("background-color", "White");
          }
        }
      }
      index2++;
    }

    reportGridView.RenderControl(htmlTextWriter);

    string style = @"<style> .textmode { mso-number-format:\@; } </style>";

    string vData = "";

    vData += "<table >";
    vData += "<tr> " +
                "<td colspan='11' align='left' style='vertical-align: text-top;font-size: 8pt; font-family: Tahoma;'> " +
                    "P & EL Realty Corporation</td> " +
             "</tr>";
    vData += "<tr> " +
                "<td colspan='11' align='left' style='vertical-align: text-top;font-size: 10pt; font-family: Tahoma;'> " +
                    "ACCOMODATION ROOM RESERVATION REPORT</td> " +
             "</tr>";
    vData += "<tr> " +
                "<td colspan='11' align='left' style='vertical-align: text-top;font-size: 8pt; font-family: Tahoma;'> " +
                    "&nbsp;</td> " +
             "</tr>";
    vData += "<tr> " +
                "<td align='left' style='vertical-align: text-top;font-size: 8pt; font-family: Tahoma;'> " +
                    "&nbsp;</td> " +
                "<td colspan='10' align='left' style='vertical-align: text-top;font-size: 8pt; font-family: Tahoma;'> " +
                    "STATUS: <u>" + statusNameQueryString + "</u> </td> " +
             "</tr>";
    vData += "<tr> " +
                "<td align='left' style='vertical-align: text-top;font-size: 8pt; font-family: Tahoma;'> " +
                    "&nbsp;</td> " +
                "<td colspan='10' align='left' style='vertical-align: text-top;font-size: 8pt; font-family: Tahoma;'> " +
                    "DATE FROM: <u>" + formattedDateFromParameter + "</u> </td> " +
             "</tr>";
    vData += "<tr> " +
                "<td align='left' style='vertical-align: text-top;font-size: 8pt; font-family: Tahoma;'> " +
                    "&nbsp;</td> " +
                "<td colspan='10' align='left' style='vertical-align: text-top;font-size: 8pt; font-family: Tahoma;'> " +
                    "DATE TO: <u>" + formattedDateToParameter + "</u> </td> " +
             "</tr>";
    vData += "<tr> " +
                "<td colspan='11' align='left' style='vertical-align: text-top;font-size: 8pt; font-family: Tahoma;'> " +
                    "&nbsp;</td> " +
             "</tr>";

    Response.Write(style);
    vData = string.Format("{0}</table>{1}", vData, stringWriter.ToString());
    vData += "<table>";
    vData += "<tr> " +
                "<td colspan='11' align='left' style='vertical-align: text-top;font-size: 8pt; font-family: Tahoma;'> " +
                    "&nbsp;</td> " +
             "</tr>";
    vData += "<tr> " +
                "<td align='left' style='vertical-align: text-top;font-size: 8pt; font-family: Tahoma;'> " +
                    "&nbsp;</td> " +
                "<td colspan='10' align='left' style='vertical-align: text-top;font-size: 8pt; font-family: Tahoma;'> " +
                    "GENERATED BY: <u>" + Session["FirstName"].ToString() + " " + Session["LastName"].ToString() + "</u> </td> " +
             "</tr>";
    vData += "<tr> " +
                "<td align='left' style='vertical-align: text-top;font-size: 8pt; font-family: Tahoma;'> " +
                    "&nbsp;</td> " +
                "<td colspan='10' align='left' style='vertical-align: text-top;font-size: 8pt; font-family: Tahoma;'> " +
                    "DATE GENERATED: <u>" + formattedDate + "</u> </td> " +
             "</tr>";
    vData += "</table>";
    Response.Output.Write(vData);

    Response.Flush();
    Response.End();

    #region Audit Trail

    bool isSuccess = false;

    AuditTrail auditTrail = new AuditTrail();
    auditTrail.ActionDate = DateTime.Now;
    auditTrail.ActionTaken = "Exported accomodation room reservations report";
    auditTrail.ActionDetails = "Status: " + statusNameQueryString + " || Date From: " + formattedDateFromParameter + " || Date To: " + formattedDateToParameter;
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

  public override void VerifyRenderingInServerForm(Control control)
  {
    /* Verifies that the control is rendered */
  }
}