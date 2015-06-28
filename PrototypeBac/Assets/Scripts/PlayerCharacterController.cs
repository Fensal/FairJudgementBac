using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class PlayerCharacterController : BaseCharacterController, IBalancable
{
    public event EventHandler Changed;

    public string[] previousActions;
    public string[] predictionSequence;

    private float m_JumpTimer = 0.0f;

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

    [HideInInspector]
    public IComboBaseState m_ComboState;
    [SerializeField]
    public Transform m_BackgroundPosition;
    [SerializeField]
    public SceneScript m_Scenescript;
    [HideInInspector]
    public GenericTrigger m_Trigger;

    [SerializeField]
    public Image m_Overlay;

    bool groundHit;
    public bool m_HasKey = false;
    public bool m_ApplyForce = true;
    public bool m_AllowFlash = true;
    RaycastHit GroundHitInfo;
    public bool m_StandsInWater = false;

    #endregion

    void Awake()
    {
        foreach (string s in Input.GetJoystickNames())
        {
            Debug.Log(s);
        }
        
        _MaxHealth = 100;
        _Damage = 10;
        MaxSpeed = 0.04f;
        Acceleration = 0.015f;
        Deceleration = 0.03f;
        m_CurrentState = new MovementPreBegin(this);
        m_ComboState = new NoInputState(this, new NoInputState(this, null));
        m_FacingRight = true;
        m_TotalForce = Vector3.zero;
        slopeVector = Vector3.zero;
        m_NGramWindowSize = 2;
        m_BaseZLevel = 0.0f;

        //STATS
        m_MaxHealth = 100.0f;
        m_CurrentHealth = 100.0f;

        previousActions = new string[m_NGramWindowSize + 1];
        predictionSequence = new string[m_NGramWindowSize];
        
        m_GravityForce = -0.01f;

        m_MaxFallSpeed = -0.1f;
    }

    void Start()
    {

        if(Root.Instance != null)
        {
            Root.Instance.bdata.GetBalancing(this, this.GetType());
        }  
    }

    public override void Update()
    {
        m_JumpTimer += Time.deltaTime;

        if (m_CurrentState != null)
        {
            if (Input.GetJoystickNames().Length > 0)
            {
                m_CurrentState.m_HorizontalAxis = Input.GetAxis("L_XAxis_1") + Input.GetAxis("DPad_XAxis_1");
                m_CurrentState.m_VerticalAxis = Input.GetAxis("L_YAxis_1") + Input.GetAxis("DPad_YAxis_1");
                m_CurrentState.m_JumpButton = Input.GetButton("A_1");

                m_CurrentState.m_LightAttackButton = Input.GetButton("X_1");
                m_CurrentState.m_HeavyAttackButton = Input.GetButton("Y_1");
                m_CurrentState.m_BlockButton = Input.GetButton("B_1");
                m_CurrentState.m_BlockButtonUp = Input.GetButtonUp("B_1");
                m_CurrentState.m_PlaneShiftButton = Input.GetAxis("TriggersL_1");

                if (Input.GetAxis("TriggersR_1") > 0.0f && m_Trigger != null)
                {
                    m_Trigger.StartCoroutine("ActivateSwitch");
                }
            } 
            else
            {
                m_CurrentState.m_HorizontalAxis = Input.GetAxis("Horizontal");
                m_CurrentState.m_VerticalAxis = Input.GetAxis("Vertical");
                m_CurrentState.m_JumpButton = Input.GetButton("Jump");
                m_CurrentState.m_LightAttackButton = Input.GetButton("LightAttack");
                m_CurrentState.m_HeavyAttackButton = Input.GetButton("HeavyAttack");
                m_CurrentState.m_PlaneShiftButton = Input.GetAxis("PlaneShift");

                m_CurrentState.m_BlockButton = Input.GetButton("B_1");
                m_CurrentState.m_BlockButtonUp = Input.GetButtonUp("B_1");

                if (Input.GetAxis("ActivateSwitch") > 0.0f && m_Trigger != null)
                {
                    m_Trigger.StartCoroutine("ActivateSwitch");
                }
            }

            if(m_CurrentState.m_JumpButton && m_JumpTimer > 0.5f)
            {
                m_JumpTimer = 0.0f;
                m_CurrentState.m_JumpButton = true;
            }
            else
            {
                m_CurrentState.m_JumpButton = false;
            }

            m_CurrentState.Update();
            m_CurrentState = m_CurrentState.GetNextState();
        }

        if (m_ComboState != null)
        {
            m_ComboState.Update();
            m_ComboState = m_ComboState.GetNextState();
        }

        base.Update();


        m_Center = collider.bounds.center;
        m_Height = collider.bounds.size.y;
        m_Width = collider.bounds.size.x;

        Vector3 leftPoint = new Vector3(collider.bounds.min.x, m_Center.y, transform.position.z);
        Vector3 rightPoint = new Vector3(collider.bounds.max.x, m_Center.y, transform.position.z);

        Vector3 leftRaycastOrigin = Vector3.Lerp(leftPoint, rightPoint, 0.2f);
        Vector3 rightRaycastOrigin = Vector3.Lerp(leftPoint, rightPoint, 0.8f);


        Ray rayLeft = new Ray(leftRaycastOrigin, -transform.up);
        Ray rayRight = new Ray(rightRaycastOrigin, -transform.up);
        Ray rayMiddle = new Ray(m_Center, -transform.up);

        //Debug.DrawRay(leftRaycastOrigin, -transform.up, Color.red);
        //Debug.DrawRay(rightRaycastOrigin, -transform.up, Color.red);

        if (Physics.Raycast(rayLeft, out GroundHitInfo, m_Height / 2, 1 << LayerMask.NameToLayer("Terrain")) ||
            Physics.Raycast(rayRight, out GroundHitInfo, m_Height / 2, 1 << LayerMask.NameToLayer("Terrain")) ||
            Physics.Raycast(rayMiddle, out GroundHitInfo, m_Height / 2, 1 << LayerMask.NameToLayer("Terrain")))
        {
            groundHit = true;
        }
        else
        {
            groundHit = false;
        }

        if (!(m_CurrentState is MovementClimb) && !(m_CurrentState is MovementGrabTop) && !(m_CurrentState is MovementPlaneTransition) && !m_IsDead && m_ApplyForce)
        {
            SnapFloor();
        }

        //check if ground is slope
        slopeVector = Vector3.Cross(GroundHitInfo.normal, transform.forward);

        if ((m_CurrentState is IAirborne) && groundHit)
        {
            (m_CurrentState as IAirborne).Land();
        }


        Vector3 topPoint = new Vector3(m_Center.x, collider.bounds.max.y, transform.position.z);
        Vector3 bottomPoint = new Vector3(m_Center.x, collider.bounds.min.y, transform.position.z);

        Vector3 topRaycastOrigin = Vector3.Lerp(topPoint, bottomPoint, 0.2f);
        Vector3 bottomRaycastOrigin = Vector3.Lerp(topPoint, bottomPoint, 0.6f);


        Ray rayTop = new Ray(topRaycastOrigin, m_TotalForce.normalized);
        Ray rayBottom = new Ray(bottomRaycastOrigin, m_TotalForce.normalized);
        Ray rayMid = new Ray(m_Center, m_TotalForce.normalized);

        //Debug.DrawRay(leftRaycastOrigin, -transform.up, Color.red);
        //Debug.DrawRay(rightRaycastOrigin, -transform.up, Color.red);

        bool wallHit = false;

        if (Physics.Raycast(rayTop, out m_WallHitInfo, m_Width, 1 << LayerMask.NameToLayer("Terrain")) ||
            Physics.Raycast(rayBottom, out m_WallHitInfo, m_Width, 1 << LayerMask.NameToLayer("Terrain")) ||
            Physics.Raycast(rayMid, out m_WallHitInfo, m_Width, 1 << LayerMask.NameToLayer("Terrain")))
        {
            wallHit = true;
        }
        else
        {
            wallHit = false;
        }

        //wall detection
        //bool wallHit = Physics.Linecast(m_Center, m_Center +  (m_TotalForce.normalized * m_Width), out m_WallHitInfo, 1 << LayerMask.NameToLayer("Terrain"));

        bool enemyHit = Physics.Linecast(m_Center, m_Center + (m_TotalForce.normalized * m_Width * 2.15f), out m_EnemyHitInfo, 1 << LayerMask.NameToLayer("Character"));

        if (wallHit)
        {
            m_TotalForce.x = 0.0f;
        }

        if (enemyHit)
        { 
            if (m_EnemyHitInfo.transform.gameObject.GetComponent<EnemyCharacterController>() != null && !(m_CurrentState is BattleDashThrough))
            {
                m_TotalForce.x = 0.0f;
            }
        }

        //perform linecasts to check if character clipped through wall
        RaycastHit WallHitRightInfo;
        RaycastHit WallHitLeftInfo;
        bool clippedWallHitRight = Physics.Linecast(m_Center, m_Center + (transform.right * (m_Width / 2)), out WallHitRightInfo, 1 << LayerMask.NameToLayer("Terrain"));
        bool clippedWallHitLeft = Physics.Linecast(m_Center, m_Center + (-transform.right * (m_Width / 2)), out WallHitLeftInfo, 1 << LayerMask.NameToLayer("Terrain"));
        Debug.DrawLine(m_Center, m_Center + (m_TotalForce.normalized * (m_Width / 2)));

        if (clippedWallHitRight)
        {

            transform.position += (Vector3)(WallHitRightInfo.point - (m_Center + transform.right * (m_Width / 2)));
        }
        else if (clippedWallHitLeft)
        {
            Debug.Log("Clipped");

            transform.position += (Vector3)(WallHitLeftInfo.point - (m_Center + -transform.right * (m_Width / 2)));
        }


        if (!(m_CurrentState is IAirborne) && groundHit)
        {
            m_TotalForce.y = 0.0f;
            transform.position.Set(transform.position.x, GroundHitInfo.point.y + m_Height / 2 - m_MaxFallSpeed, m_BaseZLevel);
        }
        else if (m_CurrentState is MovementWalk && !groundHit)
        {
            (m_CurrentState as MovementWalk).Drop();
        }
        else if (m_CurrentState is MovementIdle && !groundHit)
        {
            (m_CurrentState as MovementIdle).Drop();
        }
        else if (m_CurrentState is BattleWalk && !groundHit)
        {
            (m_CurrentState as BattleWalk).Drop();
        }
        else if (m_CurrentState is BattleIdle && !groundHit)
        {
            (m_CurrentState as BattleIdle).Drop();
        }

        if (m_Opponent != null)
        {
            if (Root.Instance != null)
            {
                Root.Instance.SetBattleMusic();
            }

            Animator a = GetComponent<Animator>();
            if (a != null)
            {
                a.SetFloat("BattleState", 1.0f);
            }
        }
        else
        {
            if (Root.Instance != null)
            {
                Root.Instance.SetNormalMusic();
            }

            Animator a = GetComponent<Animator>();
            if (a != null)
            {
                a.SetFloat("BattleState", 0.0f);
            }
        }
    }

    void FixedUpdate()
    {
        if (m_CurrentState is IAirborne)
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

        if (Time.timeScale != 0.0f)
        {
            m_Overlay.transform.parent.gameObject.SetActive(true);
        }


        m_CurrentState.FixedUpdate();

        if (!(m_CurrentState is MovementClimb) && !(m_CurrentState is MovementGrabTop) && !(m_CurrentState is MovementPlaneTransition) && !m_IsDead && m_ApplyForce)
        {
            SnapFloor();
        }

        if(m_Opponent == null && m_CurrentHealth < m_MaxHealth){
            m_CurrentHealth++;
        }


        if (m_ApplyForce && !m_IsDead)
        {
            transform.position += (slopeVector * m_TotalForce.x);
            transform.position += new Vector3(0f, m_TotalForce.y, 0f);
        }
        

    }

    public void updatePreviousActions(string action)
    {
        for(int i=0; i < previousActions.Length - 1; i++)
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

    public IEnumerator Die(bool instant = true)
    {
        if (!instant)
        {
            yield return new WaitForSeconds(1.0f);
        }
    }

    public override void GetHit(int Damage)
    {
        if (!(m_CurrentState is BaseBattleGetsHit))
        {
            StartCoroutine(FlashOverlay(new Color(0.25f, 0.0f, 0.0f, 0.0f)));

            FMOD_StudioSystem.instance.PlayOneShot("event:/Darien/darien_hurt_heavy", transform.position);

            GetComponent<Animator>().SetTrigger("Hit");
            m_CurrentState = new BattleGetsHit(this);
            base.GetHit(Damage);
        }
    }

    public IEnumerator FlashOverlay(Color c)
    {
        if (m_AllowFlash)
        {
            m_AllowFlash = false;
            m_Overlay.color = new Color(c.r, c.g, c.b, 0.0f);

            while (m_Overlay.color.a < 0.8f)
            {
                m_Overlay.color = new Color(m_Overlay.color.r, m_Overlay.color.g, m_Overlay.color.b, m_Overlay.color.a + 0.1f);
                if (m_Overlay.color.a >= 0.8f)
                    m_Overlay.color = new Color(m_Overlay.color.r, m_Overlay.color.g, m_Overlay.color.b, 0.8f);
                yield return new WaitForFixedUpdate();
            }

            while (m_Overlay.color.a > 0.0f)
            {
                m_Overlay.color = new Color(m_Overlay.color.r, m_Overlay.color.g, m_Overlay.color.b, m_Overlay.color.a - 0.1f);
                if (m_Overlay.color.a <= 0.0f)
                    m_Overlay.color = new Color(m_Overlay.color.r, m_Overlay.color.g, m_Overlay.color.b, 0.0f);
                yield return new WaitForFixedUpdate();
            }
            m_Overlay.color = new Color(m_Overlay.color.r, m_Overlay.color.g, m_Overlay.color.b, 0.0f);
            m_AllowFlash = true;
        }
        else
        {
           yield return null;
        }

        
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
