using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for TrainingRoomScheduleMapping
/// </summary>
public class TrainingRoomScheduleMapping
{
  public TrainingRoomScheduleMapping()
  {
    //
    // TODO: Add constructor logic here
    //
  }

  #region Fields/Properties

  private int _mappingID;

  public int MappingID
  {
    get { return _mappingID; }
    set { _mappingID = value; }
  }

  private int _partitionID;

  public int PartitionID
  {
    get { return _partitionID; }
    set { _partitionID = value; }
  }

  private DateTime _date;

  public DateTime Date
  {
    get { return _date; }
    set { _date = value; }
  }

  private string _referenceNumber;

  public string ReferenceNumber
  {
    get { return _referenceNumber; }
    set { _referenceNumber = value; }
  }

  private string _isCancelled;

  public string IsCancelled
  {
    get { return _isCancelled; }
    set { _isCancelled = value; }
  }

  #endregion

  #region Methods

  public void InsertTrainingRoomScheduleMapping(SqlConnection sqlConnection)
  {
    using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.InsertTrainingRoomScheduleMapping, sqlConnection))
    {
      sqlCommand.CommandType = CommandType.StoredProcedure;
      sqlCommand.Parameters.AddWithValue("@partitionID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.PartitionID));
      sqlCommand.Parameters.AddWithValue("@date", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(this.Date));
      sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.ReferenceNumber));
      sqlCommand.ExecuteNonQuery();
    }
  }

  public bool ValidateTrainingRoomScheduleAvailability(int roomID, DateTime startDate, DateTime endDate)
  {
    bool validationStatus = false;

    using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
    {
      using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.ValidateTrainingRoomScheduleAvailability, sqlConnection))
      {
        sqlConnection.Open();
        sqlCommand.CommandType = CommandType.StoredProcedure;
        sqlCommand.Parameters.AddWithValue("@roomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(roomID));
        sqlCommand.Parameters.AddWithValue("@startDate", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(startDate));
        sqlCommand.Parameters.AddWithValue("@endDate", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(endDate));

        using (SqlDataReader rd = sqlCommand.ExecuteReader())
        {
          while (rd.Read())
          {
            validationStatus = RDFramework.Utility.Conversion.SafeReadDatabaseValue<bool>(rd["ValidationStatus"]);
          }
        }
      }
    }

    return validationStatus;
  }

  public void CancelTrainingRoomScheduleMapping(SqlConnection sqlConnection, string ccRequestReferenceNo)
  {
    using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.CancelTrainingRoomScheduleMapping, sqlConnection))
    {
      sqlCommand.CommandType = CommandType.StoredProcedure;
      sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(ccRequestReferenceNo));
      sqlCommand.ExecuteNonQuery();
    }
  }

  public bool ValidateSummaryTrainingRoomScheduleAvailability(List<TrainingRoomRequest> trainingRoomRequestList)
  {
    bool validationStatus = true;

    foreach (TrainingRoomRequest trainingRoomRequest in trainingRoomRequestList)
    {
      using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
      {
        using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.ValidateTrainingRoomScheduleAvailability, sqlConnection))
        {
          sqlConnection.Open();
          sqlCommand.CommandType = CommandType.StoredProcedure;
          sqlCommand.Parameters.AddWithValue("@roomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(trainingRoomRequest.PartitionID));
          sqlCommand.Parameters.AddWithValue("@startDate", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(trainingRoomRequest.StartDate));
          sqlCommand.Parameters.AddWithValue("@endDate", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(trainingRoomRequest.EndDate));

          using (SqlDataReader rd = sqlCommand.ExecuteReader())
          {
            while (rd.Read())
            {
              validationStatus = RDFramework.Utility.Conversion.SafeReadDatabaseValue<bool>(rd["ValidationStatus"]);
            }
          }
        }
      }

      if (validationStatus == false)
      {
        return false;
      }
    }

    return validationStatus;
  }

  public void TranTrainingRoomScheduleMapping(int type, SqlConnection sqlConnection)
  {
    using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.TranTrainingRoomScheduleMapping, sqlConnection))
    {
      sqlCommand.CommandType = CommandType.StoredProcedure;
      sqlCommand.Parameters.AddWithValue("@type", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(type));
      sqlCommand.Parameters.AddWithValue("@partitionID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.PartitionID));
      sqlCommand.Parameters.AddWithValue("@date", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(this.Date));
      sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.ReferenceNumber));
      sqlCommand.ExecuteNonQuery();
    }
  }

  #endregion
}