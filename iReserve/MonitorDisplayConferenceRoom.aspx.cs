using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iReserveWS;
using System.Data;
using System.Web.Services.Protocols;

public partial class MonitorDisplayConferenceRoom : System.Web.UI.Page
{
    iReserveWS.Service svc = new iReserveWS.Service();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1d);
        Response.Expires = -1500;
        Response.CacheControl = "no-cache";

        if (Request.QueryString["disp"] == null)
        {
            Response.BufferOutput = true;
            Response.Redirect("MonitorDisplayConferenceRoomMain.aspx");
        }

        if (!IsPostBack)
        {
            LoadControls();
        }
    }

    private void LoadControls()
    {
        int monitorDisplayCode = Convert.ToInt32(Request.QueryString["disp"]);
        string date = DateTime.Now.ToShortDateString();
        DataTable scheduledt;

        try
        {
            scheduledt = svc.RetrieveCRDisplaySchedule(date, monitorDisplayCode);
        }

        catch (SoapException ex)
        {
            throw new Exception(Settings.GenericWebServiceMessage);
        }

        if (scheduledt.Rows.Count == 0)
        {
            Response.Write("<script language=javascript>alert('No rooms are assigned to this monitor display. Please assign rooms in File Maintenance module.');window.close();</script>");
        }
        else
        {
            if (DateTime.Now.TimeOfDay >= TimeSpan.Parse("14:00"))
            {
                displayGridView.PageIndex = 1;
            }

            displayDateLabel.Text = Convert.ToDateTime(date).ToString("dddd, MMMM dd, yyyy");

            displayGridView.DataSource = scheduledt;
            displayGridView.DataBind();
        }
    }

    protected void displayGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int columnIndex = 2; columnIndex < e.Row.Cells.Count; columnIndex++)
            {
                if (e.Row.Cells[columnIndex].Text == "&nbsp;" || e.Row.Cells[columnIndex].Text == "")
                {
                    e.Row.Cells[columnIndex].CssClass = "vacantcell";
                }
                else
                {
                    string text = e.Row.Cells[columnIndex].Text;
                    string[] array = text.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    string div = "<div class=displaycontent><span>" + array[1].ToString() + "</span></div>" +
                                "<div class=displaycontent><span><i>" + array[2].ToString() + "</i></span></div>";

                    e.Row.Cells[columnIndex].Text = div;
                    e.Row.Cells[columnIndex].CssClass = "displaycell";

                    CRRequest request = new CRRequest();
                    request.RequestReferenceNo = array[0];

                    CRRequest result = new CRRequest();

                    try
                    {
                        result = svc.RetrieveCRRequestDetails(request);
                    }

                    catch (SoapException ex)
                    {
                        throw new Exception(Settings.GenericWebServiceMessage);
                    }

                    if (Convert.ToBoolean(result.HasLoggedIn))
                    {
                        e.Row.Cells[columnIndex].CssClass = "timedincell";
                    }
                }
            }
        }
    }

    protected void displayGridView_DataBound(object sender, EventArgs e)
    {
        for (int i = displayGridView.Rows.Count - 1; i > 0; i--)
        {
            GridViewRow row = displayGridView.Rows[i];
            GridViewRow previousRow = displayGridView.Rows[i - 1];
            for (int j = 0; j < row.Cells.Count; j++)
            {
                if (row.Cells[j].Text != "&nbsp;" || row.Cells[j].Text == "")
                {
                    if (row.Cells[j].Text == previousRow.Cells[j].Text)
                    {
                        if (previousRow.Cells[j].RowSpan == 0)
                        {
                            if (row.Cells[j].RowSpan == 0)
                            {
                                previousRow.Cells[j].RowSpan += 2;
                            }
                            else
                            {
                                previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                            }
                            row.Cells[j].Visible = false;
                        }
                    }
                }
            }
        }
    }

    protected void pageTimer_Tick(object sender, EventArgs e)
    {
        if ((displayGridView.PageCount - 1) == displayGridView.PageIndex)
        {
            displayGridView.PageIndex = 0;
        }
        else
        {
            displayGridView.PageIndex++;
        }

        LoadControls();
    }
}