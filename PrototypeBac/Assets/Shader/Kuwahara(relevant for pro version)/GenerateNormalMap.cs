using UnityEngine;
using System.Collections;

public class GenerateNormalMap : MonoBehaviour {
    [SerializeField]
    RenderTexture tex;
    [SerializeField]
    Material normalMat;
    [SerializeField]
    Material sobelMat;


    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, sobelMat);
        //Graphics.Blit(tex, dest, sobelMat);
    }
}
