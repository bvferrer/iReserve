using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for CRReport
/// </summary>
public class CRReport
{
	public CRReport()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    #region Fields/Properties

    private Status _status;

    public Status Status
    {
        get { return _status; }
        set { _status = value; }
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

    #endregion

    #region Methods

    public List<CRRequest> RetrieveCRRequestRecordsReport()
    {
        List<CRRequest> requestList = new List<CRRequest>();

        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            using (SqlCommand sqlCommand = new SqlCommand(Common.usp_RetrieveCRRequestRecordsReport, sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@statusCode", this.Status.StatusCode);
                sqlCommand.Parameters.AddWithValue("@startDate", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(this.StartDate));
                sqlCommand.Parameters.AddWithValue("@endDate", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(this.EndDate));

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        CRRequest request = new CRRequest();
                        request.RequestReferenceNo = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RequestReferenceNo"]);
                        request.ConferenceRoom.RoomID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_RoomID"]);
                        request.ConferenceRoom.RoomName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RoomName"]);
                        request.Date = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_Date"]);
                        request.StartTime.TimeSlotID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StartTimeID"]);
                        request.StartTime.StartTime = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StartTime"]);
                        request.StartTime.StartTime12 = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StartTime12"]);
                        request.EndTime.TimeSlotID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_EndTimeID"]);
                        request.EndTime.EndTime = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_EndTime"]);
                        request.EndTime.EndTime12 = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_EndTime12"]);
                        request.NumberOfHours = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_NumberofHours"]);
                        request.NumberOfHoursExtended = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_NumberOfHoursExtended"]);
                        request.Agenda = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_Agenda"]);
                        request.HeadCount = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_HeadCount"]);
                        request.IsUseDataPort = RDFramework.Utility.Conversion.SafeReadDatabaseValue<bool>(rd["fld_IsUseDataPort"]).ToString();
                        request.IsUseMonitor = RDFramework.Utility.Conversion.SafeReadDatabaseValue<bool>(rd["fld_IsUseMonitor"]).ToString();
                        request.CostCenter.CostCenterID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_CostCenterID"]);
                        request.CostCenter.CostCenterName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CostCenterName"]);
                        request.Status.StatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StatusCode"]);
                        request.Status.StatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StatusName"]);
                        request.HasLoggedIn = RDFramework.Utility.Conversion.SafeReadDatabaseValue<bool>(rd["fld_HasLoggedIn"]).ToString();
                        request.RequestedByID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RequestedByID"]);
                        request.RequestedBy = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RequestedBy"]);
                        request.DateRequested = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateRequested"]);
                        request.DateConfirmed = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateConfirmed"]);
                        request.DateDeclined = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateDeclined"]);
                        request.DateCancelled = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateCancelled"]);
                        request.DateLoggedIn = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateLoggedIn"]);
                        requestList.Add(request);
                    }
                }
            }
        }

        return requestList;
    }

    public List<CRRequest> RetrieveCRRequestRecordsReportByDate()
    {
        List<CRRequest> requestList = new List<CRRequest>();

        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            using (SqlCommand sqlCommand = new SqlCommand(Common.usp_RetrieveCRRequestRecordsReportByDate, sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@startDate", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(this.StartDate));
                sqlCommand.Parameters.AddWithValue("@endDate", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(this.EndDate));

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        CRRequest request = new CRRequest();
                        request.RequestReferenceNo = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RequestReferenceNo"]);
                        request.ConferenceRoom.RoomID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_RoomID"]);
                        request.ConferenceRoom.RoomName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RoomName"]);
                        request.Date = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_Date"]);
                        request.StartTime.TimeSlotID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StartTimeID"]);
                        request.StartTime.StartTime = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StartTime"]);
                        request.StartTime.StartTime12 = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StartTime12"]);
                        request.EndTime.TimeSlotID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_EndTimeID"]);
                        request.EndTime.EndTime = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_EndTime"]);
                        request.EndTime.EndTime12 = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_EndTime12"]);
                        request.NumberOfHours = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_NumberofHours"]);
                        request.NumberOfHoursExtended = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_NumberOfHoursExtended"]);
                        request.Agenda = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_Agenda"]);
                        request.HeadCount = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_HeadCount"]);
                        request.IsUseDataPort = RDFramework.Utility.Conversion.SafeReadDatabaseValue<bool>(rd["fld_IsUseDataPort"]).ToString();
                        request.IsUseMonitor = RDFramework.Utility.Conversion.SafeReadDatabaseValue<bool>(rd["fld_IsUseMonitor"]).ToString();
                        request.CostCenter.CostCenterID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_CostCenterID"]);
                        request.CostCenter.CostCenterName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CostCenterName"]);
                        request.Status.StatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StatusCode"]);
                        request.Status.StatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StatusName"]);
                        request.HasLoggedIn = RDFramework.Utility.Conversion.SafeReadDatabaseValue<bool>(rd["fld_HasLoggedIn"]).ToString();
                        request.RequestedByID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RequestedByID"]);
                        request.RequestedBy = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RequestedBy"]);
                        request.DateRequested = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateRequested"]);
                        request.DateConfirmed = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateConfirmed"]);
                        request.DateDeclined = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateDeclined"]);
                        request.DateCancelled = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateCancelled"]);
                        request.DateLoggedIn = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_DateLoggedIn"]);
                        requestList.Add(request);
                    }
                }
            }
        }

        return requestList;
    }

    #endregion
}