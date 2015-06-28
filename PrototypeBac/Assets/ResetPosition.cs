using UnityEngine;
using System.Collections;

public class ResetPosition : MonoBehaviour {
    void OnTriggerEnter(Collider other)
    {
        Root.Instance.m_Character.transform.position = Root.Instance.m_StartPosition.position;
    }
}
