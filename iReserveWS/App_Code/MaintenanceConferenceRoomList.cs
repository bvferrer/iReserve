using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for MaintenanceConferenceRoomList
/// </summary>
public class MaintenanceConferenceRoomList
{
	public MaintenanceConferenceRoomList()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private int _RoomID;
    private string _RoomCode;
    private string _RoomName;
    private string _RoomDesc;
    private int _LocationID;
    private string _LocationName;
    private string _MaxPerson;
    private string _IsDataPortAvailable;
    private string _IsMonitorAvailable;
    private string _RatePerHour;
    private string _TabletID;
    private int _MonitorDisplayCode;
    private string _MonitorDisplayName;
    private string _IsDeleted;

    public int RoomID
    {
        get { return _RoomID; }
        set { _RoomID = value; }
    }

    public string RoomCode
    {
        get { return _RoomCode; }
        set { _RoomCode = value; }
    }

    public string RoomName
    {
        get { return _RoomName; }
        set { _RoomName = value; }
    }

    public string RoomDesc
    {
        get { return _RoomDesc; }
        set { _RoomDesc = value; }
    }

    public int LocationID
    {
        get { return _LocationID; }
        set { _LocationID = value; }
    }

    public string LocationName
    {
        get { return _LocationName; }
        set { _LocationName = value; }
    }

    public string MaxPerson
    {
        get { return _MaxPerson; }
        set { _MaxPerson = value; }
    }

    public string IsDataPortAvailable
    {
        get { return _IsDataPortAvailable; }
        set { _IsDataPortAvailable = value; }
    }

    public string IsMonitorAvailable
    {
        get { return _IsMonitorAvailable; }
        set { _IsMonitorAvailable = value; }
    }

    public string RatePerHour
    {
        get { return _RatePerHour; }
        set { _RatePerHour = value; }
    }

    public string TabletID
    {
        get { return _TabletID; }
        set { _TabletID = value; }
    }

    public int MonitorDisplayCode
    {
        get { return _MonitorDisplayCode; }
        set { _MonitorDisplayCode = value; }
    }

    public string MonitorDisplayName
    {
        get { return _MonitorDisplayName; }
        set { _MonitorDisplayName = value; }
    }

    public string IsDeleted
    {
        get { return _IsDeleted; }
        set { _IsDeleted = value; }
    }
}