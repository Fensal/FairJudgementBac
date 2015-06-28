using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

using System.Linq.Expressions;
using System.Reflection;

using System.Diagnostics;

using UnityEngine;
//using FMOD.Studio;


class Root : MonoBehaviour
{
    public BalanceData bdata = new BalanceData();

    public float gravity;
    public AsyncOperation result = null;
    public FMOD.Studio.EventInstance m_Music;
    public FMOD.Studio.EventInstance m_Waterfall;
    public FMOD.Studio.EventInstance m_Walksound;
    public FMOD.Studio.EventInstance m_ParticleSound;
    public List<string> m_ActiveScene;
    public SceneScript m_CurrentSceneScript;
    public GameObject m_Outdoors;
    public GameObject m_Chapel;
    public LoadingScreenScript m_LoadingScreen;

    public Transform m_StartPosition;
    [SerializeField]
    GameObject m_CharacterPrefab;

    GameObject m_CharacterObject;
    public PlayerCharacterController m_Character;

    private static Root _Instance = null;
    public static Root Instance
    {
        get
        {
            return _Instance;
        }
    }

    void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);

        Application.LoadLevelAdditive("LoadingScreen");

        gravity = -0.01f;

        Application.LoadLevelAdditiveAsync("MainMenu");

        
    }

    void Start()
    {
        if (m_Music == null)
        {
            m_Music = FMOD_StudioSystem.instance.GetEvent("event:/music/music");
        }

        if (m_Waterfall == null)
        {
            m_Waterfall = FMOD_StudioSystem.instance.GetEvent("event:/Environment/waterfall");
        }

        if (m_Walksound == null)
        {
            m_Walksound = FMOD_StudioSystem.instance.GetEvent("event:/Darien/darien_waterwalk");
        }

        if (m_ParticleSound == null)
        {
            m_ParticleSound = FMOD_StudioSystem.instance.GetEvent("event:/Darien/particles");
        }
    }

    public void CreateCharacter()
    {
        if (m_Character == null)
        {
            m_CharacterObject = Instantiate(m_CharacterPrefab, m_StartPosition.position, Quaternion.identity) as GameObject;
            m_CharacterObject.transform.parent = this.transform;
            m_Character = m_CharacterObject.GetComponent<PlayerCharacterController>();
        }
    }

    void Update()
    {
        if (result != null && result.isDone)
        {
            if (m_LoadingScreen.gameObject.activeSelf)
            {
                m_LoadingScreen.StopAllCoroutines(); ;
                m_LoadingScreen.StartCoroutine(m_LoadingScreen.FadeOut());
            }

            result = null;
        }
    }

    public void SetBattleMusic()
    {
        FMOD.Studio.ParameterInstance p;
        float fmodisstillannoying;
        m_Music.getParameter("music_trans", out p);
        p.getValue(out fmodisstillannoying);
        if (fmodisstillannoying != 3.0f)
        {
            m_Music.setParameterValue("music_trans", 2.0f);
        }
        
    }

    public void SetNormalMusic()
    {
        if(m_Music != null)
            m_Music.setParameterValue("music_trans", 3.0f);

        StartCoroutine(WaitForBattleEnd());
    }

    IEnumerator WaitForBattleEnd()
    {
        yield return new WaitForSeconds(6.0f);

        if (m_CurrentSceneScript != null)
        {
            if (m_CurrentSceneScript.gameObject.name == "PlayGround")
            {
                if (m_Music != null)
                    m_Music.setParameterValue("music_trans", 0.0f);
            }
            if (m_CurrentSceneScript.gameObject.name == "Cathedral")
            {
                if (m_Music != null)
                    m_Music.setParameterValue("music_trans", 1.0f);
            }
        }
        
    }


    public static string Check<T>(Expression<Func<T>> expr)
    {
        var body = ((MemberExpression)expr.Body);
        return body.Member.Name;
    }
}
