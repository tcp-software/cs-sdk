using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Xml;

public class ResponseStatus
{
    #region Private Variables

    private string _code = null;
    private string _text = null;
    private string _token = null;
    private string _error = null;

    #endregion

    #region Properties

    public string Code
    {
        get { return _code; }
        set { _code = value; }
    }

    public string Text
    {
        get { return _text; }
        set { _text = value; }
    }

    public string Token
    {
        get { return _token; }
        set { _token = value; }
    }

    public string Error
    {
        get { return _error; }
        set { _error = value; }
    }

    #endregion
}

public class APIResponse
{

    #region Fields
    private ResponseStatus _ResponseStatus;
    private DataFields _ResponseData;
    #endregion

    #region Property

    public ResponseStatus Status
    {
        get { return _ResponseStatus; }
        set { _ResponseStatus = value; }
    }

    public DataFields Data
    {
        get { return _ResponseData; }
        set { _ResponseData = value; }
    }
        

    #endregion

    #region Methods

    public void DecodeResponse(StreamReader xmlResponse)
    {
        _ResponseStatus = new ResponseStatus();
        _ResponseData = new DataFields();

        string xmlData = xmlResponse.ReadToEnd();
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xmlData);
        
        XmlNodeList nodeList = xmlDocument.SelectNodes("/response");

        foreach (XmlNode xmlNode in nodeList[0].ChildNodes)
        {
            switch (xmlNode.Name)
            {
                case "status":
                    _ResponseStatus.Code = xmlNode.InnerText;
                    _ResponseStatus.Text = getResponseText(xmlNode.InnerText);
                    break;
                case "token":
                    _ResponseStatus.Token = xmlNode.InnerText;
                    break;
                case "error":
                    _ResponseStatus.Error = xmlNode.InnerText;
                    break;
                case "data":
                    _ResponseData = NodeToDataFields(xmlNode.ChildNodes);
                    break;
                default: 
                    break;
            }
        }

    }

    private DataFields NodeToDataFields(XmlNodeList xmlNodes)
    {
        int i = 0;
        string itemKey;
        DataFields df = new DataFields();
        foreach (XmlNode dataNode in xmlNodes)
        {
            if (dataNode.Name == "item")
            {
                i++;
                try
                {
     			itemKey = dataNode.Attributes["id"].Value;
		}
		catch
		(Exception noAttr)
		{
    			itemKey = i.ToString();
    		}
            }
            else
            {
                itemKey = dataNode.Name;
            }
            if (dataNode.ChildNodes.Count <= 1)
            {
                DataItem item = new DataItem(dataNode.InnerText, null);
                df.Add(itemKey, item);
            }
            else
            {
                DataItem di = new DataItem();
                DataFields dfs = new DataFields();
                dfs = NodeToDataFields(dataNode.ChildNodes);
                di = new DataItem(dataNode.InnerText, dfs);
                df.Add(itemKey, di);
            }
        }
        return df;
    }
    
    
    private string getResponseText( string code )
	{// return a reason text for a response code
        string reason = null;
		switch( code )
		{// select a response code to display
			case "-3" : reason = "Flagged API Key - Pemanently Banned"; break;
			case "-2" : reason = "Flagged API Key - Too Many invalid access attempts - contact us"; break;
			case "-1" : reason = "Flagged API Key - Temporarily Disabled - contact us"; break;
			case "1" : reason = "Success -"; break;
			case "2" : reason = "Invalid API key - App must be granted a valid key by ShiftPlanning"; break;
			case "3" : reason = "Invalid token key - Please re-authenticate"; break;
			case "4" : reason = "Invalid Method - No Method with that name exists in our API"; break;
			case "5" : reason = "Invalid Module - No Module with that name exists in our API"; break;
			case "6" : reason = "Invalid Action - No Action with that name exists in our API"; break;
			case "7" : reason = "Authentication Failed - You do not have permissions to access the service"; break;
			case "8" : reason = "Missing parameters - Your request is missing a required parameter"; break;
			case "9" : reason = "Invalid parameters - Your request has an invalid parameter type"; break;
			case "10" : reason = "Extra parameters - Your request has an extra/unallowed parameter type"; break;
			case "12" : reason = "Create Failed - Your CREATE request failed"; break;
			case "13" : reason = "Update Failed - Your UPDATE request failed"; break;
			case "14" : reason = "Delete Failed - Your DELETE request failed"; break;
			case "20" : reason = "Incorrect Permissions - You don\'t have the proper permissions to access this"; break;
			case "90" : reason = "Suspended API key - Access for your account has been suspended, please contact ShiftPlanning"; break;
			case "91" : reason = "Throttle exceeded - You have exceeded the max allowed requests. Try again later."; break;
			case "98" : reason = "Bad API Paramaters - Invalid POST request. See Manual."; break;
			case "99" : reason = "Service Offline - This service is temporarily offline. Try again later."; break;
			default : reason = "Error code not found"; break;
		}
		// return the reason text
		return reason;
	}
    #endregion
}
