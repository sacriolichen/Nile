using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nile.Definition
{
    public class CommonTags
    {
        /// <summary>
        /// these tags are used in test plan handling
        /// </summary>
        public static string TestPlan_Sequence = "Sequence";
        public static string TestPlan_Properties = "Properties";
        public static string TestPlan_TestName = "Name";
        public static string TestPlan_ResourceName = "FileName";
        public static string TestPlan_MethodName = "Method";
        public static string TestPlan_Input = "Input";
        public static string TestPlan_Output = "OutputSpec";
        public static string TestPlan_MeasPointID = "ID";
        public static string TestPlan_MeasPointName = "PointName";//description is also acceptable
        public static string TestPlan_Limit1 = "Limit1";
        public static string TestPlan_Limit2 = "Limit2";
        public static string TestPlan_Path = "Path";
        public static string TestPlan_ValueType = "ValueType";
        public static string TestPlan_ValueUnit = "Unit";
        public static string TestPlan_CompareRule = "CompareRule";
        public static string TestPlan_EditTime = "EditTime";

        /// <summary>
        ///below tags for test result section in recording.
        /// </summary>
        public static string TestResult_ItemName = "ItemName";
        public static string TestResult_StartTime = "StartTime";
        public static string TestResult_EndTime = "EndTime";
        public static string TestResult_PointName = "PointName";
        public static string TestResult_Description = TestPlan_MeasPointName;
        public static string TestResult_Value = "Value";
        public static string TestResult_Limit1 = TestPlan_Limit1;
        public static string TestResult_Limit2 = TestPlan_Limit2;
        public static string TestResult_ValueType = TestPlan_ValueType;
        public static string TestResult_CompareRule = TestPlan_CompareRule;
        public static string TestResult_Properties = "Properties";
        public static string TestResult_Status = "Status";
        public static string TestResult_PointStatus_NA = "NotAvailable";
        public static string TestResult_PointStatus_NV = "NoVerification";
        public static string TestResult_PointStatus_P = "Pass";
        public static string TestResult_PointStatus_F = "Fail";
        public static string TestResult_PointStatus_Un = "Unknown";

        //common
        public static string Common_LongDateTime = "yyyyMMdd-HHmmss.ffff";
        public static string Common_ExecutionID = "ExecutionID";//An unique ID (GUID) to identify each test plan running.
    }
}
