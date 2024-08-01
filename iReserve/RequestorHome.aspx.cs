using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iReserveWS;
using System.Data;
using System.Web.Services.Protocols;

public partial class RequestorHome : System.Web.UI.Page
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
            if (profileName != "Requestor")
            {
                Response.Write("<script language=javascript> alert('You are not allowed to access this page. Please click on the Ok Button to go back to the Home Page.'); window.location.href ='Default.aspx';</script>");
            }
        }

        if (!IsPostBack)
        {

        }

        refreshRequestorReserveGridView();
        refreshPendingGridView();
        refreshCCRequestorReserveGridView();
    }

    #region Requestor Search

    protected void requestorSearchButton_Click(object sender, EventArgs e)
    {
        if (requestorSearchTextBox.Text.Trim() == "")
        {
            searchValidationLabel.Text = "Please enter reference number.";
        }
        else
        {
            refreshRequestorSearchGridView();
        }
    }

    public void refreshRequestorSearchGridView()
    {
        searchValidationLabel.Text = "";

        #region Conference Room

        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("RequestReferenceNo", typeof(string)));
        dt.Columns.Add(new DataColumn("Date", typeof(DateTime)));
        dt.Columns.Add(new DataColumn("StartTime12", typeof(string)));
        dt.Columns.Add(new DataColumn("EndTime12", typeof(string)));
        dt.Columns.Add(new DataColumn("RoomName", typeof(string)));
        dt.Columns.Add(new DataColumn("DateRequested", typeof(DateTime)));
        dt.Columns.Add(new DataColumn("Status", typeof(string)));

        CRRequest request = new CRRequest();
        request.RequestReferenceNo = requestorSearchTextBox.Text.Trim();
        request.RequestedByID = Convert.ToString(Session["UserID"]);

        CRRequest result = new CRRequest();

        try
        {
            result = svc.RetrieveCRRequestDetailsByRequestor(request);
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericWebServiceMessage);
        }

        #endregion

        #region Convention Center

        DataTable dt2 = new DataTable();
        dt2.Columns.Add(new DataColumn("CCRequestReferenceNo", typeof(string)));
        dt2.Columns.Add(new DataColumn("EventName", typeof(string)));
        dt2.Columns.Add(new DataColumn("StartDate", typeof(DateTime)));
        dt2.Columns.Add(new DataColumn("EndDate", typeof(DateTime)));
        dt2.Columns.Add(new DataColumn("DateCreated", typeof(DateTime)));
        dt2.Columns.Add(new DataColumn("StatusName", typeof(string)));

        RetrieveCCRequestDetailsByRequestorRequest retrieveCCRequestDetailsByRequestorRequest = new RetrieveCCRequestDetailsByRequestorRequest();
        retrieveCCRequestDetailsByRequestorRequest.CCRequestReferenceNo = requestorSearchTextBox.Text.Trim();
        retrieveCCRequestDetailsByRequestorRequest.CreatedByID = Convert.ToString(Session["UserID"]);

        RetrieveCCRequestDetailsByRequestorResult retrieveCCRequestDetailsByRequestorResult = svc.RetrieveCCRequestDetailsByRequestor(retrieveCCRequestDetailsByRequestorRequest);

        #endregion

        if (result.RequestReferenceNo == null)
        {
            if (retrieveCCRequestDetailsByRequestorResult.CCRequest.CCRequestReferenceNo == null)
            {
                searchValidationLabel.Text = "No record with this reference number found.";
            }
            else
            {
                searchValidationLabel.Text = "";

                DataRow dr = dt2.NewRow();
                dr["CCRequestReferenceNo"] = retrieveCCRequestDetailsByRequestorResult.CCRequest.CCRequestReferenceNo;
                dr["EventName"] = retrieveCCRequestDetailsByRequestorResult.CCRequest.EventName;
                dr["StartDate"] = retrieveCCRequestDetailsByRequestorResult.CCRequest.StartDate;
                dr["EndDate"] = retrieveCCRequestDetailsByRequestorResult.CCRequest.EndDate;
                dr["DateCreated"] = retrieveCCRequestDetailsByRequestorResult.CCRequest.DateCreated;
                dr["StatusName"] = retrieveCCRequestDetailsByRequestorResult.CCRequest.StatusName;
                dt2.Rows.Add(dr);

                ccRequestSearchGridView.DataSource = dt2;
                ccRequestSearchGridView.DataBind();
            }
        }
        else
        {
            searchValidationLabel.Text = "";

            DataRow dr = dt.NewRow();
            dr["RequestReferenceNo"] = result.RequestReferenceNo;
            dr["Date"] = result.Date;
            dr["StartTime12"] = result.StartTime.StartTime12;
            dr["EndTime12"] = result.EndTime.EndTime12;
            dr["RoomName"] = result.ConferenceRoom.RoomName;
            dr["DateRequested"] = result.DateRequested;
            dr["Status"] = result.Status.StatusName;
            dt.Rows.Add(dr);

            requestorSearchGridView.DataSource = dt;
            requestorSearchGridView.DataBind();
        }
    }

    protected void requestorSearchGridView_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "if(this.style.backgroundColor!='#D1DDF1'){this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#b2d2ff';}");
            e.Row.Attributes.Add("onmouseout", "if(this.style.backgroundColor!='#D1DDF1'){this.style.backgroundColor=this.originalstyle;}");
        }
    }

    protected void requestorSearchGridView_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["ReferenceNumber"] = requestorSearchGridView.SelectedValue.ToString();
        Response.Redirect("RequestDetails.aspx");
    }

    protected void ccRequestSearchGridView_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "if(this.style.backgroundColor!='#D1DDF1'){this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#b2d2ff';}");
            e.Row.Attributes.Add("onmouseout", "if(this.style.backgroundColor!='#D1DDF1'){this.style.backgroundColor=this.originalstyle;}");
        }
    }

    protected void ccRequestSearchGridView_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["CCReferenceNumber"] = ccRequestSearchGridView.SelectedValue.ToString();
        Response.Redirect("CCRequestDetails.aspx");
    }

    #endregion

    #region Conference Room

    #region Confirmed Grid View

    public void refreshRequestorReserveGridView()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("RequestReferenceNo", typeof(string)));
        dt.Columns.Add(new DataColumn("Date", typeof(DateTime)));
        dt.Columns.Add(new DataColumn("StartTime12", typeof(string)));
        dt.Columns.Add(new DataColumn("EndTime12", typeof(string)));
        dt.Columns.Add(new DataColumn("RoomName", typeof(string)));
        dt.Columns.Add(new DataColumn("DateRequested", typeof(DateTime)));

        CRRequest request = new CRRequest();
        request.Status = new Status();
        request.Status.StatusCode = StatusCode.Confirmed;
        request.RequestedByID = Convert.ToString(Session["UserID"]);

        CRRequest[] requestList;

        try
        {
            requestList = svc.RetrieveCRRequestRecordsByRequestor(request);
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
            dr["DateRequested"] = record.DateRequested;
            dt.Rows.Add(dr);
        }

        requestorGridView.DataSource = dt;
        requestorGridView.DataBind();

        if (requestorGridView.Rows.Count == 0)
        {
            DataTable dt2 = dt.Clone();
            dt2.Rows.Add(dt2.NewRow());

            requestorGridView.DataSource = dt2;
            requestorGridView.DataBind();

            int columncount = requestorGridView.Rows[0].Cells.Count;
            requestorGridView.Rows[0].Cells.Clear();
            requestorGridView.Rows[0].Cells.Add(new TableCell());
            requestorGridView.Rows[0].Cells[0].ColumnSpan = columncount;
            requestorGridView.Rows[0].Cells[0].Text = "No record found";
        }
    }

    protected void requestorGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;

        if (newPageIndex == -1)
        {
            requestorGridView.PageIndex = e.NewPageIndex + 1;
        }
        else
        {
            requestorGridView.PageIndex = e.NewPageIndex;
        }

        refreshRequestorReserveGridView();
    }

    protected void requestorGridView_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "if(this.style.backgroundColor!='#D1DDF1'){this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#b2d2ff';}");
            e.Row.Attributes.Add("onmouseout", "if(this.style.backgroundColor!='#D1DDF1'){this.style.backgroundColor=this.originalstyle;}");
        }
    }

    protected void requestorGridView_DataBound(object sender, EventArgs e)
    {
        GridViewRow gvrPager = requestorGridView.BottomPagerRow;
        if (gvrPager == null) return;

        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages2");
        Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

        if (ddlPages != null)
        {
            for (int i = 0; i < requestorGridView.PageCount; i++)
            {
                int intPageNumber = i + 1;
                ListItem lstItem = new ListItem(intPageNumber.ToString());
                if (i == requestorGridView.PageIndex)
                {
                    lstItem.Selected = true;
                }
                ddlPages.Items.Add(lstItem);
            }
        }

        if (lblPageCount != null)
        {
            lblPageCount.Text = requestorGridView.PageCount.ToString();
        }
    }

    protected void ddlPages2_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gvrPager = requestorGridView.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages2");

        requestorGridView.PageIndex = ddlPages.SelectedIndex;
        refreshRequestorReserveGridView();
    }

    protected void requestorGridView_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["ReferenceNumber"] = requestorGridView.SelectedValue.ToString();
        Response.Redirect("RequestDetails.aspx");
    }

    #endregion

    #region For Confirmation Grid View

    public void refreshPendingGridView()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("RequestReferenceNo", typeof(string)));
        dt.Columns.Add(new DataColumn("Date", typeof(DateTime)));
        dt.Columns.Add(new DataColumn("StartTime12", typeof(string)));
        dt.Columns.Add(new DataColumn("EndTime12", typeof(string)));
        dt.Columns.Add(new DataColumn("RoomName", typeof(string)));
        dt.Columns.Add(new DataColumn("DateRequested", typeof(DateTime)));

        CRRequest request = new CRRequest();
        request.Status = new Status();
        request.Status.StatusCode = StatusCode.ForConfirmation;
        request.RequestedByID = Convert.ToString(Session["UserID"]);

        CRRequest[] requestList;

        try
        {
            requestList = svc.RetrieveCRRequestRecordsByRequestor(request);
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
            dr["DateRequested"] = record.DateRequested;
            dt.Rows.Add(dr);
        }

        pendingGridView.DataSource = dt;
        pendingGridView.DataBind();

        if (pendingGridView.Rows.Count == 0)
        {
            DataTable dt2 = dt.Clone();
            dt2.Rows.Add(dt2.NewRow());

            pendingGridView.DataSource = dt2;
            pendingGridView.DataBind();

            int columncount = pendingGridView.Rows[0].Cells.Count;
            pendingGridView.Rows[0].Cells.Clear();
            pendingGridView.Rows[0].Cells.Add(new TableCell());
            pendingGridView.Rows[0].Cells[0].ColumnSpan = columncount;
            pendingGridView.Rows[0].Cells[0].Text = "No record found";
        }
    }

    protected void pendingGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;

        if (newPageIndex == -1)
        {
            pendingGridView.PageIndex = e.NewPageIndex + 1;
        }
        else
        {
            pendingGridView.PageIndex = e.NewPageIndex;
        }

        refreshPendingGridView();
    }

    protected void pendingGridView_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "if(this.style.backgroundColor!='#D1DDF1'){this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#b2d2ff';}");
            e.Row.Attributes.Add("onmouseout", "if(this.style.backgroundColor!='#D1DDF1'){this.style.backgroundColor=this.originalstyle;}");
        }
    }

    protected void pendingGridView_DataBound(object sender, EventArgs e)
    {
        GridViewRow gvrPager = pendingGridView.BottomPagerRow;
        if (gvrPager == null) return;

        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages3");
        Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

        if (ddlPages != null)
        {
            for (int i = 0; i < pendingGridView.PageCount; i++)
            {
                int intPageNumber = i + 1;
                ListItem lstItem = new ListItem(intPageNumber.ToString());
                if (i == pendingGridView.PageIndex)
                {
                    lstItem.Selected = true;
                }
                ddlPages.Items.Add(lstItem);
            }
        }

        if (lblPageCount != null)
        {
            lblPageCount.Text = pendingGridView.PageCount.ToString();
        }
    }

    protected void ddlPages3_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gvrPager = pendingGridView.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages3");

        pendingGridView.PageIndex = ddlPages.SelectedIndex;
        refreshPendingGridView();
    }

    protected void pendingGridView_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["ReferenceNumber"] = pendingGridView.SelectedValue.ToString();
        Response.Redirect("RequestDetails.aspx");
    }

    #endregion

    #endregion

    #region Convention Center

    public void refreshCCRequestorReserveGridView()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("CCRequestReferenceNo", typeof(string)));
        dt.Columns.Add(new DataColumn("EventName", typeof(string)));
        dt.Columns.Add(new DataColumn("StartDate", typeof(DateTime)));
        dt.Columns.Add(new DataColumn("EndDate", typeof(DateTime)));
        dt.Columns.Add(new DataColumn("DateCreated", typeof(DateTime)));

        RetrieveCCRequestRecordsByRequestorRequest retrieveCCRequestRecordsByRequestorRequest = new RetrieveCCRequestRecordsByRequestorRequest();
        retrieveCCRequestRecordsByRequestorRequest.StatusCode = StatusCode.Confirmed;
        retrieveCCRequestRecordsByRequestorRequest.CreatedByID = Convert.ToString(Session["UserID"]);

        RetrieveCCRequestRecordsByRequestorResult retrieveCCRequestRecordsByRequestorResult = svc.RetrieveCCRequestRecordsByRequestor(retrieveCCRequestRecordsByRequestorRequest);

        if (retrieveCCRequestRecordsByRequestorResult.ResultStatus != iReserveWS.ResultStatus.Successful)
        {
            Utilities.MyMessageBox(retrieveCCRequestRecordsByRequestorResult.Message);
        }
        else
        {
            ccRequestGridView.DataSource = retrieveCCRequestRecordsByRequestorResult.CCRequestList;
            ccRequestGridView.DataBind();

            if (ccRequestGridView.Rows.Count == 0)
            {
                DataTable dt2 = dt.Clone();
                dt2.Rows.Add(dt2.NewRow());

                ccRequestGridView.DataSource = dt2;
                ccRequestGridView.DataBind();

                int columncount = ccRequestGridView.Rows[0].Cells.Count;
                ccRequestGridView.Rows[0].Cells.Clear();
                ccRequestGridView.Rows[0].Cells.Add(new TableCell());
                ccRequestGridView.Rows[0].Cells[0].ColumnSpan = columncount;
                ccRequestGridView.Rows[0].Cells[0].Text = "No record found";
            }
        }
    }

    protected void ccRequestGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;

        if (newPageIndex == -1)
        {
            ccRequestGridView.PageIndex = e.NewPageIndex + 1;
        }
        else
        {
            ccRequestGridView.PageIndex = e.NewPageIndex;
        }

        refreshCCRequestorReserveGridView();
    }

    protected void ccRequestGridView_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "if(this.style.backgroundColor!='#D1DDF1'){this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#b2d2ff';}");
            e.Row.Attributes.Add("onmouseout", "if(this.style.backgroundColor!='#D1DDF1'){this.style.backgroundColor=this.originalstyle;}");
        }
    }

    protected void ccRequestGridView_DataBound(object sender, EventArgs e)
    {
        GridViewRow gvrPager = ccRequestGridView.BottomPagerRow;
        if (gvrPager == null) return;

        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages4");
        Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

        if (ddlPages != null)
        {
            for (int i = 0; i < ccRequestGridView.PageCount; i++)
            {
                int intPageNumber = i + 1;
                ListItem lstItem = new ListItem(intPageNumber.ToString());
                if (i == ccRequestGridView.PageIndex)
                {
                    lstItem.Selected = true;
                }
                ddlPages.Items.Add(lstItem);
            }
        }

        if (lblPageCount != null)
        {
            lblPageCount.Text = ccRequestGridView.PageCount.ToString();
        }
    }

    protected void ddlPages4_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gvrPager = ccRequestGridView.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages4");

        ccRequestGridView.PageIndex = ddlPages.SelectedIndex;
        refreshRequestorReserveGridView();
    }

    protected void ccRequestGridView_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["CCReferenceNumber"] = ccRequestGridView.SelectedValue.ToString();
        Response.Redirect("CCRequestDetails.aspx");
    }

    #endregion
}