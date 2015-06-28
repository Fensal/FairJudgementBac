using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MovementWalk : MovementState
{
    private float m_Velocity;

    public MovementWalk(BaseCharacterController basecharactercontroller) : base(basecharactercontroller)
    {
        Debug.Log("[Movement State]: Walk");
        Enter();
    }
    public override void Enter()
    {
        m_Velocity = m_Character.m_TotalForce.x;
        m_NextState = this;

        m_Character.GetComponent<Animator>().SetBool("BlockDown", false);
        m_Character.GetComponent<Animator>().SetBool("BlockUp", false);
        m_Character.GetComponent<Animator>().SetBool("ClimbUp", false);
        m_Character.GetComponent<Animator>().SetBool("Hang", false);
        m_Character.GetComponent<Animator>().SetBool("Jump", false);

        if ((m_Character as PlayerCharacterController).m_StandsInWater && Root.Instance != null)
        {
            if (Root.Instance.m_Walksound != null)
            {
                Root.Instance.m_Walksound.start();
            }
        }
    }

    public override void Update()
    {
        base.Update();

        m_Character.GetComponent<Animator>().SetFloat("VelocityX", Math.Abs(m_Character.m_TotalForce.x));      


        if (Math.Sign(m_HorizontalAxis) == 1)
        {
            m_Character.m_FacingRight = true;
        }
        
        if (Math.Sign(m_HorizontalAxis) == -1)
        {
            m_Character.m_FacingRight = false;
        }

        m_Character.m_TotalForce = new Vector3(m_Velocity, 0.0f, 0.0f);
        if (m_PlaneShiftButton > 0.0f)
        {
            m_NextState = new MovementPlaneShift(m_Character);
        }
        else
        {
            if (Math.Abs(m_Velocity) == 0.0f && m_HorizontalAxis == 0.0f)
            {
                m_NextState = new MovementIdle(m_Character);
            }
            if (m_JumpButton == true)
            {
                m_NextState = new MovementJump(m_Character);
            }
            if (m_Character.m_Opponent != null)
            {
                m_NextState = new BattleWalk(m_Character);
            }

            if (m_LightAttackButton == true)
            {

                if (m_VerticalAxis < -0.1f)
                    m_NextState = new MovementLightAttackDown(m_Character);
                else
                    m_NextState = new MovementLightAttackUp(m_Character);
            }

            if (m_HeavyAttackButton == true)
            {
                if (m_VerticalAxis < -0.1f)
                    m_NextState = new MovementHeavyAttackDown(m_Character);
                else
                    m_NextState = new MovementHeavyAttackUp(m_Character);
            }

            if (m_BlockButton == true)
            {
                if (m_VerticalAxis < -0.1f)
                {
                    m_Character.GetComponent<Animator>().SetBool("BlockDown", true);
                    m_NextState = new MovementBlockDown(m_Character);
                }

                else
                {
                    m_Character.GetComponent<Animator>().SetBool("BlockUp", true);
                    m_NextState = new MovementBlockUp(m_Character);
                }
            }
        }
    }

    public override void FixedUpdate()
    {
        if (Math.Sign(m_HorizontalAxis) == 1)
        {
            if (m_Velocity + m_Character.m_Acceleration < m_Character.m_MaxSpeed)
            {
                m_Velocity += m_Character.m_Acceleration;
            }
            else
            {
                m_Velocity = m_Character.m_MaxSpeed;
            }
        }
        else if (Math.Sign(m_HorizontalAxis) == -1)
        {

            if (Math.Abs(m_Velocity - m_Character.m_Acceleration) < m_Character.m_MaxSpeed)
            {
                m_Velocity -= m_Character.m_Acceleration;
            }
            else
            {
                m_Velocity = (-1) * m_Character.m_MaxSpeed;
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
        if (m_NextState != this)
        {
            if(!(m_NextState is IAirborne))
                m_Character.m_TotalForce.x = 0.0f;
            m_Character.GetComponent<Animator>().SetFloat("VelocityX", 0.0f);

            if (/*(m_Character as PlayerCharacterController).m_StandsInWater && */Root.Instance != null && Root.Instance.m_Walksound != null)
            {
                Root.Instance.m_Walksound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }
        }
        return base.GetNextState();
    }

    public void Drop()
    {
        m_NextState = new MovementDrop(m_Character);
    }
}