using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for RetrieveSOAReportRequest
/// </summary>
public class RetrieveSOAReportRequest
{
	public RetrieveSOAReportRequest()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    private DateTime _startDate;

    public DateTime StartDate
    {
        get { return _startDate; }
        set { _startDate = value; }
    }

    private DateTime _endDate;

    public DateTime EndDate
    {
        get { return _endDate; }
        set { _endDate = value; }
    }

    public RetrieveSOAReportResult Process()
    {
        RetrieveSOAReportResult returnValue = new RetrieveSOAReportResult();

        SOAReport soaReport = new SOAReport();
        returnValue.SOAReportList = soaReport.RetrieveSOAReport(this.StartDate, this.EndDate);

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.RetrieveSOAReportSuccessful;

        return returnValue;
    }
}