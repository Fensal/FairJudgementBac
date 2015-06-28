﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BattleBlockUp : BaseBattleBlockUp
{
    private float timer = 0.0f;
    private float registrationTimer = 0.0f;
    public float parryTimer = 0.0f;

    public BattleBlockUp(BaseCharacterController basecharactercontroller)
        : base(basecharactercontroller)
    {
        Debug.Log("[Battle State]: Block Up");
        Enter();
    }
    public override void Enter()
    {
        base.Enter();
        m_NextState = this;
        m_Character.m_TotalForce = Vector3.zero;

        parryTimer = 0.0f;

        m_PlayerController = (PlayerCharacterController)m_Character;
    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;
        registrationTimer += Time.deltaTime;
        parryTimer += Time.deltaTime;

        if (registrationTimer > 2.0f)
        {
            m_PlayerController.updatePreviousActions(this.ToString());

            registrationTimer = 0.0f;
        }

        if (!m_BlockButton && timer > m_Character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length)
        {
            m_Character.GetComponent<Animator>().SetBool("BlockUp", false);
            m_NextState = new BattleIdle(m_Character);
        }

        if (m_VerticalAxis <= -0.1f)
        {
            m_Character.GetComponent<Animator>().SetBool("BlockDown", true);
            m_Character.GetComponent<Animator>().SetBool("BlockUp", false);
            m_NextState = new BattleBlockDown(m_Character);
        }

        m_Character.m_TotalForce.x = 0.0f;
    }

    public override IBaseState GetNextState()
    {
        if (m_NextState != this)
        {
            m_Character.GetComponent<Animator>().SetBool("BlockUp", false);
            m_Character.m_WeaponCollider.enabled = false;
        }

        return base.GetNextState();
    }
}