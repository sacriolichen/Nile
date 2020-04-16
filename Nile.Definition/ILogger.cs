////////////////////////////////////////////////////////////////////////////////
//
// Author: Chen, Changwei
//
//------------------------------------------------------------------------------
//
// interface, ILog.
//
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nile.Definition
{
    #region LogSendEventArgs
    /// <summary>
    /// This class is used to encapsulate the data passed when logging a send event.
    /// </summary>
    /// <remarks>
    /// This class is used to pass required information to the Everest communications logger that
    /// is integrated in Session Factory.  The user creates an instance of the class and passes it
    /// as an argument to the <see cref="ILogger.ILog_Send"/> event. All fields are mandatory.
    /// </remarks>
    public class LogSendEventArgs : EventArgs
    {
        #region Data
        /// <summary>
        /// This is the data that was sent, in Byte format.
        /// </summary>
        public string Text;
        #endregion

        #region TimeStamp
        /// <summary>
        /// This is the date and time that the data was sent.
        /// </summary>
        public DateTime TimeStamp;
        #endregion

        #region SessionName
        /// <summary>
        /// This is the name of the session that sent the data.
        /// </summary>
        public string SessionName;
        #endregion

        /*
        #region ResourceDescriptor
        /// <summary>
        /// This is the resource descriptor describing the address of the device to which
        /// the data was sent.
        /// </summary>
        public string ResourceDescriptor;
        #endregion
        */
        #region Constructor
        /// <summary>
        /// The constructor for the class.
        /// </summary>
        /// <param name="data">The data to be logged.</param>
        /// <param name="timeStamp">The time at which the data was sent to the device.</param>
        /// <param name="sessionName">The name of the session that sent the data.</param>
        /// <param name="resourceDescriptor">The address of the device to which the data was sent.</param>
        public LogSendEventArgs(string Text, DateTime TimeStamp, string SessionName)//, string resourceDescriptor)
        {
            this.Text = Text;
            this.TimeStamp = TimeStamp;
            this.SessionName = SessionName;
            //this.ResourceDescriptor = resourceDescriptor;
        }
        #endregion
    }
    #endregion
}
