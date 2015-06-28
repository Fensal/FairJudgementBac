using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HintScript : MonoBehaviour {
    [SerializeField]
    Image m_Image;
    bool m_IsPlaying = false;
    [SerializeField]
    float m_Duration = 3.0f;

    void Awake()
    {
        m_Image.color = new Color(m_Image.color.r, m_Image.color.g, m_Image.color.b, 0.0f);
    }
    void OnEnable()
    {
        m_Image.color = new Color(m_Image.color.r, m_Image.color.g, m_Image.color.b, 0.0f);
    }
	
	public void ShowHint(){
        StartCoroutine(DisplayHint());
    }

    IEnumerator DisplayHint()
    {
        if (!m_IsPlaying)
        {
            m_IsPlaying = true;
            while (m_Image.color.a < 1.0f)
            {
                float a = m_Image.color.a + 0.1f;
                m_Image.color = new Color(m_Image.color.r, m_Image.color.g, m_Image.color.b, a);
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(m_Duration);

            while (m_Image.color.a > 0.0f)
            {
                float a = m_Image.color.a - 0.1f;
                m_Image.color = new Color(m_Image.color.r, m_Image.color.g, m_Image.color.b, a);
                yield return new WaitForSeconds(0.1f);
            }
            m_IsPlaying = false;
        }
    }
}
