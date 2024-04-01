using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for RetrieveAccomodationRoomRequestReportRequest
/// </summary>
public class RetrieveAccomodationRoomRequestReportRequest
{
	public RetrieveAccomodationRoomRequestReportRequest()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    private string _selectedStatus;

    public string SelectedStatus
    {
        get { return _selectedStatus; }
        set { _selectedStatus = value; }
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

    public RetrieveAccomodationRoomRequestReportResult Process()
    {
        RetrieveAccomodationRoomRequestReportResult returnValue = new RetrieveAccomodationRoomRequestReportResult();

        AccomodationRoomRequestReport accomodationRoomRequestReport = new AccomodationRoomRequestReport();
        returnValue.AccomodationRoomRequestReportList = accomodationRoomRequestReport.RetrieveAccomodationRoomRequestReport(this.SelectedStatus, this.StartDate, this.EndDate);

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.RetrieveTrainingRoomRequestReportSuccessful;

        return returnValue;
    }
}