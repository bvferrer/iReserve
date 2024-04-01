using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for Enum
/// </summary>
public class Enum
{
    public enum MessageSource
    {
        RemoteService,
        Application
    }

    public enum ResponseStatus
    {
        Successful,
        Failed,
        Error
    }
}