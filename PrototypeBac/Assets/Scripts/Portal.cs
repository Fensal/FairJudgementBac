using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{
    [SerializeField]
    GameObject m_Scene;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            StartCoroutine(ShiftBack());
        }
    }

    IEnumerator ShiftBack()
    {
        if (Root.Instance != null)
        {
            Root.Instance.m_LoadingScreen.gameObject.SetActive(true);
            Root.Instance.m_LoadingScreen.StartCoroutine(Root.Instance.m_LoadingScreen.FadeIn(false));
            yield return new WaitForSeconds(2.0f);

            Root.Instance.m_Chapel.SetActive(false);
            //Destroy(m_Scene);

            Root.Instance.m_Outdoors.SetActive(true);

            Root.Instance.m_LoadingScreen.StopAllCoroutines();
            Root.Instance.m_LoadingScreen.StartCoroutine(Root.Instance.m_LoadingScreen.FadeOut());
        }
    }
}
