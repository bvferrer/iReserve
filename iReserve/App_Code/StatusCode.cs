using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for StatusCode
/// </summary>
public class StatusCode
{
	public StatusCode()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    public static int Confirmed
    {
        get
        {
            return 0;
        }
    }

    public static int Cancelled
    {
        get
        {
            return 1;
        }
    }

    public static int ForConfirmation
    {
        get
        {
            return 2;
        }
    }

    public static int ForCancellation
    {
        get
        {
            return 3;
        }
    }

    public static int Declined
    {
        get
        {
            return 4;
        }
    }

    public static int Failed
    {
        get
        {
            return 5;
        }
    }

    public static string GetStatusName(int statusCode)
    {
        string statusName = "";

        switch (statusCode)
        {
            case 0:
                statusName = "Confirmed";
                break;
            case 1:
                statusName = "Cancelled";
                break;
            case 4:
                statusName = "Declined";
                break;
            default:
                break;
        }

        return statusName;
    }
}