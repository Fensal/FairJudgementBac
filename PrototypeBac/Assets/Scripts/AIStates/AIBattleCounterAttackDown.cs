using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AIBattleCounterAttackDown : BaseBattleCounterAttackDown
{
    private float timer = 0.0f;

    public AIBattleCounterAttackDown(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.LogWarning("[AI Battle State]: Counter Attack Down");
        Enter();
    }
    public override void Enter()
    {
        m_NextState = this;

        FMOD_StudioSystem.instance.PlayOneShot("event:/Weapons/parry", m_Character.transform.position);
        m_Character.GetComponent<Animator>().SetTrigger("CounterAttackDown");

    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        if (timer > 0.6f)
        {
            //m_Character.GetComponent<Animator>().SetBool("CounterAttackDown", false);
            m_Character.m_Opponent.GetHit(10);
            m_NextState = new AIBattleIdle(m_Character);
        }
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }
}