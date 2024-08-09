using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Web;

/// <summary>
/// Summary description for ChargedCompanyCostCenter
/// </summary>
public class ChargedCompanyCostCenter
{
  public ChargedCompanyCostCenter()
  {
    //
    // TODO: Add constructor logic here
    //
  }
  #region Properties
  private int? _chargedCompanyCostCenterID;
  public int? ChargedCompanyCostCenterID { get { return _chargedCompanyCostCenterID; } set { _chargedCompanyCostCenterID = value; } }

  private int _chargedCompanyID;
  public int ChargedCompanyID { get { return _chargedCompanyID; } set { _chargedCompanyID = value; } }


  private string _costCenterCode;
  public string CostCenterCode { get { return _costCenterCode; } set { _costCenterCode = value; } }


  private string _costCenterName;
  public string CostCenterName { get { return _costCenterName; } set { _costCenterName = value; } }


  private string _SAPCode;
  public string SAPCode { get { return _SAPCode; } set { _SAPCode = value; } }

  private string _createdBy;
  public string CreatedBy { get { return _createdBy; } set { _createdBy = value; } }

  private string _dateCreated;
  public string DateCreated { get { return _dateCreated; } set { _dateCreated = value; } }

  private string _updatedBy;
  public string UpdatedBy { get { return _updatedBy; } set { _updatedBy = value; } }

  private string _deletedBy;
  public string DeletedBy { get { return _deletedBy; } set { _deletedBy = value; } }

  private string _dateDeleted;
  public string DateDeleted { get { return _dateDeleted; } set { _dateDeleted = value; } }

  private string _dateUpdated;
  public string DateUpdated { get { return _dateUpdated; } set { _dateUpdated = value; } }

  #endregion

  #region Methods
  public List<ChargedCompanyCostCenter> RetrieveCostCenterByChargedCompanyId(int companyId)
  {
    List<ChargedCompanyCostCenter> requestList = new List<ChargedCompanyCostCenter>();
    using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
    {
      using (SqlCommand sqlCommand = new SqlCommand(Common.usp_RetrieveCostCenterByChargedCompanyId, sqlConnection))
      {
        sqlConnection.Open();
        sqlCommand.CommandType = CommandType.StoredProcedure;
        sqlCommand.Parameters.AddWithValue("@companyId", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(companyId));
        using (SqlDataReader rd = sqlCommand.ExecuteReader())
        {
          while (rd.Read())
          {
            ChargedCompanyCostCenter request = new ChargedCompanyCostCenter();
            request.ChargedCompanyID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_ChargedCompanyID"]);
            request.ChargedCompanyCostCenterID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_ChargedCompanyCostCenterID"]);
            request.CostCenterCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CostCenterCode"]);
            request.CostCenterName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CostCenterName"]);
            request.SAPCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_SAPCode"]);
            request.CreatedBy = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CreatedBy"]);
            request.DateCreated = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_DateCreated"]);
            request.UpdatedBy = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_UpdatedBy"]);
            request.DeletedBy = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_DeletedBy"]);
            request.DateDeleted = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_DateDeleted"]);
            request.DateUpdated = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_DateUpdated"]);
            requestList.Add(request);
          }
        }
      }
      return requestList;
    }
  }
  #endregion
}