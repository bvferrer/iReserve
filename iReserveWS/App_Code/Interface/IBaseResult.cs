using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for IBaseResult
/// </summary>
internal interface IBaseResult
{
    ResultStatus ResultStatus
    {
        get;
        set;
    }

    int LogID
    {
        get;
        set;
    }

    int MessageID
    {
        get;
        set;
    }

    string Message
    {
        get;
        set;
    }
}