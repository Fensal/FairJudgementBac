using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class SwitchScript : MonoBehaviour
{
    [SerializeField]
    List<Animator> m_Animator;
    [SerializeField]
    List<Collider> m_Collider;

    [SerializeField]
    List<GameObject> m_ObjectsToActivate;
    [SerializeField]
    List<GameObject> m_ObjectsToDeactivate;

    [SerializeField]
    List<SwitchScript> m_Dependancies;

    [SerializeField]
    WaterKillScript m_Water;

    [SerializeField]
    bool m_AllowTrigger = true;
    [SerializeField]
    bool m_TouchActivated = false;
    bool m_Activated = false;

    [SerializeField]
    CameraMovement m_CameraScript;
    [SerializeField]
    int m_OldPath;
    [SerializeField]
    int m_NewPath;

    [SerializeField]
    Transform m_Focus;
    [SerializeField]
    float m_FocusTime = 0.0f;
    [SerializeField]
    float m_FocusSpeed = 0.1f;

    [SerializeField]
    List<string> m_Events;
    [SerializeField]
    string m_FallbackEvent;

    [SerializeField]
    Transform m_NewStartPosition;

    public void HitSwitch()
    {
        if (m_AllowTrigger && !m_Activated)
        {
            foreach(string s in m_Events)
            {
                FMOD_StudioSystem.instance.PlayOneShot("event:/Environment/" + s, Vector3.zero);
            }

            foreach (Animator a in m_Animator)
            {
                a.SetTrigger("Trigger");
            }


            foreach (Collider c in m_Collider)
            {
                c.enabled = false;
            }

            foreach (GameObject g in m_ObjectsToActivate)
            {
                g.SetActive(true);
            }

            foreach (GameObject g in m_ObjectsToDeactivate)
            {
                g.SetActive(false);
            }

            if (m_Water != null)
            {
                m_Water.StartCoroutine(m_Water.DrainWater());
            }

            foreach (SwitchScript s in m_Dependancies)
            {
                s.Unlock();
            }

            if (m_Focus != null)
            {
                m_CameraScript.StartCoroutine(m_CameraScript.FocusLerp(m_Focus, m_FocusTime, m_FocusSpeed));
            }

            if (Root.Instance != null && m_NewStartPosition != null)
            {
                Root.Instance.m_StartPosition = m_NewStartPosition;
                //Root.Instance.m_CurrentSceneScript.m_StartPosition = m_NewStartPosition;
            }

            StartCoroutine(SwitchPaths());

            m_Activated = true;
        }
        else if (!m_AllowTrigger && !m_Activated)
        {
            foreach (Animator a in m_Animator)
            {
                a.SetTrigger("TryTrigger");
            }

            if (m_FallbackEvent != "")
            {
                FMOD_StudioSystem.instance.PlayOneShot("event:/Environment/" + m_FallbackEvent, Vector3.zero);
            }
        }
    }

    public void Unlock()
    {
        m_AllowTrigger = true;
    }

    IEnumerator SwitchPaths()
    {
        yield return new WaitForSeconds(2.5f);
        if (m_CameraScript && m_OldPath + m_NewPath != 0)
        {
            m_CameraScript.SwitchPaths(m_OldPath, m_NewPath);
        }
    }

    public void Activate()
    {
        if (m_TouchActivated)
        {
            HitSwitch();
        }
    }
}
