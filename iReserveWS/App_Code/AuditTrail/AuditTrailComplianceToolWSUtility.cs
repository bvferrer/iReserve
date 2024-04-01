using System;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web;
using AppCryptor;

/// <summary>
/// Summary description for AuditTrailComplianceToolWSUtility
/// </summary>
public class AuditTrailComplianceToolWSUtility
{
	public AuditTrailComplianceToolWSUtility()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    #region Fields/Properties

    private static wsAuditTrailComplianceTool.Service _wsAuditTrailComplianceTool = new wsAuditTrailComplianceTool.Service();

    #endregion

    #region Public Methods

    public static void AddAuditTrail(wsAuditTrailComplianceTool.AuditTrail auditTrail, ConnectionStringKey connectionStringKey)
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings[connectionStringKey.ConnectionStringName].ConnectionString);

        wsAuditTrailComplianceTool.AddAuditTrailRequest addAuditTrailRequest = new wsAuditTrailComplianceTool.AddAuditTrailRequest();
        addAuditTrailRequest.AuditTrail = auditTrail;
        addAuditTrailRequest.DatabaseName = AESCrypt.EncryptDecrypt(builder.InitialCatalog, "Encrypt");
        addAuditTrailRequest.PassKey = ConfigurationManager.AppSettings["PassKey"].ToString();
        addAuditTrailRequest.Password = AESCrypt.EncryptDecrypt(builder.Password, "Encrypt");
        addAuditTrailRequest.ServerName = AESCrypt.EncryptDecrypt(builder.DataSource, "Encrypt");
        addAuditTrailRequest.UserId = AESCrypt.EncryptDecrypt(builder.UserID, "Encrypt");

        wsAuditTrailComplianceTool.AddAuditTrailResponse addAuditTrailResponse = _wsAuditTrailComplianceTool.AddAuditTrail(addAuditTrailRequest);

        switch (addAuditTrailResponse.ResponseStatus)
        {
            case wsAuditTrailComplianceTool.ResponseStatus.Successful:
                {
                    return;
                }
            case wsAuditTrailComplianceTool.ResponseStatus.Failed:
            case wsAuditTrailComplianceTool.ResponseStatus.Error:
                {
                    string errorMessage = "Error writing audit logs.";
                    throw new Exception(errorMessage);
                }
            default:
                {
                    string errorMessage = "Unregistered AuditTrailcomplianceToolWS.ExecutionStatus " + addAuditTrailResponse.ResponseStatus.ToString() + ".";
                    throw new Exception(errorMessage);
                }
        }
    }

    #endregion
}