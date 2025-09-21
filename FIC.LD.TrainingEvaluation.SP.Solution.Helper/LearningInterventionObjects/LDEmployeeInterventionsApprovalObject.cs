using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIC.LD.TrainingEvaluation.SP.Solution.Helper
{
    public class LDEmployeeInterventionsApprovalObject
    {
        public int EmployeeInterventionID
        { get; set; }

        public int LDEmployeeID
        { get; set; }

        public int FICLDPolicyANDProcedures
        { get; set; }

        public int FundsAvailable
        { get; set; }

        public int Approved
        { get; set; }
    }
}
