using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AIBattleBlocked : BaseBattleBlocked
{
    private float timer = 0.0f;

    public AIBattleBlocked(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.LogWarning("[AI Battle State]: Blocked");
        Enter();
    }
    public override void Enter()
    {
        base.Enter();

        FMOD_StudioSystem.instance.PlayOneShot("event:/Weapons/block", m_Character.transform.position);
        //m_Character.m_Opponent.StartCoroutine((m_Character.m_Opponent as PlayerCharacterController).FlashOverlay(new Color(0.75f, 0.75f, 0.75f, 0.0f)));

        //m_Character.GetHit(5);

        m_NextState = this;
    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        if (timer > m_Character.GetComponent<Animator>().GetCurrentAnimationClipState(0).Length)
        {
            
            m_NextState = new AIBattleIdle(m_Character);
        }
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }
}