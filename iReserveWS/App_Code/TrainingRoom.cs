using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.Serialization;

/// <summary>
/// Summary description for TrainingRoom
/// </summary>
public class TrainingRoom
{
    #region Constructor
    public TrainingRoom()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region Fields/Properties

    private int _tRoomID;

    public int TRoomID
    {
        get { return _tRoomID; }
        set { _tRoomID = value; }
    }

    private string _tRoomCode;

    public string TRoomCode
    {
        get { return _tRoomCode; }
        set { _tRoomCode = value; }
    }

    private string _tRoomName;

    public string TRoomName
    {
        get { return _tRoomName; }
        set { _tRoomName = value; }
    }

    private string _tRoomDesc;

    public string TRoomDesc
    {
        get { return _tRoomDesc; }
        set { _tRoomDesc = value; }
    }

    private int _numberOfPartition;

    public int NumberOfPartition
    {
        get { return _numberOfPartition; }
        set { _numberOfPartition = value; }
    }

    private int _locationID;

    public int LocationID
    {
        get { return _locationID; }
        set { _locationID = value; }
    }

    private string _locationName;

    public string LocationName
    {
        get { return _locationName; }
        set { _locationName = value; }
    }

    private bool _isDeleted;

    public bool IsDeleted
    {
        get { return _isDeleted; }
        set { _isDeleted = value; }
    }

    #endregion

    #region Methods

    public List<TrainingRoom> RetrieveTrainingRoomRecords(string roomCode, string roomName)
    {
        List<TrainingRoom> trainingRoomList = new List<TrainingRoom>();

        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            sqlConnection.Open();
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveTrainingRoomRecords, sqlConnection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@TRoomCode", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(roomCode));
                sqlCommand.Parameters.AddWithValue("@TRoomName", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(roomName));

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        TrainingRoom trainingRoom = new TrainingRoom();
                        trainingRoom.TRoomID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_TRoomID"]);
                        trainingRoom.TRoomCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_TRoomCode"]);
                        trainingRoom.TRoomName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_TRoomName"]);
                        trainingRoom.TRoomDesc = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_TRoomDesc"]);
                        trainingRoom.LocationID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_LocationID"]);
                        trainingRoom.LocationName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_LocationName"]);
                        trainingRoomList.Add(trainingRoom);
                    }
                }
            }
        }

        return trainingRoomList;
    }

    public void RetrieveTrainingRoomRecordDetails(int trainingRoomID)
    {
        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            sqlConnection.Open();
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveTrainingRoomRecordDetails, sqlConnection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@TRoomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(trainingRoomID));

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        this.TRoomID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_TRoomID"]);
                        this.TRoomCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_TRoomCode"]);
                        this.TRoomName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_TRoomName"]);
                        this.TRoomDesc = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_TRoomDesc"]);
                        this.LocationID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_LocationID"]);
                        this.LocationName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_LocationName"]);
                        this.NumberOfPartition = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_NumberOfPartition"]);
                    }
                }
            }
        }
    }

    public int ValidateTrainingRoomRecord(int type, int tRoomID, string tRoomCode, string tRoomName)
    {
        int validationStatus = 0;

        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            sqlConnection.Open();
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.ValidateTrainingRoomRecord, sqlConnection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Type", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(type));
                sqlCommand.Parameters.AddWithValue("@TRoomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(tRoomID));
                sqlCommand.Parameters.AddWithValue("@TRoomCode", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(tRoomCode));
                sqlCommand.Parameters.AddWithValue("@TRoomName", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(tRoomName));

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        validationStatus = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["ValidationStatus"]);
                    }
                }
            }
        }

        return validationStatus;
    }

    public void TrainingRoomTransaction(int type, TrainingRoom trainingRoom, AuditTrail auditTrailDetails, SqlConnection sqlConnection)
    {
        int tRoomID = 0;

        using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.TranTrainingRoom, sqlConnection))
        {
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Type", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(type));
            sqlCommand.Parameters.AddWithValue("@UserID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(auditTrailDetails.UserID));
            sqlCommand.Parameters.AddWithValue("@TRoomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(trainingRoom.TRoomID));
            sqlCommand.Parameters.AddWithValue("@TRoomCode", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(trainingRoom.TRoomCode));
            sqlCommand.Parameters.AddWithValue("@TRoomName", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(trainingRoom.TRoomName));
            sqlCommand.Parameters.AddWithValue("@TRoomDesc", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(trainingRoom.TRoomDesc));
            sqlCommand.Parameters.AddWithValue("@LocationID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(trainingRoom.LocationID));
            sqlCommand.Parameters.AddWithValue("@NumberOfPart", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(trainingRoom.NumberOfPartition));
            sqlCommand.Parameters.AddWithValue("@IsDeleted", trainingRoom.IsDeleted);
            sqlCommand.Parameters.AddWithValue("@MACAddress", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(auditTrailDetails.MacAdress));
            sqlCommand.Parameters.AddWithValue("@Browser", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(auditTrailDetails.Browser));
            sqlCommand.Parameters.AddWithValue("@BrowserVersion", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(auditTrailDetails.BrowserVersion));

            using (SqlDataReader rd = sqlCommand.ExecuteReader())
            {
                if (rd.Read())
                {
                    tRoomID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["TRoomID"]);
                }
            }
        }

        this.TRoomID = tRoomID;
    }

    public DataTable RetrieveTRCalendarSchedule(string dateFrom)
    {
        DataTable trScheduleDataTable = new DataTable("TRScheduleDataTable");

        using (SqlConnection connection = new SqlConnection(Settings.iReserveConnectionStringWriter))
        {
            using (SqlCommand cmd = new SqlCommand(StoredProcedures.RetrieveTRCalendarSchedule, connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                connection.Open();
                cmd.Parameters.AddWithValue("@dateFrom", dateFrom);
                cmd.ExecuteNonQuery();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(trScheduleDataTable);
            }
        }

        return trScheduleDataTable;
    }

    #endregion
}