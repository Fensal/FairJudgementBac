using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BattleDashThrough : BaseBattleDash
{
    public BattleDashThrough(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.LogWarning("[Battle State]: Dash Through");
        Enter();
    }
    public override void Enter() 
    {
        base.Enter();

        m_Character.m_TotalForce.x = 0.4f * m_Ortientation;

        m_NextState = this;

        m_PlayerController = (PlayerCharacterController)m_Character;
    }

    public override void Update()
    {
        base.Update();


        RaycastHit GroundHitInfo;
        Ray ray = new Ray(m_Character.m_Center, -m_Character.transform.up);
        Physics.Raycast(ray, out GroundHitInfo, m_Character.m_Height /*- m_TotalForce.y*/, 1 << LayerMask.NameToLayer("Terrain"));
        m_Character.transform.position = new Vector3(m_Character.transform.position.x, GroundHitInfo.point.y + m_Character.m_Height / 2, m_Character.transform.position.z);

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

        if (Math.Abs(m_Character.m_TotalForce.x) <= 0.1f)
        {
            m_PlayerController.updatePreviousActions(this.ToString());
            m_NextState = new BattleIdle(m_Character);
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