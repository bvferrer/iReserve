using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for RetrieveAccRoomCalendarScheduleRequest
/// </summary>
public class RetrieveAccRoomCalendarScheduleRequest
{
	public RetrieveAccRoomCalendarScheduleRequest()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private string _dateFrom;

    public string DateFrom
    {
        get { return _dateFrom; }
        set { _dateFrom = value; }
    }

    public RetrieveAccRoomCalendarScheduleResult Process()
    {
        RetrieveAccRoomCalendarScheduleResult returnValue = new RetrieveAccRoomCalendarScheduleResult();

        AccomodationRoom accomodationRoom = new AccomodationRoom();
        returnValue.AccRoomScheduleDataTable = accomodationRoom.RetrieveAccRoomCalendarSchedule(this.DateFrom);

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.RetrieveAccRoomCalendarScheduleSuccessful;

        return returnValue;
    }
}