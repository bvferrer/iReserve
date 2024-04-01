using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for EmployeeDetails
/// </summary>
public class EmployeeDetails
{
    public EmployeeDetails()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Fields/Properties

    private string _employeeID;

    public string EmployeeID
    {
        get { return _employeeID; }
        set { _employeeID = value; }
    }

    private string _firstName;

    public string FirstName
    {
        get { return _firstName; }
        set { _firstName = value; }
    }

    private string _middleName;

    public string MiddleName
    {
        get { return _middleName; }
        set { _middleName = value; }
    }

    private string _lastName;

    public string LastName
    {
        get { return _lastName; }
        set { _lastName = value; }
    }

    private string _email;

    public string Email
    {
        get { return _email; }
        set { _email = value; }
    }

    private string _costCenter;

    public string CostCenter
    {
        get { return _costCenter; }
        set { _costCenter = value; }
    }

    private string _agency;

    public string Agency
    {
        get { return _agency; }
        set { _agency = value; }
    }

    private string _group;

    public string Group
    {
        get { return _group; }
        set { _group = value; }
    }

    private string _division;

    public string Division
    {
        get { return _division; }
        set { _division = value; }
    }

    private string _department;

    public string Department
    {
        get { return _department; }
        set { _department = value; }
    }

    private string _position;

    public string Position
    {
        get { return _position; }
        set { _position = value; }
    }

    private string _employmentType;

    public string EmploymentType
    {
        get { return _employmentType; }
        set { _employmentType = value; }
    }

    private string _employmentTypeDescription;

    public string EmploymentTypeDescription
    {
        get { return _employmentTypeDescription; }
        set { _employmentTypeDescription = value; }
    }

    private string _supervisorID;

    public string SupervisorID
    {
        get { return _supervisorID; }
        set { _supervisorID = value; }
    }

    private string _supervisorName;

    public string SupervisorName
    {
        get { return _supervisorName; }
        set { _supervisorName = value; }
    }

    private string _supervisorEmail;

    public string SupervisorEmail
    {
        get { return _supervisorEmail; }
        set { _supervisorEmail = value; }
    }

    #endregion

    #region Methods

    public void GetRequestorDetails(string employeeID)
    {
        using (SqlConnection sqlConnection = new SqlConnection(Settings.iReserveConnectionStringReader))
        {
            using (SqlCommand sqlCommand = new SqlCommand(Common.usp_RetrieveRequestorDetails, sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@empID", RDFramework.Utility.Conversion.SafeSetDatabaseValue<string>(employeeID));

                using (SqlDataReader rd = sqlCommand.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        this.Email = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_Email"]);
                        this.SupervisorEmail = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_SupervisorEmail"]);
                        this.Group = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_Group"]);
                        this.Agency = RDFramework.Utility.Conversion.SafeReadDatabaseValue<string>(rd["fld_Agency"]);
                    }
                }
            }
        }
    }

    #endregion
}