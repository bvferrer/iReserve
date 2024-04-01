using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for ConnectionStringKey
/// </summary>
public class ConnectionStringKey
{
	public ConnectionStringKey()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    #region Fields/Properties

    private string _connectionStringName;
    public string ConnectionStringName
    {
        get { return _connectionStringName; }
        set { _connectionStringName = value; }
    }

    #endregion
}