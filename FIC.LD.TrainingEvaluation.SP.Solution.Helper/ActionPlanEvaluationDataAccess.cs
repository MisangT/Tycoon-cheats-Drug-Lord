using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIC.LD.TrainingEvaluation.SP.Solution.Helper
{
    public class ActionPlanEvaluationDataAccess
    {
        /// <summary>
        /// Insert Manager's Action Plan Evaluation Details
        /// </summary>
        /// <param name="POSTInterventionEvaluation"></param>
        public void InsertManagerActionPlanEvaluation(ManagerActionPlanEvaluationObject ManagerAPE)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
                {
                    List<ManagerActionPlanEvaluationObject> ManagerActionPlanEvaluationObject = new List<ManagerActionPlanEvaluationObject>();
                    ManagerActionPlanEvaluationObject.Add(new ManagerActionPlanEvaluationObject { APEInterventionID = ManagerAPE.APEInterventionID, APEEmployeeID = ManagerAPE.APEEmployeeID, SkillsDemonstration = ManagerAPE.SkillsDemonstration, ManagerApplicationOfNewSkills = ManagerAPE.ManagerApplicationOfNewSkills, JobPerformanceChanges = ManagerAPE.JobPerformanceChanges, AdditionalComments = ManagerAPE.AdditionalComments, EmployeePerformanceImprovement = ManagerAPE.EmployeePerformanceImprovement, ManagerSubmitted = ManagerAPE.ManagerSubmitted, EmployeeSubmitted = ManagerAPE.EmployeeSubmitted });
                    connection.Execute("FIC_InsertManagerActionPlanEvaluation @APEInterventionID, @APEEmployeeID, @SkillsDemonstration, @ManagerApplicationOfNewSkills, @JobPerformanceChanges, @AdditionalComments, @EmployeePerformanceImprovement, @ManagerSubmitted, @EmployeeSubmitted", ManagerActionPlanEvaluationObject);
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Insert Employee's Action Plan Evaluation Details
        /// </summary>
        /// <param name="POSTInterventionEvaluation"></param>
        public void InsertEmployeeActionPlanEvaluation(EmployeeActionPlanEvaluationObject EmployeeAPE)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
                {
                    List<EmployeeActionPlanEvaluationObject> EmployeeActionPlanEvaluationObject = new List<EmployeeActionPlanEvaluationObject>();
                    EmployeeActionPlanEvaluationObject.Add(new EmployeeActionPlanEvaluationObject { APEInterventionID = EmployeeAPE.APEInterventionID, APEEmployeeID = EmployeeAPE.APEEmployeeID, LearningInterventionRating = EmployeeAPE.LearningInterventionRating, EmployeeApplicationOfNewSkillsChoice = EmployeeAPE.EmployeeApplicationOfNewSkillsChoice, EmployeeApplicationOfNewSkillsComments = EmployeeAPE.EmployeeApplicationOfNewSkillsComments, RequiredResourcesAndSupportOfferedChoice = EmployeeAPE.RequiredResourcesAndSupportOfferedChoice, RequiredResourcesAndSupportOfferedComments = EmployeeAPE.RequiredResourcesAndSupportOfferedComments, EmployeeNewKnowledge = EmployeeAPE.EmployeeNewKnowledge, EmployeeSubmitted = EmployeeAPE.EmployeeSubmitted });
                    connection.Execute("FIC_UpdateEmployeeActionPlanEvaluation @APEInterventionID, @APEEmployeeID, @LearningInterventionRating, @EmployeeApplicationOfNewSkillsChoice, @EmployeeApplicationOfNewSkillsComments, @RequiredResourcesAndSupportOfferedChoice, @RequiredResourcesAndSupportOfferedComments, @EmployeeNewKnowledge, @Submitted", EmployeeActionPlanEvaluationObject);
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Update a record that was submitted by the Manager on section 1
        /// - Add the employee details on section 2
        /// </summary>
        /// <param name="Intervention"></param>
        /// <returns></returns>
        public int UpdateEmployeeActionPlanEvaluation(EmployeeActionPlanEvaluationObject EmployeeAPE)
        {
            var InterventionID = 0;

            try
            {
                using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
                {
                    InterventionID = connection.Query<int>("FIC_UpdateEmployeeActionPlanEvaluation @APEInterventionID, @APEEmployeeID, @LearningInterventionRating, @EmployeeApplicationOfNewSkillsChoice, @EmployeeApplicationOfNewSkillsComments, @RequiredResourcesAndSupportOfferedChoice, @RequiredResourcesAndSupportOfferedComments, @EmployeeNewKnowledge, @EmployeeSubmitted, @EmployeeDateCreated", new { APEInterventionID = EmployeeAPE.APEInterventionID, APEEmployeeID = EmployeeAPE.APEEmployeeID, LearningInterventionRating = EmployeeAPE.LearningInterventionRating, EmployeeApplicationOfNewSkillsChoice = EmployeeAPE.EmployeeApplicationOfNewSkillsChoice, EmployeeApplicationOfNewSkillsComments = EmployeeAPE.EmployeeApplicationOfNewSkillsComments, RequiredResourcesAndSupportOfferedChoice = EmployeeAPE.RequiredResourcesAndSupportOfferedChoice, RequiredResourcesAndSupportOfferedComments = EmployeeAPE.RequiredResourcesAndSupportOfferedComments, EmployeeNewKnowledge = EmployeeAPE.EmployeeNewKnowledge, EmployeeSubmitted = EmployeeAPE.EmployeeSubmitted, EmployeeDateCreated = EmployeeAPE.EmployeeDateCreated }).Single();
                }
            }
            catch (Exception ex)
            {

            }

            return InterventionID;
        }

        /// <summary>
        /// Gets Action Plan Evaluation Section 1 - Manager by Intervention ID
        /// </summary>
        /// <param name="InterventionID"></param>
        /// <returns></returns>        
        public ManagerActionPlanEvaluationObject GetManagerActionPlanEvaluationSection(int InterventionID)
        {
            using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
            {
                var output = connection.Query<ManagerActionPlanEvaluationObject>("FIC_SELECTManagerActionPlanEvaluation @InterventionID", new { InterventionID = InterventionID }).SingleOrDefault();
                return output;
            }
        }

        /// <summary>
        /// Gets Action Plan Evaluation Section 2 - Employee by Intervention ID
        /// </summary>
        /// <param name="InterventionID"></param>
        /// <returns></returns>        
        public EmployeeActionPlanEvaluationObject GetEmployeeActionPlanEvaluationSection(int InterventionID)
        {
            using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
            {
                var output = connection.Query<EmployeeActionPlanEvaluationObject>("FIC_SELECTEmployeeActionPlanEvaluation @InterventionID", new { InterventionID = InterventionID }).SingleOrDefault();
                return output;
            }
        }

        /// <summary>
        /// Gets the Employee Action Plan Evaluations
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public List<EmployeeActionPlanEvaluation> EmployeeActionPlanEvaluationsList(int? EmployeeID)
        {
            using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
            {
                var output = connection.Query<EmployeeActionPlanEvaluation>("FIC_SelectEmployeeActionPlanEvaluations @EmployeeID", new { EmployeeID = EmployeeID }).ToList();
                return output;
            }
        }

        /// <summary>
        /// Gets the Employee Action Plan Evaluations
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public List<EmployeeActionPlanEvaluation> EmployeeActionPlanEvaluationsList()
        {
            using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
            {
                var output = connection.Query<EmployeeActionPlanEvaluation>("FIC_SelectAdminEmployeeActionPlanEvaluations").ToList();
                return output;
            }
        }
        
        /// <summary>
        /// Checks if SECTION 1 - Manager Section part of the form is Completed
        /// </summary>
        /// <param name="InterventionID"></param>
        /// <returns></returns>
        public int? IsAPESectionOneCompleted(ManagerActionPlanEvaluationObject Intervention)
        {
            int? IsSubmitted = null;

            try
            {
                using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
                {
                    IsSubmitted = connection.Query<int?>("FIC_ActionPlanEvaluationCompleted @InterventionID", new { InterventionID = Intervention.APEInterventionID }).Single();
                }
            }
            catch (Exception ex)
            {
            }
            return IsSubmitted;
        }

    }
}
