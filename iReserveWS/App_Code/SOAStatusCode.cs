using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for SOAStatusCode
/// </summary>
public class SOAStatusCode
{
	public SOAStatusCode()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    public static int ForProcessing
    {
        get
        {
            return 0;
        }
    }

    public static int ForApproval
    {
        get
        {
            return 1;
        }
    }

    public static int Approved
    {
        get
        {
            return 2;
        }
    }

    public static int Completed
    {
        get
        {
            return 3;
        }
    }

    public static int Disapproved
    {
        get
        {
            return 4;
        }
    }
}