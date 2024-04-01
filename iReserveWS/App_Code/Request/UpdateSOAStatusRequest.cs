using System;
using System.Collections.Generic;
using System.Web;
using System.Transactions;
using System.Data.SqlClient;

/// <summary>
/// Summary description for UpdateSOAStatusRequest
/// </summary>
public class UpdateSOAStatusRequest
{
    public UpdateSOAStatusRequest()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    private string _ccRequestReferenceNo;

    public string CCRequestReferenceNo
    {
        get { return _ccRequestReferenceNo; }
        set { _ccRequestReferenceNo = value; }
    }

    private int _soaStatusCode;

    public int SOAStatusCode
    {
        get { return _soaStatusCode; }
        set { _soaStatusCode = value; }
    }

    private SOAHistory _soaHistory;

    public SOAHistory SOAHistory
    {
        get { return _soaHistory; }
        set { _soaHistory = value; }
    }

    public UpdateSOAStatusResult Process()
    {
        UpdateSOAStatusResult returnValue = new UpdateSOAStatusResult();

        using (TransactionScope transactionScope = new TransactionScope())
        {
            using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringWriter))
            {
                sqlConnection.Open();

                CCRequest request = new CCRequest();
                request.UpdateCCRequestSOAStatus(sqlConnection, this.CCRequestReferenceNo, this.SOAStatusCode);
                this.SOAHistory.InsertSOAHistory(sqlConnection);

                EmailNotification emailNotification = new EmailNotification();

                if (this.SOAStatusCode == 3)
                {
                    emailNotification.SendSOADetails(this.CCRequestReferenceNo);
                }
                else
                {
                    emailNotification.SendSOAEmailNotification(this.CCRequestReferenceNo);
                }
            }

            transactionScope.Complete();
        }

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.UpdateSOAStatusSuccessful;

        return returnValue;
    }
}