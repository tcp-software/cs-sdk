using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace CS_SDK_Tester
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        ShiftPlanning shiftPlanning;
        APIResponse response;
        DataFields session;

        string helpMessage =
@"Quick Access ShiftPlanning SDK Methods:
 * doLogin(RequestFields login_details)
 * doLogout()
 * getMessages()
 * getMessageDetails(int message_id)
 * createMessage(RequestFields message_data )
 * deleteMessage(int message_id)
 * getWallMessages()
 * createWallMessage(RequestFields message_data )
 * deleteWallMessage(message_id, WallMessageDeleteType wmt)
 * getEmployees()
 * getEmployeeDetails(int employee_id)
 * updateEmployee(RequestFields updated_employee_data )
 * createEmployee(RequestFields employee_data )
 * deleteEmployee(int employee_id)
 * getStaffSkills()
 * getStaffSkillDetails(int skill_id)
 * createStaffSkill(RequestFields skill_data)
 * updateStaffSkill(RequestFields skill_data )
 * deleteStaffSkill(intskill_id )
 * createPing(RequestFields ping_data )
 * getSchedules()
 * getScheduleDetails(RequestFields schedule_id)
 * createSchedule(RequestFields schedule_data )
 * updateSchedule(RequestFields schedule_data )
 * deleteSchedule(RequestFields schedule_id )
 * getShifts()
 * getShiftDetails(RequestFields shift_id)
 * updateShift(RequestFields shift_data)
 * createShift(RequestFields shift_data)
 * deleteShift(RequestFields shift_id)
 * getVacationSchedules(RequestFields period)
 * getVacationScheduleDetails(int schedule_id)
 * createVacationSchedule(RequestFields schedule_data)
 * updateVacationSchedule(RequestFields schedule_data)
 * deleteVacationSchedule(RequestFields schedule_id)
 * getScheduleConflicts()
 * getAdminSettings()
 * updateAdminSettings(RequestFields new_settings)
 * getAdminFiles()
 * getAdminFileDetails(ing file_id)
 * updateAdminFile(RequestFields file_data )
 * createAdminFile(RequestFields file_data )
 * deleteAdminFile(ing file_id)
 * getAdminBackups()
 * getAdminBackupDetails(int backup_id)
 * createAdminBackup(RequestFields backup_data)
 * deleteAdminBackup(int backup_id )
 * getAPIConfig()
 * getAPIMethods()

 All Quick-Access methods return a class: APIResponse

    Each APIRespose will have 2 Object

    1. Status - Response status of the API request (success/fail)

        Properties (Data Type: String)
            1. Code - Response code (Eg: 1 = Success)
            2. Text - Test Format of reponse code (Data Type: String)
            3. Error - Detailed error message (null value if request success)
            4. Token - Token for subsequent access

    2. Data - Response data of the API request

        Data will have multiple item
        Each item will have value 
        Some item may have Sub Data

        Example:             

        The method getShifts() responses will look like this, 
                where the indexes [1], [2] are serial number of each shift
        
            [1]
            |____ [id] 
            |____ [schedule_name]
            |____ ....
            |____ [employee]
                        |_____ [id]
                        |_____ [name]
                        |_____ ....
            [2]
            |____ [id] 
            |____ [schedule_name]
            |____ ....
            |____ [employee]
                        |_____ [id]
                        |_____ [name]
                        |_____ ....
            .....";

        // form load method
        private void Form1_Load(object sender, EventArgs e)
        {
            //load help message
            txtLog.AppendText(helpMessage);

            btnLogout.Enabled = false;
        }

        //Login to Session
        private void btnLogin_Click(object sender, EventArgs e)
        {
            //initialize SDK
            shiftPlanning = new ShiftPlanning(txtAPIKey.Text);
            if (shiftPlanning.getLoginStatus())
            {
            }

            try
            {
                //log
                txtLog.AppendText("\r\n\r\nMethod: doLogin");

                //login to session
                //preparing list of request field
                RequestFields login_details = new RequestFields();
                login_details.Add("username", txtUsername.Text);
                login_details.Add("password", txtPassword.Text);
                //calling method
                response = shiftPlanning.doLogin(login_details);

                //checking response
                if (response.Status.Code == "1")
                {//if login success
                    //get the current session 
                    session = shiftPlanning.getSession();
                    //update log
                    txtLog.AppendText("\r\nLogin " + response.Status.Text +
                        "\r\nWelcome " + response.Data["employee"].Item["name"].Value +
                        "\r\nBusiness Name: " + response.Data["business"].Item["name"].Value +
                        "\r\nToken: " + response.Status.Token);
                    btnLogout.Enabled = true;
                }
                else
                {
                    //update log
                    txtLog.AppendText("\r\n\r\nLogin " + response.Status.Text +
                           "\r\nError: " + response.Status.Error);
                }
            }
            catch (Exception ex)
            {
                txtLog.AppendText("\r\n\r\nException: " + ex.Message);
            }
        }


        //Logout from Session
        private void btnLogout_Click(object sender, EventArgs e)
        {
            //log
            txtLog.AppendText("\r\n\r\nMethod: doLogout");
            shiftPlanning.doLogout();
            this.Dispose();
        }

        //get list of employees
        private void btnGetEmployee_Click(object sender, EventArgs e)
        {
            try
            {
                //log
                txtLog.AppendText("\r\n\r\nMethod: getEmployees");

                //call method
                response = shiftPlanning.getEmployees();
                for (int i = 1; i <= response.Data.Count; i++)
                {
                    txtLog.AppendText("\r\n\r\nEmployee Id: " + response.Data[i.ToString()].Item["id"].Value +
                        "\r\nName: " + response.Data[i.ToString()].Item["name"].Value +
                        "\r\nemail:" + response.Data[i.ToString()].Item["email"].Value +
                        "\r\nwage:" + response.Data[i.ToString()].Item["wage"].Value);
                }
            }
            catch (Exception ex)
            {
                txtLog.AppendText("\r\n\r\nException: " + ex.Message);
            }
        }

        // get employee details by id
        private void btnGet1Employee_Click(object sender, EventArgs e)
        {
            try
            {
                //get first employee id
                response = shiftPlanning.getEmployees();
                int firstEmpId = int.Parse(response.Data["1"].Item["id"].Value);

                //log
                txtLog.AppendText("\r\n\r\nMethod: getEmployeeDetails");

                //get employee details by id
                response = shiftPlanning.getEmployeeDetails(firstEmpId);
                txtLog.AppendText("\r\nEmployee id:" + response.Data["id"].Value +
                       "\r\nName: " + response.Data["name"].Value +
                       "\r\nemail:" + response.Data["email"].Value +
                       "\r\nwage:" + response.Data["wage"].Value);
            }
            catch (Exception ex)
            {
                txtLog.AppendText("\r\n\r\nException: " + ex.Message);
            }
        }

        private void btnChangeWages_Click(object sender, EventArgs e)
        {
            try
            {
                //get first employee id
                response = shiftPlanning.getEmployees();
                int firstEmpId = int.Parse(response.Data["1"].Item["id"].Value);

                //log
                txtLog.AppendText("\r\n\r\nMethod: updateEmployee");

                string wage = Interaction.InputBox("Please enter the wages", "C# SDK", null, 0, 0);

                //preparing list of request field
                RequestFields employee_data = new RequestFields();
                employee_data.Add("id", firstEmpId);
                employee_data.Add("wage", wage);

                response = shiftPlanning.updateEmployee(employee_data);
                if (response.Status.Code == "1")
                    txtLog.AppendText("\r\nEmployee record updated.");
            }
            catch (Exception ex)
            {
                txtLog.AppendText("\r\n\r\nException: " + ex.Message);
            }
        }

        string shiftid;

        private void btnShiftDetails_Click(object sender, EventArgs e)
        {
            try
            {
                txtLog.AppendText("\r\n\r\nMethod: getShifts");

                RequestFields filter_fields = new RequestFields();
                //create filters
                //filter_fields.Add("schedule", 35316);
                response = shiftPlanning.getShifts(filter_fields);
                for (int i = 1; i <= response.Data.Count; i++)
                {
                    string msg;
                    msg = "\r\n\r\nShift Id: " + response.Data[i.ToString()].Item["id"].Value +
                    "\r\nShift's Schedule Name: " + response.Data[i.ToString()].Item["schedule_name"].Value;                        
                    txtLog.AppendText(msg);
                    if (i == 1) shiftid = response.Data[i.ToString()].Item["id"].Value;
                }
            }
            catch (Exception ex)
            {
                txtLog.AppendText("\r\n\r\nException: " + ex.Message);
            }

        }

        private void getShDetailById_Click(object sender, EventArgs e)
        {
            try
            {

                //get shift details
                string shift_id = Interaction.InputBox("Please enter shift id", "C# SDK", shiftid, 0, 0);

                txtLog.AppendText("\r\n\r\nMethod: getShiftDetails");

                string msg;
                response = shiftPlanning.getShiftDetails(int.Parse(shift_id));
                //checking response
                if (response.Status.Code == "1")
                {//if login success

                    msg = "\r\nShift Id:" + response.Data["id"].Value +
                        "\r\nShift's Schedule Name: " + response.Data["schedule_name"].Value +
                        "\r\nfrom: " + response.Data["start_time"].Item["time"].Value +
                        " to " + response.Data["end_time"].Item["time"].Value +
                        "\r\nstart date: " + response.Data["start_date"].Item["day"].Value +
                        "/" + response.Data["start_date"].Item["month"].Value +
                        "/" + response.Data["start_date"].Item["year"].Value +
                        "\r\nend date: " + response.Data["end_date"].Item["day"].Value +
                        "/" + response.Data["end_date"].Item["month"].Value +
                        "/" + response.Data["end_date"].Item["year"].Value;
                    if (response.Data["employees"].Item != null)
                    {//if employee assigned
                        msg += "\r\n  Employees assigned for this job:";
                        for (int i = 1; i <= response.Data["employees"].Item.Count; i++)
                        {
                            msg += "\r\n   " + i.ToString() + ". " + response.Data["employees"].Item[i.ToString()].Item["name"].Value;
                        }
                    }
                    else
                        msg += "\r\nNo employee assigned for this job";
                    txtLog.AppendText(msg);
                }
                else
                {
                    //update log
                    txtLog.AppendText("\r\nShift Details Method " + response.Status.Text +
                           "\r\nError: " + response.Status.Error);
                }
            }
            catch (Exception ex)
            {
                txtLog.AppendText("\r\n\r\nException: " + ex.Message);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFileName.Text = openFileDialog1.FileName;
            }
        }

        private void btnUploadFile_Click(object sender, EventArgs e)
        {
            try
            {
                txtLog.AppendText("\r\n\r\nMethod: createAdminFile");
                RequestFields fields = new RequestFields();
                fields.Add("filename", txtFileName.Text);
                response = shiftPlanning.createAdminFile(fields);
                txtLog.AppendText("\r\nCreate File Status: " + response.Status.Text);
            }
            catch (Exception ex)
            {
                txtLog.AppendText("\r\n\r\nException: " + ex.Message);
            }
        }


    }
}
