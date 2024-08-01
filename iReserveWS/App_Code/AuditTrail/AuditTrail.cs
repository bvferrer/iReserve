using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for AuditTrail
/// </summary>
public class AuditTrail
{
    public AuditTrail()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Properties

    private int _auditID;

    public int AuditID
    {
        get { return _auditID; }
        set { _auditID = value; }
    }

    private string _userID;

    public string UserID
    {
        get { return _userID; }
        set { _userID = value; }
    }

    private string _ipAddress;

    public string IpAddress
    {
        get { return _ipAddress; }
        set { _ipAddress = value; }
    }

    private string _macAdress;

    public string MacAdress
    {
        get { return _macAdress; }
        set { _macAdress = value; }
    }

    private string _browser;

    public string Browser
    {
        get { return _browser; }
        set { _browser = value; }
    }

    private string _browserVersion;

    public string BrowserVersion
    {
        get { return _browserVersion; }
        set { _browserVersion = value; }
    }

    private DateTime _actionDate;

    public DateTime ActionDate
    {
        get { return _actionDate; }
        set { _actionDate = value; }
    }

    private string _actionTaken;

    public string ActionTaken
    {
        get { return _actionTaken; }
        set { _actionTaken = value; }
    }

    private string _actionDetails;

    public string ActionDetails
    {
        get { return _actionDetails; }
        set { _actionDetails = value; }
    }

    #endregion

    #region Methods

    public bool InsertAuditTrailEntry()
    {
        bool isSuccess;

        try
        {
            wsAuditTrailComplianceTool.AuditTrail auditTrail = new wsAuditTrailComplianceTool.AuditTrail();
            auditTrail.ActionDate = this.ActionDate;
            auditTrail.ActionTaken = this.ActionTaken;
            auditTrail.ActionDetails = this.ActionDetails;
            auditTrail.BrowserName = this.Browser;
            auditTrail.BrowserVersion = this.BrowserVersion;
            auditTrail.IpAddress = this.IpAddress;
            auditTrail.MacAddress = this.MacAdress;
            auditTrail.UserId = this.UserID;

            ConnectionStringKey connectionStringKey = new ConnectionStringKey();
            connectionStringKey.ConnectionStringName = "iReserve_Writer";

            try
            {
                AuditTrailComplianceToolWSUtility.AddAuditTrail(auditTrail, connectionStringKey);

                isSuccess = true;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message.ToString();
                isSuccess = false;
            }
        }
        catch
        {
            isSuccess = false;
        }

        return isSuccess;
    }

    #endregion
}