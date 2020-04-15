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
*   File name:  TM1.cs
*   Document no:
*   Document ver:
*   Design Responsible: 12234871
*   Description: [Description("Demo of TM")]
*   Date: 3/12/2020 10:23:09 AM
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
    public class TMExample1 : TestClassBase
    {
        public TMExample1(Hashtable TestParams) : base(TestParams)
        {//Do nothing
        }
        public override ArrayList Do(Dictionary<string, object> Input)
        {
            //Dictionary<string, object> dictInput = (Dictionary<string, object>)args[0];
            int iInput = Convert.ToInt32(Input["Radius"]);

            double area = Math.PI * Math.Pow(iInput, 2);

            ArrayList alOutput = new ArrayList();
            alOutput.Add(area);
            return alOutput;
        }
    }

}
