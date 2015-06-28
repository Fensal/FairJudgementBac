using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BaseBattlePursue : BaseBattleState
{

    public BaseBattlePursue(BaseCharacterController basecharactercontroller)
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