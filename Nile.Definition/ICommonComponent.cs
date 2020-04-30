////////////////////////////////////////////////////////////////////////////////
//
// Author: Chen, Changwei
//
//------------------------------------------------------------------------------
//
// interface, ICommonComponent.
//
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nile.Definition
{
    #region OnOffTypes
    /// <summary>
    /// This enumeration is used to defined the On or Off state of a setting.
    /// </summary>
    /// <remarks>This enumeration is used in place of binary or boolean to remove confusion associated
    /// with active high/Low signals.</remarks>
    public enum OnOffTypes
    {
        /// <summary>No state has been defined.</summary>
        Undefined = -1,
        /// <summary>The setting should be in the On state</summary>
        On = 0,
        /// <summary>The setting should be in the Off state.</summary>
        Off = 1
    };
    #endregion OnOffTypes

    /// <summary>
    /// Base interface for all drivers.
    /// </summary>
    public interface ICommonComponent
    {
        //--- Properties------------------------------------------------------------------------
        /// <summary>
        /// this string indicator itself by interfacename_positionnumber,
        /// </summary>
        //string SessionName { get; set; }

        #region IsInitialized
        /// <summary>
        /// When implemented by a driver, this readonly property indicates whether the 
        /// <see cref="Initialize">Initialize</see> method has been called.
        /// </summary>
        bool IsInitialized { get; }

        ILog ILogSession { set; }
        #endregion

        //--- Methods --------------------------------------------------------------------------
        /// <summary>
        /// This method will return SessionName(w/ position number) or interfacei name (w/o position number)
        /// </summary>
        /// <param name="IncludingPosition">Including Position number or not</param>
        /// <returns>SessionName or interface name</returns>
        string ToString(bool IncludingPosition);

        /// <summary>
        /// This method will parse session name to interface name and position name
        /// </summary>
        /// <param name="SessionName"></param>
        void SetSessionName(string SessionName);
        #region Initialize
        /// <summary>
        /// When implmented by a driver, this method performs all of the necessary 
        /// intialization for the user to be able to start using the instrument.
        /// </summary>
        /// <param name="options">This object array can be used to define instrument dependant 
        /// options.  The user should refer to the documentation for the specific 
        /// implementation for a description of what this parameter is expected to contain.</param>
        void Initialize(Dictionary<string, object> Options);
        #endregion

        #region Reset
        /// <summary>
        /// When implemented by a driver, this method resets the instrument and driver 
        /// such that they are in a known default state.
        /// </summary>
        void Reset();
        #endregion

        #region Send
        /// <summary>
        /// Maybe useful
        /// </summary>
        /// <param name="data"></param>
        void Send(string data);
        #endregion

        #region Receive
        /// <summary>
        /// Maybe useful
        /// </summary>
        string Receive();
        #endregion
    }
}
