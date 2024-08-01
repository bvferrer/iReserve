using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1d);
        Response.Expires = -1500;
        Response.CacheControl = "no-cache";

        //To bypass Login module during development
        //Session["UserID"] = "130281";
        //Session["FirstName"] = "Kim Jerick";
        //Session["LastName"] = "Esguerra";
        //Session["FullName"] = "Kim Jerick Esguerra";
        //Session["MacAddress"] = "test";
        ////Session["ProfileName"] = "Conference Room Administrator";
        //Session["ProfileName"] = "Requestor";
        ////Session["ProfileName"] = "Convention Center Administrator";
        ////Session["ProfileName"] = "SOA Approver";

        if (Convert.ToString(Session["UserID"]) == "")
        {
            Response.BufferOutput = true;
            Response.Redirect("Login.aspx");
        }

        if (Convert.ToString(Session["ProfileName"]) == "NCVI Super Admin")
        {
            Session["ProfileName"] = "Requestor";
        }

        string profileName = Convert.ToString(Session["ProfileName"]);

        if (profileName == "Conference Room Administrator")
        {
            Response.Redirect("AdminHome.aspx");
        }
        else if (profileName == "Convention Center Administrator")
        {
            Response.Redirect("CCAdminHome.aspx");
        }
        else if (profileName == "SOA Approver")
        {
            Response.Redirect("SOAApproverHome.aspx");
        }
        else if (profileName == "Requestor")
        {
            Response.Redirect("RequestorHome.aspx");
        }
    }
}