using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;
using System.Web;

/// <summary>
/// Summary description for UpdateCCRequestPercentDiscountRequest
/// </summary>
public class UpdateCCRequestPercentDiscountRequest
{
  public UpdateCCRequestPercentDiscountRequest()
  {
    //
    // TODO: Add constructor logic here
    //
    _ccRequest = new CCRequest();
  }
  private CCRequest _ccRequest;
  public CCRequest CCRequest
  {
    get { return _ccRequest; }
    set { _ccRequest = value; }
  }

  public UpdateCCRequestPercentDiscountResult Process()
  {
    UpdateCCRequestPercentDiscountResult returnValue = new UpdateCCRequestPercentDiscountResult();

    using (TransactionScope transactionScope = new TransactionScope())
    {
      using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringWriter))
      {
        sqlConnection.Open();

        this.CCRequest.UpdateCCRequestPercentDiscount(sqlConnection);
      }

      transactionScope.Complete();
    }

    returnValue.ResultStatus = ResultStatus.Successful;
    returnValue.Message = Messages.UpdateCCRequestPercentDiscountSuccessfull;

    return returnValue;
  }
}