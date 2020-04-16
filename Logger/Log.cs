////////////////////////////////////////////////////////////////////////////////
//
// Author: Chen, Changwei
//
//------------------------------------------------------------------------------
//
// Class of Log.
//
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nile.Component;
using Nile.Definition;

namespace Nile.Logger
{
    public class Log : ComponentBase, ILog, ICommonComponent, IDisposable//, ILogger
    {
        string SerialNumber { get; set; }
        string FileName { get; set; }
        int Position { get; set; }
        DateTime StartTime { get; set; }
        string Path { get; set; }
        private StreamWriter swLog = null;
        private int[] CheckedSeverity = null;

        #region Event and delegate

        #endregion

        #region constructors
        public Log()
        {
        }

        public Log(string SerialNumber)
        {
            this.SerialNumber = SerialNumber;
        }

        public Log(DutInfo DUT)
        {
            this.SerialNumber = DUT.SerialNumber;
            this.Position = DUT.Position;
        }

        public Log(DirectoryInfo Path)
        {
            this.Path = Path.FullName;
        }

        public Log(DutInfo DUT, DirectoryInfo Path)
        {
            this.Path = Path.FullName;
            this.SerialNumber = DUT.SerialNumber;
            this.Position = DUT.Position;
            swLog = Open();
        }

        void IDisposable.Dispose()
        {
            if (swLog != null)
            {
                swLog.Close();
                swLog.Dispose();
            }
            RenameLog();
        }

        ~Log()
        {
            try
            {
                if (swLog != null)
                {
                    swLog.Close();
                    swLog.Dispose();
                }
            }
            catch (Exception ex)
            { }
            RenameLog();
        }
        #endregion

        #region private member
        private string BuildFileName(int Position, string SerialNumber, string Path)
        {
            string strDut = "DUT";
            string strSerial = "_Serial_";
            StartTime = DateTime.Now;
            string strTime = StartTime.ToString(CommonTags.Common_LongDateTime);

            if (Position >= 1)
            {
                strDut = string.Format("{0}{1}", strDut, Position);
            }
            if (false == string.IsNullOrEmpty(SerialNumber))
            {
                strSerial= strSerial.Replace("Serial", SerialNumber);
            }
            if (true == System.IO.Directory.Exists(Path))
            {
                return System.IO.Path.Combine(Path, string.Format("{0}{1}{2}.log", strDut, strSerial, strTime));
            }
            else
            {
                return System.IO.Path.Combine(this.GetType().Assembly.Location, string.Format("{0}{1}{2}.log", strDut, strSerial, strTime));
            }
        }

        /// <summary>
        /// to create log file or open/append existed file. 
        /// </summary>
        /// <returns>StreamWriter instance of log</returns>
        private StreamWriter Open()
        {
            try
            {
                if (swLog == null)
                {
                    if (true == string.IsNullOrEmpty(FileName))//file name is not initialized yet
                    {
                        FileName = BuildFileName(this.Position, this.SerialNumber, this.Path);
                    }
                    else//not empty, file name initialized 
                    {//do nothing
                    }
                    return new StreamWriter(FileName, true);
                }
                else
                {
                    return swLog;
                }
            }
            catch(Exception ex)
            {
                swLog.Close();
                Thread.Sleep(1000);
                swLog = Open();
                AppendLine(string.Format("[Fatal]\t{0}", ex.Message));
                return swLog;
            }
        }

        /// <summary>
        /// This method writes unformatted text
        /// </summary>
        /// <param name="Text">log text without time stamp</param>
        private void AppendLine(DateTime TimeStamp, string Text)
        {
            if (swLog == null)
            {
                swLog = Open();
            }
            swLog.WriteLine("[{0}]:\t{1}", TimeStamp.ToString(CommonTags.Common_LongDateTime), Text);
            swLog.Flush();
        }

        /// <summary>
        /// This method writes formatted text
        /// </summary>
        /// <param name="Text">log text without time stamp</param>
        private void AppendLine(string Text)
        {
            if (swLog == null)
            {
                swLog = Open();
            }
            swLog.WriteLine("{0}", Text);
            swLog.Flush();
        }

