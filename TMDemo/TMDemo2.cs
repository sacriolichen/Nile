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
*   File name:  TMDemo2.cs
*   Document no:
*   Document ver:
*   Design Responsible: 12234871
*   Description: [Description("Demo of test method to calc area")]
*   Date: 4/12/2020 11:29:47 AM
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
    //A demo TM, calling driver and adding/getting data to/from DataTable
    public class TMDemo2 : TestClassBase
    {
        IArea pIArea = null;

        public TMDemo2()
        {
            IArea pIArea = (IArea)GetInterface("IArea");
        }

        public TMDemo2(Hashtable TestParams) : base(TestParams)
        {
            pIArea = (IArea)GetInterface("IArea");
        }

        public ArrayList Do(Dictionary<string, object> Input)
        {
            if (pIDataTable == null)
            {
                //error
                ///TODO
                ///To implement error handling later
            }
            foreach (KeyValuePair<string, object> kvp in Input)
            {
                int i = Convert.ToInt32(kvp.Value);
                int oldvalue = 1;
                if (true == pIDataTable.ValueExists("OldValue", Position))
                {
                    oldvalue = Convert.ToInt32(pIDataTable.GetValue("OldValue", Position));
                }
                double d = oldvalue * pIArea.Quadrate(Convert.ToDouble(kvp.Value));

                if (true == pIDataTable.ValueExists("OldValue", Position))
                {
                    oldvalue = Convert.ToInt32(pIDataTable.RemoveValueFromTable("OldValue", Position));
                }
                pIDataTable.AddValue("OldValue", d, Position);
                SaveValue(d);
                //SaveValue(Math.PI * Math.Pow(i, 2));
            }

            return Output;
        }
    }

}
