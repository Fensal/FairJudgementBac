using UnityEngine;
using System.Collections;

public class DepthScript2 : MonoBehaviour
{
    public Shader shader;
    private RenderTexture m_Texture;
    [SerializeField]
    private Material m_Mat;
    public void Awake()
    {
        m_Texture = new RenderTexture(Screen.width, Screen.height, 24);
        camera.targetTexture = m_Texture;
        if (m_Mat != null)
        {
            m_Mat.SetTexture("_CharTex", m_Texture);
        }

        transform.camera.depthTextureMode = DepthTextureMode.Depth;
        //transform.camera.targetTexture.format = RenderTextureFormat.Depth;
        if (shader)
            transform.camera.SetReplacementShader(shader, null);
    }
}