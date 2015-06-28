using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AIBattleDash : BaseBattleDash
{
    private int m_Orientation;

    public AIBattleDash(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.LogWarning("[AI Battle State]: Dash");
        Enter();
    }
    public override void Enter()
    {
        m_NextState = this;

        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        RaycastHit GroundHitInfo;
        Ray ray = new Ray(m_Character.m_Center, -m_Character.transform.up);
        Physics.Raycast(ray, out GroundHitInfo, m_Character.m_Height, 1 << LayerMask.NameToLayer("Terrain"));
        m_Character.transform.position = new Vector3(m_Character.transform.position.x, GroundHitInfo.point.y + m_Character.m_Height / 2, m_Character.transform.position.z);

        if (Math.Abs(m_Character.m_TotalForce.x) <= 0.1f)
        {
            m_NextState = new AIBattleIdle(m_Character);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }
}