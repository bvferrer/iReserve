using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for TrainingRoomRequestReport
/// </summary>
public class TrainingRoomRequestReport
{
	public TrainingRoomRequestReport()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    #region Properties

    private string _ccRequestReferenceNo;

    public string CCRequestReferenceNo
    {
        get { return _ccRequestReferenceNo; }
        set { _ccRequestReferenceNo = value; }
    }

    private string _eventName;

    public string EventName
    {
        get { return _eventName; }
        set { _eventName = value; }
    }

    private string _tRoomName;

    public string TRoomName
    {
        get { return _tRoomName; }
        set { _tRoomName = value; }
    }

    private int _numberOfPartition;

    public int NumberOfPartition
    {
        get { return _numberOfPartition; }
        set { _numberOfPartition = value; }
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

    private string _statusName;

    public string StatusName
    {
        get { return _statusName; }
        set { _statusName = value; }
    }

    private string _costCenterName;

    public string CostCenterName
    {
        get { return _costCenterName; }
        set { _costCenterName = value; }
    }

    private DateTime _dateCreated;

    public DateTime DateCreated
    {
        get { return _dateCreated; }
        set { _dateCreated = value; }
    }

    private DateTime _dateCancelled;

    public DateTime DateCancelled
    {
        get { return _dateCancelled; }
        set { _dateCancelled = value; }
    }

    #endregion

    #region Methods

    public List<TrainingRoomRequestReport> RetrieveTrainingRoomRequestReport(string selectedStatus, DateTime startDate, DateTime endDate)
    {
        List<TrainingRoomRequestReport> trainingRoomRequestReportList = new List<TrainingRoomRequestReport>();

        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveTrainingRoomRequestReport, sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@selectedStatus", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(selectedStatus));
                sqlCommand.Parameters.AddWithValue("@startDate", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(startDate));
                sqlCommand.Parameters.AddWithValue("@endDate", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(endDate));

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        TrainingRoomRequestReport trainingRoomRequestReport = new TrainingRoomRequestReport();
                        trainingRoomRequestReport.CCRequestReferenceNo = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CCRequestReferenceNo"]);
                        trainingRoomRequestReport.EventName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_EventName"]);
                        trainingRoomRequestReport.TRoomName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_TRoomName"]);
                        trainingRoomRequestReport.NumberOfPartition = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_NumberOfPartition"]);
                        trainingRoomRequestReport.StartDate = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_StartDate"]);
                        trainingRoomRequestReport.EndDate = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_EndDate"]);
                        trainingRoomRequestReport.NumberOfDays = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_NumberOfDays"]);
                        trainingRoomRequestReport.StatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StatusName"]);
                        trainingRoomRequestReport.CostCenterName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CostCenterName"]);
                        trainingRoomRequestReport.DateCreated = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateCreated"]);
                        trainingRoomRequestReport.DateCancelled = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateCancelled"]);
                        trainingRoomRequestReportList.Add(trainingRoomRequestReport);
                    }
                }
            }
        }

        return trainingRoomRequestReportList;
    }

    #endregion
}