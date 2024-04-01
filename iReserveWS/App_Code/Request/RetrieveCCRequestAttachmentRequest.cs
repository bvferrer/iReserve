using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for RetrieveCCRequestAttachmentRequest
/// </summary>
public class RetrieveCCRequestAttachmentRequest
{
	public RetrieveCCRequestAttachmentRequest()
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

    public RetrieveCCRequestAttachmentResult Process()
    {
        RetrieveCCRequestAttachmentResult returnValue = new RetrieveCCRequestAttachmentResult();

        CCRequestAttachment ccRequestAttachment = new CCRequestAttachment();
        returnValue.CCAttachmentList = ccRequestAttachment.RetrieveCCRequestAttachment(this.CCRequestReferenceNo);

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.RetrieveCCRequestAttachmentSuccessful;

        return returnValue;
    }
}