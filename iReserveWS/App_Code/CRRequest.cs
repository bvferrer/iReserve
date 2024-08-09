using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Data.SqlClient;

/// <summary>
/// Summary description for CRRequest
/// </summary>
public class CRRequest
{

  public CRRequest()
  {
    this.ConferenceRoom = new ConferenceRoom();
    this.StartTime = new CRTimeSlot();
    this.EndTime = new CRTimeSlot();
    this.CostCenter = new CostCenter();
    this.Status = new Status();
    this.ChargedCompanyCostCenter = new ChargedCompanyCostCenter();
  }

  #region Properties

  private int _requestID;

  public int RequestID
  {
    get { return _requestID; }
    set { _requestID = value; }
  }

  private string _requestReferenceNo;

  public string RequestReferenceNo
  {
    get { return _requestReferenceNo; }
    set { _requestReferenceNo = value; }
  }

  private ConferenceRoom _conferenceRoom;

  public ConferenceRoom ConferenceRoom
  {
    get { return _conferenceRoom; }
    set { _conferenceRoom = value; }
  }

  private DateTime _date;

  public DateTime Date
  {
    get { return _date; }
    set { _date = value; }
  }

  private CRTimeSlot _startTime;

  public CRTimeSlot StartTime
  {
    get { return _startTime; }
    set { _startTime = value; }
  }

  private CRTimeSlot _endTime;

  public CRTimeSlot EndTime
  {
    get { return _endTime; }
    set { _endTime = value; }
  }

  private int _numberOfHours;

  public int NumberOfHours
  {
    get { return _numberOfHours; }
    set { _numberOfHours = value; }
  }

  private int _numberOfHoursExtended;

  public int NumberOfHoursExtended
  {
    get { return _numberOfHoursExtended; }
    set { _numberOfHoursExtended = value; }
  }

  private string _agenda;

  public string Agenda
  {
    get { return _agenda; }
    set { _agenda = value; }
  }

  private int _headCount;

  public int HeadCount
  {
    get { return _headCount; }
    set { _headCount = value; }
  }

  private string _isUseDataPort;

  public string IsUseDataPort
  {
    get { return _isUseDataPort; }
    set { _isUseDataPort = value; }
  }

  private string _isUseMonitor;

  public string IsUseMonitor
  {
    get { return _isUseMonitor; }
    set { _isUseMonitor = value; }
  }

  private CostCenter _costCenter;

  public CostCenter CostCenter
  {
    get { return _costCenter; }
    set { _costCenter = value; }
  }

  private Status _status;

  public Status Status
  {
    get { return _status; }
    set { _status = value; }
  }

  private string _hasLoggedIn;

  public string HasLoggedIn
  {
    get { return _hasLoggedIn; }
    set { _hasLoggedIn = value; }
  }

  private string _requestedByID;

  public string RequestedByID
  {
    get { return _requestedByID; }
    set { _requestedByID = value; }
  }

  private string _requestedBy;

  public string RequestedBy
  {
    get { return _requestedBy; }
    set { _requestedBy = value; }
  }

  private DateTime _dateRequested;

  public DateTime DateRequested
  {
    get { return _dateRequested; }
    set { _dateRequested = value; }
  }

  private DateTime _dateConfirmed;

  public DateTime DateConfirmed
  {
    get { return _dateConfirmed; }
    set { _dateConfirmed = value; }
  }

  private DateTime _dateDeclined;

  public DateTime DateDeclined
  {
    get { return _dateDeclined; }
    set { _dateDeclined = value; }
  }

  private DateTime _dateCancelled;

  public DateTime DateCancelled
  {
    get { return _dateCancelled; }
    set { _dateCancelled = value; }
  }

  private DateTime _dateLoggedIn;

  public DateTime DateLoggedIn
  {
    get { return _dateLoggedIn; }
    set { _dateLoggedIn = value; }
  }

  private CRRequestAttachment _attachment;

  public CRRequestAttachment Attachment
  {
    get { return _attachment; }
    set { _attachment = value; }
  }

  private List<CRRequestAttendee> _attendeeList;

  public List<CRRequestAttendee> AttendeeList
  {
    get { return _attendeeList; }
    set { _attendeeList = value; }
  }

  private CRRequestHistory _requestHistory;

