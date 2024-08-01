using System;
using System.Collections.Generic;
using System.Web;
using System.Net.Mail;
using System.IO;
using System.Diagnostics;
using System.Text;

/// <summary>
/// Summary description for EmailNotification
/// </summary>
public class EmailNotification
{
    #region Constructor
    public EmailNotification()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region Fields/Properties

    private string[] _emailTo;

    public string[] EmailTo
    {
        get { return _emailTo; }
        set { _emailTo = value; }
    }

    private string[] _emailCC;

    public string[] EmailCC
    {
        get { return _emailCC; }
        set { _emailCC = value; }
    }

    private string _subject;

    public string Subject
    {
        get { return _subject; }
        set { _subject = value; }
    }

    private string _body;

    public string Body
    {
        get { return _body; }
        set { _body = value; }
    }

    private bool _isHtml;

    public bool IsHtml
    {
        get { return _isHtml; }
        set { _isHtml = value; }
    }

    #endregion

    #region Methods

    public void SendEmailNotification(CRRequest request)
    {
        //Get email addresses
        EmployeeDetails requestor = new EmployeeDetails();
        requestor.GetRequestorDetails(request.RequestedByID);

        //Get request details
        CRRequest requestDetails = new CRRequest();
        requestDetails.RetrieveCRRequestDetails(request.RequestReferenceNo);

        CRRequestHistory requestHistory = new CRRequestHistory();
        requestHistory.RetrieveCRRequestHistoryDetailsByStatus(request);

        //Construct email
        string subject = string.Format(Settings.EmailSubjectFormat, requestDetails.Status.StatusName.ToUpper());

        string body;

        if (requestDetails.Status.StatusCode == StatusCode.Confirmed)
        {
            body = File.ReadAllText(HttpContext.Current.Server.MapPath(Settings.RequestorConfirmedNotification));
            body = body.Replace("[TimeInCode]", requestDetails.RequestReferenceNo.Substring(requestDetails.RequestReferenceNo.Length - 7));
        }
        else
        {
            body = File.ReadAllText(HttpContext.Current.Server.MapPath(Settings.RequestorNotification));
        }

        body = body.Replace("[RequestorName]", requestDetails.RequestedBy)
            .Replace("[Message]", Settings.GetEmailMessage(requestDetails.Status.StatusCode))
            .Replace("[RefNumber]", requestDetails.RequestReferenceNo)
            .Replace("[RoomName]", requestDetails.ConferenceRoom.RoomName)
            .Replace("[Date]", requestDetails.Date.ToShortDateString())
            .Replace("[StartTime]", requestDetails.StartTime.StartTime12)
            .Replace("[EndTime]", requestDetails.EndTime.EndTime12)
            .Replace("[Agenda]", requestDetails.Agenda)
            .Replace("[Status]", requestDetails.Status.StatusName)
            .Replace("[DateRequested]", requestDetails.DateRequested.ToString())
            .Replace("[Remarks]", requestHistory.Remarks);

        using (MailMessage email = new MailMessage())
        {
            string requestorEmail = string.Empty;
            string supervisorEmail = string.Empty;
            string[] adminEmail = new string[] { };

            if (!string.IsNullOrEmpty(requestor.Email))
            {
                requestorEmail = requestor.Email;
                MailAddress requestorAddress = new MailAddress(requestorEmail);
                email.To.Add(requestorAddress);
            }

            if (!string.IsNullOrEmpty(requestor.SupervisorEmail))
            {
                supervisorEmail = requestor.SupervisorEmail;
                MailAddress supervisorAddress = new MailAddress(supervisorEmail);
                email.CC.Add(supervisorAddress);
            }

            if (!string.IsNullOrEmpty(Settings.AdminEmailAddress))
            {
                adminEmail = Settings.AdminEmailAddress.Split(',');
            }

            foreach (string emailAdd in adminEmail)
            {
                MailAddress adminAddress = new MailAddress(emailAdd);
                email.CC.Add(adminAddress);
            }

            email.Subject = subject;
            email.Body = body;
            email.IsBodyHtml = true;

            //Send email
            SmtpClient smtp = new SmtpClient();
            smtp.Send(email);
        }
    }

