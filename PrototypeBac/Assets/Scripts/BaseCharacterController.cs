using System;
using System.Linq;
using System.Collections;
using System.Text;
using UnityEngine;

public class BaseCharacterController : MonoBehaviour
{
    public bool m_FacingRight;
    public IBaseState m_CurrentState;
    public float m_Acceleration;
    public float m_Deceleration;
    public float m_MaxSpeed;
    public Vector3 m_TotalForce;
    public float m_GravityForce;
    public float m_MaxFallSpeed;
    public float m_Height;
    public float m_Width;
    public float m_BaseZLevel;
    public Vector3 m_Center;
    public Collider m_Collider;
    public RaycastHit m_WallHitInfo;
    public RaycastHit m_EnemyHitInfo;
    public SpriteRenderer m_Renderer;
    public Vector3 slopeVector;
    public BaseCharacterController m_Opponent;
    public int m_NGramWindowSize;
    public Animation m_WeaponAnimation;
    public Collider m_WeaponCollider;
    public float m_AttackCooldown;

    public bool m_IsDead = false;

    public float m_BattleDistance;
    public float m_BattleDistanceY;

    //STATS
    public float m_MaxHealth;
    public float m_CurrentHealth;

    [SerializeField]
    Transform m_Rotated;

    public virtual void Update()
    {
        if (m_FacingRight)
        {
            m_Rotated.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
           // m_Renderer.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
            m_Renderer.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            m_Rotated.rotation = Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f));
            //m_Renderer.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f));
            m_Renderer.transform.localScale = new Vector3(-1.0f, 01.0f, 1.0f);
        }
    }

    public virtual void GetHit(int Damage)
    {
        GetComponent<Animator>().SetBool("BlockUp", false);
        GetComponent<Animator>().SetBool("BlockDown", false);
        m_CurrentHealth -= Damage;

        if (m_CurrentHealth <= 0)
        {
            m_TotalForce = Vector3.zero;
            if (this is EnemyCharacterController)
            {
                StartCoroutine("DeathEnemy");
            }
            if ( this is PlayerCharacterController)
            {
                StartCoroutine("DeathCharacter");
                
                //Application.LoadLevel("Playground");
            }
        }
    }

    public IEnumerator DeathEnemy()
    {
        FMOD_StudioSystem.instance.PlayOneShot("event:/Enemy/enemy_death", transform.position);
        float timer = 0.0f;
        GetComponent<Animator>().SetTrigger("Die");
        
        m_IsDead = true;
        m_Opponent.m_Opponent = null;

        
        while (timer <= GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        //Destroy(this.gameObject);
    }

    public IEnumerator DeathCharacter()
    {
        m_IsDead = true;

        FMOD_StudioSystem.instance.PlayOneShot("event:/Darien/darien_death", transform.position);
        float timer = 0.0f;
        GetComponent<Animator>().SetTrigger("Die");

        
        m_CurrentHealth = 100;

        bool played = false;
        while (timer <= GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length)
        {
            if (timer >= 2.0f && !played)
            {
                Root.Instance.m_LoadingScreen.gameObject.SetActive(true);
                Root.Instance.StartCoroutine(Root.Instance.m_LoadingScreen.FadeIn(false));
                played = true;
            }
                
            if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Death"))
                timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        


        transform.position = Root.Instance.m_StartPosition.position;
        Root.Instance.m_Character.m_TotalForce = Vector3.zero;

        m_Opponent = null;

        yield return new WaitForSeconds(3.0f);

        Root.Instance.StartCoroutine(Root.Instance.m_LoadingScreen.FadeOut());
        yield return new WaitForSeconds(1.0f);

        m_IsDead = false;
    }

}