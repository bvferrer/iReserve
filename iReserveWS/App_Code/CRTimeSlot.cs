using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for ConferenceRoomTimeSlot
/// </summary>
public class CRTimeSlot
{
    public CRTimeSlot()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    #region Fields/Properties

    private int _timeSlotID;

    public int TimeSlotID
    {
        get { return _timeSlotID; }
        set { _timeSlotID = value; }
    }

    private string _startTime;

    public string StartTime
    {
        get { return _startTime; }
        set { _startTime = value; }
    }

    private string _endTime;

    public string EndTime
    {
        get { return _endTime; }
        set { _endTime = value; }
    }

    private string _startTime12;

    public string StartTime12
    {
        get { return _startTime12; }
        set { _startTime12 = value; }
    }

    private string _endTime12;

    public string EndTime12
    {
        get { return _endTime12; }
        set { _endTime12 = value; }
    }

    private bool _isEnabled;

    public bool IsEnabled
    {
        get { return _isEnabled; }
        set { _isEnabled = value; }
    }

    #endregion

    #region Methods

    public void RetrieveCRTimeSlotDetails(CRTimeSlot timeSlot)
    {
        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            using (SqlCommand sqlCommand = new SqlCommand(Common.usp_RetrieveCRTimeSlotByID, sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@timeSlotID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(timeSlot.TimeSlotID));

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        this.TimeSlotID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_TimeSlotID"]);
                        this.StartTime = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StartTime"]);
                        this.EndTime = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_EndTime"]);
                        this.StartTime12 = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StartTime12"]);
                        this.EndTime12 = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_EndTime12"]);
                        this.IsEnabled = RDFramework.Utility.Conversion.SafeReadDatabaseValue<bool>(rd["fld_IsEnabled"]);
                    }
                }
            }
        }
    }

    #endregion
}