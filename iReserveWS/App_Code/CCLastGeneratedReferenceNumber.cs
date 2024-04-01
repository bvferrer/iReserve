using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for CCLastGeneratedReferenceNumber
/// </summary>
public class CCLastGeneratedReferenceNumber
{
	public CCLastGeneratedReferenceNumber()
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

    public void RetrieveLastGeneratedCCRefNo(DateTime dateGenerated)
    {
        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringWriter))
        {
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveLastGeneratedCCRefNo, sqlConnection))
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