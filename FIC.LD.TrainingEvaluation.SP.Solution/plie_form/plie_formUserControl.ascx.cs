using FIC.LD.TrainingEvaluation.SP.Solution.Helper;
using Microsoft.SharePoint;
using System;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


namespace FIC.LD.TrainingEvaluation.SP.Solution.plie_form
{
    public partial class plie_formUserControl : UserControl
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
                    if (Request.QueryString["w"] != null)
                    {
                        Encryption decrypt = new Encryption();
                        int InterventionID = Convert.ToInt32(decrypt.Decrypt(Request.QueryString["w"]));
                        // Checks if the Individual Skills Development Form is Completed
                        if (IndividualSkillsDevelopmentApprovalCompleted(InterventionID))
                        {
                            ViewState["InterventionID"] = InterventionID;

                            SetEmployeeDetails(InterventionID);
                            SetEmployeeSurvey(InterventionID);
                        }
                        else
                            throw new Exception("Please make sure that the Individual Skills Development Form has been completed before working on the current form.");
                    }
                    else
                    {
                        throw new Exception("Form is currently not available. Please contact the system Administrator.");
                    }
                }
                catch (Exception ex)
                {
                    MainContent.Visible = false;
                    ErrorMessageDiv.Visible = true;
                    ErrorMessageDiv.InnerText = ex.Message;
                }
            }
        }

        /// <summary>
        /// Checks if the Individual Skills Development Approval Form is Completed
        /// </summary>
        /// <returns></returns>
        private bool IndividualSkillsDevelopmentApprovalCompleted(int InterventionID)
        {
            int? IsSubmitted = null;

            IndividualSkillsDevelopmentApprovalCompletedObjects completed = new IndividualSkillsDevelopmentApprovalCompletedObjects();
            completed.InterventionID = InterventionID;

            EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();
            IsSubmitted = dataaccess.IsIndividualSkillsDevelopmentApprovalCompleted(completed);

            return IsSubmitted == 1 ? true : false;
        }

        /// <summary>
        /// Set the Employee and the survey details if submitted
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
                        if (EmployeeDetails != null && EmployeeDetails.EmployeeID == EmployeeID)
                        {
                            txtEmpFullName.Text = EmployeeDetails.FullName;
                            txtDepartment.Text = EmployeeDetails.Department;
                            txtLearningIntervention.Text = EmployeeDetails.NameOfLearning;
                            txtServiceProvider.Text = EmployeeDetails.ServiceProvider;

                            FromDate = Convert.ToDateTime(EmployeeDetails.StartDateOfTraining).ToString("dd-MMMM-yyyy");
                            ToDate = Convert.ToDateTime(EmployeeDetails.EndDateOfTraining).ToString("dd-MMMM-yyyy");

                            txtTrainingDates.Text = FromDate + " To " + ToDate;
                        }
                        else
                        {
                            throw new Exception("Access denied");
                        }
                        
                        ViewState["FullEmpName"] = EmployeeDetails.FullName;
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

        private void SetEmployeeSurvey(int InterventionID)
        {
            PostLearningInterventionsDataAccess PostLearningDataAccess = new PostLearningInterventionsDataAccess();
            PostLearningInterventionEvaluationEmployeeObject POSTLearningInterventionObjectResults = null;

            try
            {
                POSTLearningInterventionObjectResults = PostLearningDataAccess.GetPOSTInterventionSurvey(InterventionID);

                if (POSTLearningInterventionObjectResults != null)
                {
                    CourseWorthTimeandExpectationsDropDownList.SelectedItem.Text = POSTLearningInterventionObjectResults.CourseWorthTimeandExpectations;
                    ApplySkillsLearnedDropDownList.SelectedItem.Text = POSTLearningInterventionObjectResults.ApplySkillsLearned;
                    QualityOfPresenterDropDownList.SelectedItem.Text = POSTLearningInterventionObjectResults.QualityOfPresenter;
                    LearningInterventionCoordinationDropDownList.SelectedItem.Text = POSTLearningInterventionObjectResults.LearningInterventionCoordination;
                    txtHighlightsOfTraining.Value = POSTLearningInterventionObjectResults.HighlightsOfTraining;

                    DisableControlsIfSubmittedBefore();
                }
            }
            catch (Exception ex)
            {

            }

        }

        /// <summary>
        /// Disable Controls If Survey was Submitted Before
        /// </summary>
        public void DisableControlsIfSubmittedBefore()
        {
            EmployeeSubmit.Enabled = false;
            CourseWorthTimeandExpectationsDropDownList.Enabled = false;
            ApplySkillsLearnedDropDownList.Enabled = false;
            QualityOfPresenterDropDownList.Enabled = false;
            LearningInterventionCoordinationDropDownList.Enabled = false;
            txtHighlightsOfTraining.Disabled = true;
        }

        protected void EmployeeSubmit_Click(object sender, EventArgs e)
        {
            ErrorMessageDiv.Visible = false;

            try
            {
                InsertPOSTInterventionevaluation();
                //SendEmailToLDAdmin();
            }
            catch (Exception ex)
            {
                ErrorMessageDiv.Visible = true;
                ErrorMessageDiv.InnerText = ex.Message;
            }
        }

        /// <summary>
        /// Send an Email to LD for about the PLIE submission
        /// </summary>
        private void SendEmailToLDAdmin()
        {
            // Compse and send Email to LD Team member
            EmailObject email = new EmailObject()
            {
                From = Configs.FromEmail,
                To = $"{SPGroupUserList(Configs.LDAdminSPGroupName)}",
                Subject = "Post Learning Intervention Evaluation",
                Body = CreatePLIECompleteEmployeeToLDEmailBody(@"C:\Program Files\Common Files\microsoft shared\Web Server Extensions\16\TEMPLATE\LAYOUTS\FIC.LD.TrainingEvaluation.SP.Solution\emailtemplates\PLIECompleteEmployeeToLD.html")
            };

            // Send Email
            Email send = new Email();
            send.SendEmail(email, false);
        }

        /// <summary>
        /// Creates new application email body
        /// </summary>
        /// <returns></returns>
        private string CreatePLIECompleteEmployeeToLDEmailBody(string etemplate)
        {
            string EmpFullName = (string)ViewState["FullEmpName"];
            
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   
            using (StreamReader reader = new StreamReader(etemplate))
            {
                body = reader.ReadToEnd();
            }

            //replacing the required strings             
            body = body.Replace("{EmployeeName}", EmpFullName);
            body = body.Replace("{InterventionURL}", $"{Configs.SPSiteURL}/CS/HR/LD/SitePages/PLIEForm.aspx?w={Request.QueryString["w"]}");
            return body;
        }

        /// <summary>
        /// Insert Into POST Intervention Evaluation Table
        /// </summary>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        private void InsertPOSTInterventionevaluation()
        {
            Encryption decrypt = new Encryption();

            bool pageValidationValid = false;
            int? EmployeeID = 0;

            try
            {
                PostLearningInterventionsDataAccess PostLearningDataAccess = new PostLearningInterventionsDataAccess();

                EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();
                EmployeeID = dataaccess.EmployeeID(LoginName);

                if (EmployeeID != null)
                {
                    PostLearningInterventionEvaluationEmployeeObject POSTEmployeeIntervention = new PostLearningInterventionEvaluationEmployeeObject() { PostInterventionID = Convert.ToInt32(decrypt.Decrypt(Request.QueryString["w"])), PostInterventionEmployeeID = (int?)EmployeeID, CourseWorthTimeandExpectations = CourseWorthTimeandExpectationsDropDownList.SelectedItem.ToString(), ApplySkillsLearned = ApplySkillsLearnedDropDownList.SelectedItem.ToString(), QualityOfPresenter = QualityOfPresenterDropDownList.SelectedItem.ToString(), LearningInterventionCoordination = LearningInterventionCoordinationDropDownList.SelectedItem.ToString(), HighlightsOfTraining = txtHighlightsOfTraining.Value, Submitted = 1 };
                    PostLearningDataAccess.InsertPostEmployeeInterventionEvaluation(POSTEmployeeIntervention);

                    pageValidationValid = true;
                }

                if (pageValidationValid == true)
                {
                    SendEmailToLDAdmin();
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
    }
}
