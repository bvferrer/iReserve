using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// Summary description for AccomodationRoomScheduleMappingTransactionRequest
/// </summary>
public class AccomodationRoomScheduleMappingTransactionRequest
{
    public AccomodationRoomScheduleMappingTransactionRequest()
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

    private int _accRoomID;

    public int AccRoomID
    {
        get { return _accRoomID; }
        set { _accRoomID = value; }
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

    private string _remarks;

    public string Remarks
    {
        get { return _remarks; }
        set { _remarks = value; }
    }

    public AccomodationRoomScheduleMappingTransactionResult Process()
    {
        AccomodationRoomScheduleMappingTransactionResult returnValue = new AccomodationRoomScheduleMappingTransactionResult();

        AccomodationRoomScheduleMapping accomodationRoomScheduleMapping = new AccomodationRoomScheduleMapping();
        accomodationRoomScheduleMapping.AccRoomID = this.AccRoomID;
        accomodationRoomScheduleMapping.ReferenceNumber = this.Remarks;

        DateTime date = this.StartDate;
        DateTime endDate = this.EndDate;

        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringWriter))
        {
            sqlConnection.Open();

            while (date <= endDate)
            {
                accomodationRoomScheduleMapping.Date = date;
                accomodationRoomScheduleMapping.TranAccomodationRoomScheduleMapping(this.Type, sqlConnection);

                date = date.AddDays(1);
            }
        }

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.AccomodationRoomScheduleMappingTransactionSuccessful;

        return returnValue;
    }
}