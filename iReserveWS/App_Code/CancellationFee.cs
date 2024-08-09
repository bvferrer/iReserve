using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Web;

/// <summary>
/// Summary description for CancellationFee
/// </summary>
public class CancellationFee
{
  public CancellationFee()
  {
    //
    // TODO: Add constructor logic here
    //
  }



  #region Fields/Properties

  private int _cancellationID;
  public int CancellationID
  {
    get { return _cancellationID; }
    set { _cancellationID = value; }
  }

  private string _description;
  public string Description
  {
    get { return _description; }
    set { _description = value; }
  }
  private float _cancellationFeePercent;
  public float CancellationFeePercent
  {
    get { return _cancellationFeePercent; }
    set { _cancellationFeePercent = value; }
  }
  #endregion


  #region Methods
  public CancellationFee RetrieveCancellationFee()
  {

    CancellationFee cancellationFee = new CancellationFee();
    using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
    {

      using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveCancellationFee, sqlConnection))
      {
        sqlConnection.Open();
        sqlCommand.CommandType = CommandType.StoredProcedure;
        sqlCommand.Parameters.AddWithValue("@cancellationID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(1));
        using (SqlDataReader rd = sqlCommand.ExecuteReader())
        {
          while (rd.Read())
          {
            cancellationFee.CancellationFeePercent = RDFramework.Utility.Conversion.SafeReadDatabaseValue<float>(rd["fld_CancellationFee"]);
            cancellationFee.CancellationID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_CancellationID"]);
            cancellationFee.Description = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_Description"]);
          }
        }
      }
    }
    return cancellationFee;
  }

  public void InsertCCRequestCancellation(string requestReferenceNo)
  {
    using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringWriter))
    {
      sqlConnection.Open();
      using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.InsertCCRequestCancellation, sqlConnection))
      {
        sqlCommand.CommandType = CommandType.StoredProcedure;
        sqlCommand.Parameters.AddWithValue("@requestReferenceNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(requestReferenceNo));
        sqlCommand.Parameters.AddWithValue("@cancellationId", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.CancellationID));
        sqlCommand.ExecuteNonQuery();
      }
    }
  }
  #endregion
}