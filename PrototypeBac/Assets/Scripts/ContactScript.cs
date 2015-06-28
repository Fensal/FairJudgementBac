using UnityEngine;
using System.Collections;

public class ContactScript : MonoBehaviour {
    [SerializeField]
    HintScript m_HintScript;
    [SerializeField]
    SwitchScript m_Switch;
    private bool m_Played = false;
    [SerializeField]
    private bool m_RequiresKey = false;
    [SerializeField]
    private int m_PassesRequired = 0;

    [SerializeField]
    GameObject m_ToActivate;
    private int m_Passes = 0;

    void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer == LayerMask.NameToLayer("CharacterBody") || other.gameObject.layer == LayerMask.NameToLayer("Character")) && m_Passes >= m_PassesRequired)
        {
            if ((m_RequiresKey || !m_Played) && !Root.Instance.m_Character.m_HasKey)
            {
                m_HintScript.ShowHint();
                m_Played = true;
            }
            

            if (Root.Instance.m_Character.m_HasKey && m_Switch != null)
            {
                m_Switch.Unlock();
                m_Switch.HitSwitch();
            }

            if (m_ToActivate != null)
            {
                m_ToActivate.SetActive(true);
            }

            m_Passes++;
        }
    }
}
