using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AIBattleHit : BaseBattleHit
{
    private float timer = 0.0f;


    public AIBattleHit(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.LogWarning("[AI Battle State]: Hit");
        Enter();
    }
    public override void Enter()
    {
        m_NextState = this;
    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;
        if (timer >= 1.0f)
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