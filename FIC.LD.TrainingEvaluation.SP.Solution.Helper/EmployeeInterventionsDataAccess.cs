using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace FIC.LD.TrainingEvaluation.SP.Solution.Helper
{
    public class EmployeeInterventionsDataAccess
    {
        /// <summary>
        /// Check Employee ID
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public int? EmployeeID(string Username)
        {
            int? EmployeeID = null;

            try
            {
                using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
                {
                    EmployeeID = connection.Query<int>("FIC_EmployeeID @Username", new { Username = Username }).Single();
                }
            }
            catch (Exception ex)
            {
            }
            return EmployeeID;
        }

        /// <summary>
        /// Gets the Employee Username by EmployeeID
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>                      
        public string EmployeeUserName(int EmployeeID)
        {
            string Username = string.Empty;

            try
            {
                using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
                {
                    Username = connection.Query<string>("FIC_EmployeeUserName @EmployeeID", new { EmployeeID = EmployeeID }).Single();
                }
            }
            catch (Exception ex)
            {
            }
            return Username;
        }

        /// <summary>
        /// Get Employee Manager Name
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public string EmployeeManager(string Username)
        {
            string ManagerName = string.Empty;

            try
            {
                using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
                {
                    ManagerName = connection.Query<string>("FIC_EmployeeManager @Username", new { Username = Username }).Single();
                }
            }
            catch (Exception ex)
            {
            }
            return ManagerName;
        }

        /// <summary>
        /// Get Employee Designation
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public string EmployeeDesignation(string Username)
        {
            string Designation = string.Empty;

            try
            {
                using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
                {
                    Designation = connection.Query<string>("FIC_EmployeeDesignation @Username", new { Username = Username }).Single();
                }
            }
            catch (Exception ex)
            {
            }
            return Designation;
        }

        /// <summary>
        /// Insert Employee
        /// </summary>
        /// <param name="Employee"></param>
        public int InsertEmployee(EmployeeObject Employee)
        {
            var EmployeeID = 0;
            try
            {
                using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
                {
                    EmployeeID = connection.Query<int>("FIC_InsertEmployee @Username, @Name, @Surname, @Designation, @Department , @ManagerID", new { Username = Employee.Username, Name = Employee.Name, Surname = Employee.Surname, Designation = Employee.Designation, Department = Employee.Department, ManagerID = Employee.ManagerID }).Single();
                }
            }
            catch
            {
            }

            return EmployeeID;
        }       

        /// <summary>
        /// Update Employee
        /// </summary>
        /// <param name="Employee"></param>
        public void UpdateEmployee(EmployeeObject Employee)
        {            
            try
            {
                using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
                {
                    connection.Execute("FIC_UpdateEmployee @EmployeeID, @Designation, @ManagerID", new { EmployeeID = Employee.EmployeeID, Designation = Employee.Designation, ManagerID = Employee.ManagerID });
                }
            }
            catch
            {
            }            
        }

        /// <summary>
        /// Insert Employee Intervention
        /// </summary>
        /// <param name="Intervention"></param>
        public int InsertEmployeeIntervention(EmployeeInterventionsObject Intervention)
        {
            var InterventionID = 0;

            try
            {
                using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
                {
                    InterventionID = connection.Query<int>("FIC_InsertEmployeeIntervention @EmployeeID, @DiscussedWithManager, @TrainingType, @TrainingTypeContractSubmitted, @Disclaimer, @DevelopmentNeeds,@NameOfLearning, @EmployeeExpectations, @Submitted", new { EmployeeID = Intervention.EmployeeID, DiscussedWithManager = Intervention.DiscussedWithManager, TrainingType = Intervention.TrainingType, TrainingTypeContractSubmitted = Intervention.TrainingTypeContractSubmitted, Disclaimer = Intervention.Disclaimer, DevelopmentNeeds = Intervention.DevelopmentNeeds, NameOfLearning = Intervention.NameOfLearning, EmployeeExpectations = Intervention.EmployeeExpectations, Submitted = Intervention.Submitted }).Single();
                }
            }
            catch
            {
            }

            return InterventionID;
        }

        /// <summary>
        /// Updates the Employee Intervention by InterventionID
        /// </summary>
        /// <param name="Intervention"></param>
        /// <returns></returns>
        public int UpdateEmployeeIntervention(EmployeeInterventionsObject Intervention)
        {
            var InterventionID = 0;

            try
            {
                using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
                {
                    InterventionID = connection.Query<int>("FIC_UpdateEmployeeIntervention @InterventionID, @EmployeeID, @DiscussedWithManager, @Disclaimer, @DevelopmentNeeds,@NameOfLearning,@EmployeeExpectations,@WorkBackContractSubmitted,@Submitted", new { InterventionID = Intervention.InterventionID, EmployeeID = Intervention.EmployeeID, DiscussedWithManager = Intervention.DiscussedWithManager, Disclaimer = Intervention.Disclaimer, DevelopmentNeeds = Intervention.DevelopmentNeeds, NameOfLearning = Intervention.NameOfLearning, EmployeeExpectations = Intervention.EmployeeExpectations, WorkBackContractSubmitted = Intervention.WorkBackContractSubmitted, Submitted = Intervention.Submitted }).Single();
                }
            }
            catch
            {
            }

            return InterventionID;
        }

        /// <summary>
        /// Insert LD Employee Interventions
        /// </summary>
        /// <param name="Intervention"></param>
        public void InsertLDAdminEmployeeIntervention(LDAdminEmployeeInterventionsObject Intervention)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
                {
                    List<LDAdminEmployeeInterventionsObject> EmployeeIntervention = new List<LDAdminEmployeeInterventionsObject>();
                    EmployeeIntervention.Add(new LDAdminEmployeeInterventionsObject { EmployeeInterventionID = Intervention.EmployeeInterventionID, LDEmployeeID = Intervention.LDEmployeeID, ServiceProvider = Intervention.ServiceProvider, StartDateOfTraining = Intervention.StartDateOfTraining, EndDateOfTraining = Intervention.EndDateOfTraining, Cost = Intervention.Cost, TypeOfIntervention = Intervention.TypeOfIntervention, IsBelowThreshold = Intervention.IsBelowThreshold, Approved = Intervention.Approved, Submitted = Intervention.Submitted });
                    connection.Execute("FIC_InsertLDAdminEmployeeIntervention @EmployeeInterventionID,@LDEmployeeID,@ServiceProvider,@StartDateOfTraining,@EndDateOfTraining,@Cost,@TypeOfIntervention,@IsBelowThreshold,@Approved,@Submitted", EmployeeIntervention);
                }
            }
            catch(Exception ex)
            {
            }
        }

        /// <summary>
        /// Update LD Employee Intervention
        /// </summary>
        /// <param name="Intervention"></param>
        public void UpdateLDAdminEmployeeIntervention(LDAdminEmployeeInterventionsObject Intervention)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
                {
                    List<LDAdminEmployeeInterventionsObject> EmployeeIntervention = new List<LDAdminEmployeeInterventionsObject>();
                    EmployeeIntervention.Add(new LDAdminEmployeeInterventionsObject { EmployeeInterventionID = Intervention.EmployeeInterventionID, LDEmployeeID = Intervention.LDEmployeeID, ServiceProvider = Intervention.ServiceProvider, StartDateOfTraining = Intervention.StartDateOfTraining, EndDateOfTraining = Intervention.EndDateOfTraining, Cost = Intervention.Cost, TypeOfIntervention = Intervention.TypeOfIntervention, IsBelowThreshold = Intervention.IsBelowThreshold, ReceivedSignedContract = Intervention.ReceivedSignedContract, Approved = Intervention.Approved, Submitted = Intervention.Submitted });
                    connection.Execute("FIC_UpdateLDAdminEmployeeIntervention @EmployeeInterventionID,@LDEmployeeID,@ServiceProvider,@StartDateOfTraining,@EndDateOfTraining,@Cost,@TypeOfIntervention,@IsBelowThreshold,@ReceivedSignedContract,@Approved,@Submitted", EmployeeIntervention);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Insert Manager Employee Interventions
        /// </summary>
        /// <param name="Intervention"></param>
        public void InsertManagerEmployeeIntervention(ManagerEmployeeInterventionsObject Intervention)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
                {
                    List<ManagerEmployeeInterventionsObject> EmployeeIntervention = new List<ManagerEmployeeInterventionsObject>();
                    EmployeeIntervention.Add(new ManagerEmployeeInterventionsObject { EmployeeInterventionID = Intervention.EmployeeInterventionID, ManagerEmployeeID = Intervention.ManagerEmployeeID, Expectations = Intervention.Expectations, Submitted = Intervention.Submitted });
                    connection.Execute("FIC_InsertManagerEmployeeIntervention @EmployeeInterventionID,@ManagerEmployeeID,@Expectations,@Submitted", EmployeeIntervention);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Insert Manager Employee Interventions Approval
        /// </summary>
        /// <param name="Intervention"></param>
        public void InsertManagerEmployeeInterventionsApproval(ManagerEmployeeInterventionsApprovalObject Intervention)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
                {
                    List<ManagerEmployeeInterventionsApprovalObject> EmployeeIntervention = new List<ManagerEmployeeInterventionsApprovalObject>();
                    EmployeeIntervention.Add(new ManagerEmployeeInterventionsApprovalObject { EmployeeInterventionID = Intervention.EmployeeInterventionID, ManagerEmployeeID = Intervention.ManagerEmployeeID, WasInterventionONPDP = Intervention.WasInterventionONPDP, Approved = Intervention.Approved });
                    connection.Execute("FIC_InsertManagerInterventionsApproval @EmployeeInterventionID,@ManagerEmployeeID,@WasInterventionONPDP,@Approved", EmployeeIntervention);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Insert L & D Employee Interventions Approval
        /// </summary>
        /// <param name="Intervention"></param>
        public void InsertLDEmployeeInterventionsApproval(LDEmployeeInterventionsApprovalObject Intervention)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
                {
                    List<LDEmployeeInterventionsApprovalObject> EmployeeIntervention = new List<LDEmployeeInterventionsApprovalObject>();
                    EmployeeIntervention.Add(new LDEmployeeInterventionsApprovalObject { EmployeeInterventionID = Intervention.EmployeeInterventionID, LDEmployeeID = Intervention.LDEmployeeID, FICLDPolicyANDProcedures = Intervention.FICLDPolicyANDProcedures, FundsAvailable = Intervention.FundsAvailable, Approved = Intervention.Approved });
                    connection.Execute("FIC_InsertLDEmployeeInterventionsApproval @EmployeeInterventionID,@LDEmployeeID,@FICLDPolicyANDProcedures,@FundsAvailable,@Approved", EmployeeIntervention);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Gets a Intervention for Employee
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="InterventionID"></param>
        /// <returns></returns>
    
        /// <summary>
        /// Gets Intervention List by EmployeeID
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public List<EmployeeInterventionsObject> EmployeeInterventions(int? EmployeeID)
        {
            using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
            {
                var output = connection.Query<EmployeeInterventionsObject>("FIC_SELECTEmployeeInterventions @EmployeeID", new { EmployeeID = EmployeeID }).ToList();
                return output;
            }
        }      

        /// <summary>
        /// Gets ManagerEmployeesInterventions by ManagerID
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public List<AdminEmployeesInterventionsObject> ManagerEmployeesInterventions(int ManagerID)
        {
            using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
            {
                var output = connection.Query<AdminEmployeesInterventionsObject>("FIC_SELECTManagerEmployeesInterventions @ManagerID", new { ManagerID = ManagerID }).ToList();
                return output;
            }
        }

        /// <summary>
        /// Get Employee Intervention by InterventionID
        /// </summary>
        /// <param name="InterventionID"></param>
        /// <returns></returns>
        public EmployeeInterventionsObject Intervention(int InterventionID)
        {
            using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
            {
                var output = connection.Query<EmployeeInterventionsObject>("FIC_EmployeeIntervention @InterventionID", new { InterventionID = InterventionID }).Single();
                return output;
            }
        }

        /// <summary>
        /// Get List of all Intervention
        /// </summary>
        /// <returns></returns>
        public List<AdminEmployeesInterventionsObject> Interventions()
        {
            using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
            {
                var output = connection.Query<AdminEmployeesInterventionsObject>("FIC_SELECTEmployeesInterventions").ToList();
                return output;
            }
        }

        public List<EmployeeDetailsObject> SelectEmployeesForLDAdmin()
        {
            using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
            {
                var output = connection.Query<EmployeeDetailsObject>("FIC_SelectEmployeesForLDAdmin").ToList();
                return output;
            }
        }
        
        /// <summary>
        /// Get LDEmployeeIntervention by InterventionID
        /// </summary>
        /// <param name="InterventionID"></param>
        /// <returns></returns>
        public LDAdminEmployeeInterventionsObject LDAdminEmployeeIntervention(int InterventionID)
        {
            using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
            {
                var output = connection.Query<LDAdminEmployeeInterventionsObject>("FIC_LDAdminEmployeeIntervention @InterventionID", new { InterventionID = InterventionID }).Single();
                return output;
            }
        }

        /// <summary>
        /// Get ManagerEmployeeIntervention by InterventionID
        /// </summary>
        /// <param name="InterventionID"></param>
        /// <returns></returns>
        public ManagerEmployeeInterventionsObject ManagerEmployeeIntervention(int InterventionID)
        {
            using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
            {
                var output = connection.Query<ManagerEmployeeInterventionsObject>("FIC_ManagerEmployeeIntervention @InterventionID", new { InterventionID = InterventionID }).Single();
                return output;
            }
        }

        /// <summary>
        /// Get ManagerEmployeeInterventionsApproval by InterventionID
        /// </summary>
        /// <param name="InterventionID"></param>
        /// <returns></returns>
        public ManagerEmployeeInterventionsApprovalObject ManagerEmployeeInterventionApproval(int InterventionID)
        {
            using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
            {
                var output = connection.Query<ManagerEmployeeInterventionsApprovalObject>("FIC_ManagerEmployeeInterventionApproval @InterventionID", new { InterventionID = InterventionID }).Single();
                return output;
            }
        }

        /// <summary>
        /// Get LDEmployeeInterventionsApproval by InterventionID
        /// </summary>
        /// <param name="InterventionID"></param>
        /// <returns></returns>
        public LDEmployeeInterventionsApprovalObject LDEmployeeInterventionApproval(int InterventionID)
        {
            using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
            {
                var output = connection.Query<LDEmployeeInterventionsApprovalObject>("FIC_LDEmployeeInterventionApproval @InterventionID", new { InterventionID = InterventionID }).Single();
                return output;
            }
        }      

        /// <summary>
        /// Insert Form Status 
        /// </summary>
        /// <param name="Status"></param>
        public void InsertInterventionFormStatus(FormStatusObjects Status)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
                {
                    List<FormStatusObjects> FormStatus = new List<FormStatusObjects>();
                    FormStatus.Add(new FormStatusObjects { InterventionID = Status.InterventionID , EmployeeID = Status.EmployeeID , Status = Status.Status, Reason = Status.Reason });
                    connection.Execute("FIC_InsertInterventionFormStatus @InterventionID,@EmployeeID,@Status,@Reason", FormStatus);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Gets Employee details by InterventionID
        /// </summary>
        /// <param name="InterventionID"></param>
        /// <returns></returns>
        public EmployeeDetailsObject EmployeeDetails(int InterventionID)
        {
            using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
            {
                var output = connection.Query<EmployeeDetailsObject>("FIC_EmployeeDetailsBYInterventionID @InterventionID", new { InterventionID = InterventionID }).SingleOrDefault();
                return output;
            }
        }

        /// <summary>
        /// Checks if the Individual Skills Development Approval Form is Completed
        /// </summary>
        /// <param name="InterventionID"></param>
        /// <returns></returns>
        public int? IsIndividualSkillsDevelopmentApprovalCompleted(IndividualSkillsDevelopmentApprovalCompletedObjects Intervention)
        {
            int? IsApproved = null;

            try
            {
                using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
                {
                    IsApproved = connection.Query<int?>("FIC_IndividualSkilsDevApprovalCompleted @InterventionID", new { InterventionID = Intervention.InterventionID }).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {

            }

            return IsApproved;
        }
        
        /// <summary>
        /// Update Employee Interventions by LD Admin
        /// </summary>
        public void UpdateLDAdminEmployeeIntervention(EmployeeInterventionsObject Intervention)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
                {
                    List<EmployeeInterventionsObject> EmployeeIntervention = new List<EmployeeInterventionsObject>();
                    EmployeeIntervention.Add(new EmployeeInterventionsObject { InterventionID = Intervention.InterventionID, DevelopmentNeeds = Intervention.DevelopmentNeeds, NameOfLearning = Intervention.NameOfLearning });
                    connection.Execute("FIC_UpdateLDAdminEmployeeInterventions @InterventionID,@DevelopmentNeeds,@NameOfLearning", EmployeeIntervention);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Check if the intervention exists in the Employee Intervention table
        /// </summary>
        /// <param name="InterventionID"></param>
        /// <returns></returns>
        public EmployeeInterventionsObject CheckEmployeeIntervention(int InterventionID)
        {
            using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
            {
                var output = connection.Query<EmployeeInterventionsObject>("FIC_CheckEmployeeIntervention @InterventionID", new { InterventionID = InterventionID }).SingleOrDefault();
                return output;
            }
        }

        public EmployeeInterventionsObject CheckInterventionStatusForLDAdmin(int InterventionID)
        {
            using (IDbConnection connection = new SqlConnection(Configs.DatabaseConnection))
            {
                var output = connection.Query<EmployeeInterventionsObject>("FIC_CheckEmployeeIntervention @InterventionID", new { InterventionID = InterventionID }).SingleOrDefault();
                return output;
            }
        }
    }
}
