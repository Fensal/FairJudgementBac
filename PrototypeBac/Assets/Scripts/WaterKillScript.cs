using UnityEngine;
using System.Collections;

public class WaterKillScript : MonoBehaviour {
    [SerializeField]
    private PlayerCharacterController m_Character;
    [SerializeField]
    Transform m_Waterfall;

    private bool m_Deadly = true;

    void Start()
    {
        if (m_Character == null && Root.Instance != null)
        {
            m_Character = Root.Instance.m_Character;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CharacterBody") && m_Deadly)
        {
            m_Character.StartCoroutine("Die", true);
            
        }
    }

    public IEnumerator DrainWater()
    {
        float target =  transform.parent.localPosition.y - 1.2f;
        ParticleSystem p = m_Waterfall.GetComponentInChildren<ParticleSystem>();

        float sound = 1.0f;
        while (target < transform.parent.localPosition.y)
        {
            transform.parent.localPosition -= new Vector3(0.0f, 0.01f, 0.0f);
            if(m_Waterfall.localScale.x > 0.0f) m_Waterfall.localScale -= new Vector3(0.01f, 0.0f, 0.0f);
            if (p != null)
                p.Stop();

            if (Root.Instance != null)
            {
                sound -= 0.5f/120.0f;
                Root.Instance.m_Waterfall.setVolume(sound);
            }

            yield return new WaitForFixedUpdate();
        }

        transform.parent.localPosition = new Vector3(transform.parent.localPosition.x, target, transform.parent.localPosition.z);
        m_Deadly = false;
    }

    
}