    public void SendCCEmailNotification(string ccRequestReferenceNo)
    {
        //Get request details
        CCRequest requestDetails = new CCRequest();
        requestDetails.RetrieveCCRequestDetails(ccRequestReferenceNo);

        //Get email addresses
        EmployeeDetails requestor = new EmployeeDetails();
        requestor.GetRequestorDetails(requestDetails.CreatedByID);

        CCRequestHistory requestHistory = new CCRequestHistory();
        requestHistory.RetrieveCCRequestHistorybyStatus(requestDetails.CCRequestReferenceNo, requestDetails.StatusCode);

        //Construct email
        string subject = string.Format(Settings.EmailSubjectFormat, requestDetails.StatusName.ToUpper());

        string body = File.ReadAllText(HttpContext.Current.Server.MapPath(Settings.CCRequestorNotification));

        body = body.Replace("[RequestorName]", requestDetails.CreatedBy)
            .Replace("[Message]", Settings.GetEmailMessage(requestDetails.StatusCode))
            .Replace("[RefNumber]", requestDetails.CCRequestReferenceNo)
            .Replace("[EventName]", requestDetails.EventName)
            .Replace("[StartDate]", requestDetails.StartDate.ToString("MM/dd/yyyy"))
            .Replace("[EndDate]", requestDetails.EndDate.ToString("MM/dd/yyyy"))
            .Replace("[Status]", requestDetails.StatusName)
            .Replace("[DateRequested]", requestDetails.DateCreated.ToString("MM/dd/yyyy hh:mm tt"))
            .Replace("[Remarks]", requestHistory.Remarks);

        using (MailMessage email = new MailMessage())
        {
            string requestorEmail = string.Empty;
            string supervisorEmail = string.Empty;
            string[] adminEmail = new string[] { };

            if (!string.IsNullOrEmpty(requestor.Email))
            {
                requestorEmail = requestor.Email;
                MailAddress requestorAddress = new MailAddress(requestorEmail);
                email.To.Add(requestorAddress);
            }

            if (!string.IsNullOrEmpty(requestor.SupervisorEmail))
            {
                supervisorEmail = requestor.SupervisorEmail;
                MailAddress supervisorAddress = new MailAddress(supervisorEmail);
                email.CC.Add(supervisorAddress);
            }

            if (!string.IsNullOrEmpty(Settings.CCAdminEmailAddress))
            {
                adminEmail = Settings.CCAdminEmailAddress.Split(',');
            }

            foreach (string emailAdd in adminEmail)
            {
                MailAddress adminAddress = new MailAddress(emailAdd);
                email.CC.Add(adminAddress);
            }

            email.Subject = subject;
            email.Body = body;
            email.IsBodyHtml = true;

            //Send email
            SmtpClient smtp = new SmtpClient();
            smtp.Send(email);
        }
    }

    public void SendSOAEmailNotification(string ccRequestReferenceNo)
    {
        //Get request details
        CCRequest requestDetails = new CCRequest();
        requestDetails.RetrieveCCRequestDetails(ccRequestReferenceNo);

        SOAHistory soaHistory = new SOAHistory();
        soaHistory.RetrieveSOAHistorybyStatus(requestDetails.CCRequestReferenceNo, requestDetails.SOAStatusCode);

        //Construct email
        string subject = string.Format(Settings.SOAEmailSubjectFormat, requestDetails.SOAStatusName.ToUpper());

        string body = File.ReadAllText(HttpContext.Current.Server.MapPath(Settings.SOANotification));

        body = body.Replace("[Message]", Settings.GetSOAEmailMessage(requestDetails.SOAStatusCode))
            .Replace("[RefNumber]", requestDetails.CCRequestReferenceNo)
            .Replace("[EventName]", requestDetails.EventName)
            .Replace("[StartDate]", requestDetails.StartDate.ToString("MM/dd/yyyy"))
            .Replace("[EndDate]", requestDetails.EndDate.ToString("MM/dd/yyyy"))
            .Replace("[SOAStatus]", requestDetails.SOAStatusName)
            .Replace("[DateRequested]", requestDetails.DateCreated.ToString("MM/dd/yyyy hh:mm tt"))
            .Replace("[Remarks]", soaHistory.Remarks);

        using (MailMessage email = new MailMessage())
        {
            string[] soaApproverEmail = new string[] { };
            string[] adminEmail = new string[] { };

            if (!string.IsNullOrEmpty(Settings.SOAApproverEmailAddress))
            {
                soaApproverEmail = Settings.SOAApproverEmailAddress.Split(',');
            }

            foreach (string emailAdd in soaApproverEmail)
            {
                MailAddress soaApproverAddress = new MailAddress(emailAdd);
                email.CC.Add(soaApproverAddress);
            }

            if (!string.IsNullOrEmpty(Settings.CCAdminEmailAddress))
            {
                adminEmail = Settings.CCAdminEmailAddress.Split(',');
            }

            foreach (string emailAdd in adminEmail)
            {
                MailAddress adminAddress = new MailAddress(emailAdd);
                email.To.Add(adminAddress);
            }

            email.Subject = subject;
            email.Body = body;
            email.IsBodyHtml = true;

            //Send email
            SmtpClient smtp = new SmtpClient();
            smtp.Send(email);
        }
    }

