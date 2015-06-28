using UnityEngine;
using System.Collections;

public class ImageToTex : MonoBehaviour
{
    [SerializeField]
    Material Mat;


    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, Mat);
    }
}
