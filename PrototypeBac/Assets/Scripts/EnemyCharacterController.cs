using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class EnemyCharacterController : BaseCharacterController, IBalancable
{
    public event EventHandler Changed;

    public Transform patrolPointLeft;
    public Transform patrolPointRight;

    public HierarchicalNGramPredictor nGramPredictor;
    //public NGramPredictor nGramPredictor;

    public float retreatTimer = 0.0f;

    public BaseEnemyType m_EnemyType;

    public string[] previousActions;
    public string[] predictionSequence;

    public bool register;

    public string[] attackSequence;
    public int attackSequenceIndex;

    private void OnChanged()
    {
        if (Changed != null)
        {
            Changed(this, EventArgs.Empty);
        }
    }

    #region Balancable Members
    private int _MaxHealth;
    public int MaxHealth
    {
        get
        {
            return _MaxHealth;
        }
        set
        {
            _MaxHealth = value;
            OnChanged();
        }
    }

    private int _Damage;
    public int Damage
    {
        get
        {
            return _Damage;
        }
        set
        {
            _Damage = value;
            OnChanged();
        }
    }

    public float Acceleration
    {
        get
        {
            return m_Acceleration;
        }
        set
        {
            m_Acceleration = value;
            OnChanged();
        }
    }

    public float Deceleration
    {
        get
        {
            return m_Deceleration;
        }
        set
        {
            m_Deceleration = value;
            OnChanged();
        }
    }

    public float MaxSpeed
    {
        get
        {
            return m_MaxSpeed;
        }
        set
        {
            m_MaxSpeed = value;
            OnChanged();
        }
    }
    #endregion
    #region Standard Members
    bool groundHit;
    RaycastHit GroundHitInfo;
    public IComboBaseState m_ComboState;

    #endregion

    void Awake()
    {
        foreach (string s in Input.GetJoystickNames())
        {
            Debug.Log(s);
        }

        _MaxHealth = 1000;
        _Damage = 10;
        MaxSpeed = 0.06f;
        Acceleration = 0.015f;
        Deceleration = 0.03f;
        m_CurrentState = new AIBattleIdle(this);
        m_ComboState = new NoInputState(this, new NoInputState(this, null));
        m_FacingRight = true;
        m_TotalForce = Vector3.zero;
        m_NGramWindowSize = 2;

        m_MaxHealth = 100.0f;
        m_CurrentHealth = 100.0f;

        nGramPredictor = new HierarchicalNGramPredictor(m_NGramWindowSize +1);
        nGramPredictor.threshold = 1;

        //nGramPredictor = new NGramPredictor();
        //nGramPredictor.nValue = m_NGramWindowSize + 1;

        //nGramPredictor.Init();

        previousActions = new string[m_NGramWindowSize + 1];
        predictionSequence = new string[m_NGramWindowSize];

        //attackSequence = new string[5];
        
        
        if (Root.Instance != null)
        {
            m_GravityForce = Root.Instance.gravity;
        }
        else
        {
            m_GravityForce = -0.01f;
        }

        m_MaxFallSpeed = -0.1f;
    }

    void Start()
    {
        if (Root.Instance != null)
        {
            Root.Instance.bdata.GetBalancing(this, this.GetType());
            m_Opponent = Root.Instance.m_Character;
        }
    }

    public override void Update()
    {
        
        if (!m_IsDead) 
        {
            base.Update();

            retreatTimer += Time.deltaTime;

            if (m_CurrentState != null)
            {
                m_CurrentState.Update();
                m_CurrentState = m_CurrentState.GetNextState();
            }

            if (m_ComboState != null)
            {
                m_ComboState.Update();
                m_ComboState = m_ComboState.GetNextState();
            }

            m_Center = collider.bounds.center;
            m_Height = collider.bounds.size.y;
            m_Width = collider.bounds.size.x;

            //ground detection
            groundHit = Physics.Linecast(m_Center, m_Center + (-transform.up * m_Height), out GroundHitInfo, 1 << LayerMask.NameToLayer("Terrain"));

            SnapFloor();

            if ((m_CurrentState is IAirborne) && groundHit)
            {
                (m_CurrentState as IAirborne).Land();
            }

            //wall detection
            bool wallHit = Physics.Linecast(m_Center, m_Center + (m_TotalForce.normalized * m_Width), out m_WallHitInfo, 1 << LayerMask.NameToLayer("Terrain"));

            bool enemyHit = Physics.Linecast(m_Center, m_Center + (m_TotalForce.normalized * m_Width * 2.5f), out m_EnemyHitInfo, 1 << LayerMask.NameToLayer("Character"));

            if (wallHit)
            {
                m_TotalForce.x = 0.0f;
            }

            if (enemyHit)
            {
                if (m_EnemyHitInfo.transform.gameObject.GetComponent<PlayerCharacterController>() != null)
                {
                    m_TotalForce.x = 0.0f;
                }
            }

            //perform linecasts to check if character clipped through wall
            RaycastHit WallHitRightInfo;
            RaycastHit WallHitLeftInfo;
            bool clippedWallHitRight = Physics.Linecast(m_Center, m_Center + (transform.right * (m_Width / 2)), out WallHitRightInfo, 1 << LayerMask.NameToLayer("Terrain"));
            bool clippedWallHitLeft = Physics.Linecast(m_Center, m_Center + (-transform.right * (m_Width / 2)), out WallHitLeftInfo, 1 << LayerMask.NameToLayer("Terrain"));

            if (clippedWallHitRight)
            {
                Debug.Log("Clipped");
                transform.position += (Vector3)(WallHitRightInfo.point - (m_Center + transform.right * (m_Width / 2)));
                //m_TotalForce = (Vector3)(clippedWallHit.point - (m_Center + (Vector2)transform.right * (m_Width / 2))); 
            }
            else if (clippedWallHitLeft)
            {

                transform.position += (Vector3)(WallHitLeftInfo.point - (m_Center + -transform.right * (m_Width / 2)));
            }

            if (!(m_CurrentState is IAirborne) && groundHit)
            {
                m_TotalForce.y = 0.0f;
                transform.position = new Vector3(transform.position.x, GroundHitInfo.point.y + m_Height / 2 - m_MaxFallSpeed, 0.0f);
            }
            else if (m_CurrentState is AIMovementPatrol && !groundHit)
            {
                (m_CurrentState as AIMovementPatrol).Drop();
            }
            else if (m_CurrentState is IAirborne)
            {
                if (m_TotalForce.y + m_GravityForce > m_MaxFallSpeed)
                {
                    m_TotalForce.y += m_GravityForce;
                }
                else
                {
                    m_TotalForce.y = m_MaxFallSpeed;
                }
            }

            //GetComponent<Animator>().speed = Math.Abs(m_TotalForce.x / m_MaxSpeed);
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, m_BaseZLevel);
    }

    void FixedUpdate()
    {
        if (!m_IsDead)
        {
            if (m_CurrentState != null)
                m_CurrentState.FixedUpdate();
            else
                Debug.LogWarning("null");
        }

        SnapFloor();

        transform.position += m_TotalForce;
    }

    public void updatePreviousActions(string action)
    {
        if (register)
        {
            for (int i = 0; i < previousActions.Length - 1; i++)
            {
                previousActions[i] = previousActions[i + 1];
            }

            previousActions[previousActions.Length - 1] = action;

            for (int i = 0; i < predictionSequence.Length; i++)
                predictionSequence[i] = previousActions[i + 1];

            if (m_Opponent != null)
            {
                EnemyCharacterController enemyScript = (EnemyCharacterController)m_Opponent;

                enemyScript.nGramPredictor.RegisterSequence(previousActions);
            }
        }
    }

    public override void GetHit(int Damage)
    {
        if (!(m_CurrentState is BaseBattleGetsHit))
        {
            FMOD_StudioSystem.instance.PlayOneShot("event:/Enemy/enemy_hurt_heavy", transform.position);
            StartCoroutine(ShowBlood());

            if(!(m_CurrentState is AIBattleCounterAttacked))
                updatePreviousActions(m_CurrentState.ToString());

            GetComponent<Animator>().SetTrigger("Hit");
            m_CurrentState = new AIBattleGetsHit(this);
            base.GetHit(Damage);
        }
    }

    IEnumerator ShowBlood()
    {
        GetComponentInChildren<ParticleSystem>().Play();

        yield return new WaitForSeconds(0.2f);

        GetComponentInChildren<ParticleSystem>().Stop();
    }

    private void SnapFloor()
    {
        if (groundHit)
        {
            transform.position = new Vector3(transform.position.x, GroundHitInfo.point.y + m_Height / 2 - m_MaxFallSpeed, transform.position.z);
        }
        else
        {
            if (m_CurrentState is MovementWalk)
            {
                Ray ray = new Ray(m_Center, -transform.up);
                groundHit = Physics.Raycast(ray, out GroundHitInfo, m_Height, 1 << LayerMask.NameToLayer("Terrain"));
                if (groundHit)
                {
                    transform.position = new Vector3(transform.position.x, GroundHitInfo.point.y + m_Height / 2 - m_MaxFallSpeed, transform.position.z);
                }
            }
        }
    }
}
