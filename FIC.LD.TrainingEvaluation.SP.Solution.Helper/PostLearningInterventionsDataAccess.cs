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
    public class PostLearningInterventionsDataAccess
    {
        /// <summary>
        /// Insert Employee's Post Learning Intervention Evaluation
        /// </summary>
        /// <param name="POSTInterventionEvaluation"></param>
        public void InsertPostEmployeeInterventionEvaluation(PostLearningInterventionEvaluationEmployeeObject POSTInterventionEvaluation)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
                {
                    List<PostLearningInterventionEvaluationEmployeeObject> EmployeePOSTInterventionEvaluation = new List<PostLearningInterventionEvaluationEmployeeObject>();
                    EmployeePOSTInterventionEvaluation.Add(new PostLearningInterventionEvaluationEmployeeObject { PostInterventionID = POSTInterventionEvaluation.PostInterventionID, PostInterventionEmployeeID = POSTInterventionEvaluation.PostInterventionEmployeeID, CourseWorthTimeandExpectations = POSTInterventionEvaluation.CourseWorthTimeandExpectations, ApplySkillsLearned = POSTInterventionEvaluation.ApplySkillsLearned, QualityOfPresenter = POSTInterventionEvaluation.QualityOfPresenter, LearningInterventionCoordination = POSTInterventionEvaluation.LearningInterventionCoordination, HighlightsOfTraining = POSTInterventionEvaluation.HighlightsOfTraining, Submitted = POSTInterventionEvaluation.Submitted });
                    connection.Execute("FIC_InsertPostEmployeeInterventionEvaluation @PostInterventionID, @PostInterventionEmployeeID, @CourseWorthTimeandExpectations, @ApplySkillsLearned, @QualityOfPresenter, @LearningInterventionCoordination, @HighlightsOfTraining, @Submitted", EmployeePOSTInterventionEvaluation);
                }
            }
            catch (Exception ex)
            {

            }
        }
       
        /// <summary>
        /// Gets Post Learning Intervention Evaluation Survey by Intervention ID
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>        
        public PostLearningInterventionEvaluationEmployeeObject GetPOSTInterventionSurvey(int InterventionID)
        {
            using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
            {
                var output = connection.Query<PostLearningInterventionEvaluationEmployeeObject>("FIC_SELECTPOSTInterventionEvaluation @InterventionID", new { InterventionID = InterventionID }).SingleOrDefault();
                return output;
            }
        }

        /// <summary>
        /// Checks if the Post Employee Intervention Evaluation Completed
        /// </summary>
        /// <param name="InterventionID"></param>
        /// <returns></returns>
        public int? IsPostEmployeeInterventionEvaluationCompleted(PostEmployeeInterventionEvaluationCompletedObject InterventionID)
        {
            int? IsSubmitted = null;

            try
            {
                using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
                {
                    IsSubmitted = connection.Query<int?>("FIC_PostEmployeeInterventionEvaluationCompleted @InterventionID", new { InterventionID = InterventionID.InterventionID }).Single();
                }
            }
            catch (Exception ex)
            {
            }
            return IsSubmitted;
        }

        /// <summary>
        /// Gets the Employee Post Intervention Evaluations by EmployeeID
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public List<EmployeePostInterventionEvaluation> EmployeePostInterventionEvaluationsList(int? EmployeeID)
        {
            using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
            {
                var output = connection.Query<EmployeePostInterventionEvaluation>("FIC_SelectEmployeePostInterventionEvaluations @EmployeeID", new { EmployeeID = EmployeeID }).ToList();
                return output;
            }
        }              

        /// <summary>
        /// Gets the Employee Post Intervention Evaluations
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public List<EmployeePostInterventionEvaluation> EmployeePostInterventionEvaluationsList()
        {
            using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
            {
                var output = connection.Query<EmployeePostInterventionEvaluation>("FIC_SelectAdminEmployeePostInterventionEvaluations").ToList();
                return output;
            }
        }
    }
}
