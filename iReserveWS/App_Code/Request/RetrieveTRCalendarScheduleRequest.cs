using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for RetrieveTRCalendarScheduleRequest
/// </summary>
public class RetrieveTRCalendarScheduleRequest
{
	public RetrieveTRCalendarScheduleRequest()
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

    public RetrieveTRCalendarScheduleResult Process()
    {
        RetrieveTRCalendarScheduleResult returnValue = new RetrieveTRCalendarScheduleResult();

        TrainingRoom trainingRoom = new TrainingRoom();
        returnValue.TRScheduleDataTable = trainingRoom.RetrieveTRCalendarSchedule(this.DateFrom);

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.RetrieveTRCalendarScheduleSuccessful;

        return returnValue;
    }
}