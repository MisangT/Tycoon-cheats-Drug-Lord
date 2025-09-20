using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIC.LD.TrainingEvaluation.SP.Solution.Helper
{
    public class LDInterventionsAdminObjects
    {        
        public int ID
        { get; }
        public int InterventionID
        { get; }

        public int EmployeeID
        { get; }

        public string FullName
        { get; }

        public string Username
        { get; }

        public string DiscussedWithManager
        { get; }

        public string Disclaimer
        { get; }

        public string DevelopmentNeeds
        { get; }

        public string NameOfLearning
        { get; }
              
        public string EmployeeExpectations
        { get; }
              
        public DateTime DateCreated
        { get; }

        public string Status
        { get; }

        public string ManagerName
        { get; }

    }
}
