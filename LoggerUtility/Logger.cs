using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerUtility
{
    public static class Logger
    {
        #region Private Members
        #region Config Keys
        private const string LOGGING_TYPE = "LOGGING_TYPE";
        private const string LOGFILE_PATH = "LOGFILE_PATH";
        private const string LOGGING = "LOGGING";
        #endregion
        private static bool loggingEnabled;
        private static string loggingType = string.Empty;
        private static string loggingFilePath = string.Empty;

        private static StreamWriter streamWriter = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Empty Constructor
        /// </summary>
        static Logger()
        {
            LoadConfig();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Write Source,date,time,computer and error information to the text file
        /// </summary>        
        /// <param name="objError"></param>       
        /// <returns>false if the problem persists</returns>
        public static bool Log(LogEntry objError)
        {
            bool result = false;
            string strException = string.Empty;
            try
            {
                if (loggingEnabled)
                {

                    LogModel model = new LogModel
                    {
                        Type = objError.Type.ToString(),
                        Source = GetCallerInfo(),
                        Message = objError.Message,
                    };

                    WriteLog(model);
                    result = true;
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Load initial configuration
        /// </summary>
        private static void LoadConfig()
        {
            try
            {
                bool.TryParse(ConfigurationManager.AppSettings[LOGGING].ToString(), out loggingEnabled);
                if (loggingEnabled)
                {
                    loggingFilePath = ConfigurationManager.AppSettings[LOGFILE_PATH];
                    loggingType = ConfigurationManager.AppSettings[LOGGING_TYPE];
                    CheckLogDirectory(loggingFilePath);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        /// <summary>
        /// Create a directory if not exists
        /// </summary>
        /// <param name="strLogPath"></param>
        /// <returns></returns>
        private static bool CheckLogDirectory(string strLogPath)
        {
            try
            {
                int nFindSlashPos = strLogPath.Trim().LastIndexOf("\\");
                string strDirectoryname = strLogPath.Trim().Substring(0, nFindSlashPos);

                if (false == Directory.Exists(strDirectoryname))
                    Directory.CreateDirectory(strDirectoryname);

                return true;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Check the log file exist. If it is not available, creae it
        /// </summary>
        /// <returns>Log file path</returns>
        private static bool CreateLogFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                    fileStream.Close();
                    fileStream.Dispose();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static string GetCallerInfo()
        {
            string callerString = "";
            var stackTrace = new StackTrace();
            if (stackTrace != null)
            {
                var methodBase = stackTrace.GetFrame(2).GetMethod();
                if (methodBase != null)
                {
                    var classObj = methodBase.ReflectedType;
                    if (classObj != null)
                    {
                        var amespaceObj = classObj.Namespace;

                        callerString = "[" + amespaceObj + "::" + classObj.Name + "::" + methodBase.Name + "]";
                    }
                }
            }
            return callerString;
        }

        private static void WriteLog(LogModel model)
        {
            // if (loggingType.ToUpper().Equals(ServiceLoggingType))
            {
                //if (!string.IsNullOrEmpty(loggingServiceUrl))
                //{
                //    var client = new RestClient(loggingServiceUrl);

                //    var request = new RestRequest(Method.POST);
                //    request.AddHeader("Content-Type", "text/json");

                //    request.RequestFormat = DataFormat.Json;

                //    var jsonString = request.JsonSerializer.Serialize(model);

                //    request.AddParameter("text/json", jsonString, ParameterType.RequestBody);

                //    // execute the request
                //    var response = client.Execute(request);
                //    var content = response.Content; // raw content as string

                //}
            }
            //else if (loggingType.ToUpper().Equals(FileLoggingType))
            {

                if (File.Exists(loggingFilePath))
                {
                    //Delete File if size [512KB]
                    FileInfo fileInfo = new FileInfo(loggingFilePath);
                    float fileSize = fileInfo.Length / 1024;
                    if (fileSize > 512)
                    {
                        fileInfo.MoveTo(loggingFilePath.Substring(0, loggingFilePath.LastIndexOf('.')) + DateTime.Now.ToString("yyyyMMMdd-hhmmss") + "." + "log");
                        //File.Delete(errorLoggingFilePath);
                        CreateLogFile(loggingFilePath);
                    }
                    //Delete ends
                }
                else
                    CreateLogFile(loggingFilePath);

                streamWriter = new StreamWriter(loggingFilePath, true);
                streamWriter.WriteLine("^^-------------------------------------------------------------------^^");
                streamWriter.WriteLine("       Type : " + model.Type);
                streamWriter.WriteLine("     Source : " + model.Source);
                streamWriter.WriteLine("Date & Time : " + model.LoggedOn.ToString());
                streamWriter.WriteLine("    Message : " + model.Message);
                streamWriter.WriteLine("^^-------------------------------------------------------------------^^");
                streamWriter.Flush();
                streamWriter.Close();
            }
        }
        #endregion

        internal class LogModel
        {
            public string ApplicationName
            {
                get;
                set;
            }

            public string Host
            {
                get;
                set;
            }

            public int Id
            {
                get;
                set;
            }

            public DateTime LoggedOn
            {
                get;
                set;
            }

            public string Message
            {
                get;
                set;
            }

            public string Source
            {
                get;
                set;
            }

            public string Type
            {
                get;
                set;
            }

            public LogModel()
            {
                this.LoggedOn = DateTime.Now;
            }
        }
    }

    public class LogEntry
    {
        public LogEntry(LogType logType, string message)
        {
            this.Type = logType;
            this.Message = message;
        }
        public LogType Type { get; set; }
        public string Message { get; set; }
    }

    public enum LogType
    {
        ERROR,
        INFORMATION,
        WARNING
    }
}
