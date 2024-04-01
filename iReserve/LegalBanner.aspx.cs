using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LegalBanner : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnProceed_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session["ReadLegalBanner"] = "Read";
        Response.BufferOutput = true;
        Response.Redirect("Login.aspx");
    }
}