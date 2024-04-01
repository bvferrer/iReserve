using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for RetrieveAccomodationRoomRecordDetailsRequest
/// </summary>
public class RetrieveAccomodationRoomRecordDetailsRequest
{
	public RetrieveAccomodationRoomRecordDetailsRequest()
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

    public RetrieveAccomodationRoomRecordDetailsResult Process()
    {
        RetrieveAccomodationRoomRecordDetailsResult returnValue = new RetrieveAccomodationRoomRecordDetailsResult();

        AccomodationRoom accomodationRoom = new AccomodationRoom();
        accomodationRoom.RetrieveAccomodationRoomRecordDetails(this.RoomID);
        returnValue.AccomodationRoom = accomodationRoom;

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.RetrieveAccomodationRoomSuccessful;

        return returnValue;
    }
}