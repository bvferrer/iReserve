using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for RetrieveCCRequestRecordsByStatusRequest
/// </summary>
public class RetrieveCCRequestRecordsByStatusRequest
{
    public RetrieveCCRequestRecordsByStatusRequest()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    private int _statusCode;

    public int StatusCode
    {
        get { return _statusCode; }
        set { _statusCode = value; }
    }

    public RetrieveCCRequestRecordsByStatusResult Process()
    {
        RetrieveCCRequestRecordsByStatusResult returnValue = new RetrieveCCRequestRecordsByStatusResult();

        CCRequest ccRequest = new CCRequest();
        returnValue.CCRequestList = ccRequest.RetrieveCCRequestRecordsByStatus(this.StatusCode);

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.RetrieveCCRequestRecordsByStatusSuccessful;

        return returnValue;
    }
}