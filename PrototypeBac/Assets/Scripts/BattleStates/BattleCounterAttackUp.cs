using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BattleCounterAttackUp : BaseBattleState
{
    private float timer = 0.0f;

    public BattleCounterAttackUp(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.Log("[Battle State]: Counter Attack Up");
        Enter();
    }
    public override void Enter()
    {
        base.Enter();

        FMOD_StudioSystem.instance.PlayOneShot("event:/Weapons/parry", m_Character.transform.position);
        m_Character.GetComponent<Animator>().SetBool("CounterAttackUp", true);
        m_Character.GetComponent<Animator>().SetBool("BlockUp", false);

        m_NextState = this;
        
    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        if (timer > m_Character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length)
        {
            m_Character.GetComponent<Animator>().SetBool("CounterAttackUp", false);
            m_Character.m_Opponent.GetHit(10);
            m_NextState = new BattleIdle(m_Character);
        }
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }
}