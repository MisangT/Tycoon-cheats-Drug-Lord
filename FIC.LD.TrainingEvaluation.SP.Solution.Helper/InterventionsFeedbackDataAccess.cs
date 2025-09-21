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
    public class InterventionsFeedbackDataAccess
    {
        public List<InterventionsFeedbackObject> EmployeeFeedBackEvaluations(int? EmployeeID, int InterventionID)
        {
            using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
            {
                var output = connection.Query<InterventionsFeedbackObject>("FIC_SelectEmployeeFeedBackEvaluations @EmployeeID, @InterventionID", new { EmployeeID = EmployeeID, InterventionID = InterventionID }).ToList();
                return output;
            }
        }

        public List<InterventionsFeedbackObject> EmployeeFeedBackEvaluations(int InterventionID)
        {
            using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
            {
                var output = connection.Query<InterventionsFeedbackObject>("FIC_SelectAdminEmployeeFeedBackEvaluations @InterventionID", new { InterventionID = InterventionID }).ToList();
                return output;
            }
        }
    }
}
