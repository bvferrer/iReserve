using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using AppCryptor;
using AESCryptor;

/// <summary>
/// Summary description for PasswordRecoveryInfo
/// </summary>
public class PasswordRecoveryInfo
{
	public PasswordRecoveryInfo()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    #region Fields

    private string _Question1;
    private string _Question2;
    private string _Question3;
    private string _Answer1;
    private string _Answer2;
    private string _Answer3;
    private string _Email;
    private string _Password;

    #endregion

    #region Properties

    public string Question1
    {
        get { return _Question1; }
        set { _Question1 = value; }
    }

    public string Question2
    {
        get { return _Question2; }
        set { _Question2 = value; }
    }

    public string Question3
    {
        get { return _Question3; }
        set { _Question3 = value; }
    }

    public string Answer1
    {
        get { return _Answer1; }
        set { _Answer1 = value; }
    }

    public string Answer2
    {
        get { return _Answer2; }
        set { _Answer2 = value; }
    }

    public string Answer3
    {
        get { return _Answer3; }
        set { _Answer3 = value; }
    }

    public string Email
    {
        get { return _Email; }
        set { _Email = value; }
    }

    public string Password
    {
        get { return _Password; }
        set { _Password = value; }
    }

    #endregion
}