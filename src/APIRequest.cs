using System;
using System.Collections.Generic;
using System.Text;

class APIRequest
{

    #region Fields
    string _key;
    string _token;
    string _output;
    RequestFields _requestFields;
    #endregion

    #region Property

    public RequestFields RequestFields
    {
        get { return _requestFields; }
        set { _requestFields = value; }
    }


    public string Key
    {
        get { return _key; }
        set { _key = value; }
    }

    public string Token
    {
        get { return _token; }
        set { _token = value; }
    }

    public string Output
    {
        get { return _output; }
        set { _output = value; }
    }

    #endregion

    #region Methods

    public string GetEncoded()
    {
        string json = @"{""key"": """ + _key +
                        @""", ""token"": """ + _token +
                        @""", ""output"": """ + _output +
                        @""", ""request"": {";
                        
        if (_requestFields.Count > 0)
        {
            foreach (KeyValuePair<string, object> item in _requestFields)
            {
                if (item.Value.GetType().ToString() == "System.String")
                    json += @"""" + item.Key + @""": """ + item.Value + @""",";
                else
                    json += @"""" + item.Key + @""": " + item.Value + ",";
            }
            json = json.Substring(0, json.Length - 1);
        }
        json += "} }";
        return json;
    }

    #endregion
}
