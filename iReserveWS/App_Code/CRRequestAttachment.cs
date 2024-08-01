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

/// <summary>
/// Summary description for CRRequestAttachment
/// </summary>
public class CRRequestAttachment
{
    public CRRequestAttachment()
    {
        this.Status = new Status();
    }

    #region Properties

    private int _attachmentID;

    public int AttachmentID
    {
        get { return _attachmentID; }
        set { _attachmentID = value; }
    }

    private string _requestReferenceNo;

    public string RequestReferenceNo
    {
        get { return _requestReferenceNo; }
        set { _requestReferenceNo = value; }
    }

    private Status _status;

    public Status Status
    {
        get { return _status; }
        set { _status = value; }
    }

    private string _fileName;

    public string FileName
    {
        get { return _fileName; }
        set { _fileName = value; }
    }
    private string _fileType;

    public string FileType
    {
        get { return _fileType; }
        set { _fileType = value; }
    }
    private int _fileSize;

    public int FileSize
    {
        get { return _fileSize; }
        set { _fileSize = value; }
    }
    private byte[] _file;

    public byte[] File
    {
        get { return _file; }
        set { _file = value; }
    }

    #endregion

    #region Methods

    public void InsertCRRequestAttachment()
    {
        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringWriter))
        {
            sqlConnection.Open();

            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(Common.usp_InsertCRRequestAttachment, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.RequestReferenceNo));
                    sqlCommand.Parameters.AddWithValue("@statusCode", this.Status.StatusCode);
                    sqlCommand.Parameters.AddWithValue("@fileName", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.FileName));
                    sqlCommand.Parameters.AddWithValue("@fileType", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.FileType));
                    sqlCommand.Parameters.AddWithValue("@fileSize", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.FileSize));
                    sqlCommand.Parameters.AddWithValue("@file", this.File);
                    sqlCommand.ExecuteNonQuery();
                }
            }

            catch
            {
                throw;
            }
        }
    }

    public List<CRRequestAttachment> RetrieveCRRequestAttachment()
    {
        List<CRRequestAttachment> attachmentList = new List<CRRequestAttachment>();

        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            using (SqlCommand sqlCommand = new SqlCommand(Common.usp_RetrieveCRRequestAttachment, sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.RequestReferenceNo));

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        CRRequestAttachment attachment = new CRRequestAttachment();
                        attachment.AttachmentID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_AttachmentID"]);
                        attachment.RequestReferenceNo = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RequestReferenceNo"]);
                        attachment.Status.StatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StatusCode"]);
                        attachment.Status.StatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StatusName"]);
                        attachment.FileName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_FileName"]);
                        attachment.FileType = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_FileType"]);
                        attachment.FileSize = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_FileSize"]);
                        attachment.File = (byte[])rd["fld_File"];
                        attachmentList.Add(attachment);
                    }
                }
            }
        }

        return attachmentList;
    }

    public void RetrieveCRRequestAttachmentByStatus(CRRequest request)
    {
        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            using (SqlCommand sqlCommand = new SqlCommand(Common.usp_RetrieveCRRequestAttachmentByStatus, sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(request.RequestReferenceNo));
                sqlCommand.Parameters.AddWithValue("@statusCode", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(request.Status.StatusCode));

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        this.AttachmentID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_AttachmentID"]);
                        this.RequestReferenceNo = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_RequestReferenceNo"]);
                        this.Status.StatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StatusCode"]);
                        this.Status.StatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StatusName"]);
                        this.FileName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_FileName"]);
                        this.FileType = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_FileType"]);
                        this.FileSize = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_FileSize"]);
                        this.File = (byte[])rd["fld_File"];
                    }
                }
            }
        }
    }

    #endregion
}