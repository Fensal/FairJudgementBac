using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AIBattleParryDown : BaseBattleParryDown
{
    private float timer = 0.0f;

    public AIBattleParryDown(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.LogWarning("[AI Battle State]: Parry Down");
        Enter();
    }
    public override void Enter()
    {
        m_NextState = this;

        m_Character.GetComponent<Animator>().SetTrigger("ParryDown");
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
       //     m_Character.GetComponent<Animator>().SetBool("ParryDown", false);
        //}

        return base.GetNextState();
    }

    public void ParrySuccessful()
    {
        m_NextState = new AIBattleCounterAttackDown(m_Character);
    }
}