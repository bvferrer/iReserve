using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for AccomodationRoomRequest
/// </summary>
public class AccomodationRoomRequest
{
    public AccomodationRoomRequest()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Properties

    private int _accRequestID;

    public int AccRequestID
    {
        get { return _accRequestID; }
        set { _accRequestID = value; }
    }

    private string _ccRequestReferenceNo;

    public string CCRequestReferenceNo
    {
        get { return _ccRequestReferenceNo; }
        set { _ccRequestReferenceNo = value; }
    }

    private int _accID;

    public int AccID
    {
        get { return _accID; }
        set { _accID = value; }
    }

    private string _roomName;

    public string RoomName
    {
        get { return _roomName; }
        set { _roomName = value; }
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

    private int _numberOfNights;

    public int NumberOfNights
    {
        get { return _numberOfNights; }
        set { _numberOfNights = value; }
    }

    private int _headCount;

    public int HeadCount
    {
        get { return _headCount; }
        set { _headCount = value; }
    }

    private string _remarks;

    public string Remarks
    {
        get { return _remarks; }
        set { _remarks = value; }
    }

    private string _ratePerNight;

    public string RatePerNight
    {
        get { return _ratePerNight; }
        set { _ratePerNight = value; }
    }

    private string _amountCharge;

    public string AmountCharge
    {
        get { return _amountCharge; }
        set { _amountCharge = value; }
    }

    #endregion

    #region Methods

    public void InsertAccomodationRoomRequest(SqlConnection sqlConnection)
    {
        using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.InsertAccomodationRoomRequest, sqlConnection))
        {
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.CCRequestReferenceNo));
            sqlCommand.Parameters.AddWithValue("@accRoomID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.AccID));
            sqlCommand.Parameters.AddWithValue("@startDate", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(this.StartDate));
            sqlCommand.Parameters.AddWithValue("@endDate", RDFramework.Utility.Conversion.SafeSetDatabaseValue<DateTime>(this.EndDate));
            sqlCommand.Parameters.AddWithValue("@headCount", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.HeadCount));
            sqlCommand.Parameters.AddWithValue("@remarks", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.Remarks));
            sqlCommand.ExecuteNonQuery();
        }

        AccomodationRoomScheduleMapping accomodationRoomScheduleMapping = new AccomodationRoomScheduleMapping();
        accomodationRoomScheduleMapping.AccRoomID = this.AccID;
        accomodationRoomScheduleMapping.ReferenceNumber = this.CCRequestReferenceNo;

        DateTime date = this.StartDate;
        DateTime endDate = this.EndDate;

        while (date <= endDate)
        {
            accomodationRoomScheduleMapping.Date = date;
            accomodationRoomScheduleMapping.InsertAccomodationRoomScheduleMapping(sqlConnection);

            date = date.AddDays(1);
        }
    }

    public List<AccomodationRoomRequest> RetrieveAccomodationRoomRequestRecords(string ccRequestReferenceNo)
    {
        List<AccomodationRoomRequest> accomodationRoomRequestList = new List<AccomodationRoomRequest>();

        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveAccomodationRoomRequest, sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(ccRequestReferenceNo));

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        AccomodationRoomRequest accomodationRoomRequest = new AccomodationRoomRequest();
                        accomodationRoomRequest.AccRequestID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_AccRoomRequestID"]);
                        accomodationRoomRequest.CCRequestReferenceNo = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CCRequestReferenceNo"]);
                        accomodationRoomRequest.AccID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_AccRoomID"]);
                        accomodationRoomRequest.RoomName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RoomName"]);
                        accomodationRoomRequest.StartDate = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_StartDate"]);
                        accomodationRoomRequest.EndDate = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_EndDate"]);
                        accomodationRoomRequest.NumberOfNights = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_NumberOfNights"]);
                        accomodationRoomRequest.HeadCount = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_HeadCount"]);
                        accomodationRoomRequest.Remarks = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_Remarks"]);
                        accomodationRoomRequest.RatePerNight = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RatePerNight"]);
                        accomodationRoomRequest.AmountCharge = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_AmountCharge"]);
                        accomodationRoomRequestList.Add(accomodationRoomRequest);
                    }
                }
            }
        }

        return accomodationRoomRequestList;
    }

    #endregion
}