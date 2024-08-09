﻿using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using iReserveWS;
using System.Text;
using AppCryptor;
using AESCryptor;
using System.Web.Services.Protocols;
public partial class Room : System.Web.UI.Page
{
    public static Service svc = new Service();
    public string userID, macAddress, browser, browserVersion;

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
            if (profileName != "Conference Room Administrator" && profileName != "SOA Approver")
            {
                Response.Write("<script language=javascript> alert('You are not allowed to access this page. Please click on the Ok Button to go back to the Home Page.'); window.location.href ='Default.aspx';</script>");
            }
        }

        MaintainScrollPositionOnPostBack = true;

        refreshGridView();

        userID = HttpContext.Current.Session["UserID"].ToString();
        Session["browser"] = Request.Browser.Browser;
        browser = HttpContext.Current.Session["browser"].ToString();
        Session["browserVersion"] = Request.Browser.Type;
        browserVersion = HttpContext.Current.Session["browserVersion"].ToString();
    }

    public void refreshGridView()
    {
        string parameterLocation = paramLocationTextBox.Text;
        string parameterCode = paramCodeTextBox.Text;
        string parameterName = paramNameTextBox.Text;

        try
        {
            roomGridView.DataSource = svc.RetrieveConferenceRoomRecords(parameterLocation, parameterCode, parameterName);
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericWebServiceMessage);
        }

        roomGridView.DataBind();
    }

    protected void searchButton_Click(object sender, EventArgs e)
    {
        roomGridView.SelectedIndex = -1;
        refreshGridView();
    }
    protected void roomGridView_DataBound(object sender, EventArgs e)
    {
        GridViewRow gvrPager = roomGridView.BottomPagerRow;
        if (gvrPager == null) return;

        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

        if (ddlPages != null)
        {
            for (int i = 0; i < roomGridView.PageCount; i++)
            {
                int intPageNumber = i + 1;
                ListItem lstItem = new ListItem(intPageNumber.ToString());
                if (i == roomGridView.PageIndex)
                {
                    lstItem.Selected = true;
                }
                ddlPages.Items.Add(lstItem);
            }
        }

        if (lblPageCount != null)
        {
            lblPageCount.Text = roomGridView.PageCount.ToString();
        }
    }
    protected void roomGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int intRowIndex = e.Row.RowIndex + 1;
            e.Row.Attributes.Add("onclick", "javascript:GetRoomRowIndex(" + intRowIndex + ")");
        }
    }

    protected void ddlPages_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gvrPager = roomGridView.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");

        roomGridView.PageIndex = ddlPages.SelectedIndex;
        refreshGridView();
    }
    protected void roomGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;

        if (newPageIndex == -1)
        {
            roomGridView.PageIndex = e.NewPageIndex + 1;
        }
        else
        {
            roomGridView.PageIndex = e.NewPageIndex;
        }

        refreshGridView();
    }
    protected void roomGridView_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "if(this.style.backgroundColor!='#D1DDF1'){this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#b2d2ff';}");
            e.Row.Attributes.Add("onmouseout", "if(this.style.backgroundColor!='#D1DDF1'){this.style.backgroundColor=this.originalstyle;}");
        }
    }

    [WebMethod(EnableSession = true)]
    public static bool deleteRoom(int pType, int pRoomID, int pLocationID, string pRoomCode, string pRoomName, string pRoomDesc, int pmonitorCode,
                                                bool pIsDeleted, string pmacAddress)
    {
        bool blnDeleteLocation = false;

        int validationStatus;

        try
        {
            validationStatus = svc.ValidateConferenceRoomRecord(pType, pRoomID, pRoomCode, pRoomName, pmonitorCode);
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericWebServiceMessage);
        }

        if (validationStatus == 1)
        {
            blnDeleteLocation = false;
        }
        else if (validationStatus == 0)
        {
            string userID = HttpContext.Current.Session["UserID"].ToString();
            string browser = HttpContext.Current.Session["browser"].ToString();
            string browserVersion = HttpContext.Current.Session["browserVersion"].ToString();

            try
            {
                svc.ConferenceRoomRecordTransaction(pType, userID, pRoomID, pRoomCode, pRoomName, pRoomDesc, pLocationID,
                                        0, false, false, string.Empty, string.Empty, 0, pIsDeleted, pmacAddress, browser, browserVersion);
            }

            catch (SoapException ex)
            {
                throw new Exception(Settings.GenericWebServiceMessage);
            }

            #region Audit Trail

            bool isSuccess = false;

            AuditTrail auditTrail = new AuditTrail();
            auditTrail.ActionDate = DateTime.Now;
            auditTrail.ActionTaken = "Delete Conference Room";
            auditTrail.ActionDetails = "Deleted room || Room ID: " + pRoomID + " || Room Name: " + pRoomName;
            auditTrail.Browser = HttpContext.Current.Request.Browser.Browser;
            auditTrail.BrowserVersion = HttpContext.Current.Request.Browser.Version;
            auditTrail.IpAddress = HttpContext.Current.Request.ServerVariables[32];
            System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();

            auditTrail.MacAdress = HttpContext.Current.Session["MacAddress"].ToString();
            auditTrail.UserID = HttpContext.Current.Session["UserID"].ToString();

            try
            {
                isSuccess = svc.InsertAuditTrailEntry(auditTrail);
            }

            catch (SoapException ex)
            {
                throw new Exception(Settings.GenericAuditTrailMessage);
            }

            #endregion

            blnDeleteLocation = true;
        }

        return blnDeleteLocation;
    }
}