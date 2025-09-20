using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using Microsoft.SharePoint;

namespace FIC.LD.TrainingEvaluation.SP.Solution.Helper
{
    public class ActiveDirectoryDataAccess
    {
        /// <summary>
        /// Get Employee Information from ActiveDirectory using Employee EmailAddress
        /// </summary>
        /// <param name="EmailAddress"></param>
        /// <returns></returns>
        public ActiveDirectoryEmployeeObject EmployeeDetails(string UserName)
        {
            ActiveDirectoryEmployeeObject ADEmployeeDetails = new ActiveDirectoryEmployeeObject();

            try
            {
                DirectoryEntry directoryentry = GetDirectoryObject();
                DirectorySearcher directoryentrySearch = new DirectorySearcher();
                directoryentrySearch.SearchRoot = directoryentry;
                directoryentrySearch.Filter = string.Format("(&(objectClass=user)(SAMAccountName={0}))", UserName);

                SearchResult result = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    result = directoryentrySearch.FindOne();
                });

                if (!(result == null))
                {
                    try { ADEmployeeDetails.Name = Convert.ToString(result.Properties["givenName"][0]);}catch { }
                    try { ADEmployeeDetails.SurName = Convert.ToString(result.Properties["sn"][0]); } catch { }
                    try { ADEmployeeDetails.Logonname = Convert.ToString(result.Properties["sAMAccountName"][0]); } catch { }
                    try { ADEmployeeDetails.Designation = Convert.ToString(result.Properties["description"][0]); } catch(Exception) { throw new Exception($"Missing Employee Designation for {UserName}"); }
                    try { ADEmployeeDetails.Department = Convert.ToString(result.Properties["department"][0]); } catch (Exception) { throw new Exception($"Missing Employee Department for {UserName}");  }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ADEmployeeDetails;
        }

        /// <summary>
        /// Get Active Directory Object
        /// </summary>
        /// <returns></returns>
        private DirectoryEntry GetDirectoryObject()
        {
            DirectoryEntry ActiveDirectoryConnection;
            ActiveDirectoryConnection = new DirectoryEntry("LDAP://DC=fic,DC=gov,DC=za");
            ActiveDirectoryConnection.AuthenticationType = AuthenticationTypes.Secure;
            return ActiveDirectoryConnection;
        }
    }
}
