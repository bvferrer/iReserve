<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e)
    {
        HttpException lastErrorWrapper = Server.GetLastError() as HttpException;

        Exception ex = lastErrorWrapper;

        if (lastErrorWrapper.InnerException != null)
        {
            ex = lastErrorWrapper.InnerException;
        }

        SystemEventLog eventLog = new SystemEventLog();
        eventLog.WrapServerError(ex.ToString());

        Server.ClearError();
        Server.Transfer("~/ErrorPages/Error.aspx");
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
