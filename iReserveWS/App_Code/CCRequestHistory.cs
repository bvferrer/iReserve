using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for CCRequestHistory
/// </summary>
public class CCRequestHistory
{
    public CCRequestHistory()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Properties

    private int _ccHistoryID;

    public int CCHistoryID
    {
        get { return _ccHistoryID; }
        set { _ccHistoryID = value; }
    }

    private string _ccRequestReferenceNumber;

    public string CCRequestReferenceNumber
    {
        get { return _ccRequestReferenceNumber; }
        set { _ccRequestReferenceNumber = value; }
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

    private DateTime _dateProcessed;

    public DateTime DateProcessed
    {
        get { return _dateProcessed; }
        set { _dateProcessed = value; }
    }

    private string _processedByID;

    public string ProcessedByID
    {
        get { return _processedByID; }
        set { _processedByID = value; }
    }

    private string _processedBy;

    public string ProcessedBy
    {
        get { return _processedBy; }
        set { _processedBy = value; }
    }

    private string _remarks;

    public string Remarks
    {
        get { return _remarks; }
        set { _remarks = value; }
    }

    #endregion

    #region Methods

    public void InsertCCRequestHistory(SqlConnection sqlConnection)
    {
        using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.InsertCCRequestHistory, sqlConnection))
        {
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.CCRequestReferenceNumber));
            sqlCommand.Parameters.AddWithValue("@statusCode", this.StatusCode);
            sqlCommand.Parameters.AddWithValue("@processedByID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.ProcessedByID));
            sqlCommand.Parameters.AddWithValue("@processedBy", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.ProcessedBy));
            sqlCommand.Parameters.AddWithValue("@remarks", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.Remarks));
            sqlCommand.ExecuteNonQuery();
        }
    }

    public void RetrieveCCRequestHistorybyStatus(string ccRequestReferenceNo, int statusCode)
    {
        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveCCRequestHistoryByStatus, sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(ccRequestReferenceNo));
                sqlCommand.Parameters.AddWithValue("@statusCode", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(statusCode));

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        this.CCHistoryID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_CCHistoryID"]);
                        this.CCRequestReferenceNumber = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CCRequestReferenceNo"]);
                        this.StatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StatusCode"]);
                        this.DateProcessed = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateProcessed"]);
                        this.ProcessedByID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_ProcessedByID"]);
                        this.ProcessedBy = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_ProcessedBy"]);
                        this.Remarks = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_Remarks"]);
                    }
                }
            }
        }
    }

    public List<CCRequestHistory> RetrieveCCRequestHistory(string ccRequestReferenceNumber)
    {
        List<CCRequestHistory> historyList = new List<CCRequestHistory>();

        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveCCRequestHistory, sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(ccRequestReferenceNumber));

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        CCRequestHistory history = new CCRequestHistory();
                        history.CCHistoryID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_CCHistoryID"]);
                        history.CCRequestReferenceNumber = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CCRequestReferenceNo"]);
                        history.StatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StatusCode"]);
                        history.StatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StatusName"]);
                        history.DateProcessed = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateProcessed"]);
                        history.ProcessedByID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_ProcessedByID"]);
                        history.ProcessedBy = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_ProcessedBy"]);
                        history.Remarks = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_Remarks"]);
                        historyList.Add(history);
                    }
                }
            }
        }

        return historyList;
    }

    #endregion
}