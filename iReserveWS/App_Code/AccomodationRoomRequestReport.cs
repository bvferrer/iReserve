using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for AccomodationRoomRequestReport
/// </summary>
public class AccomodationRoomRequestReport
{
  public AccomodationRoomRequestReport()
  {
    //
    // TODO: Add constructor logic here
    //
    _chargedCompanyCostCenter = new ChargedCompanyCostCenter();
  }

  #region Properties

  private string _ccRequestReferenceNo;

  public string CCRequestReferenceNo
  {
    get { return _ccRequestReferenceNo; }
    set { _ccRequestReferenceNo = value; }
  }

  private string _eventName;

  public string EventName
  {
    get { return _eventName; }
    set { _eventName = value; }
  }

  private string _roomName;

  public string RoomName
  {
    get { return _roomName; }
    set { _roomName = value; }
  }

  private DateTime _startDate;

  public DateTime StartDate
  {
    get { return _startDate; }
    set { _startDate = value; }
  }

  private DateTime _endDate;

  public DateTime EndDate
  {
    get { return _endDate; }
    set { _endDate = value; }
  }

  private int _numberOfNights;

  public int NumberOfNights
  {
    get { return _numberOfNights; }
    set { _numberOfNights = value; }
  }

  private string _statusName;

  public string StatusName
  {
    get { return _statusName; }
    set { _statusName = value; }
  }

  private string _costCenterName;

  public string CostCenterName
  {
    get { return _costCenterName; }
    set { _costCenterName = value; }
  }

  private DateTime _dateCreated;

  public DateTime DateCreated
  {
    get { return _dateCreated; }
    set { _dateCreated = value; }
  }

  private DateTime _dateCancelled;

  public DateTime DateCancelled
  {
    get { return _dateCancelled; }
    set { _dateCancelled = value; }
  }

  private ChargedCompanyCostCenter _chargedCompanyCostCenter;
  public ChargedCompanyCostCenter ChargedCompanyCostCenter
  {
    get { return _chargedCompanyCostCenter; }
    set { _chargedCompanyCostCenter = value; }
  }

  #endregion

  #region Methods

  public List<AccomodationRoomRequestReport> RetrieveAccomodationRoomRequestReport(string selectedStatus, DateTime startDate, DateTime endDate)
  {
    List<AccomodationRoomRequestReport> accomodationRoomRequestReportList = new List<AccomodationRoomRequestReport>();

    using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
    {
      using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveAccomodationRoomRequestReport, sqlConnection))
      {
        sqlConnection.Open();
        sqlCommand.CommandType = CommandType.StoredProcedure;
        sqlCommand.Parameters.AddWithValue("@selectedStatus", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(selectedStatus));
        sqlCommand.Parameters.AddWithValue("@startDate", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(startDate));
        sqlCommand.Parameters.AddWithValue("@endDate", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(endDate));

        using (SqlDataReader rd = sqlCommand.ExecuteReader())
        {
          while (rd.Read())
          {
            AccomodationRoomRequestReport accomodationRoomRequestReport = new AccomodationRoomRequestReport();
            accomodationRoomRequestReport.CCRequestReferenceNo = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CCRequestReferenceNo"]);
            accomodationRoomRequestReport.EventName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_EventName"]);
            accomodationRoomRequestReport.RoomName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RoomName"]);
            accomodationRoomRequestReport.StartDate = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_StartDate"]);
            accomodationRoomRequestReport.EndDate = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_EndDate"]);
            accomodationRoomRequestReport.NumberOfNights = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_NumberOfNights"]);
            accomodationRoomRequestReport.StatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StatusName"]);
            accomodationRoomRequestReport.CostCenterName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CostCenterName"]);
            accomodationRoomRequestReport.DateCreated = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateCreated"]);
            accomodationRoomRequestReport.DateCancelled = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateCancelled"]);
            accomodationRoomRequestReport.ChargedCompanyCostCenter.CostCenterName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_ChargedCompanyCostCenterName"]);
            accomodationRoomRequestReportList.Add(accomodationRoomRequestReport);
          }
        }
      }
    }

    return accomodationRoomRequestReportList;
  }

  #endregion
}