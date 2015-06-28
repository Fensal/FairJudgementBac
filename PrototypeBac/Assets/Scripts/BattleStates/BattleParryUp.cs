using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BattleParryUp : BaseBattleParryUp
{
    private float timer = 0.0f;

    public BattleParryUp(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.Log("[Battle State]: Parry Up");
        Enter();
    }
    public override void Enter()
    {
        base.Enter();
        m_Character.m_TotalForce.x = 0.0f;

        m_PlayerController = (PlayerCharacterController)m_Character;
        
        m_NextState = this;
    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        if (timer > m_Character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length)
        {
            m_PlayerController.updatePreviousActions(this.ToString());
            m_NextState = new BattleIdle(m_Character);
        }

    }

    public override IBaseState GetNextState()
    {
        if (m_NextState is BattleCounterAttackUp || m_NextState is BattleCounterAttackDown)
        {
            m_Character.StartCoroutine((m_Character as PlayerCharacterController).FlashOverlay(new Color(0.75f, 0.75f, 0.75f, 0.0f)));
        }

        //if (m_NextState != this)
        //{
        //    m_Character.GetComponent<Animator>().SetBool("ParryUp", false);
        //}

        return base.GetNextState();
    }
}