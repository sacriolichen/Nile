////////////////////////////////////////////////////////////////////////////////
//
// Author: Chen, Changwei
//
//------------------------------------------------------------------------------
//
// class, SessionManager.
//
//------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nile.Definition;

namespace Nile.SessionManagement
{
    public class SessionManager : Hashtable, ISessionManagement
    {
        /// <summary>
        /// The key is the name of interface, the value contains the filename and class name which implemented the interface
        /// </summary>
        private Dictionary<string, SessionInput> SessionMapping;
        private string SessionPath;//Session mapping file record the relative path of drivers.
        public SessionManager()
        {
            SessionMapping = new Dictionary<string, SessionInput>();
        }

        /// <summary>
        /// Initial an instance from a json file of session mapping
        /// </summary>
        /// <param name="SessionMapping"></param>
        public SessionManager(string SessionMappingFile)
        {
            try
            {
                SessionMapping = new Dictionary<string, SessionInput>();//<sessionname_position, sessioninput> The key is the session name with position number

                if (false == File.Exists(SessionMappingFile))
                {
                    throw new Exception(string.Format("Does session mapping file exist?"));
                }
                FileInfo fiJson = new FileInfo(SessionMappingFile);

                if (false == fiJson.Extension.ToLower().EndsWith("json"))
                {
                    throw new Exception(string.Format("{0} is not supported session mapping file.", fiJson.Extension));
                }

                StreamReader file = File.OpenText(SessionMappingFile);
                JsonTextReader reader = new JsonTextReader(file);
                JObject joFile = (JObject)JToken.ReadFrom(reader);

                /// TODO: Add the string to common tags
                if (true == joFile.ContainsKey("SessionMapping"))
                {
                    JObject joMapping = (JObject)joFile["SessionMapping"];
                    this.SessionMapping = LoadSessionMapping(joMapping);
                    SessionPath = string.Format("{0}\\", fiJson.DirectoryName);
                }
                else
                {
                    throw new Exception(string.Format("The file does not contain the section {0} for session mapping", "SessionMapping"));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("SessionManager(string). {0}", ex.InnerException.Message));
            }
        }

        /// <summary>
        /// Initial an instance from a json section of session mapping
        /// </summary>
        /// <param name="SessionMappingJSonSection">The json section contains session mapping</param>
        public SessionManager(JObject SessionMappingJSonSection)
        {
            try
            {
                this.SessionMapping = LoadSessionMapping(SessionMappingJSonSection);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("SessionManager(string)-> {0}", ex.InnerException.Message));
            }
        }

        #region interface member
        /// <summary>
        /// Check if the specified session exists.
        /// </summary>
        /// <param name="SessionName">The name of session(interface name usually) without position number</param>
        /// <param name="Position">The position number</param>
        /// <returns>true = exists, false = absent</returns>
        public bool SessionExists(string SessionName, int Positions)
        {
            string strName = string.Format("{0}_{1}", SessionName, Positions);
            return SessionExists(strName);
        }

        /// <summary>
        /// Create an instance by given interface name and position number.
        /// </summary>
        /// <param name="SessionName">The name of interface. sunch as ITest</param>
        /// <param name="position">The position number which need the session. each session is occupied by each position</param>
        /// <returns>The initialized instance which could be convert to specified interface object</returns>
        public object CreateSession(string SessionName, int Position = 1)
        {
            string strFile = string.Empty;
            string strClass = string.Empty;
            string strName = string.Format("{0}_{1}", SessionName, Position);

            try
            {
                if (false == this.SessionMapping.ContainsKey(strName))
                {
                    throw new Exception(string.Format("Can't get file name and class name for the session{0}", strName));
                }
                else
                {
                    //get setting
                    strFile = string.Format("{0}{1}", SessionPath, SessionMapping[strName].File);
                    strClass = SessionMapping[strName].Class;
                    Dictionary<string, object> dictConfig = SessionMapping[strName].Configuration;
                    //get instance
                    object objSession = CreateSession(SessionName, strFile, strClass, Position);

                    return objSession;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("CreateSession(string, int)->{0}", ex.InnerException.Message));
            }
            return null;
        }

        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="SessionName">The name of session(interface name usually) without position number</param>
        /// <param name="FileName">The library file contains the implementation of the interface</param>
        /// <param name="ClassName">The class name which implemented the interface</param>
        /// <param name="Position">The position number</param>
        /// <returns></returns>
        public object CreateSession(string SessionName, string FileName, string ClassName, int Position = 1)//, string MethodName, int input)
        {
            object objTest = null;

