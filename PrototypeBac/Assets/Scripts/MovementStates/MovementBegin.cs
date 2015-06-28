using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class MovementBegin : MovementState
{
    private float m_Timer = 0.0f;

    public MovementBegin(BaseCharacterController basecharactercontroller): base(basecharactercontroller)
    {
        Debug.Log("[Movement State]: Begin");
        Enter();
    }

    public override void Enter()
    {
        m_NextState = this;
    }

    public override void Update()
    {
        base.Update();

        m_Timer += Time.deltaTime;
        if (m_Timer >= m_Character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length)
        {
            m_NextState = new MovementIdle(m_Character);
        }
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }
}
