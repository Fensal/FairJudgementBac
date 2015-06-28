using UnityEngine;
using System.Collections;

public class ImageEffekt : MonoBehaviour {
    [SerializeField]
    Material Mat;


    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, null, Mat);
    }
}
