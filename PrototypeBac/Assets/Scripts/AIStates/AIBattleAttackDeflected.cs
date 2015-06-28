using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AIBattleAttackDeflected : BaseBattleAttackDeflected
{
    private float timer = 0.0f;

    public AIBattleAttackDeflected(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.LogWarning("[AI Battle State]: Attack Deflected");
        Enter();
    }
    public override void Enter()
    {
        base.Enter();
        m_NextState = this;
    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        if (timer > m_Character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length)
        {
            m_NextState = new AIBattleIdle(m_Character);
        }
        m_Character.m_TotalForce.x = 0.0f;
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }
}