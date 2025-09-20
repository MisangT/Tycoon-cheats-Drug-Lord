using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIC.LD.TrainingEvaluation.SP.Solution.Helper
{
    public class InterventionEscalationObjects
    {
        public int InterventionID
        { get; set; }

        public int EmployeeID
        { get; set; }

        public string Username
        { get; set; }

        public int ManagerID
        { get; set; }

        public string ManagerName
        { get; set; }

        public string LearningIntervention
        { get; set; }

        public string Reason
        { get; set; }
    }
}
