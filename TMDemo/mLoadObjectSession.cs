/*******************************************************************************\
                                  '                                                     
                           '                                                            
                                     °                                                 
                /¯¯¯¯\    °                                                            
    '|\¯¯¯¯\/'/|       '|'  ____    °  /¯¯¯¯/|                                         
    '|;'|       '/'/       /| |\____\ '   '|       |;|             '/¯¯¯¯/|¯¯¯¯|        
     '/       /'/       /;'| /¯¯¯¯¯/|  '|\      '\|    '        |       |;|       |     
    /       /'/       /;;'/ '|        '|;|   '|;'\       \            |       |/____/|  
  '/       /'/       /;;'/'  '|\        \| '  '\;|       |/¯¯¯¯/|  |       |;|¯¯¯¯|     
  |____|;|____|;;/    '|;\_____\'   '/____/|____'|'|  |\____\|____|                     
  |       |/|       |/       \;|         |   |       |;|       '|/' |;|       ||      '|
  |____| |____|         \|_____|    |____|/|____'| ° '\|____||____| '                  
*********************************************************************************
*   File name:  IDemo.cs
*   Document no:
*   Document ver:
*   Design Responsible: 12234871
*   Description: [Description("Demo of TM to load a session")]
*   Date: 4/13/2020 09:51:36 AM
* 
*   COPYRIGHT (C) Update later                                                 *
\*******************************************************************************/
using Nile.Definition;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nile.TestMethod
{
    public class mLoadObjectSession : TestClassBase
    {
        ICommonComponent pICC = null;
        string m_strSessionName_Lable = "SESSION_NAME";

        public mLoadObjectSession(Hashtable TestParams) : base(TestParams)
        {
            //pICC = GetInterface("IArea");
        }

        public ArrayList Measurement(Dictionary<string, object> Input)
        {
            try
            {
                string strSessionName = string.Empty;
                GetInput(m_strSessionName_Lable, true, out strSessionName, string.Empty);

                pICC = GetInterface(m_strSessionName_Lable, 1) as ICommonComponent;

                if (pICC.IsInitialized == false)
                {
//                    pICC.Initialize();
                }
                else
                {
                    //Initialized
                }
                SaveValue(0);
            }
            catch
            {
                ///TODO:
                ///To implement error handling to report and do not interrupt test plan
            }
            return Output;
        }
    }

}