    public void SendSOADetails(string ccRequestReferenceNo)
    {
        //Get request details
        CCRequest requestDetails = new CCRequest();
        requestDetails.RetrieveCCRequestDetails(ccRequestReferenceNo);

        List<TrainingRoomRequestCharge> trainingRoomRequestChargeList = new List<TrainingRoomRequestCharge>();
        List<AccomodationRoomRequest> accomodationRoomRequestList = new List<AccomodationRoomRequest>();
        List<OtherCharge> otherChargeList = new List<OtherCharge>();

        TrainingRoomRequestCharge trainingRoomRequestCharge = new TrainingRoomRequestCharge();
        trainingRoomRequestChargeList = trainingRoomRequestCharge.RetrieveTrainingRoomRequestCharges(ccRequestReferenceNo);

        AccomodationRoomRequest accomodationRoomRequest = new AccomodationRoomRequest();
        accomodationRoomRequestList = accomodationRoomRequest.RetrieveAccomodationRoomRequestRecords(ccRequestReferenceNo);

        OtherCharge otherCharge = new OtherCharge();
        otherChargeList = otherCharge.RetrieveCCRequestOtherCharges(ccRequestReferenceNo);

        //Get email addresses
        EmployeeDetails requestor = new EmployeeDetails();
        requestor.GetRequestorDetails(requestDetails.CreatedByID);

        //Construct email
        string subject = string.Format(Settings.SOADetailsEmailSubjectFormat);

        string body = File.ReadAllText(HttpContext.Current.Server.MapPath(Settings.SOADetails));

        body = body.Replace("[RequestorName]", requestDetails.CreatedBy)
            .Replace("[RefNumber]", requestDetails.CCRequestReferenceNo)
            .Replace("[EventName]", requestDetails.EventName)
            .Replace("[CostCenter]", requestDetails.CostCenterName)
            .Replace("[StartDate]", requestDetails.StartDate.ToString("MMM d, yyyy"))
            .Replace("[EndDate]", requestDetails.EndDate.ToString("MMM d, yyyy"))
            .Replace("[DateRequested]", requestDetails.DateCreated.ToString("MMM d, yyyy hh:mm tt"))
            .Replace("[TrainingRoom]", Functions.ConvertTrainingRoomListToRow(trainingRoomRequestChargeList))
            .Replace("[AccomodationRoom]", Functions.ConvertAccomdationRoomListToRow(accomodationRoomRequestList))
            .Replace("[OtherCharges]", Functions.ConvertOtherChargeListToRow(otherChargeList))
            .Replace("[TotalPayable]", "PHP. " + Functions.GetTotalPayable(trainingRoomRequestChargeList, accomodationRoomRequestList, otherChargeList).ToString("#,###.00"));

        using (MailMessage email = new MailMessage())
        {
            string requestorEmail = string.Empty;
            string supervisorEmail = string.Empty;
            string[] adminEmail = new string[] { };
            string[] soaApproverEmail = new string[] { };

            if (!string.IsNullOrEmpty(requestor.Email))
            {
                requestorEmail = requestor.Email;
                MailAddress requestorAddress = new MailAddress(requestorEmail);
                email.To.Add(requestorAddress);
            }

            if (!string.IsNullOrEmpty(requestor.SupervisorEmail))
            {
                supervisorEmail = requestor.SupervisorEmail;
                MailAddress supervisorAddress = new MailAddress(supervisorEmail);
                email.CC.Add(supervisorAddress);
            }

            if (!string.IsNullOrEmpty(Settings.CCAdminEmailAddress))
            {
                adminEmail = Settings.CCAdminEmailAddress.Split(',');
            }

            foreach (string emailAdd in adminEmail)
            {
                MailAddress adminAddress = new MailAddress(emailAdd);
                email.CC.Add(adminAddress);
            }

            if (!string.IsNullOrEmpty(Settings.SOAApproverEmailAddress))
            {
                soaApproverEmail = Settings.SOAApproverEmailAddress.Split(',');
            }

            foreach (string emailAdd in soaApproverEmail)
            {
                MailAddress soaApproverAddress = new MailAddress(emailAdd);
                email.CC.Add(soaApproverAddress);
            }

            email.Subject = subject;
            email.Body = body;
            email.IsBodyHtml = true;

            //Send email
            SmtpClient smtp = new SmtpClient();
            smtp.Send(email);
        }
    }

