using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AIMovementIdle : AIMovementState
{

    private DecisionTreeAI decisionTreeAI;

    public AIMovementIdle(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        decisionTreeAI = m_Character.GetComponent<DecisionTreeAI>();
        Debug.LogWarning("[AI Movement State]: Idle");
        Enter();
    }

    public override void Enter()
    {
        m_NextState = this;

        m_Character.m_TotalForce = Vector3.zero;
    }

    public override void Update()
    {
        base.Update();


        if (decisionTreeAI.currentAction.getTypeToString() != this.GetType().ToString())
        {
            m_NextState = decisionTreeAI.currentAction.action();
        }
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }
}