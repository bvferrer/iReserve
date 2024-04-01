using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for MaintenanceLocationList
/// </summary>
public class MaintenanceLocationList
{
	public MaintenanceLocationList()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private int _LocationID;
    private string _LocationCode;
    private string _LocationName;
    private string _LocationDesc;
    private string _IsDeleted;

    public int LocationID
    {
        get { return _LocationID; }
        set { _LocationID = value; }
    }
    public string LocationCode
    {
        get { return _LocationCode; }
        set { _LocationCode = value; }
    }
    public string LocationName
    {
        get { return _LocationName; }
        set { _LocationName = value; }
    }
    public string LocationDesc
    {
        get { return _LocationDesc; }
        set { _LocationDesc = value; }
    }
    public string IsDeleted
    {
        get { return _IsDeleted; }
        set { _IsDeleted = value; }
    }
}