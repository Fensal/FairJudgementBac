using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AIBattleSuccessfulAttack : BaseBattleSuccessfulAttack
{
    private float timer = 0.0f;

    public AIBattleSuccessfulAttack(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.LogWarning("[AI Battle State]: Successful Attack");
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

        if (timer > 1.0f)
            m_NextState = new AIBattleIdle(m_Character);
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }
}