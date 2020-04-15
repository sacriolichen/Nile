using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nile.Definition;

namespace ResultReport
{
    public class ReportController
    {
        public string SN { get; private set; }
        public DutInfo Dutinfo;
        private DateTime StartTime;
        private string FullName;
        private string ExecutionID = string.Empty;

        public ReportController(string ExecutionID, string SerialNumber)
        {
            this.ExecutionID = ExecutionID;
            SN = SerialNumber;
            StartTime = DateTime.Now;
            FullName = CreateTestRecord(StartTime, SerialNumber);
        }

        public ReportController(string ExecutionID)
        {
            this.ExecutionID = ExecutionID;
            SN = string.Empty;
            StartTime = DateTime.Now;
            FullName = CreateTestRecord(StartTime);
        }

        public void ItemStarted(object sender, ItemStartEventArgs e)
        {
            try
            {
                if (false == e.ExecutionID.Equals(this.ExecutionID))
                {
                    throw new Exception(string.Format("Execution ID changed."));
                }
                string FileName = string.Empty;
                DutInfo Dut = null;
                FileName = string.Empty;
                TestItem tiStart = e.Item;

                if (true == string.IsNullOrEmpty(FullName) || false == System.IO.File.Exists(FullName))//full name is null or the file does not exist
                {
                    Dut = e.Dut;
                    tiStart = e.Item;
                    if (false == string.IsNullOrEmpty(Dut.SerialNumber))
                    {
                        FileName = string.Format("{0}_", Dut.SerialNumber);
                    }
                    if (null != e.TimeStamp)//not null
                    {
                        FileName = string.Format("{0}{1}.json)", FileName, e.TimeStamp.ToString(CommonTags.Common_LongDateTime));
                    }
                }
                //Open record file
                JObject joFile = OpenFile();//open record file

                //ToDo: add an item.
                TestItemInfo ItemInfo = e.Item.ItemInfo;
                DutInfo DUT = e.Dut;
                string strPointID;

                for (int index = 0; index < tiStart.OutputSpec.Count; index++)
                {
                    Dictionary<string, string> dictPointInfo = PointRecord(tiStart, index, null);
                    strPointID = tiStart.OutputSpec[index].MeasPointID;

                    JObject joInfo = JObject.Parse(JsonConvert.SerializeObject(dictPointInfo));//convert dictionary to jobject
                    joFile.Add(strPointID, joInfo);//add new point to record. update values by point id later.
                }

                File.WriteAllText(FullName, joFile.ToString());//save recode
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// build dictionary instance for storing test point info which used in record or show
        /// </summary>
        /// <param name="Item">the whole item info</param>
        /// <param name="index">the index in the outputspec. the order of measure point should be aligned with TM</param>
        /// <param name="OutputValues">Contains the returned value from TM. If null, the test is not started yet.</param>
        /// <returns>Dictionary of all necessary info for recorde or show</returns>
        private Dictionary<string, string> PointRecord(TestItem Item, int index, ArrayList OutputValues)
        {
            Dictionary<string, string> dictPointInfo = new Dictionary<string, string>();
            try
            {
                dictPointInfo.Add(CommonTags.TestPlan_TestName, Item.ItemInfo.ItemName);
                dictPointInfo.Add(CommonTags.TestPlan_MeasPointID, Item.OutputSpec[index].MeasPointID);
                dictPointInfo.Add(CommonTags.TestPlan_MeasPointName, Item.OutputSpec[index].MeasPointName);
                dictPointInfo.Add(CommonTags.TestPlan_Limit1, Convert.ToString(Item.OutputSpec[index].Limit1));
                dictPointInfo.Add(CommonTags.TestPlan_Limit2, Convert.ToString(Item.OutputSpec[index].Limit2));
                dictPointInfo.Add(CommonTags.TestPlan_ValueUnit, Item.OutputSpec[index].ValueUnit);
                switch (Item.OutputSpec[index].CompareRule)
                {
                    case ResultCompareRules.Equal:
                        dictPointInfo.Add(CommonTags.TestPlan_CompareRule, "=");
                        break;
                    case ResultCompareRules.Greater:
                        dictPointInfo.Add(CommonTags.TestPlan_CompareRule, ">");
                        break;
                    case ResultCompareRules.GreaterEqual:
                        dictPointInfo.Add(CommonTags.TestPlan_CompareRule, ">=");
                        break;
                    case ResultCompareRules.LELE:
                        dictPointInfo.Add(CommonTags.TestPlan_CompareRule, "<=<=");
                        break;
                    case ResultCompareRules.Less:
                        dictPointInfo.Add(CommonTags.TestPlan_CompareRule, "<");
                        break;
                    case ResultCompareRules.LL:
                        dictPointInfo.Add(CommonTags.TestPlan_CompareRule, "<<");
                        break;
                    case ResultCompareRules.NoVerication:
                        dictPointInfo.Add(CommonTags.TestPlan_CompareRule, "No Verification");
                        break;
                }
                //add value info before or after test
                if (null == OutputValues)//not test
                {
                    dictPointInfo.Add(CommonTags.TestResult_Value, "NA");
                    dictPointInfo.Add(CommonTags.TestResult_Status, "Aborted");
                }
                else
                {
                    dictPointInfo.Add(CommonTags.TestResult_Value, Convert.ToString(OutputValues[index]));
                    dictPointInfo.Add(CommonTags.TestResult_Status, JudgeResultStatus(OutputValues[index],
                                                                                    Item.OutputSpec[index].ValueType,
                                                                                    Convert.ToInt32(Item.OutputSpec[index].CompareRule),
                                                                                    Item.OutputSpec[index].Limit1,
                                                                                    Item.OutputSpec[index].Limit2));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed to build point info before item starts. {0}", ex.Message));
            }
            return dictPointInfo;
        }
        public void ItemEnded(object sender, ItemEndEventArgs e)
        {
            try
            {
                if (false == e.ExecutionID.Equals(this.ExecutionID))
                {
                    throw new Exception(string.Format("Execution ID changed."));
                }
                if (false == e.ExecutionID.Equals(this.ExecutionID))
                {
                    return;//not the unit on this position.
                }
                string FileName = string.Empty;
                DutInfo Dut = null;
                FileName = string.Empty;

                if (true == string.IsNullOrEmpty(FullName) || false == System.IO.File.Exists(FullName))//full name is null or the file does not exist
                {
                    Dut = e.Dut;
                    if (false == string.IsNullOrEmpty(Dut.SerialNumber))
                    {
                        FileName = string.Format("{0}_", Dut.SerialNumber);
                    }
                    if (null != e.TimeStamp)//not null
                    {
                        FileName = string.Format("{0}{1}.json)", FileName, e.TimeStamp.ToString(CommonTags.Common_LongDateTime));
                    }
                }
                for (int index = 0; index < e.Item.OutputSpec.Count; index++)
                //ToDo: add an item.
                {
                    string strPointID = e.Item.OutputSpec[index].MeasPointID;
                    Dictionary<string, string> dictPointInfo = PointRecord(e.Item, index, e.OutputValue);

                    JObject joNew = JObject.Parse(JsonConvert.SerializeObject(dictPointInfo));//convert dictionary to jobject

                    JObject joFile = OpenFile();//open record file
                    if (false == string.IsNullOrEmpty(strPointID))
                    {
                        joFile[strPointID].Replace((JToken)joNew); // replace the item info with test value
                    }
                    else
                    {
                        throw new Exception("Can't get Point ID from test plan. It's necessary to identify test.");
                    }
                    File.WriteAllText(FullName, joFile.ToString());//save recode
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private string CreateTestRecord(DateTime TimeStamp, string SerialNumber = null)
        {
            string strFileName = string.Empty;
            try
            {
                string strTime = TimeStamp.ToString(CommonTags.Common_LongDateTime);
                if (true == string.IsNullOrEmpty(SerialNumber))
                {
                    strFileName = string.Format("{0}\\{1}.json", System.Environment.CurrentDirectory, strTime);
                }
                else
                {
                    strFileName = string.Format("{0}\\{1}_{2}.json", System.Environment.CurrentDirectory, SerialNumber, strTime);
                }

                JObject joProperties = new JObject(new JProperty(CommonTags.TestResult_Properties, new JObject(new JProperty(CommonTags.TestResult_StartTime, TimeStamp))));

                File.WriteAllText(strFileName, joProperties.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error in create json record file. {0}", ex.Message));
            }
            return strFileName;
        }

        private JObject OpenFile()
        {
            JObject joFile = null;
            if (true == string.IsNullOrEmpty(SN) && StartTime == null)
            {
                throw new Exception(string.Format("Can't initialize a test record file since SN and start time are empty."));
            }
            using (StreamReader file = File.OpenText(FullName))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                joFile = (JObject)JToken.ReadFrom(reader);
            }
            return joFile;
        }

        public static string JudgeResultStatus(object Value, ResultValueTypes ValueType, int CompareRule, object Limit1 = null, object Limit2 = null)
        {
            try
            {
                switch ((ResultCompareRules)CompareRule)
                {
                    case ResultCompareRules.NoVerication:
                        return CommonTags.TestResult_PointStatus_NV;
                    case ResultCompareRules.Equal:
                        return EqualCompare(Value, Limit1);
                    case ResultCompareRules.Greater:
                        if (ValueType == ResultValueTypes.Bool || ValueType == ResultValueTypes.String || ValueType == ResultValueTypes.Unknown)
                        {
                            return CommonTags.TestResult_PointStatus_NA;
                        }
                        else
                        {
                            int CompareIndex = ValueCompare(Value, Limit1);
                            if (CompareIndex == 1) // >
                            {
                                return CommonTags.TestResult_PointStatus_P;
                            }
                            else// <=
                            {
                                return CommonTags.TestResult_PointStatus_F;
                            }
                        }
                    case ResultCompareRules.GreaterEqual:
                        if (ValueType == ResultValueTypes.Bool || ValueType == ResultValueTypes.String || ValueType == ResultValueTypes.Unknown)
                        {
                            return CommonTags.TestResult_PointStatus_NA;
                        }
                        else
                        {
                            int CompareIndex = ValueCompare(Value, Limit1);
                            if (CompareIndex == -1)// <
                            {
                                return CommonTags.TestResult_PointStatus_F;
                            }
                            else//>=
                            {
                                return CommonTags.TestResult_PointStatus_P;
                            }
                        }
                    case ResultCompareRules.Less:
                        if (ValueType == ResultValueTypes.Bool || ValueType == ResultValueTypes.String || ValueType == ResultValueTypes.Unknown)
                        {
                            return CommonTags.TestResult_PointStatus_NA;
                        }
                        else
                        {
                            int CompareIndex = ValueCompare(Value, Limit1);
                            if (CompareIndex == -1)// <
                            {
                                return CommonTags.TestResult_PointStatus_P;
                            }
                            else//>=
                            {
                                return CommonTags.TestResult_PointStatus_F;
                            }
                        }
                    case ResultCompareRules.LessEqual:
                        if (ValueType == ResultValueTypes.Bool || ValueType == ResultValueTypes.String || ValueType == ResultValueTypes.Unknown)
                        {
                            return CommonTags.TestResult_PointStatus_NA;
                        }
                        else
                        {
                            int CompareIndex = ValueCompare(Value, Limit1);
                            if (CompareIndex == 1)// >
                            {
                                return CommonTags.TestResult_PointStatus_F;
                            }
                            else//<=
                            {
                                return CommonTags.TestResult_PointStatus_P;
                            }
                        }
                    case ResultCompareRules.LL:// limit1 < value < limit2
                        if (ValueType == ResultValueTypes.Bool || ValueType == ResultValueTypes.String || ValueType == ResultValueTypes.Unknown)
                        {
                            return CommonTags.TestResult_PointStatus_NA;
                        }
                        else
                        {
                            int CompareIndex1 = ValueCompare(Value, Limit1);
                            int CompareIndex2 = ValueCompare(Value, Limit2);
                            if (CompareIndex1 == 1 && CompareIndex2 == -1)// limit1 < value < limit2
                            {
                                return CommonTags.TestResult_PointStatus_P;
                            }
                            else// value <= limit1 or value >= limit2
                            {
                                return CommonTags.TestResult_PointStatus_F;
                            }
                        }
                    case ResultCompareRules.LELE:// limit1 <= value <= limit2
                        if (ValueType == ResultValueTypes.Bool || ValueType == ResultValueTypes.String || ValueType == ResultValueTypes.Unknown)
                        {
                            return CommonTags.TestResult_PointStatus_NA;
                        }
                        else
                        {
                            int CompareIndex1 = ValueCompare(Value, Limit1);
                            int CompareIndex2 = ValueCompare(Value, Limit2);
                            if (CompareIndex1 == -1 || CompareIndex2 == 1)//  value < limit1 or value > limit2
                            {
                                return CommonTags.TestResult_PointStatus_F;
                            }
                            else// limit1 <= value <= limit2 
                            {
                                return CommonTags.TestResult_PointStatus_P;
                            }
                        }
                    default:
                        return CommonTags.TestResult_PointStatus_NA;
                }
            }
            catch (Exception ex)
            {
            }
            return CommonTags.TestResult_PointStatus_Un;
        }

        public static string EqualCompare<T>(T Value, T Limit1)
        {
            try
            {
                if (true == Value.Equals(Limit1))
                {
                    return CommonTags.TestResult_PointStatus_P;
                }
                else
                {
                    return CommonTags.TestResult_PointStatus_F;
                }
            }
            catch (Exception ex)
            {
                return CommonTags.TestResult_PointStatus_Un;
            }
        }

        public static int ValueCompare<T>(T Value, T Limit)//only int and double
        {
            try
            {
                if (false == Limit.GetType().Equals(Value.GetType()))//in case of double vs integer, convert all to double.
                {
                    double dValue = Convert.ToDouble(Value);
                    double dLimit = Convert.ToDouble(Limit);
                    return (dValue as IComparable).CompareTo(dLimit);
                }
                else // same type
                { return (Value as IComparable).CompareTo(Limit); }
            }
            catch (Exception ex)
            {
                return -99999999;//type mismatch?
            }
        }
    }
}
