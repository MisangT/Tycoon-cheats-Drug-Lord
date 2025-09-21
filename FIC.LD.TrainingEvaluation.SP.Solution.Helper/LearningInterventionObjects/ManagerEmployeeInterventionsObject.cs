using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIC.LD.TrainingEvaluation.SP.Solution.Helper
{
    public class ManagerEmployeeInterventionsObject
    {
        public int EmployeeInterventionID
        { get; set; }

        public int ManagerEmployeeID
        { get; set; }

        public string Expectations
        { get; set; }

        public int Submitted
        { get; set; }
        
    }
}
