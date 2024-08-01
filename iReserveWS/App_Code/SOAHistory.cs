using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for SOAHistory
/// </summary>
public class SOAHistory
{
	public SOAHistory()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    #region Properties

    private int _soaHistoryID;

    public int SOAHistoryID
    {
        get { return _soaHistoryID; }
        set { _soaHistoryID = value; }
    }

    private string _ccRequestReferenceNumber;

    public string CCRequestReferenceNumber
    {
        get { return _ccRequestReferenceNumber; }
        set { _ccRequestReferenceNumber = value; }
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

    public void InsertSOAHistory(SqlConnection sqlConnection)
    {
        using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.InsertSOAHistory, sqlConnection))
        {
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.CCRequestReferenceNumber));
            sqlCommand.Parameters.AddWithValue("@soaStatusCode", this.SOAStatusCode);
            sqlCommand.Parameters.AddWithValue("@processedByID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.ProcessedByID));
            sqlCommand.Parameters.AddWithValue("@processedBy", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.ProcessedBy));
            sqlCommand.Parameters.AddWithValue("@remarks", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.Remarks));
            sqlCommand.ExecuteNonQuery();
        }
    }

    public void RetrieveSOAHistorybyStatus(string ccRequestReferenceNo, int soaStatusCode)
    {
        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveSOAHistoryByStatus, sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(ccRequestReferenceNo));
                sqlCommand.Parameters.AddWithValue("@soaStatusCode", soaStatusCode);

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        this.SOAHistoryID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_SOAHistoryID"]);
                        this.CCRequestReferenceNumber = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CCRequestReferenceNo"]);
                        this.SOAStatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_SOAStatusCode"]);
                        this.DateProcessed = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateProcessed"]);
                        this.ProcessedByID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_ProcessedByID"]);
                        this.ProcessedBy = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_ProcessedBy"]);
                        this.Remarks = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_Remarks"]);
                    }
                }
            }
        }
    }

    public List<SOAHistory> RetrieveSOAHistory(string ccRequestReferenceNumber)
    {
        List<SOAHistory> soaHistoryList = new List<SOAHistory>();

        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveSOAHistory, sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(ccRequestReferenceNumber));

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        SOAHistory soaHistory = new SOAHistory();
                        soaHistory.SOAHistoryID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_SOAHistoryID"]);
                        soaHistory.CCRequestReferenceNumber = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CCRequestReferenceNo"]);
                        soaHistory.SOAStatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_SOAStatusCode"]);
                        soaHistory.SOAStatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_SOAStatusName"]);
                        soaHistory.DateProcessed = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateProcessed"]);
                        soaHistory.ProcessedByID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_ProcessedByID"]);
                        soaHistory.ProcessedBy = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_ProcessedBy"]);
                        soaHistory.Remarks = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_Remarks"]);
                        soaHistoryList.Add(soaHistory);
                    }
                }
            }
        }

        return soaHistoryList;
    }

    #endregion
}