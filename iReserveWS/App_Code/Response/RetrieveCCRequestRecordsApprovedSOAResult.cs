﻿using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for RetrieveCCRequestRecordsApprovedSOAResult
/// </summary>
public class RetrieveCCRequestRecordsApprovedSOAResult : IBaseResult
{
	public RetrieveCCRequestRecordsApprovedSOAResult()
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

    private List<CCRequest> _ccRequestList;

    public List<CCRequest> CCRequestList
    {
        get { return _ccRequestList; }
        set { _ccRequestList = value; }
    }
}