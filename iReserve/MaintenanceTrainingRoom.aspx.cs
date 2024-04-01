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

public partial class MaintenanceTrainingRoom : System.Web.UI.Page
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
            if (profileName != "Convention Center Administrator")
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

        RetrieveTrainingRoomRecordsRequest retrieveTrainingRoomRecordsRequest = new RetrieveTrainingRoomRecordsRequest();
        retrieveTrainingRoomRecordsRequest.RoomCode = parameterCode;
        retrieveTrainingRoomRecordsRequest.RoomName = parameterName;

        RetrieveTrainingRoomRecordsResult retrieveTrainingRoomRecordsResult = svc.RetrieveTrainingRoomRecords(retrieveTrainingRoomRecordsRequest);

        if (retrieveTrainingRoomRecordsResult.ResultStatus == iReserveWS.ResultStatus.Successful)
        {
            trainingRoomGridView.DataSource = retrieveTrainingRoomRecordsResult.TrainingRoomList;
            trainingRoomGridView.DataBind();
        }
        else
        {
            Utilities.MyMessageBoxWithHomeRedirect(retrieveTrainingRoomRecordsResult.Message);
        }
    }

    protected void searchButton_Click(object sender, EventArgs e)
    {
        trainingRoomGridView.SelectedIndex = -1;
        refreshGridView();
    }
    protected void trainingRoomGridView_DataBound(object sender, EventArgs e)
    {
        GridViewRow gvrPager = trainingRoomGridView.BottomPagerRow;
        if (gvrPager == null) return;

        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

        if (ddlPages != null)
        {
            for (int i = 0; i < trainingRoomGridView.PageCount; i++)
            {
                int intPageNumber = i + 1;
                ListItem lstItem = new ListItem(intPageNumber.ToString());
                if (i == trainingRoomGridView.PageIndex)
                {
                    lstItem.Selected = true;
                }
                ddlPages.Items.Add(lstItem);
            }
        }

        if (lblPageCount != null)
        {
            lblPageCount.Text = trainingRoomGridView.PageCount.ToString();
        }
    }
    protected void trainingRoomGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int intRowIndex = e.Row.RowIndex + 1;
            e.Row.Attributes.Add("onclick", "javascript:GetRoomRowIndex(" + intRowIndex + ")");
        }
    }

    protected void ddlPages_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gvrPager = trainingRoomGridView.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");

        trainingRoomGridView.PageIndex = ddlPages.SelectedIndex;
        refreshGridView();
    }
    protected void trainingRoomGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        int newPageIndex = e.NewPageIndex;

        if (newPageIndex == -1)
        {
            trainingRoomGridView.PageIndex = e.NewPageIndex + 1;
        }
        else
        {
            trainingRoomGridView.PageIndex = e.NewPageIndex;
        }

        refreshGridView();
    }
    protected void trainingRoomGridView_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "if(this.style.backgroundColor!='#D1DDF1'){this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#b2d2ff';}");
            e.Row.Attributes.Add("onmouseout", "if(this.style.backgroundColor!='#D1DDF1'){this.style.backgroundColor=this.originalstyle;}");
        }
    }

    [WebMethod(EnableSession = true)]
    public static bool deleteRoom(int pType, int pRoomID, int pLocationID, string pRoomCode, string pRoomName, string pRoomDesc, int pNumberOfPartition,
                                                bool pIsDeleted, string pmacAddress)
    {
        bool blnDeleteRoom = false;

        int validationStatus;

        TrainingRoom validateTrainingRoom = new TrainingRoom();
        validateTrainingRoom.TRoomID = pRoomID;
        validateTrainingRoom.TRoomCode = pRoomCode;
        validateTrainingRoom.TRoomName = pRoomName;

        ValidateTrainingRoomRecordRequest validateTrainingRoomRecordRequest = new ValidateTrainingRoomRecordRequest();
        validateTrainingRoomRecordRequest.Type = pType;
        validateTrainingRoomRecordRequest.TrainingRoom = validateTrainingRoom;

        ValidateTrainingRoomRecordResult validateTrainingRoomRecordResult = new ValidateTrainingRoomRecordResult();
        validateTrainingRoomRecordResult = svc.ValidateTrainingRoomRecord(validateTrainingRoomRecordRequest);

        if (validateTrainingRoomRecordResult.ResultStatus != iReserveWS.ResultStatus.Successful)
        {
            Utilities.MyMessageBox(validateTrainingRoomRecordResult.Message);
        }
        else
        {
            validationStatus = validateTrainingRoomRecordResult.ValidationStatus;

            if (validationStatus == 1)
            {
                blnDeleteRoom = false;
            }
            else if (validationStatus == 0)
            {
                string userID = HttpContext.Current.Session["UserID"].ToString();
                string browser = HttpContext.Current.Session["browser"].ToString();
                string browserVersion = HttpContext.Current.Session["browserVersion"].ToString();

                TrainingRoom tranTrainingRoom = new TrainingRoom();
                tranTrainingRoom.TRoomID = pRoomID;
                tranTrainingRoom.TRoomCode = pRoomCode;
                tranTrainingRoom.TRoomName = pRoomName;
                tranTrainingRoom.TRoomDesc = pRoomDesc;
                tranTrainingRoom.LocationID = pLocationID;
                tranTrainingRoom.NumberOfPartition = pNumberOfPartition;
                tranTrainingRoom.IsDeleted = pIsDeleted;

                AuditTrail tranAuditTrail = new AuditTrail();
                tranAuditTrail.ActionDate = DateTime.Now;
                tranAuditTrail.ActionTaken = "Delete Training Room";
                tranAuditTrail.ActionDetails = "Deleted room || Room ID: " + pRoomID + " || Room Name: " + pRoomName;
                tranAuditTrail.Browser = HttpContext.Current.Request.Browser.Browser;
                tranAuditTrail.BrowserVersion = HttpContext.Current.Request.Browser.Version;
                tranAuditTrail.IpAddress = HttpContext.Current.Request.ServerVariables[32];

                System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
                tranAuditTrail.MacAdress = HttpContext.Current.Session["MacAddress"].ToString();
                tranAuditTrail.UserID = HttpContext.Current.Session["UserID"].ToString();

                TrainingRoomTransactionRequest trainingRoomTransactionRequest = new TrainingRoomTransactionRequest();
                trainingRoomTransactionRequest.Type = pType;
                trainingRoomTransactionRequest.TrainingRoom = tranTrainingRoom;
                trainingRoomTransactionRequest.AuditTrail = tranAuditTrail;

                TrainingRoomTransactionResult trainingRoomTransactionResult = new TrainingRoomTransactionResult();
                trainingRoomTransactionResult = svc.TrainingRoomTransaction(trainingRoomTransactionRequest);

                if (trainingRoomTransactionResult.ResultStatus != iReserveWS.ResultStatus.Successful)
                {
                    blnDeleteRoom = false;

                    Utilities.MyMessageBox(trainingRoomTransactionResult.Message);
                }
                else
                {
                    blnDeleteRoom = true;

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

        return blnDeleteRoom;
    }
}