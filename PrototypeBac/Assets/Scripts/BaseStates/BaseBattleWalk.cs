using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BaseBattleWalk : BaseBattleState
{
    private float m_Velocity;

    public BaseBattleWalk(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Enter();
    }
    public override void Enter()
    {
        m_NextState = this;
    }

    public override void Update()
    {
        base.Update();
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }

    public virtual void HitWall()
    {
        m_NextState = new BattleWallWalk(m_Character);

    }
}
