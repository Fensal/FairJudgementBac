using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BaseBattlePlaneShift : BaseBattleState
{

    public BaseBattlePlaneShift(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Enter();
    }

    public override void Enter()
    {
        m_Character.m_TotalForce.x = 0.0f;
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
