using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for RetrieveAccomodationRoomRecordsRequest
/// </summary>
public class RetrieveAccomodationRoomRecordsRequest
{
    public RetrieveAccomodationRoomRecordsRequest()
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

    public RetrieveAccomodationRoomRecordsResult Process()
    {
        RetrieveAccomodationRoomRecordsResult returnValue = new RetrieveAccomodationRoomRecordsResult();

        AccomodationRoom accomodationRoom = new AccomodationRoom();
        returnValue.AccomodationRoomList = accomodationRoom.RetrieveAccomodationRoomRecords(this.RoomCode, this.RoomName);

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.RetrieveAccomodationRoomSuccessful;

        return returnValue;
    }
}