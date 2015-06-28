using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BattleAttackDeflected : BaseBattleAttackDeflected
{
    private float timer = 0.0f;

    public BattleAttackDeflected(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.Log("[Battle State]: Attack Deflected");
        Enter();
    }
    public override void Enter()
    {
        base.Enter();
        m_Character.GetComponent<Animator>().SetBool("AttackBlockedTest", true);
        FMOD_StudioSystem.instance.PlayOneShot("event:/Weapons/weapon_impact", m_Character.transform.position);

        m_Character.StartCoroutine((m_Character as PlayerCharacterController).FlashOverlay(new Color(0.75f, 0.75f, 0.75f, 0.0f)));
        m_NextState = this;
        m_Character.m_TotalForce = Vector3.zero;
    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;
        if (timer > m_Character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length)
        {
            m_Character.GetComponent<Animator>().SetBool("AttackBlockedTest", false);
            m_NextState = new BattleIdle(m_Character);
        }
        m_Character.m_TotalForce.x = 0.0f;
    }

    public override IBaseState GetNextState()
    {
        if (m_NextState != this)
        {
            m_Character.GetComponent<Animator>().SetBool("AttackBlockedTest", false);
        }
        return base.GetNextState();
    }
}