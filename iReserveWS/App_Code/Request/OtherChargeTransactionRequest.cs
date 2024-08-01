using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for OtherChargeTransactionRequest
/// </summary>
public class OtherChargeTransactionRequest
{
	public OtherChargeTransactionRequest()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    private int _type;

    public int Type
    {
        get { return _type; }
        set { _type = value; }
    }

    private OtherCharge _otherCharge;

    public OtherCharge OtherCharge
    {
        get { return _otherCharge; }
        set { _otherCharge = value; }
    }

    public OtherChargeTransactionResult Process()
    {
        OtherChargeTransactionResult returnValue = new OtherChargeTransactionResult();

        this.OtherCharge.TranOtherCharge(this.Type);

        returnValue.ResultStatus = ResultStatus.Successful;
        returnValue.Message = Messages.OtherChargeTransactionSuccessful;

        return returnValue;
    }
}