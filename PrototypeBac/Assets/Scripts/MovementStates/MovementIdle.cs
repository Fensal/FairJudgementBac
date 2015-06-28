using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MovementIdle : MovementState
{

    public MovementIdle(BaseCharacterController basecharactercontroller) : base(basecharactercontroller)
    {
        Debug.Log("[Movement State]: Idle");
        Enter();
    }

    public override void Enter()
    {
        m_Character.m_TotalForce.x = 0.0f;
        m_Character.GetComponent<Animator>().SetBool("BlockDown", false);
        m_Character.GetComponent<Animator>().SetBool("BlockUp", false);
        m_Character.GetComponent<Animator>().SetBool("ClimbUp", false);
        m_Character.GetComponent<Animator>().SetBool("Hang", false);
        m_Character.GetComponent<Animator>().SetBool("Jump", false);
        m_NextState = this;

    }

    public override void Update()
    {
        base.Update();


        if (m_PlaneShiftButton > 0.0f)
        {
            m_NextState = new MovementPlaneShift(m_Character);
        }
        else
        {
            
            if (Math.Abs(m_HorizontalAxis) >= 0.1f)
            {
                m_NextState = new MovementWalk(m_Character);
            }
            if (m_JumpButton == true)
            {
                m_NextState = new MovementJump(m_Character);
            }
            if (m_CrouchButton == true)
            {
                m_NextState = new MovementCrouch(m_Character);
            }
            if(m_Character.m_Opponent != null)
            {
                m_NextState = new BattleIdle(m_Character);
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

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }

    public void Drop()
    {
        m_NextState = new MovementDrop(m_Character);
    }
}