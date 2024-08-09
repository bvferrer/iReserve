using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Drawing.Imaging;
using AppCryptor;
using AESCryptor;

/// <summary>
/// Summary description for DAL
/// </summary>
public class DAL
{

  #region Constructor
  public DAL()
  {
    //
    // TODO: Add constructor logic here
    //
  }
  #endregion

  #region ChangePassword

  public PasswordRecoveryInfo ChangePassword(string UserID)
  {
    using (SqlConnection conStr = new SqlConnection(Settings.AACFConnectionString))
    {
      string sql = "SELECT [fld_Password], [fld_Email], DATEDIFF(day, GetDate(), [fld_PassExpDate]) AS PassExpire " +
                              "FROM [dbo].[tbl_Users] Where [fld_UserID] = @userID";
      SqlCommand cmd = new SqlCommand(sql, conStr);
      cmd.Parameters.Add("@userID", SqlDbType.VarChar).Value = UserID;
      conStr.Open();
      using (SqlDataReader reader = cmd.ExecuteReader())
      {
        PasswordRecoveryInfo passChange = new PasswordRecoveryInfo();
        reader.Read();

        passChange.Password = AESCrypt.EncryptDecrypt(reader["fld_Password"].ToString(), "Decrypt");
        passChange.Email = reader["fld_Email"].ToString();
        return passChange;
      }
    }
  }
  #endregion

  #region UserPasswordRecovery

  public PasswordRecoveryInfo UserPasswordRecovery(string UserID)
  {
    using (SqlConnection conStr = new SqlConnection(Settings.AACFConnectionString))
    {
      string sql = "SELECT [fld_Password], [fld_Email], [fld_Question1], [fld_Question2], [fld_Question3], " +
                              "[fld_Answer1], [fld_Answer2], [fld_Answer3] FROM [dbo].[tbl_Users] WHERE [fld_UserID] = @userID";

      SqlCommand cmd = new SqlCommand(sql, conStr);
      cmd.Parameters.Add("@userID", SqlDbType.VarChar).Value = UserID;
      conStr.Open();
      using (SqlDataReader reader = cmd.ExecuteReader())
      {
        PasswordRecoveryInfo recoverPassword = new PasswordRecoveryInfo();

        if (reader.Read())
        {
          recoverPassword.Question1 = reader["fld_Question1"].ToString();
          recoverPassword.Question2 = reader["fld_Question2"].ToString();
          recoverPassword.Question3 = reader["fld_Question3"].ToString();
          recoverPassword.Email = reader["fld_Email"].ToString();
          recoverPassword.Password = AESCrypt.EncryptDecrypt(reader["fld_Password"].ToString(), "Decrypt");
          recoverPassword.Answer1 = AESCrypt.EncryptDecrypt(reader["fld_Answer1"].ToString(), "Decrypt");
          recoverPassword.Answer2 = AESCrypt.EncryptDecrypt(reader["fld_Answer2"].ToString(), "Decrypt");
          recoverPassword.Answer3 = AESCrypt.EncryptDecrypt(reader["fld_Answer3"].ToString(), "Decrypt");
        }
        reader.Close();
        return recoverPassword;
      }
    }
  }

  #endregion

  #region FILE MAINTENANCE - LOCATION

  #region RetrieveLocationRecords
  public List<MaintenanceLocationList> RetrieveLocationRecords(string LocationCode, string LocationName)
  {
    try
    {
      using (SqlConnection connection = new SqlConnection(Settings.iReserveConnectionStringReader))
      {
        using (SqlCommand cmd = new SqlCommand(Common.usp_RetrieveLocationRecords, connection))
        {
          cmd.CommandType = CommandType.StoredProcedure;
          List<MaintenanceLocationList> locationList = new List<MaintenanceLocationList>();

          connection.Open();
          cmd.Parameters.AddWithValue("@LocationCode", LocationCode);
          cmd.Parameters.AddWithValue("@LocationName", LocationName);
          SqlDataReader reader = cmd.ExecuteReader();

          while (reader.Read())
          {
            MaintenanceLocationList locationRecord = new MaintenanceLocationList();
            locationRecord.LocationID = Convert.ToInt32(reader["fld_LocationID"]);
            locationRecord.LocationCode = reader["fld_LocationCode"].ToString();
            locationRecord.LocationName = reader["fld_LocationName"].ToString();
            locationRecord.LocationDesc = reader["fld_LocationDesc"].ToString();
            locationList.Add(locationRecord);
          }

          return locationList;
        }
      }
    }
    catch (Exception error)
    {
      throw error;
    }
  }
  #endregion

