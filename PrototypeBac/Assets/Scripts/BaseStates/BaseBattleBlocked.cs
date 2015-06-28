using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BaseBattleBlocked : BaseBattleState
{

    public BaseBattleBlocked(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Enter();
    }
    public override void Enter()
    {
        m_Character.GetComponent<Animator>().SetTrigger("AttackBlocked");
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