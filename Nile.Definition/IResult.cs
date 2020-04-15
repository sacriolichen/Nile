using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nile.Definition
{
    public enum ResultCompareRules
    {
        NoVerication    = 0,
        Equal           = 1,
        Greater         = 2,    // > limit1
        GreaterEqual    = 3,    // >= limit1
        Less            = 4,    // < limit1
        LessEqual       = 5,    // <= limit1
        LL              = 6,    // limit1 < value < limit2
        LELE            = 7,    // limit1 <= value <= limit2
    }
    public enum ResultValueTypes
    {
        //unknown
        Unknown = 0,
        //Intege
        Int = 1,
        //double
        Double = 2,
        //string
        String = 3,
        //bool
        Bool = 4
    }

    public enum TestPointResult
    {
        Unknown = 0,
        Pass = 1,
        Fail = 2,
        NoVerification = 3,
        NotAvailable = 4
    }
    class PointResult:Dictionary<string, string>
    {
        public string PointResult_TestName { get; set; }
        public string PointResult_StartTime { get; set; }
        public string PointResult_EndTime { get; set; }
        public string PointResult_PointName { get; set; }
        public string PointResult_Description { get; set; }
        public string PointResult_Value { get; set; }
        public string PointResult_Limit1 { get; set; }
        public string PointResult_Limit2 { get; set; }
        public string PointResult_ValueType { get; set; }
        public string PointResult_CompareRule { get; set; }

        public PointResult()
        {
            this.Add(CommonTags.TestResult_PointName, "");
            this.Add(CommonTags.TestResult_Description, "");
            this.Add(CommonTags.TestResult_Value, "");
            this.Add(CommonTags.TestResult_Limit1, "");
            this.Add(CommonTags.TestResult_Limit2, "");
            this.Add(CommonTags.TestResult_ValueType, "");
            this.Add(CommonTags.TestResult_CompareRule, "");
        }

        public PointResult(Dictionary<string, object> PointSpec, object value, string ItemName, DateTime StartTime, DateTime EndTime)
        {
            try
            {
                this.Add(CommonTags.TestResult_PointName, Convert.ToString(PointSpec[CommonTags.TestPlan_MeasPointName]));
                this.Add(CommonTags.TestResult_Limit1, Convert.ToString(PointSpec[CommonTags.TestPlan_Limit1]));
                this.Add(CommonTags.TestResult_Limit2, Convert.ToString(PointSpec[CommonTags.TestPlan_Limit2]));
                this.Add(CommonTags.TestResult_ValueType, Convert.ToString(PointSpec[CommonTags.TestPlan_ValueType]));
                this.Add(CommonTags.TestResult_CompareRule, Convert.ToString(PointSpec[CommonTags.TestPlan_CompareRule]));
                this.Add(CommonTags.TestResult_Value, Convert.ToString(value));
                this.Add(CommonTags.TestResult_ItemName, ItemName);
                if (StartTime == null || EndTime == null)
                {
                    this.Add(CommonTags.TestResult_StartTime, StartTime.ToString("NA"));
                    this.Add(CommonTags.TestResult_EndTime, EndTime.ToString("NA"));
                }
                else
                {
                    this.Add(CommonTags.TestResult_StartTime, StartTime.ToString(CommonTags.Common_LongDateTime));
                    this.Add(CommonTags.TestResult_EndTime, EndTime.ToString(CommonTags.Common_LongDateTime));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error in build result dictionary.{0}", ex.Message));
            }
        }
    }
}
