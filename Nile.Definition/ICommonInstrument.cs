using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nile.Definition
{
    public interface ICommonInstrument
    {
        #region InstrumentIdentification
        /// <summary>
        /// This method returns a string containing the identification information 
        /// for the instrument.
        /// </summary>
        /// <returns>A string containing the identification information for the 
        /// instrument.</returns>
        /// <remarks>
        /// The implementation of this method is specific to the instrument.  
        /// The implementation should document the format of the string returned.
        /// </remarks>
        #endregion

        #region Initialized
        /// <summary>
        /// When implemented by a driver, this readonly property indicates whether the 
        /// <see cref="Initialize">Initialize</see> method has been called.
        /// </summary>
        bool Initialized { get; }
        #endregion

        #region ReadBufferSize
        /// <summary>
        /// This property controls the maximum size of the read buffer
        /// </summary>
        int ReadBufferSize { get; set; }
        #endregion

        #region InstrumentIdentification
        /// <summary>
        /// This method returns a string containing the identification information 
        /// for the instrument.
        /// </summary>
        /// <returns>A string containing the identification information for the 
        /// instrument.</returns>
        /// <remarks>
        /// The implementation of this method is specific to the instrument.  
        /// The implementation should document the format of the string returned.
        /// </remarks>
        string InstrumentIdentification();
        #endregion

        #region Initialize
        /// <summary>
        /// When implmented by a driver, this method performs all of the necessary 
        /// intialization for the user to be able to start using the instrument.
        /// </summary>
        /// <param name="logicalName">This string is the logical name for this driver instance.</param>
        /// <param name="options">This object array can be used to define instrument dependant 
        /// options.  The user should refer to the documentation for the specific 
        /// implementation for a description of what this parameter is expected to contain.
        /// The first element of the array is setting config file (json) </param>
        /// <remarks>
        /// The Driver should initilize the communications layer first using <paramref name="logicalName"/>.  
        /// </remarks>
        void Initialize(string logicalName, object[] options);
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
        /// When implemented by a driver, this method sends a text comman directly to 
        /// the instrument.
        /// </summary>
        /// <param name="send">This string is the text command to send to the 
        /// instrument.</param>
        /// <remarks>
        /// This method will send the command directly to the instrument without 
        /// modification.  This mehtod is provided mainly for debugging purposes. it 
        /// is considered bad practice to implement instrument functionality using this 
        /// method.
        /// </remarks>
        void Send(string send);
        #endregion

        #region Receive
        /// <summary>
        /// When implemented by a driver, this method triggers a read from the 
        /// instrument and returns the response.
        /// </summary>
        /// <returns>The string used to return the response.</returns>
        string Receive();
        #endregion

        #region SendReceive
        /// <summary>
        /// When implemented by an driver, this method will send a command to the 
        /// instrument and return the response.
        /// </summary>
        /// <param name="send">The string containing the command to send.</param>
        /// <returns>The string used to return the response.</returns>
        /// <seealso cref="Send"/>
        /// <seealso cref="Receive"/>
        string SendReceive(string send);
        #endregion
    }
}
