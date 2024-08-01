using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for AACFUserProfile
/// </summary>
public class AACFUserProfile
{
    public AACFUserProfile()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Fields
    private string _UserName;
    private string _UserAccess;
    private string _FailedAttempts;
    private string _HasLoggedIn;
    private string _PasswordExpiryDate;
    private string _Profiles;
    private string _FirstName;
    private string _MiddleName;
    private string _LastName;
    private string _Unit;
    private string _IsLocked;
    #endregion

    #region Properties
    public string UserName
    {
        get { return _UserName; }
        set { _UserName = value; }
    }
    public string UserAccess
    {
        get { return _UserAccess; }
        set { _UserAccess = value; }
    }
    public string FailedAttempts
    {
        get { return _FailedAttempts; }
        set { _FailedAttempts = value; }
    }
    public string HasLoggedIn
    {
        get { return _HasLoggedIn; }
        set { _HasLoggedIn = value; }
    }
    public string PasswordExpiryDate
    {
        get { return _PasswordExpiryDate; }
        set { _PasswordExpiryDate = value; }
    }
    public string Profiles
    {
        get { return _Profiles; }
        set { _Profiles = value; }
    }
    public string FirstName
    {
        get { return _FirstName; }
        set { _FirstName = value; }
    }
    public string MiddleName
    {
        get { return _MiddleName; }
        set { _MiddleName = value; }
    }
    public string LastName
    {
        get { return _LastName; }
        set { _LastName = value; }
    }
    public string Unit
    {
        get { return _Unit; }
        set { _Unit = value; }
    }
    public string IsLocked
    {
        get { return _IsLocked; }
        set { _IsLocked = value; }
    }
    #endregion

}