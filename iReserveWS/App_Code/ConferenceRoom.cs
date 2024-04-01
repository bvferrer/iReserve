using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for ConferenceRoom
/// </summary>
public class ConferenceRoom
{
	public ConferenceRoom()
	{

    }

    #region Properties

    private int _roomID;

    public int RoomID
    {
        get { return _roomID; }
        set { _roomID = value; }
    }

    private string _roomCode;

    public string RoomCode
    {
        get { return _roomCode; }
        set { _roomCode = value; }
    }

    private string _roomName;

    public string RoomName
    {
        get { return _roomName; }
        set { _roomName = value; }
    }

    private string _roomDesc;

    public string RoomDesc
    {
        get { return _roomDesc; }
        set { _roomDesc = value; }
    }

    private Location _location;

    public Location Location
    {
        get { return _location; }
        set { _location = value; }
    }

    private string _maxPerson;

    public string MaxPerson
    {
        get { return _maxPerson; }
        set { _maxPerson = value; }
    }

    private string _isDataPortAvailable;

    public string IsDataPortAvailable
    {
        get { return _isDataPortAvailable; }
        set { _isDataPortAvailable = value; }
    }

    private string _isMonitorAvailable;

    public string IsMonitorAvailable
    {
        get { return _isMonitorAvailable; }
        set { _isMonitorAvailable = value; }
    }

    private string _ratePerHour;

    public string RatePerHour
    {
        get { return _ratePerHour; }
        set { _ratePerHour = value; }
    }

    private string _tabletID;

    public string TabletID
    {
        get { return _tabletID; }
        set { _tabletID = value; }
    }

    private MonitorDisplay _monitorDisplay;

    public MonitorDisplay MonitorDisplay
    {
        get { return _monitorDisplay; }
        set { _monitorDisplay = value; }
    }

    private string _isDeleted;

    public string IsDeleted
    {
        get { return _isDeleted; }
        set { _isDeleted = value; }
    }

    #endregion
}