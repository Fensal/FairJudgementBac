using UnityEngine;
using System.Collections;
using System;
using FMOD;

public class CameraTrigger : MonoBehaviour {
    public BaseCharacterController m_Character;
    public GenerateParticles m_PlaneshiftScript;

    void Awake()
    {
        if (m_Character == null && Root.Instance != null)
        {
            m_Character = Root.Instance.m_Character;
            m_PlaneshiftScript = m_Character.gameObject.GetComponentInChildren<GenerateParticles>();
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "CameraCollider_CameraMoves")
        {
            m_PlaneshiftScript.m_AllowPlaneShift = 2;
        }

        if (other.gameObject.name == "CameraCollider_CameraStays")
        {
            m_PlaneshiftScript.m_AllowPlaneShift = 1;
        }

        if (other.gameObject.name == "CameraCollider_ChangeScene")
        {
            m_PlaneshiftScript.m_AllowPlaneShift = 3;
        }

        if (other.gameObject.name == "CameraCollider_Waterfall" && Root.Instance != null)
        {
            Root.Instance.m_Waterfall.start();
        }

        if (other.gameObject.name == "CameraCollider_WaterStep")
        {
            (m_Character as PlayerCharacterController).m_StandsInWater = true;
        }

        if (other.gameObject.name == "CameraCollider_LoadCredits")
        {
            m_PlaneshiftScript.m_AllowPlaneShift = 4;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "CameraCollider_Waterfall" && Root.Instance != null)
        {
            Vector3 center = other.collider.bounds.center;

            float xLeft = center.x - (other.collider.bounds.size.x / 2.0f);
            float xRight = center.x + (other.collider.bounds.size.x / 2.0f);

            float maxDistance = Math.Abs(xLeft - xRight);
            float leftDistance = Math.Abs(xLeft - m_Character.transform.position.x);

            Root.Instance.m_Waterfall.setParameterValue("waterfall", leftDistance / maxDistance * 100.0f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "CameraCollider_CameraMoves" || other.gameObject.name == "CameraCollider_CameraStays" || other.gameObject.name == "CameraCollider_ChangeScene" || other.gameObject.name == "CameraCollider_LoadCredits")
        {
            m_PlaneshiftScript.m_AllowPlaneShift = 0;
        }

        if (other.gameObject.name == "CameraCollider_Waterfall" && Root.Instance != null)
        {
            Root.Instance.m_Waterfall.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        if (other.gameObject.name == "CameraCollider_WaterStep")
        {
            (m_Character as PlayerCharacterController).m_StandsInWater = false;
        }
    }
}
