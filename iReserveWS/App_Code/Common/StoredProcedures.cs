using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for StoredProcedures
/// </summary>
internal class StoredProcedures
{
  internal const string RetrieveAccomodationRoomRecords = "[Reader].[usp_RetrieveAccomodationRoomRecords]";
  internal const string RetrieveAccomodationRoomRecordDetails = "[Reader].[usp_RetrieveAccomodationRoomRecordDetails]";
  internal const string ValidateAccomodationRoomRecord = "[Reader].[usp_ValidateAccomodationRoomRecord]";
  internal const string TranAccomodationRoom = "[Updater].[usp_TranAccomodationRoom]";

  internal const string RetrieveTrainingRoomRecords = "[Reader].[usp_RetrieveTrainingRoomRecords]";
  internal const string RetrieveTrainingRoomRecordDetails = "[Reader].[usp_RetrieveTrainingRoomRecordDetails]";
  internal const string ValidateTrainingRoomRecord = "[Reader].[usp_ValidateTrainingRoomRecord]";
  internal const string TranTrainingRoom = "[Updater].[usp_TranTrainingRoom]";

  internal const string RetrieveTRPartitionRecords = "[Reader].[usp_RetrieveTRPartitionRecords]";
  internal const string RetrieveTRPartitionRecordDetails = "[Reader].[usp_RetrieveTRPartitionRecordDetails]";
  internal const string TranTRPartition = "[Updater].[usp_TranTRPartition]";

  internal const string RetrieveTRRateRecords = "[Reader].[usp_RetrieveTRRateRecords]";
  internal const string TranTRRate = "[Updater].[usp_TranTRRate]";

  internal const string RetrieveAccRoomCalendarSchedule = "[Updater].[usp_RetrieveAccRoomCalendarSchedule]";
  internal const string RetrieveTRCalendarSchedule = "[Updater].[usp_RetrieveTrainingRoomCalendarSchedule]";

  internal const string RetrieveLastGeneratedCCRefNo = "[Updater].[usp_RetrieveLastGeneratedCCRefNo]";
  internal const string InsertLastGeneratedCCRefNo = "[Updater].[usp_InsertLastGeneratedCCRefNo]";

  internal const string RetrieveAccomodationRoomRequest = "[Reader].[usp_RetrieveAccomodationRoomRequest]";
  internal const string ValidateAccomodationRoomScheduleAvailability = "[Reader].[usp_ValidateAccomodationRoomScheduleAvailability]";
  internal const string InsertAccomodationRoomRequest = "[Updater].[usp_InsertAccomodationRoomRequest]";
  internal const string InsertAccomodationRoomScheduleMapping = "[Updater].[usp_InsertAccomodationRoomScheduleMapping]";
  internal const string CancelAccomodationRoomScheduleMapping = "[Updater].[usp_CancelAccomodationRoomScheduleMapping]";

  internal const string RetrieveTrainingRoomRequest = "[Reader].[usp_RetrieveTrainingRoomRequest]";
  internal const string RetrieveTrainingRoomRequestCharges = "[Reader].[usp_RetrieveTrainingRoomRequestCharges]";
  internal const string ValidateTrainingRoomScheduleAvailability = "[Reader].[usp_ValidateTrainingRoomScheduleAvailability]";
  internal const string InsertTrainingRoomRequest = "[Updater].[usp_InsertTrainingRoomRequest]";
  internal const string InsertTrainingRoomRequestDetail = "[Updater].[usp_InsertTrainingRoomRequestDetail]";
  internal const string InsertTrainingRoomRequestCharges = "[Updater].[usp_InsertTrainingRoomRequestCharges]";
  internal const string InsertTrainingRoomScheduleMapping = "[Updater].[usp_InsertTrainingRoomScheduleMapping]";
  internal const string CancelTrainingRoomScheduleMapping = "[Updater].[usp_CancelTrainingRoomScheduleMapping]";

  internal const string RetrieveCCRequestAttachment = "[Reader].[usp_RetrieveCCRequestAttachment]";
  internal const string RetrieveCCRequestAttachmentByStatus = "[Reader].[usp_RetrieveCCRequestAttachmentByStatus]";
  internal const string InsertCCRequestAttachment = "[Updater].[usp_InsertCCRequestAttachment]";

  internal const string RetrieveCCRequestHistory = "[Reader].[usp_RetrieveCCRequestHistory]";
  internal const string RetrieveCCRequestHistoryByStatus = "[Reader].[usp_RetrieveCCRequestHistoryByStatus]";
  internal const string InsertCCRequestHistory = "[Updater].[usp_InsertCCRequestHistory]";

  internal const string RetrieveCCRequestDetails = "[Reader].[usp_RetrieveCCRequestDetails]";
  internal const string RetrieveCCRequestDetailsByRequestor = "[Reader].[usp_RetrieveCCRequestDetailsByRequestor]";
  internal const string RetrieveCCRequestRecordsByRequestor = "[Reader].[usp_RetrieveCCRequestRecordsByRequestor]";
  internal const string RetrieveCCRequestRecordsByStatus = "[Reader].[usp_RetrieveCCRequestRecordsByStatus]";
  internal const string InsertCCRequest = "[Updater].[usp_InsertCCRequest]";
  internal const string UpdateCCRequestStatus = "[Updater].[usp_UpdateCCRequestStatus]";

  internal const string RetrieveTrainingRoomRequestReport = "[Reader].[usp_RetrieveTrainingRoomRequestReport]";
  internal const string RetrieveAccomodationRoomRequestReport = "[Reader].[usp_RetrieveAccomodationRoomRequestReport]";
  internal const string RetrieveSOAReport = "[Reader].[usp_RetrieveSOAReport]";

  internal const string RetrieveCCRequestRecordsBySOAStatus = "[Reader].[usp_RetrieveCCRequestRecordsBySOAStatus]";
  internal const string RetrieveCCRequestRecordsApprovedSOA = "[Reader].[usp_RetrieveCCRequestRecordsApprovedSOA]";
  internal const string UpdateSOAStatus = "[Updater].[usp_UpdateSOAStatus]";

  internal const string RetrieveSOAHistory = "[Reader].[usp_RetrieveSOAHistory]";
  internal const string RetrieveSOAHistoryByStatus = "[Reader].[usp_RetrieveSOAHistoryByStatus]";
  internal const string InsertSOAHistory = "[Updater].[usp_InsertSOAHistory]";

  internal const string RetrieveCCRequestOtherCharges = "[Reader].[usp_RetrieveCCRequestOtherCharges]";
  internal const string TranOtherCharge = "[Updater].[usp_TranOtherCharge]";

  internal const string TranTrainingRoomScheduleMapping = "[Updater].[usp_TranTrainingRoomScheduleMapping]";
  internal const string TranAccomodationRoomScheduleMapping = "[Updater].[usp_TranAccomodationRoomScheduleMapping]";

  internal const string RetrieveCancellationFee = "[Reader].[usp_RetrieveCancellationFee]";
  internal const string InsertCCRequestCancellation = "[Updater].[usp_InsertCCRequestCancellation]";
  internal const string UpdateCCRequestPercentDiscount = "[Updater].[usp_UpdateCCRequestPercentDiscount]";
}
