using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AIBattleCounterAttacked : BaseBattleCounterAttacked
{
    private float timer = 0.0f;

    public AIBattleCounterAttacked(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.LogWarning("[AI Battle State]: Counter Attacked");
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

        if (timer > m_Character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length)
            m_NextState = new AIBattleIdle(m_Character);
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }
}