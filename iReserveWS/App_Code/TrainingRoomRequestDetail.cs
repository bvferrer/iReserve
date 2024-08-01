using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for TrainingRoomRequestDetail
/// </summary>
public class TrainingRoomRequestDetail
{
	public TrainingRoomRequestDetail()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    #region Properties

    private int _trRequestDetailID;

    public int TrRequestDetailID
    {
        get { return _trRequestDetailID; }
        set { _trRequestDetailID = value; }
    }

    private string _ccRequestReferenceNo;

    public string CCRequestReferenceNo
    {
        get { return _ccRequestReferenceNo; }
        set { _ccRequestReferenceNo = value; }
    }

    private int _partitionID;

    public int PartitionID
    {
        get { return _partitionID; }
        set { _partitionID = value; }
    }

    private string _partitionName;

    public string PartitionName
    {
        get { return _partitionName; }
        set { _partitionName = value; }
    }

    private DateTime _date;

    public DateTime Date
    {
        get { return _date; }
        set { _date = value; }
    }

    private DateTime _startDateTime;

    public DateTime StartDateTime
    {
        get { return _startDateTime; }
        set { _startDateTime = value; }
    }

    private DateTime _endDateTime;

    public DateTime EndDateTime
    {
        get { return _endDateTime; }
        set { _endDateTime = value; }
    }

    private int _numberOfHours;

    public int NumberOfHours
    {
        get { return _numberOfHours; }
        set { _numberOfHours = value; }
    }

    #endregion

    #region Methods

    public void InsertTrainingRoomRequestDetail(SqlConnection sqlConnection)
    {
        using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.InsertTrainingRoomRequestDetail, sqlConnection))
        {
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.CCRequestReferenceNo));
            sqlCommand.Parameters.AddWithValue("@partitionID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.PartitionID));
            sqlCommand.Parameters.AddWithValue("@date", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(this.Date));
            sqlCommand.Parameters.AddWithValue("@startDateTime", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(this.StartDateTime));
            sqlCommand.Parameters.AddWithValue("@endDateTime", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(this.EndDateTime));
            sqlCommand.ExecuteNonQuery();
        }
    }

    #endregion
}