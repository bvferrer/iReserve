using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for ValidateSummaryScheduleAvailabilityRequest
/// </summary>
public class ValidateSummaryScheduleAvailabilityRequest
{
	public ValidateSummaryScheduleAvailabilityRequest()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private List<TrainingRoomRequest> _trainingRoomRequestList;

    public List<TrainingRoomRequest> TrainingRoomRequestList
    {
        get { return _trainingRoomRequestList; }
        set { _trainingRoomRequestList = value; }
    }

    private List<AccomodationRoomRequest> _accomodationRoomRequestList;

    public List<AccomodationRoomRequest> AccomodationRoomRequestList
    {
        get { return _accomodationRoomRequestList; }
        set { _accomodationRoomRequestList = value; }
    }

    public ValidateSummaryScheduleAvailabilityResult Process()
    {
        ValidateSummaryScheduleAvailabilityResult returnValue = new ValidateSummaryScheduleAvailabilityResult();

        TrainingRoomScheduleMapping trainingRoomScheduleMapping = new TrainingRoomScheduleMapping();
        AccomodationRoomScheduleMapping accomodationRoomScheduleMapping = new AccomodationRoomScheduleMapping();

        if (!trainingRoomScheduleMapping.ValidateSummaryTrainingRoomScheduleAvailability(this.TrainingRoomRequestList))
        {
            returnValue.ValidationStatus = false;
        }
        else if (!accomodationRoomScheduleMapping.ValidateSummaryAccomodationRoomScheduleAvailability(this.AccomodationRoomRequestList))
        {
            returnValue.ValidationStatus = false;
        }
        else
        {
            returnValue.ValidationStatus = true;
        }

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.ValidateSummaryScheduleAvailabilitySuccessful;

        return returnValue;
    }
}