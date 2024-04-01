using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for CRScheduleMapping
/// </summary>
public class CRScheduleMapping
{
	public CRScheduleMapping()
	{
        this.ConferenceRoom = new ConferenceRoom();
        this.TimeSlot = new CRTimeSlot();
    }

    #region Fields/Properties

    private int _mappingID;

    public int MappingID
    {
        get { return _mappingID; }
        set { _mappingID = value; }
    }

    private ConferenceRoom _conferenceRoom;

    public ConferenceRoom ConferenceRoom
    {
        get { return _conferenceRoom; }
        set { _conferenceRoom = value; }
    }

    private CRTimeSlot _timeSlot;

    public CRTimeSlot TimeSlot
    {
        get { return _timeSlot; }
        set { _timeSlot = value; }
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

    public void InsertCRScheduleMapping()
    {
        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringWriter))
        {
            sqlConnection.Open();

            using (SqlCommand sqlCommand = new SqlCommand(Common.usp_InsertCRScheduleMapping, sqlConnection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@roomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.ConferenceRoom.RoomID));
                sqlCommand.Parameters.AddWithValue("@timeSlotID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.TimeSlot.TimeSlotID));
                sqlCommand.Parameters.AddWithValue("@date", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(this.Date));
                sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.ReferenceNumber));
                sqlCommand.ExecuteNonQuery();
            }
        }
    }

    public void UpdateCRScheduleMappingStatus()
    {
        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringWriter))
        {
            sqlConnection.Open();

            using (SqlCommand sqlCommand = new SqlCommand(Common.usp_UpdateCRScheduleMappingStatus, sqlConnection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.ReferenceNumber));
                sqlCommand.ExecuteNonQuery();
            }
        }
    }

    #endregion
}