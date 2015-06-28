using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FMOD.Studio;

public class BattleDrop : BaseBattleState, IAirborne
{
    public BattleDrop(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.Log("[Battle State]: Drop");
        Enter();
    }

    public override void Enter()
    {
        m_NextState = this;
        //m_Character.m_TotalForce.y = 0.0f;
    }

    public override void Update()
    {
        base.Update();
    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }

    public void Land()
    {
        if (!(m_Character as PlayerCharacterController).m_StandsInWater)
        {
            FMOD_StudioSystem.instance.PlayOneShot("event:/Darien/darien_land", m_Character.transform.position);
        }
        else
        {
            FMOD_StudioSystem.instance.PlayOneShot("event:/Darien/darien_waterland", m_Character.transform.position);
        }
        
        if (Math.Abs(m_Character.m_TotalForce.x) < 0.001f)
        {
            m_NextState = new BattleIdle(m_Character);
        }
        else
        {
            m_NextState = new BattleWalk(m_Character);
        }
    }
}