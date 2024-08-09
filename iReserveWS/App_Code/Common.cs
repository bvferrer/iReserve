using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using System.Collections.Generic;

/// <summary>
/// Summary description for Common
/// </summary>
public class Common
{
    public Common()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Stored Procedures
    public static string usp_RetrieveLocationRecords
    {
        get { return "[Reader].[usp_RetrieveLocationRecords]"; }
    }

    public static string usp_RetrieveLocationRecordDetails
    {
        get { return "[Reader].[usp_RetrieveLocationRecordDetails]"; }
    }

    public static string usp_ValidateLocationRecord
    {
        get { return "[Reader].[usp_ValidateLocationRecord]"; }
    }

    public static string usp_Tran_Location
    {
        get { return "[Updater].[usp_Tran_Location]"; }
    }

    public static string usp_RetrieveConferenceRoomRecords
    {
        get { return "[Reader].[usp_RetrieveConferenceRoomRecords]"; }
    }

    public static string usp_RetrieveConferenceRoomRecordDetails
    {
        get { return "[Reader].[usp_RetrieveConferenceRoomRecordDetails]"; }
    }

    public static string usp_ValidateConferenceRoomRecord
    {
        get { return "[Reader].[usp_ValidateConferenceRoomRecord]"; }
    }

    public static string usp_Tran_ConferenceRoom
    {
        get { return "[Updater].[usp_Tran_ConferenceRoom]"; }
    }

    public static string usp_GetConferenceRoomScheduleTable
    {
        get { return "[Updater].[usp_GetConferenceRoomScheduleTable]"; }
    }

    public static string usp_RetrieveMonitorDisplayRecords
    {
        get { return "[Reader].[usp_RetrieveMonitorDisplayRecords]"; }
    }

    public static string usp_RetrieveCRTimeSlot
    {
        get { return "[Reader].[usp_RetrieveCRTimeSlot]"; }
    }

    public static string usp_RetrieveCostCenter
    {
        get { return "[Reader].[usp_RetrieveCostCenter]"; }
    }

    public static string usp_GetLastGeneratedCRRF
    {
        get { return "[Updater].[usp_GetLastGeneratedCRRF]"; }
    }

    public static string usp_InsertCRRequest
    {
        get { return "[Updater].[usp_InsertCRRequest]"; }
    }

    public static string usp_InsertCRRequestAttachment
    {
        get { return "[Updater].[usp_InsertCRRequestAttachment]"; }
    }

    public static string usp_InsertCRRequestAttendee
    {
        get { return "[Updater].[usp_InsertCRRequestAttendee]"; }
    }

    public static string usp_InsertCRRequestHistory
    {
        get { return "[Updater].[usp_InsertCRRequestHistory]"; }
    }

    public static string usp_RetrieveCRRequestDetails
    {
        get { return "[Reader].[usp_RetrieveCRRequestDetails]"; }
    }

    public static string usp_RetrieveCRRequestHistoryByStatus
    {
        get { return "[Reader].[usp_RetrieveCRRequestHistoryByStatus]"; }
    }

    public static string usp_RetrieveRequestorDetails
    {
        get { return "[Reader].[usp_RetrieveRequestorDetails]"; }
    }

    public static string usp_RetrieveCRRequestRecordsByStatus
    {
        get { return "[Reader].[usp_RetrieveCRRequestRecordsByStatus]"; }
    }

    public static string usp_RetrieveCRRequestAttendees
    {
        get { return "[Reader].[usp_RetrieveCRRequestAttendees]"; }
    }

    public static string usp_UpdateCRRequestStatus
    {
        get { return "[Updater].[usp_UpdateCRRequestStatus]"; }
    }

    public static string usp_InsertCRScheduleMapping
    {
        get { return "[Updater].[usp_InsertCRScheduleMapping]"; }
    }

    public static string usp_ValidateScheduleAvailability
    {
        get { return "[Reader].[usp_ValidateScheduleAvailability]"; }
    }

    public static string usp_RetrieveCRRequestRecordsByRequestor
    {
        get { return "[Reader].[usp_RetrieveCRRequestRecordsByRequestor]"; }
    }

    public static string usp_RetrieveCRRequestDetailsByRequestor
    {
        get { return "[Reader].[usp_RetrieveCRRequestDetailsByRequestor]"; }
    }

    public static string usp_RetrieveCRRequestHistory
    {
        get { return "[Reader].[usp_RetrieveCRRequestHistory]"; }
    }

    public static string usp_UpdateCRScheduleMappingStatus
    {
        get { return "[Updater].[usp_UpdateCRScheduleMappingStatus]"; }
    }

    public static string usp_RetrieveCRRequestRecordsReport
    {
        get { return "[Reader].[usp_RetrieveCRRequestRecordsReport]"; }
    }

    public static string usp_RetrieveCRRequestRecordsReportByDate
    {
        get { return "[Reader].[usp_RetrieveCRRequestRecordsReportByDate]"; }
    }

    public static string usp_RetrieveCRRequestAttachment
    {
        get { return "[Reader].[usp_RetrieveCRRequestAttachment]"; }
    }

    public static string usp_RetrieveCRRequestAttachmentByStatus
    {
        get { return "[Reader].[usp_RetrieveCRRequestAttachmentByStatus]"; }
    }

    public static string usp_RetrieveCRDisplaySchedule
    {
        get { return "[Updater].[usp_RetrieveCRDisplaySchedule]"; }
    }

    public static string usp_RetrieveRequestCountByStatus
    {
        get { return "[Reader].[usp_RetrieveRequestCountByStatus]"; }
    }

    public static string usp_RetrieveCRTimeSlotByID
    {
        get { return "[Reader].[usp_RetrieveCRTimeSlotByID]"; }
    }

    public static string usp_RetrieveCRSimilarPendingRequests
    {
        get { return "[Reader].[usp_RetrieveCRSimilarPendingRequests]"; }
    }

  public static string usp_RetrieveChargedCompany
  {
    get { return "[Reader].[usp_RetrieveChargedCompany]"; }
  }
   public static string usp_RetrieveCostCenterByChargedCompanyId
  {
    get { return "[Reader].[usp_RetrieveCostCenterByChargedCompanyId]"; }
  }
  #endregion
}
