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
using System.Data;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Drawing;


namespace FIC.LD.TrainingEvaluation.SP.Solution.admindashboard
{
    public partial class admindashboardUserControl : UserControl
    {
        #region Private Variables

        private const string LearningDevelopmentAdminTeam = "LearningDevelopmentAdminTeam";
        private const string LearningDevelopmentManagersTeam = "LearningDevelopmentManagersTeam";

        #endregion

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
                    if (IsUserMemberOfLDAdminSPGroup(SPUserLoginName, LearningDevelopmentAdminTeam) || IsUserMemberOfLDApproverSPGroup(SPUserLoginName, LearningDevelopmentManagersTeam))
                        PopulateEmployeesForLDAdmin();
                    else
                        throw new UnauthorizedAccessException("Access denied.");
                }
            }
            catch (Exception ex)
            {
                ErrorMessageDiv.Visible = true;
                ErrorMessageDiv.InnerText = ex.Message;
            }
        }

        #endregion

        #region Private Methods  

        /// <summary>
        /// Populates the Grid with the Employees
        /// </summary>
        private void PopulateEmployeesForLDAdmin()
        {
            try
            {
                EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();

                List<EmployeeDetailsObject> EmployeesLists = new List<EmployeeDetailsObject>();
                EmployeesLists = dataaccess.SelectEmployeesForLDAdmin();

                EmployeesGridView.DataSource = EmployeesLists;
                EmployeesGridView.DataBind();

                AddEmpNamesIntoDropDownList(EmployeesLists);
            }
            catch (Exception ex)
            {
                //
            }
        }

        /// <summary>
        /// Filters Grid by EmployeeName
        /// </summary>
        /// <param name="FilterValue"></param>
        private void PopulateEmployeesForLDAdmin(string FilterValue)
        {
            EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();

            List<EmployeeDetailsObject> EmployeesLists = new List<EmployeeDetailsObject>();
            EmployeesLists = dataaccess.SelectEmployeesForLDAdmin().Where(x => x.FullName == FilterValue).ToList();

            // Saves results in ViewState
            ViewState["EMPLOYEELIST"] = EmployeesLists;

            EmployeesGridView.DataSource = EmployeesLists;
            EmployeesGridView.DataBind();
        }

        /// <summary>
        /// Populates the Grid with the Employees
        /// </summary>
        /// <returns></returns>
        private List<EmployeeDetailsObject> SetCurrentGridView()
        {
            EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();

            List<EmployeeDetailsObject> EmployeesLists = new List<EmployeeDetailsObject>();
            EmployeesLists = dataaccess.SelectEmployeesForLDAdmin();

            return EmployeesLists;
        }

        private void AddEmpNamesIntoDropDownList(List<EmployeeDetailsObject> EmployeesLists)
        {
            if (EmployeesLists.Count > 0)
            {
                EmpFullNameDropDownList.Items.Add(new ListItem("View All", "0"));
                foreach (EmployeeDetailsObject item in EmployeesLists)
                    EmpFullNameDropDownList.Items.Add(new ListItem(item.FullName, item.FullName));                
                EmpFullNameDropDownList.DataBind();
            }

            // Remove duplicates
            RemoveDuplicateItems(EmpFullNameDropDownList);

            EmpFullNameDropDownList.SelectedIndex = 0;
        }

        private void BindWhenPaging()
        {
            List<EmployeeDetailsObject> EmployeesLists = new List<EmployeeDetailsObject>();

            if (ViewState["EMPLOYEELIST"] == null)
            {
                EmployeesLists = (List<EmployeeDetailsObject>)ViewState["EMPLOYEELIST"];

                if (EmployeesLists.Count > 0)
                {
                    EmployeesGridView.DataSource = EmployeesLists;
                    EmployeesGridView.DataBind();
                }
            }
        }

        public static void RemoveDuplicateItems(DropDownList EmpFullNameDropDownList)
        {
            for (int i = 0; i < EmpFullNameDropDownList.Items.Count; i++)
            {
                EmpFullNameDropDownList.SelectedIndex = i;
                string str = EmpFullNameDropDownList.SelectedItem.ToString();
                for (int counter = i + 1; counter < EmpFullNameDropDownList.Items.Count; counter++)
                {
                    EmpFullNameDropDownList.SelectedIndex = counter;
                    string compareStr = EmpFullNameDropDownList.SelectedItem.ToString();
                    if (str == compareStr)
                    {
                        EmpFullNameDropDownList.Items.RemoveAt(counter);
                        counter = counter - 1;
                    }
                }
            }
        }
        
        /// <summary>
        /// Bindd Employee Interventions to Grid
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="LearningInterventionFormsGridView"></param>
        private void BindEmployeeInterventions(int? EmployeeID, GridView LearningInterventionFormsGridView)
        {
            try
            {
                EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();

                InterventionsFeedbackDataAccess dataaccessinterventionevaluation = new InterventionsFeedbackDataAccess();
                List<EmployeeInterventionsObject> EvaluationInterventions = dataaccess.EmployeeInterventions(EmployeeID).Where
                                             (s => s.Status != "Learning Need Rejected").ToList();
                LearningInterventionFormsGridView.DataSource = EvaluationInterventions;
                LearningInterventionFormsGridView.DataBind();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Bind Employee Feedback Forms to Grid
        /// </summary>
        /// <param name="InterventionID"></param>
        /// <param name="EmployeeFeedbackInterventionsGridView"></param>
        private void BindEmployeeFeedbackForms(int InterventionID, GridView EmployeeFeedbackInterventionsGridView)
        {
            try
            {
                EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();

                InterventionsFeedbackDataAccess dataaccessinterventionevaluation = new InterventionsFeedbackDataAccess();
                List<InterventionsFeedbackObject> EvaluationInterventions = dataaccessinterventionevaluation.EmployeeFeedBackEvaluations(InterventionID);
                EmployeeFeedbackInterventionsGridView.DataSource = EvaluationInterventions;
                EmployeeFeedbackInterventionsGridView.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Checks if the Current User is the Member of SP LearningDevelopmentTeam group
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
        /// Checks if the Current User is the Member of SP LearningDevelopmentTeam group
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

        #endregion

        #region Protected Methods

        /// <summary>
        /// View Intervention Details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ViewIntervention_Click(object sender, EventArgs e)
        {
            Button ViewIntervention = (Button)sender;
            GridViewRow row = (GridViewRow)ViewIntervention.NamingContainer;

            // Reference ID Hiddenfield value
            HiddenField HiddenInterventionID = (HiddenField)row.FindControl("HiddenInterventionID");
            int ID = int.Parse(HiddenInterventionID.Value);

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

        protected void ViewManApprovalSection_Click(object sender, EventArgs e)
        {
            Button ViewManApprovalSection = (Button)sender;
            GridViewRow row = (GridViewRow)ViewManApprovalSection.NamingContainer;

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

        protected void EmpFullNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear ViewState object
            ViewState["EMPLOYEELIST"] = null;

            if (EmpFullNameDropDownList.SelectedIndex > 0)
                PopulateEmployeesForLDAdmin(EmpFullNameDropDownList.SelectedItem.Text);
            else
                PopulateEmployeesForLDAdmin();
        }

        protected void EmployeesGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            List<EmployeeDetailsObject> EmployeesLists = new List<EmployeeDetailsObject>();

            if (ViewState["EMPLOYEELIST"] != null)
            {
                EmployeesLists = (List<EmployeeDetailsObject>)ViewState["EMPLOYEELIST"];
                EmployeesGridView.PageIndex = e.NewPageIndex;
                EmployeesGridView.DataSource = EmployeesLists;
                EmployeesGridView.DataBind();
            }
            else
            {
                EmployeesGridView.PageIndex = e.NewPageIndex;
                EmployeesGridView.DataSource = SetCurrentGridView();
                EmployeesGridView.DataBind();
            }
        }

        protected void ShowHideEmployeeDashBoardFormsImageButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                ImageButton imgShowHide = (sender as ImageButton);
                GridViewRow row = (imgShowHide.NamingContainer as GridViewRow);
                if (imgShowHide.CommandArgument == "Show")
                {
                    row.FindControl("LearningInterventionFormsPanel").Visible = true;
                    imgShowHide.CommandArgument = "Hide";
                    imgShowHide.ImageUrl = "../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/images/minus.png";
                    int EmployeeID = Convert.ToInt16(EmployeesGridView.DataKeys[row.RowIndex].Value);
                    GridView LearningInterventionFormsGridView = row.FindControl("LearningInterventionFormsGridView") as GridView;
                    BindEmployeeInterventions(EmployeeID, LearningInterventionFormsGridView);
                }
                else
                {
                    row.FindControl("LearningInterventionFormsPanel").Visible = false;
                    imgShowHide.CommandArgument = "Show";
                    imgShowHide.ImageUrl = "../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/images/plus.png";
                }
            }
            catch
            {
            }
        }

        protected void ShowHideEmployeeFeedbackInterventionsImageButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                ImageButton imgShowHide = (sender as ImageButton);
                GridViewRow row = (imgShowHide.NamingContainer as GridViewRow);
                if (imgShowHide.CommandArgument == "Show")
                {
                    row.FindControl("EmployeeFeedbackInterventionsPanel").Visible = true;
                    imgShowHide.CommandArgument = "Hide";
                    imgShowHide.ImageUrl = "../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/images/minus.png";

                    int InterventionID = Convert.ToInt32((row.NamingContainer as GridView).DataKeys[row.RowIndex].Value);

                    GridView EmployeeFeedbackInterventionsGridView = row.FindControl("EmployeeFeedbackInterventionsGridView") as GridView;
                    BindEmployeeFeedbackForms(InterventionID, EmployeeFeedbackInterventionsGridView);
                }
                else
                {
                    row.FindControl("EmployeeFeedbackInterventionsPanel").Visible = false;
                    imgShowHide.CommandArgument = "Show";
                    imgShowHide.ImageUrl = "../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/images/plus.png";
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void EmployeeFeedbackInterventions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TableCell FormTypeCell = e.Row.Cells[0];
                string FormType = FormTypeCell.Text;

                if (FormType == "Post Learning Intervention Evaluation")
                {
                    // Get FormType From the EmployeeFormsGridView
                    TableCell StatusCell = e.Row.Cells[4];
                    string Status = StatusCell.Text;

                    Button btnButton = (Button)e.Row.FindControl("EmployeedFeedbackButton");
                    if (Status == "Completed")
                        btnButton.Text = "View Post Learning Intervention Evaluation";
                    else
                        btnButton.Visible = false;
                }

                if (FormType == "Post Learning Action Plan")
                {
                    // Get FormType From the EmployeeFormsGridView
                    TableCell StatusCell = e.Row.Cells[4];
                    string Status = StatusCell.Text;

                    Button btnButton = (Button)e.Row.FindControl("EmployeedFeedbackButton");
                    if (Status == "Completed")
                        btnButton.Text = "View Post Learning Action Plan";
                    else
                        btnButton.Visible = false;
                }


                if (FormType == "Action Plan Evaluation")
                {
                    // Get FormType From the EmployeeFormsGridView
                    TableCell StatusCell = e.Row.Cells[4];
                    string Status = StatusCell.Text;

                    Button btnButton = (Button)e.Row.FindControl("EmployeedFeedbackButton");
                    if (Status == "Completed")
                        btnButton.Text = "View Post Action Plan Evaluation";
                    else
                        btnButton.Visible = false;
                }
            }
        }

        protected void EmployeedFeedbackButton_Click(object sender, EventArgs e)
        {
            Button ViewIntervention = (Button)sender;
            GridViewRow row = (GridViewRow)ViewIntervention.NamingContainer;

            // Reference ID Hiddenfield value
            HiddenField hiddenID = (HiddenField)row.FindControl("EmployeeDashboardFormHiddenID");
            int ID = int.Parse(hiddenID.Value);

            // Get FormType From the EmployeeFormsGridView
            string FormType = row.Cells[0].Text;

            if (ID > 0)
            {
                // Reference Encryption Instance
                Encryption encrypt = new Encryption();

                if (FormType == "Post Learning Intervention Evaluation")
                {
                    Response.Redirect(string.Format("/CS/HR/LD/SitePages/plie.aspx?w={0}", encrypt.Encrypt(Convert.ToString(ID))));
                }

                if (FormType == "Post Learning Action Plan")
                {
                    Response.Redirect(string.Format("/CS/HR/LD/SitePages/plap.aspx?w={0}", encrypt.Encrypt(Convert.ToString(ID))));
                }

                if (FormType == "Action Plan Evaluation")
                {
                    Response.Redirect(string.Format("/CS/HR/LD/SitePages/actionplan.aspx?w={0}", encrypt.Encrypt(Convert.ToString(ID))));
                }
            }
            else
            {
                // Show
            }
        }

        #endregion

        #region Public Methods

        #endregion
    }
}
