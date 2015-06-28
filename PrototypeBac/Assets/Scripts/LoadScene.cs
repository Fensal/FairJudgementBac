using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LoadScene : MonoBehaviour {
    [SerializeField]
    List<Button> m_Buttons;

    public void StartGame()
    {
        StartCoroutine(StartGameAsync());
    }


    private IEnumerator StartGameAsync()
    {
        FMOD_StudioSystem.instance.PlayOneShot("event:/Environment/button", Vector3.zero);

        //TODO:
        //fade to black
        foreach (Button b in m_Buttons)
        {
            b.interactable = false;
        }

        if (Root.Instance != null)
        {
            Root.Instance.m_LoadingScreen.gameObject.SetActive(true);
            Root.Instance.m_LoadingScreen.StartCoroutine(Root.Instance.m_LoadingScreen.FadeIn());

            Root.Instance.result = Application.LoadLevelAsync("PlayGround");

            while (Root.Instance.result != null && !Root.Instance.result.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            AsyncOperation op = Application.LoadLevelAsync("PlayGround");

            while (!op.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        Destroy(this.gameObject);
    }

    public void LoadControls()
    {
        FMOD_StudioSystem.instance.PlayOneShot("event:/Environment/button", Vector3.zero);
        Application.LoadLevelAdditive("Controls");
    }

    public void Quit()
    {
        FMOD_StudioSystem.instance.PlayOneShot("event:/Environment/button", Vector3.zero);
        Application.Quit();
    }

    public void ReturnToMainMenu()
    {
        FMOD_StudioSystem.instance.PlayOneShot("event:/Environment/button", Vector3.zero);
        Time.timeScale = 1.0f;

        if (Root.Instance != null)
        {
            Root.Instance.m_Music.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            Root.Instance.m_Waterfall.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            Root.Instance.m_Walksound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            Root.Instance.m_Music.setPaused(false);
            Root.Instance.m_Waterfall.setPaused(false);
            Root.Instance.m_Walksound.setPaused(false);
        }

        Destroy(Root.Instance.m_Character.gameObject);
        Application.LoadLevel("MainMenu");
        
        Destroy(this.gameObject);
    }

    public void Return()
    {
        FMOD_StudioSystem.instance.PlayOneShot("event:/Environment/button", Vector3.zero);
        Destroy(this.gameObject);
    }

    public void ReturnToGame()
    {
        FMOD_StudioSystem.instance.PlayOneShot("event:/Environment/button", Vector3.zero);
        Time.timeScale = 1.0f;

        if (Root.Instance != null)
        {
            Root.Instance.m_Music.setPaused(false);
            Root.Instance.m_Waterfall.setPaused(false);
            Root.Instance.m_Walksound.setPaused(false);
        }

        Destroy(this.gameObject);
    }
}
