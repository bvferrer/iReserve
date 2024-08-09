using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for SOAReport
/// </summary>
public class SOAReport
{
  public SOAReport()
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

  private string _createdBy;

  public string CreatedBy
  {
    get { return _createdBy; }
    set { _createdBy = value; }
  }

  private string _costCenterName;

  public string CostCenterName
  {
    get { return _costCenterName; }
    set { _costCenterName = value; }
  }

  private string _trainingRoom;

  public string TrainingRoom
  {
    get { return _trainingRoom; }
    set { _trainingRoom = value; }
  }

  private string _accomodationRoom;

  public string AccomodationRoom
  {
    get { return _accomodationRoom; }
    set { _accomodationRoom = value; }
  }

  private string _otherCharges;

  public string OtherCharges
  {
    get { return _otherCharges; }
    set { _otherCharges = value; }
  }

  private string _totalAmountCharge;

  public string TotalAmountCharge
  {
    get { return _totalAmountCharge; }
    set { _totalAmountCharge = value; }
  }

  private string _orDate;

  public string ORDate
  {
    get { return _orDate; }
    set { _orDate = value; }
  }

  private string _orNumber;

  public string ORNumber
  {
    get { return _orNumber; }
    set { _orNumber = value; }
  }

  private string orAmount;

  public string ORAmount
  {
    get { return orAmount; }
    set { orAmount = value; }
  }

  private string remarks;

  public string Remarks
  {
    get { return remarks; }
    set { remarks = value; }
  }

  private ChargedCompanyCostCenter _chargedCompanyCostCenter;
  public ChargedCompanyCostCenter ChargedCompanyCostCenter
  {
    get { return _chargedCompanyCostCenter; }
    set { _chargedCompanyCostCenter = value; }
  }

  private string _statusCode;
  public string StatusCode
  {
    get { return _statusCode; }
    set { _statusCode = value; }
  }

  private string _dateCancelled;
  public string DateCancelled
  {
    get { return _dateCancelled; }
    set { _dateCancelled = value; }
  }
  private string _soaStatusCode;
  public string SOAStatusCode
  {
    get { return _soaStatusCode; }
    set { _soaStatusCode = value; }
  }

  private string _cancellationFee;
  public string CancellationFee
  {
    get { return _cancellationFee; }
    set { _cancellationFee = value; }
  }

  private float _percentDiscount;
  public float PercentDiscount
  {
    get { return _percentDiscount; }
    set { _percentDiscount = value; }
  }

  #endregion

  #region Methods

  public List<SOAReport> RetrieveSOAReport(DateTime startDate, DateTime endDate)
  {
    List<SOAReport> soaReportList = new List<SOAReport>();

    using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
    {
      using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveSOAReport, sqlConnection))
      {
        sqlConnection.Open();
        sqlCommand.CommandType = CommandType.StoredProcedure;
        sqlCommand.Parameters.AddWithValue("@startDate", RDFramework.Utility.Conversion.SafeSetDatabaseValue(startDate));
        sqlCommand.Parameters.AddWithValue("@endDate", RDFramework.Utility.Conversion.SafeSetDatabaseValue(endDate));

        using (SqlDataReader rd = sqlCommand.ExecuteReader())
        {
          while (rd.Read())
          {
            SOAReport soaReport = new SOAReport();
            soaReport.CCRequestReferenceNo = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CCRequestReferenceNo"]);
            soaReport.EventName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_EventName"]);
            soaReport.StartDate = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_StartDate"]);
            soaReport.EndDate = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_EndDate"]);
            soaReport.CreatedBy = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CreatedBy"]);
            soaReport.CostCenterName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CostCenterName"]);
            soaReport.TrainingRoom = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_TrainingRoom"]);
            soaReport.AccomodationRoom = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_AccomodationRoom"]);
            soaReport.OtherCharges = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_OtherCharges"]);
            soaReport.TotalAmountCharge = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_TotalAmountCharge"]);
            soaReport.ChargedCompanyCostCenter.CostCenterName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_ChargedCompanyCostCenterName"]);
            soaReport.StatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StatusCode"]);
            soaReport.DateCancelled = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_DateCancelled"]);
            soaReport.SOAStatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_SOAStatusCode"]);
            soaReport.CancellationFee = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CancellationFee"]);
            soaReport.PercentDiscount = RDFramework.Utility.Conversion.SafeReadDatabaseValue<float>(rd["fld_PercentDiscount"]);
            soaReportList.Add(soaReport);
          }
        }
      }
    }

    return soaReportList;
  }

  #endregion
}