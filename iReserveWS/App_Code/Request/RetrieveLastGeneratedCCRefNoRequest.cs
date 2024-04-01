using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for RetrieveLastGeneratedCCRefNoRequest
/// </summary>
public class RetrieveLastGeneratedCCRefNoRequest
{
	public RetrieveLastGeneratedCCRefNoRequest()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private DateTime _dateGenerated;

    public DateTime DateGenerated
    {
        get { return _dateGenerated; }
        set { _dateGenerated = value; }
    }

    public RetrieveLastGeneratedCCRefNoResult Process()
    {
        RetrieveLastGeneratedCCRefNoResult returnValue = new RetrieveLastGeneratedCCRefNoResult();

        CCLastGeneratedReferenceNumber ccLastGeneratedReferenceNumber = new CCLastGeneratedReferenceNumber();
        ccLastGeneratedReferenceNumber.RetrieveLastGeneratedCCRefNo(this.DateGenerated);

        returnValue.CCLastGeneratedReferenceNumber = ccLastGeneratedReferenceNumber;
        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.RetrieveLastGeneratedCCRefNoSuccessful;

        return returnValue;
    }
}