using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIC.LD.TrainingEvaluation.SP.Solution.Helper
{
    public class ManagerActionPlanEvaluationObject
    {
        public int? APEInterventionID
        { get; set; }

        public int? APEEmployeeID
        { get; set; }

        public string Department
        { get; set; }

        public string NameOfLearningIntervention
        { get; set; }

        public DateTime StartDateOfTraining
        { get; set; }

        public DateTime EndDateOfTraining
        { get; set; }

        public string LineManagerName
        { get; set; }

        public int SkillsDemonstration
        { get; set; }

        public string ManagerApplicationOfNewSkills
        { get; set; }

        public string JobPerformanceChanges
        { get; set; }

        public string AdditionalComments
        { get; set; }

        public int EmployeePerformanceImprovement
        { get; set; }

        public int ManagerSubmitted
        { get; set; }

        public int EmployeeSubmitted
        { get; set; }
    }
}
