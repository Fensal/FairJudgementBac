using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MovementLightAttackDown : MovementState, IAttackState
{
    private float timer = 0.0f;

    public MovementLightAttackDown(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.Log("[Movement State]: Light Attack Down");
        Enter();
    }
    public override void Enter()
    {
        base.Enter();

        m_NextState = this;

        m_Character.m_TotalForce = Vector3.zero;

        m_Character.m_WeaponCollider.enabled = true;
        m_Character.GetComponent<Animator>().SetTrigger("LightAttackDown");
        m_Character.GetComponent<Animator>().SetFloat("LookingRight", (float)Math.Sign(m_Character.m_Renderer.transform.localScale.x));

        m_Character.GetComponent<Animator>().SetBool("AttackBlockedTest", false);

        FMOD_StudioSystem.instance.PlayOneShot("event:/Darien/darien_attack_light", m_Character.transform.position);

        m_PlayerController = (PlayerCharacterController)m_Character;
    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        if (timer > m_Character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length)
        {
            m_NextState = new MovementIdle(m_Character);
        }

    }

    public override IBaseState GetNextState()
    {
        if (m_NextState != this)
        {
            m_Character.m_WeaponCollider.enabled = false;
        }
        return base.GetNextState();
    }

    public void WeaponCollide(Collider other)
    {
        if (m_NextState == this)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Switch") || other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {

                SwitchScript s = other.GetComponentInParent<SwitchScript>();

                if (s != null)
                {
                    s.HitSwitch();
                }
                else
                {
                    s = other.transform.parent.GetComponentInParent<SwitchScript>();

                    if (s != null)
                    {
                        s.HitSwitch();
                    }
                }
            }
        }
    }
}

