﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BaseBattleIdle : BaseBattleState
{
    private float timer = 0.0f;

    public BaseBattleIdle(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Enter();
    }

    public override void Enter()
    {
        m_NextState = this;
        m_Character.m_TotalForce.x = 0.0f;
    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }
}