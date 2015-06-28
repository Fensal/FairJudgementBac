using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BaseBattleHit : BaseBattleState
{
    //TODO:
    //implement knockback, etc. here
    public BaseBattleHit(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
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

    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }
}