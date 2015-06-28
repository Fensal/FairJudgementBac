using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class BaseBattleWallWalk : BaseBattleState
{

    public BaseBattleWallWalk(BaseCharacterController basecharactercontroller)
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
}

