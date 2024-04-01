using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for RetrieveTrainingRoomRecordsRequest
/// </summary>
public class RetrieveTrainingRoomRecordsRequest
{
	public RetrieveTrainingRoomRecordsRequest()
	{
		//
		// TODO: Add constructor logic here
		//
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

    public RetrieveTrainingRoomRecordsResult Process()
    {
        RetrieveTrainingRoomRecordsResult returnValue = new RetrieveTrainingRoomRecordsResult();

        TrainingRoom trainingRoom = new TrainingRoom();
        returnValue.TrainingRoomList = trainingRoom.RetrieveTrainingRoomRecords(this.RoomCode, this.RoomName);

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.RetrieveTrainingRoomSuccessful;

        return returnValue;
    }
}