using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for ValidateSummaryScheduleAvailabilityResult
/// </summary>
public class ValidateSummaryScheduleAvailabilityResult : IBaseResult
{
	public ValidateSummaryScheduleAvailabilityResult()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    private ResultStatus _resultStatus;

    public ResultStatus ResultStatus
    {
        get { return _resultStatus; }
        set { _resultStatus = value; }
    }

    private int _logID;

    public int LogID
    {
        get { return _logID; }
        set { _logID = value; }
    }

    private int _messageID;

    public int MessageID
    {
        get { return _messageID; }
        set { _messageID = value; }
    }

    private string _message;

    public string Message
    {
        get { return _message; }
        set { _message = value; }
    }

    private bool _validationStatus;

    public bool ValidationStatus
    {
        get { return _validationStatus; }
        set { _validationStatus = value; }
    }
}