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
        #region public members
        protected delegateLogSend dlgtLogSend;
        public ILog ILogSession { get; set; }

        public ComponentBase()
        {
        }

        /// <summary>
        /// This method will collect data and call the trigger of event
        /// </summary>
        /// <param name="Severity">Severity of message</param>
        /// <param name="TextFormat">text format</param>
        /// <param name="param">variant value in text format</param>
        protected void Logging(DebugSeverityTypes Severity, string TextFormat, params object[] param)
        {
            if (dlgtLogSend != null && ILogSession != null)
            {
                string strText = string.Format(TextFormat, param);
                LogSendEventArgs e = new LogSendEventArgs(strText,
                                                    DateTime.Now,
                                                    Severity,
                                                    this.SessionName);
                dlgtLogSend(this, e);
            }
        }

        protected Dictionary<string, object> ComponentOptions;
        public string SessionName { get; set; }
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
        #endregion

        #region interface member
        /// <summary>
        /// The indicator of the session (component) is initialized or not
        /// </summary>
        public virtual bool IsInitialized { get; set; }

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

            string position = SessionName.Substring(SessionName.LastIndexOf('_') + 1);
            dlgtLogSend += new delegateLogSend(ILogSession.OnLogReceived);
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
