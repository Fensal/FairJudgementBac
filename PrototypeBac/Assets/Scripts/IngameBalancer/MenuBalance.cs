using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MenuBalance : MonoBehaviour {
    bool m_ShowChracters = false;
    bool m_ShowDetails = false;
    object m_CurrentCharacter = null;

    //void OnGUI()
    //{
    //    if (!m_ShowChracters && !m_ShowDetails)
    //    {
    //        if (GUI.Button(new Rect(0, 0, 200, 20), "ShowCharacters"))
    //        {
    //            m_ShowChracters = true;
    //            m_ShowDetails = false;
    //            Time.timeScale = 0.0f;
    //        }
    //    }

    //    if(GUI.Button(new Rect(200, 0, 200, 20), "ResetLevel"))
    //    {
    //        Application.LoadLevel("Playground");
    //    }

    //    if (m_ShowChracters)
    //    {
    //        if (GUI.Button(new Rect(0, 0, 200, 20), "Back"))
    //        {
    //            m_ShowChracters = false;
    //            m_ShowDetails = false;
    //            m_CurrentCharacter = null;
    //            Time.timeScale = 1.0f;
    //        }
    //        int i = 0;
    //        int j = 0;
    //        foreach (KeyValuePair<object, BalanceObject> o in Root.Instance.bdata.m_BalanceObjects)
    //        {
    //            int x = 0 + (200 * i);
    //            int y = 20 + (20 * j);
    //            if (GUI.Button(new Rect(x, y, 200, 20), o.Key.ToString()))
    //            {
    //                m_ShowChracters = false;
    //                m_ShowDetails = true;
    //                m_CurrentCharacter = o.Key;
    //            }

    //            i++;
    //            if(i == 5){
    //                i = 0;
    //                j++;
    //            }
    //        }
    //    }

    //    if (m_ShowDetails)
    //    {
    //        if (GUI.Button(new Rect(0, 0, 200, 20), "Back"))
    //        {
    //            m_ShowChracters = true;
    //            m_ShowDetails = false;
    //            m_CurrentCharacter = null;
    //        }
    //        if (GUI.Button(new Rect(200, 0, 200, 20), "Print"))
    //        {
    //            Root.Instance.bdata.Print();
    //        }


    //        if (m_CurrentCharacter != null)
    //        {
    //            GUI.Label(new Rect(0, 20, 100, 20), "Int Values: ");

    //            List<string> keys = new List<string>(Root.Instance.bdata.m_BalanceObjects[m_CurrentCharacter].intDic.Keys);
    //            int index = 0;
    //            foreach (string s in keys)
    //            {
    //                GUI.Label(new Rect(0, 40 + (20 * index), 200, 20), s + ": " + Root.Instance.bdata.m_BalanceObjects[m_CurrentCharacter].intDic[s]);
    //                Root.Instance.bdata.SetInt(m_CurrentCharacter, s, Convert.ToInt32(GUI.HorizontalSlider(new Rect(200, 40 + (20 * index), 100, 20), Root.Instance.bdata.m_BalanceObjects[m_CurrentCharacter].intDic[s], 0, 100)));
    //                index++;
    //            }

    //            GUI.Label(new Rect(300, 20, 100, 20), "Float Values: ");

    //            keys = new List<string>(Root.Instance.bdata.m_BalanceObjects[m_CurrentCharacter].floatDic.Keys);
    //            index = 0;
    //            foreach (string s in keys)
    //            {
    //                GUI.Label(new Rect(300, 40 + (20 * index), 200, 20), s + ": " + Root.Instance.bdata.m_BalanceObjects[m_CurrentCharacter].floatDic[s]);
    //                Root.Instance.bdata.SetFloat(m_CurrentCharacter, s, GUI.HorizontalSlider(new Rect(500, 40 + (20 * index), 100, 20), Root.Instance.bdata.m_BalanceObjects[m_CurrentCharacter].floatDic[s], 0, 1));
    //                index++;
    //            }

    //            GUI.Label(new Rect(600, 20, 100, 20), "String Values: ");

    //            keys = new List<string>(Root.Instance.bdata.m_BalanceObjects[m_CurrentCharacter].stringDic.Keys);
    //            index = 0;
    //            foreach (string s in keys)
    //            {
    //                GUI.Label(new Rect(600, 40 + (20 * index), 200, 20), s + ": " + Root.Instance.bdata.m_BalanceObjects[m_CurrentCharacter].stringDic[s]);
    //                Root.Instance.bdata.SetString(m_CurrentCharacter, s, GUI.TextField(new Rect(800, 40 + (20 * index), 100, 20), Root.Instance.bdata.m_BalanceObjects[m_CurrentCharacter].stringDic[s], 100));
    //                index++;
    //            }

    //        }
    //    }
    //}
}
