using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Transactions;

/// <summary>
/// Summary description for TrainingRoomTransactionRequest
/// </summary>
public class TrainingRoomTransactionRequest
{
	public TrainingRoomTransactionRequest()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    private int _type;

    public int Type
    {
        get { return _type; }
        set { _type = value; }
    }

    private TrainingRoom _trainingRoom;

    public TrainingRoom TrainingRoom
    {
        get { return _trainingRoom; }
        set { _trainingRoom = value; }
    }

    private AuditTrail _auditTrail;

    public AuditTrail AuditTrail
    {
        get { return _auditTrail; }
        set { _auditTrail = value; }
    }

    private List<TRPartition> _trPartitionList;

    public List<TRPartition> TRPartitionList
    {
        get { return _trPartitionList; }
        set { _trPartitionList = value; }
    }

    private List<TRRate> _trRateList;

    public List<TRRate> TRRateList
    {
        get { return _trRateList; }
        set { _trRateList = value; }
    }

    public TrainingRoomTransactionResult Process()
    {
        TrainingRoomTransactionResult returnValue = new TrainingRoomTransactionResult();

        using (TransactionScope transactionScope = new TransactionScope())
        {
            using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringWriter))
            {
                sqlConnection.Open();

                TrainingRoom trainingRoom = new TrainingRoom();
                trainingRoom.TrainingRoomTransaction(this.Type, this.TrainingRoom, this.AuditTrail, sqlConnection);

                TRPartition trPartition = new TRPartition();
                trPartition.TRPartitionTransaction(this.Type, trainingRoom.TRoomID, this.TRPartitionList, this.AuditTrail, sqlConnection);

                TRRate trRate = new TRRate();
                trRate.TRRateTransaction(this.Type, trainingRoom.TRoomID, this.TRRateList, this.AuditTrail, sqlConnection);
            }

            transactionScope.Complete();
        }

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.TrainingRoomTransactionSuccessful;

        return returnValue;
    }
}