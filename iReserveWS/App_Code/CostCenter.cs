using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for CostCenter
/// </summary>
public class CostCenter
{
  public CostCenter()
  {
    //
    // TODO: Add constructor logic here
    //
  }

  private int _costCenterID;

  public int CostCenterID
  {
    get { return _costCenterID; }
    set { _costCenterID = value; }
  }

  private string _costCenterName;

  public string CostCenterName
  {
    get { return _costCenterName; }
    set { _costCenterName = value; }
  }

  private string _ChargedCompanyID;
  public string ChargedCompanyID
  {
    get { return _ChargedCompanyID; }
    set { _ChargedCompanyID = value; }
  }
}