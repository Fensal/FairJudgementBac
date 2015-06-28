using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AIMovementPatrol : AIMovementState
{
    private float m_Velocity;
    private float timer = 0.0f;
    private Transform patrolPointLeft;
    private Transform patrolPointRight;

    private int direction = 1;
    private DecisionTreeAI decisionTreeAI;

    public AIMovementPatrol(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.LogWarning("[AI Movement State]: Patrol");

        decisionTreeAI = m_Character.GetComponent<DecisionTreeAI>();
        Enter();
    }
    public override void Enter()
    {
        m_NextState = this;

        EnemyCharacterController enemy = (EnemyCharacterController)m_Character;

        patrolPointLeft = enemy.patrolPointLeft;
        patrolPointRight = enemy.patrolPointRight;
    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        if (Math.Sign(m_HorizontalAxis) == 1)
        {
            m_Character.m_FacingRight = true;
        }

        if (Math.Sign(m_HorizontalAxis) == -1)
        {
            m_Character.m_FacingRight = false;
        }

        //m_Character.GetComponent<Animator>().SetFloat("Velocity", Math.Abs(m_Velocity));
        m_Character.m_TotalForce = new Vector3(m_Velocity, 0.0f, 0.0f);

        if (decisionTreeAI.currentAction.getTypeToString() != this.GetType().ToString())
        {
            
            m_NextState = decisionTreeAI.currentAction.action();
        }
    }

    public override void FixedUpdate()
    {
        if (direction == 1 && m_Character.transform.position.x >= patrolPointRight.position.x)
            direction = -1;
        else if( direction == -1 && m_Character.transform.position.x <= patrolPointLeft.position.x)
            direction = 1;

        if (direction == 1)
        {
            if (m_Velocity + m_Character.m_Acceleration < m_Character.m_MaxSpeed/2)
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
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }

    /*public void HitWall()
    {
        if (Math.Abs(m_Character.m_TotalForce.x) >= m_Character.m_MaxSpeed / 2)
        {
            m_NextState = new MovementWallRun(m_Character);
        }
        else
        {
            m_NextState = new MovementWallWalk(m_Character);
        }
    }
    */
    public void Drop()
    {
        m_NextState = new AIMovementDrop(m_Character);
    }
}
