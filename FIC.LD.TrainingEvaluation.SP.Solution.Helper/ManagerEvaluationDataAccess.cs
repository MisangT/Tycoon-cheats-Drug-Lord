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
    public class ManagerEvaluationDataAccess
    {
        /// <summary>
        /// Gets the Manager Post Learning Action Plan Evaluations
        /// </summary>
        /// <param name="ManagerID"></param>
        /// <returns></returns>
        public List<ManagerPostLearningActionPlanEvaluationObject> ManagerPostLearningActionPlanEvaluationsList(int? ManagerID)
        {
            using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
            {
                var output = connection.Query<ManagerPostLearningActionPlanEvaluationObject>("FIC_SelectManagerPostLearningActionPlanEvaluations @ManagerID", new { ManagerID = ManagerID }).ToList();
                return output;
            }
        }

        /// <summary>
        /// Gets the Manager Action Plan Evaluations
        /// </summary>
        /// <param name="ManagerID"></param>
        /// <returns></returns>
        public List<ManagerPostLearningActionPlanEvaluationObject> ManagerActionPlanEvaluationsList(int? ManagerID)
        {
            using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
            {
                var output = connection.Query<ManagerPostLearningActionPlanEvaluationObject>("FIC_SelectManagerActionPlanEvaluations @ManagerID", new { ManagerID = ManagerID }).ToList();
                return output;
            }
        }
    }
}
