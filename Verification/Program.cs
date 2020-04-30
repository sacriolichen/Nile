using Nile.Definition;
using Nile.SessionManagement;
using Nile.TestSequence;
using Nile.TestSequence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nile
{
    class Program
    {
        static void Main(string[] args)
        {
            Hashtable CoreParams = new Hashtable();
            TestPlan TP = TestPlanReader.LoadJson("TestPlanExample.json");
            DutInfo DUT = new DutInfo();
            DUT.SerialNumber = "ABCD1234";
            DUT.Position = 1;

            #region Init core sessions
            SessionManager sm = new SessionManager("SessionMapSample.json");
            CoreParams.Add(CommonTags.CoreData_SessionManage, sm);

            RsrDataTable DataTable = new RsrDataTable();
            CoreParams.Add(CommonTags.CoreData_DataTable, DataTable);
            #endregion

            Processor processor = new Processor(TP, DUT, CoreParams);
            processor.PlanExecution();


            #region Clear sessions after test
            ///TODO:
            ///reset instead of dispose?
            
            lock (sm)
            {
                List<string> listToRemove = new List<string>();

                foreach (DictionaryEntry de in sm)
                {
                    string strName = Convert.ToString(de.Key);
                    if (strName.EndsWith(string.Format("_{0}",DUT.Position.ToString())))
                    {
                        listToRemove.Add(strName);
                    }
                }

                foreach (string strName in listToRemove)
                {
                    string strSessionName = strName.Substring(0, strName.LastIndexOf('_'));
                    sm.Remove(strSessionName, DUT.Position);
                }
            }
            #endregion
        }
    }
}