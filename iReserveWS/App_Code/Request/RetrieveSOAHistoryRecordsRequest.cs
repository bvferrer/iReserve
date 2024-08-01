using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for RetrieveSOAHistoryRecordsRequest
/// </summary>
public class RetrieveSOAHistoryRecordsRequest
{
	public RetrieveSOAHistoryRecordsRequest()
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

    public RetrieveSOAHistoryRecordsResult Process()
    {
        RetrieveSOAHistoryRecordsResult returnValue = new RetrieveSOAHistoryRecordsResult();

        SOAHistory soaHistory = new SOAHistory();
        returnValue.SOAHistoryList = soaHistory.RetrieveSOAHistory(this.CCRequestReferenceNo);

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.RetrieveSOAHistoryRecordsSuccessful;

        return returnValue;
    }
}