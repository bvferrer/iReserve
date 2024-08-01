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
/// Summary description for CRLastGeneratedReferenceNumber
/// </summary>
public class CRLastGeneratedReferenceNumber
{
	public CRLastGeneratedReferenceNumber()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    #region Properties

    private string _lastReferenceNumber;

    public string LastReferenceNumber
    {
        get { return _lastReferenceNumber; }
        set { _lastReferenceNumber = value; }
    }

    private DateTime _dateGenerated;

    public DateTime DateGenerated
    {
        get { return _dateGenerated; }
        set { _dateGenerated = value; }
    }

    #endregion

    #region Methods

    public void GetLastGeneratedReferenceNumber(DateTime dateGenerated)
    {
        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringWriter))
        {
            using (SqlCommand sqlCommand = new SqlCommand(Common.usp_GetLastGeneratedCRRF, sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@dateGenerated", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(dateGenerated));

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        this.DateGenerated = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateGenerated"]);
                        this.LastReferenceNumber = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_LastReFNo"]);
                    }
                }
            }
        }
    }

    #endregion
}