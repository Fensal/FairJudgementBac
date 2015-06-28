using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AIBattleBlockUp : BaseBattleBlockUp
{
    private float timer = 0.0f;

    public AIBattleBlockUp(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.LogWarning("[AI Battle State]: Block Up");
        Enter();
    }
    public override void Enter()
    {
        base.Enter();
        m_NextState = this;

        m_EnemyController = (EnemyCharacterController)m_Character;
    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        if (timer > 3.0f)
        {
            m_Character.GetComponent<Animator>().SetBool("BlockUp", false);

            m_EnemyController.updatePreviousActions(this.ToString());

            m_NextState = new AIBattleIdle(m_Character);
        }
    }

    public override IBaseState GetNextState()
    {
        if (m_NextState != this)
        {
            m_EnemyController.updatePreviousActions(this.ToString());
            m_Character.m_WeaponCollider.enabled = false;
        }

        return base.GetNextState();
    }
}