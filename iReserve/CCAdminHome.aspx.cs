using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iReserveWS;
using System.Data;
using System.Web.Services.Protocols;

public partial class CCAdminHome : System.Web.UI.Page
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
      if (profileName != "Convention Center Administrator")
      {
        Response.Write("<script language=javascript> alert('You are not allowed to access this page. Please click on the Ok Button to go back to the Home Page.'); window.location.href ='Default.aspx';</script>");
      }
    }

    if (!IsPostBack)
    {

    }

    RefreshCCAdminConfirmedGridView();
    RefreshForSOAProcessingGridView();
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
      dt2.Columns.Add(new DataColumn("ChargedCompany", typeof(string)));
      dt2.Columns.Add(new DataColumn("CostCenterName", typeof(string)));
      dt2.Columns.Add(new DataColumn("StatusName", typeof(string)));

      DataRow dr = dt2.NewRow();
      dr["CCRequestReferenceNo"] = retrieveCCRequestDetailsResult.CCRequest.CCRequestReferenceNo;
      dr["EventName"] = retrieveCCRequestDetailsResult.CCRequest.EventName;
      dr["StartDate"] = retrieveCCRequestDetailsResult.CCRequest.StartDate;
      dr["EndDate"] = retrieveCCRequestDetailsResult.CCRequest.EndDate;
      dr["DateCreated"] = retrieveCCRequestDetailsResult.CCRequest.DateCreated;
      dr["CreatedBy"] = retrieveCCRequestDetailsResult.CCRequest.CreatedBy;
      dr["ChargedCompany"] = retrieveCCRequestDetailsResult.CCRequest.CostCenterName;

      RetrieveCCRequestDetailsResult res = retrieveCCRequestDetailsResult;
      string costCenter = res.CCRequest.ChargedCompanyCostCenter.CostCenterName != null ? res.CCRequest.ChargedCompanyCostCenter.CostCenterName : string.Empty;

      dr["CostCenterName"] = costCenter;
      dr["StatusName"] = retrieveCCRequestDetailsResult.CCRequest.StatusName;
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
    Response.Redirect("CCAdminRequestDetails.aspx");
  }

  #endregion

  #region Confirmed Requests

  public void RefreshCCAdminConfirmedGridView()
  {
    DataTable dt = new DataTable();
    dt.Columns.Add(new DataColumn("CCRequestReferenceNo", typeof(string)));
    dt.Columns.Add(new DataColumn("EventName", typeof(string)));
    dt.Columns.Add(new DataColumn("StartDate", typeof(DateTime)));
    dt.Columns.Add(new DataColumn("EndDate", typeof(DateTime)));
    dt.Columns.Add(new DataColumn("DateCreated", typeof(DateTime)));
    dt.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
    dt.Columns.Add(new DataColumn("ChargedCompany", typeof(string)));
    dt.Columns.Add(new DataColumn("CostCenterName", typeof(string)));

    RetrieveCCRequestRecordsByStatusRequest retrieveCCRequestRecordsByStatusRequest = new RetrieveCCRequestRecordsByStatusRequest();
    retrieveCCRequestRecordsByStatusRequest.StatusCode = StatusCode.Confirmed;

    RetrieveCCRequestRecordsByStatusResult retrieveCCRequestRecordsByStatusResult;

    try
    {
      retrieveCCRequestRecordsByStatusResult = svc.RetrieveCCRequestRecordsByStatus(retrieveCCRequestRecordsByStatusRequest);
    }
    catch (Exception ex)
    {
      throw new Exception(Settings.GenericWebServiceMessage);
    }



    if (retrieveCCRequestRecordsByStatusResult.ResultStatus != iReserveWS.ResultStatus.Successful)
    {
      Utilities.MyMessageBox(retrieveCCRequestRecordsByStatusResult.Message);
    }
    else
    {

      foreach (CCRequest record in retrieveCCRequestRecordsByStatusResult.CCRequestList)
      {
        DataRow dr = dt.NewRow();
        dr["CCRequestReferenceNo"] = record.CCRequestReferenceNo;
        dr["EventName"] = record.EventName;
        dr["StartDate"] = record.StartDate;
        dr["EndDate"] = record.EndDate;
        dr["DateCreated"] = record.DateCreated;
        dr["CreatedBy"] = record.CreatedBy;
        dr["ChargedCompany"] = record.CostCenterName;
        string costCenter = record.ChargedCompanyCostCenter.CostCenterName != null ? record.ChargedCompanyCostCenter.CostCenterName : string.Empty;
        dr["CostCenterName"] = costCenter;
        dt.Rows.Add(dr);
      }


      ccAdminConfirmedGridView.DataSource = dt;
      ccAdminConfirmedGridView.DataBind();

      if (ccAdminConfirmedGridView.Rows.Count == 0)
      {

        DataTable dt2 = dt.Clone();
        dt2.Rows.Add(dt2.NewRow());

        ccAdminConfirmedGridView.DataSource = dt2;
        ccAdminConfirmedGridView.DataBind();

        int columncount = ccAdminConfirmedGridView.Rows[0].Cells.Count;
        ccAdminConfirmedGridView.Rows[0].Cells.Clear();
        ccAdminConfirmedGridView.Rows[0].Cells.Add(new TableCell());
        ccAdminConfirmedGridView.Rows[0].Cells[0].ColumnSpan = columncount;
        ccAdminConfirmedGridView.Rows[0].Cells[0].Text = "No record found";
      }
    }
  }

  protected void ccAdminConfirmedGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
  {
    int newPageIndex = e.NewPageIndex;

    if (newPageIndex == -1)
    {
      ccAdminConfirmedGridView.PageIndex = e.NewPageIndex + 1;
    }
    else
    {
      ccAdminConfirmedGridView.PageIndex = e.NewPageIndex;
    }

    RefreshCCAdminConfirmedGridView();
  }

  protected void ccAdminConfirmedGridView_RowCreated(object sender, GridViewRowEventArgs e)
  {
    if (e.Row.RowType == DataControlRowType.DataRow)
    {
      e.Row.Attributes.Add("onmouseover", "if(this.style.backgroundColor!='#D1DDF1'){this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#b2d2ff';}");
      e.Row.Attributes.Add("onmouseout", "if(this.style.backgroundColor!='#D1DDF1'){this.style.backgroundColor=this.originalstyle;}");
    }
  }

  protected void ccAdminConfirmedGridView_DataBound(object sender, EventArgs e)
  {
    GridViewRow gvrPager = ccAdminConfirmedGridView.BottomPagerRow;
    if (gvrPager == null) return;

    DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages4");
    Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

    if (ddlPages != null)
    {
      for (int i = 0; i < ccAdminConfirmedGridView.PageCount; i++)
      {
        int intPageNumber = i + 1;
        ListItem lstItem = new ListItem(intPageNumber.ToString());
        if (i == ccAdminConfirmedGridView.PageIndex)
        {
          lstItem.Selected = true;
        }
        ddlPages.Items.Add(lstItem);
      }
    }

    if (lblPageCount != null)
    {
      lblPageCount.Text = ccAdminConfirmedGridView.PageCount.ToString();
    }
  }

  protected void ddlPages4_SelectedIndexChanged(object sender, EventArgs e)
  {
    GridViewRow gvrPager = ccAdminConfirmedGridView.BottomPagerRow;
    DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages4");

    ccAdminConfirmedGridView.PageIndex = ddlPages.SelectedIndex;
    RefreshCCAdminConfirmedGridView();
  }

  protected void ccAdminConfirmedGridView_SelectedIndexChanged(object sender, EventArgs e)
  {
    Session["CCReferenceNumber"] = ccAdminConfirmedGridView.SelectedValue.ToString();
    Response.Redirect("CCAdminRequestDetails.aspx");
  }

  #endregion

  #region For SOA Processing

  public void RefreshForSOAProcessingGridView()
  {
    DataTable dt = new DataTable();
    dt.Columns.Add(new DataColumn("CCRequestReferenceNo", typeof(string)));
    dt.Columns.Add(new DataColumn("EventName", typeof(string)));
    dt.Columns.Add(new DataColumn("StartDate", typeof(DateTime)));
    dt.Columns.Add(new DataColumn("EndDate", typeof(DateTime)));
    dt.Columns.Add(new DataColumn("DateCreated", typeof(DateTime)));
    dt.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
    dt.Columns.Add(new DataColumn("ChargedCompany", typeof(string)));
    dt.Columns.Add(new DataColumn("CostCenterName", typeof(string)));

    RetrieveCCRequestRecordsBySOAStatusRequest retrieveCCRequestRecordsBySOAStatusRequest = new RetrieveCCRequestRecordsBySOAStatusRequest();
    retrieveCCRequestRecordsBySOAStatusRequest.SOAStatusCode = Convert.ToInt16(soaStatusDropDownList.SelectedValue);

    RetrieveCCRequestRecordsBySOAStatusResult retrieveCCRequestRecordsBySOAStatusResult;

    try
    {
      retrieveCCRequestRecordsBySOAStatusResult = svc.RetrieveCCRequestRecordsBySOAStatus(retrieveCCRequestRecordsBySOAStatusRequest);
    }
    catch(Exception ex)
    {
      throw new Exception(Settings.GenericWebServiceMessage);
    }


    if (retrieveCCRequestRecordsBySOAStatusResult.ResultStatus != iReserveWS.ResultStatus.Successful)
    {
      Utilities.MyMessageBox(retrieveCCRequestRecordsBySOAStatusResult.Message);
    }
    else
    {
      foreach (CCRequest record in retrieveCCRequestRecordsBySOAStatusResult.CCRequestList)
      {
        DataRow dr = dt.NewRow();
        dr["CCRequestReferenceNo"] = record.CCRequestReferenceNo;
        dr["EventName"] = record.EventName;
        dr["StartDate"] = record.StartDate;
        dr["EndDate"] = record.EndDate;
        dr["DateCreated"] = record.DateCreated;
        dr["CreatedBy"] = record.CreatedBy;
        dr["ChargedCompany"] = record.CostCenterName;
        string costCenter = record.ChargedCompanyCostCenter.CostCenterName != null ? record.ChargedCompanyCostCenter.CostCenterName : string.Empty;
        dr["CostCenterName"] = costCenter;
        dt.Rows.Add(dr);
      }

      forSOAProcessingGridView.DataSource = dt;
      forSOAProcessingGridView.DataBind();

      if (forSOAProcessingGridView.Rows.Count == 0)
      {
        DataTable dt2 = dt.Clone();
        dt2.Rows.Add(dt2.NewRow());

        forSOAProcessingGridView.DataSource = dt2;
        forSOAProcessingGridView.DataBind();

        int columncount = forSOAProcessingGridView.Rows[0].Cells.Count;
        forSOAProcessingGridView.Rows[0].Cells.Clear();
        forSOAProcessingGridView.Rows[0].Cells.Add(new TableCell());
        forSOAProcessingGridView.Rows[0].Cells[0].ColumnSpan = columncount;
        forSOAProcessingGridView.Rows[0].Cells[0].Text = "No record found";
      }
    }
  }

  protected void forSOAProcessingGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
  {
    int newPageIndex = e.NewPageIndex;

    if (newPageIndex == -1)
    {
      forSOAProcessingGridView.PageIndex = e.NewPageIndex + 1;
    }
    else
    {
      forSOAProcessingGridView.PageIndex = e.NewPageIndex;
    }

    RefreshForSOAProcessingGridView();
  }

  protected void forSOAProcessingGridView_RowCreated(object sender, GridViewRowEventArgs e)
  {
    if (e.Row.RowType == DataControlRowType.DataRow)
    {
      e.Row.Attributes.Add("onmouseover", "if(this.style.backgroundColor!='#D1DDF1'){this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#b2d2ff';}");
      e.Row.Attributes.Add("onmouseout", "if(this.style.backgroundColor!='#D1DDF1'){this.style.backgroundColor=this.originalstyle;}");
    }
  }

  protected void forSOAProcessingGridView_DataBound(object sender, EventArgs e)
  {
    GridViewRow gvrPager = forSOAProcessingGridView.BottomPagerRow;
    if (gvrPager == null) return;

    DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages5");
    Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

    if (ddlPages != null)
    {
      for (int i = 0; i < forSOAProcessingGridView.PageCount; i++)
      {
        int intPageNumber = i + 1;
        ListItem lstItem = new ListItem(intPageNumber.ToString());
        if (i == forSOAProcessingGridView.PageIndex)
        {
          lstItem.Selected = true;
        }
        ddlPages.Items.Add(lstItem);
      }
    }

    if (lblPageCount != null)
    {
      lblPageCount.Text = forSOAProcessingGridView.PageCount.ToString();
    }
  }

  protected void ddlPages5_SelectedIndexChanged(object sender, EventArgs e)
  {
    GridViewRow gvrPager = forSOAProcessingGridView.BottomPagerRow;
    DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages5");

    forSOAProcessingGridView.PageIndex = ddlPages.SelectedIndex;
    RefreshForSOAProcessingGridView();
  }

  protected void forSOAProcessingGridView_SelectedIndexChanged(object sender, EventArgs e)
  {
    Session["CCReferenceNumber"] = forSOAProcessingGridView.SelectedValue.ToString();
    Response.Redirect("CCAdminRequestDetails.aspx");
  }

  #endregion
}