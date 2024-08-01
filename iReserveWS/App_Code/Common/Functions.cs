using System;
using System.Collections.Generic;
using System.Web;
using System.Net.Mail;
using System.IO;

/// <summary>
/// Summary description for Functions
/// </summary>
internal class Functions
{
    internal static void SendEmailNotification(EmailNotification emailNotification)
    {
        using (MailMessage email = new MailMessage())
        {
            if (emailNotification.EmailTo.Length > 0)
            {
                foreach (string emailRecipient in emailNotification.EmailTo)
                {
                    MailAddress emailTo = new MailAddress(emailRecipient);
                    email.To.Add(emailTo);
                }
            }

            if (emailNotification.EmailCC.Length > 0)
            {
                foreach (string emailRecipient in emailNotification.EmailCC)
                {
                    MailAddress emailCC = new MailAddress(emailRecipient);
                    email.CC.Add(emailCC);
                }
            }

            email.Subject = emailNotification.Subject;
            email.Body = emailNotification.Body;
            email.IsBodyHtml = emailNotification.IsHtml;

            //Send email
            SmtpClient smtp = new SmtpClient();
            smtp.Send(email);
        }
    }

    internal static string ConvertTrainingRoomListToRow(List<TrainingRoomRequestCharge> trainingRoomRequestChargeList)
    {
        string returnValue = "";

        if (trainingRoomRequestChargeList.Count == 0)
        {
            returnValue = "<tr><td colspan='7'>n/a</td></tr>";
        }
        else
        {
            foreach (TrainingRoomRequestCharge trainingRoomRequestCharge in trainingRoomRequestChargeList)
            {
                returnValue += "<tr>" +
                    "<td>" + trainingRoomRequestCharge.TRoomName + "</td>" +
                    "<td>" + trainingRoomRequestCharge.NumberOfPartition + "</td>" +
                    "<td>" + trainingRoomRequestCharge.Date.ToString("MMM d, yyyy") + "</td>" +
                    "<td>" + trainingRoomRequestCharge.StartDateTime.ToString("h:mm tt") + "</td>" +
                    "<td>" + trainingRoomRequestCharge.EndDateTime.ToString("h:mm tt") + "</td>" +
                    "<td>" + trainingRoomRequestCharge.NumberOfHours + "</td>" +
                    "<td>" + trainingRoomRequestCharge.RatePerDay + "</td>" +
                    "<td>" + trainingRoomRequestCharge.ExtensionRatePerHour + "</td>" +
                    "<td>" + trainingRoomRequestCharge.AmountCharge + "</td>";

                returnValue += "</tr>";
            }
        }

        return returnValue;
    }

    internal static string ConvertAccomdationRoomListToRow(List<AccomodationRoomRequest> accomodationRoomRequestList)
    {
        string returnValue = "";

        if (accomodationRoomRequestList.Count == 0)
        {
            returnValue = "<tr><td colspan='6'>n/a</td></tr>";
        }
        else
        {
            foreach (AccomodationRoomRequest accomodationRoomRequest in accomodationRoomRequestList)
            {
                returnValue += "<tr>" +
                    "<td>" + accomodationRoomRequest.RoomName + "</td>" +
                    "<td>" + accomodationRoomRequest.StartDate.ToString("MMM d, yyyy") + "</td>" +
                    "<td>" + accomodationRoomRequest.EndDate.ToString("MMM d, yyyy") + "</td>" +
                    "<td>" + accomodationRoomRequest.NumberOfNights + "</td>" +
                    "<td>" + accomodationRoomRequest.RatePerNight + "</td>" +
                    "<td>" + accomodationRoomRequest.AmountCharge + "</td>";

                returnValue += "</tr>";
            }
        }

        return returnValue;
    }

    internal static string ConvertOtherChargeListToRow(List<OtherCharge> otherChargeList)
    {
        string returnValue = "";

        if (otherChargeList.Count == 0)
        {
            returnValue = "<tr><td colspan='2'>n/a</td></tr>";
        }
        else
        {
            foreach (OtherCharge otherCharge in otherChargeList)
            {
                returnValue += "<tr>" +
                    "<td>" + otherCharge.Particulars + "</td>" +
                    "<td>" + otherCharge.AmountCharge + "</td>";

                returnValue += "</tr>";
            }
        }

        return returnValue;
    }

    internal static decimal GetTotalPayable(List<TrainingRoomRequestCharge> trainingRoomRequestChargeList, List<AccomodationRoomRequest> accomodationRoomRequestList, List<OtherCharge> otherChargeList)
    {
        decimal returnValue = 0;

        foreach (TrainingRoomRequestCharge trCharge in trainingRoomRequestChargeList)
        {
            returnValue += Convert.ToDecimal(trCharge.AmountCharge);
        }

        foreach (AccomodationRoomRequest arRequest in accomodationRoomRequestList)
        {
            returnValue += Convert.ToDecimal(arRequest.AmountCharge);
        }

        foreach (OtherCharge otherCharge in otherChargeList)
        {
            returnValue += Convert.ToDecimal(otherCharge.AmountCharge);
        }

        return returnValue;
    }
}