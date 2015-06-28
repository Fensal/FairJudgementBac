using UnityEngine;
using System.Collections;

public class FixRenderTexturResolution : MonoBehaviour {
    [SerializeField]
    Material m_Mat;
    private RenderTexture m_Texture;
	void Start () {
        m_Texture = new RenderTexture(Screen.width, Screen.height, 24);

        GetComponent<Camera>().targetTexture = m_Texture;
        m_Mat.SetTexture("_CharTex", m_Texture);
	}
}
