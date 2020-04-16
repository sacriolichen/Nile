////////////////////////////////////////////////////////////////////////////////
//
// Author: Chen, Changwei
//
//------------------------------------------------------------------------------
//
// interface, IProcessor.
//
//------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nile.Definition
{
    class Processor
    {
    }
    public class ItemStartEventArgs : EventArgs
    {
        public TestItem Item { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public DutInfo Dut { get; private set; }
        //public ArrayList OutputSpec { get; private set; }
        public string ExecutionID;

        public ItemStartEventArgs(DutInfo Dut, TestItem TItem, string ExecutionID, DateTime Timestamp)
        {
            this.Item = TItem;
            this.TimeStamp = Timestamp;
            this.Dut = Dut;
            this.ExecutionID = ExecutionID;
            //this.OutputSpec = OutputSpec;
        }
    }
    public class ItemEndEventArgs : EventArgs
    {
        public DutInfo Dut { get; private set; }
        public TestItem Item { get; private set; }
        public DateTime TimeStamp { get; private set; }
        //public ArrayList OutputSpec { get; private set; }
        public ArrayList OutputValue { get; private set; }
        public string ExecutionID;

        public ItemEndEventArgs(DutInfo Dut, TestItem TItem, ArrayList OutputValue, string ExecutionID, DateTime Timestamp)
        {
            this.Item = TItem;
            this.OutputValue = OutputValue;
            this.TimeStamp = Timestamp;
            this.ExecutionID = ExecutionID;
        }
    }
}
