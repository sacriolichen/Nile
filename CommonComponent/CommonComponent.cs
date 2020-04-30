////////////////////////////////////////////////////////////////////////////////
//
// Author: Chen, Changwei
//
//------------------------------------------------------------------------------
//
// Base class, CommonComponent.
//
//------------------------------------------------------------------------------
using Nile.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nile.Component
{
    //delegate for log
    public delegate void delegateLogSend(Object sender, LogSendEventArgs e);
    /// <summary>
    /// Base class for all instrument drivers and external library wrapper
    /// </summary>
    public class ComponentBase : ICommonComponent
    {
        delegate bool Function(object objNull);
        Function ObjectIsNull = delegate (object objNull) { return objNull == null; };

        #region public members
        protected delegateLogSend dlgtLogSend;
        protected string InterfaceName;
        protected int Position;
        public ILog ILogSession { get; set; }

        public ComponentBase()
        {
            Position = -1;//default value. it should be greater than 0
        }

        /// <summary>
        /// This method will collect data and call the trigger of event
        /// </summary>
        /// <param name="Severity">Severity of message</param>
        /// <param name="TextFormat">text format</param>
        /// <param name="param">variable value in text format</param>
        protected void Logging(DebugSeverityTypes Severity, string TextFormat, params object[] param)
        {
            if (dlgtLogSend != null && ILogSession != null)
            {
                string strText = string.Format(TextFormat, param);
                LogSendEventArgs e = new LogSendEventArgs(strText,
                                                    DateTime.Now,
                                                    Severity,
                                                    this.ToString(true));
                dlgtLogSend(this, e);
            }
        }

        protected Dictionary<string, object> ComponentOptions;
        //public string SessionName { get; set; }
        /// <summary>
        /// To get configuration for driver. Each driver use this method to get initial setting.
        /// </summary>
        /// <param name="Name">Session name, like ITest</param>
        /// <param name="Mandatory">The input is mandatory (true) or not (false).</param>
        /// <param name="DefaultValue">The fault value in case of absent in input list and mandatory.</param>
        /// <returns>The data as type object for specified Name</returns>
        protected object GetConfig(string Name, bool Mandatory, object DefaultValue)
        {
            try
            {
                if (true == ComponentOptions.ContainsKey(Name))
                {
                    return ComponentOptions[Name];
                }
                else if (true == Mandatory)
               {
                    throw new Exception(string.Format("The option {0} is missing in driver settings and it's mandatory", Name));
                }
                else
                {
                    return DefaultValue;//default
                }
            }
            catch (System.InvalidCastException ex)
            {
                throw new Exception(string.Format("Wrong data type in driver setting. -> {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}.-> {1}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, ex.Message));
            }
            return null;
        }
        #endregion

        #region private member
        private void ParseSessionName(string SessionName)
        {
            Match match = Regex.Match(SessionName, "(I[A-Za-z0-9]+)_([0-9]+)");

            if (match.Success)
            {
                this.Position = Convert.ToInt32(match.Groups[2].Value);
                this.InterfaceName = Convert.ToString(match.Groups[1].Value);
            }
            else
            {
                throw new Exception(string.Format("{0} is not a valid Session Name", SessionName));
            }
        }
        #endregion

        #region interface member
        /// <summary>
        /// The indicator of the session (component) is initialized or not
        /// </summary>
        public virtual bool IsInitialized { get; set; }

        public void SetSessionName(string SessionName)
        {
            ParseSessionName(SessionName);
        }
        /// <summary>
        /// intialization for the user to be able to start using the instrument.
        /// It's common part. And also it's could be override by derived class. 
        /// </summary>
        /// <param name="options">This object array can be used to define instrument dependant
        /// options.  The user should refer to the documentation for the specific 
        /// implementation for a description of what this parameter is expected to contain.</param>
        /// <remarks> It's better to be called before overrided method.</remarks>
        public virtual void Initialize(Dictionary<string, object> Options)
        {
            if (IsInitialized == true)
            {
                return;
            }
            ComponentOptions = Options;


            //string position = SessionName.Substring(SessionName.LastIndexOf('_') + 1);
            dlgtLogSend += new delegateLogSend(ILogSession.OnLogReceived);
        }

        /// <summary>
        /// This method will return the name of the instance.
        /// </summary>
        /// <param name="IncludingPosition">If the position number is required</param>
        /// <returns>return interface name when input false. Return interfance name with position number when input true</returns>
        public string ToString(bool IncludingPosition)
        {
            //to check if position number and interfacename are given.
            if (this.Position <= 0 || string.IsNullOrEmpty(this.InterfaceName))
            {
                throw new Exception("Position number or InterfaceName of the instance is not initialized.");
            }

            if (IncludingPosition)
            {
                return string.Format("{0}_{1}", this.InterfaceName, this.Position);
            }
            else
            {
                return this.InterfaceName;
            }
        }

        public virtual void Reset()
        {
            throw new NotImplementedException();
        }
        public virtual void Send(string data)
        {
            throw new NotImplementedException();
        }

        public virtual string Receive()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
