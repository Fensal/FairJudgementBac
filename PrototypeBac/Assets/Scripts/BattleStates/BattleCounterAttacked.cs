using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BattleCounterAttacked : BaseBattleState
{
    private float timer = 0.0f;

    public BattleCounterAttacked(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.Log("[Battle State]: Counter Attacked");
        Enter();
    }
    public override void Enter()
    {
        m_NextState = this;

        FMOD_StudioSystem.instance.PlayOneShot("event:/Weapons/weapon_impact", m_Character.transform.position);

        timer += Time.deltaTime;

        if (timer > m_Character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length)
            m_NextState = new BattleIdle(m_Character);
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