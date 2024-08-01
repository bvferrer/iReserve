using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for RetrieveTRPartitionRecordDetailsRequest
/// </summary>
public class RetrieveTRPartitionRecordDetailsRequest
{
	public RetrieveTRPartitionRecordDetailsRequest()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private int _partitionID;

    public int PartitionID
    {
        get { return _partitionID; }
        set { _partitionID = value; }
    }

    public RetrieveTRPartitionRecordDetailsResult Process()
    {
        RetrieveTRPartitionRecordDetailsResult returnValue = new RetrieveTRPartitionRecordDetailsResult();

        TrainingRoom trainingRoom = new TrainingRoom();
        TRPartition trPartition = new TRPartition();
        trainingRoom = trPartition.RetrieveTRPartitionRecordDetails(this.PartitionID);

        returnValue.TrainingRoom = trainingRoom;
        returnValue.TRPartition = trPartition;

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.RetrieveTRPartitionRecordDetailsSuccessful;

        return returnValue;
    }
}