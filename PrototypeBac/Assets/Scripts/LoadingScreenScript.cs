using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingScreenScript : MonoBehaviour
{
    [SerializeField]
    Image m_Image;
    private Sprite m_Sprite;
    [SerializeField]
    Camera m_Camera;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (Root.Instance != null)
        {
            Root.Instance.m_LoadingScreen = this;
        }
        m_Sprite = m_Image.sprite;

        this.gameObject.SetActive(false);
	}

    void Update()
    {
        if (Root.Instance.m_Outdoors != null && m_Camera.enabled == true)
        {
            m_Camera.enabled = false;
        }
    }

    public IEnumerator FadeIn(bool m_LevelTransition = true)
    {
        if (!m_LevelTransition)
        {
            m_Image.sprite = null;
            m_Image.color = Color.black;
        }
        else
        {
            m_Image.sprite = m_Sprite;
            m_Image.color = Color.white;
        }
        m_Image.color = new Color(m_Image.color.r, m_Image.color.g, m_Image.color.b, 0.0f);

        while(m_Image.color.a < 1.0f)
        {
            float a = m_Image.color.a + 0.1f;
            m_Image.color = new Color(m_Image.color.r, m_Image.color.g, m_Image.color.b, a);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator FadeOut()
    {
        m_Image.color = new Color(m_Image.color.r, m_Image.color.g, m_Image.color.b, 1.0f);

        while (m_Image.color.a > 0.0f)
        {
            float a = m_Image.color.a - 0.1f;
            m_Image.color = new Color(m_Image.color.r, m_Image.color.g, m_Image.color.b, a);
            yield return new WaitForSeconds(0.1f);
        }

        this.gameObject.SetActive(false);
    }
}
