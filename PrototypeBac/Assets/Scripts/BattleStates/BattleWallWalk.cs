using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class BattleWallWalk : BaseBattleWallWalk
{

    public BattleWallWalk(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.Log("[Battle State]: Wall Walk");
        Enter();
    }

    public override void Enter()
    {
        m_NextState = this;
    }

    public override void Update()
    {
        base.Update();

        if (m_Character.m_WallHitInfo.point.x > m_Character.transform.position.x && Math.Sign(m_HorizontalAxis) == -1)
        {
            m_NextState = new BattleWalk(m_Character);
            m_Character.transform.position -= new Vector3(0.05f, 0.0f, 0.0f);
            
        }

        if (m_Character.m_WallHitInfo.point.x < m_Character.transform.position.x && Math.Sign(m_HorizontalAxis) == 1)
        {
            m_NextState = new BattleWalk(m_Character);
            m_Character.transform.position += new Vector3(0.05f, 0.0f, 0.0f);
        }

        if (m_HorizontalAxis == 0.0f)
        {
            m_NextState = new BattleWalk(m_Character);
        }
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }
}

