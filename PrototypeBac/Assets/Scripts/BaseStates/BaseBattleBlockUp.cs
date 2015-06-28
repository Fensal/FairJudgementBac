using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BaseBattleBlockUp : BaseBattleState
{

    public BaseBattleBlockUp(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Enter();
    }
    public override void Enter()
    {
        m_NextState = this;
        m_Character.m_TotalForce = Vector3.zero;

        m_Character.m_WeaponCollider.enabled = true;

        m_Character.GetComponent<Animator>().SetBool("BlockUp", true);
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