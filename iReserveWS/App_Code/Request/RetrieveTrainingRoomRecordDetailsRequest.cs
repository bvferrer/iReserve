using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for RetrieveTrainingRoomRecordDetailsRequest
/// </summary>
public class RetrieveTrainingRoomRecordDetailsRequest
{
	public RetrieveTrainingRoomRecordDetailsRequest()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    private int _roomID;

    public int RoomID
    {
        get { return _roomID; }
        set { _roomID = value; }
    }

    public RetrieveTrainingRoomRecordDetailsResult Process()
    {
        RetrieveTrainingRoomRecordDetailsResult returnValue = new RetrieveTrainingRoomRecordDetailsResult();
        returnValue.TRPartitionList = new List<TRPartition>();
        returnValue.TRRateList = new List<TRRate>();

        TrainingRoom trainingRoom = new TrainingRoom();
        trainingRoom.RetrieveTrainingRoomRecordDetails(this.RoomID);

        TRPartition trPartition = new TRPartition();
        List<TRPartition> trPartitionList = trPartition.RetrieveTRPartitionRecords(this.RoomID);

        TRRate trRate = new TRRate();
        List<TRRate> trRateList = trRate.RetrieveTRRateRecords(this.RoomID);

        returnValue.TrainingRoom = trainingRoom;
        returnValue.TRPartitionList = trPartitionList;
        returnValue.TRRateList = trRateList;

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.RetrieveTrainingRoomSuccessful;

        return returnValue;
    }
}