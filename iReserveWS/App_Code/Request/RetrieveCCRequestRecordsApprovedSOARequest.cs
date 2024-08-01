using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for RetrieveCCRequestRecordsApprovedSOARequest
/// </summary>
public class RetrieveCCRequestRecordsApprovedSOARequest
{
	public RetrieveCCRequestRecordsApprovedSOARequest()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    public RetrieveCCRequestRecordsApprovedSOAResult Process()
    {
        RetrieveCCRequestRecordsApprovedSOAResult returnValue = new RetrieveCCRequestRecordsApprovedSOAResult();

        CCRequest ccRequest = new CCRequest();
        returnValue.CCRequestList = ccRequest.RetrieveCCRequestRecordsApprovedSOA();

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.RetrieveCCRequestRecordsApprovedSOASuccessful;

        return returnValue;
    }
}