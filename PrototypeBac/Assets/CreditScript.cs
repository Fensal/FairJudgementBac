using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreditScript : MonoBehaviour
{
    [SerializeField]
    Image m_Background;
    [SerializeField]
    Image m_Credits1;

    void Awake()
    {
        m_Background.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        m_Credits1.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        

        StartCoroutine(ShowCredits());
    }

    IEnumerator ShowCredits()
    {
        while (m_Background.color.a < 1.0f)
        {
            m_Background.color = new Color(m_Background.color.r, m_Background.color.g, m_Background.color.b, m_Background.color.a+0.01f);
            yield return new WaitForFixedUpdate();
        }

        Application.LoadLevelAdditive("MainMenu");

        while (m_Credits1.color.a < 1.0f)
        {
            m_Credits1.color = new Color(m_Credits1.color.r, m_Credits1.color.g, m_Credits1.color.b, m_Credits1.color.a + 0.01f);
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(5.0f);

        while (m_Credits1.color.a > 0.0f)
        {
            m_Credits1.color = new Color(m_Credits1.color.r, m_Credits1.color.g, m_Credits1.color.b, m_Credits1.color.a - 0.01f);
            yield return new WaitForFixedUpdate();
        }

        while (m_Background.color.a > 0.0f)
        {
            m_Background.color = new Color(m_Background.color.r, m_Background.color.g, m_Background.color.b, m_Background.color.a - 0.01f);
            yield return new WaitForFixedUpdate();
        }

        Destroy(this.gameObject);
    }
}
