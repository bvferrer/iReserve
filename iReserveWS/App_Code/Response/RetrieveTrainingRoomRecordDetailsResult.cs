using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for RetrieveTrainingRoomRecordDetailsResult
/// </summary>
public class RetrieveTrainingRoomRecordDetailsResult : IBaseResult
{
	public RetrieveTrainingRoomRecordDetailsResult()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    private ResultStatus _resultStatus;

    public ResultStatus ResultStatus
    {
        get { return _resultStatus; }
        set { _resultStatus = value; }
    }

    private int _logID;

    public int LogID
    {
        get { return _logID; }
        set { _logID = value; }
    }

    private int _messageID;

    public int MessageID
    {
        get { return _messageID; }
        set { _messageID = value; }
    }

    private string _message;

    public string Message
    {
        get { return _message; }
        set { _message = value; }
    }

    private TrainingRoom _trainingRoom;

    public TrainingRoom TrainingRoom
    {
        get { return _trainingRoom; }
        set { _trainingRoom = value; }
    }

    private List<TRPartition> _trPartitionList;

    public List<TRPartition> TRPartitionList
    {
        get { return _trPartitionList; }
        set { _trPartitionList = value; }
    }

    private List<TRRate> trRateList;

    public List<TRRate> TRRateList
    {
        get { return trRateList; }
        set { trRateList = value; }
    }
}