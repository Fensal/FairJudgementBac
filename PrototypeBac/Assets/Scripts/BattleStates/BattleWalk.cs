using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BattleWalk : BaseBattleWalk
{
    private float m_Velocity;
    private float timer = 0.0f;
    private float m_ButtonTimer = 0.0f;

    public BattleWalk(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.Log("[Battle State]: Walk");
        Enter();
    }
    public override void Enter()
    {
        m_NextState = this;
    }

    public override void Update()
    {
        base.Update();

        m_Character.GetComponent<Animator>().SetFloat("VelocityX", Math.Abs(m_Character.m_TotalForce.x));

        timer += Time.deltaTime;

        if (Math.Sign(m_HorizontalAxis) == 1)
        {
            float maxSpeed = m_Character.m_MaxSpeed / 2;
            //if (m_Character.m_FacingRight)
            //    maxSpeed = m_Character.m_MaxSpeed/4*3;
            //else
            //    maxSpeed = m_Character.m_MaxSpeed / 2;

            if (m_Velocity + m_Character.m_Acceleration < maxSpeed)
            {
                m_Velocity += m_Character.m_Acceleration;
            }
            else
            {
                m_Velocity = maxSpeed;
            }
        }
        else if (Math.Sign(m_HorizontalAxis) == -1)
        {

            float maxSpeed;
            if (!m_Character.m_FacingRight)
                maxSpeed = m_Character.m_MaxSpeed;
            else
                maxSpeed = m_Character.m_MaxSpeed / 2;

            if (Math.Abs(m_Velocity - m_Character.m_Acceleration) < maxSpeed || m_Velocity > 0f)
            {
                m_Velocity -= m_Character.m_Acceleration;
            }
            else
            {
                m_Velocity = (-1) * maxSpeed;
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

        

        if (Math.Abs(m_Velocity) == 0.0f && m_HorizontalAxis == 0.0f)
        {
            m_NextState = new BattleIdle(m_Character);
        }

        if (m_LightAttackButton == true)
        {
            m_Velocity = 0.0f;

            if (m_VerticalAxis < -0.1f)
                m_NextState = new BattleLightAttackDown(m_Character);
            else
                m_NextState = new BattleLightAttackUp(m_Character);
        }

        if (m_HeavyAttackButton == true)
        {
            m_Velocity = 0.0f;

            if (m_VerticalAxis < -0.1f)
                m_NextState = new BattleHeavyAttackDown(m_Character);
            else
                m_NextState = new BattleHeavyAttackUp(m_Character);
        }

        if (m_JumpButton == true)
        {
            if (Math.Sign(m_HorizontalAxis) == Math.Sign(m_Character.transform.localScale.x))
            {
                m_Character.m_CurrentState = new BattleStepBack(m_Character);
            }
            else
            {
                m_Character.m_CurrentState = new BattleDash(m_Character);
            }
        }


        if (m_BlockButton == true)
        {
            m_ButtonTimer += Time.deltaTime;

            if (m_ButtonTimer >= 0.01f)
            {
                if (m_VerticalAxis < -0.1f)
                {
                    m_Character.GetComponent<Animator>().SetBool("BlockDown", true);
                    m_NextState = new BattleBlockDown(m_Character);
                }

                else
                {
                    m_Character.GetComponent<Animator>().SetBool("BlockUp", true);
                    m_NextState = new BattleBlockUp(m_Character);
                }
            }
        }
        if (m_BlockButtonUp)
        {
            if (m_VerticalAxis < -0.1f)
            {
                m_Character.GetComponent<Animator>().SetTrigger("ParryDown");
                m_NextState = new BattleParryDown(m_Character);
            }

            else
            {
                m_Character.GetComponent<Animator>().SetTrigger("ParryUp");
                m_NextState = new BattleParryUp(m_Character);
            }

        }

        if (m_BlockButtonUp)
        {
            m_NextState = new BattleParryUp(m_Character);
        }

        if(m_Character.m_Opponent == null)
        {
            m_NextState = new MovementWalk(m_Character);
        }

        m_Character.m_TotalForce = new Vector3(m_Velocity, 0.0f, 0.0f);
    }

    public override IBaseState GetNextState()
    {
        if (m_NextState != this)
        {
            m_Character.m_TotalForce = Vector3.zero;
            m_Character.GetComponent<Animator>().SetFloat("VelocityX", Math.Abs(m_Character.m_TotalForce.x));
        }
        return base.GetNextState();
    }

    public override void HitWall()
    {
        m_NextState = new BattleWallWalk(m_Character);

    }

    public void Drop()
    {
        m_NextState = new BattleDrop(m_Character);
    }
}
