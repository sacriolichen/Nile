////////////////////////////////////////////////////////////////////////////////
//
// Author: Chen, Changwei
//
//------------------------------------------------------------------------------
//
// interface, ISessionManagement.
//
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nile.Definition
{
    public interface ISessionManagement
    {
        /// <summary>
        /// Create an instance by given interface name and position number.
        /// </summary>
        /// <param name="SessionName">The name of interface. sunch as ITest</param>
        /// <param name="position">The position number which need the session. each session is occupied by each position</param>
        /// <returns>The initialized instance which could be convert to specified interface object</returns>
        object CreateSession(string SessionName, int Position = 1);

        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="SessionName">The name of session(interface name usually) without position number</param>
        /// <param name="FileName">The library file contains the implementation of the interface</param>
        /// <param name="ClassName">The class name which implemented the interface</param>
        /// <param name="Position">The position number</param>
        /// <returns></returns>
        object CreateSession(string SessionName, string FileName, string ClassName, int Position = 1);

        /// <summary>
        /// To get session (driver instance) from manager by session name and position number
        /// </summary>
        /// <param name="SessionName">The name of session(interface name usually) without position number</param>
        /// <param name="Position">The position number</param>
        /// <returns>The instance of driver</returns>
        object GetSessionByName(string SessionName, int Position);

        ///<Summary>
        ///remove session from session manager
        ///</Summary>
        void Remove(string SessionName, int Position);

        /// <summary>
        /// Check if the specified session exists.
        /// </summary>
        /// <param name="SessionName">The name of session(interface name usually) without position number</param>
        /// <param name="Position">The position number</param>
        /// <returns>true = exists, false = absent</returns>
        bool SessionExists(string SessionName, int Positions);

        /// <summary>
        /// To initialize an instance 
        /// </summary>
        /// <param name="SessionName">The name (usually, interface name) of session without position number</param>
        /// <param name="Position">Position number</param>
        void InitializeSession(string SessionName, int Positions);
    }

    /// <summary>
    /// to store the file name and class name which support an interface
    /// <para="File">File name without folder</para>
    /// <para="Class">Test method name</para>
    /// <para="Configration>Initial settings</para>
    /// </summary>
    public struct SessionInput
    {
        public string File { get; set; }
        public string Class { get; set; }
        public Dictionary<string, object> Configuration;

        public SessionInput(string FileName, string ClassName)
        {
            this.File = FileName;
            this.Class = ClassName;
            Configuration = new Dictionary<string, object>();
        }
    }
}

