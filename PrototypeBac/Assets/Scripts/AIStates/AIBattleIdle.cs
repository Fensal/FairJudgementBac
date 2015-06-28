using System;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class AIBattleIdle : BaseBattleIdle
{
    private float timer = 0.0f;

    

    public AIBattleIdle(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.LogWarning("[AI Battle State]: Idle");
        Enter();
    }

    public override void Enter()
    {
        m_NextState = this;

        //m_PlayerController = (PlayerCharacterController)m_Character;
        m_EnemyController = (EnemyCharacterController)m_Character;
        m_OpponentController = (EnemyCharacterController)m_Character.m_Opponent;

        m_Character.m_TotalForce = Vector3.zero;

        

    }

    public override void Update()
    {
        base.Update();

        m_Character.m_TotalForce = Vector3.zero;

        m_Character.GetComponent<Animator>().SetFloat("VelocityX", 0.0f);

        timer += Time.deltaTime;

        


        if ( !m_Character.m_Opponent.m_IsDead)
            m_NextState = m_EnemyController.m_EnemyType.GetNextAction();

        if (Mathf.Abs(m_Character.m_Opponent.transform.position.x - m_Character.transform.position.x) > m_Character.m_BattleDistance ||
            Mathf.Abs(m_Character.m_Opponent.transform.position.z - m_Character.transform.position.z) > 1.0f ||
            Mathf.Abs(m_Character.m_Opponent.transform.position.y - m_Character.transform.position.y) > 1.0f)
        {
            m_Character.m_Opponent.m_Opponent = null;

            m_NextState = new AIMovementIdle(m_Character);
        }

    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }
}