  #region RetrieveLocationRecordDetails
  public MaintenanceLocationList RetrieveLocationRecordDetails(int LocationID)
  {
    using (SqlConnection connection = new SqlConnection(Settings.iReserveConnectionStringReader))
    {
      using (SqlCommand cmd = new SqlCommand(Common.usp_RetrieveLocationRecordDetails, connection))
      {
        cmd.CommandType = CommandType.StoredProcedure;
        MaintenanceLocationList locationRecord = new MaintenanceLocationList();

        connection.Open();
        cmd.Parameters.AddWithValue("@LocationID", LocationID);
        SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
          locationRecord.LocationID = Convert.ToInt32(reader["fld_LocationID"]);
          locationRecord.LocationCode = reader["fld_LocationCode"].ToString();
          locationRecord.LocationName = reader["fld_LocationName"].ToString();
          locationRecord.LocationDesc = reader["fld_LocationDesc"].ToString();
        }

        reader.Close();

        return locationRecord;
      }
    }
  }
  #endregion

  #region ValidateLocationRecord
  public int ValidateLocationRecord(int Type, int LocationID, string LocationCode, string LocationName)
  {
    int validationStatus = 0;

    using (SqlConnection con = new SqlConnection())
    {
      con.ConnectionString = Settings.iReserveConnectionStringReader;
      con.Open();

      using (SqlCommand cmd = new SqlCommand(Common.usp_ValidateLocationRecord, con))
      {
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Type", Type);
        cmd.Parameters.AddWithValue("@LocationID", LocationID);
        cmd.Parameters.AddWithValue("@LocationCode", LocationCode);
        cmd.Parameters.AddWithValue("@LocationName", LocationName);

        SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
          validationStatus = Convert.ToInt32(reader["ValidationStatus"]);
        }

        reader.Close();
      }
    }

    return validationStatus;
  }
  #endregion

  #region LocationRecordTransaction
  public void LocationRecordTransaction(int Type, string UserID, int LocationID,
                                          string LocationCode, string LocationName,
                                          string LocationDesc, bool IsDeleted, string MACAddress,
                                          string Browser, string BrowserVersion)
  {
    using (SqlConnection con = new SqlConnection())
    {
      con.ConnectionString = Settings.iReserveConnectionStringWriter;
      con.Open();

      using (SqlCommand cmd = new SqlCommand(Common.usp_Tran_Location, con))
      {
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Type", Type);
        cmd.Parameters.AddWithValue("@UserID", UserID);
        cmd.Parameters.AddWithValue("@LocationID", LocationID);
        cmd.Parameters.AddWithValue("@LocationCode", LocationCode);
        cmd.Parameters.AddWithValue("@LocationName", LocationName);
        cmd.Parameters.AddWithValue("@LocationDesc", LocationDesc);
        cmd.Parameters.AddWithValue("@IsDeleted", IsDeleted);
        cmd.Parameters.AddWithValue("@MACAddress", MACAddress);
        cmd.Parameters.AddWithValue("@Browser", Browser);
        cmd.Parameters.AddWithValue("@BrowserVersion", BrowserVersion);
        cmd.ExecuteNonQuery();
      }
    }
  }
  #endregion

  #endregion

  #region FILE MAINTENANCE - CONFERENCE ROOM

  #region RetrieveConferenceRoomRecords
  public List<MaintenanceConferenceRoomList> RetrieveConferenceRoomRecords(string LocationName, string RoomCode, string RoomName)
  {
    try
    {
      using (SqlConnection connection = new SqlConnection(Settings.iReserveConnectionStringReader))
      {
        using (SqlCommand cmd = new SqlCommand(Common.usp_RetrieveConferenceRoomRecords, connection))
        {
          cmd.CommandType = CommandType.StoredProcedure;
          List<MaintenanceConferenceRoomList> roomList = new List<MaintenanceConferenceRoomList>();

          connection.Open();
          cmd.Parameters.AddWithValue("@LocationName", LocationName);
          cmd.Parameters.AddWithValue("@RoomCode", RoomCode);
          cmd.Parameters.AddWithValue("@RoomName", RoomName);
          SqlDataReader reader = cmd.ExecuteReader();

          while (reader.Read())
          {
            MaintenanceConferenceRoomList roomRecord = new MaintenanceConferenceRoomList();
            roomRecord.RoomID = Convert.ToInt32(reader["fld_RoomID"]);
            roomRecord.RoomCode = reader["fld_RoomCode"].ToString();
            roomRecord.RoomName = reader["fld_RoomName"].ToString();
            roomRecord.RoomDesc = reader["fld_RoomDesc"].ToString();
            roomRecord.LocationID = Convert.ToInt32(reader["fld_LocationID"]);
            roomRecord.LocationName = reader["fld_LocationName"].ToString();
            roomList.Add(roomRecord);
          }

          return roomList;
        }
      }
    }
    catch (Exception error)
    {
      throw error;
    }
  }
  #endregion

  #region RetrieveConferenceRoomRecordDetails
  public MaintenanceConferenceRoomList RetrieveConferenceRoomRecordDetails(int RoomID)
  {
    using (SqlConnection connection = new SqlConnection(Settings.iReserveConnectionStringReader))
    {
      using (SqlCommand cmd = new SqlCommand(Common.usp_RetrieveConferenceRoomRecordDetails, connection))
      {
        cmd.CommandType = CommandType.StoredProcedure;
        MaintenanceConferenceRoomList roomRecord = new MaintenanceConferenceRoomList();

        connection.Open();
        cmd.Parameters.AddWithValue("@RoomID", RoomID);
        SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
          roomRecord.RoomID = Convert.ToInt32(reader["fld_RoomID"]);
          roomRecord.RoomCode = reader["fld_RoomCode"].ToString();
          roomRecord.RoomName = reader["fld_RoomName"].ToString();
          roomRecord.RoomDesc = reader["fld_RoomDesc"].ToString();
          roomRecord.LocationID = Convert.ToInt32(reader["fld_LocationID"]);
          roomRecord.LocationName = reader["fld_LocationName"].ToString();
          roomRecord.MaxPerson = reader["fld_MaxPerson"].ToString();
          roomRecord.IsDataPortAvailable = reader["fld_IsDataPortAvailable"].ToString();
          roomRecord.IsMonitorAvailable = reader["fld_IsMonitorAvailable"].ToString();
          roomRecord.RatePerHour = reader["fld_RatePerHour"].ToString();
          roomRecord.TabletID = reader["fld_TabletID"].ToString();
          roomRecord.MonitorDisplayCode = Convert.ToInt32(reader["fld_MonitorDisplayCode"]);
          roomRecord.MonitorDisplayName = reader["fld_MonitorDisplayName"].ToString();
        }

        reader.Close();

        return roomRecord;
      }
    }
  }
  #endregion

  #region ValidateConferenceRoomRecord
  public int ValidateConferenceRoomRecord(int Type, int RoomID, string RoomCode, string RoomName, int monitorCode)
  {
    int validationStatus = 0;

    using (SqlConnection con = new SqlConnection())
    {
      con.ConnectionString = Settings.iReserveConnectionStringReader;
      con.Open();

      using (SqlCommand cmd = new SqlCommand(Common.usp_ValidateConferenceRoomRecord, con))
      {
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Type", Type);
        cmd.Parameters.AddWithValue("@RoomID", RoomID);
        cmd.Parameters.AddWithValue("@RoomCode", RoomCode);
        cmd.Parameters.AddWithValue("@RoomName", RoomName);
        cmd.Parameters.AddWithValue("@MonitorDisplayCode", monitorCode);

        SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
          validationStatus = Convert.ToInt32(reader["ValidationStatus"]);
        }

        reader.Close();
      }
    }

    return validationStatus;
  }
  #endregion

  #region ConferenceRoomRecordTransaction
  public void ConferenceRoomRecordTransaction(int Type, string UserID, int RoomID, string RoomCode, string RoomName,
                                          string RoomDesc, int LocationID, int MaxPerson, bool IsDataPortAvailable,
                                          bool IsMonitorAvailable, string RatePerHour, string TabletID,
                                          int MonitorDisplayCode, bool IsDeleted,
                                          string MACAddress, string Browser, string BrowserVersion)
  {
    using (SqlConnection con = new SqlConnection())
    {
      con.ConnectionString = Settings.iReserveConnectionStringWriter;
      con.Open();

      using (SqlCommand cmd = new SqlCommand(Common.usp_Tran_ConferenceRoom, con))
      {
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Type", Type);
        cmd.Parameters.AddWithValue("@UserID", UserID);
        cmd.Parameters.AddWithValue("@RoomID", RoomID);
        cmd.Parameters.AddWithValue("@RoomCode", RoomCode);
        cmd.Parameters.AddWithValue("@RoomName", RoomName);
        cmd.Parameters.AddWithValue("@RoomDesc", RoomDesc);
        cmd.Parameters.AddWithValue("@LocationID", LocationID);
        cmd.Parameters.AddWithValue("@MaxPerson", MaxPerson);
        cmd.Parameters.AddWithValue("@IsDataPortAvailable", IsDataPortAvailable);
        cmd.Parameters.AddWithValue("@IsMonitorAvailable", IsMonitorAvailable);
        cmd.Parameters.AddWithValue("@RatePerHour", String.IsNullOrEmpty(RatePerHour) ? Convert.ToDecimal("0") : Convert.ToDecimal(RatePerHour));
        cmd.Parameters.AddWithValue("@TabletID", TabletID);
        cmd.Parameters.AddWithValue("@MonitorDisplayCode", MonitorDisplayCode);
        cmd.Parameters.AddWithValue("@IsDeleted", IsDeleted);
        cmd.Parameters.AddWithValue("@MACAddress", MACAddress);
        cmd.Parameters.AddWithValue("@Browser", Browser);
        cmd.Parameters.AddWithValue("@BrowserVersion", BrowserVersion);
        cmd.ExecuteNonQuery();
      }
    }
  }
  #endregion

  #region RetrieveMonitorDisplayRecords
  public List<MonitorDisplay> RetrieveMonitorDisplayRecords()
  {
    try
    {
      using (SqlConnection connection = new SqlConnection(Settings.iReserveConnectionStringReader))
      {
        using (SqlCommand cmd = new SqlCommand(Common.usp_RetrieveMonitorDisplayRecords, connection))
        {
          cmd.CommandType = CommandType.StoredProcedure;
          List<MonitorDisplay> monitorList = new List<MonitorDisplay>();

          connection.Open();
          SqlDataReader reader = cmd.ExecuteReader();

          while (reader.Read())
          {
            MonitorDisplay monitor = new MonitorDisplay();
            monitor.MonitorDisplayID = Convert.ToInt32(reader["fld_MonitorDisplayID"]);
            monitor.MonitorDisplayCode = Convert.ToInt32(reader["fld_MonitorDisplayCode"]);
            monitor.MonitorDisplayName = reader["fld_MonitorDisplayName"].ToString();
            monitorList.Add(monitor);
          }

          return monitorList;
        }
      }
    }
    catch (Exception error)
    {
      throw error;
    }
  }
  #endregion

  #endregion

  #region CALENDAR - CONFERENCE ROOM

  #region RetrieveConferenceRoomSchedules

  public DataTable RetrieveConferenceRoomSchedules(string dateFrom, int roomID)
  {
    try
    {
      DataTable dt = new DataTable("RoomSchedule");

      using (SqlConnection connection = new SqlConnection(Settings.iReserveConnectionStringWriter))
      {
        using (SqlCommand cmd = new SqlCommand(Common.usp_GetConferenceRoomScheduleTable, connection))
        {
          cmd.CommandType = CommandType.StoredProcedure;

          connection.Open();
          cmd.Parameters.AddWithValue("@dateFrom", dateFrom);
          cmd.Parameters.AddWithValue("@roomID", roomID);
          cmd.ExecuteNonQuery();

          SqlDataAdapter da = new SqlDataAdapter(cmd);
          da.Fill(dt);
        }
      }

      return dt;
    }
    catch (Exception error)
    {
      throw error;
    }
  }

  #endregion

  #region RetrieveTimeSlots

  public List<CRTimeSlot> RetrieveTimeSlots()
  {
    try
    {
      using (SqlConnection connection = new SqlConnection(Settings.iReserveConnectionStringReader))
      {
        using (SqlCommand cmd = new SqlCommand(Common.usp_RetrieveCRTimeSlot, connection))
        {
          cmd.CommandType = CommandType.StoredProcedure;

          List<CRTimeSlot> timeSlotList = new List<CRTimeSlot>();

          connection.Open();
          SqlDataReader reader = cmd.ExecuteReader();

          while (reader.Read())
          {
            CRTimeSlot timeSlot = new CRTimeSlot();
            timeSlot.TimeSlotID = Convert.ToInt32(reader["fld_TimeSlotID"]);
            timeSlot.StartTime = reader["fld_StartTime"].ToString();
            timeSlot.EndTime = reader["fld_EndTime"].ToString();
            timeSlot.StartTime12 = reader["fld_StartTime12"].ToString();
            timeSlot.EndTime12 = reader["fld_EndTime12"].ToString();
            timeSlot.IsEnabled = Convert.ToBoolean(reader["fld_IsEnabled"]);
            timeSlotList.Add(timeSlot);
          }

          return timeSlotList;
        }
      }
    }
    catch (Exception error)
    {
      throw error;
    }
  }

  #endregion

  #region RetrieveCostCenterRecords
  public List<CostCenter> RetrieveCostCenterRecords()
  {
    try
    {
      using (SqlConnection connection = new SqlConnection(Settings.iReserveConnectionStringReader))
      {
        using (SqlCommand cmd = new SqlCommand(Common.usp_RetrieveCostCenter, connection))
        {
          cmd.CommandType = CommandType.StoredProcedure;
          List<CostCenter> costCenterList = new List<CostCenter>();

          connection.Open();
          SqlDataReader reader = cmd.ExecuteReader();

          while (reader.Read())
          {
            CostCenter costCenter = new CostCenter();
            costCenter.CostCenterID = Convert.ToInt32(reader["fld_CostCenterID"]);
            costCenter.CostCenterName = reader["fld_CostCenterName"].ToString();
            costCenter.ChargedCompanyID = reader["fld_ChargedCompanyID"].ToString();
            costCenterList.Add(costCenter);
          }

          return costCenterList;
        }
      }
    }
    catch (Exception error)
    {
      throw error;
    }
  }
  #endregion

  #region RetrieveCRDisplaySchedule

  public DataTable RetrieveCRDisplaySchedule(string date, int monitorDisplayCode)
  {
    try
    {
      DataTable dt = new DataTable("DisplaySchedule");

      using (SqlConnection connection = new SqlConnection(Settings.iReserveConnectionStringWriter))
      {
        using (SqlCommand cmd = new SqlCommand(Common.usp_RetrieveCRDisplaySchedule, connection))
        {
          cmd.CommandType = CommandType.StoredProcedure;

          connection.Open();
          cmd.Parameters.AddWithValue("@monitorDisplayCode", monitorDisplayCode);
          cmd.Parameters.AddWithValue("@date", date);
          cmd.ExecuteNonQuery();

          SqlDataAdapter da = new SqlDataAdapter(cmd);
          da.Fill(dt);
        }
      }

      return dt;
    }
    catch (Exception error)
    {
      throw error;
    }
  }

  #endregion

  #endregion

  #region RetrieveRequestCountByStatus

  public DataTable RetrieveRequestCountByStatus(int statusCode)
  {
    try
    {
      DataTable dt = new DataTable("RequestCount");

      using (SqlConnection connection = new SqlConnection(Settings.iReserveConnectionStringReader))
      {
        using (SqlCommand cmd = new SqlCommand(Common.usp_RetrieveRequestCountByStatus, connection))
        {
          cmd.CommandType = CommandType.StoredProcedure;

          connection.Open();
          cmd.Parameters.AddWithValue("@statusCode", statusCode);
          cmd.ExecuteNonQuery();

          SqlDataAdapter da = new SqlDataAdapter(cmd);
          da.Fill(dt);
        }
      }

      return dt;
    }
    catch (Exception error)
    {
      throw error;
    }
  }

  #endregion
}