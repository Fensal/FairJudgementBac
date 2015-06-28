using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BattleSuccessfulAttack : BaseBattleSuccessfulAttack
{
    private float timer = 0.0f;

    public BattleSuccessfulAttack(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.Log("[Battle State]: Successful Attack");
        Enter();
    }
    public override void Enter()
    {

        base.Enter();
        m_NextState = this;

        FMOD_StudioSystem.instance.PlayOneShot("event:/Weapons/weapon_impact", m_Character.transform.position);

        m_Character.m_TotalForce.x = 0.0f;
    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        if (timer > 1.0f)
            m_NextState = new BattleIdle(m_Character);
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }
}