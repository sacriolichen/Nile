////////////////////////////////////////////////////////////////////////////////
//
// Author: Chen, Changwei
//
//------------------------------------------------------------------------------
//
// base class, TestClassBase.
//
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Nile.Definition;
using System.Collections;

namespace Nile.TestMethod
{
    //public class TestClassBase : ITestClass
    public class TestClassBase
    {
        protected int refID;
        protected Type instanceType;
        protected object testInstance;
        //public ArrayList Input { get; protected set; }

        public delegate void Send2LogEventHanler(object sender, Send2LogEventArgs send2LogEvArgs);
        public event Send2LogEventHanler eventSent2Log;

        protected ArrayList Output;
        protected Dictionary<string, object> InputDictionary;

        #region Core data
        private ISessionManagement pISessionManagement;
        protected IDataTable pIDataTable;
        private Hashtable CoreData;
        protected int Position;
        #endregion

        public TestClassBase()
        {
            Init();
        }

        public TestClassBase(Hashtable TestParams)
        {
            Init();

            //import core data
            CoreData = TestParams;

            try
            {
                pISessionManagement = (ISessionManagement)GetCoreInterface(CommonTags.CoreData_SessionManage);
                pIDataTable = (IDataTable)GetCoreInterface(CommonTags.CoreData_DataTable);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// seems useless
        /// </summary>
        /// <returns></returns>
        internal int CreateInstance()
        {
            try
            {
                this.testInstance = Activator.CreateInstance(this.instanceType);
            }
            catch (Exception exception)
            {
                //BridgeExceptionCatch.ReportException(new ResourceLoaderException(exception.InnerException.Message), this.instanceType.FullName);
                throw new Exception(exception.InnerException.Message + this.instanceType.FullName);
            }
            return this.refID;
        }

        //public virtual ArrayList Do(Dictionary<string, object> args)
        public virtual ArrayList Do(Dictionary<string, object> Input)
        {
            try
            {
  //              if (args != null | args.Count > 0)
                {
//                    Dictionary<string, object> Input = args;
                    //Dictionary<string, object> Input = (Dictionary<string, object>)args[0];
                }
                ArrayList alOutputValue;
                if (this.testInstance == null)
                {
                    this.CreateInstance();
                }
                Type type = this.testInstance.GetType();
                try
                {
                    object[] Args = new object[] { Input };
                    alOutputValue = (ArrayList)type.InvokeMember("Do", BindingFlags.InvokeMethod, null, this.testInstance, Args);
                    //alOutputValue = (ArrayList)type.InvokeMember("Do", BindingFlags.InvokeMethod, null, this.testInstance, args);
                }
                catch (MissingMethodException)
                {
                    throw new Exception("Measure method is missing. Explicit implementation of testmethod interface not allowed");
                }
                return alOutputValue;
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("Error: Failed at call TestClassBase.Do"));
            }
        }

        #region Send info to log file
        public void Send2Log(object sender, string Message)
        {
            Send2LogEventArgs e = new Send2LogEventArgs(Message, DateTime.Now);
            e.Message = Message;
            if (eventSent2Log != null)
            {
                eventSent2Log(sender, e);
            }
        }
        #endregion

        #region protected member
        /// <summary>
        /// To get expected data from core data
        /// </summary>
        /// <param name="SessionName">Expected session</param>
        /// <returns>Object of Expected session</returns>
        private object GetCoreInterface(string SessionName)
        {
            try
            {
                if (CoreData.ContainsKey(SessionName))
                {
                    if (null != CoreData[SessionName])
                    {
                        return CoreData[SessionName];
                    }
                    else
                    {
                        throw new Exception(string.Format("Get null for {0}.", SessionName));
                    }
                }
                else
                {
                    throw new Exception(string.Format("Can't get {0} from core.", SessionName));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}", ex.Message));
            }
        }

        /// <summary>
        /// To get driver from session manager
        /// </summary>
        /// <param name="SessionName">Expected session</param>
        /// <returns>Object</returns>
        protected object GetInterface(string SessionName)
        {
            string strName = string.Format("{0}_{1}", SessionName, Position);

            try
            {
                object objSession = null;
                if (true == pISessionManagement.SessionExists(SessionName, Position))
                {
                    objSession = pISessionManagement.GetSessionByName(SessionName, Position);
                }
                else
                {

                    objSession = pISessionManagement.CreateSession(SessionName, Position);
                    //throw new Exception(string.Format("Can't find {0} in SessionManager.", strName));
                }
                ICommonComponent pICC = objSession as ICommonComponent;
                if(false == pICC.IsInitialized)
                {
                    pISessionManagement.InitializeSession(SessionName, Position);
                }

                return objSession;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}->{1}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, ex.Message));
            }
        }

        /// <summary>
        /// To get driver from session manager with specified position number.
        /// Not imeplemented yet.
        /// </summary>
        /// <param name="SessionName">Expected session</param>
        /// <returns>Object</returns>
        protected object GetInterface(string SessionName, int Position)
        {
            throw new NotImplementedException();
            return null;
        }

        /// <summary>
        /// A common way in base class to save output 
        /// </summary>
        /// <typeparam name="T">Int, string, bool, Double</typeparam>
        /// <param name="MeasValue">The output value</param>
        /// <returns></returns>
        protected int SaveValue<T>(T MeasValue)
        {
            try
            {
                Output.Add(MeasValue);
                Console.WriteLine("Saved: {0}", MeasValue);
            }
            catch
            {
                return 1;//non-zero return value means anything unexpected happens. To extend values for different cases.
            }
            return 0;//default, means no error
        }

        /// <summary>
        /// To save the values in expected order
        /// </summary>
        /// <typeparam name="T">Int, string, bool, Double</typeparam>
        /// <param name="MeasValue">The ouput value</param>
        /// <param name="Index">0 based index</param>
        /// <returns></returns>
        protected int SaveValue<T>(T MeasValue, int Index)
        {
            try
            {
                int ListLengthNow = Output.Count;
                if (Index < ListLengthNow) //the expected position already saved a value
                {
                    Output[Index] = MeasValue;
                }
                else if (Index == ListLengthNow)//the next
                {
                    Output.Add(MeasValue);
                }
                else if (Index > ListLengthNow) //skip several position
                {
                    do
                    {
                        Output.Add("NA");
                    } while (Index < Output.Count);
                    Output.Add(MeasValue);
                }
            }
            catch
            {
                return 1;//non-zero return value means anything unexpected happens. To extend values for different cases.
            }
            return 0;//default, means no error
        }


        /// <summary>
        /// To get input this test needs
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Name">Session name, like SLEEP_TIME</param>
        /// <param name="Mandatory">The input is mandatory (true) or not (false).</param>
        /// <param name="Input">expected input</param>
        /// <param name="DefaultValue">The fault value in case of absent in input list and mandatory.</param>
        protected void GetInput<T>(string Name, bool Mandatory, out T Input, T DefaultValue)
        {
            try
            {
                if (true == InputDictionary.ContainsKey(Name))
                {
                    Input = (T)InputDictionary[Name];
                }
                else if (true == Mandatory)
                {
                    throw new Exception(string.Format("The option {0} is missing in test plan and it's mandatory", Name));
                }
                else
                {
                    Input = DefaultValue;//default
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
        }
        /*
        protected bool GetInput(Dictionary<string, object> argsInput, string InputName, out object InputValue)
        {
            InputValue = null;
            if (true == argsInput.ContainsKey(InputName))
            {
                InputValue = argsInput[InputName];
                return true;
            }
            else
            {
                return false;
            }
        }

        protected bool GetInput(Dictionary<string, object> argsInput, string InputName, out int InputValue)
        {
            InputValue = int.MinValue;
            if (true == argsInput.ContainsKey(InputName))
            {
                InputValue = Convert.ToInt32(argsInput[InputName]);
                return true;
            }
            else
            {
                return false;
            }
        }

        protected bool GetInput(Dictionary<string, object> argsInput, string InputName, out double InputValue)
        {
            InputValue = double.MinValue ;
            if (true == argsInput.ContainsKey(InputName))
            {
                InputValue = Convert.ToDouble(argsInput[InputName]);
                return true;
            }
            else
            {
                return false;
            }
        }
        protected bool GetInput(Dictionary<string, object> argsInput, string InputName, out string InputValue)
        {
            InputValue = null;
            if (true == argsInput.ContainsKey(InputName))
            {
                InputValue = Convert.ToString(argsInput[InputName]);
                return true;
            }
            else
            {
                return false;
            }
        }
        protected bool GetInput(Dictionary<string, object> argsInput, string InputName, out bool InputValue)
        {
            InputValue = false;
            if (true == argsInput.ContainsKey(InputName))
            {
                InputValue = Convert.ToBoolean(argsInput[InputName]);
                return true;
            }
            else
            {
                return false;
            }
        }
        protected void GetInput(string SettingFile, string Module, string Method, string InputName, ref int Input)
        {
            if (false == System.IO.File.Exists(SettingFile))
            {
                throw new Exception(string.Format("[TestClassBase][LoadSetting]:{0} does not exist", SettingFile));
            }
            Utilties.GetInput(SettingFile, Module, Method, InputName, ref Input);
        }

        protected void GetInput(string SettingFile, string Module, string Method, string InputName, ref double Input)
        {
            if (false == System.IO.File.Exists(SettingFile))
            {
                throw new Exception(string.Format("[TestClassBase][LoadSetting]:{0} does not exist", SettingFile));
            }
            Utilties.GetInput(SettingFile, Module, Method, InputName, ref Input);
        }

        protected void GetInput(string SettingFile, string Module, string Method, string InputName, ref string Input)
        {
            if (false == System.IO.File.Exists(SettingFile))
            {
                throw new Exception(string.Format("[TestClassBase][LoadSetting]:{0} does not exist", SettingFile));
            }
            Utilties.GetInput(SettingFile, Module, Method, InputName, ref Input);
        }

        protected void GetInput(string SettingFile, string Module, string Method, string InputName, ref bool Input)
        {
            if (false == System.IO.File.Exists(SettingFile))
            {
                throw new Exception(string.Format("[TestClassBase][LoadSetting]:{0} does not exist", SettingFile));
            }
            Utilties.GetInput(SettingFile, Module, Method, InputName, ref Input);
        }
        */
        #endregion

        #region private member
        /// <summary>
        /// Initialize global variant
        /// </summary>
        private void Init()
        {
            InputDictionary = new Dictionary<string, object>();
            Output = new ArrayList();

            Position = 1;
        }

        #endregion
    }
}