  public CRRequestHistory RequestHistory
  {
    get { return _requestHistory; }
    set { _requestHistory = value; }
  }

  private int _chargedCompanyCostCenterID;
  public int ChargedCompanyCostCenterID
  {
    get { return _chargedCompanyCostCenterID; }
    set { _chargedCompanyCostCenterID = value; }
  }
  private int _chargedCompanyID;
  public int ChargedCompanyID
  {
    get { return _chargedCompanyID; }
    set { _chargedCompanyID = value; }
  }

  private ChargedCompanyCostCenter _chargedCompanyCostCenter;
  public ChargedCompanyCostCenter ChargedCompanyCostCenter
  {
    get
    {
      return _chargedCompanyCostCenter;
    }
    set
    {
      _chargedCompanyCostCenter = value;
    }
  }
  #endregion

  #region Methods

  public void InsertCRRequest()
  {
    using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringWriter))
    {
      sqlConnection.Open();

      using (SqlCommand sqlCommand = new SqlCommand(Common.usp_InsertCRRequest, sqlConnection))
      {
        sqlCommand.CommandType = CommandType.StoredProcedure;
        sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.RequestReferenceNo));
        sqlCommand.Parameters.AddWithValue("@roomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.ConferenceRoom.RoomID));
        sqlCommand.Parameters.AddWithValue("@date", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(this.Date));
        sqlCommand.Parameters.AddWithValue("@startTimeID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.StartTime.TimeSlotID));
        sqlCommand.Parameters.AddWithValue("@endTimeID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.EndTime.TimeSlotID));
        sqlCommand.Parameters.AddWithValue("@agenda", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.Agenda));
        sqlCommand.Parameters.AddWithValue("@headCount", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.HeadCount));
        sqlCommand.Parameters.AddWithValue("@isUseDataPort", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.IsUseDataPort));
        sqlCommand.Parameters.AddWithValue("@isUseMonitor", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.IsUseMonitor));
        sqlCommand.Parameters.AddWithValue("@costCenterID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.CostCenter.CostCenterID));
        sqlCommand.Parameters.AddWithValue("@statusCode", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.Status.StatusCode));
        sqlCommand.Parameters.AddWithValue("@requestedByID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.RequestedByID));
        sqlCommand.Parameters.AddWithValue("@requestedBy", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.RequestedBy));
        sqlCommand.Parameters.AddWithValue("@chargedCompanyCostCenterID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.ChargedCompanyCostCenterID));
        sqlCommand.Parameters.AddWithValue("@chargedCompanyID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.ChargedCompanyID));
        sqlCommand.ExecuteNonQuery();
      }
    }
  }

  public void InsertCRRequestAttendeeList()
  {
    foreach (CRRequestAttendee attendee in this.AttendeeList)
    {
      attendee.InsertCRRequestAttendee();
    }
  }

  public void RetrieveCRRequestDetails(string requestReferenceNo)
  {
    using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
    {
      using (SqlCommand sqlCommand = new SqlCommand(Common.usp_RetrieveCRRequestDetails, sqlConnection))
      {
        sqlConnection.Open();
        sqlCommand.CommandType = CommandType.StoredProcedure;
        sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(requestReferenceNo));

        using (SqlDataReader rd = sqlCommand.ExecuteReader())
        {
          if (rd.Read())
          {
            this.RequestReferenceNo = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RequestReferenceNo"]);
            this.ConferenceRoom.RoomID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_RoomID"]);
            this.ConferenceRoom.RoomName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RoomName"]);
            this.Date = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_Date"]);
            this.StartTime.TimeSlotID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StartTimeID"]);
            this.StartTime.StartTime = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StartTime"]);
            this.StartTime.StartTime12 = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StartTime12"]);
            this.EndTime.TimeSlotID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_EndTimeID"]);
            this.EndTime.EndTime = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_EndTime"]);
            this.EndTime.EndTime12 = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_EndTime12"]);
            this.NumberOfHours = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_NumberofHours"]);
            this.Agenda = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_Agenda"]);
            this.HeadCount = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_HeadCount"]);
            this.IsUseDataPort = RDFramework.Utility.Conversion.SafeReadDatabaseValue<bool>(rd["fld_IsUseDataPort"]).ToString();
            this.IsUseMonitor = RDFramework.Utility.Conversion.SafeReadDatabaseValue<bool>(rd["fld_IsUseMonitor"]).ToString();
            this.CostCenter.CostCenterID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_CostCenterID"]);
            this.CostCenter.CostCenterName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CostCenterName"]);
            this.Status.StatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StatusCode"]);
            this.Status.StatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StatusName"]);
            this.HasLoggedIn = RDFramework.Utility.Conversion.SafeReadDatabaseValue<bool>(rd["fld_HasLoggedIn"]).ToString();
            this.RequestedByID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RequestedByID"]);
            this.RequestedBy = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RequestedBy"]);
            this.DateRequested = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateRequested"]);
            this.DateConfirmed = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateConfirmed"]);
            this.DateDeclined = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateDeclined"]);
            this.DateCancelled = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateCancelled"]);
            this.ChargedCompanyCostCenter.CostCenterName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_ChargedCompanyCostCenterName"]);

          }
        }
      }
    }
  }

  public List<CRRequest> RetrieveCRRequestRecordsByStatus()
  {
    List<CRRequest> requestList = new List<CRRequest>();

    using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
    {
      using (SqlCommand sqlCommand = new SqlCommand(Common.usp_RetrieveCRRequestRecordsByStatus, sqlConnection))
      {
        sqlConnection.Open();
        sqlCommand.CommandType = CommandType.StoredProcedure;
        sqlCommand.Parameters.AddWithValue("@statusCode", this.Status.StatusCode);

        using (SqlDataReader rd = sqlCommand.ExecuteReader())
        {
          while (rd.Read())
          {
            CRRequest request = new CRRequest();
            request.RequestReferenceNo = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RequestReferenceNo"]);
            request.ConferenceRoom.RoomID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_RoomID"]);
            request.ConferenceRoom.RoomName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RoomName"]);
            request.Date = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_Date"]);
            request.StartTime.TimeSlotID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StartTimeID"]);
            request.StartTime.StartTime = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StartTime"]);
            request.StartTime.StartTime12 = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StartTime12"]);
            request.EndTime.TimeSlotID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_EndTimeID"]);
            request.EndTime.EndTime = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_EndTime"]);
            request.EndTime.EndTime12 = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_EndTime12"]);
            request.NumberOfHours = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_NumberofHours"]);
            request.Agenda = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_Agenda"]);
            request.HeadCount = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_HeadCount"]);
            request.IsUseDataPort = RDFramework.Utility.Conversion.SafeReadDatabaseValue<bool>(rd["fld_IsUseDataPort"]).ToString();
            request.IsUseMonitor = RDFramework.Utility.Conversion.SafeReadDatabaseValue<bool>(rd["fld_IsUseMonitor"]).ToString();
            request.CostCenter.CostCenterID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_CostCenterID"]);
            request.CostCenter.CostCenterName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CostCenterName"]);
            request.Status.StatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StatusCode"]);
            request.Status.StatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StatusName"]);
            request.HasLoggedIn = RDFramework.Utility.Conversion.SafeReadDatabaseValue<bool>(rd["fld_HasLoggedIn"]).ToString();
            request.RequestedByID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RequestedByID"]);
            request.RequestedBy = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RequestedBy"]);
            request.DateRequested = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateRequested"]);
            request.DateConfirmed = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateConfirmed"]);
            request.DateDeclined = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateDeclined"]);
            request.DateCancelled = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateCancelled"]);
            requestList.Add(request);
          }
        }
      }
    }

    return requestList;
  }

  public void RetrieveCRRequestAttendees()
  {
    List<CRRequestAttendee> attendeeList = new List<CRRequestAttendee>();

    using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
    {
      using (SqlCommand sqlCommand = new SqlCommand(Common.usp_RetrieveCRRequestAttendees, sqlConnection))
      {
        sqlConnection.Open();
        sqlCommand.CommandType = CommandType.StoredProcedure;
        sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.RequestReferenceNo));

        using (SqlDataReader rd = sqlCommand.ExecuteReader())
        {
          while (rd.Read())
          {
            CRRequestAttendee attendee = new CRRequestAttendee();
            attendee.AttendeeID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_AttendeeID"]);
            attendee.RequestReferenceNumber = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RequestReferenceNo"]);
            attendee.FullName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_FullName"]);
            attendee.Company = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_Company"]);
            attendeeList.Add(attendee);
          }
        }
      }
    }

    this.AttendeeList = attendeeList;
  }

  public void UpdateCRRequestStatus()
  {
    using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringWriter))
    {
      sqlConnection.Open();

      using (SqlCommand sqlCommand = new SqlCommand(Common.usp_UpdateCRRequestStatus, sqlConnection))
      {
        sqlCommand.CommandType = CommandType.StoredProcedure;
        sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.RequestReferenceNo));
        sqlCommand.Parameters.AddWithValue("@statusCode", this.Status.StatusCode);
        sqlCommand.ExecuteNonQuery();
      }
    }
  }

  public void InsertScheduleMapping()
  {
    CRScheduleMapping scheduleMapping = new CRScheduleMapping();
    scheduleMapping.ConferenceRoom.RoomID = this.ConferenceRoom.RoomID;
    scheduleMapping.Date = this.Date;
    scheduleMapping.ReferenceNumber = this.RequestReferenceNo;

    int timeSlot = this.StartTime.TimeSlotID;
    int endTimeSlot = this.EndTime.TimeSlotID;

    while (timeSlot <= endTimeSlot)
    {
      scheduleMapping.TimeSlot.TimeSlotID = timeSlot;
      scheduleMapping.InsertCRScheduleMapping();

      timeSlot++;
    }
  }

  public void UpdateScheduleMappingStatus()
  {
    CRScheduleMapping scheduleMapping = new CRScheduleMapping();
    scheduleMapping.ReferenceNumber = this.RequestReferenceNo;
    scheduleMapping.UpdateCRScheduleMappingStatus();
  }

  public bool ValidateScheduleAvailability()
  {
    bool validationStatus = false;

    using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
    {
      using (SqlCommand sqlCommand = new SqlCommand(Common.usp_ValidateScheduleAvailability, sqlConnection))
      {
        sqlConnection.Open();
        sqlCommand.CommandType = CommandType.StoredProcedure;
        sqlCommand.Parameters.AddWithValue("@roomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.ConferenceRoom.RoomID));
        sqlCommand.Parameters.AddWithValue("@date", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(this.Date));
        sqlCommand.Parameters.AddWithValue("@startTimeID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.StartTime.TimeSlotID));
        sqlCommand.Parameters.AddWithValue("@endTimeID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.EndTime.TimeSlotID));

        using (SqlDataReader rd = sqlCommand.ExecuteReader())
        {
          while (rd.Read())
          {
            validationStatus = RDFramework.Utility.Conversion.SafeReadDatabaseValue<bool>(rd["ValidationStatus"]);
          }
        }
      }
    }

    return validationStatus;
  }

  public List<CRRequest> RetrieveCRRequestRecordsByRequestor()
  {
    List<CRRequest> requestList = new List<CRRequest>();

    using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
    {
      using (SqlCommand sqlCommand = new SqlCommand(Common.usp_RetrieveCRRequestRecordsByRequestor, sqlConnection))
      {
        sqlConnection.Open();
        sqlCommand.CommandType = CommandType.StoredProcedure;
        sqlCommand.Parameters.AddWithValue("@requestorID", this.RequestedByID);
        sqlCommand.Parameters.AddWithValue("@statusCode", this.Status.StatusCode);

        using (SqlDataReader rd = sqlCommand.ExecuteReader())
        {
          while (rd.Read())
          {
            CRRequest request = new CRRequest();
            request.RequestReferenceNo = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RequestReferenceNo"]);
            request.ConferenceRoom.RoomID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_RoomID"]);
            request.ConferenceRoom.RoomName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RoomName"]);
            request.Date = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_Date"]);
            request.StartTime.TimeSlotID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StartTimeID"]);
            request.StartTime.StartTime = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StartTime"]);
            request.StartTime.StartTime12 = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StartTime12"]);
            request.EndTime.TimeSlotID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_EndTimeID"]);
            request.EndTime.EndTime = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_EndTime"]);
            request.EndTime.EndTime12 = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_EndTime12"]);
            request.NumberOfHours = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_NumberofHours"]);
            request.Agenda = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_Agenda"]);
            request.HeadCount = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_HeadCount"]);
            request.IsUseDataPort = RDFramework.Utility.Conversion.SafeReadDatabaseValue<bool>(rd["fld_IsUseDataPort"]).ToString();
            request.IsUseMonitor = RDFramework.Utility.Conversion.SafeReadDatabaseValue<bool>(rd["fld_IsUseMonitor"]).ToString();
            request.CostCenter.CostCenterID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_CostCenterID"]);
            request.CostCenter.CostCenterName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CostCenterName"]);
            request.Status.StatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StatusCode"]);
            request.Status.StatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StatusName"]);
            request.HasLoggedIn = RDFramework.Utility.Conversion.SafeReadDatabaseValue<bool>(rd["fld_HasLoggedIn"]).ToString();
            request.RequestedByID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RequestedByID"]);
            request.RequestedBy = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RequestedBy"]);
            request.DateRequested = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateRequested"]);
            request.DateConfirmed = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateConfirmed"]);
            request.DateDeclined = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateDeclined"]);
            request.DateCancelled = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateCancelled"]);
            requestList.Add(request);
          }
        }
      }
    }

    return requestList;
  }

  public void RetrieveCRRequestDetailsByRequestor(CRRequest request)
  {
    using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
    {
      using (SqlCommand sqlCommand = new SqlCommand(Common.usp_RetrieveCRRequestDetailsByRequestor, sqlConnection))
      {
        sqlConnection.Open();
        sqlCommand.CommandType = CommandType.StoredProcedure;
        sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(request.RequestReferenceNo));
        sqlCommand.Parameters.AddWithValue("@requestorID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(request.RequestedByID));

        using (SqlDataReader rd = sqlCommand.ExecuteReader())
        {
          while (rd.Read())
          {
            this.RequestReferenceNo = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RequestReferenceNo"]);
            this.ConferenceRoom.RoomID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_RoomID"]);
            this.ConferenceRoom.RoomName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RoomName"]);
            this.Date = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_Date"]);
            this.StartTime.TimeSlotID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StartTimeID"]);
            this.StartTime.StartTime = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StartTime"]);
            this.StartTime.StartTime12 = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StartTime12"]);
            this.EndTime.TimeSlotID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_EndTimeID"]);
            this.EndTime.EndTime = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_EndTime"]);
            this.EndTime.EndTime12 = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_EndTime12"]);
            this.NumberOfHours = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_NumberofHours"]);
            this.Agenda = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_Agenda"]);
            this.HeadCount = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_HeadCount"]);
            this.IsUseDataPort = RDFramework.Utility.Conversion.SafeReadDatabaseValue<bool>(rd["fld_IsUseDataPort"]).ToString();
            this.IsUseMonitor = RDFramework.Utility.Conversion.SafeReadDatabaseValue<bool>(rd["fld_IsUseMonitor"]).ToString();
            this.CostCenter.CostCenterID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_CostCenterID"]);
            this.CostCenter.CostCenterName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CostCenterName"]);
            this.Status.StatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StatusCode"]);
            this.Status.StatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StatusName"]);
            this.HasLoggedIn = RDFramework.Utility.Conversion.SafeReadDatabaseValue<bool>(rd["fld_HasLoggedIn"]).ToString();
            this.RequestedByID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RequestedByID"]);
            this.RequestedBy = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RequestedBy"]);
            this.DateRequested = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateRequested"]);
            this.DateConfirmed = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateConfirmed"]);
            this.DateDeclined = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateDeclined"]);
            this.DateCancelled = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateCancelled"]);
          }
        }
      }
    }
  }

  public List<CRRequest> RetrieveCRSimilarPendingRequests()
  {
    List<CRRequest> requestList = new List<CRRequest>();

    using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
    {
      using (SqlCommand sqlCommand = new SqlCommand(Common.usp_RetrieveCRSimilarPendingRequests, sqlConnection))
      {
        sqlConnection.Open();
        sqlCommand.CommandType = CommandType.StoredProcedure;
        sqlCommand.Parameters.AddWithValue("@roomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.ConferenceRoom.RoomID));
        sqlCommand.Parameters.AddWithValue("@date", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(this.Date));
        sqlCommand.Parameters.AddWithValue("@startTimeID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.StartTime.TimeSlotID));
        sqlCommand.Parameters.AddWithValue("@endTimeID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.EndTime.TimeSlotID));
        sqlCommand.Parameters.AddWithValue("@statusCode", StatusCode.ForConfirmation);

        using (SqlDataReader rd = sqlCommand.ExecuteReader())
        {
          while (rd.Read())
          {
            CRRequest request = new CRRequest();
            request.RequestReferenceNo = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RequestReferenceNo"]);
            request.ConferenceRoom.RoomID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_RoomID"]);
            request.ConferenceRoom.RoomName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RoomName"]);
            request.Date = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_Date"]);
            request.StartTime.TimeSlotID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StartTimeID"]);
            request.StartTime.StartTime = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StartTime"]);
            request.StartTime.StartTime12 = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StartTime12"]);
            request.EndTime.TimeSlotID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_EndTimeID"]);
            request.EndTime.EndTime = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_EndTime"]);
            request.EndTime.EndTime12 = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_EndTime12"]);
            request.NumberOfHours = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_NumberofHours"]);
            request.Agenda = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_Agenda"]);
            request.HeadCount = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_HeadCount"]);
            request.IsUseDataPort = RDFramework.Utility.Conversion.SafeReadDatabaseValue<bool>(rd["fld_IsUseDataPort"]).ToString();
            request.IsUseMonitor = RDFramework.Utility.Conversion.SafeReadDatabaseValue<bool>(rd["fld_IsUseMonitor"]).ToString();
            request.CostCenter.CostCenterID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_CostCenterID"]);
            request.CostCenter.CostCenterName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CostCenterName"]);
            request.Status.StatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StatusCode"]);
            request.Status.StatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StatusName"]);
            request.HasLoggedIn = RDFramework.Utility.Conversion.SafeReadDatabaseValue<bool>(rd["fld_HasLoggedIn"]).ToString();
            request.RequestedByID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RequestedByID"]);
            request.RequestedBy = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RequestedBy"]);
            request.DateRequested = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateRequested"]);
            request.DateConfirmed = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateConfirmed"]);
            request.DateDeclined = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateDeclined"]);
            request.DateCancelled = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateCancelled"]);
            requestList.Add(request);
          }
        }
      }
    }

    return requestList;
  }

  public void DeclineCRSimilarPendingRequests()
  {
    List<CRRequest> similarRequestList = this.RetrieveCRSimilarPendingRequests();

    foreach (CRRequest similarRequest in similarRequestList)
    {
      similarRequest.Status.StatusCode = StatusCode.Declined;
      similarRequest.RequestHistory = new CRRequestHistory();
      similarRequest.RequestHistory.RequestReferenceNumber = similarRequest.RequestReferenceNo;
      similarRequest.RequestHistory.Status.StatusCode = StatusCode.Declined;
      similarRequest.RequestHistory.ProcessedByID = this.RequestHistory.ProcessedByID;
      similarRequest.RequestHistory.ProcessedBy = this.RequestHistory.ProcessedBy;
      similarRequest.RequestHistory.Remarks = Settings.GenericDeclineMessage;

      similarRequest.UpdateCRRequestStatus();
      similarRequest.RequestHistory.InsertCRRequestHistory();

      #region Audit Trail

      bool isSuccess = false;

      AuditTrail auditTrail = new AuditTrail();
      auditTrail.ActionDate = DateTime.Now;
      auditTrail.ActionTaken = "Declined the request";
      auditTrail.ActionDetails = "Reference Number: " + similarRequest.RequestReferenceNo + " || Status: Declined || Auto declined upon confirmation of the request " + this.RequestReferenceNo;
      auditTrail.Browser = HttpContext.Current.Request.Browser.Browser;
      auditTrail.BrowserVersion = HttpContext.Current.Request.Browser.Version;
      auditTrail.IpAddress = HttpContext.Current.Request.ServerVariables[32];
      System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();

      auditTrail.MacAdress = "n/a";
      auditTrail.UserID = "iReserve";

      Service svc = new Service();

      isSuccess = svc.InsertAuditTrailEntry(auditTrail);

      #endregion
    }

    foreach (CRRequest similarRequest in similarRequestList)
    {
      similarRequest.RetrieveCRRequestDetails(similarRequest.RequestReferenceNo);

      EmailNotification emailNotificationDecline = new EmailNotification();
      emailNotificationDecline.SendEmailNotification(similarRequest);
    }
  }

  #endregion
}