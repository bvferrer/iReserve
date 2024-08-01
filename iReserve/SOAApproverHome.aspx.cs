using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iReserveWS;
using System.Data;
using System.Web.Services.Protocols;

public partial class SOAApproverHome : System.Web.UI.Page
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
            if (profileName != "SOA Approver")
            {
                Response.Write("<script language=javascript> alert('You are not allowed to access this page. Please click on the Ok Button to go back to the Home Page.'); window.location.href ='Default.aspx';</script>");
            }
        }

        if (!IsPostBack)
        {

        }

        RefreshSOAForApprovalGridView();
        RefreshApprovedSOAGridView();
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
            RefreshAdminSearchGridView();
        }
    }

    public void RefreshAdminSearchGridView()
    {
        searchValidationLabel.Text = "";

        RetrieveCCRequestDetailsRequest retrieveCCRequestDetailsRequest = new RetrieveCCRequestDetailsRequest();
        retrieveCCRequestDetailsRequest.CCRequestReferenceNo = adminSearchTextBox.Text.Trim();

        RetrieveCCRequestDetailsResult retrieveCCRequestDetailsResult = svc.RetrieveCCRequestDetails(retrieveCCRequestDetailsRequest);

        if (retrieveCCRequestDetailsResult.CCRequest.CCRequestReferenceNo == null)
        {
            searchValidationLabel.Text = "No record with this reference number found.";
        }
        else
        {
            searchValidationLabel.Text = "";

            DataTable dt2 = new DataTable();
            dt2.Columns.Add(new DataColumn("CCRequestReferenceNo", typeof(string)));
            dt2.Columns.Add(new DataColumn("EventName", typeof(string)));
            dt2.Columns.Add(new DataColumn("StartDate", typeof(DateTime)));
            dt2.Columns.Add(new DataColumn("EndDate", typeof(DateTime)));
            dt2.Columns.Add(new DataColumn("DateCreated", typeof(DateTime)));
            dt2.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
            dt2.Columns.Add(new DataColumn("CostCenterName", typeof(string)));
            dt2.Columns.Add(new DataColumn("SOAStatusName", typeof(string)));

            DataRow dr = dt2.NewRow();
            dr["CCRequestReferenceNo"] = retrieveCCRequestDetailsResult.CCRequest.CCRequestReferenceNo;
            dr["EventName"] = retrieveCCRequestDetailsResult.CCRequest.EventName;
            dr["StartDate"] = retrieveCCRequestDetailsResult.CCRequest.StartDate;
            dr["EndDate"] = retrieveCCRequestDetailsResult.CCRequest.EndDate;
            dr["DateCreated"] = retrieveCCRequestDetailsResult.CCRequest.DateCreated;
            dr["CreatedBy"] = retrieveCCRequestDetailsResult.CCRequest.CreatedBy;
            dr["CostCenterName"] = retrieveCCRequestDetailsResult.CCRequest.CostCenterName;
            dr["SOAStatusName"] = retrieveCCRequestDetailsResult.CCRequest.SOAStatusName;
            dt2.Rows.Add(dr);

            adminSearchGridView.DataSource = dt2;
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
        Session["CCReferenceNumber"] = adminSearchGridView.SelectedValue.ToString();
        Response.Redirect("SOAApproverDetails.aspx");
    }

    #endregion

    #region For SOA Processing

    public void RefreshSOAForApprovalGridView()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("CCRequestReferenceNo", typeof(string)));
        dt.Columns.Add(new DataColumn("EventName", typeof(string)));
        dt.Columns.Add(new DataColumn("StartDate", typeof(DateTime)));
        dt.Columns.Add(new DataColumn("EndDate", typeof(DateTime)));
        dt.Columns.Add(new DataColumn("DateCreated", typeof(DateTime)));
        dt.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
        dt.Columns.Add(new DataColumn("CostCenterName", typeof(string)));

        RetrieveCCRequestRecordsBySOAStatusRequest retrieveCCRequestRecordsBySOAStatusRequest = new RetrieveCCRequestRecordsBySOAStatusRequest();
        retrieveCCRequestRecordsBySOAStatusRequest.SOAStatusCode = SOAStatusCode.ForApproval;

        RetrieveCCRequestRecordsBySOAStatusResult retrieveCCRequestRecordsBySOAStatusResult = svc.RetrieveCCRequestRecordsBySOAStatus(retrieveCCRequestRecordsBySOAStatusRequest);

        if (retrieveCCRequestRecordsBySOAStatusResult.ResultStatus != iReserveWS.ResultStatus.Successful)
        {
            Utilities.MyMessageBox(retrieveCCRequestRecordsBySOAStatusResult.Message);
        }
        else
        {
            soaForApprovalGridView.DataSource = retrieveCCRequestRecordsBySOAStatusResult.CCRequestList;
            soaForApprovalGridView.DataBind();

            if (soaForApprovalGridView.Rows.Count == 0)
            {
                DataTable dt2 = dt.Clone();
                dt2.Rows.Add(dt2.NewRow());

                soaForApprovalGridView.DataSource = dt2;
                soaForApprovalGridView.DataBind();

                int columncount = soaForApprovalGridView.Rows[0].Cells.Count;
                soaForApprovalGridView.Rows[0].Cells.Clear();
                soaForApprovalGridView.Rows[0].Cells.Add(new TableCell());
                soaForApprovalGridView.Rows[0].Cells[0].ColumnSpan = columncount;
                soaForApprovalGridView.Rows[0].Cells[0].Text = "No record found";
            }
        }
    }

    protected void soaForApprovalGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;

        if (newPageIndex == -1)
        {
            soaForApprovalGridView.PageIndex = e.NewPageIndex + 1;
        }
        else
        {
            soaForApprovalGridView.PageIndex = e.NewPageIndex;
        }

        RefreshSOAForApprovalGridView();
    }

    protected void soaForApprovalGridView_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "if(this.style.backgroundColor!='#D1DDF1'){this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#b2d2ff';}");
            e.Row.Attributes.Add("onmouseout", "if(this.style.backgroundColor!='#D1DDF1'){this.style.backgroundColor=this.originalstyle;}");
        }
    }

    protected void soaForApprovalGridView_DataBound(object sender, EventArgs e)
    {
        GridViewRow gvrPager = soaForApprovalGridView.BottomPagerRow;
        if (gvrPager == null) return;

        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages5");
        Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

        if (ddlPages != null)
        {
            for (int i = 0; i < soaForApprovalGridView.PageCount; i++)
            {
                int intPageNumber = i + 1;
                ListItem lstItem = new ListItem(intPageNumber.ToString());
                if (i == soaForApprovalGridView.PageIndex)
                {
                    lstItem.Selected = true;
                }
                ddlPages.Items.Add(lstItem);
            }
        }

        if (lblPageCount != null)
        {
            lblPageCount.Text = soaForApprovalGridView.PageCount.ToString();
        }
    }

    protected void ddlPages5_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gvrPager = soaForApprovalGridView.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages5");

        soaForApprovalGridView.PageIndex = ddlPages.SelectedIndex;
        RefreshSOAForApprovalGridView();
    }

    protected void soaForApprovalGridView_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["CCReferenceNumber"] = soaForApprovalGridView.SelectedValue.ToString();
        Response.Redirect("SOAApproverDetails.aspx");
    }

    #endregion

    #region Approved SOA

    public void RefreshApprovedSOAGridView()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("CCRequestReferenceNo", typeof(string)));
        dt.Columns.Add(new DataColumn("EventName", typeof(string)));
        dt.Columns.Add(new DataColumn("StartDate", typeof(DateTime)));
        dt.Columns.Add(new DataColumn("EndDate", typeof(DateTime)));
        dt.Columns.Add(new DataColumn("DateCreated", typeof(DateTime)));
        dt.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
        dt.Columns.Add(new DataColumn("CostCenterName", typeof(string)));

        RetrieveCCRequestRecordsApprovedSOARequest retrieveCCRequestRecordsApprovedSOARequest = new RetrieveCCRequestRecordsApprovedSOARequest();
        RetrieveCCRequestRecordsApprovedSOAResult retrieveCCRequestRecordsApprovedSOAResult = svc.RetrieveCCRequestRecordsApprovedSOA(retrieveCCRequestRecordsApprovedSOARequest);

        if (retrieveCCRequestRecordsApprovedSOAResult.ResultStatus != iReserveWS.ResultStatus.Successful)
        {
            Utilities.MyMessageBox(retrieveCCRequestRecordsApprovedSOAResult.Message);
        }
        else
        {
            approvedSOAGridView.DataSource = retrieveCCRequestRecordsApprovedSOAResult.CCRequestList;
            approvedSOAGridView.DataBind();

            if (approvedSOAGridView.Rows.Count == 0)
            {
                DataTable dt2 = dt.Clone();
                dt2.Rows.Add(dt2.NewRow());

                approvedSOAGridView.DataSource = dt2;
                approvedSOAGridView.DataBind();

                int columncount = approvedSOAGridView.Rows[0].Cells.Count;
                approvedSOAGridView.Rows[0].Cells.Clear();
                approvedSOAGridView.Rows[0].Cells.Add(new TableCell());
                approvedSOAGridView.Rows[0].Cells[0].ColumnSpan = columncount;
                approvedSOAGridView.Rows[0].Cells[0].Text = "No record found";
            }
        }
    }

    protected void approvedSOAGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;

        if (newPageIndex == -1)
        {
            approvedSOAGridView.PageIndex = e.NewPageIndex + 1;
        }
        else
        {
            approvedSOAGridView.PageIndex = e.NewPageIndex;
        }

        RefreshApprovedSOAGridView();
    }

    protected void approvedSOAGridView_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "if(this.style.backgroundColor!='#D1DDF1'){this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#b2d2ff';}");
            e.Row.Attributes.Add("onmouseout", "if(this.style.backgroundColor!='#D1DDF1'){this.style.backgroundColor=this.originalstyle;}");
        }
    }

    protected void approvedSOAGridView_DataBound(object sender, EventArgs e)
    {
        GridViewRow gvrPager = approvedSOAGridView.BottomPagerRow;
        if (gvrPager == null) return;

        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages5");
        Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

        if (ddlPages != null)
        {
            for (int i = 0; i < approvedSOAGridView.PageCount; i++)
            {
                int intPageNumber = i + 1;
                ListItem lstItem = new ListItem(intPageNumber.ToString());
                if (i == approvedSOAGridView.PageIndex)
                {
                    lstItem.Selected = true;
                }
                ddlPages.Items.Add(lstItem);
            }
        }

        if (lblPageCount != null)
        {
            lblPageCount.Text = approvedSOAGridView.PageCount.ToString();
        }
    }

    protected void ddlPages6_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gvrPager = approvedSOAGridView.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages6");

        approvedSOAGridView.PageIndex = ddlPages.SelectedIndex;
        RefreshApprovedSOAGridView();
    }

    protected void approvedSOAGridView_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["CCReferenceNumber"] = approvedSOAGridView.SelectedValue.ToString();
        Response.Redirect("SOAApproverDetails.aspx");
    }

    #endregion
}