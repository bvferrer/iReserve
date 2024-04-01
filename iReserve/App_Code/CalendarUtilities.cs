using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for CalendarUtilities
/// </summary>
public class CalendarUtilities
{
	public CalendarUtilities()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static string GetDateRange(string dateFrom)
    {
        string dateCollection = "";
        int counter = 0;
        int numberOfDays = 20;

        while (counter < numberOfDays)
        {
            dateCollection += Convert.ToDateTime(dateFrom).ToString("MM/dd/yyyy");

            if (counter < (numberOfDays - 1))
            {
                dateCollection += ",";
            }

            dateFrom = Convert.ToDateTime(dateFrom).AddDays(1).ToString("MM/dd/yyyy");
            counter += 1;
        }

        return dateCollection;
    }

    public static void LoadInventoryHeader(string dateFrom, string dateTo, HtmlGenericControl divName)
    {
        string[] weekday = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
        string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        int c_year = Convert.ToDateTime(dateFrom).Year;
        string r_year = "<tr class =calYear><td rowspan=3 class=time>Time</td>";
        int daysInYear = 0;

        int c_month = Convert.ToDateTime(dateFrom).Month;
        string r_month = "<tr class = calMonth>";
        int daysInMonth = 0;

        string r_days = "<tr>";

        for (DateTime date = Convert.ToDateTime(dateFrom); date <= Convert.ToDateTime(dateTo); date = date.AddDays(1))
        {

            if (date.Year != c_year)
            {
                r_year += "<td colspan=" + daysInYear + ">" + c_year + "</td>";
                c_year = date.Year;
                daysInYear = 0;
            }
            daysInYear++;

            if (date.Month != c_month)
            {
                r_month += "<td colspan=" + daysInMonth + ">" + months[c_month - 1] + "</td>";
                c_month = date.Month;
                daysInMonth = 0;
            }
            daysInMonth++;

            r_days += "<td class=calDay>" + date.Day + "<br>" + weekday[(int)date.DayOfWeek] + "</td>";
        }

        r_days += "</tr>";
        r_year += "<td colspan=" + (daysInYear) + ">" + c_year + "</td>";
        r_year += "</tr>";
        r_month += "<td colspan=" + (daysInMonth) + ">" + months[c_month - 1] + "</td>";
        r_month += "</tr>";
        string table = "<div class=calendar><table>" + r_year + r_month + r_days + "</table></div>";

        divName.InnerHtml = table;
    }

    public static void LoadTRInventoryHeader(string dateFrom, string dateTo, HtmlGenericControl divName)
    {
        string[] weekday = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
        string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        int c_year = Convert.ToDateTime(dateFrom).Year;
        string r_year = "<tr class =calYear><td rowspan=3 class=room>Training Rooms</td>";
        int daysInYear = 0;

        int c_month = Convert.ToDateTime(dateFrom).Month;
        string r_month = "<tr class = calMonth>";
        int daysInMonth = 0;

        string r_days = "<tr>";

        for (DateTime date = Convert.ToDateTime(dateFrom); date <= Convert.ToDateTime(dateTo); date = date.AddDays(1))
        {

            if (date.Year != c_year)
            {
                r_year += "<td colspan=" + daysInYear + ">" + c_year + "</td>";
                c_year = date.Year;
                daysInYear = 0;
            }
            daysInYear++;

            if (date.Month != c_month)
            {
                r_month += "<td colspan=" + daysInMonth + ">" + months[c_month - 1] + "</td>";
                c_month = date.Month;
                daysInMonth = 0;
            }
            daysInMonth++;

            r_days += "<td class=calDay>" + date.Day + "<br>" + weekday[(int)date.DayOfWeek] + "</td>";
        }

        r_days += "</tr>";
        r_year += "<td colspan=" + (daysInYear) + ">" + c_year + "</td>";
        r_year += "</tr>";
        r_month += "<td colspan=" + (daysInMonth) + ">" + months[c_month - 1] + "</td>";
        r_month += "</tr>";
        string table = "<div class=calendar><table>" + r_year + r_month + r_days + "</table></div>";

        divName.InnerHtml = table;
    }

    public static void LoadARInventoryHeader(string dateFrom, string dateTo, HtmlGenericControl divName)
    {
        string[] weekday = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
        string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        int c_year = Convert.ToDateTime(dateFrom).Year;
        string r_year = "<tr class =calYear><td rowspan=3 class=room>Accomodation Rooms</td>";
        int daysInYear = 0;

        int c_month = Convert.ToDateTime(dateFrom).Month;
        string r_month = "<tr class = calMonth>";
        int daysInMonth = 0;

        string r_days = "<tr>";

        for (DateTime date = Convert.ToDateTime(dateFrom); date <= Convert.ToDateTime(dateTo); date = date.AddDays(1))
        {

            if (date.Year != c_year)
            {
                r_year += "<td colspan=" + daysInYear + ">" + c_year + "</td>";
                c_year = date.Year;
                daysInYear = 0;
            }
            daysInYear++;

            if (date.Month != c_month)
            {
                r_month += "<td colspan=" + daysInMonth + ">" + months[c_month - 1] + "</td>";
                c_month = date.Month;
                daysInMonth = 0;
            }
            daysInMonth++;

            r_days += "<td class=calDay>" + date.Day + "<br>" + weekday[(int)date.DayOfWeek] + "</td>";
        }

        r_days += "</tr>";
        r_year += "<td colspan=" + (daysInYear) + ">" + c_year + "</td>";
        r_year += "</tr>";
        r_month += "<td colspan=" + (daysInMonth) + ">" + months[c_month - 1] + "</td>";
        r_month += "</tr>";
        string table = "<div class=calendar><table>" + r_year + r_month + r_days + "</table></div>";

        divName.InnerHtml = table;
    }
}