        /// <summary>
        /// this method will parse the initial setting of severities to be logged to an array of int.
        /// </summary>
        /// <param name="LogSeverities">LogSeverities is the summary of: Unknown=1, Information=2,Warning=4,Important=8,Fator=16</param>
        /// <returns>Checked severity list</returns>
        private int[] InitCheckedSeverity(int LogSeverities)
        {
            sbyte severities = Convert.ToSByte(LogSeverities);
            List<int> listSeverities = new List<int>();

            try
            {
                if (0b_0000_0001 == (severities & 0b_0000_0001))
                {
                    listSeverities.Add(Convert.ToInt32(DebugSeverityTypes.Unknown));
                }
                if (0b_0000_0010 == (severities & 0b_0000_0010))
                {
                    listSeverities.Add(Convert.ToInt32(DebugSeverityTypes.Info));
                }
                if (0b_0000_0100 == (severities & 0b_0000_0100))
                {
                    listSeverities.Add(Convert.ToInt32(DebugSeverityTypes.Warning));
                }
                if (0b_0000_1000 == (severities & 0b_0000_1000))
                {
                    listSeverities.Add(Convert.ToInt32(DebugSeverityTypes.Important));
                }
                if (0b_0001_0000 == (severities & 0b_0001_0000))
                {
                    listSeverities.Add(Convert.ToInt32(DebugSeverityTypes.Fatal));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}->{1}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, ex.Message));
            }
            return listSeverities.ToArray();
        }

        /// <summary>
        /// This method will change file name if any info of position number, serial number
        /// and path is changed. The open/close status will not be changed.
        /// </summary>
        /// <returns></returns>
        private string RenameLog()
        {
            bool bReopen = false;

            try
            {
                //build new name according to latest info
                string strNewName = BuildFileName(this.Position, this.SerialNumber, this.Path);

                if (true == string.IsNullOrEmpty(FileName))
                {
                    return strNewName;
                }
                else
                {
                    if (true == strNewName.ToLower().Equals(FileName.ToLower()))
                    {//if file name is unchanged, do nothing and return old 
                        return FileName;
                    }
                    else
                    {
                        if (swLog != null)//file opened
                        {
                            bReopen = true;//file opened

                            //close before rename
                            swLog.Close();
                            swLog.Dispose();
                            swLog = null;
                        }
                        //rename
                        File.Move(FileName, strNewName);
                        FileName = strNewName; //update file name info

                        //reopen if it was opened before rename
                        if (true == bReopen)
                        {
                            swLog = Open();
                        }
                        return FileName;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}->{1}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, ex.Message));
            }
        }
        #endregion

        #region public member
        public void OnLogReceived(object sender, LogSendEventArgs e)
        {
            //log example:
            //[20200413_144559.234]:    [error] [session] [text]
            try
            {
                if (Array.IndexOf(CheckedSeverity, Convert.ToInt32(e.Severity)) == -1)
                {
                    //The debug msg is not required in initial settings.
                    return;
                }
                string strSeverity = string.Empty;
                switch (e.Severity)
                {
                    case DebugSeverityTypes.Unknown: strSeverity = "Unknown\t\t";
                        break;
                    case DebugSeverityTypes.Info: strSeverity = "Info\t\t";
                        break;
                    case DebugSeverityTypes.Warning: strSeverity = "Warn\t\t";
                        break;
                    case DebugSeverityTypes.Important: strSeverity = "Important\t";
                        break;
                    case DebugSeverityTypes.Fatal: strSeverity = "Fatal\t\t";
                        break;
                }

                string Text = string.Format("[{0}]:\t[{1}][{2}]\t\t{3}",
                                e.TimeStamp.ToString(CommonTags.Common_LongDateTime),
                                strSeverity,
                                e.SessionName,
                                e.Text);
                AppendLine(Text);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}->{1}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, ex.Message));
            }
        }

        public override bool IsInitialized { get; set; }
        public override void Initialize(Dictionary<string, object> Options)
        {
            base.Initialize(Options);
            try
            {
                if (this.ComponentOptions.ContainsKey(CommonTags.CoreData_Config_LogSeverity))
                {
                    //get severity to be logged
                    CheckedSeverity = InitCheckedSeverity(Convert.ToInt32(Options[CommonTags.CoreData_Config_LogSeverity]));

                    //Get destination to store log files.
                    string strPath = Convert.ToString(Options[CommonTags.CoreData_Config_LogPath]);
                    if (false == Directory.Exists(strPath))
                    {
                        Directory.CreateDirectory(strPath);
                    }
                    this.Path = strPath;

                    //
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}.-> {1}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, ex.Message));
            }
            this.IsInitialized = true;
        }

        public override void Reset()
        {
            string str = string.Format("");
            throw new System.NotImplementedException("To implemented once needed.");
        }

        public override void Send(string Command)
        {
            throw new System.NotImplementedException("To implemented once needed.");
        }
        public override string Receive()
        {
            throw new System.NotImplementedException("To implemented once needed.");
        }
        #endregion
    }
}
