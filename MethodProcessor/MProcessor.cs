////////////////////////////////////////////////////////////////////////////////
//
// Author: Chen, Changwei
//
//------------------------------------------------------------------------------
//
// Class of Processor.
//
//------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Nile.Definition;
using Nile.Logger;
using Nile.TestSequence;
using ResultReport;

namespace Nile
{
    public delegate void delegateTestItemStart(Object sender, ItemStartEventArgs e);
    public delegate void delegateTestItemEnd(Object sender, ItemEndEventArgs e);
    public class Processor
    {
        public TestPlan CurrentPlan;
        public DutInfo DUTInfo;
        public string ExecutionID = string.Empty;
        private int First = 0;
        private Hashtable hashCoreParams;
        public event delegateTestItemStart dlgtStart;
        public event delegateTestItemEnd dlgtEnd;
        public Processor(TestPlan TP, //test plan
                        DutInfo Dut, // Info of device under test
                        Hashtable CoreParams)// core data
        {
            CurrentPlan = TP;
            DUTInfo = Dut;
            hashCoreParams = CoreParams;
            ExecutionID = Guid.NewGuid().ToString();

            //TODO: To define a upper level controller to control a test
            //below three lines could be defined or declared out of this class.
            ReportController rcReportor = new ReportController(ExecutionID);
            this.dlgtStart += new delegateTestItemStart(rcReportor.ItemStarted);
            this.dlgtEnd += new delegateTestItemEnd(rcReportor.ItemEnded);
        }

        public void PlanExecution()
        {
            List<TestItem> listSequence = CurrentPlan.Sequence;
            Dictionary<string, string> dictProperties = CurrentPlan.Properties;
            ISessionManagement pISM = null;
            try
            {
                #region relative set for the thread
                //inital log instance by sessionmanager
                if (hashCoreParams.ContainsKey(CommonTags.CoreData_SessionManage))
                {
                    pISM = (ISessionManagement)hashCoreParams[CommonTags.CoreData_SessionManage];
                }
                Log logger = new Log(DUTInfo);
                string strSessionName = string.Format("ILog_{0}", DUTInfo.Position);
                object objLog = pISM.CreateSession("ILog", DUTInfo.Position); //create session and add it to sessionmanager 
                pISM.InitializeSession("ILog", DUTInfo.Position);


                #endregion


                foreach (TestItem tirItem in listSequence)
                {
                    //Timestampe of start
                    TimeSpan tsSpan;
                    //get info of current test

                    //call delegation for showing the next test
                    //TODO: implement delegations

                    ArrayList alOutputValue = RunningItem(tirItem);

                    //restore current indicator

                    //call delegation for reporting and showing test result
                    //TODO: implement delegations
                }
            }
            catch (Exception ex)
            {
                //TODO: Implement the exception handling
            }
        }

        public ArrayList RunningItem(TestItem Item)
        {
            ArrayList alOutputValues = new ArrayList();
            Assembly asm = null;
            Type[] typeArray = null;
            Type type = null;
            object[] argsInput = new object[1];
            object objTest = null;
            MethodInfo[] miArray = null;
            MethodInfo miDo = null;

            try
            {
                //dlgtStart(this, new ItemStartEventArgs(DUTInfo, ItemInfo, DateTime.Now, this.ExecutionID));
                OnItemStart(DUTInfo, Item, this.ExecutionID, DateTime.Now);

                try
                {
                    asm = Assembly.LoadFrom(Item.ItemInfo.FileName);
                    typeArray = asm.GetExportedTypes();
                    foreach (Type a in typeArray)
                    {
                        if (true == a.ToString().EndsWith(Item.ItemInfo.Method))
                        {
                            //type = asm.GetType(a.ToString());
                            type = a;
                            break;
                        }
                    }
                    if (type == null)
                    {
                        throw new Exception(string.Format("Can't load method {0} in file {1}", Item.ItemInfo.Method, Item.ItemInfo.FileName));
                    }

                    //create an instance of class of the test method
                    //TODO: get initialized instrument handle
                    //objTest = Activator.CreateInstance(type, handleInstruments);
                    //objTest = asm.CreateInstance(type.ToString());
                    objTest = Activator.CreateInstance(type, new object[] { hashCoreParams });

                    //Get field info of the class
                    FieldInfo[] myfields = type.GetFields();

                    //read the function list of the method and check, initialize
                    miArray = type.GetMethods();
                    foreach (MethodInfo mi in miArray)
                    {
                        if (true == mi.Name.Equals("Do"))
                        {
                            miDo = mi;
                            //miDo = type.GetMethod("Do");
                            break;
                        }
                    }
                    if (miDo == null)
                    {
                        if (type == null)
                        {
                            throw new Exception(string.Format("Can't load the measurement function from specified method {0} in file {1}", Item.ItemInfo.Method, Item.ItemInfo.FileName));
                        }
                    }

                    //initial args with dictionary of all input
                    //argsInput[0] = Item.Input;
                    //Dictionary<string, object>[] Items = new Dictionary<string, object>[1] { Item.Input };
                    //Call the test
                    alOutputValues = (ArrayList)miDo.Invoke(objTest, new object[] { Item.Input });
                    OnItemEnd(DUTInfo, Item, alOutputValues, this.ExecutionID, DateTime.Now);
                }
                catch(Exception ex)
                {
                    ///TODO:
                    ///implement error handling later
                    Console.WriteLine(ex.Message);
                }
                //alOutputValues = (ArrayList)miDo.Invoke(objTest, new object[] { Item.Input });
                //alOutputValues = (ArrayList)miDo.Invoke(objTest, argsInput);
                //alOutputValues = (ArrayList)miDo.Invoke(objTest, para);
            }
            catch (Exception ex)
            {
                ///TODO:
                ///update the exception handling.
                Console.WriteLine("Error in method invoke");
            }
            return alOutputValues;
        }

        public void OnItemStart(DutInfo DUTInfo, TestItem TItem, string ExecutionID, DateTime TimeStamp)
        {
            if (dlgtStart != null)
            {
                dlgtStart(this, new ItemStartEventArgs(DUTInfo, TItem, ExecutionID, TimeStamp));
            }
        }
        public void OnItemEnd(DutInfo DUTInfo, TestItem TItem, ArrayList Values, string ExecutionID, DateTime TimeStamp)
        {
            if (dlgtStart != null)
            {
                dlgtEnd(this, new ItemEndEventArgs(DUTInfo, TItem, Values, ExecutionID, TimeStamp));
            }
        }
    }
}