            try
            {
                //get instance
                objTest = NewInstance(SessionName, FileName, ClassName, Position);
                this.Add(string.Format("{0}_{1}", SessionName, Position), objTest);
                //set the session name to itself
                ICommonComponent pICC = objTest as ICommonComponent;
                pICC.SessionName = string.Format("{0}_{1}", SessionName, Position);
                //set logger to session
//                if (false == pICC.SessionName.ToLower().StartsWith("ilog"))
                {//if not ilog itself
                    string strLogSessionName = string.Format("ILog_{0}", Position);
                    foreach (DictionaryEntry de in this)
                    {
                        if (true == strLogSessionName.Equals(Convert.ToString(de.Key)))
                        {
                            pICC.ILogSession = de.Value as ILog;
                            break;
                        }
                    }
                }

                return objTest;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("CreateSession(string, string, string, int)->{0}", ex.InnerException.Message));
            }
            return null;
        }

        /// <summary>
        /// To get session (driver instance) from manager by session name and position number
        /// </summary>
        /// <param name="SessionName">The name of session(interface name usually) without position number</param>
        /// <param name="Position">The position number</param>
        /// <returns>The instance of driver</returns>
        public object GetSessionByName(string SessionName, int Position)
        {
            try
            {
                if (true == this.SessionExists(SessionName, Position))
                {
                    string strName = string.Format("{0}_{1}", SessionName, Position);

                    ICommonComponent pICC = this[strName] as ICommonComponent;
                    if (pICC.IsInitialized == false)
                    {
                        this.InitializeSession(SessionName, Position);
                    }
                    return this[strName];
                }
                else
                {
                    return CreateSession(SessionName, Position);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("GetSessionByName. -> {0}", ex.InnerException.Message));
            }
            return null;
        }

        ///<Summary>
        ///remove session from session manager
        ///</Summary>
        public void Remove(string SessionName, int Position)
        {
            string strName = string.Format("{0}_{1}", SessionName, Position);

            try
            {
                if (this.ContainsKey(strName))
                {
                    ///TODO:
                    ///call driver Dispose or Close or destructor function here 
                    if (this[strName] is IDisposable)
                        ((IDisposable)this[strName]).Dispose();

                    this.Remove(strName);
                }
                else
                {
                    ///TODO:
                    ///add this info to log
                    //do nothing
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Remove(string, int). -> {0}", ex.InnerException.Message));
            }
        }

        /// <summary>
        /// To initialize an instance 
        /// </summary>
        /// <param name="SessionName">The name (usually, interface name) of session without position number</param>
        /// <param name="Position">Position number</param>
        public void InitializeSession(string SessionName, int Position)
        {
            try
            {
                if (false == SessionExists(SessionName, Position))
                {
                    throw new Exception(string.Format("Can't get instance of the session {0}", string.Format("{0}_{1}", SessionName, Position)));
                }
                else
                {
                    //build name to search configs in session mapping
                    string strName = string.Format("{0}_{1}", SessionName, Position);
                    //get configuratoiin
                    Dictionary<string, object> dictSessionConfig = this.SessionMapping[strName].Configuration;

                    //get instance and call Initialize
                    ICommonComponent pICC = this[strName] as ICommonComponent;
                    pICC.Initialize(dictSessionConfig);
                    return;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}. -> {1}",
                                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName,
                                    ex.Message));
            }
        }

        public bool AddSession(string SessionName, object Session)
        {
            try
            {
                if (this.ContainsKey(SessionName) || Session == null)
                {//name exists or session is null
                    return false;
                }
                this.Add(SessionName, Session);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}. -> {1}",
                                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName,
                                    ex.Message));
            }
        }
        #endregion

        #region private member
        /// <summary>
        /// Implementation of load session mapping from json
        /// </summary>
        /// <param name="SessionMappingJSonSection">The json section contains session mapping</param>
        /// <returns></returns>
        private Dictionary<string, SessionInput> LoadSessionMapping(JObject SessionMappingJSonSection)
        {
            try
            {
                Dictionary<string, SessionInput> dictMapping = new Dictionary<string, SessionInput>();//The key is the session name with position number
                dictMapping = JsonConvert.DeserializeObject<Dictionary<string, SessionInput>>(SessionMappingJSonSection.ToString());

                return dictMapping;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("LoadSessionMapping-> {0}", ex.InnerException.Message));
            }
        }

        /// <summary>
        /// To call driver construction
        /// </summary>
        /// <param name="SessionName">Session name (interface name uasually ) without position number</param>
        /// <param name="FileName">The library file contains the implementation of the interface</param>
        /// <param name="ClassName">The class name which implemented the interface</param>
        /// <param name="Position">The position number</param>
        /// <returns></returns>
        private object NewInstance(string SessionName, string FileName, string ClassName, int Position)
        {
            Assembly asm = null;
            Type[] typeArray = null;
            Type type = null;
            object objTest = null;
            bool bFoundInterface = false;

            try
            {
                asm = Assembly.LoadFrom(FileName);
                typeArray = asm.GetExportedTypes();
                foreach (Type a in typeArray)
                {
                    if (true == a.ToString().EndsWith(ClassName))
                    {
                        type = a;
                        break;
                    }
                }
                objTest = asm.CreateInstance(type.ToString());
                
                /*
                ICommonComponent ic = objTest as ICommonComponent;

                #region Temp code to initial driver option for debugging
                Dictionary<string, object> dictDriverOptions = new Dictionary<string, object>();
                dictDriverOptions.Add("Size", 14);
                dictDriverOptions.Add("Length", -1);
                dictDriverOptions.Add("Area", 2);
                #endregion

                //ic.Initialize(dictDriverOptions);
                ic.Initialize(dictDriverOptions);
                */
                var interfaces = objTest.GetType().GetInterfaces();
                for (int i = 0; i < interfaces.Count(); i++)
                {
                    if (true == interfaces[i].Name.Equals(SessionName))
                    {
                        bFoundInterface = true;
                        break;
                    }
                }

                if (true == bFoundInterface)
                {
                    return objTest;
                }
                else
                {
                    throw new Exception(string.Format("Can't found the implemented class for interface {0} from file {1}", SessionName, FileName));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("NewInstance. -> {0}", ex.InnerException.Message));
            }
        }

        /// <summary>
        /// Check if the specified session exists.
        /// </summary>
        /// <param name="SessionName">The name of session(interface name usually) with position number</param>
        /// <returns>true = exists, false = absent</returns>
        private bool SessionExists(string SessionName)
        {
            return this.ContainsKey(SessionName);
        }
        #endregion
    }
}
