using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for AccomodationRoomTransactionRequest
/// </summary>
public class AccomodationRoomTransactionRequest
{
	public AccomodationRoomTransactionRequest()
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

    private AuditTrail _auditTrail;

    public AuditTrail AuditTrail
    {
        get { return _auditTrail; }
        set { _auditTrail = value; }
    }

    public AccomodationRoomTransactionResult Process()
    {
        AccomodationRoomTransactionResult returnValue = new AccomodationRoomTransactionResult();

        AccomodationRoom accomodationRoom = new AccomodationRoom();
        accomodationRoom.AccomodationRoomTransaction(this.Type, this.AccomodationRoom, this.AuditTrail);

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.AccomodationRoomTransactionSuccessful;

        return returnValue;
    }
}