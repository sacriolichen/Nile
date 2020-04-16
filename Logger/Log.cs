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
using System.Threading.Tasks;
using Nile.Definition;

namespace Nile.Logger
{
    public class Log
    {
        string SerialNumber { get; set; }
        string FileName { get; set; }
        int Position { get; set; }
        DateTime StartTime { get; set; }
        string Path { get; set; }
        private StreamWriter swLog = null;
        #region Event and delegate

        #endregion

        void Write()
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(path);
            int counter = 0;
            while (something_is_happening && my_flag_is_true)
            {
                file.WriteLine(some_text_goes_Inside);
                counter++;
                if (counter < 200) continue;
                file.Flush();
                counter = 0;
            }
            file.Close();
        }

        #region constructors
        public Log()
        {
            FileName = BuildFileName(0, string.Empty, string.Empty);
        }

        public Log(string SerialNumber)
        {
            this.SerialNumber = SerialNumber;
            FileName = BuildFileName(0, this.SerialNumber, string.Empty);
        }

        public Log(DutInfo DUT)
        {
            this.SerialNumber = DUT.SerialNumber;
            this.Position = DUT.Position;
            FileName = BuildFileName(this.Position, this.SerialNumber, string.Empty);
        }

        public Log(DirectoryInfo Path)
        {
            this.Path = Path.FullName;
            FileName = BuildFileName(0, string.Empty, this.Path);
        }

        public Log(DutInfo DUT, DirectoryInfo Path)
        {
            this.Path = Path.FullName;
            this.SerialNumber = DUT.SerialNumber;
            this.Position = DUT.Position;
            FileName = BuildFileName(this.Position, this.SerialNumber, this.Path);
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
                        FileName = BuildFileName(0, string.Empty, string.Empty);
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
                System.Thread.Sleep(1000);
                swLog = Open();
                AppendLine(string.Format("{0}\tError\t{1}", DateTime.Now.ToString(CommonTags.Common_LongDateTime), ex.Message));
                return swLog;
            }
        }

        private void AppendLine(string Text)
        {

        }
        #endregion

        #region public member

        #endregion
    }
}
