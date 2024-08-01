using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for MonitorDisplay
/// </summary>
public class MonitorDisplay
{
	public MonitorDisplay()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private int _monitorDisplayID;

    public int MonitorDisplayID
    {
        get { return _monitorDisplayID; }
        set { _monitorDisplayID = value; }
    }

    private int _monitorDisplayCode;

    public int MonitorDisplayCode
    {
        get { return _monitorDisplayCode; }
        set { _monitorDisplayCode = value; }
    }

    private string _monitorDisplayName;

    public string MonitorDisplayName
    {
        get { return _monitorDisplayName; }
        set { _monitorDisplayName = value; }
    }

    private bool _isDeleted;

    public bool IsDeleted
    {
        get { return _isDeleted; }
        set { _isDeleted = value; }
    }
}