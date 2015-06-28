using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BattleBlocked : BaseBattleBlocked
{
    private float timer = 0.0f;

    public BattleBlocked(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.Log("[Battle State]: Blocked");
        Enter();
    }
    public override void Enter()
    {
        base.Enter();
        m_Character.GetComponent<Animator>().SetBool("AttackBlockedTest", true);
        FMOD_StudioSystem.instance.PlayOneShot("event:/Weapons/weapon_impact", m_Character.transform.position);

        FMOD_StudioSystem.instance.PlayOneShot("event:/Weapons/block", m_Character.transform.position);
        m_Character.StartCoroutine((m_Character as PlayerCharacterController).FlashOverlay(new Color(0.75f, 0.75f, 0.75f, 0.0f)));

        m_NextState = this;
    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        if (timer > m_Character.GetComponent<Animator>().GetCurrentAnimationClipState(0).Length)
        {
            m_Character.GetComponent<Animator>().SetBool("AttackBlockedTest", false);

            m_NextState = new BattleIdle(m_Character);
        }
            
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