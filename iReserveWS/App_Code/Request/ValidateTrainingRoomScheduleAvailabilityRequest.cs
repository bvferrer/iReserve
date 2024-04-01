using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for ValidateTrainingRoomScheduleAvailabilityRequest
/// </summary>
public class ValidateTrainingRoomScheduleAvailabilityRequest
{
	public ValidateTrainingRoomScheduleAvailabilityRequest()
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

    public ValidateTrainingRoomScheduleAvailabilityResult Process()
    {
        ValidateTrainingRoomScheduleAvailabilityResult returnValue = new ValidateTrainingRoomScheduleAvailabilityResult();

        TrainingRoomScheduleMapping trainingRoomSchedule = new TrainingRoomScheduleMapping();
        returnValue.ValidationStatus = trainingRoomSchedule.ValidateTrainingRoomScheduleAvailability(this.RoomID, this.StartDate, this.EndDate);

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.ValidateTrainingRoomScheduleAvailabilitySuccessful;

        return returnValue;
    }
}