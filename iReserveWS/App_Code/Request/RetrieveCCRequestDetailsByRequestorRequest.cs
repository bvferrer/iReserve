using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for RetrieveCCRequestDetailsByRequestorRequest
/// </summary>
public class RetrieveCCRequestDetailsByRequestorRequest
{
	public RetrieveCCRequestDetailsByRequestorRequest()
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

    private string _createdByID;

    public string CreatedByID
    {
        get { return _createdByID; }
        set { _createdByID = value; }
    }

    public RetrieveCCRequestDetailsByRequestorResult Process()
    {
        RetrieveCCRequestDetailsByRequestorResult returnValue = new RetrieveCCRequestDetailsByRequestorResult();

        CCRequest ccRequest = new CCRequest();
        ccRequest.RetrieveCCRequestDetailsByRequestor(this.CCRequestReferenceNo, this.CreatedByID);
        returnValue.CCRequest = ccRequest;

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.RetrieveCCRequestDetailsByRequestorSuccessful;

        return returnValue;
    }
}