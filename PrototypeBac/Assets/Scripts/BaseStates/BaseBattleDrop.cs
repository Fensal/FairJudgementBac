using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FMOD.Studio;

public class BaseBattleDrop : BaseBattleState
{
    public BaseBattleDrop(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Enter();
    }

    public override void Enter()
    {
        m_NextState = this;
        m_Character.m_TotalForce.y = 0.0f;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }
}