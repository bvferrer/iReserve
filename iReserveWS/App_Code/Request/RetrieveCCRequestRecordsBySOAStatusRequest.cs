using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for RetrieveCCRequestRecordsBySOAStatusRequest
/// </summary>
public class RetrieveCCRequestRecordsBySOAStatusRequest
{
	public RetrieveCCRequestRecordsBySOAStatusRequest()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    private int _soaStatusCode;

    public int SOAStatusCode
    {
        get { return _soaStatusCode; }
        set { _soaStatusCode = value; }
    }

    public RetrieveCCRequestRecordsBySOAStatusResult Process()
    {
        RetrieveCCRequestRecordsBySOAStatusResult returnValue = new RetrieveCCRequestRecordsBySOAStatusResult();

        CCRequest ccRequest = new CCRequest();
        returnValue.CCRequestList = ccRequest.RetrieveCCRequestRecordsBySOAStatus(this.SOAStatusCode);

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.RetrieveCCRequestRecordsBySOAStatusSuccessful;

        return returnValue;
    }
}