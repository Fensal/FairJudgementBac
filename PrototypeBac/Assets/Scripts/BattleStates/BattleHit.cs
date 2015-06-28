using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BattleHit: BaseBattleHit
{
    private float timer = 0.0f;

    public BattleHit(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.Log("[Battle State]: Block");
        Enter();
    }
    public override void Enter()
    {
        m_NextState = this;
        m_Character.m_TotalForce = Vector3.zero;
    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;
        if (timer >= 1.0f)
        {
            m_NextState = new BattleIdle(m_Character);
        }
        m_Character.m_TotalForce.x = 0.0f;
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }
}