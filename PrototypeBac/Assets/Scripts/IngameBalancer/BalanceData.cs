using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;

public class BalanceData
{
    public Dictionary<object, BalanceObject> m_BalanceObjects;

    public BalanceData()
    {
        m_BalanceObjects = new Dictionary<object, BalanceObject>();
    }

    /// <summary>
    /// Adds balancing with custom name.
    /// </summary>
    /// <param name="obj">Object to add</param>
    /// <param name="name">Custom Name</param>
    /// <returns>Success of operation</returns>
    public bool GetBalancing(IBalancable obj, Type objecttype)
    {
        (obj as IBalancable).Changed += UpdateData;

        Type type = obj.GetType();
        if(!m_BalanceObjects.ContainsKey(obj)){
            m_BalanceObjects.Add(obj, new BalanceObject(objecttype, obj));
        }

        //get all public members of the object
        foreach (var f in type.GetProperties())
        {
            //check whether or not the type of the object is int
            bool result = f.GetValue(obj, null) is int;
            if (result)
            {
                //add to dictionary the field name and the value of this specific object converted to int
                if (m_BalanceObjects[obj].intDic.ContainsKey(f.Name))
                {
                    m_BalanceObjects[obj].intDic.Add(f.Name, Convert.ToInt32(f.GetValue(obj, null)));
                }
                else
                {
                    m_BalanceObjects[obj].intDic[f.Name] = Convert.ToInt32(f.GetValue(obj, null));
                }

            }
            else
            {
                //check whether or not the type of the object is float
                result = f.GetValue(obj, null) is float;
                if (result)
                {
                    //add to dictionary the field name and the value of this specific object converted to float
                    if (m_BalanceObjects[obj].floatDic.ContainsKey(f.Name))
                    {
                        m_BalanceObjects[obj].floatDic.Add(f.Name, Convert.ToSingle(f.GetValue(obj, null)));
                    }
                    else
                    {
                        m_BalanceObjects[obj].floatDic[f.Name] = Convert.ToSingle(f.GetValue(obj, null));
                    }
                }
                else
                {
                    //check whether or not the type of the object is string
                    result = f.GetValue(obj, null) is string;
                    if (result)
                    {
                        //add to dictionary the field name and the value of this specific object converted to string
                        if (m_BalanceObjects[obj].stringDic.ContainsKey(f.Name))
                        {
                            m_BalanceObjects[obj].stringDic.Add(f.Name, Convert.ToString(f.GetValue(obj, null)));
                        }
                        else
                        {
                            m_BalanceObjects[obj].stringDic[f.Name] = Convert.ToString(f.GetValue(obj, null));
                        }
                    }
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Is attached to and called by the event IBalanced.Changed
    /// </summary>
    /// <param name="obj">Object to update</param>
    /// <param name="e"></param>
    public void UpdateData(object obj, EventArgs e)
    {
        //using List of strings because you cant change a dictionary you're iterating though
        List<string> keys = new List<string>(m_BalanceObjects[obj].intDic.Keys);
        foreach (string key in keys)
        {
            m_BalanceObjects[obj].intDic[key] = Convert.ToInt32(obj.GetType().GetProperty(key).GetValue(obj, null));
        }

        keys = new List<string>(m_BalanceObjects[obj].floatDic.Keys);
        foreach (string key in keys)
        {
            m_BalanceObjects[obj].floatDic[key] = Convert.ToSingle(obj.GetType().GetProperty(key).GetValue(obj, null));
        }

        keys = new List<string>(m_BalanceObjects[obj].stringDic.Keys);
        foreach (string key in keys)
        {
            m_BalanceObjects[obj].stringDic[key] = Convert.ToString(obj.GetType().GetProperty(key).GetValue(obj, null));
        }
    }

    /// <summary>
    /// Dumps all balancingdata to debug console.
    /// </summary>
    public void Print()
    {
        foreach (KeyValuePair<object, BalanceObject> ob in m_BalanceObjects)
        {
            string output = "";

            output += ob.Key + "\n";
            output += "Ints:\n";
            foreach (KeyValuePair<string, int> i in ob.Value.intDic)
            {
                output += i.Key + ": " + i.Value + "\n";
            }

            output += "Floats:\n";
            foreach (KeyValuePair<string, float> f in ob.Value.floatDic)
            {
                output += f.Key + ": " + f.Value + "\n";
            }

            output += "Strings:\n";
            foreach (KeyValuePair<string, string> s in ob.Value.stringDic)
            {
                output += s.Key + ": " + s.Value + "\n";
            }
            Debug.Log(output);
        }
        Debug.Log("----------------------------");
    }

    /// <summary>
    /// Used this to edit int values of your object
    /// </summary>
    /// <param name="obj">object to edit</param>
    /// <param name="name">name of membervariable</param>
    /// <param name="value">value to set</param>
    public void SetInt(object obj, string name, int value)
    {
        obj.GetType().GetProperty(name).SetValue(obj, value, null);
    }

    /// <summary>
    /// Used this to edit float values of your object
    /// </summary>
    /// <param name="obj">object to edit</param>
    /// <param name="name">name of membervariable</param>
    /// <param name="value">value to set</param>
    public void SetFloat(object obj, string name, float value)
    {
        obj.GetType().GetProperty(name).SetValue(obj, value, null);
    }

    /// <summary>
    /// Used this to edit string values of your object
    /// </summary>
    /// <param name="obj">object to edit</param>
    /// <param name="name">name of membervariable</param>
    /// <param name="value">value to set</param>
    public void SetString(object obj, string name, string value)
    {
        obj.GetType().GetProperty(name).SetValue(obj, value, null);
    }
}
