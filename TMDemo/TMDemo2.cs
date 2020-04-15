using Nile.Definition;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nile.TestMethod
{
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
