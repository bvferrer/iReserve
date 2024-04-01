using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for RetrieveCCRequestHistoryRecordsRequest
/// </summary>
public class RetrieveCCRequestHistoryRecordsRequest
{
	public RetrieveCCRequestHistoryRecordsRequest()
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

    public RetrieveCCRequestHistoryRecordsResult Process()
    {
        RetrieveCCRequestHistoryRecordsResult returnValue = new RetrieveCCRequestHistoryRecordsResult();

        CCRequestHistory ccRequestHistory = new CCRequestHistory();
        returnValue.CCRequestHistoryList = ccRequestHistory.RetrieveCCRequestHistory(this.CCRequestReferenceNo);

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.RetrieveCCRequestHistoryRecordsSuccessful;

        return returnValue;
    }
}