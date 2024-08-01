using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for Location
/// </summary>
public class Location
{
	public Location()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    #region Properties

    private int _locationID;

    public int LocationID
    {
        get { return _locationID; }
        set { _locationID = value; }
    }

    private string _locationCode;

    public string LocationCode
    {
        get { return _locationCode; }
        set { _locationCode = value; }
    }

    private string _locationName;

    public string LocationName
    {
        get { return _locationName; }
        set { _locationName = value; }
    }

    private string _locationDesc;

    public string LocationDesc
    {
        get { return _locationDesc; }
        set { _locationDesc = value; }
    }

    private string _isDeleted;

    public string IsDeleted
    {
        get { return _isDeleted; }
        set { _isDeleted = value; }
    }

    #endregion
}