    public void ConstructErrorNotification(string message, EventLogEntryType eventLogEntryType, int eventID)
    {
        StringBuilder logDetails = new StringBuilder();
        logDetails.AppendLine(string.Format("Machine Name: {0}", System.Environment.MachineName));
        logDetails.AppendLine(string.Format("Event ID: {0}", eventID));
        logDetails.AppendLine(string.Format("DateTime: {0:MM-dd-yyyy hh:mm:ss tt}", DateTime.Now));
        logDetails.AppendLine(string.Format("Event Type: {0}", eventLogEntryType.ToString()));
        logDetails.AppendLine(string.Format("Event Source: {0}", Settings.EventSource));
        logDetails.AppendLine(string.Format("Details: {0}{1}{0}", Environment.NewLine, message));
        logDetails.AppendLine(string.Format("******** End of line ********{0}{0}{0}", Environment.NewLine));

        string[] eventEmailList = Settings.EventEmailTo.Split(',');
        string[] emailCCList = new string[] { };

        this.EmailTo = eventEmailList;
        this.EmailCC = emailCCList;
        this.IsHtml = false;
        this.Subject = string.Format(Messages.ErrorEmailSubject, eventLogEntryType.ToString().ToUpper(), Settings.EventSource, Settings.EventEnvironment);
        this.Body = logDetails.ToString();
    }

    public void ConstructCommonErrorNotification(string message, EventLogEntryType eventLogEntryType, int eventID, string machineName, string eventSource)
    {
        StringBuilder logDetails = new StringBuilder();
        logDetails.AppendLine(string.Format("Machine Name: {0}", machineName));
        logDetails.AppendLine(string.Format("Event ID: {0}", eventID));
        logDetails.AppendLine(string.Format("DateTime: {0:MM-dd-yyyy hh:mm:ss tt}", DateTime.Now));
        logDetails.AppendLine(string.Format("Event Type: {0}", eventLogEntryType.ToString()));
        logDetails.AppendLine(string.Format("Event Source: {0}", eventSource));
        logDetails.AppendLine(string.Format("Details: {0}{1}{0}", Environment.NewLine, message));
        logDetails.AppendLine(string.Format("******** End of line ********{0}{0}{0}", Environment.NewLine));

        string[] eventEmailList = Settings.EventEmailTo.Split(',');
        string[] emailCCList = new string[] { };

        this.EmailTo = eventEmailList;
        this.EmailCC = emailCCList;
        this.IsHtml = false;
        this.Subject = string.Format(Messages.ErrorEmailSubject, eventLogEntryType.ToString().ToUpper(), eventSource, Settings.EventEnvironment);
        this.Body = logDetails.ToString();
    }

    #endregion

}