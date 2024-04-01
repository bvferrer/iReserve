using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iReserveWS;
using System.Data;
using System.Web.Services.Protocols;

public partial class AdminHome : System.Web.UI.Page
{
    public static Service svc = new Service();

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
            BindRoomDropdown();
        }

        BindReserveCountRepeater();
        refreshReserveGridView();
        //refreshCancelGridView();
    }

    #region Admin Search

    protected void adminSearchButton_Click(object sender, EventArgs e)
    {
        if (adminSearchTextBox.Text.Trim() == "")
        {
            searchValidationLabel.Text = "Please enter reference number.";
        }
        else
        {
            refreshAdminSearchGridView();
        }
    }

    public void refreshAdminSearchGridView()
    {
        searchValidationLabel.Text = "";

        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("RequestReferenceNo", typeof(string)));
        dt.Columns.Add(new DataColumn("Date", typeof(DateTime)));
        dt.Columns.Add(new DataColumn("StartTime12", typeof(string)));
        dt.Columns.Add(new DataColumn("EndTime12", typeof(string)));
        dt.Columns.Add(new DataColumn("RoomName", typeof(string)));
        dt.Columns.Add(new DataColumn("DateRequested", typeof(DateTime)));
        dt.Columns.Add(new DataColumn("Status", typeof(string)));


        CRRequest request = new CRRequest();
        request.RequestReferenceNo = adminSearchTextBox.Text.Trim();

        CRRequest result = new CRRequest();

        try
        {
            result = svc.RetrieveCRRequestDetails(request);
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericWebServiceMessage);
        }

        if (result.RequestReferenceNo == null)
        {
            searchValidationLabel.Text = "No record with this reference number found.";
        }
        else
        {
            DataRow dr = dt.NewRow();
            dr["RequestReferenceNo"] = result.RequestReferenceNo;
            dr["Date"] = result.Date;
            dr["StartTime12"] = result.StartTime.StartTime12;
            dr["EndTime12"] = result.EndTime.EndTime12;
            dr["RoomName"] = result.ConferenceRoom.RoomName;
            dr["DateRequested"] = result.DateRequested;
            dr["Status"] = result.Status.StatusName;
            dt.Rows.Add(dr);

            adminSearchGridView.DataSource = dt;
            adminSearchGridView.DataBind();
        }
    }

    protected void adminSearchGridView_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "if(this.style.backgroundColor!='#D1DDF1'){this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#b2d2ff';}");
            e.Row.Attributes.Add("onmouseout", "if(this.style.backgroundColor!='#D1DDF1'){this.style.backgroundColor=this.originalstyle;}");
        }
    }

    protected void adminSearchGridView_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["ReferenceNumber"] = adminSearchGridView.SelectedValue.ToString();
        Response.Redirect("AdminRequestDetails.aspx");
    }

    #endregion

    #region For Confirmation Grid View

    private void BindReserveCountRepeater()
    {
        try
        {
            reserveCountDataList.DataSource = svc.RetrieveRequestCountByStatus(StatusCode.ForConfirmation);
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericWebServiceMessage);
        }

        reserveCountDataList.DataBind();
    }

    private void BindRoomDropdown()
    {
        try
        {
            reserveRoomDropDownList.DataSource = svc.RetrieveConferenceRoomRecords(string.Empty, string.Empty, string.Empty);
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericWebServiceMessage);
        }

        reserveRoomDropDownList.DataValueField = "RoomID";
        reserveRoomDropDownList.DataTextField = "RoomName";
        reserveRoomDropDownList.DataBind();
        reserveRoomDropDownList.Items.Insert(0, new ListItem("All", "0"));
        reserveRoomDropDownList.SelectedIndex = 0;
    }

    public void refreshReserveGridView()
    {
        CRRequest request = new CRRequest();
        request.Status = new Status();
        request.Status.StatusCode = StatusCode.ForConfirmation;

        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("RequestReferenceNo", typeof(string)));
        dt.Columns.Add(new DataColumn("Date", typeof(DateTime)));
        dt.Columns.Add(new DataColumn("StartTime12", typeof(string)));
        dt.Columns.Add(new DataColumn("EndTime12", typeof(string)));
        dt.Columns.Add(new DataColumn("RoomName", typeof(string)));
        dt.Columns.Add(new DataColumn("RequestedBy", typeof(string)));

        CRRequest[] requestList;

        try
        {
            requestList = svc.RetrieveCRRequestRecordsByStatus(request);
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericWebServiceMessage);
        }

        foreach (CRRequest record in requestList)
        {
            DataRow dr = dt.NewRow();
            dr["RequestReferenceNo"] = record.RequestReferenceNo;
            dr["Date"] = record.Date;
            dr["StartTime12"] = record.StartTime.StartTime12;
            dr["EndTime12"] = record.EndTime.EndTime12;
            dr["RoomName"] = record.ConferenceRoom.RoomName;
            dr["RequestedBy"] = record.RequestedBy;
            dt.Rows.Add(dr);
        }

        if (reserveRoomDropDownList.SelectedIndex != 0)
        {
            dt.DefaultView.RowFilter = "RoomName = '" + reserveRoomDropDownList.SelectedItem + "'";
        }

        reserveGridView.DataSource = dt;
        reserveGridView.DataBind();

        if (reserveGridView.Rows.Count == 0)
        {
            DataTable dt2 = dt.Clone();
            dt2.Rows.Add(dt2.NewRow());

            reserveGridView.DataSource = dt2;
            reserveGridView.DataBind();

            int columncount = reserveGridView.Rows[0].Cells.Count;
            reserveGridView.Rows[0].Cells.Clear();
            reserveGridView.Rows[0].Cells.Add(new TableCell());
            reserveGridView.Rows[0].Cells[0].ColumnSpan = columncount;
            reserveGridView.Rows[0].Cells[0].Text = "No record found";
        }
    }

    protected void reserveGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;

        if (newPageIndex == -1)
        {
            reserveGridView.PageIndex = e.NewPageIndex + 1;
        }
        else
        {
            reserveGridView.PageIndex = e.NewPageIndex;
        }

        refreshReserveGridView();
    }

    protected void reserveGridView_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "if(this.style.backgroundColor!='#D1DDF1'){this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#b2d2ff';}");
            e.Row.Attributes.Add("onmouseout", "if(this.style.backgroundColor!='#D1DDF1'){this.style.backgroundColor=this.originalstyle;}");
        }
    }

    protected void reserveGridView_DataBound(object sender, EventArgs e)
    {
        GridViewRow gvrPager = reserveGridView.BottomPagerRow;
        if (gvrPager == null) return;

        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

        if (ddlPages != null)
        {
            for (int i = 0; i < reserveGridView.PageCount; i++)
            {
                int intPageNumber = i + 1;
                ListItem lstItem = new ListItem(intPageNumber.ToString());
                if (i == reserveGridView.PageIndex)
                {
                    lstItem.Selected = true;
                }
                ddlPages.Items.Add(lstItem);
            }
        }

        if (lblPageCount != null)
        {
            lblPageCount.Text = reserveGridView.PageCount.ToString();
        }
    }

    protected void ddlPages_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gvrPager = reserveGridView.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");

        reserveGridView.PageIndex = ddlPages.SelectedIndex;
        refreshReserveGridView();
    }

    protected void reserveGridView_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["ReferenceNumber"] = reserveGridView.SelectedValue.ToString();
        Response.Redirect("ConfirmRequestDetails.aspx");
    }

    #endregion

    #region For Cancellation Grid View

    //public void refreshCancelGridView()
    //{
    //    CRRequest request = new CRRequest();
    //    request.Status = new Status();
    //    request.Status.StatusCode = StatusCode.ForCancellation;

    //    DataTable dt = new DataTable();
    //    dt.Columns.Add(new DataColumn("RequestReferenceNo", typeof(string)));
    //    dt.Columns.Add(new DataColumn("Date", typeof(DateTime)));
    //    dt.Columns.Add(new DataColumn("StartTime12", typeof(string)));
    //    dt.Columns.Add(new DataColumn("EndTime12", typeof(string)));
    //    dt.Columns.Add(new DataColumn("RoomName", typeof(string)));
    //    dt.Columns.Add(new DataColumn("RequestedBy", typeof(string)));

    //    CRRequest[] requestList;

    //    try
    //    {
    //        requestList = svc.RetrieveCRRequestRecordsByStatus(request);
    //    }

    //    catch (SoapException ex)
    //    {
    //        throw new Exception(Settings.GenericWebServiceMessage);
    //    }

    //    foreach (CRRequest record in requestList)
    //    {
    //        DataRow dr = dt.NewRow();
    //        dr["RequestReferenceNo"] = record.RequestReferenceNo;
    //        dr["Date"] = record.Date;
    //        dr["StartTime12"] = record.StartTime.StartTime12;
    //        dr["EndTime12"] = record.EndTime.EndTime12;
    //        dr["RoomName"] = record.ConferenceRoom.RoomName;
    //        dr["RequestedBy"] = record.RequestedBy;
    //        dt.Rows.Add(dr);
    //    }

    //    cancelGridView.DataSource = dt;
    //    cancelGridView.DataBind();

    //    if (cancelGridView.Rows.Count == 0)
    //    {
    //        DataTable dt2 = dt.Clone();
    //        dt2.Rows.Add(dt2.NewRow());

    //        cancelGridView.DataSource = dt2;
    //        cancelGridView.DataBind();

    //        int columncount = cancelGridView.Rows[0].Cells.Count;
    //        cancelGridView.Rows[0].Cells.Clear();
    //        cancelGridView.Rows[0].Cells.Add(new TableCell());
    //        cancelGridView.Rows[0].Cells[0].ColumnSpan = columncount;
    //        cancelGridView.Rows[0].Cells[0].Text = "No record found";
    //    }
    //}

    //protected void cancelGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    int newPageIndex = e.NewPageIndex;

    //    if (newPageIndex == -1)
    //    {
    //        cancelGridView.PageIndex = e.NewPageIndex + 1;
    //    }
    //    else
    //    {
    //        cancelGridView.PageIndex = e.NewPageIndex;
    //    }

    //    refreshCancelGridView();
    //}

    //protected void cancelGridView_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        e.Row.Attributes.Add("onmouseover", "if(this.style.backgroundColor!='#D1DDF1'){this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#b2d2ff';}");
    //        e.Row.Attributes.Add("onmouseout", "if(this.style.backgroundColor!='#D1DDF1'){this.style.backgroundColor=this.originalstyle;}");
    //    }
    //}

    //protected void cancelGridView_DataBound(object sender, EventArgs e)
    //{
    //    GridViewRow gvrPager = cancelGridView.BottomPagerRow;
    //    if (gvrPager == null) return;

    //    DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages3");
    //    Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

    //    if (ddlPages != null)
    //    {
    //        for (int i = 0; i < cancelGridView.PageCount; i++)
    //        {
    //            int intPageNumber = i + 1;
    //            ListItem lstItem = new ListItem(intPageNumber.ToString());
    //            if (i == cancelGridView.PageIndex)
    //            {
    //                lstItem.Selected = true;
    //            }
    //            ddlPages.Items.Add(lstItem);
    //        }
    //    }

    //    if (lblPageCount != null)
    //    {
    //        lblPageCount.Text = cancelGridView.PageCount.ToString();
    //    }
    //}

    //protected void ddlPages3_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    GridViewRow gvrPager = cancelGridView.BottomPagerRow;
    //    DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages3");

    //    cancelGridView.PageIndex = ddlPages.SelectedIndex;
    //    refreshCancelGridView();
    //}

    //protected void cancelGridView_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Session["ReferenceNumber"] = cancelGridView.SelectedValue.ToString();
    //    Response.Redirect("CancelRequestDetails.aspx");
    //}

    #endregion

}