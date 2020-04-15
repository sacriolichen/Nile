using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nile.Definition
{
    public interface ITestPlan
    {
        //        Dictionary<string, string> Properties;

    }

    public class MeasurePoint
    {
        public string MeasPointID { get; set; }
        public string MeasPointName { get; set; }
        public object Limit1 { get; set; }
        public object Limit2 { get; set; }
        public ResultValueTypes ValueType { get; set; }
        public ResultCompareRules CompareRule { get; set; }
        public string ValueUnit { get; set; }

        public MeasurePoint()
        { }

        public MeasurePoint(string MeasPointID, string MeasPointName, object Limit1, object Limit2, int ValueType, int CompareRule, string ValueUnit)
        {
            this.MeasPointID = MeasPointID;
            this.MeasPointName = MeasPointName;
            this.Limit1 = Limit1;
            this.Limit2 = Limit2;
            this.ValueType = (ResultValueTypes)ValueType;
            this.CompareRule = (ResultCompareRules)CompareRule;
            this.ValueUnit = ValueUnit;
        }
    }

    public class OutputInfo : List<MeasurePoint>
    {
        public string GUID { get; set; }
        public OutputInfo()
        { }
        public OutputInfo(string GUID)
        {
            this.GUID = GUID;
        }

        public MeasurePoint GetPointByPointID(string MeasurePointID)
        {
            MeasurePoint MP = this.Where(p => p.MeasPointID.ToLower().Equals(MeasurePointID.ToLower())).FirstOrDefault();

            foreach (MeasurePoint Point in this)
            {
                if (true == Point.MeasPointID.ToLower().Equals(MeasurePointID.ToLower()))
                {
                    return Point;
                }
            }
            return null;
        }

        public bool PointIDExists(string MeasurePointID)
        {
            foreach (MeasurePoint Point in this)
            {
                if (true == Point.MeasPointID.ToLower().Equals(MeasurePointID.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }

        public void RemoveByPointID(string MeasurePointID)
        {
            this.Remove(this.Where(p => p.MeasPointID.ToLower().Equals(MeasurePointID.ToLower())).FirstOrDefault());
        }
    }

    //public class InputInfo : Dictionary<string, object>
    //{
    //    //public string GUID { get; set; }
    //    public InputInfo()
    //    {
    //    }

    //    //public InputInfo(string GUID)
    //    //{
    //    //    this.GUID = GUID;
    //    //}
    //}

    /// <summary>
    /// The info is to write test plan file
    /// </summary>
    public class TestItemInfo
    {
        public string ItemName { get; set; }
        public string FileName { get; set; }
        public string Method { get; set; }
        public TestItemInfo()
        {
        }
        public TestItemInfo(string ItemName, string FileName, string Method)
        {
            this.ItemName = ItemName;
            this.FileName = FileName;
            this.Method = Method;
        }
    }

    /// <summary>
    /// The info get from file and used in testing
    /// </summary>
    //public class TestItemInfoRead : TestItemInfoBase
    //{
    //    public string GUID { get; set; }
    //    public TestItemInfoRead(string GUID, string ItemName, string FileName, string Method)
    //    {
    //        //this.GUID = GUID;
    //        //this.ItemName = ItemName;
    //        //this.FileName = FileName;
    //        //this.Method = Method;
    //    }
    //}

    public class TestItem
    {
        public TestItemInfo ItemInfo;
        public Dictionary<string, object> Input;
        public OutputInfo OutputSpec;

        public TestItem()
        {
        }

        public TestItem(string GUID, TestItemInfo ItemInfo, Dictionary<string, object> InputInfo, OutputInfo OutputInfo)
        {
            this.ItemInfo = ItemInfo;
            this.Input = InputInfo;
            this.OutputSpec = OutputInfo;
        }
    }

    //public class TestItemRead : TestItemBase
    //{
    //    public string GUID { get; set; }
    //    public new TestItemInfoRead ItemInfo { get; set; }

    //    public int PointCount { get { return OutputSpec.Count; } }
    //    public int InputItemCount { get { return Input.Count; } }

    //    public TestItemRead()
    //    { }

    //    public TestItemRead(string GUID, TestItemInfoBase ItemInfo, InputInfo InputInfo, OutputInfo OutputInfo)
    //    {
    //        this.GUID = GUID;
    //    }
    //}

    public class TestPlan
    {
        public List<TestItem> Sequence;
        public Dictionary<string, string> Properties;

        public TestPlan()
        {
            Sequence = new List<TestItem>();
            Properties = new Dictionary<string, string>();
        }
    }
    //public class TestPlan
    //{
    //    public List<TestItemRead> Sequence;
    //    public Dictionary<string, string> Properties;
    //    public string FullFileName { get; set; }

    //    public TestPlan()
    //    {
    //        Sequence = new List<TestItemRead>();
    //        Properties = new Dictionary<string, string>();
    //    }
    //}
}
