using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for Status
/// </summary>
public class Status
{
	public Status()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    #region Properties

    private int _statusID;

    public int StatusID
    {
        get { return _statusID; }
        set { _statusID = value; }
    }

    private int _statusCode;

    public int StatusCode
    {
        get { return _statusCode; }
        set { _statusCode = value; }
    }

    private string _statusName;

    public string StatusName
    {
        get { return _statusName; }
        set { _statusName = value; }
    }

    #endregion
}