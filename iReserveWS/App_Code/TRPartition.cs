using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.Serialization;

/// <summary>
/// Summary description for TRPartition
/// </summary>
public class TRPartition
{
    #region Constructor
    public TRPartition()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region Fields/Properties

    private int _partitionID;

    public int PartitionID
    {
        get { return _partitionID; }
        set { _partitionID = value; }
    }

    private string _partitionCode;

    public string PartitionCode
    {
        get { return _partitionCode; }
        set { _partitionCode = value; }
    }

    private string _partitionName;

    public string PartitionName
    {
        get { return _partitionName; }
        set { _partitionName = value; }
    }

    private string _partitionDesc;

    public string PartitionDesc
    {
        get { return _partitionDesc; }
        set { _partitionDesc = value; }
    }

    private int _maxPerson;

    public int MaxPerson
    {
        get { return _maxPerson; }
        set { _maxPerson = value; }
    }

    private int _tRoomID;

    public int TRoomID
    {
        get { return _tRoomID; }
        set { _tRoomID = value; }
    }

    private string _tRoomName;

    public string TRoomName
    {
        get { return _tRoomName; }
        set { _tRoomName = value; }
    }

    private bool _isDeleted;

    public bool IsDeleted
    {
        get { return _isDeleted; }
        set { _isDeleted = value; }
    }

    #endregion

    #region Methods

    public List<TRPartition> RetrieveTRPartitionRecords(int tRoomID)
    {
        List<TRPartition> trPartitionList = new List<TRPartition>();

        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            sqlConnection.Open();
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveTRPartitionRecords, sqlConnection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@TRoomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(tRoomID));

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        TRPartition trPartition = new TRPartition();
                        trPartition.PartitionID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_PartitionID"]);
                        trPartition.PartitionCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_PartitionCode"]);
                        trPartition.PartitionName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_PartitionName"]);
                        trPartition.PartitionDesc = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_PartitionDesc"]);
                        trPartition.MaxPerson = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_MaxPerson"]);
                        trPartition.TRoomID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_TRoomID"]);
                        trPartition.TRoomName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_TRoomName"]);
                        trPartitionList.Add(trPartition);
                    }
                }
            }
        }

        return trPartitionList;
    }

    public TrainingRoom RetrieveTRPartitionRecordDetails(int partitionID)
    {
        TrainingRoom trainingRoom = new TrainingRoom();

        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            sqlConnection.Open();
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveTRPartitionRecordDetails, sqlConnection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@TRPartitionID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(partitionID));

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        this.PartitionID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_PartitionID"]);
                        this.PartitionCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_PartitionCode"]);
                        this.PartitionName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_PartitionName"]);
                        this.PartitionDesc = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_PartitionDesc"]);
                        this.MaxPerson = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_MaxPerson"]);
                        this.TRoomID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_TRoomID"]);
                        this.TRoomName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_TRoomName"]);
                        trainingRoom.TRoomID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_TRoomID"]);
                        trainingRoom.TRoomCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_TRoomCode"]);
                        trainingRoom.TRoomName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_TRoomName"]);
                        trainingRoom.TRoomDesc = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_TRoomDesc"]);
                        trainingRoom.LocationID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_LocationID"]);
                        trainingRoom.LocationName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_LocationName"]);
                        trainingRoom.NumberOfPartition = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_NumberOfPartition"]);
                    }
                }
            }
        }

        return trainingRoom;
    }

    public void TRPartitionTransaction(int type, int tRoomID, List<TRPartition> trPartitionList, AuditTrail auditTrailDetails, SqlConnection sqlConnection)
    {
        if (type == 1)
        {
            foreach (TRPartition trPartition in trPartitionList)
            {
                using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.TranTRPartition, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Type", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(type));
                    sqlCommand.Parameters.AddWithValue("@UserID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(auditTrailDetails.UserID));
                    sqlCommand.Parameters.AddWithValue("@PartitionID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(trPartition.PartitionID));
                    sqlCommand.Parameters.AddWithValue("@PartitionCode", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(trPartition.PartitionCode));
                    sqlCommand.Parameters.AddWithValue("@PartitionName", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(trPartition.PartitionName));
                    sqlCommand.Parameters.AddWithValue("@PartitionDesc", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(trPartition.PartitionDesc));
                    sqlCommand.Parameters.AddWithValue("@MaxPerson", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(trPartition.MaxPerson));
                    sqlCommand.Parameters.AddWithValue("@TRoomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(tRoomID));
                    sqlCommand.Parameters.AddWithValue("@IsDeleted", trPartition.IsDeleted);
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
        else if (type == 2)
        {
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.TranTRPartition, sqlConnection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Type", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(3));
                sqlCommand.Parameters.AddWithValue("@UserID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(auditTrailDetails.UserID));
                sqlCommand.Parameters.AddWithValue("@TRoomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(tRoomID)); ;
                sqlCommand.ExecuteNonQuery();
            }

            foreach (TRPartition trPartition in trPartitionList)
            {
                using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.TranTRPartition, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Type", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(type));
                    sqlCommand.Parameters.AddWithValue("@UserID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(auditTrailDetails.UserID));
                    sqlCommand.Parameters.AddWithValue("@PartitionID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(trPartition.PartitionID));
                    sqlCommand.Parameters.AddWithValue("@PartitionCode", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(trPartition.PartitionCode));
                    sqlCommand.Parameters.AddWithValue("@PartitionName", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(trPartition.PartitionName));
                    sqlCommand.Parameters.AddWithValue("@PartitionDesc", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(trPartition.PartitionDesc));
                    sqlCommand.Parameters.AddWithValue("@MaxPerson", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(trPartition.MaxPerson));
                    sqlCommand.Parameters.AddWithValue("@TRoomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(tRoomID));
                    sqlCommand.Parameters.AddWithValue("@IsDeleted", trPartition.IsDeleted);
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
        else
        {
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.TranTRPartition, sqlConnection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Type", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(type));
                sqlCommand.Parameters.AddWithValue("@UserID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(auditTrailDetails.UserID));
                sqlCommand.Parameters.AddWithValue("@TRoomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(tRoomID)); ;
                sqlCommand.ExecuteNonQuery();
            }
        }
    }

    #endregion
}