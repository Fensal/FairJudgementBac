using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AIBattleParryUp : BaseBattleParryUp
{
    private float timer = 0.0f;

    public AIBattleParryUp(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.LogWarning("[AI Battle State]: Parry Up");
        Enter();
    }
    public override void Enter()
    {
        m_NextState = this;

        m_Character.GetComponent<Animator>().SetTrigger("ParryUp");
    }

    public override void Update()
    {
        base.Update();
        
        timer += Time.deltaTime;

        if (timer > m_Character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length)
            m_NextState = new AIBattleIdle(m_Character);
    }

    public override IBaseState GetNextState()
    {

        //if (m_NextState != this)
        //{
        //    m_Character.GetComponent<Animator>().SetBool("ParryUp", false);
        //}

        return base.GetNextState();
    }

    public void ParrySuccessful()
    {
        m_NextState = new AIBattleCounterAttackUp(m_Character);
    }
}