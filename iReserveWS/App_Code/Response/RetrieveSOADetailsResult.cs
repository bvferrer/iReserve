using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for RetrieveSOADetailsResult
/// </summary>
public class RetrieveSOADetailsResult : IBaseResult
{
	public RetrieveSOADetailsResult()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    private ResultStatus _resultStatus;

    public ResultStatus ResultStatus
    {
        get { return _resultStatus; }
        set { _resultStatus = value; }
    }

    private int _logID;

    public int LogID
    {
        get { return _logID; }
        set { _logID = value; }
    }

    private int _messageID;

    public int MessageID
    {
        get { return _messageID; }
        set { _messageID = value; }
    }

    private string _message;

    public string Message
    {
        get { return _message; }
        set { _message = value; }
    }

    private CCRequest ccRequest;

    public CCRequest CCRequest
    {
        get { return ccRequest; }
        set { ccRequest = value; }
    }

    private List<TrainingRoomRequestCharge> _trainingRoomRequestChargeList;

    public List<TrainingRoomRequestCharge> TrainingRoomRequestChargeList
    {
        get { return _trainingRoomRequestChargeList; }
        set { _trainingRoomRequestChargeList = value; }
    }

    private List<AccomodationRoomRequest> _accomodationRoomRequestList;

    public List<AccomodationRoomRequest> AccomodationRoomRequestList
    {
        get { return _accomodationRoomRequestList; }
        set { _accomodationRoomRequestList = value; }
    }

    private List<OtherCharge> _otherChargeList;

    public List<OtherCharge> OtherChargeList
    {
        get { return _otherChargeList; }
        set { _otherChargeList = value; }
    }
}