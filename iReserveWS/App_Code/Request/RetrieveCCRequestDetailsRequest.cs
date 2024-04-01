using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for RetrieveCCRequestDetailsRequest
/// </summary>
public class RetrieveCCRequestDetailsRequest
{
	public RetrieveCCRequestDetailsRequest()
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

    public RetrieveCCRequestDetailsResult Process()
    {
        RetrieveCCRequestDetailsResult returnValue = new RetrieveCCRequestDetailsResult();

        CCRequest ccRequest = new CCRequest();
        ccRequest.RetrieveCCRequestDetails(this.CCRequestReferenceNo);
        returnValue.CCRequest = ccRequest;

        CCRequestAttachment ccRequestAttachment = new CCRequestAttachment();
        returnValue.CCRequestAttachmentList = ccRequestAttachment.RetrieveCCRequestAttachment(this.CCRequestReferenceNo);

        TrainingRoomRequest trainingRoomRequest = new TrainingRoomRequest();
        returnValue.TrainingRoomRequestList = trainingRoomRequest.RetrieveTrainingRoomRequestRecords(this.CCRequestReferenceNo);

        AccomodationRoomRequest accomodationRoomRequest = new AccomodationRoomRequest();
        returnValue.AccomodationRoomRequest = accomodationRoomRequest.RetrieveAccomodationRoomRequestRecords(this.CCRequestReferenceNo);

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.RetrieveCRRequestDetailsSuccessful;

        return returnValue;
    }
}