using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AIBattleBlockDown : BaseBattleBlockDown
{
    private float timer = 0.0f;

    public AIBattleBlockDown(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.LogWarning("[AI Battle State]: Block Down");
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
            m_EnemyController.updatePreviousActions(this.ToString());
            m_Character.GetComponent<Animator>().SetBool("BlockDown", false);

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