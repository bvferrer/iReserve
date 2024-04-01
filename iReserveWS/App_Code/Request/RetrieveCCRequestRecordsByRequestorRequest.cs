using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for RetrieveCCRequestRecordsByRequestorRequest
/// </summary>
public class RetrieveCCRequestRecordsByRequestorRequest
{
	public RetrieveCCRequestRecordsByRequestorRequest()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private string _createdByID;

    public string CreatedByID
    {
        get { return _createdByID; }
        set { _createdByID = value; }
    }

    private int _statusCode;

    public int StatusCode
    {
        get { return _statusCode; }
        set { _statusCode = value; }
    }

    public RetrieveCCRequestRecordsByRequestorResult Process()
    {
        RetrieveCCRequestRecordsByRequestorResult returnValue = new RetrieveCCRequestRecordsByRequestorResult();

        CCRequest ccRequest = new CCRequest();
        returnValue.CCRequestList = ccRequest.RetrieveCCRequestRecordsByRequestor(this.CreatedByID, this.StatusCode);

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.RetrieveCCRequestRecordsByRequestorSuccessful;

        return returnValue;
    }
}