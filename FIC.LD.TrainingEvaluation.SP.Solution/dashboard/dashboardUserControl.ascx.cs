using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using FIC.LD.TrainingEvaluation.SP.Solution.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace FIC.LD.TrainingEvaluation.SP.Solution.dashboard
{
    public partial class dashboardUserControl : UserControl
    {
        #region Private Properties

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
            ErrorMessageDiv.Visible = false;

            try
            {
                if (!IsPostBack)
                {
                    populateEmployeeIntervention();                  
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
        /// Populate Employee Interventions
        /// </summary>
        private void populateEmployeeIntervention()
        {
            try
            {
                EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();

                int? EmployeeID = null;

                EmployeeID = dataaccess.EmployeeID(LoginName);

                if (EmployeeID != null)
                {
                    List<EmployeeInterventionsObject> interventions = dataaccess.EmployeeInterventions(EmployeeID);
                    EmployeeInterventionsGridView.DataSource = interventions;
                    EmployeeInterventionsGridView.DataBind();
                }
                else
                {
                    EmployeeInterventionsGridView.DataSource = null;
                    EmployeeInterventionsGridView.DataBind();
                }
            }
            catch (Exception)
            {
                throw new Exception("Error occured populating Employee Interventions Grid.");
            }
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
        /// Redirects to the Post Learning Intervention Evaluation Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PostLearningInterventionEvaluationButton_Click(object sender, EventArgs e)
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
                // Reference Encryption Instances
                Encryption encrypt = new Encryption();
                Response.Redirect(string.Format("/CS/HR/LD/SitePages/actionplan.aspx?w={0}", encrypt.Encrypt(Convert.ToString(ID))));
            }
            else
            {
                // Show
            }
        }

        /// <summary>LearningInterventionForms_PageIndexChanging
        /// Show / Hide Feedback Form and Evaluation Forms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ShowHideEmployeeDashBoardFormsImageButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                ImageButton imgShowHide = (sender as ImageButton);
                GridViewRow row = (imgShowHide.NamingContainer as GridViewRow);
                if (imgShowHide.CommandArgument == "Show")
                {
                    row.FindControl("EmployeeFormsPanel").Visible = true;
                    imgShowHide.CommandArgument = "Hide";
                    imgShowHide.ImageUrl = "../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/images/minus.png";
                    string InterventionID = EmployeeInterventionsGridView.DataKeys[row.RowIndex].Value.ToString();
                    GridView EmployeeFormsGridView = row.FindControl("EmployeeFormsGridView") as GridView;
                    BindEmployeeForms(InterventionID, EmployeeFormsGridView);
                }
                else
                {
                    row.FindControl("EmployeeFormsPanel").Visible = false;
                    imgShowHide.CommandArgument = "Show";
                    imgShowHide.ImageUrl = "../_layouts/FIC.LD.TrainingEvaluation.SP.Solution/images/plus.png";
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Bind Feedback forms and Evaluation Form to the GridView
        /// </summary>
        /// <param name="InterventionID"></param>
        /// <param name="EmployeeFormsGridView"></param>
        private void BindEmployeeForms(string InterventionID, GridView EmployeeFormsGridView)
        {
            try
            {
                EmployeeInterventionsDataAccess dataaccess = new EmployeeInterventionsDataAccess();

                int? EmployeeID = null;

                EmployeeID = dataaccess.EmployeeID(LoginName);

                if (EmployeeID != null)
                {
                    InterventionsFeedbackDataAccess dataaccessinterventionevaluation = new InterventionsFeedbackDataAccess();
                    List<InterventionsFeedbackObject> EvaluationInterventions = dataaccessinterventionevaluation.EmployeeFeedBackEvaluations(EmployeeID, Convert.ToInt32(InterventionID));
                    EmployeeFormsGridView.DataSource = EvaluationInterventions;
                    EmployeeFormsGridView.DataBind();
                }
            }
            catch
            {
            }
        }

        protected void InterventionsForm_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridView InterventionsFormGridView = (sender as GridView);
                InterventionsFormGridView.PageIndex = e.NewPageIndex;
                //BindEntities(EmployeeFormsGridView.ToolTip, EmployeeFormsGridView);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Change buttons and status color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EmployeeFormsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
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

                    Button btnButton = (Button)e.Row.FindControl("PostLearningInterventionEvaluationButton");
                    if (Status == "Completed")
                        btnButton.Text = "View Post Learning Intervention Evaluation";                        
                    else
                        btnButton.Text = "Complete Post Learning Intervention Evaluation";
                }

                if (FormType == "Post Learning Action Plan")
                {
                    // Get FormType From the EmployeeFormsGridView
                    TableCell StatusCell = e.Row.Cells[4];
                    string Status = StatusCell.Text;

                    Button btnButton = (Button)e.Row.FindControl("PostLearningInterventionEvaluationButton");
                    if (Status == "Completed")
                        btnButton.Text = "View Post Learning Action Plan";                        
                    else

                        btnButton.Text = "Complete Post Learning Action Plan";
                }


                if (FormType == "Action Plan Evaluation")
                {
                    // Get FormType From the EmployeeFormsGridView
                    TableCell StatusCell = e.Row.Cells[4];
                    string Status = StatusCell.Text;

                    Button btnButton = (Button)e.Row.FindControl("PostLearningInterventionEvaluationButton");
                    if (Status == "Completed")
                        btnButton.Text = "View Post Action Plan Evaluation";
                    else
                        btnButton.Text = "Complete Post Action Plan Evaluation";

                }
            }
        }

        #endregion
    }
}
