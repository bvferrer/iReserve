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

    public static string iReserveConnectionStringReader
    {
        get
        {
            return RDFramework.Utility.Configuration.GetConnectionString("iReserve_Reader");
        }
    }

    public static string iReserveConnectionStringWriter
    {
        get
        {
            return RDFramework.Utility.Configuration.GetConnectionString("iReserve_Writer");
        }
    }

    public static string AACFConnectionString
    {
        get
        {
            return RDFramework.Utility.Configuration.GetConnectionString("AACFConnection");
        }
    }

    public static string AuditTrailConnectionString
    {
        get
        {
            return RDFramework.Utility.Configuration.GetConnectionString("iReserve_Writer");
        }
    }

    public static string EventSource
    {
        get
        {
            return RDFramework.Utility.Configuration.GetAppSetting("EventSource");
        }
    }

    public static string GenericServerMessage
    {
        get { return "Server has encountered an error. Event ID {0}"; }
    }

    public static string GenericDeclineMessage
    {
        get { return "The schedule you requested is no longer available."; }
    }

    public static string AdminEmailAddress
    {
        get
        {
            return RDFramework.Utility.Configuration.GetAppSetting("DefaultAdminEmailAdd");
        }
    }

    public static string CCAdminEmailAddress
    {
        get
        {
            return RDFramework.Utility.Configuration.GetAppSetting("DefaultCCAdminEmailAdd");
        }
    }

    public static string SOAApproverEmailAddress
    {
        get
        {
            return RDFramework.Utility.Configuration.GetAppSetting("DefaultSOAApproverEmailAdd");
        }
    }

    public static string RequestorNotification
    {
        get { return "~/EmailNotifications/RequestorNotification.htm"; }
    }

    public static string RequestorConfirmedNotification
    {
        get { return "~/EmailNotifications/RequestorConfirmedNotification.htm"; }
    }

    public static string AdminNotification
    {
        get { return "~/EmailNotifications/AdminNotification.htm"; }
    }

    public static string CCRequestorNotification
    {
        get { return "~/EmailNotifications/CCRequestorNotification.htm"; }
    }

    public static string SOANotification
    {
        get { return "~/EmailNotifications/SOANotification.htm"; }
    }

    public static string SOADetails
    {
        get { return "~/EmailNotifications/SOADetails.htm"; }
    }

    public static string EmailSubjectFormat
    {
        get { return "[{0}] iReserve Reservation Request"; }
    }

    public static string SOAEmailSubjectFormat
    {
        get { return "[{0}] iReserve Reservation Request Statement of Account"; }
    }

    public static string SOADetailsEmailSubjectFormat
    {
        get { return "iReserve Reservation Request Statement of Account"; }
    }

    public static string GetEmailMessage(int statusCode)
    {
        string message = "";

        switch (statusCode)
        {
            case 0 :
                message = "has been confirmed";
                break;
            case 1 :
                message = "has been cancelled";
                break;
            case 2 :
                message = "is currently pending for confirmation";
                break;
            case 3:
                message = "is currently pending for cancellation";
                break;
            case 4:
                message = "has been declined";
                break;
            default :
                break;
        }

        return message;
    }

    public static string GetSOAEmailMessage(int statusCode)
    {
        string message = "";

        switch (statusCode)
        {
            case 0:
                message = "is currently pending for processing";
                break;
            case 1:
                message = "is currently pending for approval";
                break;
            case 2:
                message = "has been approved";
                break;
            case 3:
                message = "has been completed";
                break;
            case 4:
                message = "has been disapproved";
                break;
            default:
                break;
        }

        return message;
    }

    internal static string EventEnvironment
    {
        get
        {
            return RDFramework.Utility.Configuration.GetAppSetting("EventEnvironment");
        }
    }

    internal static bool EventEmailErrorEnabled
    {
        get
        {
            return Convert.ToBoolean(RDFramework.Utility.Configuration.GetAppSetting("EventEmailErrorEnabled"));
        }
    }

    internal static string EventEmailTo
    {
        get
        {
            return RDFramework.Utility.Configuration.GetAppSetting("EventEmailTo");
        }
    }

    internal static string SystemID
    {
        get { return "IRSV"; }
    }

    internal static string SystemName
    {
        get { return "iReserve"; }
    }

    internal static string SystemAuditTrailEntry
    {
        get { return "iReserve"; }
    }
}