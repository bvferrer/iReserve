using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.Serialization;

/// <summary>
/// Summary description for TRRate
/// </summary>
public class TRRate
{
    #region Constructor
    public TRRate()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    #endregion

    #region Fields/Properties

    private int _rateID;

    public int RateID
    {
        get { return _rateID; }
        set { _rateID = value; }
    }

    private int _tRoomID;

    public int TRoomID
    {
        get { return _tRoomID; }
        set { _tRoomID = value; }
    }

    private string _tRoomName;

    public string TRoomName
    {
        get { return _tRoomName; }
        set { _tRoomName = value; }
    }

    private int _numberOfPartition;

    public int NumberOfPartition
    {
        get { return _numberOfPartition; }
        set { _numberOfPartition = value; }
    }

    private string _ratePerDay;

    public string RatePerDay
    {
        get { return _ratePerDay; }
        set { _ratePerDay = value; }
    }

    private string _ratePerHour;

    public string RatePerHour
    {
        get { return _ratePerHour; }
        set { _ratePerHour = value; }
    }

    private bool _isDeleted;

    public bool IsDeleted
    {
        get { return _isDeleted; }
        set { _isDeleted = value; }
    }

    #endregion

    #region Methods

    public List<TRRate> RetrieveTRRateRecords(int tRoomID)
    {
        List<TRRate> trRateList = new List<TRRate>();

        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            sqlConnection.Open();
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveTRRateRecords, sqlConnection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@TRoomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(tRoomID));

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        TRRate trRate = new TRRate();
                        trRate.RateID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_RateID"]);
                        trRate.NumberOfPartition = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_NumberOfPartition"]);
                        trRate.RatePerDay = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RatePerDay"]);
                        trRate.RatePerHour = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RatePerHour"]);
                        trRate.TRoomID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_TRoomID"]);
                        trRate.TRoomName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_TRoomName"]);
                        trRateList.Add(trRate);
                    }
                }
            }
        }

        return trRateList;
    }

    public void TRRateTransaction(int type, int tRoomID, List<TRRate> trRateList, AuditTrail auditTrailDetails, SqlConnection sqlConnection)
    {
        if (type == 1)
        {
            foreach (TRRate trRate in trRateList)
            {
                using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.TranTRRate, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Type", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(type));
                    sqlCommand.Parameters.AddWithValue("@UserID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(auditTrailDetails.UserID));
                    sqlCommand.Parameters.AddWithValue("@RateID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(trRate.RateID));
                    sqlCommand.Parameters.AddWithValue("@TRoomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(tRoomID));
                    sqlCommand.Parameters.AddWithValue("@NumberOfPartition", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(trRate.NumberOfPartition));
                    sqlCommand.Parameters.AddWithValue("@RatePerDay", RDFramework.Utility.Conversion.SafeSetDatabaseValue<decimal>(Convert.ToDecimal(trRate.RatePerDay)));
                    sqlCommand.Parameters.AddWithValue("@RatePerHour", RDFramework.Utility.Conversion.SafeSetDatabaseValue<decimal>(Convert.ToDecimal(trRate.RatePerHour)));
                    sqlCommand.Parameters.AddWithValue("@IsDeleted", trRate.IsDeleted);
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
        else if (type == 2)
        {
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.TranTRRate, sqlConnection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Type", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(3));
                sqlCommand.Parameters.AddWithValue("@UserID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(auditTrailDetails.UserID));
                sqlCommand.Parameters.AddWithValue("@TRoomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(tRoomID)); ;
                sqlCommand.ExecuteNonQuery();
            }

            foreach (TRRate trRate in trRateList)
            {
                using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.TranTRRate, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Type", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(type));
                    sqlCommand.Parameters.AddWithValue("@UserID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(auditTrailDetails.UserID));
                    sqlCommand.Parameters.AddWithValue("@RateID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(trRate.RateID));
                    sqlCommand.Parameters.AddWithValue("@TRoomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(tRoomID));
                    sqlCommand.Parameters.AddWithValue("@NumberOfPartition", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(trRate.NumberOfPartition));
                    sqlCommand.Parameters.AddWithValue("@RatePerDay", RDFramework.Utility.Conversion.SafeSetDatabaseValue<decimal>(Convert.ToDecimal(trRate.RatePerDay)));
                    sqlCommand.Parameters.AddWithValue("@RatePerHour", RDFramework.Utility.Conversion.SafeSetDatabaseValue<decimal>(Convert.ToDecimal(trRate.RatePerHour)));
                    sqlCommand.Parameters.AddWithValue("@IsDeleted", trRate.IsDeleted);
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
        else
        {
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.TranTRRate, sqlConnection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Type", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(type));
                sqlCommand.Parameters.AddWithValue("@UserID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(auditTrailDetails.UserID));
                sqlCommand.Parameters.AddWithValue("@TRoomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(tRoomID)); ;
                sqlCommand.ExecuteNonQuery();
            }
        }
    }

    #endregion
}