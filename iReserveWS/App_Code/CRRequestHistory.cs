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
/// Summary description for CRRequestHistory
/// </summary>
public class CRRequestHistory
{
    public CRRequestHistory()
    {
        this.Status = new Status();
    }

    #region Properties

    private int _historyID;

    public int HistoryID
    {
        get { return _historyID; }
        set { _historyID = value; }
    }

    private string _requestReferenceNumber;

    public string RequestReferenceNumber
    {
        get { return _requestReferenceNumber; }
        set { _requestReferenceNumber = value; }
    }

    private Status _status;

    public Status Status
    {
        get { return _status; }
        set { _status = value; }
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

    public void InsertCRRequestHistory()
    {
        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringWriter))
        {
            sqlConnection.Open();

            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(Common.usp_InsertCRRequestHistory, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.RequestReferenceNumber));
                    sqlCommand.Parameters.AddWithValue("@statusCode", this.Status.StatusCode);
                    sqlCommand.Parameters.AddWithValue("@processedByID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.ProcessedByID));
                    sqlCommand.Parameters.AddWithValue("@processedBy", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.ProcessedBy));
                    sqlCommand.Parameters.AddWithValue("@remarks", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.Remarks));
                    sqlCommand.ExecuteNonQuery();
                }
            }

            catch
            {
                throw;
            }
        }
    }

    public void RetrieveCRRequestHistoryDetailsByStatus(CRRequest request)
    {
        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            using (SqlCommand sqlCommand = new SqlCommand(Common.usp_RetrieveCRRequestHistoryByStatus, sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(request.RequestReferenceNo));
                sqlCommand.Parameters.AddWithValue("@statusCode", request.Status.StatusCode);

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        this.HistoryID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_HistoryID"]);
                        this.RequestReferenceNumber = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RequestReferenceNo"]);
                        this.Status.StatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StatusCode"]);
                        this.DateProcessed = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateProcessed"]);
                        this.ProcessedByID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_ProcessedByID"]);
                        this.ProcessedBy = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_ProcessedBy"]);
                        this.Remarks = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_Remarks"]);
                    }
                }
            }
        }
    }

    public List<CRRequestHistory> RetrieveCRRequestHistoryRecords()
    {
        List<CRRequestHistory> historyList = new List<CRRequestHistory>();

        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            using (SqlCommand sqlCommand = new SqlCommand(Common.usp_RetrieveCRRequestHistory, sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.RequestReferenceNumber));

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        CRRequestHistory history = new CRRequestHistory();
                        history.HistoryID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_HistoryID"]);
                        history.RequestReferenceNumber = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RequestReferenceNo"]);
                        history.Status.StatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StatusCode"]);
                        history.Status.StatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StatusName"]);
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