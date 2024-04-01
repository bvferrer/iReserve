using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for AccomodationRoomScheduleMapping
/// </summary>
public class AccomodationRoomScheduleMapping
{
    public AccomodationRoomScheduleMapping()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Fields/Properties

    private int _mappingID;

    public int MappingID
    {
        get { return _mappingID; }
        set { _mappingID = value; }
    }

    private int _accRoomID;

    public int AccRoomID
    {
        get { return _accRoomID; }
        set { _accRoomID = value; }
    }

    private DateTime _date;

    public DateTime Date
    {
        get { return _date; }
        set { _date = value; }
    }

    private string _referenceNumber;

    public string ReferenceNumber
    {
        get { return _referenceNumber; }
        set { _referenceNumber = value; }
    }

    private string _isCancelled;

    public string IsCancelled
    {
        get { return _isCancelled; }
        set { _isCancelled = value; }
    }

    #endregion

    #region Methods

    public void InsertAccomodationRoomScheduleMapping(SqlConnection sqlConnection)
    {
        using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.InsertAccomodationRoomScheduleMapping, sqlConnection))
        {
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@accRoomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.AccRoomID));
            sqlCommand.Parameters.AddWithValue("@date", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(this.Date));
            sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.ReferenceNumber));
            sqlCommand.ExecuteNonQuery();
        }
    }

    public bool ValidateAccomodationRoomScheduleAvailability(int roomID, DateTime startDate, DateTime endDate)
    {
        bool validationStatus = false;

        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.ValidateAccomodationRoomScheduleAvailability, sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@roomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(roomID));
                sqlCommand.Parameters.AddWithValue("@startDate", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(startDate));
                sqlCommand.Parameters.AddWithValue("@endDate", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(endDate));

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

    public void CancelAccomodationRoomScheduleMapping(SqlConnection sqlConnection, string ccRequestReferenceNo)
    {
        using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.CancelAccomodationRoomScheduleMapping, sqlConnection))
        {
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(ccRequestReferenceNo));
            sqlCommand.ExecuteNonQuery();
        }
    }

    public bool ValidateSummaryAccomodationRoomScheduleAvailability(List<AccomodationRoomRequest> accomodationRoomRequestList)
    {
        bool validationStatus = true;

        foreach (AccomodationRoomRequest accomodationRoomRequest in accomodationRoomRequestList)
        {
            using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
            {
                using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.ValidateAccomodationRoomScheduleAvailability, sqlConnection))
                {
                    sqlConnection.Open();
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@roomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(accomodationRoomRequest.AccID));
                    sqlCommand.Parameters.AddWithValue("@startDate", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(accomodationRoomRequest.StartDate));
                    sqlCommand.Parameters.AddWithValue("@endDate", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(accomodationRoomRequest.EndDate));

                    using (SqlDataReader rd = sqlCommand.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            validationStatus = RDFramework.Utility.Conversion.SafeReadDatabaseValue<bool>(rd["ValidationStatus"]);
                        }
                    }
                }
            }

            if (validationStatus == false)
            {
                return false;
            }
        }

        return validationStatus;
    }

    public void TranAccomodationRoomScheduleMapping(int type, SqlConnection sqlConnection)
    {
        using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.TranAccomodationRoomScheduleMapping, sqlConnection))
        {
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@type", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(type));
            sqlCommand.Parameters.AddWithValue("@accRoomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.AccRoomID));
            sqlCommand.Parameters.AddWithValue("@date", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(this.Date));
            sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.ReferenceNumber));
            sqlCommand.ExecuteNonQuery();
        }
    }

    #endregion
}