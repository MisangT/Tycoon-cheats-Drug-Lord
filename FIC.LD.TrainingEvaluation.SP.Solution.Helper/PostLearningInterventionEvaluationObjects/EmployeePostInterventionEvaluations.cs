using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIC.LD.TrainingEvaluation.SP.Solution.Helper
{
    public class EmployeePostInterventionEvaluation
    {
        public int InterventionID
        { get; }

        public string Username
        { get; }

        public string ManagerUserName
        { get; }

        public DateTime StartDateOfTraining
        { get; }

        public DateTime EndDateOfTraining
        { get; }

        public DateTime FirstReminderDate
        { get; }

        public DateTime DueDate
        { get; }

        public string Status
        { get; }
    }
}
