using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FMOD.Studio;

public class AIBattleStepBack : BaseBattleStepBack
{
    private float m_JumpForce;
    private float m_GravityForce;

    private int m_Orientation;

    public AIBattleStepBack(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.LogWarning("[AI Battle State]: Step Back");
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
        Physics.Raycast(ray, out GroundHitInfo, m_Character.m_Height /*- m_TotalForce.y*/, 1 << LayerMask.NameToLayer("Terrain"));
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

    public void HitWall()
    {
        //m_NextState = new BattleWallWalk(m_Character);
    }
}
