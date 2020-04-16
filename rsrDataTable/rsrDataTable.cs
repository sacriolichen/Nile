////////////////////////////////////////////////////////////////////////////////
//
// Author: Chen, Changwei
//
//------------------------------------------------------------------------------
//
// class, RsrDataTable.
//
//------------------------------------------------------------------------------
using Nile.Component;
using Nile.Definition;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nile
{
    public class RsrDataTable : ComponentBase, ICommonComponent, IDataTable
    {
        Dictionary<int, Dictionary<string, object>> m_dictAll;

        public RsrDataTable()
        {
            m_dictAll = new Dictionary<int, Dictionary<string, object>>();
            IsInitialized = true;
        }
        #region interface member
        public void AddValue(string Name, object Value, int Position)
        {
            try
            {
                if (true == ValueExists(Name, Position))
                {
                    throw new ArgumentException("Tag name exists");
                }
                if (Value == null)
                {
                    throw new ArgumentNullException();
                }
                if (true == m_dictAll.ContainsKey(Position))
                {
                    Dictionary<string, object> dictPosition = m_dictAll[Position];
                    dictPosition.Add(Name, Value);
                }
                else
                {
                    Dictionary<string, object> dictPosition = new Dictionary<string, object>();
                    dictPosition.Add(Name, Value);
                    m_dictAll.Add(Position, dictPosition);
                }
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("{0}->{1}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, ex.Message));
            }
        }

        public object GetValue(string Name, int Position)
        {
            object objValue = null;
            try
            {
                if (false == ValueExists(Name, Position))
                {
                    throw new ArgumentNullException("Tag name absent.");
                }
                objValue = m_dictAll[Position][Name];
                return objValue;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}->{1}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, ex.Message));
            }
        }

        public bool RemoveValueFromTable(string Name, int Position)
        {
            try
            {
                if (false == ValueExists(Name, Position))
                {
                    throw new ArgumentNullException("Tag name absent.");
                }
                Dictionary<string, object> dictPosition = m_dictAll[Position];
                return m_dictAll[Position].Remove(Name);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}->{1}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, ex.Message));
            }
        }

        public bool ValueExists(string Name, int Position)
        {
            try
            {
                if (true == m_dictAll.ContainsKey(Position))
                {
                    Dictionary<string, object> dictPosition = m_dictAll[Position];
                    if (true == dictPosition.ContainsKey(Name))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}->{1}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, ex.Message));
            }
        }

        public bool ValueExists(string Name, out int[] Position)
        {
            try
            {
                List<int> listRet = new List<int>();
                foreach (KeyValuePair<int, Dictionary<string, object>> kvp in m_dictAll)
                {
                    int iPos = kvp.Key;
                    Dictionary<string, object> dictPos = kvp.Value;
                    if (dictPos.ContainsKey(Name))
                    {
                        listRet.Add(iPos);
                    }
                }
                Position = listRet.ToArray();
                if (listRet.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}->{1}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, ex.Message));
            }
        }

        public string[] GetAllNames(int Position)
        {
            try
            {
                if (true == m_dictAll.ContainsKey(Position))
                {
                    Dictionary<string, object> dictPosition = m_dictAll[Position];
                    return dictPosition.Keys.ToArray();
                }
                else
                {
                    throw new ArgumentNullException("Position");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}->{1}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, ex.Message));
            }
        }

        public bool RemoveAll(int Position)
        {
            try
            {
                if (false == m_dictAll.ContainsKey(Position))
                {
                    throw new ArgumentNullException(string.Format("No specified position: {0} in DataTable", Position));
                }

                return m_dictAll.Remove(Position);
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("{0}->{1}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, ex.Message));
            }
        }

        public void RemoveAll()
        {
            try
            {
                m_dictAll.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}->{1}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, ex.Message));
            }
        }

        public override bool IsInitialized { get; set; }
        public override void Initialize(Dictionary<string, object> Options)
        {
            base.Initialize(Options);
            try
            {
                if (m_dictAll == null)
                {
                    m_dictAll = new Dictionary<int, Dictionary<string, object>>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}.-> {1}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, ex.Message));
            }
            this.IsInitialized = true;
        }

        public override void Reset()
        {
            string str = string.Format("");
            throw new System.NotImplementedException("To implemented once needed.");
        }

        public override void Send(string Command)
        {
            throw new System.NotImplementedException("To implemented once needed.");
        }
        public override string Receive()
        {
            throw new System.NotImplementedException("To implemented once needed.");
        }
        #endregion

    }
}
