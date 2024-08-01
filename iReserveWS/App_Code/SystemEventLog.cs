using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for SystemEventLog
/// </summary>
public class SystemEventLog : RDFramework.Utility. EventInformation
{
    #region Constructor
    public SystemEventLog()
        : base(HttpContext.Current.Request.PhysicalApplicationPath)
    {
        this.EventID = RDFramework.Utility.RandomGenerator.GenerateEventID();
        this.EventSource = Settings.EventSource;
    }
    #endregion

    #region Public Methods
    public void LogError(string error)
    {
        this.Message = error;
        this.EventType = System.Diagnostics.EventLogEntryType.Error;
        this.Log();
    }

    public void WrapServerError(string rawError)
    {
        this.Message = rawError;
        this.EventType = System.Diagnostics.EventLogEntryType.Error;
        this.Log();
        this.Message = string.Format(Settings.GenericServerMessage, this.EventID);

        if (Settings.EventEmailErrorEnabled)
        {
            EmailNotification errorNotification = new EmailNotification();
            errorNotification.ConstructErrorNotification(rawError, System.Diagnostics.EventLogEntryType.Error, this.EventID);
            Functions.SendEmailNotification(errorNotification);
        }
    }
    #endregion

}