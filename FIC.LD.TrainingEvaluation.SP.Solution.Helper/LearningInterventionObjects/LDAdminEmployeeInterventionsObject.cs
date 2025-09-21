using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIC.LD.TrainingEvaluation.SP.Solution.Helper
{
    public class LDAdminEmployeeInterventionsObject
    {
        public int EmployeeInterventionID
        { get; set; }

        public int? LDEmployeeID
        { get; set; }

        public string ServiceProvider
        { get; set; }

        public DateTime StartDateOfTraining
        { get; set; }

        public DateTime EndDateOfTraining
        { get; set; }

        public decimal Cost
        { get; set; }

        public string TypeOfIntervention
        { get; set; }

        public int IsBelowThreshold
        { get; set; }

        public int ReceivedSignedContract
        { get; set; }

        public int Approved
        { get; set; }

        public int Submitted
        { get; set; }
    }
}
