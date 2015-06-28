using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AIBattleCounterAttackUp : BaseBattleCounterAttackUp
{
    private float timer = 0.0f;

    public AIBattleCounterAttackUp(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.LogWarning("[AI Battle State]: Counter Attack Up");
        Enter();
    }
    public override void Enter()
    {
        m_NextState = this;

        FMOD_StudioSystem.instance.PlayOneShot("event:/Weapons/parry", m_Character.transform.position);
        m_Character.GetComponent<Animator>().SetTrigger("CounterAttackUp");

    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        if (timer > 0.6f)
        {
            //m_Character.GetComponent<Animator>().SetBool("CounterAttackUp", false);
            m_Character.m_Opponent.GetHit(10);
            m_NextState = new AIBattleIdle(m_Character);
        }
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }
}