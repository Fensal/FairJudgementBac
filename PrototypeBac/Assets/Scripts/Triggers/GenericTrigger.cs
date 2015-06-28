using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenericTrigger : MonoBehaviour {
    [SerializeField]
    private Animator m_Animator;
    [SerializeField]
    private WaterKillScript m_WaterScript;
    [SerializeField]
    public float m_SwitchTime;
    [SerializeField]
    private PlayerCharacterController m_Character;

    private bool m_SwitchOn = false;

    void Awake()
    {
        if (m_Character == null && Root.Instance != null)
        {
            m_Character = Root.Instance.m_Character;
        }
    }


    public virtual IEnumerator ActivateSwitch()
    {
        if (!m_SwitchOn)
        {
            m_Animator.SetTrigger("Close");
            //m_Animator.SetTrigger("TryClose");

            m_SwitchOn = true;
            m_WaterScript.StartCoroutine("DrainWater");
            float StartTime = Time.deltaTime;
            while (Time.deltaTime - StartTime <= m_SwitchTime)
            {
                yield return new WaitForEndOfFrame();
            }
            m_SwitchOn = false;
            //player Leave Animation
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Character")
        {
            m_Character.m_Trigger = this;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Character")
        {
            m_Character.m_Trigger = null;
        }
    }
}
