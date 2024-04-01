using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for RetrieveSOADetailsRequest
/// </summary>
public class RetrieveSOADetailsRequest
{
	public RetrieveSOADetailsRequest()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    private string _ccRequestReferenceNo;

    public string CCRequestReferenceNo
    {
        get { return _ccRequestReferenceNo; }
        set { _ccRequestReferenceNo = value; }
    }

    public RetrieveSOADetailsResult Process()
    {
        RetrieveSOADetailsResult returnValue = new RetrieveSOADetailsResult();

        CCRequest ccRequest = new CCRequest();
        ccRequest.RetrieveCCRequestDetails(this.CCRequestReferenceNo);
        returnValue.CCRequest = ccRequest;

        TrainingRoomRequestCharge trainingRoomRequestCharge = new TrainingRoomRequestCharge();
        returnValue.TrainingRoomRequestChargeList = trainingRoomRequestCharge.RetrieveTrainingRoomRequestCharges(this.CCRequestReferenceNo);

        AccomodationRoomRequest accomodationRoomRequest = new AccomodationRoomRequest();
        returnValue.AccomodationRoomRequestList = accomodationRoomRequest.RetrieveAccomodationRoomRequestRecords(this.CCRequestReferenceNo);

        OtherCharge otherCharge = new OtherCharge();
        returnValue.OtherChargeList = otherCharge.RetrieveCCRequestOtherCharges(this.CCRequestReferenceNo);

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.RetrieveCRRequestDetailsSuccessful;

        return returnValue;
    }
}