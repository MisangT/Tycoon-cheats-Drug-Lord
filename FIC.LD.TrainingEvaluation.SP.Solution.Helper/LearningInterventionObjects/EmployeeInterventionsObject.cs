using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIC.LD.TrainingEvaluation.SP.Solution.Helper
{
    public class EmployeeInterventionsObject
    {
        public int ? InterventionID
        { get; set; }

        public int ? EmployeeID
        { get; set; } 
        
        public string Designation
        { get; set; }

        public int DiscussedWithManager
        { get; set; }

        public int Disclaimer
        { get; set; }

        public string DevelopmentNeeds
        { get; set; }

        public string NameOfLearning
        { get; set; }

        public string EmployeeExpectations
        { get; set; }

        public int IsBelowThreshold
        { get; set; }

        public int WorkBackContractSubmitted
        { get; set; }

        public int Submitted
        { get; set; }

        public string Manager
        { get; set; }

        public string Status
        { get; set; }

        public string TrainingType
        { get; set; }

        public int TrainingTypeContractSubmitted
        { get; set; }

       

    }
}
