using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

/// <summary>
/// Summary description for Utilities
/// </summary>
public class Utilities
{
	public Utilities()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static void MyMessageBox(string smessage)
    {
        Page p = (Page)HttpContext.Current.CurrentHandler;

        ScriptManager.RegisterClientScriptBlock(p, typeof(Page), "Message", string.Format("alert('{0}'); window.location.href= window.location;", smessage), true);
    }

    public static void MyMessageBoxWithHomeRedirect(string smessage)
    {
        Page p = (Page)HttpContext.Current.CurrentHandler;

        ScriptManager.RegisterClientScriptBlock(p, typeof(Page), "Message", string.Format("alert('{0}'); window.location.href = 'Default.aspx';", smessage), true);
    }

    public static string FormatURLToBase64(string urlValue)
    {
        urlValue = urlValue.Replace(" ", "+");

        int mod4 = urlValue.Length % 4;
        if (mod4 > 0)
        {
            urlValue += new string('=', 4 - mod4);
        }

        return urlValue;
    }
}