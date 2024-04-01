using System;
using System.Collections.Generic;
using System.Web;
using System.Transactions;
using System.Data.SqlClient;

/// <summary>
/// Summary description for CancelCCRequestRequest
/// </summary>
public class CancelCCRequestRequest
{
	public CancelCCRequestRequest()
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

    private int _statusCode;

    public int StatusCode
    {
        get { return _statusCode; }
        set { _statusCode = value; }
    }

    private CCRequestHistory _ccRequestHistory;

    public CCRequestHistory CCRequestHistory
    {
        get { return _ccRequestHistory; }
        set { _ccRequestHistory = value; }
    }

    public CancelCCRequestResult Process()
    {
        CancelCCRequestResult returnValue = new CancelCCRequestResult();

        using (TransactionScope transactionScope = new TransactionScope())
        {
            using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringWriter))
            {
                sqlConnection.Open();

                CCRequest request = new CCRequest();
                request.UpdateCCRequestStatus(sqlConnection, this.CCRequestReferenceNo, this.StatusCode);
                this.CCRequestHistory.InsertCCRequestHistory(sqlConnection);

                TrainingRoomScheduleMapping trainingRoomScheduleMapping = new TrainingRoomScheduleMapping();
                trainingRoomScheduleMapping.CancelTrainingRoomScheduleMapping(sqlConnection, this.CCRequestReferenceNo);

                AccomodationRoomScheduleMapping accomodationRoomScheduleMapping = new AccomodationRoomScheduleMapping();
                accomodationRoomScheduleMapping.CancelAccomodationRoomScheduleMapping(sqlConnection, this.CCRequestReferenceNo);

                EmailNotification emailNotification = new EmailNotification();
                emailNotification.SendCCEmailNotification(this.CCRequestReferenceNo);
            }

            transactionScope.Complete();
        }

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.CancelCCRequestSuccessful;

        return returnValue;
    }
}