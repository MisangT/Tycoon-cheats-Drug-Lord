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

namespace FIC.LD.TrainingEvaluation.SP.Solution.plap
{
    public partial class plapUserControl : UserControl
    {
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

        #endregion

        #region Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (Request.QueryString["w"] != null)
                    {
                        Encryption decrypt = new Encryption();
                        int InterventionID = Convert.ToInt32(decrypt.Decrypt(Request.QueryString["w"]));
                        //Checks if the Post Employee Intervention Evaluation Form is Completed
                        if (PostEmployeeInterventionEvaluationCompleted(InterventionID))
                        {
                            ViewState["InterventionID"] = InterventionID;
                            PopulateEmployeeDetails(InterventionID);
                            PopulateEmployeeActionPlan(InterventionID);
                        }
                        else
                            throw new Exception("Please complete the Post Employee Intervention Evaluation before working on the current form.");
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

        #endregion

        #region Protected Methods        

        /// <summary>
        /// Saves the Employee Post Learning Action Plan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EmployeeSubmit_Click(object sender, EventArgs e)
        {
            // Error flag to check if the Submit had no errors
            bool isSuccess = false;

            // Hide error div
            ErrorMessageDiv.Visible = false;
            try
            {
                SaveEmployeePostLearningActionPlan();
                SendEmailToManager();
                isSuccess = true;
            }
            catch (Exception ex)
            {
                ErrorMessageDiv.Visible = true;
                ErrorMessageDiv.InnerText = ex.Message;
            }
            finally
            {
                if (isSuccess)
                    Response.Redirect("/CS/HR/LD/SitePages/dashboard.aspx");
            }
        }

        protected void LineManagerApproveButton_Click(object sender, EventArgs e)
        {
            bool isSuccess = false;
            try
            {
                ErrorMessageDiv.Visible = false;

                if (LoginName.ToLower() == ManagerNameTextBox.Text.Trim().ToLower())
                {
                    // Save Employee Updates
                    SaveEmployeePostLearningActionPlan();

                    PostLearningActionPlanDataAccess dataaccess = new PostLearningActionPlanDataAccess();

                    // Insert Employee Post Learning Action Plan
                    EmployeePostLearningActionPlanObject actionplan = new EmployeePostLearningActionPlanObject()
                    {
                        InterventionID = Convert.ToInt32(ViewState["InterventionID"]),
                        SubmittedByManager = 1,
                        Approved = ManagerApproveOrRejectDropDownList.SelectedIndex == 1 ? 1 : 0
                    };

                    // Updates Line Manager Inputs
                    dataaccess.UpdateEmployeePostLearningActionPlans(actionplan);

                    if (ManagerApproveOrRejectDropDownList.SelectedIndex == 1)
                        SendApprovedEmail();
                    else
                        SendRejectEmail();

                    isSuccess = true;

                }
                else
                    throw new Exception("Access denied");
            }
            catch (Exception ex)
            {
                ErrorMessageDiv.Visible = true;
                ErrorMessageDiv.InnerText = ex.Message;
            }
            finally
            {
                if (isSuccess)
                    Response.Redirect("/CS/HR/LD/SitePages/managerdashboard.aspx");
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Checks if the Post Employee Intervention Evaluation Form is Completed
        /// </summary>
        /// <returns></returns>
        private bool PostEmployeeInterventionEvaluationCompleted(int InterventionID)
        {
            int? IsSubmitted = null;

            PostEmployeeInterventionEvaluationCompletedObject completed = new PostEmployeeInterventionEvaluationCompletedObject();
            completed.InterventionID = InterventionID;

            PostLearningInterventionsDataAccess dataaccess = new PostLearningInterventionsDataAccess();
            IsSubmitted = dataaccess.IsPostEmployeeInterventionEvaluationCompleted(completed);

            return IsSubmitted == 1 ? true : false;
        }

        /// <summary>
        /// Populates the Employee Details
        /// </summary>
        /// <param name="InterventionID"></param>
        private void PopulateEmployeeDetails(int InterventionID)
        {
            try
            {
                // Local Variables
                int? EmployeeID = null;

                EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();
                EmployeeID = dataaccess.EmployeeID(LoginName);

                if (EmployeeID != null)
                {
                    EmployeeDetailsObject EmployeeDetails = dataaccess.EmployeeDetails(InterventionID);

                    if (EmployeeDetails != null || EmployeeDetails.EmployeeID == EmployeeID)
                    {
                        FullNameTextBox.Text = EmployeeDetails.FullName;
                        DesignationTextBox.Text = EmployeeDetails.Designation;
                        DepartmentTextBox.Text = EmployeeDetails.Department;
                        NameofLearningInterventionTextBox.Text = EmployeeDetails.NameOfLearning;
                        DateofAttendanceTextBox.Text = $"{EmployeeDetails.StartDateOfTraining} To {EmployeeDetails.EndDateOfTraining}";
                        // Set the Last Date of Training to a Hidden Field
                        //LastDateOfTrainingHidden.Value = EmployeeDetails.EndDateOfTraining;
                        ManagerNameTextBox.Text = EmployeeDetails.ManagerName;
                        ViewState["EMPLOYEEUSERNAME"] = EmployeeDetails.Username;
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
            catch (Exception ex)
            {
                MainContent.Visible = false;
                ErrorMessageDiv.Visible = true;
                ErrorMessageDiv.InnerText = ex.Message;
            }
        }

        /// <summary>
        /// Populate the Employee Action plan on the forms
        /// </summary>
        /// <param name="InterventionID"></param>
        private void PopulateEmployeeActionPlan(int InterventionID)
        {
            try
            {
                PostLearningActionPlanDataAccess dataaccess = new PostLearningActionPlanDataAccess();
                EmployeePostLearningActionPlanObject actionplan = dataaccess.EmployeePostLearningActionPlans(InterventionID);

                if (actionplan != null)
                {
                    InitialObjectiveTextBox1.Value = actionplan.LearningObjectives1;
                    ActionPlanTextBox1.Value = actionplan.ActionPlan1;
                    HowToImplementTextBox1.Value = actionplan.HowToImplement1;
                    WhenItShouldBeDoneTextBox1.Value = actionplan.WhenToBeDone1;

                    InitialObjectiveTextBox2.Value = actionplan.LearningObjectives2;
                    ActionPlanTextBox2.Value = actionplan.ActionPlan2;
                    HowToImplementTextBox2.Value = actionplan.HowToImplement2;
                    WhenItShouldBeDoneTextBox2.Value = actionplan.WhenToBeDone2;

                    InitialObjectiveTextBox3.Value = actionplan.LearningObjectives3;
                    ActionPlanTextBox3.Value = actionplan.ActionPlan3;
                    HowToImplementTextBox3.Value = actionplan.HowToImplement3;
                    WhenItShouldBeDoneTextBox3.Value = actionplan.WhenToBeDone3;

                    InitialObjectiveTextBox4.Value = actionplan.LearningObjectives4;
                    ActionPlanTextBox4.Value = actionplan.ActionPlan4;
                    HowToImplementTextBox4.Value = actionplan.HowToImplement4;
                    WhenItShouldBeDoneTextBox4.Value = actionplan.WhenToBeDone4;

                    ViewState["ACTIONPLANID"] = actionplan.ID;

                    LearnerMotivationTextBox.Text = actionplan.LearnerMotivation;

                    if(actionplan.Approved == 0 && LoginName.ToLower() == Convert.ToString(ViewState["EMPLOYEEUSERNAME"]).ToLower())
                        EmployeeSubmit.Visible = true;
                    else
                        EmployeeSubmit.Visible = false;

                    if (ManagerNameTextBox.Text.Trim().ToLower().Equals(LoginName.ToLower()))
                    {
                        ApproveRejectFieldSet.Visible = true;
                        LineManagerApproveButton.Visible = true;
                    }

                    if (actionplan.Approved == 1 && actionplan.SubmittedByManager == 1)
                    {
                        ApproveRejectFieldSet.Visible = true;
                        ManagerApproveOrRejectDropDownList.SelectedIndex = actionplan.Approved == 1 ? 1 : 2;
                        LineManagerApproveButton.Visible = false;
                        EmployeeSubmit.Visible = false;
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Employee Save Post Learning Action Plan
        /// </summary>
        private void SaveEmployeePostLearningActionPlan()
        {
            try
            {
                PostLearningActionPlanDataAccess dataaccess = new PostLearningActionPlanDataAccess();

                // Insert Employee Post Learning Action Plan
                EmployeePostLearningActionPlanObject actionplan = new EmployeePostLearningActionPlanObject()
                {
                    InterventionID = Convert.ToInt32(ViewState["InterventionID"]),

                    LearningObjectives1 = InitialObjectiveTextBox1.Value,
                    ActionPlan1 = ActionPlanTextBox1.Value,
                    HowToImplement1 = HowToImplementTextBox1.Value,
                    WhenToBeDone1 = WhenItShouldBeDoneTextBox1.Value,

                    LearningObjectives2 = InitialObjectiveTextBox2.Value,
                    ActionPlan2 = ActionPlanTextBox2.Value,
                    HowToImplement2 = HowToImplementTextBox2.Value,
                    WhenToBeDone2 = WhenItShouldBeDoneTextBox2.Value,

                    LearningObjectives3 = InitialObjectiveTextBox3.Value,
                    ActionPlan3 = ActionPlanTextBox3.Value,
                    HowToImplement3 = HowToImplementTextBox3.Value,
                    WhenToBeDone3 = WhenItShouldBeDoneTextBox3.Value,

                    LearningObjectives4 = InitialObjectiveTextBox4.Value,
                    ActionPlan4 = ActionPlanTextBox4.Value,
                    HowToImplement4 = HowToImplementTextBox4.Value,
                    WhenToBeDone4 = WhenItShouldBeDoneTextBox4.Value,

                    LearnerMotivation = LearnerMotivationTextBox.Text.Trim(),
                    SubmittedByEmployee = 1
                };

                // Insert Employee Post Learning Action Plan
                dataaccess.InsertEmployeePostLearningActionPlan(actionplan);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Gets the Plan ID 
        /// </summary>
        /// <returns></returns>
        private int? ActionPlanID()
        {
            int? ActionPlanID = null;

            if (Request.QueryString["w"] != null)
                ActionPlanID = (int?)Convert.ToInt32(Request.QueryString["w"]);

            if (ActionPlanID != null)
                return (int?)Convert.ToInt32(ViewState["ACTIONPLANID"]);
            else
                return null;
        }

        /// <summary>
        /// Gets the Plan ID 
        /// </summary>
        /// <returns></returns>
        private int? InterventionID()
        {
            Encryption decrypt = new Encryption();
            int? IntervetionID = null;

            if (Request.QueryString["w"] != null)
                IntervetionID = (int?)Convert.ToInt32(Request.QueryString["w"]);

            if (IntervetionID != null)
                return IntervetionID;
            else
                return null;
        }

        /// <summary>
        /// Send an Email to Manager for Approval or Rejection
        /// </summary>
        private void SendEmailToManager()
        {
            // Compse and send Email to LD Team member
            EmailObject email = new EmailObject()
            {
                From = Configs.FromEmail,
                To = $"{ManagerNameTextBox.Text}@fic.gov.za",
                Subject = "Post learning Action Plan",
                Body = CreatePostLearnigActionPlanToManagerEmailBody(@"C:\Program Files\Common Files\microsoft shared\Web Server Extensions\16\TEMPLATE\LAYOUTS\FIC.LD.TrainingEvaluation.SP.Solution\emailtemplates\EmployeeToManagerplap.html")
            };

            // Send Email
            Email send = new Email();
            send.SendEmail(email, false);
        }

        /// <summary>
        /// Creates new application email body
        /// </summary>
        /// <returns></returns>
        private string CreatePostLearnigActionPlanToManagerEmailBody(string etemplate)
        {
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   
            using (StreamReader reader = new StreamReader(etemplate))
            {
                body = reader.ReadToEnd();
            }
            //replacing the required strings  
            body = body.Replace("{ManagerName}", ManagerNameTextBox.Text);
            body = body.Replace("{EmployeeName}", FullNameTextBox.Text);
            body = body.Replace("{InterventionURL}", $"{Configs.SPSiteURL}/CS/HR/LD/SitePages/plap.aspx?w={Request.QueryString["w"]}");
            return body;
        }

        /// <summary>
        /// Compose and Send Approved email to LD Team
        /// </summary>
        private void SendApprovedEmail()
        {
            // Compse and send Email to LD Team member
            EmailObject email = new EmailObject()
            {
                From = Configs.FromEmail,
                To = $"{Convert.ToString(ViewState["EMPLOYEEUSERNAME"])}@fic.gov.za",
                CC = SPGroupUserList(Configs.LDAdminSPGroupName),
                Subject = "Post learning Action Plan Approved",
                Body = CreateApprovedEmailBody(@"C:\Program Files\Common Files\microsoft shared\Web Server Extensions\16\TEMPLATE\LAYOUTS\FIC.LD.TrainingEvaluation.SP.Solution\emailtemplates\ManagerApprovedplap.html")
            };

            // Send Email
            Email send = new Email();
            send.SendEmail(email, true);
        }

        /// <summary>
        /// Creates new application email body
        /// </summary>
        /// <returns></returns>
        private string CreateApprovedEmailBody(string etemplate)
        {
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   
            using (StreamReader reader = new StreamReader(etemplate))
            {
                body = reader.ReadToEnd();
            }
            //replacing the required strings  
            body = body.Replace("{ManagerName}", ManagerNameTextBox.Text);
            body = body.Replace("{EmployeeName}", FullNameTextBox.Text);
            body = body.Replace("{InterventionURL}", $"{Configs.SPSiteURL}/CS/HR/LD/SitePages/plap.aspx?w={Request.QueryString["w"]}");
            return body;
        }

        /// <summary>
        /// Compose and Send Reject email to the Employee
        /// </summary>
        private void SendRejectEmail()
        {
            // Compse and send Email to LD Team member
            EmailObject email = new EmailObject()
            {
                From = Configs.FromEmail,
                To = $"{FullNameTextBox.Text.Trim()}@fic.gov.za",
                Subject = "Post learning Action Plan Rejected",
                Body = CreateRejectedEmailBody(@"C:\Program Files\Common Files\microsoft shared\Web Server Extensions\16\TEMPLATE\LAYOUTS\FIC.LD.TrainingEvaluation.SP.Solution\emailtemplates\ManagerRejectedplap.html")
            };

            // Send Email
            Email send = new Email();
            send.SendEmail(email, false);
        }

        /// <summary>
        /// Create Rejected Email Body
        /// </summary>
        /// <returns></returns>
        private string CreateRejectedEmailBody(string etemplate)
        {
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   
            using (StreamReader reader = new StreamReader(etemplate))
            {
                body = reader.ReadToEnd();
            }
            //replacing the required strings  
            body = body.Replace("{ManagerName}", ManagerNameTextBox.Text);
            body = body.Replace("{EmployeeName}", FullNameTextBox.Text);
            body = body.Replace("{InterventionURL}", $"{Configs.SPSiteURL}/CS/HR/LD/SitePages/plap.aspx?w={Request.QueryString["w"]}");
            return body;
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
    }
}
