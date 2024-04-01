using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for RetrieveCRRequestAttachmentByStatusRequest
/// </summary>
public class RetrieveCCRequestAttachmentByStatusRequest
{
	public RetrieveCCRequestAttachmentByStatusRequest()
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

    private int _statusCode;

    public int StatusCode
    {
        get { return _statusCode; }
        set { _statusCode = value; }
    }

    public RetrieveCCRequestAttachmentByStatusResult Process()
    {
        RetrieveCCRequestAttachmentByStatusResult returnValue = new RetrieveCCRequestAttachmentByStatusResult();

        CCRequestAttachment ccRequestAttachment = new CCRequestAttachment();
        ccRequestAttachment.RetrieveCCRequestAttachmentByStatus(this.CCRequestReferenceNo, this.StatusCode);
        returnValue.CCRequestAttachment = ccRequestAttachment;

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.RetrieveCCRequestAttachmentByStatusSuccessful;

        return returnValue;
    }
}