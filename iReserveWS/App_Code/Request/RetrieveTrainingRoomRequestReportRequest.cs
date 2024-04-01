using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for RetrieveTrainingRoomRequestReportRequest
/// </summary>
public class RetrieveTrainingRoomRequestReportRequest
{
	public RetrieveTrainingRoomRequestReportRequest()
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

    public RetrieveTrainingRoomRequestReportResult Process()
    {
        RetrieveTrainingRoomRequestReportResult returnValue = new RetrieveTrainingRoomRequestReportResult();

        TrainingRoomRequestReport trainingRoomRequestReport = new TrainingRoomRequestReport();
        returnValue.TrainingRoomRequestReportList = trainingRoomRequestReport.RetrieveTrainingRoomRequestReport(this.SelectedStatus, this.StartDate, this.EndDate);

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.RetrieveTrainingRoomRequestReportSuccessful;

        return returnValue;
    }
}