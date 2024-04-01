using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// Summary description for TrainingRoomScheduleMappingTransactionRequest
/// </summary>
public class TrainingRoomScheduleMappingTransactionRequest
{
	public TrainingRoomScheduleMappingTransactionRequest()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    private int _type;

    public int Type
    {
        get { return _type; }
        set { _type = value; }
    }

    private int _partitionID;

    public int PartitionID
    {
        get { return _partitionID; }
        set { _partitionID = value; }
    }

    private DateTime _startDate;

    public DateTime StartDate
    {
        get { return _startDate; }
        set { _startDate = value; }
    }

    private DateTime _endDate;

    public DateTime EndDate
    {
        get { return _endDate; }
        set { _endDate = value; }
    }

    private string _remarks;

    public string Remarks
    {
        get { return _remarks; }
        set { _remarks = value; }
    }

    public TrainingRoomScheduleMappingTransactionResult Process()
    {
        TrainingRoomScheduleMappingTransactionResult returnValue = new TrainingRoomScheduleMappingTransactionResult();

        TrainingRoomScheduleMapping trainingRoomScheduleMapping = new TrainingRoomScheduleMapping();
        trainingRoomScheduleMapping.PartitionID = this.PartitionID;
        trainingRoomScheduleMapping.ReferenceNumber = this.Remarks;

        DateTime date = this.StartDate;
        DateTime endDate = this.EndDate;

        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringWriter))
        {
            sqlConnection.Open();

            while (date <= endDate)
            {
                trainingRoomScheduleMapping.Date = date;
                trainingRoomScheduleMapping.TranTrainingRoomScheduleMapping(this.Type, sqlConnection);

                date = date.AddDays(1);
            }
        }

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.TrainingRoomScheduleMappingTransactionSuccessful;

        return returnValue;
    }
}