using UnityEngine;
using System.Collections;

public class ActivateSwitch : MonoBehaviour
{    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CharacterBody") || other.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            SwitchScript s = GetComponentInParent<SwitchScript>();

            if (s != null)
            {
                s.Activate();
            }
            else
            {
                s = transform.parent.GetComponentInParent<SwitchScript>();

                if (s != null)
                {
                    s.Activate();
                }
                else
                {
                    Animator a = GetComponent<Animator>();
                    if (a != null)
                    {
                        a.SetTrigger("Trigger");
                        Root.Instance.m_Character.m_HasKey = true;
                    }
                }
            }
        }
    }
}
