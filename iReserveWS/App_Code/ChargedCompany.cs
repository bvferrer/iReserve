using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Web;

/// <summary>
/// Summary description for ChargedCompany
/// </summary>
public class ChargedCompany
{
  public ChargedCompany()
  {
    //
    // TODO: Add constructor logic here
    //
  }
  #region Properties
  private int _chargedCompanyID;
  public int ChargedCompanyID { get { return _chargedCompanyID; } set { _chargedCompanyID = value; } }

  private string _companyCode;
  public string CompanyCode { get { return _companyCode; } set { _companyCode = value; } }

  private string _companyName;
  public string CompanyName { get { return _companyName; } set { _companyName = value; } }

  private string _companyAddress;
  public string CompanyAddress { get { return _companyAddress; } set { _companyAddress = value; } }

  private string _companyTelephone;
  public string CompanyTelephone { get { return _companyTelephone; } set { _companyTelephone = value; } }

  private string _companyFax;
  public string CompanyFax { get { return _companyFax; } set { _companyFax = value; } }

  private string _companyTIN;
  public string CompanyTIN { get { return _companyTIN; } set { _companyTIN = value; } }


  private int _enabled;
  public int Enabled { get { return _enabled; } set { _enabled = value; } }
  private string _bankCode;
  public string BankCode { get { return _bankCode; } set { _bankCode = value; } }
  private int _companyTypeID;
  public int CompanyTypeID { get { return _companyTypeID; } set { _companyTypeID = value; } }

  private int _isNAV;
  public int IsNAV { get { return _isNAV; } set { _isNAV = value; } }

  private string _sAPCode;
  public string SAPCode { get { return _sAPCode; } set { _sAPCode = value; } }

  private string _dateSAPEnabled;
  public string DateSAPEnabled { get { return _dateSAPEnabled; } set { _dateSAPEnabled = value; } }

  #endregion

  #region Methods

  public List<ChargedCompany> RetrieveChargedCompany()  
  {
    List<ChargedCompany> requestList = new List<ChargedCompany>();
    using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
    {
      using (SqlCommand sqlCommand = new SqlCommand(Common.usp_RetrieveChargedCompany, sqlConnection))
      {
        sqlConnection.Open();
        sqlCommand.CommandType = CommandType.StoredProcedure;

        using (SqlDataReader rd = sqlCommand.ExecuteReader())
        {
          while (rd.Read())
          {
            ChargedCompany request = new ChargedCompany();
            request.ChargedCompanyID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_ChargedCompanyID"]);
            request.CompanyCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CompanyCode"]);
            request.CompanyName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CompanyName"]);
            request.CompanyAddress = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CompanyAddress"]);
            request.CompanyTelephone = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CompanyTelephone"]);
            request.CompanyFax = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CompanyFax"]);
            request.CompanyTIN = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CompanyTIN"]);
            request.Enabled = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_Enabled"]);
            request.BankCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_BankCode"]);
            request.CompanyTypeID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_CompanyTypeID"]); 
            request.IsNAV = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_IsNAV"]);
            request.SAPCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_SAPCode"]);
            request.DateSAPEnabled = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_DateSAPEnabled"]); 
            requestList.Add(request);
          }
        }
      }
      return requestList;
    }
  }
  #endregion
}