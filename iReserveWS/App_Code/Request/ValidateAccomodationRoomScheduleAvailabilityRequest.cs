using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for ValidateAccomodationRoomScheduleAvailabilityRequest
/// </summary>
public class ValidateAccomodationRoomScheduleAvailabilityRequest
{
	public ValidateAccomodationRoomScheduleAvailabilityRequest()
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

    public ValidateAccomodationRoomScheduleAvailabilityResult Process()
    {
        ValidateAccomodationRoomScheduleAvailabilityResult returnValue = new ValidateAccomodationRoomScheduleAvailabilityResult();

        AccomodationRoomScheduleMapping accomodationRoomSchedule = new AccomodationRoomScheduleMapping();
        returnValue.ValidationStatus = accomodationRoomSchedule.ValidateAccomodationRoomScheduleAvailability(this.RoomID, this.StartDate, this.EndDate);

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.ValidateAccomodationRoomScheduleAvailabilitySuccessful;

        return returnValue;
    }
}