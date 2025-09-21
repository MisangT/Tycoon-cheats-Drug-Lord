using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIC.LD.TrainingEvaluation.SP.Solution.Helper
{
    [Serializable]
    public class AdminEmployeesInterventionsObject
    {
        public int ID
        { get; }

        public int InterventionID
        { get; }

        public string FullName
        { get; }

        public string NameOfLearning
        { get; }

        public string DevelopmentNeeds
        { get; }

        public string DiscussedWithManager
        { get; }

        public string Disclaimer
        { get; }

        public string EmployeeExpectations
        { get; }

        public string Status
        { get; }

        public DateTime DateCreated
        { get; }                    
    }
}
