using System;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class AIBattleRetreat : BaseBattleRetreat
{

    private float m_Velocity;
    private float timer = 0.0f;
    private float waitTimer = 0.0f;

    public AIBattleRetreat(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.LogWarning("[AI Battle State]: Retreat");
        Enter();
    }

    public override void Enter()
    {
        m_NextState = this;

        m_EnemyController = (EnemyCharacterController)m_Character;
        //m_PlayerController = (EnemyCharacterController)m_Character.m_Opponent;

        m_OpponentController = (EnemyCharacterController)m_Character.m_Opponent;

        m_Character.m_TotalForce = Vector3.zero;
    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;
        waitTimer += Time.deltaTime;

        m_Character.m_TotalForce = new Vector3(m_Velocity, 0.0f, 0.0f);

        m_Character.GetComponent<Animator>().SetFloat("VelocityX", Math.Abs(m_Character.m_TotalForce.x));

        if (Mathf.Abs(m_Character.m_Opponent.transform.position.x - m_Character.transform.position.x) > m_Character.m_BattleDistance ||
            Mathf.Abs(m_Character.m_Opponent.transform.position.z - m_Character.transform.position.z) > 1.0f ||
            Mathf.Abs(m_Character.m_Opponent.transform.position.y - m_Character.transform.position.y) > 1.0f)
        {
            m_Character.m_Opponent.m_Opponent = null;

            m_NextState = new AIMovementIdle(m_Character);
        }

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (timer < 1f)
        {
            int direction;
            if (m_PlayerController.transform.position.x > m_EnemyController.transform.position.x)
                direction = -1;
            else
                direction = 1;

            if (direction == 1)
            {
                if (m_Velocity + m_Character.m_Acceleration < m_Character.m_MaxSpeed / 2)
                {
                    m_Velocity += m_Character.m_Acceleration;
                }
                else
                {
                    m_Velocity = m_Character.m_MaxSpeed / 2;
                }
            }
            else if (direction == -1)
            {

                if (Math.Abs(m_Velocity - m_Character.m_Acceleration) < m_Character.m_MaxSpeed / 2)
                {
                    m_Velocity -= m_Character.m_Acceleration;
                }
                else
                {
                    m_Velocity = (-1) * m_Character.m_MaxSpeed / 2;
                }
            }
            else
            {
                if (m_Velocity - m_Character.m_Deceleration > 0.0f)
                {
                    m_Velocity -= m_Character.m_Deceleration;
                }
                else if (m_Velocity + m_Character.m_Deceleration < 0.0f)
                {
                    m_Velocity += m_Character.m_Deceleration;
                }
                else
                {
                    m_Velocity = 0.0f;
                }
            }
            waitTimer = 0.0f;
        }
        else
            m_Velocity = 0.0f;

        if (waitTimer > 1f)
            m_NextState = new AIBattleIdle(m_Character);

    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }
}
