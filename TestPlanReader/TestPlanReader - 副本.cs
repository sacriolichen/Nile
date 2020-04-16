using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nile.Definition;


//ToDo:
//1. Add read section "Properties"
//2. Add get a property from TP ojbect.
//3. To verify the functions of get*** from TP object

namespace Nile.TestSequence
{
    public class TestPlanReader
    {
        public TestPlanReader()
        { }

        /// <summary>
        /// Load test plan from a json file
        /// </summary>
        /// <param name="FullFileName"></param>
        /// <returns></returns>
        public static TestPlan LoadJson(string FullFileName)
        {
            try
            {
                if (true == File.Exists(FullFileName))
                {
                    JObject joFile = TestPlanValidate(FullFileName);
                    TestPlan TP = null;
                    if (joFile != null)
                    {
                        TP = JsonConvert.DeserializeObject<TestPlan>(joFile.ToString());

                    }
                    else
                    {
                        throw new Exception(string.Format("File {0} is not valid json format test plan.", FullFileName));
                    }
                    foreach (TestItemRead TIR in TP.Sequence)
                    {
                        string strGUID = Guid.NewGuid().ToString();
                        TIR.GUID = strGUID;
                        TIR.ItemInfo.GUID = strGUID;
                    }
                    TP.FullFileName = FullFileName;
                    return TP;
                }
                else
                {
                    throw new Exception(string.Format("File {0} does not exist.", FullFileName));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error in TestPlan(string). {0}", ex.Message));
            }
        }

        /// <summary>
        /// save test plan from a test plan instance
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="TPW"></param>
        /// <returns></returns>
        public static bool SaveJsonTestPlan(string FileName, TestPlanBase TPW)
        {
            string strFileName = string.Empty;
            try
            {
                if (TPW.Properties.ContainsKey(CommonTags.TestPlan_EditTime))
                {
                    TPW.Properties[CommonTags.TestPlan_EditTime] = DateTime.Now.ToLongTimeString();
                }
                else
                {
                    TPW.Properties.Add(CommonTags.TestPlan_EditTime, DateTime.Now.ToLongTimeString());
                }

                JObject joFile = JObject.Parse(JsonConvert.SerializeObject(TPW));//convert dictionary to jobject
                File.WriteAllText(FileName, joFile.ToString());//save recode
                return true;
            }
            catch (Exception ex)
            {
                //throw new Exception(string.Format("Error in create json record file. {0}", ex.Message));
            }
            return false;
        }

        /// <summary>
        /// return the root JObject of the file. Null if invalid.
        /// </summary>
        public static JObject TestPlanValidate(string FullFileName)
        {
            try
            {
                StreamReader file = File.OpenText(FullFileName);
                JsonTextReader reader = new JsonTextReader(file);
                JObject joTP = (JObject)JToken.ReadFrom(reader);

                return joTP;
            }
            catch (Exception ex)
            {
                //ToDo: send the error message to log.
                return null ;
            }
        }
    }
}
