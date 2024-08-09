using System;
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

public partial class MaintenanceAccomodationRoom : System.Web.UI.Page
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
            if (profileName != "Convention Center Administrator" && profileName != "SOA Approver")
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
        string parameterCode = paramCodeTextBox.Text;
        string parameterName = paramNameTextBox.Text;

        RetrieveAccomodationRoomRecordsRequest retrieveAccomodationRoomRecordsRequest = new RetrieveAccomodationRoomRecordsRequest();
        retrieveAccomodationRoomRecordsRequest.RoomCode = parameterCode;
        retrieveAccomodationRoomRecordsRequest.RoomName = parameterName;

        RetrieveAccomodationRoomRecordsResult retrieveAccomodationRoomRecordsResult = svc.RetrieveAccomodationRoomRecords(retrieveAccomodationRoomRecordsRequest);

        if(retrieveAccomodationRoomRecordsResult.ResultStatus == iReserveWS.ResultStatus.Successful)
        {
            accRoomGridView.DataSource = retrieveAccomodationRoomRecordsResult.AccomodationRoomList;
            accRoomGridView.DataBind();
        }
        else
        {
             Utilities.MyMessageBoxWithHomeRedirect(retrieveAccomodationRoomRecordsResult.Message);
        }
    }

    protected void searchButton_Click(object sender, EventArgs e)
    {
        accRoomGridView.SelectedIndex = -1;
        refreshGridView();
    }
    protected void accRoomGridView_DataBound(object sender, EventArgs e)
    {
        GridViewRow gvrPager = accRoomGridView.BottomPagerRow;
        if (gvrPager == null) return;

        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

        if (ddlPages != null)
        {
            for (int i = 0; i < accRoomGridView.PageCount; i++)
            {
                int intPageNumber = i + 1;
                ListItem lstItem = new ListItem(intPageNumber.ToString());
                if (i == accRoomGridView.PageIndex)
                {
                    lstItem.Selected = true;
                }
                ddlPages.Items.Add(lstItem);
            }
        }

        if (lblPageCount != null)
        {
            lblPageCount.Text = accRoomGridView.PageCount.ToString();
        }
    }
    protected void accRoomGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int intRowIndex = e.Row.RowIndex + 1;
            e.Row.Attributes.Add("onclick", "javascript:GetRoomRowIndex(" + intRowIndex + ")");
        }
    }

    protected void ddlPages_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gvrPager = accRoomGridView.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");

        accRoomGridView.PageIndex = ddlPages.SelectedIndex;
        refreshGridView();
    }
    protected void accRoomGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;

        if (newPageIndex == -1)
        {
            accRoomGridView.PageIndex = e.NewPageIndex + 1;
        }
        else
        {
            accRoomGridView.PageIndex = e.NewPageIndex;
        }

        refreshGridView();
    }
    protected void accRoomGridView_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "if(this.style.backgroundColor!='#D1DDF1'){this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#b2d2ff';}");
            e.Row.Attributes.Add("onmouseout", "if(this.style.backgroundColor!='#D1DDF1'){this.style.backgroundColor=this.originalstyle;}");
        }
    }

    [WebMethod(EnableSession = true)]
    public static bool deleteRoom(int pType, int pRoomID, int pLocationID, string pRoomCode, string pRoomName, string pRoomDesc, int pMaxPerson, string pRatePerNight,
                                                bool pIsDeleted, string pmacAddress)
    {
        bool blnDeleteLocation = false;

        int validationStatus;

        AccomodationRoom validateAccomodationRoom = new AccomodationRoom();
        validateAccomodationRoom.AccRoomID = pRoomID;
        validateAccomodationRoom.RoomCode = pRoomCode;
        validateAccomodationRoom.RoomName = pRoomName;

        ValidateAccomodationRoomRecordRequest validateAccomodationRoomRecordRequest = new ValidateAccomodationRoomRecordRequest();
        validateAccomodationRoomRecordRequest.Type = pType;
        validateAccomodationRoomRecordRequest.AccomodationRoom = validateAccomodationRoom;

        ValidateAccomodationRoomRecordResult validateAccomodationRoomRecordResult = new ValidateAccomodationRoomRecordResult();
        validateAccomodationRoomRecordResult = svc.ValidateAccomodationRoomRecord(validateAccomodationRoomRecordRequest);

        if (validateAccomodationRoomRecordResult.ResultStatus != iReserveWS.ResultStatus.Successful)
        {
            Utilities.MyMessageBox(validateAccomodationRoomRecordResult.Message);
        }
        else
        {
            validationStatus = validateAccomodationRoomRecordResult.ValidationStatus;

            if (validationStatus == 1)
            {
                blnDeleteLocation = false;
            }
            else if (validationStatus == 0)
            {
                string userID = HttpContext.Current.Session["UserID"].ToString();
                string browser = HttpContext.Current.Session["browser"].ToString();
                string browserVersion = HttpContext.Current.Session["browserVersion"].ToString();

                AccomodationRoom tranAccomodationRoom = new AccomodationRoom();
                tranAccomodationRoom.AccRoomID = pRoomID;
                tranAccomodationRoom.LocationID = pLocationID;
                tranAccomodationRoom.RoomCode = pRoomCode;
                tranAccomodationRoom.RoomName = pRoomName;
                tranAccomodationRoom.RoomDesc = pRoomDesc;
                tranAccomodationRoom.MaxPerson = pMaxPerson;
                tranAccomodationRoom.RatePerNight = pRatePerNight;
                tranAccomodationRoom.IsDeleted = pIsDeleted;

                AuditTrail tranAuditTrail = new AuditTrail();
                tranAuditTrail.ActionDate = DateTime.Now;
                tranAuditTrail.ActionTaken = "Delete Accomodation Room";
                tranAuditTrail.ActionDetails = "Deleted room || Room ID: " + pRoomID + " || Room Name: " + pRoomName;
                tranAuditTrail.Browser = HttpContext.Current.Request.Browser.Browser;
                tranAuditTrail.BrowserVersion = HttpContext.Current.Request.Browser.Version;
                tranAuditTrail.IpAddress = HttpContext.Current.Request.ServerVariables[32];

                System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
                tranAuditTrail.MacAdress = HttpContext.Current.Session["MacAddress"].ToString();
                tranAuditTrail.UserID = HttpContext.Current.Session["UserID"].ToString();

                AccomodationRoomTransactionRequest accomodationRoomTransactionRequest = new AccomodationRoomTransactionRequest();
                accomodationRoomTransactionRequest.Type = pType;
                accomodationRoomTransactionRequest.AccomodationRoom = tranAccomodationRoom;
                accomodationRoomTransactionRequest.AuditTrail = tranAuditTrail;

                AccomodationRoomTransactionResult accomodationRoomTransactionResult = new AccomodationRoomTransactionResult();
                accomodationRoomTransactionResult = svc.AccomodationRoomTransaction(accomodationRoomTransactionRequest);

                if (accomodationRoomTransactionResult.ResultStatus != iReserveWS.ResultStatus.Successful)
                {
                    blnDeleteLocation = false;

                    Utilities.MyMessageBox(accomodationRoomTransactionResult.Message);
                }
                else
                {
                    blnDeleteLocation = true;

                    #region Audit Trail

                    bool isSuccess = false;

                    try
                    {
                        isSuccess = svc.InsertAuditTrailEntry(tranAuditTrail);
                    }

                    catch (SoapException ex)
                    {
                        throw new Exception(Settings.GenericAuditTrailMessage);
                    }

                    #endregion
                }
            }
        }

        return blnDeleteLocation;
    }
}