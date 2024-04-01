using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for TrainingRoomRequest
/// </summary>
public class TrainingRoomRequest
{
    public TrainingRoomRequest()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Properties

    private int _trRequestID;

    public int TRRequestID
    {
        get { return _trRequestID; }
        set { _trRequestID = value; }
    }

    private string _ccRequestReferenceNo;

    public string CCRequestReferenceNo
    {
        get { return _ccRequestReferenceNo; }
        set { _ccRequestReferenceNo = value; }
    }

    private int _partitionID;

    public int PartitionID
    {
        get { return _partitionID; }
        set { _partitionID = value; }
    }

    private string _partitionName;

    public string PartitionName
    {
        get { return _partitionName; }
        set { _partitionName = value; }
    }

    private DateTime _startDate;

    public DateTime StartDate
    {
        get { return _startDate; }
        set { _startDate = value; }
    }

    private DateTime _endDate;

    public DateTime EndDate
    {
        get { return _endDate; }
        set { _endDate = value; }
    }

    private int _numberOfDays;

    public int NumberOfDays
    {
        get { return _numberOfDays; }
        set { _numberOfDays = value; }
    }

    private int _headCount;

    public int HeadCount
    {
        get { return _headCount; }
        set { _headCount = value; }
    }

    private string _equipmentToUse;

    public string EquipmentToUse
    {
        get { return _equipmentToUse; }
        set { _equipmentToUse = value; }
    }

    private string _remarks;

    public string Remarks
    {
        get { return _remarks; }
        set { _remarks = value; }
    }

    #endregion

    #region Methods

    public void InsertTrainingRoomRequest(SqlConnection sqlConnection)
    {
        using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.InsertTrainingRoomRequest, sqlConnection))
        {
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.CCRequestReferenceNo));
            sqlCommand.Parameters.AddWithValue("@partitionID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.PartitionID));
            sqlCommand.Parameters.AddWithValue("@startDate", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(this.StartDate));
            sqlCommand.Parameters.AddWithValue("@endDate", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(this.EndDate));
            sqlCommand.Parameters.AddWithValue("@headCount", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.HeadCount));
            sqlCommand.Parameters.AddWithValue("@equipmentToUse", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.EquipmentToUse));
            sqlCommand.Parameters.AddWithValue("@remarks", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.Remarks));
            sqlCommand.ExecuteNonQuery();
        }

        TrainingRoomScheduleMapping trainingRoomScheduleMapping = new TrainingRoomScheduleMapping();
        trainingRoomScheduleMapping.PartitionID = this.PartitionID;
        trainingRoomScheduleMapping.ReferenceNumber = this.CCRequestReferenceNo;

        DateTime date = this.StartDate;
        DateTime endDate = this.EndDate;

        while (date <= endDate)
        {
            trainingRoomScheduleMapping.Date = date;
            trainingRoomScheduleMapping.InsertTrainingRoomScheduleMapping(sqlConnection);

            date = date.AddDays(1);
        }
    }

    public void InsertTrainingRoomRequestCharges(SqlConnection sqlConnection, string ccRequestReferenceNo)
    {
        using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.InsertTrainingRoomRequestCharges, sqlConnection))
        {
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(ccRequestReferenceNo));
            sqlCommand.ExecuteNonQuery();
        }
    }

    public List<TrainingRoomRequest> RetrieveTrainingRoomRequestRecords(string ccRequestReferenceNo)
    {
        List<TrainingRoomRequest> trainingRoomRequestList = new List<TrainingRoomRequest>();

        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveTrainingRoomRequest, sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(ccRequestReferenceNo));

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        TrainingRoomRequest trainingRoomRequest = new TrainingRoomRequest();
                        trainingRoomRequest.TRRequestID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_TRRequestID"]);
                        trainingRoomRequest.CCRequestReferenceNo = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CCRequestReferenceNo"]);
                        trainingRoomRequest.PartitionID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_PartitionID"]);
                        trainingRoomRequest.PartitionName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_PartitionName"]);
                        trainingRoomRequest.StartDate = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_StartDate"]);
                        trainingRoomRequest.EndDate = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_EndDate"]);
                        trainingRoomRequest.NumberOfDays = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_NumberOfDays"]);
                        trainingRoomRequest.HeadCount = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_HeadCount"]);
                        trainingRoomRequest.EquipmentToUse = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_EquipmentToUse"]);
                        trainingRoomRequest.Remarks = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_Remarks"]);
                        trainingRoomRequestList.Add(trainingRoomRequest);
                    }
                }
            }
        }

        return trainingRoomRequestList;
    }

    #endregion
}