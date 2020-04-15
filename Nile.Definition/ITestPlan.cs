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

    /// <summary>
    /// A class of measure point
    /// </summary>
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

    /// <summary>
    /// A class of output info collection
    /// </summary>
    public class OutputInfo : List<MeasurePoint>
    {
        //public string GUID { get; set; }
        public OutputInfo()
        { }
        public OutputInfo(string GUID)
        {
            //this.GUID = GUID;
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
    /// Necessary info to run a test.
    /// Consists of basic info, input and ouputspec
    /// </summary>
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

    /// <summary>
    /// In basic, it's a list of all test item for DUT
    /// </summary>
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
}
