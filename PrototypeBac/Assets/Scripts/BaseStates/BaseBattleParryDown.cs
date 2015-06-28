using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BaseBattleParryDown : BaseBattleState, IAttackState
{

    public BaseBattleParryDown(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Enter();
    }
    public override void Enter()
    {
        m_NextState = this;

        m_Character.GetComponent<Animator>().SetTrigger("Parry");
    }

    public override void Update()
    {
        base.Update();


    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }

    public virtual void WeaponCollide(Collider other)
    {
    }
}