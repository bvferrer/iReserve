using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SessionAbandon : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["FirstLogOnChecker"] = "";
        Session.Contents.Remove("AccountStatus");
        Session.Contents.Remove("UserID");
        Response.Redirect("Login.aspx");
    }
}