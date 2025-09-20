using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using FIC.LD.TrainingEvaluation.SP.Solution.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace FIC.LD.TrainingEvaluation.SP.Solution.managerboard
{
    public partial class managerboardUserControl : UserControl
    {
        #region Private Properties

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
            ErrorMessageDiv.Visible = false;

            try
            {
                if (!IsPostBack)
                {
                    if (IsUserMemberOfLineManagersSPGroup(SPUserLoginName,Configs.LineManagersSPGroupName))
                    {
                        populateManagerEmployeesInterventions();
                        populateManagerPostLearningActionPlanEvaluations();
                        populateManagerActionPlanEvaluations();
                    }
                    else
                        throw new Exception("Access denied");                  
                }
            }
            catch (Exception ex)
            {
                tabs.Visible = false;
                ErrorMessageDiv.Visible = true;
                ErrorMessageDiv.InnerText = ex.Message;
            }
        }

        #endregion

        #region Proctected Methods

        protected void EmployeeIntervention_Click(object sender, EventArgs e)
        {
            Button ViewIntervention = (Button)sender;
            GridViewRow row = (GridViewRow)ViewIntervention.NamingContainer;

            // Reference ID Hiddenfield value
            HiddenField hiddenID = (HiddenField)row.FindControl("HiddenID");
            int ID = int.Parse(hiddenID.Value);

            if (ID > 0)
            {
                // Reference Encryption Instance
                Encryption encrypt = new Encryption();
                Response.Redirect(string.Format("/CS/HR/LD/SitePages/apply.aspx?w={0}", encrypt.Encrypt(Convert.ToString(ID))));
            }
            else
            {
                // Show
            }

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populates the Manager Employees Interventions
        /// </summary>
        private void populateManagerEmployeesInterventions()
        {
            EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();

            int? ManagerID = dataaccess.EmployeeID(LoginName);

            if (ManagerID != null)
            {
                ManagerEmployeesInterventionsGridView.DataSource = dataaccess.ManagerEmployeesInterventions((int)ManagerID);
                ManagerEmployeesInterventionsGridView.DataBind();
            }
            else
            {
                ManagerEmployeesInterventionsGridView.DataSource = null;
                ManagerEmployeesInterventionsGridView.DataBind();
            }
        }

        /// <summary>
        /// Populate Employee Learning Action Plan Evaluations
        /// </summary>
        private void populateManagerPostLearningActionPlanEvaluations()
        {
            try
            {
                EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();

                int? EmployeeID = null;

                EmployeeID = dataaccess.EmployeeID(LoginName);

                if (EmployeeID != null)
                {
                    ManagerEvaluationDataAccess evaluationdataaccess = new ManagerEvaluationDataAccess();
                    PostLearningActionPlanGridView.DataSource = evaluationdataaccess.ManagerPostLearningActionPlanEvaluationsList(EmployeeID);
                    PostLearningActionPlanGridView.DataBind();
                }
            }
            catch (Exception)
            {
                throw new Exception("Error occured populating the Post Learning Action Plan Grid.");
            }
        }

        /// <summary>
        /// Populate Employee Learning Action Plan Evaluations
        /// </summary>
        private void populateManagerActionPlanEvaluations()
        {
            try
            {
                EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();

                int? EmployeeID = null;

                EmployeeID = dataaccess.EmployeeID(LoginName);

                if (EmployeeID != null)
                {
                    ManagerEvaluationDataAccess evaluationdataaccess = new ManagerEvaluationDataAccess();
                    ActionPlanEvaluationGridView.DataSource = evaluationdataaccess.ManagerActionPlanEvaluationsList(EmployeeID);
                    ActionPlanEvaluationGridView.DataBind();
                }
            }
            catch (Exception)
            {
                throw new Exception("Error occured populating the Action Plan Evaluation Grid.");
            }
        }

        /// <summary>
        /// Checks if the Current User is the Member of SP Line Managers group
        /// </summary>
        /// <returns></returns>
        private bool IsUserMemberOfLineManagersSPGroup(SPUser user, string groupName)
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


        #endregion

        #region Protected Methods

        /// <summary>
        /// Redirects to the Post Learning Action Plan Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PostLearningActionPlan_Click(object sender, EventArgs e)
        {
            Button ViewIntervention = (Button)sender;
            GridViewRow row = (GridViewRow)ViewIntervention.NamingContainer;

            // Reference ID Hiddenfield value
            HiddenField hiddenID = (HiddenField)row.FindControl("HiddenID");
            int ID = int.Parse(hiddenID.Value);

            if (ID > 0)
            {
                // Reference Encryption Instance
                Encryption encrypt = new Encryption();

                Response.Redirect(string.Format("/CS/HR/LD/SitePages/apply.aspx?w={0}", encrypt.Encrypt(Convert.ToString(ID))));
            }
            else
            {
                // Show
            }
        }

        /// <summary>
        /// Redirects to the Action Plan Evaluation Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionPlanEvaluation_Click(object sender, EventArgs e)
        {
            Button ViewActionPlan = (Button)sender;
            GridViewRow row = (GridViewRow)ViewActionPlan.NamingContainer;

            // Reference ID Hiddenfield value
            HiddenField hiddenID = (HiddenField)row.FindControl("HiddenID");
            int ID = int.Parse(hiddenID.Value);

            if (ID > 0)
            {
                // Reference Encryption Instance            s
                Encryption encrypt = new Encryption();
                Response.Redirect(string.Format("/CS/HR/LD/SitePages/ActionPlan.aspx?w={0}", encrypt.Encrypt(Convert.ToString(ID))));
            }
            else
            {
                // Show
            }
        }
        #endregion
    }
}
