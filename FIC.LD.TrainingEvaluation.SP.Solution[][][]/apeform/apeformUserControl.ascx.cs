using FIC.LD.TrainingEvaluation.SP.Solution.Helper;
using Microsoft.SharePoint;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


namespace FIC.LD.TrainingEvaluation.SP.Solution.apeform
{
    public partial class apeformUserControl : UserControl
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            //Hide Error message container
            ErrorMessageDiv.Visible = false;

            if (!IsPostBack)
            {
                try
                {
                    SetUserDetails();
                }
                catch (Exception ex)
                {
                    MainContent.Visible = false;
                    ErrorMessageDiv.Visible = true;
                    ErrorMessageDiv.InnerText = ex.Message;
                }
            }
        }

        private void SetUserDetails()
        {
            string[] splitUserName = null;
            string splitName = String.Empty;
            string splitSName = String.Empty;
            string splitFullName = String.Empty;

            if (Request.QueryString["w"] != null)
            {
                Encryption decrypt = new Encryption();
                int InterventionID = Convert.ToInt32(decrypt.Decrypt(Request.QueryString["w"]));
                ViewState["InterventionID"] = InterventionID;

                // Checks if the Post Learning Action Plan Form is Completed
                if (PostLearningActionPlanCompleted(InterventionID))
                {
                    string capitalizeLoginName = LoginName;
                    capitalizeLoginName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(capitalizeLoginName.ToLower());

                    EmployeeInterventionsDataAccess EmployeeLearningDataAccess = new EmployeeInterventionsDataAccess();
                    EmployeeDetailsObject EmployeeDetails = new EmployeeDetailsObject();
                    EmployeeDetails = EmployeeLearningDataAccess.EmployeeDetails(InterventionID);

                    //Remove the comma on the Full name
                    if (EmployeeDetails.FullName.Contains(","))
                    {
                        splitUserName = EmployeeDetails.FullName.Split(',');

                        splitName = splitUserName[0].ToString();
                        splitSName = splitUserName[1].ToString();

                        splitFullName = splitName.Trim() + "." + splitSName.Trim();
                    }
                    else
                    {
                        splitUserName = EmployeeDetails.FullName.Split(' ');

                        splitName = splitUserName[0].ToString();
                        splitSName = splitUserName[1].ToString();

                        splitFullName = splitName.Trim() + "." + splitSName.Trim();
                    }

                    if (EmployeeDetails.ManagerName == capitalizeLoginName)
                    {
                        SetEmployeeDetails(InterventionID);
                        SetManagerAPEDetails(InterventionID);

                        EmployeePerformanceImprovementDiv.Visible = true;

                        tblEmployeeAPE.Visible = false;
                    }
                    else if (splitFullName == capitalizeLoginName)
                    {
                        if (APESectionOneCompleted(InterventionID))
                        {
                            SetEmployeeDetails(InterventionID);
                            SetEmployeeAPEDetails(InterventionID);
                        }
                        else
                        {
                            throw new Exception("Please note that your Line Manager has to complete Section 1 of the Action Plan Evaluation form before you work on it.");
                        }
                    }
                }
                else
                {
                    throw new Exception("Please complete the Post Learning Action Plan before working on the current form.");
                }
            }
            else
            {
                throw new Exception("Form is currently not available. Please contact the system Administrator.");
            }
        }

        /// <summary>
        /// Checks if the Post Employee Intervention Evaluation Form is Completed
        /// </summary>
        /// <returns></returns>
        private bool PostLearningActionPlanCompleted(int InterventionID)
        {
            int? IsSubmitted = null;

            PostLearningActionPlanCompletedObject completed = new PostLearningActionPlanCompletedObject();
            completed.InterventionID = InterventionID;

            PostLearningActionPlanDataAccess dataaccess = new PostLearningActionPlanDataAccess();
            IsSubmitted = dataaccess.IsPostLearningActionPlanCompleted(completed);

            return IsSubmitted == 1 ? true : false;
        }

        /// <summary>
        /// Checks if SECTION 1 - Manager Section part of the form is Completed
        /// </summary>
        /// <returns></returns>
        private bool APESectionOneCompleted(int InterventionID)
        {
            int? IsSubmitted = null;

            ManagerActionPlanEvaluationObject completed = new ManagerActionPlanEvaluationObject();
            completed.APEInterventionID = InterventionID;

            ActionPlanEvaluationDataAccess dataaccess = new ActionPlanEvaluationDataAccess();
            IsSubmitted = dataaccess.IsAPESectionOneCompleted(completed);

            return IsSubmitted == 1 ? true : false;
        }

        /// <summary>
        /// Set the Employee or Manager deatils
        /// </summary>
        private void SetEmployeeDetails(int InterventionID)
        {
            EmployeeInterventionsDataAccess EmployeeLearningDataAccess = new EmployeeInterventionsDataAccess();

            string FromDate = String.Empty;
            string ToDate = String.Empty;
            int? EmployeeID = null;

            EmployeeID = EmployeeLearningDataAccess.EmployeeID(LoginName);

            try
            {
                if (EmployeeID != null)
                {
                    EmployeeDetailsObject EmployeeDetails = new EmployeeDetailsObject();
                    EmployeeDetails = EmployeeLearningDataAccess.EmployeeDetails(InterventionID);

                    if (EmployeeDetails != null)
                    {
                        string capitalizeLoginName = LoginName;
                        capitalizeLoginName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(capitalizeLoginName.ToLower());

                        if (EmployeeDetails != null && EmployeeDetails.EmployeeID == EmployeeID)
                        {
                            txtEmpFullName.Text = EmployeeDetails.FullName;
                            txtDesignation.Text = EmployeeDetails.Designation;
                            txtDepartment.Text = EmployeeDetails.Department;
                            txtLearningIntervention.Text = EmployeeDetails.NameOfLearning;
                            txtManager.Text = EmployeeDetails.ManagerName;

                            FromDate = Convert.ToDateTime(EmployeeDetails.StartDateOfTraining).ToString("dd-MMMM-yyyy");
                            ToDate = Convert.ToDateTime(EmployeeDetails.EndDateOfTraining).ToString("dd-MMMM-yyyy");

                            txtTrainingDates.Text = FromDate + " To " + ToDate;
                        }
                        else if (EmployeeDetails != null && EmployeeDetails.ManagerName == capitalizeLoginName)
                        {
                            txtEmpFullName.Text = EmployeeDetails.FullName;
                            txtDesignation.Text = EmployeeDetails.Designation;
                            txtDepartment.Text = EmployeeDetails.Department;
                            txtLearningIntervention.Text = EmployeeDetails.NameOfLearning;
                            txtManager.Text = EmployeeDetails.ManagerName;

                            FromDate = Convert.ToDateTime(EmployeeDetails.StartDateOfTraining).ToString("dd-MMMM-yyyy");
                            ToDate = Convert.ToDateTime(EmployeeDetails.EndDateOfTraining).ToString("dd-MMMM-yyyy");

                            txtTrainingDates.Text = FromDate + " To " + ToDate;
                        }
                        else
                        {
                            throw new Exception("Access denied");
                        }

                        ViewState["FullEmpName"] = EmployeeDetails.FullName;
                        ViewState["ManagerName"] = EmployeeDetails.ManagerName;
                        ViewState["NameOfLearning"] = EmployeeDetails.NameOfLearning;
                    }
                    else
                    {
                        throw new Exception("Access denied");
                    }
                }
            }
            catch (Exception ex)
            {
                //
            }
        }

        protected void ManagerSubmit_Click(object sender, EventArgs e)
        {
            ErrorMessageDiv.Visible = false;

            try
            {
                //Insert Manager APE
                InsertManagerActionPlanEvaluation();
                //Send Notification from Manager to Emp for completion of sec 2
                // SendAPEEmailFromManagerToEmployee();
            }
            catch (Exception ex)
            {
                ErrorMessageDiv.Visible = true;
                ErrorMessageDiv.InnerText = ex.Message;
            }
        }

        /// <summary>
        /// Insert Manager Action Plan Evaluation 
        /// </summary>
        private void InsertManagerActionPlanEvaluation()
        {
            Encryption decrypt = new Encryption();

            bool pageValidationValid = false;
            int? EmployeeID = 0;

            try
            {
                ActionPlanEvaluationDataAccess actionPlanEvaluationDataAccess = new ActionPlanEvaluationDataAccess();

                EmployeeInterventionsDataAccess EmpDataaccess = new EmployeeInterventionsDataAccess();
                EmployeeID = EmpDataaccess.EmployeeID(LoginName);

                if (EmployeeID != null)
                {
                    ManagerActionPlanEvaluationObject ManagerAPEObject = new ManagerActionPlanEvaluationObject() { APEInterventionID = Convert.ToInt32(decrypt.Decrypt(Request.QueryString["w"])), APEEmployeeID = (int?)EmployeeID, SkillsDemonstration = Convert.ToInt32(SkillsDemonstrationDropDownList.SelectedValue), ManagerApplicationOfNewSkills = txtManagerApplicationOfNewSkills.Value, JobPerformanceChanges = txtJobPerformanceChanges.Value, AdditionalComments = txtAdditionalComments.Value, EmployeePerformanceImprovement = Convert.ToInt32(EmployeePerformanceImprovementDropDownList.SelectedValue), ManagerSubmitted = 1, EmployeeSubmitted = 0 };
                    actionPlanEvaluationDataAccess.InsertManagerActionPlanEvaluation(ManagerAPEObject);

                    pageValidationValid = true;
                }

                if (pageValidationValid == true)
                {
                    SendAPEEmailFromManagerToEmployee();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (pageValidationValid)
                    Response.Redirect("/CS/HR/LD/SitePages/managerdashboard.aspx");
            }
        }

        /// <summary>
        ///Notify Employee that Manager has completed Action Plan Evaluation Section 1
        /// </summary>
        private void SendAPEEmailFromManagerToEmployee()
        {
            string EmpFullName = (string)ViewState["FullEmpName"];
            EmpFullName = EmpFullName.Replace(" ", ".");

            // Compse and send Email to LD Team member
            EmailObject email = new EmailObject()
            {
                From = Configs.FromEmail,
                To = $"{EmpFullName}@fic.gov.za",
                Subject = "Action Plan Evaluation - Section 1",
                Body = CreateAPECompleteEmployeeToLDEmailBody(@"C:\Program Files\Common Files\microsoft shared\Web Server Extensions\16\TEMPLATE\LAYOUTS\FIC.LD.TrainingEvaluation.SP.Solution\emailtemplates\APENotificeFromManagerToEmployee.html")
            };

            // Send Email
            Email send = new Email();
            send.SendEmail(email, false);
        }

        /// <summary>
        /// Creates new application email body
        /// </summary>
        /// <returns></returns>
        private string CreateAPECompleteEmployeeToLDEmailBody(string etemplate)
        {
            string ManagerName = (string)ViewState["ManagerName"];
            //ManagerName = ManagerName.Replace(" ", ".");

            string body = string.Empty;
            //using streamreader for reading my htmltemplate   
            using (StreamReader reader = new StreamReader(etemplate))
            {
                body = reader.ReadToEnd();
            }
            //replacing the required strings             
            body = body.Replace("{EmployeeName}", txtEmpFullName.Text);
            body = body.Replace("{ManagerName}", ManagerName);
            body = body.Replace("{InterventionURL}", $"{Configs.SPSiteURL}/CS/HR/LD/SitePages/ActionPlan.aspx?w={Request.QueryString["w"]}");
            return body;
        }

        /// <summary>
        /// Set Manager Action Plan Evaluation - Section 1 
        /// </summary>
        private void SetManagerAPEDetails(int InterventionID)
        {
            EmployeeInterventionsDataAccess EmployeeLearningDataAccess = new EmployeeInterventionsDataAccess();

            string FromDate = String.Empty;
            string ToDate = String.Empty;
            int? EmployeeID = null;

            EmployeeID = EmployeeLearningDataAccess.EmployeeID(LoginName);

            try
            {
                if (EmployeeID != null)
                {
                    string capitalizeLoginName = LoginName;
                    capitalizeLoginName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(capitalizeLoginName.ToLower());

                    ActionPlanEvaluationDataAccess ActionPlanEvaluationDataAccess = new ActionPlanEvaluationDataAccess();
                    ManagerActionPlanEvaluationObject ManagerAPEObject = new ManagerActionPlanEvaluationObject();

                    ManagerAPEObject = ActionPlanEvaluationDataAccess.GetManagerActionPlanEvaluationSection(InterventionID);

                    EmployeeDetailsObject EmployeeDetails = new EmployeeDetailsObject();
                    EmployeeDetails = EmployeeLearningDataAccess.EmployeeDetails(InterventionID);

                    if (ManagerAPEObject != null)
                    {
                        if (ManagerAPEObject != null && EmployeeDetails.ManagerName == capitalizeLoginName)
                        {
                            tblEmployeeAPE.Visible = false;

                            SkillsDemonstrationDropDownList.Text = Convert.ToString(ManagerAPEObject.SkillsDemonstration);
                            txtManagerApplicationOfNewSkills.Value = Convert.ToString(ManagerAPEObject.ManagerApplicationOfNewSkills);
                            txtJobPerformanceChanges.Value = Convert.ToString(ManagerAPEObject.JobPerformanceChanges);
                            txtAdditionalComments.Value = Convert.ToString(ManagerAPEObject.AdditionalComments);
                            EmployeePerformanceImprovementDropDownList.Text = Convert.ToString(ManagerAPEObject.EmployeePerformanceImprovement);

                            DisableManagerControlsIfSubmittedBefore();
                        }
                        else
                        {
                            throw new Exception("Access denied");
                        }
                    }
                    else
                    {
                        if (EmployeeDetails.ManagerName == capitalizeLoginName)
                        {
                            tblEmployeeAPE.Visible = false;
                            EmployeeSubmit.Visible = false;
                        }

                        throw new Exception("Access denied");
                    }
                }
            }
            catch (Exception ex)
            {
                //
            }
        }

        ///// <summary>
        /////  Set Employee Action Plan Evaluation - Section 2
        ///// </summary>
        private void SetEmployeeAPEDetails(int InterventionID)
        {
            EmployeeInterventionsDataAccess EmployeeLearningDataAccess = new EmployeeInterventionsDataAccess();

            string FromDate = String.Empty;
            string ToDate = String.Empty;
            int? EmployeeID = null;

            EmployeeID = EmployeeLearningDataAccess.EmployeeID(LoginName);

            try
            {
                Encryption decrypt = new Encryption();

                if (EmployeeID != null)
                {
                    ActionPlanEvaluationDataAccess ActionPlanEvaluationDataAccess = new ActionPlanEvaluationDataAccess();
                    EmployeeActionPlanEvaluationObject EmployeeAPEObject = new EmployeeActionPlanEvaluationObject();

                    EmployeeAPEObject = ActionPlanEvaluationDataAccess.GetEmployeeActionPlanEvaluationSection(InterventionID);

                    if (EmployeeAPEObject != null)
                    {
                        EmployeeDetailsObject EmployeeDetails = new EmployeeDetailsObject();
                        EmployeeDetails = EmployeeLearningDataAccess.EmployeeDetails(InterventionID);

                        if (EmployeeAPEObject != null && EmployeeDetails.EmployeeID == EmployeeID)
                        {
                            tblEmployeeAPE.Visible = true;

                            ManagerActionPlanEvaluationObject ManagerAPEObject = new ManagerActionPlanEvaluationObject();

                            ManagerAPEObject = ActionPlanEvaluationDataAccess.GetManagerActionPlanEvaluationSection(InterventionID);

                            //Set Manager Deatils
                            SkillsDemonstrationDropDownList.Text = Convert.ToString(ManagerAPEObject.SkillsDemonstration);
                            txtManagerApplicationOfNewSkills.Value = Convert.ToString(ManagerAPEObject.ManagerApplicationOfNewSkills);
                            txtJobPerformanceChanges.Value = Convert.ToString(ManagerAPEObject.JobPerformanceChanges);
                            txtAdditionalComments.Value = Convert.ToString(ManagerAPEObject.AdditionalComments);

                            //Disable Manager Controls
                            DisableManagerControlsIfSubmittedBefore();

                            //Set Employee Controls
                            LearningInterventionRatingDropDownList.SelectedItem.Text = Convert.ToString(EmployeeAPEObject.LearningInterventionRating);
                            EmployeeApplicationOfNewSkillsChoiceDropDownList.Text = Convert.ToString(EmployeeAPEObject.EmployeeApplicationOfNewSkillsChoice);
                            txtEmployeeApplicationOfNewSkillsComments.Value = Convert.ToString(EmployeeAPEObject.EmployeeApplicationOfNewSkillsComments);
                            RequiredResourcesAndSupportOfferedChoiceDropDownList.Text = Convert.ToString(EmployeeAPEObject.RequiredResourcesAndSupportOfferedChoice);
                            txtRequiredResourcesAndSupportOfferedComments.Value = Convert.ToString(EmployeeAPEObject.RequiredResourcesAndSupportOfferedComments);
                            txtEmployeeNewKnowledge.Value = Convert.ToString(EmployeeAPEObject.EmployeeNewKnowledge);

                            //Lock form controls if the employee details are not null
                            if (EmployeeAPEObject.LearningInterventionRating != null)
                            {
                                DisableEmployeeControlsIfSubmittedBefore();
                            }
                        }
                        else
                        {
                            throw new Exception("Access denied");
                        }
                    }
                    else
                    {
                        throw new Exception("Access denied");
                    }
                }
            }
            catch (Exception ex)
            {
                //
            }
        }

        /// <summary>
        /// Disable Controls when login person is manager and the record was sublited
        /// </summary>
        public void DisableManagerControlsIfSubmittedBefore()
        {
            ManagerSubmit.Visible = false;
            ManagerSubmit.Enabled = false;

            SkillsDemonstrationDropDownList.Enabled = false;
            txtManagerApplicationOfNewSkills.Disabled = true;
            txtJobPerformanceChanges.Disabled = true;
            txtAdditionalComments.Disabled = true;
            EmployeePerformanceImprovementDropDownList.Enabled = false;
            EmployeePerformanceImprovementDiv.Visible = false;
        }

        /// <summary>
        /// Disable Controls when login person is employee and the record was sublited
        /// </summary>
        public void DisableEmployeeControlsIfSubmittedBefore()
        {
            //Manager Controls
            ManagerSubmit.Enabled = false;
            ManagerSubmit.Visible = false;

            SkillsDemonstrationDropDownList.Enabled = false;
            txtManagerApplicationOfNewSkills.Disabled = true;
            txtJobPerformanceChanges.Disabled = true;
            txtAdditionalComments.Disabled = true;

            //Employee Controls
            EmployeeSubmit.Enabled = false;

            LearningInterventionRatingDropDownList.Enabled = false;
            EmployeeApplicationOfNewSkillsChoiceDropDownList.Enabled = false;
            txtEmployeeApplicationOfNewSkillsComments.Disabled = true;
            RequiredResourcesAndSupportOfferedChoiceDropDownList.Enabled = false;
            txtRequiredResourcesAndSupportOfferedComments.Disabled = true;
            txtEmployeeNewKnowledge.Disabled = true;

            ManagerSubmit.Enabled = false;
        }

        protected void EmployeeSubmit_Click(object sender, EventArgs e)
        {
            ErrorMessageDiv.Visible = false;

            try
            {
                UpdateEmployeeActionPlanEvaluation();
            }
            catch (Exception ex)
            {
                ErrorMessageDiv.Visible = true;
                ErrorMessageDiv.InnerText = ex.Message;
            }
        }

        /// <summary>
        /// Insert Employee Action Plan Evaluation 
        /// </summary>
        private void UpdateEmployeeActionPlanEvaluation()
        {
            Encryption decrypt = new Encryption();

            bool pageValidationValid = false;
            int? EmployeeID = 0;
            int InterventionID = 0;

            try
            {
                ActionPlanEvaluationDataAccess actionPlanEvaluationDataAccess = new ActionPlanEvaluationDataAccess();

                EmployeeInterventionsDataAccess EmpDataaccess = new EmployeeInterventionsDataAccess();
                EmployeeID = EmpDataaccess.EmployeeID(LoginName);

                EmployeeActionPlanEvaluationObject EmployeeAPEObject = null;

                if (EmployeeID != null)
                {
                    EmployeeAPEObject = new EmployeeActionPlanEvaluationObject() { APEInterventionID = Convert.ToInt32(decrypt.Decrypt(Request.QueryString["w"])), APEEmployeeID = (int?)EmployeeID, LearningInterventionRating = LearningInterventionRatingDropDownList.SelectedItem.Text, EmployeeApplicationOfNewSkillsChoice = Convert.ToInt32(EmployeeApplicationOfNewSkillsChoiceDropDownList.SelectedValue), EmployeeApplicationOfNewSkillsComments = txtEmployeeApplicationOfNewSkillsComments.Value, RequiredResourcesAndSupportOfferedChoice = Convert.ToInt32(RequiredResourcesAndSupportOfferedChoiceDropDownList.SelectedValue), RequiredResourcesAndSupportOfferedComments = txtRequiredResourcesAndSupportOfferedComments.Value, EmployeeNewKnowledge = txtEmployeeNewKnowledge.Value, EmployeeSubmitted = 1, EmployeeDateCreated = Convert.ToDateTime(DateTime.Now.ToString("dd-MMMM-yyyy")) };
                    // Insert Intervention
                    InterventionID = actionPlanEvaluationDataAccess.UpdateEmployeeActionPlanEvaluation(EmployeeAPEObject);

                    pageValidationValid = true;
                }

                if (pageValidationValid == true)
                {
                    SendAPEEmailFromEmployeeToLDTeam(InterventionID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (pageValidationValid)
                    Response.Redirect("/CS/HR/LD/SitePages/dashboard.aspx");
            }
        }

        /// <summary>
        /// Notify LD Team that Employee has completed Action Plan Evaluation Section 2
        /// </summary>
        private void SendAPEEmailFromEmployeeToLDTeam(int InterventionID)
        {
            // Compse and send Email to LD Team member
            EmailObject email = new EmailObject()
            {
                From = Configs.FromEmail,
                To = SPGroupUserList(Configs.LDAdminSPGroupName),
                Subject = "Action Plan Evaluation - Section 2",
                Body = CreateAPEEmailFromEmployeeToLDEmailBody(@"C:\Program Files\Common Files\microsoft shared\Web Server Extensions\16\TEMPLATE\LAYOUTS\FIC.LD.TrainingEvaluation.SP.Solution\emailtemplates\APEEmailFromEmployeeToLDTeam.html", InterventionID)
            };

            // Send Email
            Email send = new Email();
            send.SendEmail(email, false);
        }

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

        /// <summary>
        /// Creates new application email body
        /// </summary>
        /// <returns></returns>
        private string CreateAPEEmailFromEmployeeToLDEmailBodys(string etemplate, int InterventionID)
        {
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   
            using (StreamReader reader = new StreamReader(etemplate))
            {
                body = reader.ReadToEnd();
            }
            //replacing the required strings             
            body = body.Replace("{EmployeeName}", txtEmpFullName.Text);
            body = body.Replace("{LearningIntervention}", txtLearningIntervention.Text);
            body = body.Replace("{InterventionURL}", $"{Configs.SPSiteURL}/CS/HR/LD/SitePages/ActionPlan.aspx?w={Request.QueryString["w"]}");
            return body;
        }

        private string CreateAPEEmailFromEmployeeToLDEmailBody(string etemplate, int InterventionID)
        {
            string EmpFullName = (string)ViewState["FullEmpName"];
            //EmpFullName = EmpFullName.Replace(" ", ".");

            string NameOfLearning = (string)ViewState["NameOfLearning"];
           // NameOfLearning = NameOfLearning.Replace(" ", ".");
            
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   
            using (StreamReader reader = new StreamReader(etemplate))
            {
                body = reader.ReadToEnd();
            }
            //replacing the required strings  
            body = body.Replace("{EmployeeName}", EmpFullName);
            body = body.Replace("{LearningIntervention}", NameOfLearning);

            string _InterventionID = string.Empty;
            Encryption decrypt = new Encryption();
            _InterventionID = decrypt.Encrypt(Convert.ToString(InterventionID));

            // body = body.Replace("{InterventionURL}", string.Format("{0}/CS/HR/LD/SitePages/apply.aspx?w={1}", Configs.SPSiteURL, _InterventionID));
            body = body.Replace("{InterventionURL}", $"{Configs.SPSiteURL}/CS/HR/LD/SitePages/ActionPlan.aspx?w={Request.QueryString["w"]}");

            return body;
        }
    }
}
