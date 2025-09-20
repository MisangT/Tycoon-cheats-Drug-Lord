using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIC.LD.TrainingEvaluation.SP.Solution.Helper
{
    [Serializable]
    public class EmployeeDetailsObject
    {
        public int InterventionID
        { get; set; }

        public int EmployeeID
        { get; set; }

        public string Username
        { get; set; }

        public string FullName
        { get; set; }

        public string Designation
        { get; set; }

        public string Department
        { get; set; }

        public string NameOfLearning
        { get; set; }

        public string ServiceProvider
        { get; set; }

        public string StartDateOfTraining
        { get; set; }

        public string EndDateOfTraining
        { get; set; }

        public string ManagerName
        { get; set; }
    }
}
