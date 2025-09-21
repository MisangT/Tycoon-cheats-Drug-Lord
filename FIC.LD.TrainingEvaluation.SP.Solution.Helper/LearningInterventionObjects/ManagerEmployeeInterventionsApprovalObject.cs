using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIC.LD.TrainingEvaluation.SP.Solution.Helper
{
    public class ManagerEmployeeInterventionsApprovalObject
    {
        public int EmployeeInterventionID
        { get; set; }

        public int ManagerEmployeeID
        { get; set; }

        public int WasInterventionONPDP
        { get; set; }

        public int Approved
        { get; set; }
    }
}
