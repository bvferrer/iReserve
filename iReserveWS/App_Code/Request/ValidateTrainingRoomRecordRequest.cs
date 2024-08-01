using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for ValidateTrainingRoomRecordRequest
/// </summary>
public class ValidateTrainingRoomRecordRequest
{
	public ValidateTrainingRoomRecordRequest()
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

    private TrainingRoom _trainingRoom;

    public TrainingRoom TrainingRoom
    {
        get { return _trainingRoom; }
        set { _trainingRoom = value; }
    }

    public ValidateTrainingRoomRecordResult Process()
    {
        ValidateTrainingRoomRecordResult returnValue = new ValidateTrainingRoomRecordResult();

        TrainingRoom trainingRoom = new TrainingRoom();
        returnValue.ValidationStatus = trainingRoom.ValidateTrainingRoomRecord(this.Type, this.TrainingRoom.TRoomID, this.TrainingRoom.TRoomCode, this.TrainingRoom.TRoomName);

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.ValidateTrainingRoomRecordSuccessful;

        return returnValue;
    }
}