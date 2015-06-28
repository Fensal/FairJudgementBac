using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BattleCounterAttackDown : BaseBattleState
{
    private float timer = 0.0f;

    public BattleCounterAttackDown(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.Log("[Battle State]: Counter Attack Down");
        Enter();
    }
    public override void Enter()
    {
        base.Enter();

        FMOD_StudioSystem.instance.PlayOneShot("event:/Weapons/parry", m_Character.transform.position);
        m_Character.GetComponent<Animator>().SetBool("CounterAttackDown", true);
        m_Character.GetComponent<Animator>().SetBool("BlockDown", false);

        m_NextState = this;

    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        if (timer > m_Character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length)
        {
            m_Character.GetComponent<Animator>().SetBool("CounterAttackDown", false);
            m_Character.m_Opponent.GetHit(10);
            m_NextState = new BattleIdle(m_Character);
        }
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }
}