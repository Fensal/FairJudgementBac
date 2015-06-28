using UnityEngine;
using System.Collections;

public class DepthScript : MonoBehaviour
{
    public Shader shader;
    private RenderTexture m_Texture;
    [SerializeField]
    private Material m_Mat;
    public void Awake()
    {
        transform.camera.depthTextureMode = DepthTextureMode.Depth;
        //transform.camera.targetTexture.format = RenderTextureFormat.Depth;
        if (shader)
            transform.camera.SetReplacementShader(shader, null);
    }
}