using UnityEngine;
using System.Collections;

public class SceneScript : MonoBehaviour {
    [HideInInspector]
    public string m_CurrentScene;
    [SerializeField]
    public Transform m_StartPosition;
    [SerializeField]
    public Transform m_ReturnPosition;

    float deltaTime = 0.0f;

	void Awake () {
        m_CurrentScene = gameObject.name;
        if (Root.Instance != null)
        {
            Root.Instance.m_ActiveScene.Add(m_CurrentScene);
            Debug.LogError(Root.Instance.m_ActiveScene[Root.Instance.m_ActiveScene.Count - 1]);

            Root.Instance.m_StartPosition = m_StartPosition;
            
            
            Root.Instance.m_CurrentSceneScript = this;
        }
	}

    void Start()
    {
        if (Root.Instance != null)
        {
            Root.Instance.m_Music.start();

            if (Root.Instance.m_ActiveScene[Root.Instance.m_ActiveScene.Count - 1] == "PlayGround")
            {
                Root.Instance.m_Music.setParameterValue("music_trans", 0.0f);
                Root.Instance.m_Outdoors = this.gameObject;
            }
            if (Root.Instance.m_ActiveScene[Root.Instance.m_ActiveScene.Count - 1] == "Cathedral")
            {
                Root.Instance.m_Music.setParameterValue("music_trans", 1.0f);
                Root.Instance.m_Chapel = this.gameObject;
            }
        }

        if (Root.Instance.m_Character == null)
        {
            Root.Instance.CreateCharacter();
        }
        else
        {
            Root.Instance.m_Character.transform.position = Root.Instance.m_StartPosition.position;
            Root.Instance.m_Character.m_TotalForce = Vector3.zero;
        }

        Root.Instance.m_Character.GetComponentInChildren<GenerateParticles>().m_AllowPlaneShift = 0;

        StartCoroutine(DisablePhysics());
    }

    void OnEnable()
    {
        Root.Instance.m_StartPosition = m_StartPosition;
        if (Root.Instance.m_Character == null)
        {
            Root.Instance.CreateCharacter();
        }
        else
        {
            Root.Instance.m_Character.transform.position = Root.Instance.m_StartPosition.position;
            Root.Instance.m_Character.m_TotalForce = Vector3.zero;
        }
        Root.Instance.m_CurrentSceneScript = this;

        Root.Instance.m_Character.GetComponentInChildren<GenerateParticles>().m_AllowPlaneShift = 0;

        StartCoroutine(DisablePhysics());
    }

    void OnDestroy()
    {
        if (Root.Instance != null)
        {
            Root.Instance.m_ActiveScene.RemoveAt(Root.Instance.m_ActiveScene.Count - 1);
        }
    }
	
	void Update () {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

        //FMOD.Studio.PLAYBACK_STATE r;
        //Root.Instance.m_Music.getPlaybackState(out r);
        //if (r != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        //{
        //    Root.Instance.m_Music.start();
        //}


        if (Time.timeScale != 0.0f)
        {
            if (Input.GetJoystickNames().Length > 0)
            {
                if (Input.GetButton("Start_1"))
                {
                    Time.timeScale = 0.0f;
                    if (Root.Instance != null)
                    {
                        Root.Instance.m_Music.setPaused(true);
                        Root.Instance.m_Waterfall.setPaused(true);
                        Root.Instance.m_Walksound.setPaused(true);
                    }

                    Root.Instance.m_Character.m_Overlay.transform.parent.gameObject.SetActive(false);
                    Application.LoadLevelAdditive("PauseMenu");
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.Escape))
                {
                    Time.timeScale = 0.0f;
                    if (Root.Instance != null)
                    {
                        Root.Instance.m_Music.setPaused(true);
                        Root.Instance.m_Waterfall.setPaused(true);
                        Root.Instance.m_Walksound.setPaused(true);
                    }

                    Root.Instance.m_Character.m_Overlay.transform.parent.gameObject.SetActive(false);
                    Application.LoadLevelAdditive("PauseMenu");
                }
            }
        }
        
	}

    public void ReloadScene()
    {
        Application.LoadLevel(m_CurrentScene);
    }

    private IEnumerator DisablePhysics()
    {
        
        Root.Instance.m_Character.m_ApplyForce = false;

        
        yield return new WaitForSeconds(1.0f);

        Root.Instance.m_Character.m_ApplyForce = true;
        Root.Instance.m_StartPosition = m_StartPosition;
    }

    //void OnGUI()
    //{
    //    int w = Screen.width, h = Screen.height;

    //    GUIStyle style = new GUIStyle();

    //    Rect rect = new Rect(0, 20, w, h * 2 / 100);
    //    style.alignment = TextAnchor.UpperLeft;
    //    style.fontSize = h * 2 / 100;
    //    style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
    //    float msec = deltaTime * 1000.0f;
    //    float fps = 1.0f / deltaTime;
    //    string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
    //    GUI.Label(rect, text, style);

    //    if (GUI.Button(new Rect(Screen.width - 400, 0, 200, 20), "Stop/Start Music") && Root.Instance != null)
    //    {
    //        bool fmodisannoying;
    //        Root.Instance.m_Music.getPaused(out fmodisannoying);

    //        Root.Instance.m_Music.setPaused(!fmodisannoying);
    //    }

    //    if (GUI.Button(new Rect(Screen.width - 400, 20, 200, 20), " Load Outdoors") && Root.Instance != null)
    //    {
    //        if (Root.Instance != null)
    //        {
    //            Root.Instance.m_LoadingScreen.gameObject.SetActive(true);
    //            Root.Instance.m_LoadingScreen.StartCoroutine(Root.Instance.m_LoadingScreen.FadeIn());

    //            Root.Instance.m_Outdoors.gameObject.SetActive(true);

    //            Root.Instance.m_LoadingScreen.StopAllCoroutines();
    //            Root.Instance.m_LoadingScreen.StartCoroutine(Root.Instance.m_LoadingScreen.FadeOut());
    //        }
    //    }

    //    if (GUI.Button(new Rect(Screen.width - 200, 20, 200, 20), " Load Chapel") && Root.Instance != null)
    //    {
    //        if (Root.Instance != null)
    //        {
    //            m_StartPosition = null;

    //            Root.Instance.m_Outdoors.gameObject.SetActive(false);

    //            Root.Instance.m_LoadingScreen.gameObject.SetActive(true);
    //            Root.Instance.m_LoadingScreen.StartCoroutine(Root.Instance.m_LoadingScreen.FadeIn());

    //            Root.Instance.result = Application.LoadLevelAdditiveAsync("Cathedral");
    //        }
    //    }
    //}
}
