using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for TrainingRoomRequestCharge
/// </summary>
public class TrainingRoomRequestCharge
{
	public TrainingRoomRequestCharge()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    #region Properties

    private int _trChargeID;

    public int TRChargeID
    {
        get { return _trChargeID; }
        set { _trChargeID = value; }
    }

    private string _ccRequestReferenceNo;

    public string CCRequestReferenceNo
    {
        get { return _ccRequestReferenceNo; }
        set { _ccRequestReferenceNo = value; }
    }

    private int _tRoomID;

    public int TRoomID
    {
        get { return _tRoomID; }
        set { _tRoomID = value; }
    }

    private string _tRoomCode;

    public string TRoomCode
    {
        get { return _tRoomCode; }
        set { _tRoomCode = value; }
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

    private DateTime _date;

    public DateTime Date
    {
        get { return _date; }
        set { _date = value; }
    }

    private DateTime _startDateTime;

    public DateTime StartDateTime
    {
        get { return _startDateTime; }
        set { _startDateTime = value; }
    }

    private DateTime _endDateTime;

    public DateTime EndDateTime
    {
        get { return _endDateTime; }
        set { _endDateTime = value; }
    }

    private int _numberOfHours;

    public int NumberOfHours
    {
        get { return _numberOfHours; }
        set { _numberOfHours = value; }
    }

    private string _ratePerDay;

    public string RatePerDay
    {
        get { return _ratePerDay; }
        set { _ratePerDay = value; }
    }

    private string _extensionRatePerHour;

    public string ExtensionRatePerHour
    {
        get { return _extensionRatePerHour; }
        set { _extensionRatePerHour = value; }
    }

    private string _amountCharge;

    public string AmountCharge
    {
        get { return _amountCharge; }
        set { _amountCharge = value; }
    }

    #endregion

    #region Methods

    public List<TrainingRoomRequestCharge> RetrieveTrainingRoomRequestCharges(string ccRequestReferenceNo)
    {
        List<TrainingRoomRequestCharge> trainingRoomRequestChargeList = new List<TrainingRoomRequestCharge>();

        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveTrainingRoomRequestCharges, sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(ccRequestReferenceNo));

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        TrainingRoomRequestCharge trainingRoomRequestCharge = new TrainingRoomRequestCharge();
                        trainingRoomRequestCharge.TRChargeID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_TRChargeID"]);
                        trainingRoomRequestCharge.CCRequestReferenceNo = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CCRequestReferenceNo"]);
                        trainingRoomRequestCharge.TRoomID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_TRoomID"]);
                        trainingRoomRequestCharge.TRoomCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_TRoomCode"]);
                        trainingRoomRequestCharge.TRoomName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_TRoomName"]);
                        trainingRoomRequestCharge.NumberOfPartition = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_NumberOfPartition"]);
                        trainingRoomRequestCharge.Date = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_Date"]);
                        trainingRoomRequestCharge.StartDateTime = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_StartDateTime"]);
                        trainingRoomRequestCharge.EndDateTime = RDFramework.Utility.Conversion.SafeReadDatabaseValue<DateTime>(rd["fld_EndDateTime"]);
                        trainingRoomRequestCharge.NumberOfHours = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_NumberOfHours"]);
                        trainingRoomRequestCharge.RatePerDay = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RatePerDay"]);
                        trainingRoomRequestCharge.ExtensionRatePerHour = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_ExtensionRatePerHour"]);
                        trainingRoomRequestCharge.AmountCharge = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_AmountCharge"]);
                        trainingRoomRequestChargeList.Add(trainingRoomRequestCharge);
                    }
                }
            }
        }

        return trainingRoomRequestChargeList;
    }

    #endregion
}