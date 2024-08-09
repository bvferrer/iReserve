using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for CCRequest
/// </summary>
public class CCRequest
{
  public CCRequest()
  {
    //
    // TODO: Add constructor logic here
    //
    _chargedCompanyCostCenter = new ChargedCompanyCostCenter();
  }

  #region Properties

  private int _ccRequestID;

  public int CCRequestID
  {
    get { return _ccRequestID; }
    set { _ccRequestID = value; }
  }

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

  private int _costCenterID;

  public int CostCenterID
  {
    get { return _costCenterID; }
    set { _costCenterID = value; }
  }

  private string _costCenterName;

  public string CostCenterName
  {
    get { return _costCenterName; }
    set { _costCenterName = value; }
  }

  private int _statusCode;

  public int StatusCode
  {
    get { return _statusCode; }
    set { _statusCode = value; }
  }

  private string _statusName;

  public string StatusName
  {
    get { return _statusName; }
    set { _statusName = value; }
  }

  private int _soaStatusCode;

  public int SOAStatusCode
  {
    get { return _soaStatusCode; }
    set { _soaStatusCode = value; }
  }

  private string _soaStatusName;

  public string SOAStatusName
  {
    get { return _soaStatusName; }
    set { _soaStatusName = value; }
  }

  private string _createdByID;

  public string CreatedByID
  {
    get { return _createdByID; }
    set { _createdByID = value; }
  }

  private string _createdBy;

  public string CreatedBy
  {
    get { return _createdBy; }
    set { _createdBy = value; }
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
    get { return _chargedCompanyCostCenter; }
    set { _chargedCompanyCostCenter = value; }
  }

  private float _percentDiscount;
  public float PercentDiscount
  {
    get { return _percentDiscount; }
    set { _percentDiscount = value; }
  }

  #endregion

  #region Methods

