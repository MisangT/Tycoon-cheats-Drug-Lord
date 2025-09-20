using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace FIC.LD.TrainingEvaluation.SP.Solution.Helper
{
    public class Configs
    {
        /// <summary>
        /// Gets the Database Connection
        /// </summary>
        public static string DatabaseConnection
        {
            get
            {
                string DBConnection = string.Empty;
                DBConnection = ConfigurationManager.ConnectionStrings["LDTrainingConnection"].ConnectionString;

                if (DBConnection.Length > 0)
                    return DBConnection;
                else
                    return "Database connection setting missing";
            }
        }

        /// <summary>
        /// Gets the Training Cost Threshold from the Config
        /// </summary>
        public static string TrainingCostThreshold
        {
            get
            {
                string trainingCostThreshold;
                trainingCostThreshold = ConfigurationManager.AppSettings["TrainingCostThreshold"]; 

                if (trainingCostThreshold.Length > 0)
                    return trainingCostThreshold;
                else
                    return "TrainingCostThreshold setting missing";
            }
        }

        /// <summary>
        /// Gets the Training Cost Threshold Label from the Config
        /// </summary>
        public static string TrainingCostThresholdLabel
        {
            get
            {
                string trainingCostThresholdLabel = string.Empty;
                trainingCostThresholdLabel = ConfigurationManager.AppSettings["TrainingCostThresholdLabel"];

                if (trainingCostThresholdLabel.Length > 0)
                    return trainingCostThresholdLabel;
                else
                    return "TrainingCostThresholdLabel setting missing";
            }
        }

        /// <summary>
        /// Get the SMTP Host
        /// </summary>
        public static string SMTPHost
        {
            get
            {
                string smtphost = string.Empty;
                smtphost = ConfigurationManager.AppSettings["SMTP"];

                if (smtphost.Length > 0)
                    return smtphost;
                else
                    return "SMTP Host setting missing";
            }
        }

        /// <summary>
        /// Get the SPSite URL
        /// </summary>
        public static string SPSiteURL
        {
            get
            {
                string spsiteurl = string.Empty;
                spsiteurl = ConfigurationManager.AppSettings["SPSiteURL"];

                if (spsiteurl.Length > 0)
                    return spsiteurl;
                else
                    return "SPSiteURL setting missing";
            }
        }

        /// <summary>
        /// Get the Workback library name
        /// </summary>
        public static string WorkbackLibraryName
        {
            get
            {
                string library = string.Empty;
                library = ConfigurationManager.AppSettings["DocumentLibrary"];

                if (library.Length > 0)
                    return library;
                else
                    return "DocumentLibrary library setting missing";
            }
        }

        /// <summary>
        /// Get the Type of Training library name
        /// </summary>
        public static string TrainingTypeLibraryName
        {
            get
            {
                string library = string.Empty;
                library = ConfigurationManager.AppSettings["TrainingTypeLibraryName"];

                if (library.Length > 0)
                    return library;
                else
                    return "DocumentLibrary library setting missing";
            }
        }

        /// <summary>
        /// Get the from email address
        /// </summary>
        public static string FromEmail
        {
            get
            {
                string fromemail = string.Empty;
                fromemail = ConfigurationManager.AppSettings["FromEmail"];

                if (fromemail.Length > 0)
                    return fromemail;
                else
                    return "From Email setting missing";
            }
        }

        /// <summary>
        /// Gets SharePoint Group for LD Admin
        /// </summary>
        public static string LDAdminSPGroupName
        {
            get
            {
                string ldadminspgroupname = string.Empty;
                ldadminspgroupname = ConfigurationManager.AppSettings["LDAdminSPGroupName"];

                if (ldadminspgroupname.Length > 0)
                    return ldadminspgroupname;
                else
                    return "LDAdminSPGroupName setting missing";
            }
        }

        /// <summary>
        /// Gets SharePoint Group for LD Managers
        /// </summary>
        public static string LDManagerSPGroupName
        {
            get
            {
                string ldmanagerspgroupname = string.Empty;
                ldmanagerspgroupname = ConfigurationManager.AppSettings["LDManagerSPGroupName"];

                if (ldmanagerspgroupname.Length > 0)
                    return ldmanagerspgroupname;
                else
                    return "LDManagerSPGroupName setting missing";
            }
        }

        /// <summary>
        /// Get the encryption key
        /// </summary>
        public static string EncryptDecryptKey
        {
            get
            {
                string encryptionKey = string.Empty;
                encryptionKey = ConfigurationManager.AppSettings["EncryptDecryptKey"];

                if (encryptionKey.Length > 0)
                    return encryptionKey;
                else
                    return "Encryption or Decryption setting missing";
            }
        }

        /// <summary>
        /// Gets SharePoint Group for Line Managers
        /// </summary>
        public static string LineManagersSPGroupName
        {
            get
            {
                string linemanagersspgroupname = string.Empty;
                linemanagersspgroupname = ConfigurationManager.AppSettings["LineManagersSPGroupName"];

                if (linemanagersspgroupname.Length > 0)
                    return linemanagersspgroupname;
                else
                    return "LineManagersSPGroupName setting missing";
            }
        }
    }
}
