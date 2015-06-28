using UnityEngine;
using System.Collections;
using System;

public class GenerateParticles : MonoBehaviour {
    [SerializeField]
    public GameObject m_PlaneShiftCircle;
    [SerializeField]
    public GameObject m_CurrentPlaneCircle;
    [SerializeField]
    public GameObject m_GhostPrefab;
    [SerializeField]
    public PlayerCharacterController m_Character;
    [SerializeField]
    public CameraMovement m_CameraScript;
    [HideInInspector]
    public uint m_AllowPlaneShift = 0;

    private GameObject m_SpawnLocation = null;
    private GameObject m_CurrentLocation = null;
    private bool m_BlockInput = false;
    private RaycastHit m_Hit;
    private GameObject m_Ghost;

    void Awake()
    {
        if (m_Character == null && Root.Instance != null)
        {
            m_Character = Root.Instance.m_Character;
        }
    }

    public void Update()
    {
        if (m_AllowPlaneShift != 0 && !(m_Character.m_CurrentState is MovementPlaneShift))
        {
            Vector3 ParticlePosition;
            if (m_CameraScript.m_BackPlane)
            {
                ParticlePosition = new Vector3(transform.position.x, transform.position.y, 0.0f);
            }
            else
            {
                ParticlePosition = transform.position;
            }
            Physics.Raycast(ParticlePosition, -Vector3.up, out m_Hit, 1 << LayerMask.NameToLayer("Terrain"));

            if (m_Hit.collider != null && m_Ghost == null)
            {
                m_Ghost = Instantiate(m_GhostPrefab, m_Hit.point + new Vector3(0.0f, m_Character.m_Height/2, 0.0f), Quaternion.FromToRotation(Vector3.up, m_Hit.normal)) as GameObject;
            }
            else if (m_Hit.collider != null)
            {
                m_Ghost.transform.position = m_Hit.point + new Vector3(0.0f, m_Character.m_Height / 2, 0.0f);
                if (m_Character.m_FacingRight)
                {
                    m_Ghost.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                else
                {
                    m_Ghost.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                m_Ghost.transform.rotation = Quaternion.FromToRotation(Vector3.up, m_Hit.normal);
                
                m_Ghost.GetComponent<Animator>().SetFloat("VelocityX", Math.Abs(m_Character.m_TotalForce.x));
            }
        }
        else
        {
            Destroy(m_Ghost);
            m_Ghost = null;
        }
        
    }

    public void HighlightPosition()
    {
        if (m_AllowPlaneShift == 3)
        {
            if (m_CurrentLocation == null)
            {
                m_CurrentLocation = Instantiate(m_CurrentPlaneCircle, m_Character.transform.position - new Vector3(0.0f, m_Character.m_Height / 2 - 0.4f, m_Character.m_Width / 2 + 0.1f), Quaternion.identity) as GameObject;
                m_CurrentLocation.transform.parent = m_Character.transform;
            }

            if (Root.Instance.m_ParticleSound != null)
            {
                Root.Instance.m_ParticleSound.start();
            }
        }
        else if (m_AllowPlaneShift != 0)
        {
            Vector3 ParticlePosition;
            if (m_CameraScript.m_BackPlane)
            {
                ParticlePosition = new Vector3(transform.position.x, transform.position.y, 0.0f);
            }
            else
            {
                ParticlePosition = transform.position;
            }

            Physics.Raycast(ParticlePosition, -Vector3.up, out m_Hit, 1 << LayerMask.NameToLayer("Terrain"));

            if (m_Hit.collider != null && m_SpawnLocation == null && m_CurrentLocation == null)
            {
                m_SpawnLocation = Instantiate(m_PlaneShiftCircle, m_Hit.point, Quaternion.FromToRotation(Vector3.up, m_Hit.normal)) as GameObject;

                m_CurrentLocation = Instantiate(m_CurrentPlaneCircle, m_Character.transform.position - new Vector3(0.0f, m_Character.m_Height / 2 - 0.4f, m_Character.m_Width / 2 + 0.1f), Quaternion.identity) as GameObject;
                m_CurrentLocation.transform.parent = m_Character.transform;
            }

            if (Root.Instance.m_ParticleSound != null)
            {
                Root.Instance.m_ParticleSound.start();
            }
        }
    }

    public void DestroyPosition()
    {
        if (m_SpawnLocation != null)
        {
            ParticleSystem SpawnSystem = m_SpawnLocation.GetComponentInChildren<ParticleSystem>();
            SpawnSystem.Stop();

            Destroy(m_SpawnLocation, 0.75f);
            
            m_SpawnLocation = null;
        }

        if (m_CurrentLocation != null)
        {
            ParticleSystem CurrentSystem = m_CurrentLocation.GetComponentInChildren<ParticleSystem>();
            CurrentSystem.Stop();

            Destroy(m_CurrentLocation, 0.75f);

            m_CurrentLocation = null;
        }

        if (Root.Instance.m_ParticleSound != null)
        {
            Root.Instance.m_ParticleSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }

    public void MoveCamera(MovementPlaneTransition State)
    {
        if (!m_BlockInput && m_AllowPlaneShift == 2)
        {
            StartCoroutine("Move", State);
        }
        else if (!m_BlockInput && m_AllowPlaneShift == 1)
        {
            StartCoroutine("MoveWithoutCamera", State);
        }
        else if (!m_BlockInput && m_AllowPlaneShift == 3)
        {
            StartCoroutine("ChangeScene", State);
        }
        else if(!m_BlockInput && m_AllowPlaneShift == 4)
        {
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
            Application.LoadLevel("Credits");
        }
        else if (!m_BlockInput)
        {
            State.FinishTransition();
        }
    }

    public IEnumerator Move(MovementPlaneTransition State)
    {
        Vector3 ParticlePosition;
        if (m_CameraScript.m_BackPlane)
        {
            ParticlePosition = new Vector3(transform.position.x, transform.position.y, 0.0f);
        }
        else
        {
            ParticlePosition = transform.position;
        }

        Physics.Raycast(ParticlePosition, -Vector3.up, out m_Hit, 1 << LayerMask.NameToLayer("Terrain"));

        m_BlockInput = true;

        yield return m_CameraScript.StartCoroutine("Move", m_Hit.point);

        m_BlockInput = false;


        DestroyPosition();
        State.FinishTransition();
    }

    public IEnumerator MoveWithoutCamera(MovementPlaneTransition State)
    {
        Vector3 ParticlePosition;
        if (m_CameraScript.m_BackPlane)
        {
            ParticlePosition = new Vector3(transform.position.x, transform.position.y, 0.0f);
        }
        else
        {
            ParticlePosition = transform.position;
        }

        Physics.Raycast(ParticlePosition, -Vector3.up, out m_Hit, 1 << LayerMask.NameToLayer("Terrain"));

        m_BlockInput = true;

        yield return m_CameraScript.StartCoroutine("MoveWithoutCamera", m_Hit.point);

        m_BlockInput = false;

        DestroyPosition();
        State.FinishTransition();
    }

    public IEnumerator ChangeScene(MovementPlaneTransition State)
    {
        if (Root.Instance != null)
        {
            m_BlockInput = true;

            yield return new WaitForEndOfFrame();

            if (Root.Instance.m_CurrentSceneScript.m_ReturnPosition != null)
            {
                Root.Instance.m_CurrentSceneScript.m_StartPosition = Root.Instance.m_CurrentSceneScript.m_ReturnPosition;
            }



            Root.Instance.m_LoadingScreen.gameObject.SetActive(true);
            Root.Instance.m_LoadingScreen.StartCoroutine(Root.Instance.m_LoadingScreen.FadeIn(false));

            yield return new WaitForSeconds(1.0f);


            if (Root.Instance.m_Outdoors != null)
            {
                Root.Instance.m_Outdoors.gameObject.SetActive(false);
            }

            
            if (Root.Instance.m_Chapel == null)
            {
                Root.Instance.result = Application.LoadLevelAdditiveAsync("Cathedral");

                while (Root.Instance.result != null && !Root.Instance.result.isDone)
                {

                    yield return new WaitForFixedUpdate();
                }
            }
            else
            {
                Root.Instance.m_Chapel.SetActive(true);

                yield return new WaitForSeconds(2.0f);
            }
            
            
            Root.Instance.m_LoadingScreen.StartCoroutine(Root.Instance.m_LoadingScreen.FadeOut());

            DestroyPosition();
            State.FinishTransition();

            m_BlockInput = false;
        }
    }
}
