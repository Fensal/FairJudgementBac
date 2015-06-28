using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class BattleWallJump : BaseBattleWallJump, IAirborne
{
    public BattleWallJump(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.Log("[Battle State]: Wall Jump");
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

    public void Land()
    {
        m_NextState = new BattleWallWalk(m_Character);
    }
}
