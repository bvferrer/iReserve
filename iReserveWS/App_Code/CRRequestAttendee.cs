using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Data.SqlClient;

/// <summary>
/// Summary description for CRRequestAttendee
/// </summary>
public class CRRequestAttendee
{
    public CRRequestAttendee()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Properties

    private int _attendeeID;

    public int AttendeeID
    {
        get { return _attendeeID; }
        set { _attendeeID = value; }
    }

    private string _requestReferenceNumber;

    public string RequestReferenceNumber
    {
        get { return _requestReferenceNumber; }
        set { _requestReferenceNumber = value; }
    }

    private string _fullName;

    public string FullName
    {
        get { return _fullName; }
        set { _fullName = value; }
    }

    private string _company;

    public string Company
    {
        get { return _company; }
        set { _company = value; }
    }

    #endregion

    #region Methods

    public void InsertCRRequestAttendee()
    {
        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringWriter))
        {
            sqlConnection.Open();

            using (SqlCommand sqlCommand = new SqlCommand(Common.usp_InsertCRRequestAttendee, sqlConnection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.RequestReferenceNumber));
                sqlCommand.Parameters.AddWithValue("@fullName", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.FullName));
                sqlCommand.Parameters.AddWithValue("@company", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.Company));
                sqlCommand.ExecuteNonQuery();
            }
        }
    }

    #endregion
}