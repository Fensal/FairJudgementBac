using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BattleIdle : BaseBattleIdle
{
    private float timer = 0.0f;
    private float m_ButtonTimer = 0.0f;
    private int m_Ortientation;

    public BattleIdle(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.Log("[Battle State]: Idle");
        Enter();
    }

    public override void Enter()
    {
        m_NextState = this;

        if (m_Character.m_FacingRight)
        {
            m_Ortientation = 1;
        }
        else
        {
            m_Ortientation = -1;
        }

        m_Character.m_TotalForce.x = 0.0f;
    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        if (Math.Abs(m_HorizontalAxis) >= 0.1f)
        {
            if (Math.Sign(m_HorizontalAxis) == m_Ortientation && m_Character is PlayerCharacterController)
            {
                (m_Character as PlayerCharacterController).m_ComboState = new ForwardState(m_Character, (m_Character as PlayerCharacterController).m_ComboState);
            }
            m_NextState = new BattleWalk(m_Character);

            if (Math.Sign(m_HorizontalAxis) != m_Ortientation && m_Character is PlayerCharacterController)
            {
                (m_Character as PlayerCharacterController).m_ComboState = new BackState(m_Character, (m_Character as PlayerCharacterController).m_ComboState);
            }
            m_NextState = new BattleWalk(m_Character);
        }

        

        if (m_LightAttackButton == true)
        {
            
            if (m_VerticalAxis < -0.1f)
                m_NextState = new BattleLightAttackDown(m_Character);
            else
                m_NextState = new BattleLightAttackUp(m_Character);
        }

        if (m_HeavyAttackButton == true)
        {
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

        if(m_Character.m_Opponent == null)
        {
            m_NextState = new MovementIdle(m_Character);
        }
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }

    public void Drop()
    {
        m_NextState = new BattleDrop(m_Character);
    }
}
