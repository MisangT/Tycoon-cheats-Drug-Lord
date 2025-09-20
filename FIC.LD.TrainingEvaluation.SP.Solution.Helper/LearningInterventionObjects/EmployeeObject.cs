using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIC.LD.TrainingEvaluation.SP.Solution.Helper
{
    public class EmployeeObject
    {
        public int EmployeeID
        { get; set; }

        public string Username
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public string Surname
        {
            get; set;
        }

        public string Designation
        {
            get; set;
        }

        public string Department
        {
            get; set;
        }

        public int? ManagerID
        { get; set; }
    }
}
