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

    public static string GetSOAStatusName(int soaStatusCode)
    {
        string soaStatusName = "";

        switch (soaStatusCode)
        {
            case 0:
                soaStatusName = "For Processing";
                break;
            case 1:
                soaStatusName = "For Approval";
                break;
            case 2:
                soaStatusName = "Approved";
                break;
            case 3:
                soaStatusName = "Completed";
                break;
            case 4:
                soaStatusName = "Disapproved";
                break;
            default:
                break;
        }

        return soaStatusName;
    }
}