using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using FIC.LD.TrainingEvaluation.SP.Solution.Helper;
using Microsoft.SharePoint;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FIC.LD.TrainingEvaluation.SP.Solution.isdaform
{
    public partial class isdaformUserControl : UserControl
    {
        #region Private Variables

        private const string LearningDevelopmentAdminTeam = "LearningDevelopmentAdminTeam";
        private const string LearningDevelopmentManagersTeam = "LearningDevelopmentManagersTeam";

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Login for the current user
        /// </summary>
        private string LoginName
        {
            get
            {
                string LoginName = SPContext.Current.Web.CurrentUser.LoginName.Split('\\').ElementAtOrDefault(1);
                return LoginName;
            }
        }

        /// <summary>
        /// Gets the current User
        /// </summary>
        private SPUser SPUserLoginName
        {
            get
            {
                SPUser LoginName = SPContext.Current.Web.CurrentUser;
                return LoginName;
            }
        }

        #endregion

        #region Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            //Hide Error message container
            ErrorMessageDiv.Visible = false;
            Encryption decrypt = new Encryption();

            // Set the ThresHoldValueLabel
            SetThresHoldValueLabel();

            SetDivsToHidden();

            try
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["w"] != null)
                        SetupForm();

                    if (Request.QueryString["i"] != null)
                        RejectButton_Click(sender, e);
                }

                SetEmployeeDetails();
                SetEmployeeManagerANDDesignation();
            }
            catch (Exception ex)
            {
                MainContent.Visible = false;
                ErrorMessageDiv.Visible = true;
                ErrorMessageDiv.InnerText = ex.Message;
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// If QueryString is not null setup the Form
        /// </summary>
        protected void SetupForm()
        {
            Encryption decrypt = new Encryption();

            try
            {
                if (Request.QueryString["w"] != null)
                {
                    string Status = string.Empty;
                    string TypeOfTraining = string.Empty;

                    // reference the Encryption Instance                        
                    ViewState["INTERVENTIONID"] = decrypt.Decrypt(Request.QueryString["w"]);

                    if (ViewState["INTERVENTIONID"] != null)
                    {
                        if (IsInterventionValid(Convert.ToString(ViewState["INTERVENTIONID"])))
                        {
                            // Assign the InterventionID to the Local variable
                            int InterventionID = Convert.ToInt32(ViewState["INTERVENTIONID"]);

                            // Sets the InterventionID to a hiddenfield
                            InterventionIDHidden.Value = Convert.ToString(InterventionID);

                            // Sets the Controls values
                            var tuple = SetEmployeeIntervention(InterventionID);

                            // If the Current User is not the Employee Intervention or Intervention Manager or Member of LDTrainingTeam - deny access
                            IsUserAuthorized();

                            Status = tuple.Item1;
                            TypeOfTraining = tuple.Item2;

                            // Sets the Intervention Status to a hiddenfield
                            InterventionStatusHidden.Value = Status;

                            if (Status == "Assigned To Manager" && IsInterventionForEmployee())
                            {
                                // Show or Hide Workback Section if the Cost is above the Threshold                                
                                //GenericSkillsForm.Visible = true;
                                EmployeeSubmit.Visible = true;
                                ShowContractLink(TypeOfTraining);
                            }

                            if (Status == "Assigned To Manager" && IsUserEmployeeManager())
                            {
                                DisableEmployeeDetails();
                                ManagerStep2DIV.Visible = true;
                                LineManagerApproveButton.Visible = true;
                                EmployeeSubmit.Visible = false;
                            }

                            if (Status == "Assigned To L&D Manager")
                            {
                                DisableEmployeeDetails();
                                ManagerStep2DIV.Visible = true;

                                SetManagerInterventionExpectation(InterventionID);

                                SetManagerInterventionApproval(InterventionID);

                                if (!IsUserEmployeeManager())
                                    DisableManagerDetails();

                                if (IsUserMemberOfLDApproverSPGroup(SPContext.Current.Web.CurrentUser, LearningDevelopmentManagersTeam))
                                {
                                    LDManagerStep3DIV.Visible = true;
                                    SetLDManagerInterventionApproval(InterventionID);
                                    SetLDEmployeeIntervention(InterventionID);
                                    LDManagerApproveButton.Visible = true;
                                }
                            }

                            if (Status == "Learning Need Approved")
                            {
                                DisableEmployeeDetails();
                                DisableManagerDetails();
                                DisableLDManagerDetails();
                                ManagerStep2DIV.Visible = true;
                                LDManagerStep3DIV.Visible = true;

                                if (IsUserMemberOfLDAdminSPGroup(SPContext.Current.Web.CurrentUser, LearningDevelopmentAdminTeam))
                                {
                                    PreparationSourcingDIV.Visible = true;
                                    LDAdminStep4DIV.Visible = false;
                                    LDAdminSubmitButton.Visible = false;
                                }

                                SetManagerInterventionExpectation(InterventionID);
                                SetManagerInterventionApproval(InterventionID);
                                LineManagerApproveButton.Visible = false;
                                SetLDManagerInterventionApproval(InterventionID);
                                SetLDEmployeeIntervention(InterventionID);
                                LDManagerApproveButton.Visible = false;
                            }

                            if (Status == "Preparation and Sourcing")
                            {
                                DisableEmployeeDetails();
                                DisableManagerDetails();
                                DisableLDManagerDetails();
                                ManagerStep2DIV.Visible = true;
                                LDManagerStep3DIV.Visible = true;

                                if (IsUserMemberOfLDAdminSPGroup(SPContext.Current.Web.CurrentUser, LearningDevelopmentAdminTeam))
                                {
                                    PreparationSourcingDIV.Visible = false;
                                    LDAdminStep4DIV.Visible = true;
                                    LDAdminSubmitButton.Visible = true;
                                }

                                SetManagerInterventionExpectation(InterventionID);
                                SetManagerInterventionApproval(InterventionID);
                                LineManagerApproveButton.Visible = false;
                                SetLDManagerInterventionApproval(InterventionID);
                                SetLDEmployeeIntervention(InterventionID);
                                LDManagerApproveButton.Visible = false;
                            }

                            if (Status == "Booking Confirmed")
                            {
                                DisableEmployeeDetails();
                                DisableManagerDetails();
                                DisableLDManagerDetails();
                                DisableLDAdminDetails();
                                ManagerStep2DIV.Visible = true;
                                LDManagerStep3DIV.Visible = true;
                                LDAdminStep4DIV.Visible = true;
                                SetManagerInterventionExpectation(InterventionID);
                                SetManagerInterventionApproval(InterventionID);
                                LineManagerApproveButton.Visible = false;
                                SetLDManagerInterventionApproval(InterventionID);
                                SetLDEmployeeIntervention(InterventionID);
                                LDManagerApproveButton.Visible = false;
                                LDAdminSubmitButton.Visible = false;
                            }
                        }
                        else
                        {
                            throw new Exception("Intervention invalid, please contact the administrator.");
                        }
                    }
                    else
                    {
                        throw new Exception("Intervention invalid, please contact the administrator.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Employee Submit buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EmployeeSubmit_Click(object sender, EventArgs e)
        {
            bool pageValidationValid = false;
            ErrorMessageDiv.Visible = false;

            //Declare Type of Training variable
            string trainingType = String.Empty;
            int trainingTypeAttachmentSubmited = 0;
            int disclaimerRequired = 0;
            string trainingLevel = String.Empty;
            int tempInterventionID = 0;

            try
            {
                if (DiscussedWithManagerRadioButtonList.SelectedValue == "1")
                {
                    #region Get and Set the Type of Training
                    if (drpdwnlstTypeOfTraining.SelectedValue == "Individual")
                    {
                        trainingType = drpdwnlstTypeOfTraining.SelectedValue;
                        trainingTypeAttachmentSubmited = 0;
                        disclaimerRequired = 0;
                        trainingLevel = trainingType;
                    }
                    else if (drpdwnlstTypeOfTraining.SelectedValue == "Generic")
                    {
                        trainingType = drpdwnlstTypeOfTraining.SelectedValue;
                        trainingTypeAttachmentSubmited = 0;
                        disclaimerRequired = 0;
                        trainingLevel = trainingType;
                    }
                    else if (drpdwnlstTypeOfTraining.SelectedValue == "Specialized")
                    {
                        trainingType = drpdwnlstTypeOfTraining.SelectedValue;

                        if (DisclaimerRadioButtonList.SelectedValue == "1")
                        {
                            if (SpecializedContractFileUpload.PostedFile.ContentLength > 0)
                            {
                                trainingTypeAttachmentSubmited = 1;
                                disclaimerRequired = 1;
                                trainingLevel = trainingType;
                            }
                            else
                            {
                                throw new Exception("Required : Specialized Contract");
                            }
                        }
                        else
                        {
                            throw new Exception("Please accept the disclaimer to continue.");
                        }
                    }
                    else if (drpdwnlstTypeOfTraining.SelectedValue == "Professional")
                    {
                        trainingType = drpdwnlstTypeOfTraining.SelectedValue;

                        if (ProfessionalSkillFormFileUpload.PostedFile.ContentLength > 0)
                        {
                            trainingTypeAttachmentSubmited = 1;
                            disclaimerRequired = 0;
                            trainingLevel = trainingType;
                        }
                        else
                        {
                            throw new Exception("Required : Professional Membership Skill Form");
                        }
                    }
                    #endregion

                    if (trainingTypeAttachmentSubmited >= 0 && trainingType != "")
                    {
                        #region //UNCOMMENT AFTER TESTING

                        if (ManagerEmailPeoplePicker.CommaSeparatedAccounts.Trim().Length > 0)
                        {
                            Tuple<int, int> output = null;
                            // Save or Update the Employee Intervention details
                            if (ViewState["INTERVENTIONSTATUS"] == null)
                            {
                                output = SaveEmployeeIntervention(trainingTypeAttachmentSubmited, disclaimerRequired);

                                tempInterventionID = output.Item1;

                                //Get Attached Contract \ Skills Form
                                if (trainingTypeAttachmentSubmited == 1)
                                {
                                    string getFileExt = string.Empty;

                                    if (trainingLevel == "Specialized")
                                    {
                                        getFileExt = Path.GetExtension(SpecializedContractFileUpload.PostedFile.FileName);
                                        Stream fstream = SpecializedContractFileUpload.PostedFile.InputStream;

                                        SaveTrainingContractToSP(fstream, getFileExt, tempInterventionID);
                                    }
                                    else if (trainingLevel == "Professional")
                                    {
                                        getFileExt = Path.GetExtension(ProfessionalSkillFormFileUpload.PostedFile.FileName);
                                        Stream fstream = ProfessionalSkillFormFileUpload.PostedFile.InputStream;

                                        SaveTrainingContractToSP(fstream, getFileExt, tempInterventionID);
                                    }
                                }

                                SaveFormStatus(output, "Assigned To Manager", "");
                                ComposeAndSendToManagerEmail(output.Item1);
                            }
                            else
                            {
                                output = UpdateEmployeeIntervention();
                            }
                        }
                        else
                        {
                            throw new Exception("Manager Name can not be blank.");
                        }
                        #endregion
                    }

                    pageValidationValid = true;
                }
                else
                {
                    throw new Exception("Please ensure that you have a discussion with your Line Manager regarding your learning needs.");
                }
            }
            catch (Exception ex)
            {
                SetEmployeeDetails();
                ErrorMessageDiv.Visible = true;
                ErrorMessageDiv.InnerText = ex.Message;
            }
            finally
            {
                if (pageValidationValid)
                    Response.Redirect("/CS/HR/LD/SitePages/dashboard.aspx");
            }
        }

        /// <summary>
        /// Line Manager Approve or Reject 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LineManagerApproveButton_Click(object sender, EventArgs e)
        {
            bool noError = false;

            try
            {
                SaveManagerEmployeeIntervention();
                noError = true;
            }
            catch (Exception ex)
            {
                ErrorMessageDiv.Visible = true;
                ErrorMessageDiv.InnerText = ex.Message;
            }
            finally
            {
                if (noError)
                {
                    Response.Redirect("/CS/HR/LD/SitePages/managerdashboard.aspx");
                }
            }
        }

        /// <summary>
        /// Saves the LD Manager Approvals
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LDManagerApproveButton_Click(object sender, EventArgs e)
        {
            bool noError = false;

            try
            {
                SaveLDManagerInterventionApproval();
                noError = true;
            }
            catch (Exception ex)
            {
                ErrorMessageDiv.Visible = true;
                ErrorMessageDiv.InnerText = ex.Message;
            }
            finally
            {
                if (noError)
                {
                    Response.Redirect("/CS/HR/LD/SitePages/admindashboard.aspx");
                }
            }
        }

        /// <summary>
        /// Submit the LDEmployeeIntervention 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LDAdminSubmitButton_Click(object sender, EventArgs e)
        {
            bool pageValidationValid = false;

            try
            {
                ErrorMessageDiv.Visible = false;

                if (IsTrainingCostNumeric(txtTrainingCost.Text.Trim()))
                {
                    // Save LD Employee Intervention Recommendation
                    if (Convert.ToString(ViewState["INTERVENTIONSTATUS"]) == "Preparation and Sourcing")
                        SaveLDAdminEmployeeIntervention();
                }
                else
                    throw new Exception("Training Cost value should only be numeric value");

                pageValidationValid = true;
            }
            catch (Exception ex)
            {
                ErrorMessageDiv.Visible = true;
                ErrorMessageDiv.InnerText = ex.Message;
            }
            finally
            {
                if (pageValidationValid)
                    Response.Redirect("/CS/HR/LD/SitePages/admindashboard.aspx");
            }
        }

        /// <summary>
        /// Send back an Intervention - change the status to previous status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SendBackButton_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Rejects an Intervention
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RejectButton_Click(object sender, EventArgs e)
        {
            bool isSuccess = false;
            try
            {
                EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();

                if (Request.QueryString["i"] != null)
                {
                    Tuple<int, int> InterventionTuple = new Tuple<int, int>(Convert.ToInt32(Request.QueryString["i"]), (int)dataaccess.EmployeeID(LoginName));
                    SaveFormStatus(InterventionTuple, "Learning Need Rejected", Convert.ToString(Request.QueryString["r"]));
                }

                isSuccess = true;
            }
            catch
            { }
            finally
            {
                if (isSuccess)
                    Response.Redirect("/CS/HR/LD/SitePages/dashboard.aspx");
            }
        }

        /// <summary>
        /// Show / Hide ManagerRejectReasonTextBox control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ManagerApproveRejectDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ManagerApproveRejectDropDownList.SelectedIndex == 2)
                ManagerRejectreasonTR.Visible = true;
            else
                ManagerRejectreasonTR.Visible = false;
        }

        /// <summary>
        /// Show / Hide LDManagerRejectReasonTextBox control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LDManagerApproveRejectDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LDManagerApproveRejectDropDownList.SelectedIndex == 2)
                LDManagerRejectReasonTr.Visible = true;
            else
                LDManagerRejectReasonTr.Visible = false;
        }

        /// <summary>
        /// Show / Hide LDAdminRejectReasonTextBox control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LDAdminApproveRejectDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LDAdminApproveRejectDropDownList.SelectedIndex == 2)
                LDAdminRejectReasonTr.Visible = true;
            else
                LDAdminRejectReasonTr.Visible = false;
        }

        protected void drpdwnlstTypeOfTraining_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpdwnlstTypeOfTraining.SelectedValue == "Individual")
            {
                IndividualDIV.Visible = true;
                GenericDIV.Visible = false;
                tblDisclaimer.Visible = false;
                ProfessionalDIV.Visible = false;

                tblSpecializedContractDIV.Visible = false;
            }
            else if (drpdwnlstTypeOfTraining.SelectedValue == "Generic")
            {
                IndividualDIV.Visible = false;
                GenericDIV.Visible = true;
                tblDisclaimer.Visible = false;
                ProfessionalDIV.Visible = false;

                tblSpecializedContractDIV.Visible = false;
            }
            else if (drpdwnlstTypeOfTraining.SelectedValue == "Specialized")
            {
                IndividualDIV.Visible = false;
                GenericDIV.Visible = false;
                tblDisclaimer.Visible = true;
                ProfessionalDIV.Visible = false;

                DisclaimerRadioButtonList.SelectedIndex = 1;
            }
            else if (drpdwnlstTypeOfTraining.SelectedValue == "Professional")
            {
                IndividualDIV.Visible = false;
                GenericDIV.Visible = false;
                tblDisclaimer.Visible = false;
                ProfessionalDIV.Visible = true;

                tblSpecializedContractDIV.Visible = false;
            }
        }

        protected void DisclaimerRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DisclaimerRadioButtonList.SelectedIndex == 0)
            {
                tblSpecializedContractDIV.Visible = true;
            }
            else if (DisclaimerRadioButtonList.SelectedIndex == 1)
            {
                tblSpecializedContractDIV.Visible = false;
            }
        }

        protected void btnActionSourcing_Click(object sender, EventArgs e)
        {
            bool noError = false;

            try
            {
                SaveSourcingAlertByLDAdmin();
                noError = true;
            }
            catch { }
            finally
            {
                if (noError)
                    Response.Redirect("/CS/HR/LD/SitePages/admindashboard.aspx");

            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Save Interventions
        /// </summary>
        private Tuple<int, int> SaveEmployeeIntervention(int trainingTypeAttachmentSubmited, int disclaimerRequired)
        {
            // Local variables
            int? EmployeeID = null;
            int? ManagerID = null;
            int InterventionID = 0;

            try
            {
                // Checks if both Employee and Manager In the Database
                EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();
                EmployeeID = dataaccess.EmployeeID(LoginName);
                ManagerID = dataaccess.EmployeeID(ManagerEmailPeoplePicker.CommaSeparatedAccounts.Trim().Split('\\').ElementAtOrDefault(1));

                // Reference Manager Email Address
                string ManagerEmailLoginName = ManagerEmailPeoplePicker.CommaSeparatedAccounts.Trim().Split('\\').ElementAtOrDefault(1);

                // Create Instances
                ActiveDirectoryDataAccess activedirectory = new ActiveDirectoryDataAccess();

                EmployeeInterventionsObject employeeintervention = null;

                ActiveDirectoryEmployeeObject EmployeeDetailsObject = null;

                EmployeeObject employee = null;

                if (EmployeeID != null)
                {
                    if (ManagerID == null)
                    {
                        ActiveDirectoryEmployeeObject ManagerDetailsObject = new ActiveDirectoryEmployeeObject();
                        ManagerDetailsObject = activedirectory.EmployeeDetails(ManagerEmailLoginName);
                        EmployeeObject manager = new EmployeeObject() { Username = ManagerDetailsObject.Logonname, Name = ManagerDetailsObject.Name, Surname = ManagerDetailsObject.SurName, Designation = ManagerDetailsObject.Designation, Department = ManagerDetailsObject.Department, ManagerID = 0 };
                        ManagerID = dataaccess.InsertEmployee(manager);
                    }

                    EmployeeDetailsObject = activedirectory.EmployeeDetails(LoginName);
                    employee = new EmployeeObject() { EmployeeID = (int)EmployeeID, Designation = txtEmpDesignation.Text.Trim(), ManagerID = ManagerID };
                    dataaccess.UpdateEmployee(employee);

                    //Edit and insert the training type
                    employeeintervention = new EmployeeInterventionsObject() { EmployeeID = EmployeeID, DiscussedWithManager = Convert.ToInt32(DiscussedWithManagerRadioButtonList.SelectedValue), TrainingType = drpdwnlstTypeOfTraining.SelectedValue, TrainingTypeContractSubmitted = trainingTypeAttachmentSubmited, Disclaimer = Convert.ToInt32(disclaimerRequired), DevelopmentNeeds = drpdwnlstDevelopmentNeeds.SelectedValue, NameOfLearning = txtLearningIntervention.Text, EmployeeExpectations = LearningObjectivesTextBox.Text, Submitted = 1 };
                    // Insert Intervention
                    InterventionID = dataaccess.InsertEmployeeIntervention(employeeintervention);
                }
                else
                {
                    //Insert Manager if ManagerID is null
                    if (ManagerID == null)
                    {
                        ActiveDirectoryEmployeeObject ManagerDetailsObject = new ActiveDirectoryEmployeeObject();
                        ManagerDetailsObject = activedirectory.EmployeeDetails(ManagerEmailLoginName);
                        EmployeeObject manager = new EmployeeObject() { Username = ManagerDetailsObject.Logonname, Name = ManagerDetailsObject.Name, Surname = ManagerDetailsObject.SurName, Designation = ManagerDetailsObject.Designation, Department = ManagerDetailsObject.Department, ManagerID = 0 };
                        ManagerID = dataaccess.InsertEmployee(manager);
                    }

                    // Insert Employee with Manager ID only if EmployeeID is null
                    if (EmployeeID == null)
                    {
                        EmployeeDetailsObject = new ActiveDirectoryEmployeeObject();
                        EmployeeDetailsObject = activedirectory.EmployeeDetails(LoginName);
                        employee = new EmployeeObject() { Username = EmployeeDetailsObject.Logonname, Name = EmployeeDetailsObject.Name, Surname = EmployeeDetailsObject.SurName, Designation = txtEmpDesignation.Text.Trim(), Department = EmployeeDetailsObject.Department, ManagerID = ManagerID };
                        EmployeeID = dataaccess.InsertEmployee(employee);

                        employeeintervention = new EmployeeInterventionsObject() { EmployeeID = EmployeeID, DiscussedWithManager = Convert.ToInt32(DiscussedWithManagerRadioButtonList.SelectedValue), TrainingType = drpdwnlstTypeOfTraining.SelectedValue, TrainingTypeContractSubmitted = trainingTypeAttachmentSubmited, Disclaimer = Convert.ToInt32(disclaimerRequired), DevelopmentNeeds = drpdwnlstDevelopmentNeeds.SelectedValue, NameOfLearning = txtLearningIntervention.Text, EmployeeExpectations = LearningObjectivesTextBox.Text, Submitted = 1 };

                        // Insert Intervention
                        InterventionID = dataaccess.InsertEmployeeIntervention(employeeintervention);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return new Tuple<int, int>(InterventionID, (int)EmployeeID);
        }

        /// <summary>
        /// Update the Employee Intervention
        /// </summary>
        /// <returns></returns>
        private Tuple<int, int> UpdateEmployeeIntervention()
        {
            // Local variables
            int? EmployeeID = null;
            int InterventionID = 0;
            int? ManagerID = null;

            EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess(); ;
            ActiveDirectoryDataAccess activedirectory = new ActiveDirectoryDataAccess();

            ManagerID = dataaccess.EmployeeID(ManagerEmailPeoplePicker.CommaSeparatedAccounts.Trim().Split('\\').ElementAtOrDefault(1));

            // Reference Manager Email Address
            string ManagerEmailLoginName = ManagerEmailPeoplePicker.CommaSeparatedAccounts.Trim().Split('\\').ElementAtOrDefault(1);

            EmployeeInterventionsObject intervention = null;

            // Gets the Current Employee ID
            EmployeeID = dataaccess.EmployeeID(LoginName);

            EmployeeObject employee = null;

            if (EmployeeID != null)
            {
                if (ManagerID == null)
                {
                    ActiveDirectoryEmployeeObject ManagerDetailsObject = new ActiveDirectoryEmployeeObject();
                    ManagerDetailsObject = activedirectory.EmployeeDetails(ManagerEmailLoginName);
                    EmployeeObject manager = new EmployeeObject()
                    {
                        Username = ManagerDetailsObject.Logonname,
                        Name = ManagerDetailsObject.Name,
                        Surname = ManagerDetailsObject.SurName,
                        Designation = ManagerDetailsObject.Designation,
                        Department = ManagerDetailsObject.Department,
                        ManagerID = 0
                    };

                    ManagerID = dataaccess.InsertEmployee(manager);

                    ActiveDirectoryEmployeeObject EmployeeDetailsObject = new ActiveDirectoryEmployeeObject();
                    employee = new EmployeeObject() { EmployeeID = (int)EmployeeID, Designation = txtEmpDesignation.Text.Trim(), ManagerID = ManagerID };
                    dataaccess.UpdateEmployee(employee);
                }

                employee = new EmployeeObject() { EmployeeID = (int)EmployeeID, Designation = txtEmpDesignation.Text.Trim(), ManagerID = ManagerID };
                dataaccess.UpdateEmployee(employee);

                intervention = new EmployeeInterventionsObject()
                {
                    InterventionID = Convert.ToInt32(ViewState["INTERVENTIONID"]),
                    EmployeeID = EmployeeID,
                    DiscussedWithManager = Convert.ToInt32(DiscussedWithManagerRadioButtonList.SelectedValue),
                    Disclaimer = Convert.ToInt32(DisclaimerRadioButtonList.SelectedValue),
                    DevelopmentNeeds = drpdwnlstDevelopmentNeeds.SelectedValue,
                    NameOfLearning = txtLearningIntervention.Text,
                    EmployeeExpectations = LearningObjectivesTextBox.Text,
                    WorkBackContractSubmitted = Convert.ToBoolean(ViewState["WORKBACKCONTRACTREQUIRED"]) == true ? 1 : 0,
                    Submitted = 1
                };
                // Insert Intervention
                InterventionID = dataaccess.UpdateEmployeeIntervention(intervention);
            }

            return new Tuple<int, int>(InterventionID, (int)EmployeeID);
        }

        /// <summary>
        /// Save the LDEmployeeIntervention
        /// </summary>
        private void SaveLDAdminEmployeeIntervention()
        {
            EmployeeInterventionsDataAccess LDempIntervent = new EmployeeInterventionsDataAccess();

            int? EmployeeID = null;
            EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();
            EmployeeID = dataaccess.EmployeeID(LoginName);

            if (EmployeeID == null)
                EmployeeID = InsertEmployee(LoginName);

            // Collect LD Cost and Instituion Details
            if (ViewState["INTERVENTIONID"] != null)
            {
                if (IsTrainingCostNumeric(txtTrainingCost.Text.Trim()))
                {
                    LDAdminEmployeeInterventionsObject Intervention = new LDAdminEmployeeInterventionsObject()
                    {
                        EmployeeInterventionID = Convert.ToInt32(ViewState["INTERVENTIONID"]),
                        LDEmployeeID = (int?)EmployeeID,
                        ServiceProvider = txtServiceProvider.Text,
                        StartDateOfTraining = Convert.ToDateTime(tbDateFrom.Value.ToString()),
                        EndDateOfTraining = Convert.ToDateTime(tbDateTo.Value.ToString()),
                        Cost = Convert.ToDecimal(txtTrainingCost.Text.Trim()),
                        TypeOfIntervention = TypeOfInterventionDropDownList.SelectedItem.Text,
                        IsBelowThreshold = (Convert.ToDecimal(txtTrainingCost.Text.Trim()) > Convert.ToDecimal(Configs.TrainingCostThreshold)) ? 1 : 0,
                        Approved = Convert.ToInt32(LDAdminApproveRejectDropDownList.SelectedItem.Value),
                        Submitted = 1
                    };

                    dataaccess.InsertLDAdminEmployeeIntervention(Intervention);

                    if (LDAdminApproveRejectDropDownList.SelectedIndex == 1)
                    {
                        SaveFormStatus(new Tuple<int, int>(Convert.ToInt32(ViewState["INTERVENTIONID"]), (int)EmployeeID), "Booking Confirmed", "");
                        ComposeAndSendApprovedEmailToEmployee();
                    }
                    else
                    {
                        SaveFormStatus(new Tuple<int, int>(Convert.ToInt32(ViewState["INTERVENTIONID"]), (int)EmployeeID), "Learning Need Rejected", LDAdminRejectReasonTextBox.Text.Trim());
                        ComposeAndSendRejectedEmailToEmployee();
                    }
                }
                else
                {
                    throw new Exception("Training Cost value should only be numeric value");
                }
            }
        }

        /// <summary>
        /// Update the LD Employee Intervention details
        /// </summary>
        /// <returns></returns>
        private Tuple<int, int> UpdateLDAdminEmployeeIntervention()
        {
            int? EmployeeID = null;
            EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();
            EmployeeID = dataaccess.EmployeeID(LoginName);

            if (EmployeeID == null)
                EmployeeID = InsertEmployee(LoginName);

            // Collect LD Cost and Instituion Details
            if (ViewState["INTERVENTIONID"] != null)
            {
                LDAdminEmployeeInterventionsObject Intervention = new LDAdminEmployeeInterventionsObject() { EmployeeInterventionID = Convert.ToInt32(ViewState["INTERVENTIONID"]), LDEmployeeID = (int?)EmployeeID, ServiceProvider = txtServiceProvider.Text, StartDateOfTraining = Convert.ToDateTime(tbDateFrom.Value.ToString()), EndDateOfTraining = Convert.ToDateTime(tbDateTo.Value.ToString()), Cost = Convert.ToDecimal(txtTrainingCost.Text.Trim()), TypeOfIntervention = TypeOfInterventionDropDownList.SelectedItem.Text, IsBelowThreshold = Convert.ToDecimal(txtTrainingCost.Text.Trim()) >= Convert.ToDecimal(Configs.TrainingCostThreshold) ? 1 : 0, ReceivedSignedContract = 0, Approved = Convert.ToInt32(LDAdminApproveRejectDropDownList.SelectedItem.Value), Submitted = 1 };
                dataaccess.UpdateLDAdminEmployeeIntervention(Intervention);
            }

            if (IsTrainingCostNumeric(txtTrainingCost.Text.Trim()))
            {
                if (LDAdminApproveRejectDropDownList.SelectedIndex == 1)
                {
                    SaveFormStatus(new Tuple<int, int>(Convert.ToInt32(ViewState["INTERVENTIONID"]), (int)EmployeeID), "Booking Confirmed", "");
                    ComposeAndSendApprovedEmailToEmployee();
                }
                else
                {
                    SaveFormStatus(new Tuple<int, int>(Convert.ToInt32(ViewState["INTERVENTIONID"]), (int)EmployeeID), "Learning Need Rejected", "");
                    ComposeAndSendRejectedEmailToEmployee();
                }
            }
            else
            {
                throw new Exception("Training Cost value should only be numeric value");
            }

            return new Tuple<int, int>(Convert.ToInt32(ViewState["INTERVENTIONID"]), (int)EmployeeID);
        }

        /// <summary>
        /// LD Admin updates the Employee Development Need and Name of the Learning Intervention
        /// </summary>
        private void UpdateLDAdminEmployeeIntervention_Old()
        {
            try
            {
                EmployeeInterventionsObject employeeintervention = new EmployeeInterventionsObject()
                {
                    InterventionID = Convert.ToInt32(ViewState["INTERVENTIONID"]),
                    DevelopmentNeeds = drpdwnlstDevelopmentNeeds.SelectedValue,
                    NameOfLearning = txtLearningIntervention.Text
                };

                EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();
                dataaccess.UpdateLDAdminEmployeeIntervention(employeeintervention);
            }
            catch
            {
                //
            }
        }

        /// <summary>
        /// Save the LDEmployeeIntervention
        /// </summary>
        private void SaveSourcingAlertByLDAdmin()
        {
            try
            {
                EmployeeInterventionsDataAccess LDempIntervent = new EmployeeInterventionsDataAccess();

                int? EmployeeID = null;
                EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();
                EmployeeID = dataaccess.EmployeeID(LoginName);

                if (EmployeeID == null)
                    EmployeeID = InsertEmployee(LoginName);

                // Collect LD Cost and Instituion Details
                if (ViewState["INTERVENTIONID"] != null)
                {
                    SaveFormStatus(new Tuple<int, int>(Convert.ToInt32(ViewState["INTERVENTIONID"]), (int)EmployeeID), "Preparation and Sourcing", "");
                }
            }
            catch
            {
            }
        }


        /// <summary>
        /// Save ManagerEmployeeIntervention
        /// </summary>
        private void SaveManagerEmployeeIntervention()
        {
            int? ManagerID = null;
            EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();

            ManagerID = dataaccess.EmployeeID(LoginName);

            if (ManagerID == null)
                ManagerID = InsertEmployee(LoginName);

            try
            {
                //Collect EmployeeInterventionID, ManagerID, Expectations, Submitted  Details 
                if (ViewState["INTERVENTIONID"] != null)
                {
                    //Get and Insert Line Manager Expectation
                    ManagerEmployeeInterventionsObject ManagerIntervention = new ManagerEmployeeInterventionsObject() { EmployeeInterventionID = Convert.ToInt32(ViewState["INTERVENTIONID"]), ManagerEmployeeID = (int)ManagerID, Expectations = txtPerformanceImprovements.Text.Trim(), Submitted = 1 };

                    //Insert Manager Intervention Input
                    dataaccess.InsertManagerEmployeeIntervention(ManagerIntervention);
               
                    //Get and Insert Line Manager Approval    
                    ManagerEmployeeInterventionsApprovalObject ManagerApproval = new ManagerEmployeeInterventionsApprovalObject() { EmployeeInterventionID = Convert.ToInt32(ViewState["INTERVENTIONID"]), ManagerEmployeeID = (int)ManagerID, WasInterventionONPDP = FoundOnPDPDropDownList.SelectedIndex == 1 ? 1 : 0, Approved = ManagerApproveRejectDropDownList.SelectedIndex == 1 ? 1 : 0 };

                    //Insert Manager Approvals
                    dataaccess.InsertManagerEmployeeInterventionsApproval(ManagerApproval);

                    if (ManagerApproveRejectDropDownList.SelectedIndex == 1)
                    {    // Save Form Status
                        SaveFormStatus(new Tuple<int, int>(Convert.ToInt32(ViewState["INTERVENTIONID"]), (int)ManagerID), "Assigned To L&D Manager", "");
                        ComposeAndSendManagerApprovedActionToLDManager();
                    }
                    else
                    {
                        SaveFormStatus(new Tuple<int, int>(Convert.ToInt32(ViewState["INTERVENTIONID"]), (int)ManagerID), "Learning Need Rejected", ManagerRejectReasonTextbox.Text.Trim());
                        ComposeAndSendManagerRejectedActionToLDAndEmployee();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save LD Manager Employee Intervention - FIC_LDEmployeeInterventionApproval
        /// </summary>
        private void SaveLDManagerInterventionApproval()
        {
            int? LDManagerID = null;
            EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();

            LDManagerID = dataaccess.EmployeeID(LoginName);

            if (LDManagerID == null)
                LDManagerID = InsertEmployee(LoginName);

            try
            {
                //Collect EmployeeInterventionID, LDEmployeeID, FICLDPolicyANDProcedures, FundsAvailable, Approved  Details 
                if (ViewState["INTERVENTIONID"] != null)
                {
                    //Get LD Manager Approval from form
                    LDEmployeeInterventionsApprovalObject LDManagerApproval = new LDEmployeeInterventionsApprovalObject() { EmployeeInterventionID = Convert.ToInt32(ViewState["INTERVENTIONID"]), LDEmployeeID = (int)LDManagerID, FICLDPolicyANDProcedures = PolicyAdheredToDropDownList.SelectedIndex == 1 ? 1 : 0, FundsAvailable = FundsAvailableDropDownList.SelectedIndex == 1 ? 1 : 0, Approved = LDManagerApproveRejectDropDownList.SelectedIndex == 1 ? 1 : 0 };

                    //Insert Manager Intervention Input
                    dataaccess.InsertLDEmployeeInterventionsApproval(LDManagerApproval);

                    if (LDManagerApproveRejectDropDownList.SelectedIndex > 0)
                    {
                        if (LDManagerApproveRejectDropDownList.SelectedIndex == 1)
                        {
                            //SaveFormStatus(new Tuple<int, int>(Convert.ToInt32(ViewState["INTERVENTIONID"]), (int)LDManagerID), "Preparation and Sourcing", "");
                            SaveFormStatus(new Tuple<int, int>(Convert.ToInt32(ViewState["INTERVENTIONID"]), (int)LDManagerID), "Learning Need Approved", "");

                            ComposeAndSendLDManagerEmailApproval();
                        }
                        else
                        {
                            SaveFormStatus(new Tuple<int, int>(Convert.ToInt32(ViewState["INTERVENTIONID"]), (int)LDManagerID), "Learning Need Rejected", LDManagerRejectReasonTextBox.Text.Trim());
                            ComposeAndSendLDManagerRejectedActionToLDAndEmployee();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Set the Employee details
        /// </summary>
        private void SetEmployeeDetails()
        {
            ActiveDirectoryDataAccess activeDirectoryDataAccess = new ActiveDirectoryDataAccess();
            EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();

            try
            {
                ActiveDirectoryEmployeeObject activedirecresult = activeDirectoryDataAccess.EmployeeDetails(LoginName);

                string EmployeeLoginname = string.Empty;

                if (ViewState["INTERVENTIONID"] != null)
                {
                    EmployeeInterventionsObject results = dataaccess.Intervention(Convert.ToInt32((ViewState["INTERVENTIONID"])));

                    EmployeeLoginname = dataaccess.EmployeeUserName((int)results.EmployeeID);

                    if (activedirecresult.Logonname == EmployeeLoginname)
                    {
                        txtEmpFullName.Text = $"{activedirecresult.Name} {activedirecresult.SurName}";
                        txtEmpDesignation.Text = results.Designation;
                        txtEmpDept.Text = activedirecresult.Department;
                    }
                    else
                    {
                        ActiveDirectoryEmployeeObject result = activeDirectoryDataAccess.EmployeeDetails(EmployeeLoginname);
                        txtEmpFullName.Text = $"{result.Name} {result.SurName}";
                        txtEmpDesignation.Text = results.Designation;
                        txtEmpDept.Text = result.Department;
                    }
                }
                else
                {
                    txtEmpFullName.Text = $"{activedirecresult.Name} {activedirecresult.SurName}";
                    txtEmpDesignation.Text = activedirecresult.Designation;
                    txtEmpDept.Text = activedirecresult.Department;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sets Employee Manager
        /// </summary>
        private void SetEmployeeManagerANDDesignation()
        {
            EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();

            // Set the Manager if the Employee has already defined a Manager
            try
            {
                if (ManagerEmailPeoplePicker.CommaSeparatedAccounts.Trim().Length == 0)
                    ManagerEmailPeoplePicker.CommaSeparatedAccounts = Convert.ToString(ViewState["EMPLOYEEMANAGER"]);

                //if(dataaccess.EmployeeDesignation(LoginName).Length > 0)
                //    txtEmpDesignation.Text = dataaccess.EmployeeDesignation(LoginName);
            }
            catch
            {
            }

        }

        /// <summary>
        /// Set the Employee Intervention details by InterventionID
        /// </summary>
        /// <param name="EmpInterventionID"></param>
        public Tuple<string, string> SetEmployeeIntervention(int InterventionID)
        {
            string InterventionStatus = string.Empty;
            string InterventionType = String.Empty;

            try
            {
                EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();
                EmployeeInterventionsObject intervention = dataaccess.Intervention(InterventionID);

                // Set the Intervention Status , EmployeeID, Flag to Indicate if the Cost is below or above threshold amount into the ViewState object
                ViewState["INTERVENTIONSTATUS"] = intervention.Status;
                ViewState["INTERVENTIONEMPLOYEEID"] = intervention.EmployeeID;
                ViewState["WORKBACKCONTRACTREQUIRED"] = intervention.IsBelowThreshold > 0 ? true : false;

                // Set Employee Designation
                txtEmpDesignation.Text = intervention.Designation;

                //Assign Manager Discussion Values
                DiscussedWithManagerRadioButtonList.SelectedIndex = intervention.DiscussedWithManager == 1 ? 0 : 1;
                //Assign Disclaimer Values
                drpdwnlstTypeOfTraining.SelectedValue = intervention.TrainingType;
                InterventionType = intervention.TrainingType;
                DisclaimerRadioButtonList.SelectedIndex = intervention.Disclaimer == 1 ? 0 : 1;
                drpdwnlstDevelopmentNeeds.SelectedValue = intervention.DevelopmentNeeds;
                txtLearningIntervention.Text = intervention.NameOfLearning;
                LearningObjectivesTextBox.Text = intervention.EmployeeExpectations;
                ManagerEmailPeoplePicker.CommaSeparatedAccounts = $"FIC\\{intervention.Manager}";

                ViewState["EMPLOYEEMANAGER"] = ManagerEmailPeoplePicker.CommaSeparatedAccounts;

                if (InterventionType == "Specialized" && intervention.Status == "Assigned To Manager")
                {
                    SpecializedContractDiv.Visible = intervention.TrainingTypeContractSubmitted == 1 ? true : false;
                    SpecializedContractDiv.InnerHtml = intervention.TrainingTypeContractSubmitted == 1 ? LoadTrainingTypeContractURL() : "";

                    SpecializedContractDiv.Visible = true;
                }
                else if (InterventionType == "Professional" && intervention.Status == "Assigned To Manager")
                {
                    ProfessionalSkillFormDiv.Visible = intervention.TrainingTypeContractSubmitted == 1 ? true : false;
                    ProfessionalSkillFormDiv.InnerHtml = intervention.TrainingTypeContractSubmitted == 1 ? LoadTrainingTypeContractURL() : "";

                    ProfessionalSkillFormDiv.Visible = true;
                }

                // If the Status is still "WaitingRecommendation"
                if (intervention.Status == "Assigned To Manager")
                {
                    EmployeeSubmit.Visible = true;
                    EmployeeSubmit.Text = "Update & Submit";
                }
                else
                    EmployeeSubmit.Visible = false;

                InterventionStatus = intervention.Status;
            }
            catch (Exception ex)
            {
            }

            return new Tuple<string, string>(InterventionStatus, InterventionType);
        }

        /// <summary>
        /// Set Manager Employee Expectation Values
        /// </summary>
        /// <param name="EmpInterventionID"></param>
        private void SetLDEmployeeIntervention(int InterventionID)
        {
            EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();

            LDAdminEmployeeInterventionsObject intervention = null;
            try
            {
                try
                {
                    intervention = dataaccess.LDAdminEmployeeIntervention(InterventionID);
                }
                catch
                {
                }

                if (intervention != null)
                {
                    //Assign values to fields on the form - ServiceProvider, StartDateOfTraining, EndDateOfTraining, Cost  
                    txtServiceProvider.Text = Convert.ToString(intervention.ServiceProvider);
                    tbDateFrom.Value = Convert.ToString(intervention.StartDateOfTraining);
                    tbDateTo.Value = Convert.ToString(intervention.EndDateOfTraining);
                    txtTrainingCost.Text = Convert.ToString(Math.Round(intervention.Cost, 2));
                    LDAdminApproveRejectDropDownList.SelectedIndex = intervention.Approved == 1 ? 1 : 0;
                    // Set Type of Intervention control values
                    //if (intervention.TypeOfIntervention.Equals("Standard"))
                    //    TypeOfInterventionDropDownList.SelectedIndex = 1;
                    //if (intervention.TypeOfIntervention.Equals("Specialised"))
                    //    TypeOfInterventionDropDownList.SelectedIndex = 2;
                }
            }
            catch
            {
                // TODO - catch exception
            }
        }

        private void SetManagerInterventionExpectation(int InterventionID)
        {
            EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();

            try
            {
                ManagerEmployeeInterventionsObject intervention = null;

                try
                {
                    intervention = dataaccess.ManagerEmployeeIntervention(InterventionID);
                }
                catch
                {
                }

                if (intervention != null)
                {
                    //Assign Manager Expectations Value                     
                    txtPerformanceImprovements.Text = intervention.Expectations;
                }

                if (ManagerEmailPeoplePicker.CommaSeparatedAccounts.Trim().Length > 0)
                {
                    // Show Submit Button if the Current User is Manager
                    string EmployeeManager = ManagerEmailPeoplePicker.CommaSeparatedAccounts.Trim().Split('\\').ElementAtOrDefault(1);
                    LineManagerApproveButton.Visible = EmployeeManager == LoginName ? true : false;
                }

            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Set Manager Employee Approval Values
        /// </summary>
        /// <param name="EmpIntervenID"></param>
        private void SetManagerInterventionApproval(int InterventionID)
        {
            EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();

            try
            {
                ManagerEmployeeInterventionsApprovalObject intervention = dataaccess.ManagerEmployeeInterventionApproval(InterventionID);
                //Assign Found in Employees's PDP Values
                FoundOnPDPDropDownList.SelectedIndex = intervention.WasInterventionONPDP == 1 ? 1 : 2;
                ManagerApproveRejectDropDownList.SelectedIndex = intervention.WasInterventionONPDP == 1 ? 1 : 2;
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Set LD Manager Employee Approval Values
        /// </summary>
        /// <param name="InterventionID"></param>
        private void SetLDManagerInterventionApproval(int InterventionID)
        {
            EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();

            try
            {
                LDEmployeeInterventionsApprovalObject intervention = null;

                try
                {
                    intervention = dataaccess.LDEmployeeInterventionApproval(InterventionID);
                }
                catch
                {
                }

                if (intervention != null)
                {
                    //Assign LD Manager Intervention Approval Values - Adhered to Policies and Procedures
                    PolicyAdheredToDropDownList.SelectedIndex = intervention.FICLDPolicyANDProcedures == 1 ? 1 : 2;
                    //Assign LD Manager Intervention Approval Values - Funds Availability
                    FundsAvailableDropDownList.SelectedIndex = intervention.FundsAvailable == 1 ? 1 : 2;
                    LDManagerApproveRejectDropDownList.SelectedIndex = intervention.Approved == 1 ? 1 : 2;
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Save Form Status
        /// </summary>
        /// <param name="output"></param>
        /// <param name="status"></param>
        private void SaveFormStatus(Tuple<int, int> output, string status, string reason)
        {
            // Set Form Status Properties
            FormStatusObjects Formstatus = new FormStatusObjects();
            Formstatus.InterventionID = output.Item1;
            Formstatus.EmployeeID = output.Item2;
            Formstatus.Status = status;
            Formstatus.Reason = reason;

            // Insert Form Status
            EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();
            dataaccess.InsertInterventionFormStatus(Formstatus);
        }

        /// <summary>
        /// Insert Employee without a Manager
        /// </summary>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        private int InsertEmployee(string LoginName)
        {
            int EmployeeID = 0;
            try
            {
                ActiveDirectoryDataAccess activedirectory = new ActiveDirectoryDataAccess();
                EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();

                ActiveDirectoryEmployeeObject EmployeeDetailsObject = new ActiveDirectoryEmployeeObject();
                EmployeeDetailsObject = activedirectory.EmployeeDetails(LoginName);
                EmployeeObject employee = new EmployeeObject() { Username = EmployeeDetailsObject.Logonname, Name = EmployeeDetailsObject.Name, Surname = EmployeeDetailsObject.SurName, Designation = EmployeeDetailsObject.Designation, Department = EmployeeDetailsObject.Department, ManagerID = 0 };
                EmployeeID = dataaccess.InsertEmployee(employee);

            }
            catch
            {
            }

            return EmployeeID;
        }

        private string CheckInterventionStatusForLDAdmin(string InterventionID)
        {
            string InterventionStatusCheck = String.Empty;

            try
            {
                EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();
                EmployeeInterventionsObject employeeIntervention = new EmployeeInterventionsObject();
                employeeIntervention = dataaccess.CheckInterventionStatusForLDAdmin(Convert.ToInt32(InterventionID));

            }
            catch
            {
                throw new Exception("Intervention invalid, please contact the administrator.");
            }

            return InterventionStatusCheck;
        }

        /// <summary>
        /// Checks if the Current User is the Member of SP LearningDevelopmentAdminTeam group
        /// </summary>
        /// <returns></returns>
        private bool IsUserMemberOfLDAdminSPGroup(SPUser user, string groupName)
        {
            bool result = false;

            if (!String.IsNullOrEmpty(groupName) && user != null)
            {
                SPGroup group = user.Groups.Cast<SPGroup>().FirstOrDefault(g => g.Name.Equals(groupName));

                if (group != null)
                    result = true;
                else
                    result = false;

            }

            return result;
        }

        /// <summary>
        /// Checks if the Current User is the Member of SP LearningDevelopmentManagersTeam group
        /// </summary>
        /// <returns></returns>
        private bool IsUserMemberOfLDApproverSPGroup(SPUser user, string groupName)
        {
            bool result = false;

            if (!String.IsNullOrEmpty(groupName) && user != null)
            {
                SPGroup group = user.Groups.Cast<SPGroup>().FirstOrDefault(g => g.Name.Equals(groupName));

                if (group != null)
                    result = true;
                else
                    result = false;

            }

            return result;
        }

        /// <summary>
        /// Test to see if the Cost value is numeric
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool IsTrainingCostNumeric(string value)
        {
            bool IsNumeric = false;
            try
            {
                double output;
                IsNumeric = double.TryParse(value, out output);
            }
            catch { }

            return IsNumeric;
        }

        /// <summary>
        /// Checks if the Current User is the Intervention Manager
        /// </summary>
        /// <returns></returns>
        private bool IsUserEmployeeManager()
        {
            return ManagerEmailPeoplePicker.CommaSeparatedAccounts.Trim().Split('\\').ElementAtOrDefault(1) == LoginName ? true : false;
        }

        /// <summary>
        /// Checks if the Current User is the Intervention Employee
        /// </summary>
        /// <returns></returns>
        private bool IsInterventionForEmployee()
        {
            EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();
            int? EmployeeID = null;
            try
            {
                EmployeeID = dataaccess.EmployeeID(LoginName);
            }
            catch
            {
                throw new Exception("Access denied.");
            }

            if (EmployeeID != null)
                return (int)EmployeeID == Convert.ToInt32(ViewState["INTERVENTIONEMPLOYEEID"]) ? true : false;
            else
                return false;
        }

        /// <summary>
        /// // If the Current User is not the Employee Intervention or Intervention Manager or Member of LDTrainingTeam hide content
        /// </summary>
        private void IsUserAuthorized()
        {
            try
            {
                if (!IsInterventionForEmployee() && !IsUserEmployeeManager() && !IsUserMemberOfLDAdminSPGroup(SPContext.Current.Web.CurrentUser, LearningDevelopmentAdminTeam) && !IsUserMemberOfLDAdminSPGroup(SPContext.Current.Web.CurrentUser, LearningDevelopmentManagersTeam))
                    throw new Exception("Access denied.");
            }
            catch (Exception ex)
            {
                //MainContent.Visible = false;
                throw ex;
            }
        }

        /// <summary>
        /// Disable Employees details
        /// </summary>
        private void DisableEmployeeDetails()
        {
            ManagerEmailPeoplePicker.Enabled = false;
            DiscussedWithManagerRadioButtonList.Enabled = false;
            DisclaimerRadioButtonList.Enabled = false;
            drpdwnlstDevelopmentNeeds.Enabled = false;
            txtLearningIntervention.Enabled = false;
            LearningObjectivesTextBox.Enabled = false;
            drpdwnlstTypeOfTraining.Enabled = false;
        }

        /// <summary>
        /// Disable Manager details
        /// </summary>
        private void DisableManagerDetails()
        {
            txtPerformanceImprovements.Enabled = false;
            FoundOnPDPDropDownList.Enabled = false;
            ManagerApproveRejectDropDownList.Enabled = false;
        }

        /// <summary>
        /// Disable LD Manager details
        /// </summary>
        private void DisableLDManagerDetails()
        {
            PolicyAdheredToDropDownList.Enabled = false;
            FundsAvailableDropDownList.Enabled = false;
            LDManagerApproveRejectDropDownList.Enabled = false;
        }

        /// <summary>
        /// Disable LD Admin details
        /// </summary>
        private void DisableLDAdminDetails()
        {
            txtServiceProvider.Enabled = false;
            tbDateFrom.Disabled = true;
            tbDateTo.Disabled = true;
            txtTrainingCost.Enabled = true;
            txtTrainingCost.Enabled = false;
            LDAdminApproveRejectDropDownList.Enabled = false;
        }

        /// <summary>
        /// Save file into SP Document Library
        /// </summary>
        private void SaveWorkBackContractToSP()
        {
            try
            {
                using (SPSite site = new SPSite(Configs.SPSiteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        Stream fstream = WorkBackFileUpload.PostedFile.InputStream;
                        byte[] content = new byte[fstream.Length];
                        fstream.Read(content, 0, (int)fstream.Length);
                        fstream.Close();

                        string filename = string.Format("contract_{0}{1}{2}", Convert.ToInt32(ViewState["INTERVENTIONID"]), LoginName, Path.GetExtension(WorkBackFileUpload.PostedFile.FileName));
                        string destinationLibrary = Configs.WorkbackLibraryName;

                        web.AllowUnsafeUpdates = true;
                        web.Files.Add($"{site.Url}/{destinationLibrary}/{filename}", content, true);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save file into SP Document Library
        /// </summary>
        private void SaveTrainingContractToSP(Stream fstream, string getFileExt, int tempInterventionID)
        {
            try
            {
                SPWeb web = SPContext.Current.Web;

                byte[] content = new byte[fstream.Length];
                fstream.Read(content, 0, (int)fstream.Length);
                fstream.Close();

                string filename = string.Format("Contract_{0}{1}{2}", Convert.ToInt32(tempInterventionID), LoginName, getFileExt);
                string destinationLibrary = Configs.TrainingTypeLibraryName;

                web.AllowUnsafeUpdates = true;

                // Add Folder to Document Library
                SPList LibraryList = web.Lists[destinationLibrary];

                SPListItem folder = LibraryList.Folders.Cast<SPListItem>().FirstOrDefault(f => f.Name.Equals(LoginName));

                if (folder == null)
                {
                    folder = LibraryList.Folders.Add(LibraryList.RootFolder.ServerRelativeUrl, SPFileSystemObjectType.Folder, LoginName);
                    folder.Update();
                    web.Files.Add($"{folder.Url}/{filename}", content, true);
                }
                else
                {
                    // Get a folder by server-relative URL.
                    string FolderUrl = $"{web.ServerRelativeUrl}/{LibraryList}/{LoginName}";
                    SPFolder _folder = web.GetFolder(FolderUrl);
                    _folder.Files.Add(filename, content, true);
                    _folder.Update();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// If the Work-back contract was attached, get the file link
        /// </summary>
        /// <returns></returns>
        private string LoadContractURL()
        {
            string fileUrl = string.Empty;

            try
            {
                using (SPSite site = new SPSite(Configs.SPSiteURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        string filepath = string.Format("Contract_{0}", Convert.ToInt32(ViewState["INTERVENTIONID"]));
                        SPDocumentLibrary library = web.Lists[Configs.WorkbackLibraryName] as SPDocumentLibrary;
                        SPListItem fileitem = library.Items.Cast<SPListItem>().FirstOrDefault(f => f.Name.StartsWith(filepath));

                        if (fileitem != null)
                            fileUrl = string.Format("<a href='/{0}' target='_blank'>Contract</a>", fileitem.Url);
                        else
                            fileUrl = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return fileUrl;
        }

        /// <summary>
        /// If the Work-back contract was attached, get the file link
        /// </summary>
        /// <returns></returns>
        private string LoadTrainingTypeContractURL()
        {
            string fileUrl = string.Empty;

            try
            {
                SPWeb web = SPContext.Current.Web;

                string filepath = string.Format("Contract_{0}", Convert.ToInt32(ViewState["INTERVENTIONID"]));
                SPDocumentLibrary library = web.Lists[Configs.TrainingTypeLibraryName] as SPDocumentLibrary;

                // Check if  the Folder Exist
                SPFolder Folder = library.RootFolder.SubFolders.Cast<SPFolder>().FirstOrDefault(f => f.Name.Equals(LoginName));

                if (Folder != null)
                {
                    SPFile fileitem = Folder.Files.Cast<SPFile>().FirstOrDefault(f => f.Name.StartsWith(filepath));

                    if (fileitem != null)
                        fileUrl = string.Format("<a href='/{0}' target='_blank'>Download Contract</a>", fileitem.Url);
                    else
                        fileUrl = string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return fileUrl;
        }

        /// <summary>
        /// If the Work-back contract was attached, get the file link
        /// </summary>
        /// <returns></returns>
        private string LoadTrainingTypeContractURLForDelete()
        {
            string fileUrl = string.Empty;

            try
            {
                SPWeb web = SPContext.Current.Web;

                string filepath = string.Format("Contract_{0}", Convert.ToInt32(ViewState["INTERVENTIONID"]));
                SPDocumentLibrary library = web.Lists[Configs.TrainingTypeLibraryName] as SPDocumentLibrary;

                // Check if  the Folder Exist
                SPFolder Folder = library.RootFolder.SubFolders.Cast<SPFolder>().FirstOrDefault(f => f.Name.Equals(LoginName));

                if (Folder != null)
                {
                    SPFile fileitem = Folder.Files.Cast<SPFile>().FirstOrDefault(f => f.Name.StartsWith(filepath));

                    if (fileitem != null)
                        fileUrl = string.Format("<a href='/{0}' target='_blank'>Download Contract</a>", fileitem.Url);
                    else
                        fileUrl = string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return fileUrl;
        }

        /// <summary>
        /// Show or hide Work-back Contract Link
        /// </summary>
        private void ShowWorkBackLink()
        {
            if (WorkbackContractDiv.InnerHtml.Trim().Length > 0)
            {
                WorkBackContract.Visible = true;
                WorkBackFileUpload.Enabled = false;
                WorkbackContractDiv.Visible = true;
            }
            else
                WorkbackContractDiv.Visible = false;
        }

        private void ShowContractLink(string TypeOfTraining)
        {
            if (TypeOfTraining == "Specialized")
            {
                if (SpecializedContractDiv.InnerHtml.Trim().Length > 0)
                {
                    tblDisclaimer.Visible = true;
                    DisclaimerRadioButtonList.SelectedValue = "1";
                    tblSpecializedContractDIV.Visible = true;
                    SpecializedContractFileUpload.Enabled = false;
                    SpecializedContractDiv.Visible = true;
                }
                else
                    SpecializedContractDiv.Visible = false;
            }
            else if (TypeOfTraining == "Professional")
            {
                if (ProfessionalSkillFormDiv.InnerHtml.Trim().Length > 0)
                {
                    ProfessionalDIV.Visible = true;
                    ProfessionalSkillsForm.Visible = true;
                    ProfessionalSkillFormFileUpload.Enabled = false;
                    ProfessionalSkillFormDiv.Visible = true;
                }
                else
                    ProfessionalSkillFormDiv.Visible = false;
            }


        }

        /// <summary>
        /// check if the Intervention is Valid
        /// </summary>
        /// <param name="InterventionID"></param>
        /// <returns></returns>
        private bool IsInterventionValid(string InterventionID)
        {
            bool isInterventionValid = false;

            try
            {
                EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();
                EmployeeInterventionsObject employeeIntervention = new EmployeeInterventionsObject();
                employeeIntervention = dataaccess.CheckEmployeeIntervention(Convert.ToInt32(InterventionID));

                if (employeeIntervention.InterventionID <= 0)
                    isInterventionValid = false;
                else
                    isInterventionValid = true;
            }
            catch
            {
                throw new Exception("Intervention invalid, please contact the administrator.");
            }

            return isInterventionValid;
        }

        /// <summary>
        /// Set the ThresHoldValueLabel
        /// </summary>
        private void SetThresHoldValueLabel()
        {
            DisclaimerDiv.InnerText = DisclaimerDiv.InnerText.Replace("($$$)", Configs.TrainingCostThresholdLabel);
        }

        private void SetDivsToHidden()
        {
            IndividualDIV.Visible = false;
            GenericDIV.Visible = false;
            // SpecializedDisclaimerDiv.Visible = false;
            ProfessionalDIV.Visible = false;
        }

        /// <summary>
        /// Get the list of Users by SP Group Name
        /// </summary>
        /// <param name="SPGroupName"></param>
        /// <returns></returns>
        private string SPGroupUserList(string SPGroupName)
        {
            string output = string.Empty;

            SPWeb web = SPContext.Current.Web;
            {
                SPUserCollection users = web.Groups[SPGroupName].Users;

                foreach (SPUser user in users.Cast<SPUser>().OrderBy(n => n.Name))
                {
                    string LoginName = user.LoginName.Split('\\').ElementAtOrDefault(1);
                    output = output + $"{LoginName}@fic.gov.za" + ",";
                }
            }

            if (output.Length > 0)
                output = output.Remove(output.Length - 1);

            return output;
        }

        #endregion

        #region Private Methods - Email Methods

        /// <summary>
        /// Compose and send Email for the new Learning Intervention Application
        /// </summary>
        private void ComposeAndSendNewApplicationEmail()
        {
            // Compse and send Email to LD Team member
            EmailObject email = new EmailObject()
            {
                From = Configs.FromEmail,
                To = Configs.LDAdminSPGroupName,
                Subject = "New learning Intervention Application Submitted",
                Body = CreateNewApplicationEmailBody(@"C:\Program Files\Common Files\microsoft shared\Web Server Extensions\16\TEMPLATE\LAYOUTS\FIC.LD.TrainingEvaluation.SP.Solution\emailtemplates\EmployeeToLD.html")
            };

            // Send Email
            Email send = new Email();
            send.SendEmail(email, false);
        }

        /// <summary>
        /// Creates new application email body
        /// </summary>
        /// <returns></returns>
        private string CreateNewApplicationEmailBody(string etemplate)
        {
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   
            using (StreamReader reader = new StreamReader(etemplate))
            {
                body = reader.ReadToEnd();
            }
            //replacing the required strings  
            body = body.Replace("{EmployeeName}", SPContext.Current.Web.CurrentUser.Name);
            body = body.Replace("{LearningIntervention}", txtLearningIntervention.Text.Trim());

            return body;
        }

        /// <summary>
        /// Compose and send Employee work-back email
        /// </summary>
        private void ComposeAndSendWorkbackContractAttachedToLDEmail()
        {
            // Compse and send Email to LD Team member
            EmailObject email = new EmailObject()
            {
                From = Configs.FromEmail,
                To = Configs.LDAdminSPGroupName,
                Subject = string.Format("{0} - Work-back Contract Attached to the form", SPContext.Current.Web.CurrentUser.Name),
                Body = CreateEmployeeWorkBackEmailBody(@"C:\Program Files\Common Files\microsoft shared\Web Server Extensions\16\TEMPLATE\LAYOUTS\FIC.LD.TrainingEvaluation.SP.Solution\emailtemplates\EmployeeToLDWorkbackContractAttached.html")
            };

            // Send Email
            Email send = new Email();
            send.SendEmail(email, false);
        }

        /// <summary>
        /// Creates Work-back email body
        /// </summary>
        /// <returns></returns>
        private string CreateEmployeeWorkBackEmailBody(string etemplate)
        {
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   
            using (StreamReader reader = new StreamReader(etemplate))
            {
                body = reader.ReadToEnd();
            }
            //replacing the required strings  
            body = body.Replace("{EmployeeName}", SPContext.Current.Web.CurrentUser.Name);

            return body;
        }

        /// <summary>
        /// Compose and send LD work-back email
        /// </summary>
        private void ComposeAndSendWorkbackContractToEmployeeEmail()
        {
            // Get the UserName for the Current Intervention Form
            EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();

            //format name for email
            string EmployeeUserName = dataaccess.EmployeeUserName(Convert.ToInt32(ViewState["INTERVENTIONEMPLOYEEID"]));

            // Compse and send Email to Employee
            EmailObject email = new EmailObject()
            {
                From = Configs.FromEmail,
                To = $"{EmployeeUserName}@fic.gov.za",
                Subject = string.Format("Work-back Contract required"),
                Body = CreateWorkBackEmailBody(@"C:\Program Files\Common Files\microsoft shared\Web Server Extensions\16\TEMPLATE\LAYOUTS\FIC.LD.TrainingEvaluation.SP.Solution\emailtemplates\LDWorkbackContractToEmployee.html")
            };

            // Send Email
            Email send = new Email();
            send.SendEmail(email, false);
        }

        /// <summary>
        /// Creates Employee WorkBack Email body
        /// </summary>
        /// <returns></returns>
        private string CreateWorkBackEmailBody(string etemplate)
        {
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   
            using (StreamReader reader = new StreamReader(etemplate))
            {
                body = reader.ReadToEnd();
            }
            //replacing the required strings  
            body = body.Replace("{EmployeeName}", $"{txtEmpFullName.Text.Trim()}");

            return body;
        }

        /// <summary>
        /// Compose and send Manager email
        /// </summary>
        /// <returns></returns>
        private void ComposeAndSendToManagerEmail(int InterventionID)
        {
            // Compse and send Email to Manager
            EmailObject email = new EmailObject()
            {
                From = Configs.FromEmail,
                To = $"{ManagerEmailPeoplePicker.CommaSeparatedAccounts.Split('\\').ElementAtOrDefault(1)}@fic.gov.za",
                Subject = string.Format("Please approve/reject Learning Intervention"),
                Body = CreateManagerEmailBody(@"C:\Program Files\Common Files\microsoft shared\Web Server Extensions\16\TEMPLATE\LAYOUTS\FIC.LD.TrainingEvaluation.SP.Solution\emailtemplates\EmployeeToManager.html", InterventionID)
            };

            // Send Email
            Email send = new Email();
            send.SendEmail(email, false);
        }

        /// <summary>
        /// Create Manager body email
        /// </summary>
        /// <param name="etemplate"></param>
        /// <returns></returns>
        private string CreateManagerEmailBody(string etemplate, int InterventionID)
        {
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   
            using (StreamReader reader = new StreamReader(etemplate))
            {
                body = reader.ReadToEnd();
            }
            //replacing the required strings  
            body = body.Replace("{ManagerName}", ManagerEmailPeoplePicker.CommaSeparatedAccounts.Split('\\').ElementAtOrDefault(1));
            body = body.Replace("{EmployeeName}", $"{txtEmpFullName.Text.Trim()}");
            body = body.Replace("{LearningInterventionName}", txtLearningIntervention.Text.Trim());

            string _InterventionID = string.Empty;
            Encryption decrypt = new Encryption();
            _InterventionID = decrypt.Encrypt(Convert.ToString(InterventionID));

            body = body.Replace("{InterventionURL}", string.Format("{0}/CS/HR/LD/SitePages/apply.aspx?w={1}", Configs.SPSiteURL, _InterventionID));

            return body;
        }

        /// <summary>
        /// Compose and send Email to LD Manager
        /// </summary>
        private void ComposeAndSendManagerApprovedActionToLDManager()
        {
            // Compse and send Email to LD Team member
            EmailObject email = new EmailObject()
            {
                From = Configs.FromEmail,
                To = $"{SPGroupUserList(Configs.LDManagerSPGroupName)}",
                Subject = string.Format("Manager approved Employee Learning Intervention"),
                Body = CreateManagerActionEmailBody(@"C:\Program Files\Common Files\microsoft shared\Web Server Extensions\16\TEMPLATE\LAYOUTS\FIC.LD.TrainingEvaluation.SP.Solution\emailtemplates\ManagerApproved.html")
            };

            // Send Email
            Email send = new Email();
            send.SendEmail(email, false);
        }

        /// <summary>
        /// Create Manager body email
        /// </summary>
        /// <param name="etemplate"></param>
        /// <returns></returns>
        private string CreateManagerActionEmailBody(string etemplate)
        {
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   
            using (StreamReader reader = new StreamReader(etemplate))
            {
                body = reader.ReadToEnd();
            }
            //replacing the required strings  
            body = body.Replace("{ManagerName}", ManagerEmailPeoplePicker.CommaSeparatedAccounts.Split('\\').ElementAtOrDefault(1));
            body = body.Replace("{EmployeeName}", $"{txtEmpFullName.Text.Trim()}");
            body = body.Replace("{InterventionURL}", string.Format("{0}/CS/HR/LD/SitePages/apply.aspx?w={1}", Configs.SPSiteURL, Request.QueryString["w"]));

            return body;
        }

        /// <summary>
        /// Compose and Send Manager Reject Email
        /// </summary>
        private void ComposeAndSendManagerRejectedActionToLDAndEmployee()
        {
            // Get the UserName for the Current Intervention Form
            EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();

            //format name for email
            string EmployeeUserName = dataaccess.EmployeeUserName(Convert.ToInt32(ViewState["INTERVENTIONEMPLOYEEID"]));

            // Compse and send Email to LD Team member
            EmailObject email = new EmailObject()
            {
                From = Configs.FromEmail,
                To = $"{EmployeeUserName}@fic.gov.za",
                CC = $"{SPGroupUserList(Configs.LDAdminSPGroupName)}",
                Subject = string.Format("Employee Learning Intervention Rejected by Line Manager"),
                Body = CreateManagerRejectedEmailBody(@"C:\Program Files\Common Files\microsoft shared\Web Server Extensions\16\TEMPLATE\LAYOUTS\FIC.LD.TrainingEvaluation.SP.Solution\emailtemplates\ManagerRejected.html")
            };

            // Send Email
            Email send = new Email();
            send.SendEmail(email, true);
        }

        /// <summary>
        /// Create Reject Email
        /// </summary>
        /// <param name="etemplate"></param>
        /// <returns></returns>
        private string CreateManagerRejectedEmailBody(string etemplate)
        {
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   
            using (StreamReader reader = new StreamReader(etemplate))
            {
                body = reader.ReadToEnd();
            }
            //replacing the required strings  
            body = body.Replace("{ManagerName}", ManagerEmailPeoplePicker.CommaSeparatedAccounts.Split('\\').ElementAtOrDefault(1));
            body = body.Replace("{EmployeeName}", $"{txtEmpFullName.Text.Trim()}");
            body = body.Replace("{RejectReason}", ManagerRejectReasonTextbox.Text.Trim());

            return body;
        }

        /// <summary>
        /// Compose and send Email to LD
        /// </summary>
        private void ComposeAndSendLDManagerEmailApproval()
        {
            // Get the UserName for the Current Intervention Form
            EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();

            //format name for email
            string EmployeeUserName = dataaccess.EmployeeUserName(Convert.ToInt32(ViewState["INTERVENTIONEMPLOYEEID"]));

            // Compse and send Email to LD Team member
            EmailObject email = new EmailObject()
            {
                From = Configs.FromEmail,
                To = $"{EmployeeUserName}@fic.gov.za",
                CC = $"{SPGroupUserList(Configs.LDAdminSPGroupName)}",
                Subject = string.Format("LD Manager approved Employee Learning Intervention"),
                Body = CreateLDManagerApprovedEmailBody(@"C:\Program Files\Common Files\microsoft shared\Web Server Extensions\16\TEMPLATE\LAYOUTS\FIC.LD.TrainingEvaluation.SP.Solution\emailtemplates\LDManagerApproved.html")
            };

            // Send Email
            Email send = new Email();
            send.SendEmail(email, true);
        }

        /// <summary>
        /// Create Manager body email
        /// </summary>
        /// <param name="etemplate"></param>
        /// <returns></returns>
        private string CreateLDManagerApprovedEmailBody(string etemplate)
        {
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   
            using (StreamReader reader = new StreamReader(etemplate))
            {
                body = reader.ReadToEnd();
            }
            //replacing the required strings  
            body = body.Replace("{ManagerName}", ManagerEmailPeoplePicker.CommaSeparatedAccounts.Split('\\').ElementAtOrDefault(1));
            body = body.Replace("{EmployeeName}", $"{txtEmpFullName.Text.Trim()}");

            body = body.Replace("{InterventionURL}", string.Format("{0}/CS/HR/LD/SitePages/apply.aspx?w={1}", Configs.SPSiteURL, Request.QueryString["w"]));

            return body;
        }

        /// <summary>
        /// Compose and Send Manager Reject Email
        /// </summary>
        private void ComposeAndSendLDManagerRejectedActionToLDAndEmployee()
        {
            // Get the UserName for the Current Intervention Form
            EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();

            //format name for email
            string EmployeeUserName = dataaccess.EmployeeUserName(Convert.ToInt32(ViewState["INTERVENTIONEMPLOYEEID"]));

            // Compse and send Email to LD Team member
            EmailObject email = new EmailObject()
            {
                From = Configs.FromEmail,
                To = $"{EmployeeUserName}@fic.gov.za,{ManagerEmailPeoplePicker.CommaSeparatedAccounts.Split('\\').ElementAtOrDefault(1)}@fic.gov.za",
                CC = $"{SPGroupUserList(Configs.LDAdminSPGroupName)}",
                Subject = string.Format("L&D Manager Rejected Employee Learning Intervention"),
                Body = CreateLDManagerRejectedEmailBody(@"C:\Program Files\Common Files\microsoft shared\Web Server Extensions\16\TEMPLATE\LAYOUTS\FIC.LD.TrainingEvaluation.SP.Solution\emailtemplates\LDManagerRejected.html")
            };

            // Send Email
            Email send = new Email();
            send.SendEmail(email, true);
        }

        /// <summary>
        /// Create Reject Email
        /// </summary>
        /// <param name="etemplate"></param>
        /// <returns></returns>
        private string CreateLDManagerRejectedEmailBody(string etemplate)
        {
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   
            using (StreamReader reader = new StreamReader(etemplate))
            {
                body = reader.ReadToEnd();
            }
            //replacing the required strings  
            body = body.Replace("{LDManagerName}", SPUserLoginName.Name);
            body = body.Replace("{EmployeeName}", $"{txtEmpFullName.Text.Trim()}");
            body = body.Replace("{RejectReason}", LDManagerRejectReasonTextBox.Text.Trim());

            return body;
        }

        /// <summary>
        /// Compose and Send Manager Approve Email To Employee
        /// </summary>
        private void ComposeAndSendApprovedEmailToEmployee()
        {
            // Get the UserName for the Current Intervention Form
            EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();

            //format name for email
            string EmployeeUserName = dataaccess.EmployeeUserName(Convert.ToInt32(ViewState["INTERVENTIONEMPLOYEEID"]));

            // Compse and send Email to LD Team member
            EmailObject email = new EmailObject()
            {
                From = Configs.FromEmail,
                To = $"{EmployeeUserName}@fic.gov.za",
                Subject = "Learning Intervention has been Approved",
                Body = CreateLDAdminApprovedEmailBody(@"C:\Program Files\Common Files\microsoft shared\Web Server Extensions\16\TEMPLATE\LAYOUTS\FIC.LD.TrainingEvaluation.SP.Solution\emailtemplates\LDAdminApproved.html")
            };

            // Send Email
            Email send = new Email();
            send.SendEmail(email, false);
        }

        /// <summary>
        /// Create Approve Email body For Employee
        /// </summary>
        /// <param name="etemplate"></param>
        /// <returns></returns>
        private string CreateLDAdminApprovedEmailBody(string etemplate)
        {
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   
            using (StreamReader reader = new StreamReader(etemplate))
            {
                body = reader.ReadToEnd();
            }

            //replacing the required strings            
            body = body.Replace("{Employee}", txtEmpFullName.Text.Trim());

            return body;
        }

        /// <summary>
        /// Create Reject Email For Employee
        /// </summary>
        private void ComposeAndSendRejectedEmailToEmployee()
        {
            // Get the UserName for the Current Intervention Form
            EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();

            //format name for email
            string EmployeeUserName = dataaccess.EmployeeUserName(Convert.ToInt32(ViewState["INTERVENTIONEMPLOYEEID"]));

            // Compse and send Email to LD Team member
            EmailObject email = new EmailObject()
            {
                From = Configs.FromEmail,
                To = $"{EmployeeUserName}@fic.gov.za",
                Subject = "Learning Intervention has been Rejected",
                Body = CreateLDAdminRejectedEmailBody(@"C:\Program Files\Common Files\microsoft shared\Web Server Extensions\16\TEMPLATE\LAYOUTS\FIC.LD.TrainingEvaluation.SP.Solution\emailtemplates\LDAdminRejected.html")
            };

            // Send Email
            Email send = new Email();
            send.SendEmail(email, false);
        }

        /// <summary>
        /// Create Reject Email body
        /// </summary>
        /// <param name="etemplate"></param>
        /// <returns></returns>
        private string CreateLDAdminRejectedEmailBody(string etemplate)
        {
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   
            using (StreamReader reader = new StreamReader(etemplate))
            {
                body = reader.ReadToEnd();
            }

            //replacing the required strings            
            body = body.Replace("{Employee}", txtEmpFullName.Text.Trim());
            body = body.Replace("{RejectReason}", LDAdminRejectReasonTextBox.Text.Trim());

            return body;
        }


        #endregion
    }
}
