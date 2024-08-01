using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for RetrieveTRPartitionRecordDetailsResult
/// </summary>
public class RetrieveTRPartitionRecordDetailsResult : IBaseResult
{
	public RetrieveTRPartitionRecordDetailsResult()
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

    private TRPartition _trPartition;

    public TRPartition TRPartition
    {
        get { return _trPartition; }
        set { _trPartition = value; }
    }
}