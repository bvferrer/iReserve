using System;
using System.Collections.Generic;
using System.Web;
using System.Transactions;
using System.Data.SqlClient;

/// <summary>
/// Summary description for InsertNewCCRequestRequest
/// </summary>
public class InsertNewCCRequestRequest
{
	public InsertNewCCRequestRequest()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private CCRequest _ccRequest;

    public CCRequest CCRequest
    {
        get { return _ccRequest; }
        set { _ccRequest = value; }
    }

    private CCRequestAttachment _ccRequestAttachment;

    public CCRequestAttachment CCRequestAttachment
    {
        get { return _ccRequestAttachment; }
        set { _ccRequestAttachment = value; }
    }

    private CCRequestHistory _ccRequestHistory;

    public CCRequestHistory CCRequestHistory
    {
        get { return _ccRequestHistory; }
        set { _ccRequestHistory = value; }
    }

    private List<TrainingRoomRequest> _trainingRoomRequestList;

    public List<TrainingRoomRequest> TrainingRoomRequestList
    {
        get { return _trainingRoomRequestList; }
        set { _trainingRoomRequestList = value; }
    }

    private List<TrainingRoomRequestDetail> _trainingRoomRequestDetailList;

    public List<TrainingRoomRequestDetail> TrainingRoomRequestDetailList
    {
        get { return _trainingRoomRequestDetailList; }
        set { _trainingRoomRequestDetailList = value; }
    }

    private List<AccomodationRoomRequest> _accomodationRoomRequestList;

    public List<AccomodationRoomRequest> AccomodationRoomRequestList
    {
        get { return _accomodationRoomRequestList; }
        set { _accomodationRoomRequestList = value; }
    }

    public InsertNewCCRequestResult Process()
    {
        InsertNewCCRequestResult returnValue = new InsertNewCCRequestResult();

        using (TransactionScope transactionScope = new TransactionScope())
        {
            using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringWriter))
            {
                sqlConnection.Open();

                this.CCRequest.InsertCCRequest(sqlConnection);
                this.CCRequestAttachment.InsertCCRequestAttachment(sqlConnection);
                this.CCRequestHistory.InsertCCRequestHistory(sqlConnection);

                foreach (TrainingRoomRequest trainingRoomRequest in this.TrainingRoomRequestList)
                {
                    trainingRoomRequest.InsertTrainingRoomRequest(sqlConnection);
                }

                foreach (TrainingRoomRequestDetail trainingRoomRequestDetail in this.TrainingRoomRequestDetailList)
                {
                    trainingRoomRequestDetail.InsertTrainingRoomRequestDetail(sqlConnection);
                }

                TrainingRoomRequest trainingRoomRequestCharges = new TrainingRoomRequest();
                trainingRoomRequestCharges.InsertTrainingRoomRequestCharges(sqlConnection, this.CCRequest.CCRequestReferenceNo);

                foreach (AccomodationRoomRequest accomodationRoomRequest in this.AccomodationRoomRequestList)
                {
                    accomodationRoomRequest.InsertAccomodationRoomRequest(sqlConnection);
                }

                EmailNotification emailNotification = new EmailNotification();
                emailNotification.SendCCEmailNotification(this.CCRequest.CCRequestReferenceNo);
            }

            transactionScope.Complete();
        }

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.InsertNewCCRequestSuccessful;

        return returnValue;
    }
}