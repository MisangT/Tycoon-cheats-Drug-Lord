using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace FIC.LD.TrainingEvaluation.SP.Solution.Helper
{
    public class PostLearningActionPlanDataAccess
    {
        /// <summary>
        /// Insert Employee PostLearning Action Plan and return ID
        /// </summary>
        /// <param name="Intervention"></param>
        public int InsertEmployeePostLearningActionPlan(EmployeePostLearningActionPlanObject ActionPlan)
        {
            var PostLearningActionPlanID = 0;

            try
            {
                using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
                {
                    PostLearningActionPlanID = connection.Query<int>("FIC_InsertEmployeePostLearningActionPlan @InterventionID, @LearningObjectives1, @ActionPlan1, @HowToImplement1, @WhenToBeDone1, @LearningObjectives2, @ActionPlan2, @HowToImplement2, @WhenToBeDone2,@LearningObjectives3, @ActionPlan3, @HowToImplement3, @WhenToBeDone3,@LearningObjectives4, @ActionPlan4, @HowToImplement4, @WhenToBeDone4, @LearnerMotivation, @SubmittedByEmployee", 
                                                                             new { InterventionID = ActionPlan.InterventionID,ActionPlan.LearningObjectives1,ActionPlan.ActionPlan1,ActionPlan.HowToImplement1,ActionPlan.WhenToBeDone1,ActionPlan.LearningObjectives2,ActionPlan.ActionPlan2,ActionPlan.HowToImplement2,ActionPlan.WhenToBeDone2,ActionPlan.LearningObjectives3,ActionPlan.ActionPlan3,ActionPlan.HowToImplement3,ActionPlan.WhenToBeDone3,ActionPlan.LearningObjectives4,ActionPlan.ActionPlan4,ActionPlan.HowToImplement4,ActionPlan.WhenToBeDone4, LearnerMotivation = ActionPlan.LearnerMotivation , SubmittedByEmployee = ActionPlan.SubmittedByEmployee }).Single();
                }
            }
            catch
            {
            }

            return PostLearningActionPlanID;
        }

        /// <summary>
        /// Insert Employee Post Learning Action Plans
        /// </summary>
        /// <param name="Intervention"></param>
        public void InsertEmployeePostLearningActionPlans(EmployeeActionPlansObject Plan)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
                {
                    List<EmployeeActionPlansObject> EmployeePlan = new List<EmployeeActionPlansObject>();
                    EmployeePlan.Add(new EmployeeActionPlansObject { ID = Plan.ID, EmployeePostLearningActionPlanID = Plan.EmployeePostLearningActionPlanID, LearningObjectives = Plan.LearningObjectives, ActionPlan = Plan.ActionPlan, HowToImplement = Plan.HowToImplement, WhenToBeDone = Plan.WhenToBeDone });
                    connection.Execute("FIC_InsertEmployeePostLearningActionPlans @ID,@EmployeePostLearningActionPlanID, @LearningObjectives,@ActionPlan,@HowToImplement,@WhenToBeDone", EmployeePlan);
                }
            }
            catch
            {
            }           
        }

        /// <summary>
        /// Update Employee Post Learning Action Plan
        /// </summary>
        /// <param name="Intervention"></param>
        public void UpdateEmployeePostLearningActionPlans(EmployeePostLearningActionPlanObject Plan)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
                {
                    List<EmployeePostLearningActionPlanObject> EmployeePlan = new List<EmployeePostLearningActionPlanObject>();
                    EmployeePlan.Add(new EmployeePostLearningActionPlanObject { InterventionID = Plan.InterventionID, SubmittedByManager = Plan.SubmittedByManager, Approved = Plan.Approved  });
                    connection.Execute("FIC_UpdateEmployeePostLearningActionPlan @InterventionID, @SubmittedByManager, @Approved", EmployeePlan);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Post Learning Action Plan by Intervention
        /// </summary>
        /// <param name="InterventionID"></param>
        /// <returns></returns>
        public EmployeePostLearningActionPlanObject EmployeePostLearningActionPlans(int InterventionID)
        {
            using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
            {                                   
                var output = connection.Query<EmployeePostLearningActionPlanObject>("FIC_SELECTEmployeePostLearningActionPlans @InterventionID", new { InterventionID = InterventionID }).Single();                
                return output;
            }
        }

        /// <summary>
        /// Checks if the Post Learning Action Plan Completed
        /// </summary>
        /// <param name="InterventionID"></param>
        /// <returns></returns>
        public int? IsPostLearningActionPlanCompleted(PostLearningActionPlanCompletedObject Intervention)
        {
            int? IsSubmitted = null;

            try
            {
                using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
                {
                    IsSubmitted = connection.Query<int?>("FIC_PostLearningActionPlanCompleted @InterventionID", new { InterventionID = Intervention.InterventionID }).Single();
                }
            }
            catch (Exception ex)
            {
            }
            return IsSubmitted;
        }

        /// <summary>
        /// Gets the Employee Post Learning Action Plan Evaluations by EmployeeID
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public List<EmployeePostLearningActionPlanEvaluation> EmployeePostLearningActionPlanEvaluationsList(int? EmployeeID)
        {
            using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
            {
                var output = connection.Query<EmployeePostLearningActionPlanEvaluation>("FIC_SelectEmployeePostLearningActionPlanEvaluations @EmployeeID", new { EmployeeID = EmployeeID }).ToList();
                return output;
            }
        }

        /// <summary>
        /// Gets the Employee Post Learning Action Plan Evaluations
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public List<EmployeePostLearningActionPlanEvaluation> EmployeePostLearningActionPlanEvaluationsList()
        {
            using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
            {
                var output = connection.Query<EmployeePostLearningActionPlanEvaluation>("FIC_SelectAdminEmployeePostLearningActionPlanEvaluations").ToList();
                return output;
            }
        }
    }
}
