using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BaseBattleAttackDeflected : BaseBattleState
{

    public BaseBattleAttackDeflected(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Enter();
    }
    public override void Enter()
    {
        m_NextState = this;

        m_Character.GetComponent<Animator>().SetTrigger("AttackBlocked");

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