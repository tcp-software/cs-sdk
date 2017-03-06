/**
 * ShiftPlanning C# SDK
 * Version: 1.0
 * Written by: Meg Soft
 * Date: 20/Nov/2010
 * http://www.humanity.com/api/
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.IO;
using System.Web;

public class ShiftPlanning
{
    #region Variables

    //private variables
    private string _key = null;
    private string _token = null;
    private int _init = 0;
    private APIRequest request = null;
    private APIResponse response = null;
    private DataFields session = null;
    
    #endregion 

    #region Constants
    // constants
	const string api_endpoint = "https://www.humanity.com/api/";
	const string output_type = "xml";

    #endregion 
    
    #region Constructor

    public ShiftPlanning(string Key)
    {//construct the SDK
        this._key=Key;
    }

    #endregion

    #region AppKey

    public string getAppKey()
	{// return the developer key
		return this._key;
	}

    
	public string setAppKey( string key )
	{// set the developer key to use
		this._key = key;
		return this._key;
	}

    public string getAppToken()
    {// return the token that's currently being used
        try
        {//
            if (this.getSession() != null)
            {// user authenticated, return the token
                return this._token;
            }
            else
            {// user not authenticated, return an error
                throw new Exception(this.internal_errors(4));
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    #endregion

    #region File Handling

    private string getFileMimeType(string extension)
    {//returns mime for extension
        string mime;
        switch (extension)
        {
            case "ez": mime = "application/andrew-inset"; break;
            case "hqx": mime = "application/mac-binhex40"; break;
            case "cpt": mime = "application/mac-compactpro"; break;
            case "doc": mime = "application/msword"; break;
            case "bin": mime = "application/octet-stream"; break;
            case "dms": mime = "application/octet-stream"; break;
            case "lha": mime = "application/octet-stream"; break;
            case "lzh": mime = "application/octet-stream"; break;
            case "exe": mime = "application/octet-stream"; break;
            case "class": mime = "application/octet-stream"; break;
            case "so": mime = "application/octet-stream"; break;
            case "dll": mime = "application/octet-stream"; break;
            case "oda": mime = "application/oda"; break;
            case "pdf": mime = "application/pdf"; break;
            case "ai": mime = "application/postscript"; break;
            case "eps": mime = "application/postscript"; break;
            case "ps": mime = "application/postscript"; break;
            case "smi": mime = "application/smil"; break;
            case "smil": mime = "application/smil"; break;
            case "wbxml": mime = "application/vnd.wap.wbxml"; break;
            case "wmlc": mime = "application/vnd.wap.wmlc"; break;
            case "wmlsc": mime = "application/vnd.wap.wmlscriptc"; break;
            case "bcpio": mime = "application/x-bcpio"; break;
            case "vcd": mime = "application/x-cdlink"; break;
            case "pgn": mime = "application/x-chess-pgn"; break;
            case "cpio": mime = "application/x-cpio"; break;
            case "csh": mime = "application/x-csh"; break;
            case "dcr": mime = "application/x-director"; break;
            case "dir": mime = "application/x-director"; break;
            case "dxr": mime = "application/x-director"; break;
            case "dvi": mime = "application/x-dvi"; break;
            case "spl": mime = "application/x-futuresplash"; break;
            case "gtar": mime = "application/x-gtar"; break;
            case "hdf": mime = "application/x-hdf"; break;
            case "js": mime = "application/x-javascript"; break;
            case "skp": mime = "application/x-koan"; break;
            case "skd": mime = "application/x-koan"; break;
            case "skt": mime = "application/x-koan"; break;
            case "skm": mime = "application/x-koan"; break;
            case "latex": mime = "application/x-latex"; break;
            case "nc": mime = "application/x-netcdf"; break;
            case "cdf": mime = "application/x-netcdf"; break;
            case "sh": mime = "application/x-sh"; break;
            case "shar": mime = "application/x-shar"; break;
            case "swf": mime = "application/x-shockwave-flash"; break;
            case "sit": mime = "application/x-stuffit"; break;
            case "sv4cpio": mime = "application/x-sv4cpio"; break;
            case "sv4crc": mime = "application/x-sv4crc"; break;
            case "tar": mime = "application/x-tar"; break;
            case "tcl": mime = "application/x-tcl"; break;
            case "tex": mime = "application/x-tex"; break;
            case "texinfo": mime = "application/x-texinfo"; break;
            case "texi": mime = "application/x-texinfo"; break;
            case "t": mime = "application/x-troff"; break;
            case "tr": mime = "application/x-troff"; break;
            case "roff": mime = "application/x-troff"; break;
            case "man": mime = "application/x-troff-man"; break;
            case "me": mime = "application/x-troff-me"; break;
            case "ms": mime = "application/x-troff-ms"; break;
            case "ustar": mime = "application/x-ustar"; break;
            case "src": mime = "application/x-wais-source"; break;
            case "xhtml": mime = "application/xhtml+xml"; break;
            case "xht": mime = "application/xhtml+xml"; break;
            case "zip": mime = "application/zip"; break;
            case "au": mime = "audio/basic"; break;
            case "snd": mime = "audio/basic"; break;
            case "mid": mime = "audio/midi"; break;
            case "midi": mime = "audio/midi"; break;
            case "kar": mime = "audio/midi"; break;
            case "mpga": mime = "audio/mpeg"; break;
            case "mp2": mime = "audio/mpeg"; break;
            case "mp3": mime = "audio/mpeg"; break;
            case "aif": mime = "audio/x-aiff"; break;
            case "aiff": mime = "audio/x-aiff"; break;
            case "aifc": mime = "audio/x-aiff"; break;
            case "m3u": mime = "audio/x-mpegurl"; break;
            case "ram": mime = "audio/x-pn-realaudio"; break;
            case "rm": mime = "audio/x-pn-realaudio"; break;
            case "rpm": mime = "audio/x-pn-realaudio-plugin"; break;
            case "ra": mime = "audio/x-realaudio"; break;
            case "wav": mime = "audio/x-wav"; break;
            case "pdb": mime = "chemical/x-pdb"; break;
            case "xyz": mime = "chemical/x-xyz"; break;
            case "bmp": mime = "image/bmp"; break;
            case "gif": mime = "image/gif"; break;
            case "ief": mime = "image/ief"; break;
            case "jpeg": mime = "image/jpeg"; break;
            case "jpg": mime = "image/jpeg"; break;
            case "jpe": mime = "image/jpeg"; break;
            case "png": mime = "image/png"; break;
            case "tiff": mime = "image/tiff"; break;
            case "tif": mime = "image/tif"; break;
            case "djvu": mime = "image/vnd.djvu"; break;
            case "djv": mime = "image/vnd.djvu"; break;
            case "wbmp": mime = "image/vnd.wap.wbmp"; break;
            case "ras": mime = "image/x-cmu-raster"; break;
            case "pnm": mime = "image/x-portable-anymap"; break;
            case "pbm": mime = "image/x-portable-bitmap"; break;
            case "pgm": mime = "image/x-portable-graymap"; break;
            case "ppm": mime = "image/x-portable-pixmap"; break;
            case "rgb": mime = "image/x-rgb"; break;
            case "xbm": mime = "image/x-xbitmap"; break;
            case "xpm": mime = "image/x-xpixmap"; break;
            case "xwd": mime = "image/x-windowdump"; break;
            case "igs": mime = "model/iges"; break;
            case "iges": mime = "model/iges"; break;
            case "msh": mime = "model/mesh"; break;
            case "mesh": mime = "model/mesh"; break;
            case "silo": mime = "model/mesh"; break;
            case "wrl": mime = "model/vrml"; break;
            case "vrml": mime = "model/vrml"; break;
            case "css": mime = "text/css"; break;
            case "html": mime = "text/html"; break;
            case "htm": mime = "text/html"; break;
            case "asc": mime = "text/plain"; break;
            case "txt": mime = "text/plain"; break;
            case "rtx": mime = "text/richtext"; break;
            case "rtf": mime = "text/rtf"; break;
            case "sgml": mime = "text/sgml"; break;
            case "sgm": mime = "text/sgml"; break;
            case "tsv": mime = "text/tab-seperated-values"; break;
            case "wml": mime = "text/vnd.wap.wml"; break;
            case "wmls": mime = "text/vnd.wap.wmlscript"; break;
            case "etx": mime = "text/x-setext"; break;
            case "xml": mime = "text/xml"; break;
            case "xsl": mime = "text/xml"; break;
            case "mpeg": mime = "video/mpeg"; break;
            case "mpg": mime = "video/mpeg"; break;
            case "mpe": mime = "video/mpeg"; break;
            case "qt": mime = "video/quicktime"; break;
            case "mov": mime = "video/quicktime"; break;
            case "mxu": mime = "video/vnd.mpegurl"; break;
            case "avi": mime = "video/x-msvideo"; break;
            case "movie": mime = "video/x-sgi-movie"; break;
            case "ice": mime = "x-conference-xcooltalk"; break;
            default: 
                throw new Exception("Mime for ." + extension + " not found");
        }
        return mime;
    }

    private RequestFields getFileData(string filename)
    {// get file details, (data, length, mimetype)
        try
        {
            RequestFields file_data = new RequestFields();
            if (File.Exists(filename))
            {// file found
                byte[] file_content = File.ReadAllBytes(filename);
                file_data.Add("filepath", filename);
                file_data.Add("filelength", file_content.Length);
                string extension = filename.Substring(filename.LastIndexOf(".") + 1);
                file_data.Add("mimetype", getFileMimeType(extension));
                
                return file_data;
            }
            else
            {// file not found
                throw new Exception(this.internal_errors(8));
            }
        }
        catch (Exception e)
        {// any error
            throw e;
        }
    }


    #endregion

    #region Error Handling

    private string internal_errors( int errno )
	{// errors internal to the ShiftPlanning SDK
        string message ;
		switch( errno )
		{// internal error messages
			case 1 :
				message = "The requested API method was not found in this SDK.";
				break;
			case 2 :
				message  = "The ShiftPlanning API is not responding.";
				break;
			case 3 :
				message  = "You must use the login method before accessing other modules of this API.";
				break;
			case 4 :
				message  = "A session has not yet been established.";
				break;
			case 5 :
				message  = "You must specify your Developer Key when using this SDK.";
				break;
			case 6 :
				message  = "The ShiftPlanning SDK needs the CURL PHP extension.";
				break;
			case 7 :
				message  = "The ShiftPlanning SDK needs the JSON PHP extension.";
				break;
			case 8 :
				message  = "File doesn\'t exist.";
				break;
			case 9 :
				message  = "Could not find the correct mime for the file supplied.";
				break;
			default :
				message  = "Could not find the requested error message.";
				break;
		}
		return message;
    }

    #endregion

    #region Session

    public bool getLoginStatus()
    {//check whether a valid session has been established
        if (_token != null)
        {// user is already authenticated
            return true;
        }
        else
        {// user has not authenticated
            return false;
        }
    }


    public DataFields getSession()
    {// check whether a valid session has been established
        if (_token != null)
        {// user is already authenticated
            return this.session;
        }
        else
        {// user has not authenticated
            return null;
        }
    }

    private void setSession()
    {// store the user data to this session
        this._token = response.Status.Token;
        this.session = response.Data;
    }

    private void destroySession()
    {// destroy the currently active session
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "staff.logout");
        requestFields.Add("method", "GET");
        this.setRequest(requestFields);
        if (response.Status.Code == "1")
        {// logout successful, remove local session data
            this._token = null;
        }
    }

    #endregion

    #region Request & Response

    private string setRequest(RequestFields requestFields)
    {// set the request parameters
        // clear out previous request data
        this.request = new APIRequest();

        // set the default response type of JSON
        this.request.Output = output_type;
        this.request.Key = this._key;
        this.request.Token = this._token;
        this.request.RequestFields = requestFields;

        this._init = 0;
        if (requestFields["module"].ToString() == "staff.login") this._init = 1;
        
        return this.api();
    }


    private string api()
    {// create the api call
        if (this.getSession() != null)
        {// session already established, use token
            // remove the developer key from the request, since it's not necessary
            this.request.Key = null;
            // set the token for this request, since the user is already authenticated
            this.request.Token = _token;
        }
        else
        {// session has not been established, use developer key to access API
            try
            {//
                if (this._key != null)
                {// developer key is set
                    this.request.Key = this._key;
                }
                else
                {// developer key is not set
                    throw new Exception(this.internal_errors(5));
                }
            }
            catch (Exception e)
            {//
                throw e;
            }
        }
        // make the api request
        return this.perform_request();
    }

    private string perform_request()
    {
        Uri uri = new Uri(api_endpoint);

        string boundary = "----------" + DateTime.Now.Ticks.ToString("x");
        HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uri);
        webrequest.CookieContainer = new CookieContainer();
        webrequest.ContentType = "multipart/form-data; boundary=" + boundary;
        webrequest.Method = "POST";
        webrequest.Timeout = 120000;

        //check whether it is file upload
        string uploadfile = "";
        if (request.RequestFields.ContainsKey("filepath"))
        {//get the file name to upload
            uploadfile = request.RequestFields["filepath"].ToString();
            request.RequestFields.Remove("filepath");
        }

        //get JSON format string for 'data' parameter
        string data = request.GetEncoded();

        //prepare header for HTTP POST
        StringBuilder sb = new StringBuilder();
        sb.Append("--");
        sb.Append(boundary);
        sb.Append("\r\n");
        sb.Append("Content-Disposition: form-data; name=\"");
        sb.Append("data");
        sb.Append("\"");
        sb.Append("\r\n");
        sb.Append("\r\n");
        sb.Append(data);
        sb.Append("\r\n");

        if (uploadfile != "")
        {//if createfile function, prepare 'filedata' field
            sb.Append("--");
            sb.Append(boundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"");
            sb.Append("filedata");
            sb.Append("\"");
            sb.Append("\r\n");
            sb.Append("\r\n");
        }

        Stream requestStream = null; 
        try
        {
            string postHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            FileStream fileStream = null;
            long length = 0;

            if (uploadfile != "")
            {//if file upload, find the length of the file 
                fileStream = new FileStream(uploadfile, FileMode.Open, FileAccess.Read);
                length = postHeaderBytes.Length + fileStream.Length + boundaryBytes.Length;
            }
            else
            {//if no file upload, just get lenght of the 'data' field
                length = postHeaderBytes.Length;
            }

            webrequest.ContentLength = length;
            requestStream = webrequest.GetRequestStream();

            // Write out our post header
            requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

            if (uploadfile != "")
            {// if file upload, write out the file contents
                byte[] buffer = new Byte[checked((uint)Math.Min(4096, (int)fileStream.Length))];
                int bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    requestStream.Write(buffer, 0, bytesRead);

                // Write out the trailing boundary
                requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            }
           
        }
        finally
        {
            if (requestStream != null) requestStream.Close();
        }

        // Read the response  
        StreamReader reader = null;
        HttpWebResponse webResponse = (HttpWebResponse)webrequest.GetResponse();
        try
        {
            // Read the responce
            reader = new StreamReader(webResponse.GetResponseStream());

            if (webResponse.StatusCode == HttpStatusCode.OK)
            {
                // initiate new response object
                response = new APIResponse();
                // decode response
                response.DecodeResponse(reader);

                if (this._init == 1)
                {//if it is first call, get the 'token'
                    this.setSession();
                }
                return response.Status.Code;

                //TO DO: need to implement debug (to log.txt)
            }
            else
            {
                throw new Exception(this.internal_errors(2));
            }
        }
        finally
        {
            if (webResponse != null) webResponse.Close();
        }
    }

    #endregion

    #region Public API Methods

    #region User Authentication Methods

    /*
     * User Authentication Methods
     *
     */
    public APIResponse doLogin(RequestFields login_details)
    {// perform a login api call
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "staff.login");
        requestFields.Add("method", "GET");
        //append message fields
        foreach (KeyValuePair<string, object> fld in login_details)
            requestFields.Add(fld.Key, fld.Value);
        this.setRequest(requestFields);
        return response;
    }

    public void doLogout()
	{// erase token and user data from current session
		this.destroySession();
    }

    #endregion

    #region Message Methods

    #region Message
    /*
	 * Message Methods
	 *
	 */
    public APIResponse getMessages()
	{// get messages for the currently logged in user
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "messaging.messages");
        requestFields.Add("method", "GET");
        this.setRequest(requestFields);
        return response;
	}

    public APIResponse getMessageDetails(int id)
    {// get messages for the currently logged in user
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "messaging.message");
        requestFields.Add("method", "GET");
        requestFields.Add("id", id);
        this.setRequest(requestFields);
        return response;
    }

    public APIResponse createMessage(RequestFields message_details)
    {// create a new message
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "messaging.message");
        requestFields.Add("method", "CREATE");
        //append message fields
        foreach (KeyValuePair<string, object> fld in message_details)
            requestFields.Add(fld.Key, fld.Value);
        this.setRequest(requestFields);
        return response;
    }

    public APIResponse deleteMessage(int id)
	{// delete a message
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "messaging.message");
        requestFields.Add("method", "DELETE");
        requestFields.Add("id", id);
        this.setRequest(requestFields);
        return response;
    }

    #endregion

    #region Wall Messages

    public APIResponse getWallMessages()
    {// get messages for the currently logged in user
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "messaging.wall");
        requestFields.Add("method", "GET");
        this.setRequest(requestFields);
        return response;
    }

    public APIResponse createWallMessage(RequestFields message_details)
    {// create a new message
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "messaging.wall");
        requestFields.Add("method", "CREATE");
        //append message fields
        foreach (KeyValuePair<string, object> fld in message_details)
            requestFields.Add(fld.Key, fld.Value);
        this.setRequest(requestFields);
        return response;
    }

    public enum WallMessageDeleteType { Message, Comment };

    public APIResponse deleteWallMessage(int id, WallMessageDeleteType wmt)
    {// delete a message
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "messaging.wall");
        requestFields.Add("method", "DELETE");
        requestFields.Add("id", id);
        requestFields.Add("delete", wmt == WallMessageDeleteType.Comment ? "comment" : "message");
        this.setRequest(requestFields);
        return response;
    }

    #endregion

    #endregion

    #region Staff Methods

    #region Employee
    /*
	 * Staff Methods
	 *
	 */
    public APIResponse getEmployees()
	{// get a list of employees
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "staff.employees");
        requestFields.Add("method", "GET");
        this.setRequest(requestFields);
        return response;
	}

    public APIResponse getEmployeeDetails(int id)
    {// get employee details by id
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "staff.employee");
        requestFields.Add("method", "GET");
        requestFields.Add("id", id);
        this.setRequest(requestFields);
        return response;
    }


    public APIResponse updateEmployee(RequestFields employee_data)
    {// update a employee
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "staff.employee");
        requestFields.Add("method", "UPDATE");
        //append employee details fields
        foreach (KeyValuePair<string, object> fld in employee_data)
            requestFields.Add(fld.Key, fld.Value);
        this.setRequest(requestFields);
        return response;
    }

    public APIResponse createEmployee(RequestFields employee_data)
    {// create a employee
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "staff.employee");
        requestFields.Add("method", "CREATE");
        //append employee details fields
        foreach (KeyValuePair<string, object> fld in employee_data)
            requestFields.Add(fld.Key, fld.Value);
        this.setRequest(requestFields);
        return response;
    }

    public APIResponse deleteEmployee(int id)
    {// delete a employee by id
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "staff.employee");
        requestFields.Add("method", "DELETE");
        requestFields.Add("id", id);
        this.setRequest(requestFields);
        return response;
    }

    #endregion

    #region Staff Skills

    public APIResponse getStaffSkills()
    {// get a list of staff skills
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "staff.skills");
        requestFields.Add("method", "GET");
        this.setRequest(requestFields);
        return response;
    }

    public APIResponse getStaffSkillDetails(int id)
    {// get skill detail by id
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "staff.skill");
        requestFields.Add("method", "GET");
        requestFields.Add("id", id);
        this.setRequest(requestFields);
        return response;
    }


    public APIResponse updateStaffSkill(RequestFields skill_details)
    {// update staff skill
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "staff.skill");
        requestFields.Add("method", "UPDATE");
        //append skill fields
        foreach (KeyValuePair<string, object> fld in skill_details)
            requestFields.Add(fld.Key, fld.Value);

        this.setRequest(requestFields);
        return response;
    }

    public APIResponse createStaffSkill(RequestFields skill_details)
    {// create new staff skill
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "staff.skill");
        requestFields.Add("method", "CREATE");
        //append skill fields
        foreach (KeyValuePair<string, object> fld in skill_details)
            requestFields.Add(fld.Key, fld.Value);
        this.setRequest(requestFields);
        return response;
    }

    public APIResponse deleteStaffSkill(int id)
    {// delete a staff skill by id
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "staff.skill");
        requestFields.Add("method", "DELETE");
        requestFields.Add("id", id);
        this.setRequest(requestFields);
        return response;
    }

    #endregion

    public APIResponse createPing(RequestFields ping_data)
    {// create new staff skill
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "staff.ping");
        requestFields.Add("method", "CREATE");
        //append skill fields
        foreach (KeyValuePair<string, object> fld in ping_data)
            requestFields.Add(fld.Key, fld.Value);
        this.setRequest(requestFields);
        return response;
    }

    #endregion

    #region Schedules Methods

    #region Schedule

    /*
	 * Schedules Methods
	 *
	 */
    public APIResponse getSchedules()
    {// get a list of schedules
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "schedule.schedules");
        requestFields.Add("method", "GET");
        this.setRequest(requestFields);
        return response;
    }

    public APIResponse getScheduleDetails(int id)
    {// get schedule details by id
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "schedule.schedule");
        requestFields.Add("method", "GET");
        requestFields.Add("id", id);
        this.setRequest(requestFields);
        return response;
    }


    public APIResponse updateSchedule(RequestFields schedule_details)
    {// update a schedule
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "schedule.schedule");
        requestFields.Add("method", "UPDATE");
        //append employee details fields
        foreach (KeyValuePair<string, object> fld in schedule_details)
            requestFields.Add(fld.Key, fld.Value);
        this.setRequest(requestFields);
        return response;
    }

    public APIResponse createSchedule(RequestFields schedule_details)
    {// create a schedule
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "schedule.schedule");
        requestFields.Add("method", "CREATE");
        //append employee details fields
        foreach (KeyValuePair<string, object> fld in schedule_details)
            requestFields.Add(fld.Key, fld.Value);
        this.setRequest(requestFields);
        return response;
    }

    public APIResponse deleteSchedule(int id)
    {// delete a schedule by id
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "schedule.schedule");
        requestFields.Add("method", "DELETE");
        requestFields.Add("id", id);
        this.setRequest(requestFields);
        return response;
    }

    #endregion

    #region Shifts

    public APIResponse getShifts(RequestFields filter)
    {// get a list of shift
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "schedule.shifts");
        requestFields.Add("method", "GET");
        //append fields
        foreach (KeyValuePair<string, object> fld in filter)
            requestFields.Add(fld.Key, fld.Value);
        this.setRequest(requestFields);
        return response;
    }

    public APIResponse getShiftDetails(int id)
    {// get shift detail by id
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "schedule.shift");
        requestFields.Add("method", "GET");
        requestFields.Add("id", id);
        this.setRequest(requestFields);
        return response;
    }


    public APIResponse updateShift(RequestFields shift_details)
    {// update shift
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "schedule.shift");
        requestFields.Add("method", "UPDATE");
        //append fields
        foreach (KeyValuePair<string, object> fld in shift_details)
            requestFields.Add(fld.Key, fld.Value);

        this.setRequest(requestFields);
        return response;
    }

    public APIResponse createShift(RequestFields shift_details)
    {// create new shift
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "schedule.shift");
        requestFields.Add("method", "CREATE");
        //append skill fields
        foreach (KeyValuePair<string, object> fld in shift_details)
            requestFields.Add(fld.Key, fld.Value);
        this.setRequest(requestFields);
        return response;
    }

    public APIResponse deleteShift(int id)
    {// delete a shift by id
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "schedule.shift");
        requestFields.Add("method", "DELETE");
        requestFields.Add("id", id);
        this.setRequest(requestFields);
        return response;
    }

    #endregion

    #region Vacation Schedules

    public APIResponse getVacationSchedules(RequestFields time_period)
    {// get schedule vacations, pass start and end params to get vacations within a certian time-period
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "schedule.vacations");
        requestFields.Add("method", "GET");
        //append fields
        foreach (KeyValuePair<string, object> fld in time_period)
            requestFields.Add(fld.Key, fld.Value);
        this.setRequest(requestFields);
        return response;
    }

    public APIResponse getVacationScheduleDetails(int id)
    {// get VacationSchedule detail by id
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "schedule.vacation");
        requestFields.Add("method", "GET");
        requestFields.Add("id", id);
        this.setRequest(requestFields);
        return response;
    }


    public APIResponse updateVacationSchedule(RequestFields vacation_details)
    {// update VacationSchedule
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "schedule.vacation");
        requestFields.Add("method", "UPDATE");
        //append fields
        foreach (KeyValuePair<string, object> fld in vacation_details)
            requestFields.Add(fld.Key, fld.Value);

        this.setRequest(requestFields);
        return response;
    }

    public APIResponse createVacationSchedule(RequestFields vacation_details)
    {// create new VacationSchedule
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "schedule.vacation");
        requestFields.Add("method", "CREATE");
        //append skill fields
        foreach (KeyValuePair<string, object> fld in vacation_details)
            requestFields.Add(fld.Key, fld.Value);
        this.setRequest(requestFields);
        return response;
    }

    public APIResponse deleteVacationSchedule(int id)
    {// delete a VacationSchedule by id
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "schedule.vacation");
        requestFields.Add("method", "DELETE");
        requestFields.Add("id", id);
        this.setRequest(requestFields);
        return response;
    }

    public APIResponse getScheduleConflicts(RequestFields fields)
    {// get schedule conflicts
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "schedule.conflicts");
        requestFields.Add("method", "GET");
        //append time_period fields
        foreach (KeyValuePair<string, object> fld in fields)
            requestFields.Add(fld.Key, fld.Value);
        this.setRequest(requestFields);
        return response;
    }

    #endregion

    #endregion

    #region Administration Methods

    /*
	 * Administration Methods
	 *
	 */

    #region Settings

    public APIResponse getAdminSettings()
    {// get admin settings
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "admin.settings");
        requestFields.Add("method", "GET");
        this.setRequest(requestFields);
        return response;
    }

    public APIResponse updateAdminSettings(RequestFields settings)
    {// update admin settings
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "admin.settings");
        requestFields.Add("method", "UPDATE");
        //append fields
        foreach (KeyValuePair<string, object> fld in settings)
            requestFields.Add(fld.Key, fld.Value);
        this.setRequest(requestFields);
        return response;
    }

    #endregion

    #region Files

    public APIResponse getAdminFiles()
    {// get admin files
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "admin.files");
        requestFields.Add("method", "GET");
        this.setRequest(requestFields);
        return response;
    }

    public APIResponse getAdminFileDetails(int id)
    {// get admin file details
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "admin.file");
        requestFields.Add("method", "GET");
        requestFields.Add("id", id);
        this.setRequest(requestFields);
        return response;
    }

    public APIResponse updateAdminFile(RequestFields details)
    {// update admin file details
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "admin.file");
        requestFields.Add("method", "UPDATE");
        //append employee details fields
        foreach (KeyValuePair<string, object> fld in details)
            requestFields.Add(fld.Key, fld.Value);
        this.setRequest(requestFields);
        return response;
    }

    public APIResponse createAdminFile(RequestFields details)
    {// create new admin file
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "admin.file");
        requestFields.Add("method", "CREATE");
        
        //append details
        foreach (KeyValuePair<string, object> fld in details)
            requestFields.Add(fld.Key, fld.Value);
        
        //append filedetails
        RequestFields file_details = getFileData(details["filename"].ToString());
        details["filename"] = Path.GetFileName(details["filename"].ToString());
        foreach (KeyValuePair<string, object> fld in file_details)
            requestFields.Add(fld.Key, fld.Value);
        this.setRequest(requestFields);
        return response;
    }

    public APIResponse deleteAdminFile(int id)
    {// delete a employee by id
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "admin.file");
        requestFields.Add("method", "DELETE");
        requestFields.Add("id", id);
        this.setRequest(requestFields);
        return response;
    }


    #endregion

    #region Backup Files

    public APIResponse getAdminBackups()
    {// get admin files
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "admin.backups");
        requestFields.Add("method", "GET");
        this.setRequest(requestFields);
        return response;
    }

    public APIResponse getAdminBackupDetails(int id)
    {// get admin file details
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "admin.backup");
        requestFields.Add("method", "GET");
        requestFields.Add("id", id);
        this.setRequest(requestFields);
        return response;
    }

    public APIResponse updateAdminBackup(RequestFields details)
    {// update admin file details
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "admin.backup");
        requestFields.Add("method", "UPDATE");
        //append employee details fields
        foreach (KeyValuePair<string, object> fld in details)
            requestFields.Add(fld.Key, fld.Value);
        this.setRequest(requestFields);
        return response;
    }

    public APIResponse createAdminBackup(RequestFields details)
    {// create new admin file
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "admin.backup");
        requestFields.Add("method", "CREATE");
        //append details
        foreach (KeyValuePair<string, object> fld in details)
            requestFields.Add(fld.Key, fld.Value);
        //append filedetails
        RequestFields file_details = getFileData(details["filename"].ToString());
        foreach (KeyValuePair<string, object> fld in file_details)
            requestFields.Add(fld.Key, fld.Value);
        this.setRequest(requestFields);
        return response;
    }

    public APIResponse deleteAdminBackup(int id)
    {// delete a employee by id
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "admin.file");
        requestFields.Add("method", "DELETE");
        requestFields.Add("id", id);
        this.setRequest(requestFields);
        return response;
    }


    #endregion

    #endregion

    #region API Methods

    /*
	 * API Methods
	 *
	 */

    public APIResponse getAPIConfig()
    {// get API config
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "api.config");
        requestFields.Add("method", "GET");
        this.setRequest(requestFields);
        return response;
    }

    public APIResponse getAPIMethods()
    {// get API methods
        RequestFields requestFields = new RequestFields();
        requestFields.Add("module", "api.methods");
        requestFields.Add("method", "GET");
        this.setRequest(requestFields);
        return response;
    }


    #endregion

    #endregion
}

