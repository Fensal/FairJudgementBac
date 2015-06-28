using UnityEngine;
using System.Collections;

public class ApplyTexToScreen : MonoBehaviour {
    [SerializeField]
    RenderTexture tex;
    [SerializeField]
    Material mat;


    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(tex, dest, mat);
    }
}
