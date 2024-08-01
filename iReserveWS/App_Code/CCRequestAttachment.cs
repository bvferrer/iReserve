using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for CCRequestAttachment
/// </summary>
public class CCRequestAttachment
{
    public CCRequestAttachment()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Properties

    private int _ccAttachmentID;

    public int CCAttachmentID
    {
        get { return _ccAttachmentID; }
        set { _ccAttachmentID = value; }
    }

    private string _ccRequestReferenceNo;

    public string CCRequestReferenceNo
    {
        get { return _ccRequestReferenceNo; }
        set { _ccRequestReferenceNo = value; }
    }

    private int _statusCode;

    public int StatusCode
    {
        get { return _statusCode; }
        set { _statusCode = value; }
    }

    private string _statusName;

    public string StatusName
    {
        get { return _statusName; }
        set { _statusName = value; }
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

    public void InsertCCRequestAttachment(SqlConnection sqlConnection)
    {
        using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.InsertCCRequestAttachment, sqlConnection))
        {
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.CCRequestReferenceNo));
            sqlCommand.Parameters.AddWithValue("@statusCode", this.StatusCode);
            sqlCommand.Parameters.AddWithValue("@fileName", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.FileName));
            sqlCommand.Parameters.AddWithValue("@fileType", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(this.FileType));
            sqlCommand.Parameters.AddWithValue("@fileSize", RDFramework.Utility.Conversion.SafeSetDatabaseValue<int>(this.FileSize));
            sqlCommand.Parameters.AddWithValue("@file", this.File);
            sqlCommand.ExecuteNonQuery();
        }
    }

    public List<CCRequestAttachment> RetrieveCCRequestAttachment(string ccRequestReferenceNo)
    {
        List<CCRequestAttachment> attachmentList = new List<CCRequestAttachment>();

        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveCCRequestAttachment, sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(ccRequestReferenceNo));

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        CCRequestAttachment attachment = new CCRequestAttachment();
                        attachment.CCAttachmentID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_CCAttachmentID"]);
                        attachment.CCRequestReferenceNo = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CCRequestReferenceNo"]);
                        attachment.StatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StatusCode"]);
                        attachment.StatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StatusName"]);
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

    public void RetrieveCCRequestAttachmentByStatus(string ccRequestReferenceNo, int statusCode)
    {
        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            using (SqlCommand sqlCommand = new SqlCommand(StoredProcedures.RetrieveCCRequestAttachmentByStatus, sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@refNo", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(ccRequestReferenceNo));
                sqlCommand.Parameters.AddWithValue("@statusCode", statusCode);

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        this.CCAttachmentID = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_CCAttachmentID"]);
                        this.CCRequestReferenceNo = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_CCRequestReferenceNo"]);
                        this.StatusCode = RDFramework.Utility.Conversion.SafeReadDatabaseValue<int>(rd["fld_StatusCode"]);
                        this.StatusName = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_StatusName"]);
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