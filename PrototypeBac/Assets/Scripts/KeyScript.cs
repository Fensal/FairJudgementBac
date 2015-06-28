using UnityEngine;
using System.Collections;

public class KeyScript : MonoBehaviour {

    [SerializeField]
    HintScript m_Hint;
    [SerializeField]
    bool m_Collected = false;


    void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer == LayerMask.NameToLayer("CharacterBody") || other.gameObject.layer == LayerMask.NameToLayer("Character")) && !m_Collected)
        {
            Animator a = GetComponent<Animator>();
            if (a != null)
            {
                a.SetTrigger("Trigger");
                FMOD_StudioSystem.instance.PlayOneShot("event:/Environment/key_pickup", Vector3.zero);
                Root.Instance.m_Character.m_HasKey = true;
                if (m_Hint != null)
                {
                    m_Hint.ShowHint();
                }
            }
            m_Collected = true;
        }
    }
}
