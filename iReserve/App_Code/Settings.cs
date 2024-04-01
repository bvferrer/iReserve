using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for Settings
/// </summary>
public class Settings
{
	public Settings()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    public static string[] ValidStartTime
    {
        get { return new string[] { "7:00", "8:00", "9:00", "10:00", "11:00", "12:00", "13:00" }; }
    }

    public static string[] ValidEndTime
    {
        get { return new string[] { "16:00", "17:00", "18:00", "19:00", "20:00", "21:00", "22:00", "23:00" }; }
    }

    public static string EventSource
    {
        get
        {
            return RDFramework.Utility.Configuration.GetAppSetting("EventSource");
        }
    }

    public static string iReserveNCVIURL
    {
        get
        {
            return RDFramework.Utility.Configuration.GetAppSetting("iReserveNCVIURL");
        }
    }

    public static string iReserveURL
    {
        get
        {
            return RDFramework.Utility.Configuration.GetAppSetting("iReserveURL");
        }
    }

    public static string GenericServerMessage
    {
        get { return "Server has encountered an error. Event ID {0}"; }
    }

    public static string GenericWebServiceMessage
    {
        get { return "Server has encountered an error on web service."; }
    }

    public static string GenericAuditTrailMessage
    {
        get { return "Server has encountered an error in writing audit trail logs."; }
    }
}