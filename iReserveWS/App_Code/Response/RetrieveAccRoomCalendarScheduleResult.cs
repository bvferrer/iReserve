using System;
using System.Collections.Generic;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for RetrieveAccRoomCalendarScheduleResult
/// </summary>
public class RetrieveAccRoomCalendarScheduleResult : IBaseResult
{
	public RetrieveAccRoomCalendarScheduleResult()
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

    private DataTable _accRoomScheduleDataTable;

    public DataTable AccRoomScheduleDataTable
    {
        get { return _accRoomScheduleDataTable; }
        set { _accRoomScheduleDataTable = value; }
    }
}