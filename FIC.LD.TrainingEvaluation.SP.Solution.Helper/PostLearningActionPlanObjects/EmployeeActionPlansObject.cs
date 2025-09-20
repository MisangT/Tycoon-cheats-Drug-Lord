using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIC.LD.TrainingEvaluation.SP.Solution.Helper
{
    public class EmployeeActionPlansObject
    {
        public int ? ID
        { get; set; }

        public int EmployeePostLearningActionPlanID
        { get; set; }

        public int ? InterventionID
        { get; set; }

        public string LearningObjectives
        { get; set; }

        public string ActionPlan
        { get; set; }
        
        public string HowToImplement
        { get; set; }

        public string WhenToBeDone
        { get; set; }
    }
}
