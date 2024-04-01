using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Net.Mail;
using System.Transactions;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class Service : System.Web.Services.WebService
{
    Common com = new Common();

    public Service()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }


    public string MEthod()
    {
        return "";
    }

    protected void ResponseWrite(string message)
    {
        Context.Response.Write(message);
    }

    #region ChangePassword
    [WebMethod(Description = "For Change Password")]
    public PasswordRecoveryInfo ChangePassword(string UserID)
    {
        try
        {
            DAL dal = new DAL();
            PasswordRecoveryInfo changePass = dal.ChangePassword(UserID);
            return changePass;
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }

    #endregion

    #region SendChangePasswordNotif
    [WebMethod(Description = "For Sending of Change Password Notification")]
    public bool SendChangePasswordNotification(string Email)
    {
        try
        {
            bool blnSuccess = false;

            MailAddress senderAddress = new MailAddress("ireserve@pjlhuillier.com", "iReserve");
            SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["Smtp.Client"]);

            string strBody = "<PRE>A new password has been set on: " + DateTime.Now.ToLongDateString() + " ";
            strBody += "at: " + DateTime.Now.ToLongTimeString() + " and will expire after 90 days. ";
            strBody += "Make sure you change your password before it expires.";

            MailMessage msg = new MailMessage(ConfigurationManager.AppSettings["Email.Sender"], Email);
            msg.From = senderAddress;
            msg.Subject = "User Password Changed";
            msg.Body = strBody;
            msg.IsBodyHtml = true;

            client.Send(msg);
            blnSuccess = true;

            return blnSuccess;
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region UserPasswordRecovery

    [WebMethod(Description = "For Password Retrieval")]
    public PasswordRecoveryInfo UserPasswordRecovery(string UserID)
    {
        try
        {
            DAL dal = new DAL();
            PasswordRecoveryInfo userPassRecover = dal.UserPasswordRecovery(UserID);
            return userPassRecover;
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region SendRecoveredPassword

    [WebMethod(Description = "For Sending of Recovered Password")]
    public bool SendRecoveredPassword(string UserID, string Password, string Email)
    {
        try
        {
            bool blnSuccess = false;

            MailAddress senderAddress = new MailAddress("ireserve@pjlhuillier.com", "iReserve");
            SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["Smtp.Client"]);

            string strBody = "<PRE>Your account information are as follows: <br /><br />";
            strBody += "User ID: " + UserID + "<br />";
            strBody += "Password: " + Password + "<br /><br /><br />";
            strBody += "If you have further question(s) or need an assistance, ";
            strBody += "contact Networld Capital Ventures Incorporated or email at infosec@pjlhuillier.com <br /><br />";
            strBody += "Note: <br />      Please do not reply to this system-generated email. No one will be able to reply.";

            MailMessage msg = new MailMessage(ConfigurationManager.AppSettings["Email.Sender"], Email);
            msg.From = senderAddress;
            msg.Subject = "User Account Information";
            msg.Body = strBody;
            msg.IsBodyHtml = true;

            client.Send(msg);
            blnSuccess = true;

            return blnSuccess;
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }

    #endregion

    #region CheckIfUserExist

    [WebMethod(Description = "Check if User exists.")]
    public bool CheckIfUserExist(string UserID)
    {
        try
        {
            bool blnUserExist = false;

            using (SqlConnection conStr = new SqlConnection(Settings.AACFConnectionString))
            {
                string sql = "SELECT [fld_UserID] FROM [dbo].[tbl_Users] WHERE [fld_UserID] = '" + UserID + "'";

                SqlCommand cmd = new SqlCommand(sql, conStr);
                conStr.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    PasswordRecoveryInfo recoverPassword = new PasswordRecoveryInfo();

                    if (reader.Read())
                    {
                        blnUserExist = true;
                    }
                    reader.Close();
                }
                return blnUserExist;
            }
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }

    #endregion

    #region RetrieveLocationRecords
    [WebMethod(Description = "Retrieve records from the Location table.")]
    public List<MaintenanceLocationList> RetrieveLocationRecords(string LocationCode, string LocationName)
    {
        try
        {
            DAL dal = new DAL();
            List<MaintenanceLocationList> locationRecordList = dal.RetrieveLocationRecords(LocationCode, LocationName);
            return locationRecordList;
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region RetrieveLocationRecordDetails
    [WebMethod(Description = "For location record retrieval.")]
    public MaintenanceLocationList RetrieveLocationRecordDetails(int LocationID)
    {
        try
        {
            DAL dal = new DAL();
            MaintenanceLocationList locationRecord = dal.RetrieveLocationRecordDetails(LocationID);
            return locationRecord;
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region ValidateLocationRecord
    [WebMethod(Description = "Validate insertion/modification/deletion of location record.")]
    public int ValidateLocationRecord(int Type, int LocationID, string LocationCode, string LocationName)
    {
        try
        {
            DAL dal = new DAL();
            int validationStatus = dal.ValidateLocationRecord(Type, LocationID, LocationCode, LocationName);
            return validationStatus;
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region LocationRecordTransaction
    [WebMethod(Description = "For location record transaction.")]
    public void LocationRecordTransaction(int Type, string UserID, int LocationID,
                                            string LocationCode, string LocationName,
                                            string LocationDesc, bool IsDeleted, string MACAddress,
                                            string Browser, string BrowserVersion)
    {
        try
        {
            DAL dal = new DAL();
            dal.LocationRecordTransaction(Type, UserID, LocationID, LocationCode, LocationName, LocationDesc, IsDeleted,
                                                MACAddress, Browser, BrowserVersion);
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region RetrieveConferenceRoomRecords
    [WebMethod(Description = "Retrieve records from the Room table.")]
    public List<MaintenanceConferenceRoomList> RetrieveConferenceRoomRecords(string LocationName, string RoomCode, string RoomName)
    {
        try
        {
            DAL dal = new DAL();
            List<MaintenanceConferenceRoomList> roomRecordList = dal.RetrieveConferenceRoomRecords(LocationName, RoomCode, RoomName);
            return roomRecordList;
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region RetrieveConferenceRoomRecordDetails
    [WebMethod(Description = "For room record retrieval.")]
    public MaintenanceConferenceRoomList RetrieveConferenceRoomRecordDetails(int RoomID)
    {
        try
        {
            DAL dal = new DAL();
            MaintenanceConferenceRoomList roomRecord = dal.RetrieveConferenceRoomRecordDetails(RoomID);
            return roomRecord;
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region ValidateConferenceRoomRecord
    [WebMethod(Description = "Validate insertion/modification/deletion of room record.")]
    public int ValidateConferenceRoomRecord(int Type, int RoomID, string RoomCode, string RoomName, int monitorCode)
    {
        try
        {
            DAL dal = new DAL();
            int validationStatus = dal.ValidateConferenceRoomRecord(Type, RoomID, RoomCode, RoomName, monitorCode);
            return validationStatus;
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region ConferenceRoomRecordTransaction
    [WebMethod(Description = "For room record transaction.")]
    public void ConferenceRoomRecordTransaction(int Type, string UserID, int RoomID, string RoomCode, string RoomName,
                                            string RoomDesc, int LocationID, int MaxPerson, bool IsDataPortAvailable,
                                            bool IsMonitorAvailable, string RatePerHour, string TabletID,
                                            int MonitorDisplayCode, bool IsDeleted,
                                            string MACAddress, string Browser, string BrowserVersion)
    {
        try
        {
            DAL dal = new DAL();
            dal.ConferenceRoomRecordTransaction(Type, UserID, RoomID, RoomCode, RoomName, RoomDesc, LocationID, MaxPerson,
                                        IsDataPortAvailable, IsMonitorAvailable, RatePerHour, TabletID, MonitorDisplayCode,
                                        IsDeleted, MACAddress, Browser, BrowserVersion);
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region RetrieveMonitorDisplayRecords
    [WebMethod(Description = "Retrieve records from the Monitor Display table.")]
    public List<MonitorDisplay> RetrieveMonitorDisplayRecords()
    {
        try
        {
            DAL dal = new DAL();
            List<MonitorDisplay> monitorDisplayRecordList = dal.RetrieveMonitorDisplayRecords();
            return monitorDisplayRecordList;
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region RetrieveConferenceRoomSchedules
    [WebMethod(Description = "Retrieve conference room schedule")]
    public DataTable RetrieveConferenceRoomSchedules(string dateFrom, int roomID)
    {
        try
        {
            DAL dal = new DAL();
            DataTable roomSchedule = new DataTable("RoomSchedule");
            roomSchedule = dal.RetrieveConferenceRoomSchedules(dateFrom, roomID);
            return roomSchedule;
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region RetrieveCRDisplaySchedule
    [WebMethod(Description = "Retrieve conference room schedule for monitor display")]
    public DataTable RetrieveCRDisplaySchedule(string date, int monitorDisplayCode)
    {
        try
        {
            DAL dal = new DAL();
            return dal.RetrieveCRDisplaySchedule(date, monitorDisplayCode);
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region RetrieveTimeSlots
    [WebMethod(Description = "Retrieve Conference Room time slots.")]
    public List<CRTimeSlot> RetrieveTimeSlots()
    {
        try
        {
            DAL dal = new DAL();
            List<CRTimeSlot> timeSlotList = dal.RetrieveTimeSlots();
            return timeSlotList;
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region RetrieveCostCenter
    [WebMethod(Description = "Retrieve Cost Center list.")]
    public List<CostCenter> RetrieveCostCenter()
    {
        try
        {
            DAL dal = new DAL();
            List<CostCenter> costCenterList = dal.RetrieveCostCenterRecords();
            return costCenterList;
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region GetLastGeneratedReferenceNumber
    [WebMethod(Description = "Get last generated reference number.")]
    public CRLastGeneratedReferenceNumber GetLastGeneratedReferenceNumber(DateTime dateGenerated)
    {
        try
        {
            CRLastGeneratedReferenceNumber lastGeneratedRefNo = new CRLastGeneratedReferenceNumber();
            lastGeneratedRefNo.GetLastGeneratedReferenceNumber(dateGenerated);
            return lastGeneratedRefNo;
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region RetrieveCRRequestRecordsByStatus
    [WebMethod(Description = "Retrieve reservation request records by status.")]
    public List<CRRequest> RetrieveCRRequestRecordsByStatus(CRRequest request)
    {
        try
        {
            return request.RetrieveCRRequestRecordsByStatus();
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region RetrieveCRRequestDetails
    [WebMethod(Description = "Retrieve reservation request details by reference number.")]
    public CRRequest RetrieveCRRequestDetails(CRRequest request)
    {
        try
        {
            CRRequest returnValue = new CRRequest();
            returnValue.RetrieveCRRequestDetails(request.RequestReferenceNo);
            returnValue.RetrieveCRRequestAttendees();

            return returnValue;
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region InsertNewRequest
    [WebMethod(Description = "Insert new request details.")]
    public Result InsertNewRequest(CRRequest request)
    {
        Result returnValue = null;

        try
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                request.InsertCRRequest();
                request.Attachment.InsertCRRequestAttachment();
                request.RequestHistory.InsertCRRequestHistory();
                request.InsertCRRequestAttendeeList();

                EmailNotification emailNotification = new EmailNotification();
                emailNotification.SendEmailNotification(request);

                transactionScope.Complete();
            }

            returnValue = new Result();
            returnValue.ResultStatus = ResultStatus.Successful;

            return returnValue;
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue = new Result();
            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region UpdateReservationRequest
    [WebMethod(Description = "Update reservation request status.")]
    public Result UpdateReservationRequest(CRRequest request)
    {
        Result returnValue = null;

        try
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                request.UpdateCRRequestStatus();
                request.RequestHistory.InsertCRRequestHistory();
                request.RetrieveCRRequestDetails(request.RequestReferenceNo);

                if (request.Status.StatusCode == StatusCode.Confirmed)
                {
                    request.InsertScheduleMapping();
                    request.DeclineCRSimilarPendingRequests();
                }
                else if (request.Status.StatusCode == StatusCode.ForCancellation)
                {
                    request.Attachment.InsertCRRequestAttachment();
                }
                else if (request.Status.StatusCode == StatusCode.Cancelled)
                {
                    request.UpdateScheduleMappingStatus();
                }

                EmailNotification emailNotification = new EmailNotification();
                emailNotification.SendEmailNotification(request);

                transactionScope.Complete();
            }

            returnValue = new Result();
            returnValue.ResultStatus = ResultStatus.Successful;

            return returnValue;
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue = new Result();
            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region CancelReservationRequest
    [WebMethod(Description = "Update reservation request status.")]
    public Result CancelReservationRequest(CRRequest request)
    {
        Result returnValue = null;

        try
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                request.UpdateCRRequestStatus();
                request.RequestHistory.InsertCRRequestHistory();
                request.RetrieveCRRequestDetails(request.RequestReferenceNo);

                EmailNotification emailNotification = new EmailNotification();
                emailNotification.SendEmailNotification(request);

                transactionScope.Complete();
            }

            returnValue = new Result();
            returnValue.ResultStatus = ResultStatus.Successful;

            return returnValue;
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue = new Result();
            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region ValidateScheduleAvailability
    [WebMethod(Description = "Check if selected room and schedule is available.")]
    public bool ValidateScheduleAvailability(CRRequest request)
    {
        try
        {
            return request.ValidateScheduleAvailability();
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region RetrieveCRRequestRecordsByRequestor
    [WebMethod(Description = "Retrieve reservation request records by requestor.")]
    public List<CRRequest> RetrieveCRRequestRecordsByRequestor(CRRequest request)
    {
        try
        {
            return request.RetrieveCRRequestRecordsByRequestor();
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region RetrieveCRRequestDetailsByRequestor
    [WebMethod(Description = "Retrieve reservation request details by reference number and requestor.")]
    public CRRequest RetrieveCRRequestDetailsByRequestor(CRRequest request)
    {
        try
        {
            CRRequest returnValue = new CRRequest();
            returnValue.RetrieveCRRequestDetailsByRequestor(request);

            return returnValue;
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region RetrieveCRRequestHistoryRecords
    [WebMethod(Description = "Retrieve reservation request history by request reference number.")]
    public List<CRRequestHistory> RetrieveCRRequestHistoryRecords(CRRequest request)
    {
        try
        {
            return request.RequestHistory.RetrieveCRRequestHistoryRecords();
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region RetrieveCRRequestAttachment
    [WebMethod(Description = "Retrieve reservation request history by request reference number.")]
    public List<CRRequestAttachment> RetrieveCRRequestAttachment(CRRequest request)
    {
        try
        {
            return request.Attachment.RetrieveCRRequestAttachment();
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region RetrieveCRRequestAttachmentByStatus
    [WebMethod(Description = "Retrieve reservation request attachment by status.")]
    public CRRequestAttachment RetrieveCRRequestAttachmentByStatus(CRRequest request)
    {
        try
        {
            CRRequestAttachment returnValue = new CRRequestAttachment();
            returnValue.RetrieveCRRequestAttachmentByStatus(request);

            return returnValue;
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region RetrieveCRRequestRecordsReport
    [WebMethod(Description = "Retrieve reservation request records by requestor.")]
    public List<CRRequest> RetrieveCRRequestRecordsReport(CRReport report)
    {
        try
        {
            return report.RetrieveCRRequestRecordsReport();
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region RetrieveCRRequestRecordsReportByDate
    [WebMethod(Description = "Retrieve reservation request records by requestor.")]
    public List<CRRequest> RetrieveCRRequestRecordsReportByDate(CRReport report)
    {
        try
        {
            return report.RetrieveCRRequestRecordsReportByDate();
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region RetrieveRequestCountByStatus
    [WebMethod(Description = "Retrieve conference room request count by status.")]
    public DataTable RetrieveRequestCountByStatus(int statusCode)
    {
        try
        {
            DAL dal = new DAL();
            return dal.RetrieveRequestCountByStatus(statusCode);
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region RetrieveCRTimeSlotDetails
    [WebMethod(Description = "Retrieve timeslot details by timeslot ID.")]
    public CRTimeSlot RetrieveCRTimeSlotDetails(CRTimeSlot timeSlot)
    {
        try
        {
            CRTimeSlot returnValue = new CRTimeSlot();
            returnValue.RetrieveCRTimeSlotDetails(timeSlot);

            return returnValue;
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region RetrieveEmployeeDetails
    [WebMethod(Description = "Retrieve employee details by employee ID.")]
    public EmployeeDetails RetrieveEmployeeDetails(EmployeeDetails employee)
    {
        try
        {
            EmployeeDetails returnValue = new EmployeeDetails();
            returnValue.GetRequestorDetails(employee.EmployeeID);

            return returnValue;
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region InsertAuditTrailEntry
    [WebMethod(Description = "Insert audit trail details.")]
    public bool InsertAuditTrailEntry(AuditTrail auditTrailDetails)
    {
        try
        {
            return auditTrailDetails.InsertAuditTrailEntry();
        }

        catch (Exception ex)
        {
            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            throw new SoapException(eventLog.Message, SoapException.ServerFaultCode, ex);
        }
    }
    #endregion

    #region SendEmailNotification
    [WebMethod(Description = "Insert audit trail details.")]
    public bool SendErrorNotification(int eventID, string rawError, string machineName, string eventSource)
    {
        bool returnValue;

        try
        {
            if (Settings.EventEmailErrorEnabled)
            {
                EmailNotification errorNotification = new EmailNotification();
                errorNotification.ConstructCommonErrorNotification(rawError, System.Diagnostics.EventLogEntryType.Error, eventID, machineName, eventSource);
                Functions.SendEmailNotification(errorNotification);
            }
            
            returnValue = true;
        }

        catch (Exception ex)
        {
            returnValue = false;
        }

        return returnValue;
    }
    #endregion

    #region RetrieveAccomodationRoomRecords
    [WebMethod(Description = "Retrieve accomodation room records.")]
    public RetrieveAccomodationRoomRecordsResult RetrieveAccomodationRoomRecords(RetrieveAccomodationRoomRecordsRequest retrieveAccomodationRoomRecordsRequest)
    {
        RetrieveAccomodationRoomRecordsResult returnValue = null;

        try
        {
            return retrieveAccomodationRoomRecordsRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new RetrieveAccomodationRoomRecordsResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region RetrieveAccomodationRoomRecordDetails
    [WebMethod(Description = "Retrieve accomodation room record details.")]
    public RetrieveAccomodationRoomRecordDetailsResult RetrieveAccomodationRoomRecordDetails(RetrieveAccomodationRoomRecordDetailsRequest retrieveAccomodationRoomRecordDetailsRequest)
    {
        RetrieveAccomodationRoomRecordDetailsResult returnValue = null;

        try
        {
            return retrieveAccomodationRoomRecordDetailsRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new RetrieveAccomodationRoomRecordDetailsResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region ValidateAccomodationRoomRecord
    [WebMethod(Description = "Valdiate accomodation room record.")]
    public ValidateAccomodationRoomRecordResult ValidateAccomodationRoomRecord(ValidateAccomodationRoomRecordRequest validateAccomodationRoomRecordRequest)
    {
        ValidateAccomodationRoomRecordResult returnValue = null;

        try
        {
            return validateAccomodationRoomRecordRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new ValidateAccomodationRoomRecordResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region AccomodationRoomTransaction
    [WebMethod(Description = "Add, edit, delete of accomodation room records.")]
    public AccomodationRoomTransactionResult AccomodationRoomTransaction(AccomodationRoomTransactionRequest accomodationRoomTransactionRequest)
    {
        AccomodationRoomTransactionResult returnValue = null;

        try
        {
            return accomodationRoomTransactionRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new AccomodationRoomTransactionResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region RetrieveTrainingRoomRecords
    [WebMethod(Description = "Retrieve training room records for grid view.")]
    public RetrieveTrainingRoomRecordsResult RetrieveTrainingRoomRecords(RetrieveTrainingRoomRecordsRequest retrieveTrainingRoomRecordsRequest)
    {
        RetrieveTrainingRoomRecordsResult returnValue = null;

        try
        {
            return retrieveTrainingRoomRecordsRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new RetrieveTrainingRoomRecordsResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region RetrieveTrainingRoomRecordDetails
    [WebMethod(Description = "Retrieve training room record details.")]
    public RetrieveTrainingRoomRecordDetailsResult RetrieveTrainingRoomRecordDetails(RetrieveTrainingRoomRecordDetailsRequest retrieveTrainingRoomRecordDetailsRequest)
    {
        RetrieveTrainingRoomRecordDetailsResult returnValue = null;

        try
        {
            return retrieveTrainingRoomRecordDetailsRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new RetrieveTrainingRoomRecordDetailsResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region ValidateTrainingRoomRecord
    [WebMethod(Description = "Validate training room record.")]
    public ValidateTrainingRoomRecordResult ValidateTrainingRoomRecord(ValidateTrainingRoomRecordRequest validateTrainingRoomRecordRequest)
    {
        ValidateTrainingRoomRecordResult returnValue = null;

        try
        {
            return validateTrainingRoomRecordRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new ValidateTrainingRoomRecordResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region TrainingRoomTransaction
    [WebMethod(Description = "Add, edit, delete of training room records.")]
    public TrainingRoomTransactionResult TrainingRoomTransaction(TrainingRoomTransactionRequest trainingRoomTransactionRequest)
    {
        TrainingRoomTransactionResult returnValue = null;

        try
        {
            return trainingRoomTransactionRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new TrainingRoomTransactionResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region RetrieveTRPartitionRecordDetails
    [WebMethod(Description = "Retrieve training room partition record details.")]
    public RetrieveTRPartitionRecordDetailsResult RetrieveTRPartitionRecordDetails(RetrieveTRPartitionRecordDetailsRequest retrieveTRPartitionRecordDetailsRequest)
    {
        RetrieveTRPartitionRecordDetailsResult returnValue = null;

        try
        {
            return retrieveTRPartitionRecordDetailsRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new RetrieveTRPartitionRecordDetailsResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region RetrieveAccRoomCalendarSchedule
    [WebMethod(Description = "Retrieve accomodation room calendar schedule.")]
    public RetrieveAccRoomCalendarScheduleResult RetrieveAccRoomCalendarSchedule(RetrieveAccRoomCalendarScheduleRequest retrieveAccRoomCalendarScheduleRequest)
    {
        RetrieveAccRoomCalendarScheduleResult returnValue = null;

        try
        {
            return retrieveAccRoomCalendarScheduleRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new RetrieveAccRoomCalendarScheduleResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region RetrieveTRCalendarSchedule
    [WebMethod(Description = "Retrieve training room calendar schedule.")]
    public RetrieveTRCalendarScheduleResult RetrieveTRCalendarSchedule(RetrieveTRCalendarScheduleRequest retrieveTRCalendarScheduleRequest)
    {
        RetrieveTRCalendarScheduleResult returnValue = null;

        try
        {
            return retrieveTRCalendarScheduleRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new RetrieveTRCalendarScheduleResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region ValidateTrainingRoomScheduleAvailability
    [WebMethod(Description = "Validate training room schedule availability.")]
    public ValidateTrainingRoomScheduleAvailabilityResult ValidateTrainingRoomScheduleAvailability(ValidateTrainingRoomScheduleAvailabilityRequest validateTrainingRoomScheduleAvailabilityRequest)
    {
        ValidateTrainingRoomScheduleAvailabilityResult returnValue = null;

        try
        {
            return validateTrainingRoomScheduleAvailabilityRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new ValidateTrainingRoomScheduleAvailabilityResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region ValidateAccomodationRoomScheduleAvailability
    [WebMethod(Description = "Validate accomodation room schedule availability.")]
    public ValidateAccomodationRoomScheduleAvailabilityResult ValidateAccomodationRoomScheduleAvailability(ValidateAccomodationRoomScheduleAvailabilityRequest validateAccomodationRoomScheduleAvailabilityRequest)
    {
        ValidateAccomodationRoomScheduleAvailabilityResult returnValue = null;

        try
        {
            return validateAccomodationRoomScheduleAvailabilityRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new ValidateAccomodationRoomScheduleAvailabilityResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region ValidateSummaryScheduleAvailability
    [WebMethod(Description = "Validate accomodation room schedule availability.")]
    public ValidateSummaryScheduleAvailabilityResult ValidateSummaryScheduleAvailability(ValidateSummaryScheduleAvailabilityRequest validateSummaryScheduleAvailabilityRequest)
    {
        ValidateSummaryScheduleAvailabilityResult returnValue = null;

        try
        {
            return validateSummaryScheduleAvailabilityRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new ValidateSummaryScheduleAvailabilityResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region RetrieveLastGeneratedCCRefNo
    [WebMethod(Description = "Retrieve last generated reference number for convention center.")]
    public RetrieveLastGeneratedCCRefNoResult RetrieveLastGeneratedCCRefNo(RetrieveLastGeneratedCCRefNoRequest retrieveLastGeneratedCCRefNoRequest)
    {
        RetrieveLastGeneratedCCRefNoResult returnValue = null;

        try
        {
            return retrieveLastGeneratedCCRefNoRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new RetrieveLastGeneratedCCRefNoResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region InsertNewCCRequest
    [WebMethod(Description = "Insert new convention center reservation request.")]
    public InsertNewCCRequestResult InsertNewCCRequest(InsertNewCCRequestRequest insertNewCCRequestRequest)
    {
        InsertNewCCRequestResult returnValue = null;

        try
        {
            return insertNewCCRequestRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new InsertNewCCRequestResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region CancelCCRequest
    [WebMethod(Description = "Cancel convention center reservation request.")]
    public CancelCCRequestResult CancelCCRequest(CancelCCRequestRequest cancelCCRequestRequest)
    {
        CancelCCRequestResult returnValue = null;

        try
        {
            return cancelCCRequestRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new CancelCCRequestResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region RetrieveCCRequestRecordsByStatus
    [WebMethod(Description = "Retrieve convention center request records by status.")]
    public RetrieveCCRequestRecordsByStatusResult RetrieveCCRequestRecordsByStatus(RetrieveCCRequestRecordsByStatusRequest retrieveCCRequestRecordsByStatusRequest)
    {
        RetrieveCCRequestRecordsByStatusResult returnValue = null;

        try
        {
            return retrieveCCRequestRecordsByStatusRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new RetrieveCCRequestRecordsByStatusResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region RetrieveCCRequestDetails
    [WebMethod(Description = "Retrieve convention center request details.")]
    public RetrieveCCRequestDetailsResult RetrieveCCRequestDetails(RetrieveCCRequestDetailsRequest retrieveCCRequestDetailsRequest)
    {
        RetrieveCCRequestDetailsResult returnValue = null;

        try
        {
            return retrieveCCRequestDetailsRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new RetrieveCCRequestDetailsResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region RetrieveCCRequestRecordsByRequestor
    [WebMethod(Description = "Retrieve convention center request records by requestor.")]
    public RetrieveCCRequestRecordsByRequestorResult RetrieveCCRequestRecordsByRequestor(RetrieveCCRequestRecordsByRequestorRequest retrieveCCRequestRecordsByRequestorRequest)
    {
        RetrieveCCRequestRecordsByRequestorResult returnValue = null;

        try
        {
            return retrieveCCRequestRecordsByRequestorRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new RetrieveCCRequestRecordsByRequestorResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region RetrieveCCRequestDetailsByRequestor
    [WebMethod(Description = "Retrieve convention center request records by requestor.")]
    public RetrieveCCRequestDetailsByRequestorResult RetrieveCCRequestDetailsByRequestor(RetrieveCCRequestDetailsByRequestorRequest retrieveCCRequestDetailsByRequestorRequest)
    {
        RetrieveCCRequestDetailsByRequestorResult returnValue = null;

        try
        {
            return retrieveCCRequestDetailsByRequestorRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new RetrieveCCRequestDetailsByRequestorResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region RetrieveCCRequestHistoryRecords
    [WebMethod(Description = "Retrieve convention center request status history.")]
    public RetrieveCCRequestHistoryRecordsResult RetrieveCCRequestHistoryRecords(RetrieveCCRequestHistoryRecordsRequest retrieveCCRequestHistoryRecordsRequest)
    {
        RetrieveCCRequestHistoryRecordsResult returnValue = null;

        try
        {
            return retrieveCCRequestHistoryRecordsRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new RetrieveCCRequestHistoryRecordsResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region RetrieveCCRequestAttachment
    [WebMethod(Description = "Retrieve convention center request attachment.")]
    public RetrieveCCRequestAttachmentResult RetrieveCCRequestAttachment(RetrieveCCRequestAttachmentRequest retrieveCCRequestAttachmentRequest)
    {
        RetrieveCCRequestAttachmentResult returnValue = null;

        try
        {
            return retrieveCCRequestAttachmentRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new RetrieveCCRequestAttachmentResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region RetrieveCCRequestAttachmentByStatus
    [WebMethod(Description = "Retrieve convention center request attachment by status.")]
    public RetrieveCCRequestAttachmentByStatusResult RetrieveCCRequestAttachmentByStatus(RetrieveCCRequestAttachmentByStatusRequest retrieveCCRequestAttachmentByStatusRequest)
    {
        RetrieveCCRequestAttachmentByStatusResult returnValue = null;

        try
        {
            return retrieveCCRequestAttachmentByStatusRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new RetrieveCCRequestAttachmentByStatusResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region RetrieveTrainingRoomRequestReport
    [WebMethod(Description = "Retrieve training room request report.")]
    public RetrieveTrainingRoomRequestReportResult RetrieveTrainingRoomRequestReport(RetrieveTrainingRoomRequestReportRequest retrieveTrainingRoomRequestReportRequest)
    {
        RetrieveTrainingRoomRequestReportResult returnValue = null;

        try
        {
            return retrieveTrainingRoomRequestReportRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new RetrieveTrainingRoomRequestReportResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region RetrieveAccomodationRoomRequestReport
    [WebMethod(Description = "Retrieve accomodation room request report.")]
    public RetrieveAccomodationRoomRequestReportResult RetrieveAccomodationRoomRequestReport(RetrieveAccomodationRoomRequestReportRequest retrieveAccomodationRoomRequestReportRequest)
    {
        RetrieveAccomodationRoomRequestReportResult returnValue = null;

        try
        {
            return retrieveAccomodationRoomRequestReportRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new RetrieveAccomodationRoomRequestReportResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region RetrieveCCRequestRecordsBySOAStatus
    [WebMethod(Description = "Retrieve convention center request records by SOA status.")]
    public RetrieveCCRequestRecordsBySOAStatusResult RetrieveCCRequestRecordsBySOAStatus(RetrieveCCRequestRecordsBySOAStatusRequest retrieveCCRequestRecordsBySOAStatusRequest)
    {
        RetrieveCCRequestRecordsBySOAStatusResult returnValue = null;

        try
        {
            return retrieveCCRequestRecordsBySOAStatusRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new RetrieveCCRequestRecordsBySOAStatusResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region RetrieveCCRequestRecordsApprovedSOA
    [WebMethod(Description = "Retrieve approved SOA convention center request records.")]
    public RetrieveCCRequestRecordsApprovedSOAResult RetrieveCCRequestRecordsApprovedSOA(RetrieveCCRequestRecordsApprovedSOARequest retrieveCCRequestRecordsApprovedSOARequest)
    {
        RetrieveCCRequestRecordsApprovedSOAResult returnValue = null;

        try
        {
            return retrieveCCRequestRecordsApprovedSOARequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new RetrieveCCRequestRecordsApprovedSOAResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region RetrieveSOAHistoryRecords
    [WebMethod(Description = "Retrieve convention center request SOA status history.")]
    public RetrieveSOAHistoryRecordsResult RetrieveSOAHistoryRecords(RetrieveSOAHistoryRecordsRequest retrieveSOAHistoryRecordsRequest)
    {
        RetrieveSOAHistoryRecordsResult returnValue = null;

        try
        {
            return retrieveSOAHistoryRecordsRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new RetrieveSOAHistoryRecordsResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region RetrieveSOADetails
    [WebMethod(Description = "Retrieve request SOA details.")]
    public RetrieveSOADetailsResult RetrieveSOADetails(RetrieveSOADetailsRequest retrieveSOADetailsRequest)
    {
        RetrieveSOADetailsResult returnValue = null;

        try
        {
            return retrieveSOADetailsRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new RetrieveSOADetailsResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region OtherChargeTransaction
    [WebMethod(Description = "Add or delete Other Charge records.")]
    public OtherChargeTransactionResult OtherChargeTransaction(OtherChargeTransactionRequest otherChargeTransactionRequest)
    {
        OtherChargeTransactionResult returnValue = null;

        try
        {
            return otherChargeTransactionRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new OtherChargeTransactionResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region UpdateSOAStatus
    [WebMethod(Description = "Update SOA status.")]
    public UpdateSOAStatusResult UpdateSOAStatus(UpdateSOAStatusRequest updateSOAStatusRequest)
    {
        UpdateSOAStatusResult returnValue = null;

        try
        {
            return updateSOAStatusRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new UpdateSOAStatusResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region RetrieveSOAReport
    [WebMethod(Description = "Retrieve SOA summary report.")]
    public RetrieveSOAReportResult RetrieveSOAReport(RetrieveSOAReportRequest retrieveSOAReportRequest)
    {
        RetrieveSOAReportResult returnValue = null;

        try
        {
            return retrieveSOAReportRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new RetrieveSOAReportResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region TrainingRoomScheduleMappingTransaction
    [WebMethod(Description = "Insert or delete training room schedule mapping records.")]
    public TrainingRoomScheduleMappingTransactionResult TrainingRoomScheduleMappingTransaction(TrainingRoomScheduleMappingTransactionRequest trainingRoomScheduleMappingTransactionRequest)
    {
        TrainingRoomScheduleMappingTransactionResult returnValue = null;

        try
        {
            return trainingRoomScheduleMappingTransactionRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new TrainingRoomScheduleMappingTransactionResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region AccomodationRoomScheduleMappingTransaction
    [WebMethod(Description = "Insert or delete accomodation room schedule mapping records.")]
    public AccomodationRoomScheduleMappingTransactionResult AccomodationRoomScheduleMappingTransaction(AccomodationRoomScheduleMappingTransactionRequest accomodationRoomScheduleMappingTransactionRequest)
    {
        AccomodationRoomScheduleMappingTransactionResult returnValue = null;

        try
        {
            return accomodationRoomScheduleMappingTransactionRequest.Process();
        }

        catch (Exception ex)
        {
            returnValue = new AccomodationRoomScheduleMappingTransactionResult();

            SystemEventLog eventLog = new SystemEventLog();
            eventLog.WrapServerError(ex.ToString());

            returnValue.LogID = eventLog.EventID;
            returnValue.Message = eventLog.Message;
            returnValue.ResultStatus = ResultStatus.Error;

            return returnValue;
        }
    }
    #endregion

    #region Codebox
    [WebMethod(Description = "Displays Code Box details.")]
    public void Codebox()
    {
        ResponseWrite(String.Format("{0}\n\n{1}\n{2}\n{3}\n{4}\n{5}\n{6}\n{7}\n{8}\n{9}\n{10}\n{11}"
                                      , "Code Box"
                                      , "OS                       :Windows"
                                      , "IDE                      :Microsoft Visual Studio 2010"
                                      , "Language                 :C#"
                                      , "Framework                :.NET Framework 2.0"
                                      , "Application Name         :iReserveWS"
                                      , "Solution Name            :iReserve"
                                      , "Current Version          :4.0"
                                      , "Last Update Date         :August 31, 2017"
                                      , "Team Code                :ASD"
                                      , "Programmer's Intitial    :KAE"
                                      , "Remarks:                 :Antipolo Convention Center Module"));
    }
    #endregion

    #region MethodName
    //[WebMethod(Description = "")]
    //public String MethodName(String s)
    //{
    //    return s;
    //}
    #endregion

}