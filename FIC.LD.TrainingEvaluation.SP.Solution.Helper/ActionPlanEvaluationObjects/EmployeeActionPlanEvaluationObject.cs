using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIC.LD.TrainingEvaluation.SP.Solution.Helper
{
    public class EmployeeActionPlanEvaluationObject
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
        
        public string LearningInterventionRating
        { get; set; }

        public int EmployeeApplicationOfNewSkillsChoice
        { get; set; }

        public string EmployeeApplicationOfNewSkillsComments
        { get; set; }

        public int RequiredResourcesAndSupportOfferedChoice
        { get; set; }

        public string RequiredResourcesAndSupportOfferedComments
        { get; set; }

        public string EmployeeNewKnowledge
        { get; set; }

        public int EmployeeSubmitted
        { get; set; }

        public DateTime EmployeeDateCreated
        { get; set; }
    }
}
