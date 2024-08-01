using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for OtherCharge
/// </summary>
public class OtherCharge
{
	public OtherCharge()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    #region Properties

    private int _otherChargeID;

    public int OtherChargeID
    {
        get { return _otherChargeID; }
        set { _otherChargeID = value; }
    }

    private string _ccRequestReferenceNo;

    public string CCRequestReferenceNo
    {
        get { return _ccRequestReferenceNo; }
        set { _ccRequestReferenceNo = value; }
    }

    private string _particulars;

    public string Particulars
    {
        get { return _particulars; }
        set { _particulars = value; }
    }

    private string _remarks;

    public string Remarks
    {
        get { return _remarks; }
        set { _remarks = value; }
    }

    private string _amountCharge;

    public string AmountCharge
    {
        get { return _amountCharge; }
        set { _amountCharge = value; }
    }

    private bool _isDeleted;

    public bool IsDeleted
    {
        get { return _isDeleted; }
        set { _isDeleted = value; }
    }

    #endregion

    #region Methods

    public List<OtherCharge> RetrieveCCRequestOtherCharges(string ccRequestReferenceNo)
    {
        List<OtherCharge> otherChargeList = new List<OtherCharge>();

        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveCCRequestOtherCharges, sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(ccRequestReferenceNo));

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        OtherCharge otherCharge = new OtherCharge();
                        otherCharge.OtherChargeID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_OtherChargeID"]);
                        otherCharge.CCRequestReferenceNo = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CCRequestReferenceNo"]);
                        otherCharge.Particulars = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_Particulars"]);
                        otherCharge.Remarks = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_Remarks"]);
                        otherCharge.AmountCharge = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_AmountCharge"]);
                        otherChargeList.Add(otherCharge);
                    }
                }
            }
        }

        return otherChargeList;
    }

    public void TranOtherCharge(int type)
    {
        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringWriter))
        {
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.TranOtherCharge, sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@type", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(type));
                sqlCommand.Parameters.AddWithValue("@otherChargeID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.OtherChargeID));
                sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.CCRequestReferenceNo));
                sqlCommand.Parameters.AddWithValue("@particulars", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.Particulars));
                sqlCommand.Parameters.AddWithValue("@remarks", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.Remarks));
                sqlCommand.Parameters.AddWithValue("@amountCharge", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.AmountCharge));
                sqlCommand.Parameters.AddWithValue("@isDeleted", this.IsDeleted);
                sqlCommand.ExecuteNonQuery();
            }
        }
    }

    #endregion
}