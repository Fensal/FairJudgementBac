using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class MovementCrouch : MovementState
{

    public MovementCrouch(BaseCharacterController basecharactercontroller) : base(basecharactercontroller)
    {
        Debug.Log("[Movement State]: Crouch");
        Enter();
    }

    public override void Enter()
    {
        m_NextState = this;
    }

    public override void Update()
    {
        base.Update();

        //BoxCollider2D col = m_Character.collider2D as BoxCollider2D;
        if (m_CrouchButton)
        {
            //col.size = new Vector2(col.size.x, 0.675f);

            //col.center = new Vector2(col.center.x, (-col.size.y / 2) - 0.12f );
        }
        else
        {
            //col.size = new Vector2(col.size.x, 1.35f);
            //col.center = new Vector2(col.center.x, -0.12f);
            m_NextState = new MovementIdle(m_Character);
        }

    }

    public override IBaseState GetNextState()
    {
        return base.GetNextState();
    }
}
