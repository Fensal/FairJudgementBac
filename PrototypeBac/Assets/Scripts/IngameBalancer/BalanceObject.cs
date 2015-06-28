using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class BalanceObject
{
    public Type m_BalanceType;
    public IBalancable m_Object;
    public Dictionary<string, int> intDic;
    public Dictionary<string, float> floatDic;
    public Dictionary<string, string> stringDic;

    public BalanceObject(Type type, IBalancable obj)
    {
        intDic = new Dictionary<string, int>();
        floatDic = new Dictionary<string, float>();
        stringDic = new Dictionary<string, string>();
    }

}
