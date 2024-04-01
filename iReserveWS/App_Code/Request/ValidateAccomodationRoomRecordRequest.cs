using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for ValidateAccomodationRoomRecordRequest
/// </summary>
public class ValidateAccomodationRoomRecordRequest
{
	public ValidateAccomodationRoomRecordRequest()
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

    private AccomodationRoom _accomodationRoom;

    public AccomodationRoom AccomodationRoom
    {
        get { return _accomodationRoom; }
        set { _accomodationRoom = value; }
    }

    public ValidateAccomodationRoomRecordResult Process()
    {
        ValidateAccomodationRoomRecordResult returnValue = new ValidateAccomodationRoomRecordResult();

        AccomodationRoom accomodationRoom = new AccomodationRoom();
        returnValue.ValidationStatus = accomodationRoom.ValidateAccomodationRoomRecord(this.Type, this.AccomodationRoom.AccRoomID, this.AccomodationRoom.RoomCode, this.AccomodationRoom.RoomName);

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.ValidateAccomodationRoomRecordSuccessful;

        return returnValue;
    }

}