using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.Serialization;

/// <summary>
/// Summary description for AccomodationRoom
/// </summary>
public class AccomodationRoom
{
    #region Constructor
    public AccomodationRoom()
	{

    }
    #endregion

    #region Fields/Properties

    private int _accRoomID;

    public int AccRoomID
    {
        get { return _accRoomID; }
        set { _accRoomID = value; }
    }

    private string _roomCode;

    public string RoomCode
    {
        get { return _roomCode; }
        set { _roomCode = value; }
    }

    private string _roomName;

    public string RoomName
    {
        get { return _roomName; }
        set { _roomName = value; }
    }

    private string _roomDesc;

    public string RoomDesc
    {
        get { return _roomDesc; }
        set { _roomDesc = value; }
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

    private int _maxPerson;

    public int MaxPerson
    {
        get { return _maxPerson; }
        set { _maxPerson = value; }
    }

    private string _ratePerNight;

    public string RatePerNight
    {
        get { return _ratePerNight; }
        set { _ratePerNight = value; }
    }

    private bool _isDeleted;

    public bool IsDeleted
    {
        get { return _isDeleted; }
        set { _isDeleted = value; }
    }

    #endregion

    #region Methods

    public List<AccomodationRoom> RetrieveAccomodationRoomRecords(string roomCode, string roomName)
    {
        List<AccomodationRoom> accomodationRoomList = new List<AccomodationRoom>();

        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            sqlConnection.Open();
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveAccomodationRoomRecords, sqlConnection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@RoomCode", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(roomCode));
                sqlCommand.Parameters.AddWithValue("@RoomName", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(roomName));

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        AccomodationRoom accomodationRoom = new AccomodationRoom();
                        accomodationRoom.AccRoomID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_AccRoomID"]);
                        accomodationRoom.RoomCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RoomCode"]);
                        accomodationRoom.RoomName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RoomName"]);
                        accomodationRoom.RoomDesc = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RoomDesc"]);
                        accomodationRoom.LocationID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_LocationID"]);
                        accomodationRoom.LocationName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_LocationName"]);
                        accomodationRoomList.Add(accomodationRoom);
                    }
                }
            }
        }

        return accomodationRoomList;
    }

    public void RetrieveAccomodationRoomRecordDetails(int accRoomID)
    {
        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            sqlConnection.Open();
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveAccomodationRoomRecordDetails, sqlConnection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@RoomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(accRoomID));

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        this.AccRoomID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_AccRoomID"]);
                        this.RoomCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RoomCode"]);
                        this.RoomName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RoomName"]);
                        this.RoomDesc = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RoomDesc"]);
                        this.LocationID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_LocationID"]);
                        this.LocationName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_LocationName"]);
                        this.MaxPerson = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_MaxPerson"]);
                        this.RatePerNight = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RatePerNight"]);
                    }
                }
            }
        }
    }

    public int ValidateAccomodationRoomRecord(int type, int accRoomID, string roomCode, string roomName)
    {
        int validationStatus = 0;

        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            sqlConnection.Open();
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.ValidateAccomodationRoomRecord, sqlConnection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Type", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(type));
                sqlCommand.Parameters.AddWithValue("@AccRoomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(accRoomID));
                sqlCommand.Parameters.AddWithValue("@RoomCode", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(roomCode));
                sqlCommand.Parameters.AddWithValue("@RoomName", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(roomName));

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

    public void AccomodationRoomTransaction(int type, AccomodationRoom accomodationRoom, AuditTrail auditTrailDetails)
    {
        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringWriter))
        {
            sqlConnection.Open();

            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.TranAccomodationRoom, sqlConnection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Type", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(type));
                sqlCommand.Parameters.AddWithValue("@UserID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(auditTrailDetails.UserID));
                sqlCommand.Parameters.AddWithValue("@AccRoomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(accomodationRoom.AccRoomID));
                sqlCommand.Parameters.AddWithValue("@RoomCode", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(accomodationRoom.RoomCode));
                sqlCommand.Parameters.AddWithValue("@RoomName", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(accomodationRoom.RoomName));
                sqlCommand.Parameters.AddWithValue("@RoomDesc", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(accomodationRoom.RoomDesc));
                sqlCommand.Parameters.AddWithValue("@LocationID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(accomodationRoom.LocationID));
                sqlCommand.Parameters.AddWithValue("@MaxPerson", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(accomodationRoom.MaxPerson));
                sqlCommand.Parameters.AddWithValue("@RatePerNight", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(accomodationRoom.RatePerNight));
                sqlCommand.Parameters.AddWithValue("@IsDeleted", accomodationRoom.IsDeleted);
                sqlCommand.Parameters.AddWithValue("@MACAddress", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(auditTrailDetails.MacAdress));
                sqlCommand.Parameters.AddWithValue("@Browser", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(auditTrailDetails.Browser));
                sqlCommand.Parameters.AddWithValue("@BrowserVersion", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(auditTrailDetails.BrowserVersion));
                sqlCommand.ExecuteNonQuery();
            }
        }
    }

    public DataTable RetrieveAccRoomCalendarSchedule(string dateFrom)
    {
        DataTable accRoomScheduleDataTable = new DataTable("AccRoomScheduleDataTable");

        using (SqlConnection connection = new SqlConnection(Settings.iReserveConnectionStringWriter))
        {
            using (SqlCommand cmd = new SqlCommand(StoredProcedures.RetrieveAccRoomCalendarSchedule, connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                connection.Open();
                cmd.Parameters.AddWithValue("@dateFrom", dateFrom);
                cmd.ExecuteNonQuery();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(accRoomScheduleDataTable);
            }
        }

        return accRoomScheduleDataTable;
    }

    #endregion
}