  public void InsertCCRequest(SqlConnection sqlConnection)
  {
    using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.InsertCCRequest, sqlConnection))
    {
      sqlCommand.CommandType = CommandType.StoredProcedure;
      sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.CCRequestReferenceNo));
      sqlCommand.Parameters.AddWithValue("@eventName", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.EventName));
      sqlCommand.Parameters.AddWithValue("@startDate", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(this.StartDate));
      sqlCommand.Parameters.AddWithValue("@endDate", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(this.EndDate));
      sqlCommand.Parameters.AddWithValue("@costCenterID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.CostCenterID));
      sqlCommand.Parameters.AddWithValue("@statusCode", this.StatusCode);
      sqlCommand.Parameters.AddWithValue("@soaStatusCode", this.SOAStatusCode);
      sqlCommand.Parameters.AddWithValue("@createdByID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.CreatedByID));
      sqlCommand.Parameters.AddWithValue("@createdBy", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.CreatedBy));
      sqlCommand.Parameters.AddWithValue("@chargedCompanyCostCenterID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.ChargedCompanyCostCenterID));
      sqlCommand.Parameters.AddWithValue("@chargedCompanyID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.ChargedCompanyID));
      sqlCommand.ExecuteNonQuery();
    }
  }

  public void UpdateCCRequestStatus(SqlConnection sqlConnection, string ccRequestReferenceNo, int statusCode)
  {
    using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.UpdateCCRequestStatus, sqlConnection))
    {
      sqlCommand.CommandType = CommandType.StoredProcedure;
      sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(ccRequestReferenceNo));
      sqlCommand.Parameters.AddWithValue("@statusCode", statusCode);
      sqlCommand.ExecuteNonQuery();
    }
  }

  public void RetrieveCCRequestDetails(string ccRequestReferenceNo)
  {
    using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
    {
      using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveCCRequestDetails, sqlConnection))
      {
        sqlConnection.Open();
        sqlCommand.CommandType = CommandType.StoredProcedure;
        sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(ccRequestReferenceNo));

        using (SqlDataReader rd = sqlCommand.ExecuteReader())
        {
          if (rd.Read())
          {
            this.CCRequestReferenceNo = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CCRequestReferenceNo"]);
            this.EventName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_EventName"]);
            this.StartDate = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_StartDate"]);
            this.EndDate = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_EndDate"]);
            this.CostCenterID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_CostCenterID"]);
            this.CostCenterName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CostCenterName"]);
            this.StatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StatusCode"]);
            this.StatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StatusName"]);
            this.SOAStatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_SOAStatusCode"]);
            this.SOAStatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_SOAStatusName"]);
            this.CreatedByID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CreatedByID"]);
            this.CreatedBy = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CreatedBy"]);
            this.DateCreated = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateCreated"]);
            this.ChargedCompanyCostCenter.CostCenterName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_ChargedCompanyCostCenterName"]);
            this.PercentDiscount = RDFramework.Utility.Conversion.SafeReadDatabaseValue<float>(rd["fld_PercentDiscount"]);
          }
        }
      }
    }
  }

  public List<CCRequest> RetrieveCCRequestRecordsByStatus(int statusCode)
  {
    List<CCRequest> requestList = new List<CCRequest>();

    using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
    {
      using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveCCRequestRecordsByStatus, sqlConnection))
      {
        sqlConnection.Open();
        sqlCommand.CommandType = CommandType.StoredProcedure;
        sqlCommand.Parameters.AddWithValue("@statusCode", statusCode);

        using (SqlDataReader rd = sqlCommand.ExecuteReader())
        {
          while (rd.Read())
          {
            CCRequest request = new CCRequest();
            request.CCRequestReferenceNo = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CCRequestReferenceNo"]);
            request.EventName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_EventName"]);
            request.StartDate = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_StartDate"]);
            request.EndDate = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_EndDate"]);
            request.CostCenterID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_CostCenterID"]);
            request.CostCenterName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CostCenterName"]);
            request.StatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StatusCode"]);
            request.StatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StatusName"]);
            request.SOAStatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_SOAStatusCode"]);
            request.SOAStatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_SOAStatusName"]);
            request.CreatedByID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CreatedByID"]);
            request.CreatedBy = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CreatedBy"]);
            request.DateCreated = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateCreated"]);
            request.ChargedCompanyCostCenter.CostCenterName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_ChargedCompanyCostCenterName"]);
            request.PercentDiscount = RDFramework.Utility.Conversion.SafeReadDatabaseValue<float>(rd["fld_PercentDiscount"]);
            requestList.Add(request);
          }
        }
      }
    }

    return requestList;
  }

  public List<CCRequest> RetrieveCCRequestRecordsByRequestor(string createdByID, int statusCode)
  {
    List<CCRequest> requestList = new List<CCRequest>();

    using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
    {
      using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveCCRequestRecordsByRequestor, sqlConnection))
      {
        sqlConnection.Open();
        sqlCommand.CommandType = CommandType.StoredProcedure;
        sqlCommand.Parameters.AddWithValue("@requestorID", createdByID);
        sqlCommand.Parameters.AddWithValue("@statusCode", statusCode);

        using (SqlDataReader rd = sqlCommand.ExecuteReader())
        {
          while (rd.Read())
          {
            CCRequest request = new CCRequest();
            request.CCRequestReferenceNo = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CCRequestReferenceNo"]);
            request.EventName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_EventName"]);
            request.StartDate = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_StartDate"]);
            request.EndDate = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_EndDate"]);
            request.CostCenterID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_CostCenterID"]);
            request.CostCenterName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CostCenterName"]);
            request.StatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StatusCode"]);
            request.StatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StatusName"]);
            request.SOAStatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_SOAStatusCode"]);
            request.SOAStatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_SOAStatusName"]);
            request.CreatedByID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CreatedByID"]);
            request.CreatedBy = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CreatedBy"]);
            request.DateCreated = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateCreated"]);
            requestList.Add(request);
          }
        }
      }
    }

    return requestList;
  }

  public void RetrieveCCRequestDetailsByRequestor(string ccRequestReferenceNo, string createdByID)
  {
    using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
    {
      using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveCCRequestDetailsByRequestor, sqlConnection))
      {
        sqlConnection.Open();
        sqlCommand.CommandType = CommandType.StoredProcedure;
        sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(ccRequestReferenceNo));
        sqlCommand.Parameters.AddWithValue("@requestorID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(createdByID));

        using (SqlDataReader rd = sqlCommand.ExecuteReader())
        {
          while (rd.Read())
          {
            this.CCRequestReferenceNo = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CCRequestReferenceNo"]);
            this.EventName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_EventName"]);
            this.StartDate = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_StartDate"]);
            this.EndDate = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_EndDate"]);
            this.CostCenterID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_CostCenterID"]);
            this.CostCenterName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CostCenterName"]);
            this.StatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StatusCode"]);
            this.StatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StatusName"]);
            this.SOAStatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_SOAStatusCode"]);
            this.SOAStatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_SOAStatusName"]);
            this.CreatedByID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CreatedByID"]);
            this.CreatedBy = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CreatedBy"]);
            this.DateCreated = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateCreated"]);
          }
        }
      }
    }
  }

  public List<CCRequest> RetrieveCCRequestRecordsBySOAStatus(int soaStatusCode)
  {
    List<CCRequest> requestList = new List<CCRequest>();

    using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
    {
      using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveCCRequestRecordsBySOAStatus, sqlConnection))
      {
        sqlConnection.Open();
        sqlCommand.CommandType = CommandType.StoredProcedure;
        sqlCommand.Parameters.AddWithValue("@soaStatusCode", soaStatusCode);

        using (SqlDataReader rd = sqlCommand.ExecuteReader())
        {
          while (rd.Read())
          {
            CCRequest request = new CCRequest();
            request.CCRequestReferenceNo = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CCRequestReferenceNo"]);
            request.EventName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_EventName"]);
            request.StartDate = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_StartDate"]);
            request.EndDate = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_EndDate"]);
            request.CostCenterID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_CostCenterID"]);
            request.CostCenterName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CostCenterName"]);
            request.StatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StatusCode"]);
            request.StatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StatusName"]);
            request.SOAStatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_SOAStatusCode"]);
            request.SOAStatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_SOAStatusName"]);
            request.CreatedByID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CreatedByID"]);
            request.CreatedBy = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CreatedBy"]);
            request.DateCreated = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateCreated"]);
            request.ChargedCompanyCostCenter.CostCenterName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_ChargedCompanyCostCenterName"]);
            requestList.Add(request);
          }
        }
      }
    }

    return requestList;
  }

  public List<CCRequest> RetrieveCCRequestRecordsApprovedSOA()
  {
    List<CCRequest> requestList = new List<CCRequest>();

    using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
    {
      using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveCCRequestRecordsApprovedSOA, sqlConnection))
      {
        sqlConnection.Open();
        sqlCommand.CommandType = CommandType.StoredProcedure;

        using (SqlDataReader rd = sqlCommand.ExecuteReader())
        {
          while (rd.Read())
          {
            CCRequest request = new CCRequest();
            request.CCRequestReferenceNo = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CCRequestReferenceNo"]);
            request.EventName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_EventName"]);
            request.StartDate = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_StartDate"]);
            request.EndDate = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_EndDate"]);
            request.CostCenterID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_CostCenterID"]);
            request.CostCenterName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CostCenterName"]);
            request.StatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StatusCode"]);
            request.StatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StatusName"]);
            request.SOAStatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_SOAStatusCode"]);
            request.SOAStatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_SOAStatusName"]);
            request.CreatedByID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CreatedByID"]);
            request.CreatedBy = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CreatedBy"]);
            request.DateCreated = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateCreated"]);
            request.ChargedCompanyCostCenter.CostCenterName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_ChargedCompanyCostCenterName"]);
            requestList.Add(request);
          }
        }
      }
    }

    return requestList;
  }

  public void UpdateCCRequestSOAStatus(SqlConnection sqlConnection, string ccRequestReferenceNo, int soaStatusCode)
  {
    using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.UpdateSOAStatus, sqlConnection))
    {
      sqlCommand.CommandType = CommandType.StoredProcedure;
      sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(ccRequestReferenceNo));
      sqlCommand.Parameters.AddWithValue("@soaStatusCode", soaStatusCode);
      sqlCommand.ExecuteNonQuery();
    }
  }

  public void UpdateCCRequestPercentDiscount(SqlConnection sqlConnection)
  {
    using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.UpdateCCRequestPercentDiscount, sqlConnection))
    {
      sqlCommand.CommandType = CommandType.StoredProcedure;
      sqlCommand.Parameters.AddWithValue("@referenceNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.CCRequestReferenceNo));
      sqlCommand.Parameters.AddWithValue("@percentDiscount", this.PercentDiscount);
      sqlCommand.ExecuteNonQuery();
    }
  }

  #endregion
}